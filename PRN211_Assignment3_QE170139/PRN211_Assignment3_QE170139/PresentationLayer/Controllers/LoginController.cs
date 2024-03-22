using DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BusinessObject; 

namespace PresentationLayer.Controllers
{
    public class LoginController : Controller
    {
        private readonly IMemberRepository _memberRepository;
        //private readonly AppConfig _appConfig;

        public LoginController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
            //_appConfig = AppConfigProvider.LoadAppConfig();
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(string email, string password)
        {

            var member = _memberRepository.Login(email, password);
            if (member != null)
            {
                if(member.MemberId != 0) { 
                    // Set role là member trong Session
                    HttpContext.Session.SetString("UserRole", "Member");
                }
                else
                {
                    // Set role là admin trong Session
                    HttpContext.Session.SetString("UserRole", "Admin");
                }
                HttpContext.Session.SetString("UserId", member.MemberId.ToString());
                return RedirectToAction("Index", "Home");
            }
            else
            {
                    return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("UserRole");
            HttpContext.Session.Remove("UserId");
            return RedirectToAction("Index", "Login");
        }



    }
}
