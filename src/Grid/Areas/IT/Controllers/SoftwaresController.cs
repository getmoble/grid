using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.IT.DAL.Interfaces;
using Grid.Features.IT.Entities;
using Grid.Filters;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;

namespace Grid.Areas.IT.Controllers
{
    [GridPermission(PermissionCode = 230)]
    public class SoftwaresController : GridBaseController
    {
        private readonly ISoftwareRepository _softwareRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SoftwaresController(ISoftwareRepository softwareRepository,
                                   IUnitOfWork unitOfWork)
        {
            _softwareRepository = softwareRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var softwares = _softwareRepository.GetAll("Category");
            return View(softwares.ToList());
        }
      
    }
}
