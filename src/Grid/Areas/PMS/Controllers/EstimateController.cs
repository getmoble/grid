using System.Linq;
using System.Web.Mvc;
using Grid.Areas.PMS.Models;
using Grid.Features.Common;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Features.PMS.Entities;
using Grid.Features.PMS.ViewModels;

namespace Grid.Areas.PMS.Controllers
{
    public class EstimateController : ProjectsBaseController
    {
        private readonly IEstimateRepository _estimateRepository;
        private readonly IEstimateLineItemRepository _estimateLineItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EstimateController(IEstimateRepository estimateRepository,
                                  IEstimateLineItemRepository estimateLineItemRepository,
                                  IUnitOfWork unitOfWork)
        {
            _estimateRepository = estimateRepository;
            _estimateLineItemRepository = estimateLineItemRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public ActionResult CreateSheet(EstimateModel estimate)
        {
            var newEstimate = new Estimate
            {
                Title = estimate.Title,
                Comments = estimate.Comments,
                CreatedByUserId = WebUser.Id
            };

            _estimateRepository.Create(newEstimate);
            _unitOfWork.Commit();

            foreach (var lineItem in estimate.Rows)
            {
                var newEstimateLineItem = new EstimateLineItem()
                {
                    EstimateId = newEstimate.Id,
                    Module = lineItem.Module,
                    Task = lineItem.Task,
                    Effort = lineItem.Effort,
                    Comment = lineItem.Comment,
                    WorkType = lineItem.WorkType
                };

                _estimateLineItemRepository.Create(newEstimateLineItem);
            }

            _unitOfWork.Commit();

            return Json(true);
        }

        [HttpGet]
        public ActionResult GetEstimate(int id)
        {
            var estimate = _estimateRepository.Get(id);
            var lineItems = _estimateLineItemRepository.GetAllBy(l => l.EstimateId == estimate.Id).ToList();

            if (estimate != null)
            {
                var model = new EstimateModel()
                {
                    Id = estimate.Id,
                    Title = estimate.Title,
                    Comments = estimate.Comments
                };

                var lineItemModels = lineItems.Select(l => new EstimateLineItemModel
                {
                    Module = l.Module,
                    Task =  l.Task,
                    Effort = l.Effort,
                    WorkType = l.WorkType,
                    Comment = l.Comment
                }).ToList();

                model.Rows = lineItemModels;

                return Json(model, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSheet(EstimateModel estimate)
        {
            var selectedEstimate = _estimateRepository.Get(estimate.Id);
            if (selectedEstimate != null)
            {
                selectedEstimate.Title = estimate.Title;
                selectedEstimate.Comments = estimate.Comments;

                selectedEstimate.UpdatedByUserId = WebUser.Id;

                _estimateRepository.Update(selectedEstimate);
                _unitOfWork.Commit();

                // Remove Existing
                var existingRows = _estimateLineItemRepository.GetAllBy(t => t.EstimateId == selectedEstimate.Id);
                foreach (var existingRow in existingRows)
                {
                    _estimateLineItemRepository.Delete(existingRow);
                }

                _unitOfWork.Commit();

                // Add Fresh
                foreach (var lineItem in estimate.Rows)
                {
                    var newEstimateLineItem = new EstimateLineItem
                    {
                        EstimateId = selectedEstimate.Id,
                        Module = lineItem.Module,
                        Task = lineItem.Task,
                        Effort = lineItem.Effort,
                        Comment = lineItem.Comment,
                        WorkType = lineItem.WorkType
                    };

                    _estimateLineItemRepository.Create(newEstimateLineItem);
                }

                _unitOfWork.Commit();

                return Json(true);
            }

            return Json(false);
        }

        public ActionResult Index(EstimateSearchViewModel vm)
        {
            var estimates = _estimateRepository.GetAll();
            vm.Estimates = estimates.OrderByDescending(c => c.CreatedOn).ToList();
            return View(vm);
        }
    }
}