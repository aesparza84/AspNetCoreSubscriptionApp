using Microsoft.AspNetCore.Mvc;
using Models;

namespace AspSubscriptionTracker.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        [Route("/")]
        public IActionResult Index(Subscription sub)
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateSubscription(Subscription sub)
        {
            //Bring to Create View (input fields)
            //Accept input and create Sub Model

            if (!ModelState.IsValid)
            {
                List<string> errors = new List<string>();

                foreach (var item in ModelState.Values)
                {
                    foreach (var prop in item.Errors)
                    {
                        errors.Add(prop.ErrorMessage);
                    }
                }

                string errorMessahe = string.Join("\n", errors);

                return View("Index", sub);
            }

            return View("CreateView", sub);
        }

        [Route("delete")]
        public IActionResult DeleteSubscription()
        {
            return View("DeleteView");
        }
    }
}
