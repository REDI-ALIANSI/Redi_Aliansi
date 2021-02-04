using Application.Common.Interfaces;
using Application.Common.Model;
using Application.SMS.CONTENT.Queries;
using Application.SMS.CONTENT.ViewModel;
using Application.SMS.OPERATOR.Queries;
using Application.SMS.SERVICE.Queries;
using Domain.Entities.SMS;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.SMS.CONTENT.Command
{
    public class VIewEditContentHandler : IRequestHandler<ViewEditContent, EditContentVM>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public VIewEditContentHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<EditContentVM> Handle(ViewEditContent request, CancellationToken cancellationToken)
        {
            try
            {
                var ListOperators = await _mediator.Send(new GetOperators { });
                var SelectListOperator = new SelectList(ListOperators, "OperatorId", "OperatorName");

                var ListServices = await _mediator.Send(new GetServices { });
                var selectListService = new SelectList(ListServices, "ServiceId", "Name");

                var ListContentType = await _mediator.Send(new GetAllContentType { });
                var selectListContentType = new SelectList(ListContentType, "ContentTypeId", "Name");

                request.editContentVM.ContentType = selectListContentType;
                request.editContentVM.ListVMService = selectListService;
                request.editContentVM.ListVMOperator = SelectListOperator;

                if (request.editContentVM.ContentId >= 0)
                {
                    request.editContentVM.EditContentResult = Result.Success();

                    var Econtent = await _context.Contents.Where(c => c.ContentId == request.editContentVM.ContentId)
                                                                 .Include(c => c.Message)
                                                                 .FirstOrDefaultAsync();

                    if(Econtent != null && Econtent.ContentSchedule > DateTime.Today)
                    {
                        var messageContent = new Message();
                        if (String.IsNullOrEmpty(request.editContentVM.ContentText))
                        {
                            //If Request only provide Content ID
                            messageContent = await _context.Messages.Where(m => m.MessageId == Econtent.MessageId)
                                                                            .FirstOrDefaultAsync();
                            if (messageContent != null)
                            {
                                request.editContentVM.ContentType.Where(c => c.Value.Equals(Econtent.ContentTypeId.ToString())).FirstOrDefault().Selected = true;
                                request.editContentVM.ListVMService.Where(s => s.Value.Equals(messageContent.ServiceId.ToString())).FirstOrDefault().Selected = true;
                                request.editContentVM.ListVMOperator.Where(o => o.Value.Equals(messageContent.OperatorId.ToString())).FirstOrDefault().Selected = true;

                                if(Econtent.ContentPath != null)
                                    request.editContentVM.ContentPath = Econtent.ContentPath;
                                request.editContentVM.ContentSchedule = Econtent.ContentSchedule;
                                request.editContentVM.ContentText = Econtent.ContentText;
                                request.editContentVM.ContentTypeId= Econtent.ContentTypeId;
                                request.editContentVM.MessageOrder = messageContent.Order;
                                request.editContentVM.ServiceId = messageContent.ServiceId;
                                request.editContentVM.OperatorId = messageContent.OperatorId;
                                request.editContentVM.ContentId = Econtent.ContentId;
                                messageContent.MessageId = Econtent.MessageId;
                            }
                        }
                        else
                        {
                            messageContent = await _context.Messages.Where(m => m.ServiceId == request.editContentVM.ServiceId
                                                                            && m.OperatorId == request.editContentVM.OperatorId
                                                                            && m.MessageType.Equals("PUSH")
                                                                            && m.Order == request.editContentVM.MessageOrder)
                                                                            .FirstOrDefaultAsync();

                            if (messageContent != null)
                            {
                                Econtent.ContentPath = request.editContentVM.ContentPath;
                                Econtent.ContentSchedule = request.editContentVM.ContentSchedule;
                                Econtent.ContentText = request.editContentVM.ContentText;
                                Econtent.ContentTypeId = request.editContentVM.ContentTypeId.Value;
                                Econtent.MessageId = messageContent.MessageId;

                                _context.Contents.Update(Econtent);
                                await _context.SaveChangesAsync(cancellationToken);
                            }
                            else
                            {
                                var errorIEnurable = new List<string>() { "Content Message Not Found!" };
                                request.editContentVM.EditContentResult = Result.Failure(errorIEnurable);
                            }
                        }
                    }
                    else
                    {
                        var errorIEnurable = new List<string>() { "Content Already Processed" };
                        request.editContentVM.EditContentResult = Result.Failure(errorIEnurable);
                    }    
                }
                else
                {
                    var errorIEnurable = new List<string>() { "Content Model Are Invalid!" };
                    request.editContentVM.EditContentResult = Result.Failure(errorIEnurable);
                }

                return request.editContentVM;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
