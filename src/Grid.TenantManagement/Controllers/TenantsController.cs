using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Grid.Data;
using Grid.Data.MultiTenancy;
using Grid.Data.MultiTenancy.Entities;
using Grid.Entities.Company;
using Grid.Entities.HRMS;
using Grid.Infrastructure;
using Grid.TenantManagement.ViewModels;

namespace Grid.TenantManagement.Controllers
{
    public class TenantsController : Controller
    {
        private TenantDataContext db = new TenantDataContext();

        public ActionResult Index()
        {
            return View(db.Tenants.ToList());
        }

        public ActionResult Details(int id)
        {
            var tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            var scheduledJobs = db.TenantScheduledJobs.Where(t => t.TenantId == tenant.Id).ToList();

            var vm = new TenantDetailsViewModel(tenant)
            {
                ScheduledJobs = scheduledJobs
            };

            return View(vm);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Tenants.Add(tenant);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tenant);
        }

        public ActionResult Seed(int id)
        {
            var tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Seed(Tenant tenant)
        {
            var selectedTenant = db.Tenants.Find(tenant.Id);
            if (selectedTenant != null)
            {
                var connectionString = selectedTenant.ConnectionString;
                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (var context = new GridDataContext(connectionString))
                    {
                        var accessRule = AccessRule.CreateNewUserAccessRule(true);
                        context.AccessRules.Add(accessRule);
                        context.SaveChanges();

                        var person = new Person();
                        context.Persons.Add(person);
                        context.SaveChanges();

                        var manageCompany = new Permission {Title = "Manage Company", PermissionCode = 200};
                        context.Permissions.Add(manageCompany);
                        var manageHRMS = new Permission { Title = "Manage HRMS", PermissionCode = 210 };
                        context.Permissions.Add(manageHRMS);
                        var manageSales= new Permission { Title = "Manage Sales", PermissionCode = 220 };
                        context.Permissions.Add(manageSales);
                        var manageIT = new Permission { Title = "Manage IT", PermissionCode = 230 };
                        context.Permissions.Add(manageIT);
                        var managePMS= new Permission { Title = "Manage PMS", PermissionCode = 240 };
                        context.Permissions.Add(managePMS);
                        var manageInventory= new Permission { Title = "Manage Inventory", PermissionCode = 250 };
                        context.Permissions.Add(manageInventory);
                        var manageLMS = new Permission { Title = "Manage LMS", PermissionCode = 215 };
                        context.Permissions.Add(manageLMS);
                        var manageTimeSheet= new Permission { Title = "Manage TimeSheet", PermissionCode = 300 };
                        context.Permissions.Add(manageTimeSheet);
                        var manageRecruit = new Permission { Title = "Manage Recruit", PermissionCode = 500 };
                        context.Permissions.Add(manageRecruit);
                        var superPermission = new Permission { Title = "Super Permission", PermissionCode = 9999 };
                        context.Permissions.Add(superPermission);
                        var manageTMS = new Permission { Title = "Manage TMS", PermissionCode = 550 };
                        context.Permissions.Add(manageTMS);
                        var manageKBS = new Permission { Title = "Manage KBS", PermissionCode = 700 };
                        context.Permissions.Add(manageKBS);
                        var viewSettings = new Permission { Title = "View Settings", PermissionCode = 909 };
                        context.Permissions.Add(viewSettings);
                        var manageCRM = new Permission { Title = "Manage CRM", PermissionCode = 222 };
                        context.Permissions.Add(manageCRM);
                        var manageTicketDesk = new Permission { Title = "Manage Ticket Desk", PermissionCode = 1100 };
                        context.Permissions.Add(manageTicketDesk);
                        context.SaveChanges();

                        var adminRole = new Role {Name = "Admin"};
                        context.Roles.Add(adminRole);
                        context.SaveChanges();

                        // Role Permission Mapping
                        var permissions = context.Permissions.ToList();
                        foreach (var permission in permissions)
                        {
                            var newMap = new RolePermission
                            {
                                RoleId = adminRole.Id,
                                PermissionId = permission.Id
                            };

                            context.RolePermissions.Add(newMap);
                        }
                        context.SaveChanges();

                        // Create User
                        var user = new User
                        {
                            EmployeeCode = "Code",
                            Username = selectedTenant.Email,
                            Password = HashHelper.Hash("123456"),
                            AccessRuleId = accessRule.Id,
                            PersonId = person.Id
                        };

                        context.Users.Add(user);
                        context.SaveChanges();

                        // Role Member Mapping
                        var newMember = new RoleMember
                        {
                            RoleId = adminRole.Id,
                            UserId = user.Id
                        };

                        context.RoleMembers.Add(newMember);
                        context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(tenant);
        }

        public ActionResult SeedEmailTemplates(int id)
        {
            var tenant = db.Tenants.Find(id);

            if (tenant == null)
            {
                return HttpNotFound();
            }

            return View(tenant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeedEmailTemplates(Tenant tenant)
        {
            var selectedTenant = db.Tenants.Find(tenant.Id);

            if (selectedTenant != null)
            {
                var connectionString = selectedTenant.ConnectionString;
                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (var tenantContext = new TenantDataContext())
                    {
                        var emailTemplates = tenantContext.EmailTemplates.ToList();

                        using (var context = new GridDataContext(connectionString))
                        {
                            // Select Admin as the Creator 
                            var selectedUser = context.Users.FirstOrDefault(u => u.Username == selectedTenant.Email);

                            if (selectedUser != null)
                            {
                                foreach (var emailTemplate in emailTemplates)
                                {
                                    context.EmailTemplates.Add(new Entities.Company.EmailTemplate
                                    {
                                        Name = emailTemplate.Name,
                                        Content = emailTemplate.Content,
                                        CreatedOn = DateTime.UtcNow,
                                        CreatedByUserId = selectedUser.Id
                                    });
                                }
                            }

                            context.SaveChanges();

                            return RedirectToAction("Index");
                        }
                    }
                }
            }

            return View(tenant);
        }

        public ActionResult Edit(int id)
        {
            var tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tenant tenant)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tenant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tenant);
        }

        public ActionResult Delete(int id)
        {
            var tenant = db.Tenants.Find(id);
            if (tenant == null)
            {
                return HttpNotFound();
            }
            return View(tenant);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var tenant = db.Tenants.Find(id);
            db.Tenants.Remove(tenant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
