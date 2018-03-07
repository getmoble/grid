using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.LMS.DAL.Interfaces;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.LMS.Controllers
{
    public class HolidayController : LeaveBaseController
    {
        private readonly IHolidayRepository _holidayRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HolidayController(IHolidayRepository holidayRepository,
                                  IUnitOfWork unitOfWork)
        {
            _holidayRepository = holidayRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var user = WebUser.Id;          
            return View();
        }    
    }
}
