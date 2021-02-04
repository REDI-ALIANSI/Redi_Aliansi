using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.SMS.CONTENT.ViewModel;
using Serilog;
using Application.SMS.CONTENT.Command;
using Application.Common.Interfaces;

namespace WebCMS_Redi.Controllers
{
    [Authorize(Roles = "InternalManager,Administrator,ContentAdm")]
    public class ContentController : BaseController
    {
        private readonly ILogger _logger = Serilog.Log.ForContext<ContentController>();
        private readonly ICurrentUserService _currentUserService;

        public ContentController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var vm = new ContentViewVM()
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(10),
                    ServiceId = 0,
                    OperatorId = 0
                };
                vm = await Mediator.Send(new ViewIndexContent { contentViewVM = vm });
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContentViewVM vm)
        {
            try
            {
                //Get Contents here
                vm = await Mediator.Send(new ViewIndexContent { contentViewVM = vm });
                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw ex;
            }
        }

        [HttpPost]
        public async Task<string> Delete(int contentId)
        {
            try
            {
                string response = String.Empty;
                var result = await Mediator.Send(new DeleteContent { ContentId = contentId });
                if (result.Succeeded)
                    response = "Content have been deleted";
                else response = result.Errors.FirstOrDefault();

                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> InsertContent()
        {
            try
            {
                var vm = new InsertContentVM();
                var result = await Mediator.Send(new InsertContent { contentVM = vm });

                return View(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertContent(InsertContentVM insertVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await Mediator.Send(new InsertContent { contentVM = insertVM });
                    ModelState.Clear();
                    //Check if Content is successfully inserted to database
                    if (result.InsertResult.Succeeded)
                        ViewBag.Message = "Content have been successfully submited";
                    else ViewBag.Message = result.InsertResult.Errors.FirstOrDefault();

                    return View(result);
                }
                else
                {
                    ViewBag.Message = "Content Model is invalid";
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditContentIndex(int contentId)
        {
            try
            {
                var vm = new EditContentVM();
                vm.ContentId = contentId;
                vm = await Mediator.Send(new ViewEditContent { editContentVM = vm });
                if (vm.EditContentResult.Succeeded)
                    return View(vm);
                else
                {
                    TempData["EditStatus"] = vm.EditContentResult.Errors.FirstOrDefault();
                    return RedirectToAction("Index");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditContent(EditContentVM vm)
        {
            try
            {
                vm = await Mediator.Send(new ViewEditContent { editContentVM = vm });

                if(vm.EditContentResult.Succeeded)
                    TempData["EditStatus"] = "Content have been successfully edited";
                else TempData["EditStatus"] = vm.EditContentResult.Errors.FirstOrDefault();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}