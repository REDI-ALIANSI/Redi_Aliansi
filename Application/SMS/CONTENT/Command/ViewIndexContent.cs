using Application.SMS.CONTENT.ViewModel;
using MediatR;

namespace Application.SMS.CONTENT.Command
{
    public class ViewIndexContent : IRequest<ContentViewVM>
    {
        public ContentViewVM contentViewVM { get; set; }
    }
}
