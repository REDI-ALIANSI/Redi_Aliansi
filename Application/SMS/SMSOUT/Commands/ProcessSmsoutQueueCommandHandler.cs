using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.SMS.SMSDN.Commands;
using Domain.Entities.SMS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.SMSOUT.Commands
{
    public class ProcessSmsoutQueueCommandHandler : IRequestHandler<ProcessSmsoutQueueCommand,SmsoutVm>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMsgQ _msgQ;
        private readonly IMediator _mediator;

        public ProcessSmsoutQueueCommandHandler(IRediSmsDbContext context, IMsgQ msgQ, IMediator mediator)
        {
            _context = context;
            _msgQ = msgQ;
            _mediator = mediator;
        }

        public async Task<SmsoutVm> Handle(ProcessSmsoutQueueCommand request, CancellationToken cancellationToken)
        {
            var vm = new SmsoutVm();
            try
            {
                string msgQueue = await _msgQ.ConsumerQueue(request.Queue, request.QueueAuth);
                if (!msgQueue.Equals("ERROR : NO MESSAGE FOUND"))
                {
                    var smsoutQ = JsonSerializer.Deserialize<SmsoutD>(msgQueue);
                    smsoutQ.Message = _context.Messages.Where(o => o.MessageId.Equals(smsoutQ.MessageId)).SingleOrDefault();
                    vm.Msisdn = smsoutQ.Msisdn;
                    vm.Mt_Message = smsoutQ.Mt_Message;
                    vm.DateCreated = smsoutQ.DateCreated;
                    vm.IsDnWatch = smsoutQ.IsDnWatch;
                    vm.Sparam = smsoutQ.Sparam;
                    vm.Iparam = smsoutQ.Iparam;
                    vm.MtTxId = smsoutQ.MtTxId;
                    vm.OperatorId = smsoutQ.OperatorId;
                    vm.MessageId = smsoutQ.MessageId;
                    vm.ServiceId = smsoutQ.ServiceId;
                    vm.Sid = smsoutQ.Message.SidBilling;
                    
                    if (!(smsoutQ is null))
                    {
                        //to do send to each OperatorId URI here, return smsoutQ.trx_status 200 if success or 500 if failed
                        if (smsoutQ.DateToProcessed <= DateTime.Now)
                        {
                            vm.DateProcessed = DateTime.Now;
                            //Indosat Send MT Process
                            if (smsoutQ.OperatorId.Equals(51021))
                            {
                                var vmIndosat = await _mediator.Send(new SmsoutIndosatCommand
                                {
                                    Smsout = smsoutQ
                                }, cancellationToken);
                                if (vmIndosat.Response.ToLower().Contains("error"))
                                {
                                    smsoutQ.Trx_Status = "500";
                                }
                                else
                                { 
                                    smsoutQ.Trx_Status = "200"; 
                                }
                                vm.Response = vmIndosat.Response;

                                vm.URI_Hit = vmIndosat.URI_Hit;
                            }
                            //Excelcom Send MT Process
                            else if (smsoutQ.OperatorId.Equals(51011))
                            {
                                //Get MT Transaction ID from API Operator (For Excelcom)
                                var vmXl = await _mediator.Send(new SmsoutXlCommand
                                {
                                    Smsout = smsoutQ
                                }, cancellationToken);
                                if (vmXl.Response.ToLower().Contains("error"))
                                {
                                    smsoutQ.Trx_Status = "500";
                                }
                                else
                                {
                                    smsoutQ.Trx_Status = "200";
                                    smsoutQ.MtTxId = vmXl.Response;
                                }
                                vm.Response = vmXl.ResponseRaw;
                                vm.URI_Hit = vmXl.URI_Hit;
                            }
                            //Telkomsel Send MT Process
                            else if (smsoutQ.OperatorId.Equals(51010))
                            {
                                //GET ERROR CODE FROM MT API
                                var vmTsel = await _mediator.Send(new SmsoutTselCommand
                                {
                                    Smsout = smsoutQ
                                }, cancellationToken);
                                vm.Response = vmTsel.Response;

                                if (vmTsel.Response.Contains("ERROR"))
                                    smsoutQ.Trx_Status = "500";
                                else
                                {
                                    smsoutQ.Trx_Status = "200";
                                   //var DnQ = new SmsdnD();
                                   //  DnQ.ErrorCode = vmTsel.Response;
                                   //  DnQ.MtTxId = smsoutQ.MtTxId;
                                   //  DnQ.ErrorDesc = String.Empty;
                                   //  DnQ.Status = (vmTsel.Response.Equals("1") ? "Delivered" : "Failed");

                                    await _mediator.Send(new InsertDnRequest
                                    {
                                        DnErrorcode = vmTsel.Response,
                                        DnMtid = smsoutQ.MtTxId,
                                        Status = vmTsel.Response.Equals("1") ? "Delivered" : "Failed"
                                    }, cancellationToken);
                                } 
                                vm.URI_Hit = vmTsel.URI_Hit;
                            }
                            //if success / status code 200 add to table SMSOUTD here / Retry to queue
                            int retryLimit = _context.Operators.Where(o => o.OperatorId.Equals(smsoutQ.OperatorId))
                                                .Select(o => o.RetryLimit)
                                                .FirstOrDefault();
                            if (smsoutQ.Trx_Status.Equals("200") || smsoutQ.Iparam >= retryLimit)
                            {
                                smsoutQ.DateProcessed = DateTime.Now;
                                await _context.SmsoutDs.AddAsync(smsoutQ);
                                await _context.SaveChangesAsync(cancellationToken);
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(smsoutQ.Sparam) && !smsoutQ.Trx_Status.Equals("100"))
                                {
                                    smsoutQ.Sparam = "RETRY";
                                    smsoutQ.Iparam = 1;
                                }
                                else if (smsoutQ.Sparam.Equals("RETRY"))
                                    smsoutQ.Iparam += 1;

                                await _msgQ.ProducerQueue(smsoutQ, request.Queue, request.QueueAuth);
                            }
                        }
                        else
                        {
                            await _msgQ.ProducerQueue(smsoutQ, request.Queue, request.QueueAuth);
                        }
                    }
                    else
                    {
                        throw new NotFoundException(nameof(SmsoutD), msgQueue);
                    }
                }
                else
                {
                    throw new NotFoundException(nameof(SmsoutD), msgQueue);
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
