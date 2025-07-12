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

        //GET, entering the page
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View("CreateView");
        }

        //POST, for validation
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Subscription sub)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateView", sub);
            }

            //Service Interaction
            Guid addedId = await subService.AddSubAsync(sub);
            
            if (addedId != Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, $"Subscription already assocatiated with email {sub.Email}");
                return View("CreateView", sub);
            }
          
            return RedirectToAction("Index"); 
        }

        [HttpGet]
        [Route("edit/{subId}")]
        public async Task<IActionResult> Edit([FromRoute]Guid? subId)
        {
            if (subId == null)
                return BadRequest($"Subid is NULL");

            Subscription requestedSub = await subService.FindAsync(subId);

            if (requestedSub == null)
                return BadRequest($"{subId} not found in database");

            return View("EditView", requestedSub);
        }

        //POST, for validation
        [HttpPost]
        [Route("edit/{subId}")]
        public IActionResult Edit(Guid subId, Subscription sub)
        {
            if (subId != sub.Id)
                return BadRequest("ID no match");

            if (!ModelState.IsValid)
            {
                return View("Editview", sub);
            }

            //Service Interaction
            bool updated = subService.Update(sub);

            if (!updated)
            {
                ModelState.AddModelError(string.Empty, $"Subscription already assocatiated with email {sub.Email}");
                return View("EditView", sub);
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
