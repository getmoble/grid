using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Grid.TenantManagement.Models;

namespace Grid.TenantManagement.Controllers
{
    public class AccountController : Controller
    {
        private const string FallbackReturnUrl = "/Home/Index";

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel vm, string returnUrl = FallbackReturnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (vm.Email == "admin@gridintra.net" && vm.Password == "pass@123")
                    {
                        var serializeModel = new PrincipalModel("admin@gridintra.net");
                        var serializer = new JavaScriptSerializer();
                        var userData = serializer.Serialize(serializeModel);
                        var authTicket = new FormsAuthenticationTicket(1, "admin@gridintra.net", DateTime.Now, DateTime.Now.AddMinutes(60),
                            false, userData);
                        var eticket = FormsAuthentication.Encrypt(authTicket);
                        var fcookie = new HttpCookie(FormsAuthentication.FormsCookieName, eticket);
                        Response.Cookies.Add(fcookie);

                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);

                        return RedirectToAction("SignIn", "Account");
                    }
                }
                
                ModelState.AddModelError(string.Empty, "Invalid Credentials, Please enter a valid username & password");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError(string.Empty, exception.Message);
            }

            return View(vm);
        }

        [HttpGet]
        public virtual ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("SignIn", "Account");
        }
    }
}