using System.Linq;
using System.Web.Mvc;
using Grid.Features.Common;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.HRMS.Entities;
using Grid.Infrastructure;

namespace Grid.Areas.Company.Controllers
{
    public class EmailTemplatesController : GridBaseController
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmailTemplatesController(IEmailTemplateRepository emailTemplateRepository,
                                IUnitOfWork unitOfWork)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var emailTemplates = _emailTemplateRepository.GetAll("CreatedByUser,UpdatedByUser");
            return View(emailTemplates.ToList());
        }

    }
}
