using Grid.Features.PMS.DAL.Interfaces;
using Grid.Infrastructure;
using Grid.Infrastructure.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Grid.Areas.PMS.Controllers
{
    [GridPermission(PermissionCode = 230)]

    public class ProjectMemberRolesController : GridBaseController
    {
        private readonly IProjectMemberRoleRepository _memberRoleRepository;

        public ProjectMemberRolesController(IProjectMemberRoleRepository memberRoleRepository)
        {
            _memberRoleRepository = memberRoleRepository;
        }

        public ActionResult Index()
        {
            var roles = _memberRoleRepository.GetAll("Department");
            return View(roles.ToList());
        }

    }
}
