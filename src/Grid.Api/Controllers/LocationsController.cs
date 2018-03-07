using System.Linq;
using System.Web.Mvc;
using Grid.Api.Models;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;

namespace Grid.Api.Controllers
{
    public class LocationsController : GridApiBaseController
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationsController(ILocationRepository locationRepository,
                                   IUnitOfWork unitOfWork)
        {
            _locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public JsonResult Index()
        {
            var apiResult = TryExecute(() =>
            {
                return _locationRepository.GetAll(o => o.OrderByDescending(l => l.CreatedOn)).ToList();
            }, "Locations Fetched sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get(int id)
        {
            var apiResult = TryExecute(() => _locationRepository.Get(id), "Location Fetched sucessfully");
            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var apiResult = TryExecute(() =>
            {
                _locationRepository.Delete(id);
                _unitOfWork.Commit();
                return true;
            }, "Location deleted sucessfully");

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Location location)
        {
            ApiResult<Location> apiResult;

            if (ModelState.IsValid)
            {
                if (location.Id > 0)
                {
                    apiResult = TryExecute(() =>
                    {
                        _locationRepository.Update(location);
                        _unitOfWork.Commit();
                        return location;
                    }, "Location updated sucessfully");
                }
                else
                {
                    apiResult = TryExecute(() =>
                    {
                        _locationRepository.Create(location);
                        _unitOfWork.Commit();
                        return location;
                    }, "Location created sucessfully");
                }
            }
            else
            {
                apiResult = ApiResultFromModelErrors<Location>();
            }

            return Json(apiResult, JsonRequestBehavior.AllowGet);
        }
    }
}