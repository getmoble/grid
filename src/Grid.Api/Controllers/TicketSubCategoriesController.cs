using System;
using System.Linq;
using System.Web.Mvc;
using Grid.Api.Filters;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.TicketDesk.DAL.Interfaces;
using Grid.Features.TicketDesk.Entities;
using Grid.Api.Models.TicketDesk;

namespace Grid.Api.Controllers
{
    public class TicketSubCategoriesController : GridApiBaseController
    {
        private readonly ITicketSubCategoryRepository _ticketSubCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TicketSubCategoriesController(ITicketSubCategoryRepository ticketSubCategoryRepository,
                                   IUnitOfWork unitOfWork)
        {
            _ticketSubCategoryRepository = ticketSubCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index(TicketSubCategoryFilterModel vm)
        {
            var apiResult = TryExecute(() =>
            {
                Func<IQueryable<TicketSubCategory>, IQueryable<TicketSubCategory>> ticketSubCategoryFilter = q =>
                {
                    if (vm.CategoryId.HasValue)
                    {
                        q = q.Where(r => r.TicketCategoryId == vm.CategoryId);
                    }

                    q = q.OrderBy(t => t.Title);

                    return q;
                };

                var subCategories = _ticketSubCategoryRepository.Search(ticketSubCategoryFilter).ToList();
                return subCategories;
            }, "Ticket  Sub Categories Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _ticketSubCategoryRepository.Get(id, "TicketCategory"), "Ticket Sub Category fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Update(TicketSubCategoryModel ticketSubCategoryModel)
        {
            ApiResult<TicketSubCategory> apiResult;

            if (ModelState.IsValid)
            {
                if (ticketSubCategoryModel.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        var ticketSubCategory = new TicketSubCategory
                        {
                            Title = ticketSubCategoryModel.Title,
                            TicketCategoryId = ticketSubCategoryModel.TicketCategoryId,                           
                            Description = ticketSubCategoryModel.Description,                           
                            Id = ticketSubCategoryModel.Id
                        };
                        _ticketSubCategoryRepository.Update(ticketSubCategory);
                        _unitOfWork.Commit();
                        return ticketSubCategory;
                    }, "Ticket Sub Category updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        var ticketSubCategory = new TicketSubCategory
                        {
                            Title = ticketSubCategoryModel.Title,
                            TicketCategoryId = ticketSubCategoryModel.TicketCategoryId,
                            Description = ticketSubCategoryModel.Description,
                            Id = ticketSubCategoryModel.Id
                        };
                        _ticketSubCategoryRepository.Create(ticketSubCategory);
                        _unitOfWork.Commit();
                        return ticketSubCategory;
                    }, "Ticket Sub Category created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<TicketSubCategory>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _ticketSubCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Ticket Sub Category deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}
