using Application.Common.Model;
using Application.SMS.CONTENT.ViewModel;
using MediatR;

namespace Application.SMS.CONTENT.Command
{
    public class InsertContent : IRequest<InsertContentVM>
    {
        public InsertContentVM contentVM { get; set; }
    }
}
