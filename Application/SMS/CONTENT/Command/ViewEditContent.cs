using Application.SMS.CONTENT.ViewModel;
using MediatR;

namespace Application.SMS.CONTENT.Command
{
    public class ViewEditContent : IRequest<EditContentVM>
    {
        public EditContentVM editContentVM { get; set; }
    }
}
