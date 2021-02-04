using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities.SMS;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Application.Common.Exceptions;
using System.Text.Json;
using Application.Common.Behaviour;
using System.Collections.Generic;
using Application.SMS.MESSAGE.Queries;
using Application.SMS.SERVICE.Queries;
using Application.SMS.SUBSCRIPTION.Commands;
using Application.SMS.SMSOUT.Commands;
using Application.SMS.SERVICE.ViewModel;
using Newtonsoft.Json;

namespace Application.SMS.SMSIN.Commands
{
    public class ProcessSmsinQueueCommandHandler : IRequestHandler<ProcessSmsinQueueCommand, SmsinVm>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMsgQ _msgQ;
        private readonly IMediator _mediator;
        private readonly IHttpRequest _httpRequest;

        public ProcessSmsinQueueCommandHandler(IRediSmsDbContext context, IMsgQ msgQ, IMediator mediator, IHttpRequest httpRequest)
        {
            _context = context;
            _msgQ = msgQ;
            _mediator = mediator;
            _httpRequest = httpRequest;
        }

        public async Task<SmsinVm> Handle(ProcessSmsinQueueCommand request, CancellationToken cancellationToken)
        {
            var vm = new SmsinVm();
            try
            {
                string msgQueue = await _msgQ.ConsumerQueue(request.queue, request.QueueAuth);
                string MtTxId = String.Empty;
                if (!msgQueue.Equals("ERROR : NO MESSAGE FOUND"))
                {
                    var smsinQ = System.Text.Json.JsonSerializer.Deserialize<SmsinD>(msgQueue);
                    var subs = await _context.Subscriptions.Where(s => s.Msisdn.Equals(smsinQ.Msisdn) &&
                                                                                   s.ServiceId.Equals(smsinQ.ServiceId) &&
                                                                                   s.OperatorId.Equals(smsinQ.OperatorId)).FirstOrDefaultAsync();

                    if (!(smsinQ is null))
                    {
                        /*Get Sequence Action ON/OFF/PULL moved to dll for more dynamic process*/
                        string[] ArrKeyword = smsinQ.Mo_Message.ToUpper().Split(' ');
                        string messageType = String.Empty;
                        int Order = 1;
                        if (await _mediator.Send(new CheckReservedKeyword { Mo_Message = ArrKeyword[0] }, cancellationToken))
                        {
                            if ((ArrKeyword[0].Equals("ON") || ArrKeyword[0].Equals("REG")))
                            {
                                if (subs is null)
                                {
                                    messageType = "ON";
                                    //Create to Subsscription
                                    subs = await _mediator.Send(new InsertSubscription
                                    {
                                        rMoMessage = smsinQ.Mo_Message,
                                        rMsisdn = smsinQ.Msisdn,
                                        rOperatorid = smsinQ.OperatorId,
                                        rServiceid = smsinQ.ServiceId
                                    }, cancellationToken);
                                }
                            }
                            else if (ArrKeyword[0].Equals("UNREG") || ArrKeyword[0].Equals("STOP"))
                            {
                                messageType = "OFF";
                                //Unreg Subscription
                                await _mediator.Send(new UnregSubscription { rState = "UNREG", rSubscription = subs, rUnreg_keyword = smsinQ.Mo_Message },cancellationToken);
                            }
                            else
                            {
                                messageType = "PULL";
                                if(ArrKeyword.Count() > 1)
                                {
                                    if (ArrKeyword[1].ToUpper() == "HELP" || ArrKeyword[1].ToUpper() == "INFO")
                                    {
                                        messageType = "HELP";
                                    }
                                    else
                                    {
                                        int LastIndex = ArrKeyword.Count() - 1;
                                        Order = Convert.ToInt32(ArrKeyword[LastIndex]);
                                    }
                                }
                                MtTxId = smsinQ.MotxId;
                            }
                        }
                        else
                        {
                            messageType = "PULL";
                            if (ArrKeyword.Count() > 1)
                            {
                                if (ArrKeyword[1].ToUpper() == "HELP" || ArrKeyword[1].ToUpper() == "INFO")
                                {
                                    messageType = "HELP";
                                }
                                else
                                {
                                    int LastIndex = ArrKeyword.Count() - 1;
                                    Order = Convert.ToInt32(ArrKeyword[LastIndex]);
                                }
                            }
                            MtTxId = smsinQ.MotxId;
                        }

                        var iservice = await _mediator.Send(new GetServiceById { ServiceId = smsinQ.ServiceId }, cancellationToken);

                        //Get Messages By Type
                        List<Message> getMessages = await _mediator.Send(new GetMessagesByType
                        { MessageType = messageType, Serviceid = smsinQ.ServiceId, OperatorId = smsinQ.OperatorId }
                                                                        , cancellationToken);


                        foreach (var getMessage in getMessages)
                        {
                            getMessage.Service = null;
                        }
                       
                        
                        //Hit API Custom service if its custom service
                        if (iservice.IsCustom)
                        {
                            //Put URL Webapisms here *this one is development
                            string UrlWebapisms = @"http://localhost/redi.webapisms/api/";
                            string Uri = UrlWebapisms + iservice.ServiceCustom + "/Smsin";
                            var ReqObj = new CustomServiceSmsinRequest() { smsin = smsinQ, messages = getMessages };
                            var Resp = await _httpRequest.PostHttpResp(Uri, ReqObj);
                            var RespCustApi = JsonConvert.DeserializeObject<CustomServiceSmsinResponse>(Resp);
                            if (RespCustApi.result.ToUpper().Equals("OK"))
                            {
                                if (RespCustApi.MessageIds.Count() > 0)
                                {
                                    getMessages = new List<Message>();
                                    foreach (var MessageId in RespCustApi.MessageIds)
                                    {
                                        getMessages.Add(await _mediator.Send(new GetMessagesbyId { MessageId = MessageId }));
                                    }
                                }
                            }
                        }

                        if (getMessages.Count() > 0)
                        {
                            foreach (var message in getMessages)
                            {
                                if (messageType != "PULL")
                                    MtTxId = Guid.NewGuid().ToString("N");
                                //Send Message SMSOUTQ process here
                                await _mediator.Send(new SendSmsoutQueueCommand
                                {
                                    rIparam = 0,
                                    rSparam = String.Empty,
                                    rMessage = message,
                                    rQueue = "SMSOUTQ",
                                    rMsisdn = smsinQ.Msisdn,
                                    rServiceId = smsinQ.ServiceId,
                                    rMtTxId = MtTxId,
                                    QueueAuth = request.QueueAuth
                                }, cancellationToken);
                            }
                            //define VM for presentation
                            vm.Msisdn = smsinQ.Msisdn;
                            vm.Mo_Message = smsinQ.Mo_Message;
                            vm.MotxId = smsinQ.MotxId;
                            vm.ServiceId = smsinQ.ServiceId;
                            vm.OperatorId = smsinQ.OperatorId;
                            //vm.Shortcode = smsinQ.Shortcode;
                            vm.Status = 200;
                        }
                        else
                        {
                            vm.Status = 500;
                            vm.trx_status = "Message(s) for keyword not found";
                        }

                        await _context.SmsinDs.AddAsync(smsinQ);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        vm.Status = 500;
                        vm.trx_status = "Failed Deserialize SMSINQ";
                    }   
                }
                else
                {
                    vm.Status = 500;
                    vm.trx_status = "Message Queue not found";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return vm;
        }
    }
}
