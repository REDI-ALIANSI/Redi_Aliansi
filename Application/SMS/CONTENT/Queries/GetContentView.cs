using System;
using System.Collections.Generic;
using Application.Common.Model;
using Application.SMS.CONTENT.ViewModel;
using Domain.Entities.SMS;
using MediatR;

namespace Application.SMS.CONTENT.Queries
{
    public class GetContentView : IRequest<List<Content>>
    {
        public ContentViewVM ViewVM { get; set; }
    }
}
