using Grid.Features.Common;
using Grid.Features.CRM.DAL.Interfaces;
using Grid.Features.HRMS.DAL.Interfaces;
using Grid.Features.PMS.DAL.Interfaces;
using Grid.Hubs;
using Grid.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grid.Areas.PMS.Controllers
{
    public class ProjectController : ProjectsBaseController
    {        
        // GET: PMS/Project
        [GridPermission(PermissionCode = 300)]
        public ActionResult Index()
        {
            return View();
        }
    }
}