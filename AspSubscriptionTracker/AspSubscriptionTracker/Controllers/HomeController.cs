using Microsoft.AspNetCore.Mvc;

namespace AspSubscriptionTracker.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("home")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("create")]
        public IActionResult CreateSubscription()
        {
            //Bring to Create View (input fields)
            //Accept input and create Sub Model

            return View("CreateView");
        }

        [Route("delete")]
        public IActionResult DeleteSubscription()
        {
            return View("DeleteView");
        }
    }
}
