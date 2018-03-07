using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Features.HRMS.Entities;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.HRMS.Controllers
{
    [GridPermission(PermissionCode = 210)]
    public class HobbiesController : GridBaseController
    {
        private readonly IHobbyRepository _hobbyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public HobbiesController(IHobbyRepository hobbyRepository,
                                IUnitOfWork unitOfWork)
        {
            _hobbyRepository = hobbyRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var hobbies = _hobbyRepository.GetAllBy(o => o.OrderBy(h => h.Title));
            return View(hobbies);
        }

    }
}
