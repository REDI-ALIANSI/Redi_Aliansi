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
    public class InsertContentHandler : IRequestHandler<InsertContent, InsertContentVM>
    {
        private readonly IRediSmsDbContext _context;
        private readonly IMediator _mediator;

        public InsertContentHandler(IRediSmsDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<InsertContentVM> Handle(InsertContent request, CancellationToken cancellationToken)
        {
            try
            {
                var ListOperators = await _mediator.Send(new GetOperators { });
                var SelectListOperator = new SelectList(ListOperators, "OperatorId", "OperatorName");

                var ListServices = await _mediator.Send(new GetServices { });
                var selectListService = new SelectList(ListServices, "ServiceId", "Name");

                var ListContentType = await _mediator.Send(new GetAllContentType { });
                var selectListContentType = new SelectList(ListContentType, "ContentTypeId", "Name");

                var vm = new InsertContentVM()
                {
                    ContentType = selectListContentType,
                    ListVMService = selectListService,
                    ListVMOperator = SelectListOperator
                };

                if (!String.IsNullOrEmpty(request.contentVM.ContentText))
                {
                    vm.InsertResult = Result.Success();
                    //Get PUSH Message
                    var messageContent = await _context.Messages.Where(m => m.ServiceId == request.contentVM.ServiceId
                                                                            && m.OperatorId == request.contentVM.OperatorId
                                                                            && m.MessageType.Equals("PUSH")
                                                                            && m.Order == request.contentVM.MessageOrder)
                                                                            .FirstOrDefaultAsync();

                    //validate if message is exist
                    if (messageContent != null)
                    {
                        var ContentValidation = await _context.Contents.Where(c => c.ContentSchedule == request.contentVM.ContentSchedule
                                                                            && c.MessageId == messageContent.MessageId)
                                                                            .FirstOrDefaultAsync();

                        if (ContentValidation == null)
                        {
                            var NewContent = new Content
                            {
                                ContentText = request.contentVM.ContentText,
                                ContentTypeId = request.contentVM.ContentTypeId.Value,
                                ContentSchedule = request.contentVM.ContentSchedule.Value,
                                MessageId = messageContent.MessageId
                            };
                            if (messageContent.IsRichContent) NewContent.ContentPath = request.contentVM.ContentPath;

                            await _context.Contents.AddAsync(NewContent);
                            await _context.SaveChangesAsync(cancellationToken);
                        }
                        else
                        {
                            var errorIEnurable = new List<string>() { "Renewal Content already inputed!" };
                            vm.InsertResult = Result.Failure(errorIEnurable);
                        }
                    }
                    else
                    {
                        var errorIEnurable = new List<string>() { "Renewal Content Message Not Found!" };
                        vm.InsertResult = Result.Failure(errorIEnurable);
                    }
                }

                return vm;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
