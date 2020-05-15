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

namespace Application.SMS.SMSIN.Commands
{
    public class ProcessSmsinQueueCommandHandler : IRequestHandler<ProcessSmsinQueueCommand, SmsinVm>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMsgQ _msgQ;
        private readonly IMediator _mediator;
        private readonly IExecuteDllService _executeDllService;

        public ProcessSmsinQueueCommandHandler(IRediSmsDbContext context, IMsgQ msgQ, IMediator mediator, IExecuteDllService executeDllService)
        {
            _context = context;
            _msgQ = msgQ;
            _mediator = mediator;
            _executeDllService = executeDllService;
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
                    var smsinQ = JsonSerializer.Deserialize<SmsinD>(msgQueue);
                    var subs = await _context.Subscriptions.Where(s => s.Msisdn.Equals(smsinQ.Msisdn) &&
                                                                                   s.ServiceId.Equals(smsinQ.ServiceId) &&
                                                                                   s.OperatorId.Equals(smsinQ.OperatorId)).FirstOrDefaultAsync();

                    if (!(smsinQ is null))
                    {
                        /*Get Sequence Action ON/OFF/PULL moved to dll for more dynamic process*/
                        string[] ArrKeyword = smsinQ.Mo_Message.ToUpper().Split(' ');
                        string messageType = String.Empty;
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
                                MtTxId = smsinQ.MotxId;
                            }
                        }
                        
                        //Get Messages By Type
                        List<Message> getMessages = await _mediator.Send(new GetMessagesByType
                        { MessageType = messageType, Serviceid = smsinQ.ServiceId, OperatorId = smsinQ.OperatorId }
                                                                        , cancellationToken);

                        var service = await _mediator.Send(new GetServiceById { ServiceId = smsinQ.ServiceId }, cancellationToken);
                        
                        //execute service script if its custom service :: HOMEWORK : How to log this?
                        if (service.IsCustom)
                        {
                            /*string DllPath = request.appsDllPath + service.ServiceCustom;
                            object[] dllParams = new object[] { smsinQ, getMessages };
                            object DllResult = _executeDllService.ProcessExecute(DllPath, service.ServiceCustom, "Smsin", dllParams);
                            if (!DllResult.Equals(null))
                                getMessages = (List<Message>)DllResult;
                            else throw new NotFoundException(nameof(DllResult), DllResult);*/
                        }

                        if (getMessages.Count() > 0)
                        {
                            foreach (var message in getMessages)
                            {
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
                        }
                        else throw new NotFoundException(nameof(List<Message>), getMessages);
                        //define VM for presentation
                        vm.Msisdn = smsinQ.Msisdn;
                        vm.Mo_Message = smsinQ.Mo_Message;
                        vm.MotxId = smsinQ.MotxId;
                        vm.ServiceId = smsinQ.ServiceId;
                        vm.OperatorId = smsinQ.OperatorId;
                        //vm.Shortcode = smsinQ.Shortcode;
                        vm.Status = 200;

                        await _context.SmsinDs.AddAsync(smsinQ);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    else
                    {
                        throw new NotFoundException(nameof(SmsdnD), msgQueue);
                    }   
                }
                else
                {
                    throw new NotFoundException(nameof(SmsdnD), msgQueue);
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
