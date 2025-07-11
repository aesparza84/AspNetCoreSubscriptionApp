using System.Diagnostics;
using AspSubscriptionTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace AspSubscriptionTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubscriptionService subService;

        public HomeController(ISubscriptionService sub)
        {
            subService = sub;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            List<Subscription> list = await subService.ViewAllAsync();
            
            if (list != null)
                return View(list);
                
            return View();
        }

        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View("CreateView");
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Subscription sub)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateView", sub);
            }

            //Service Interaction
            bool added = await subService.AddSubAsync(sub);
            
            if (!added)
            {
                ModelState.AddModelError(string.Empty, $"Subscription already assocatiated with email {sub.Email}");
                return View("CreateView", sub);
            }
          
            return RedirectToAction("Index"); 
        }

        [Route("delete")]
        public IActionResult DeleteSubscription()
        {
            //Nothing calls this yet
            return View("DeleteView");
        }
    }
}
