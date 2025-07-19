using System.Diagnostics;
using AspSubscriptionTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;

namespace AspSubscriptionTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISubscriptionService subService;
        private readonly IMemoryCache memoryCache;

        private const string cacheKey = "allSubscriptionsKey";
        public HomeController(ISubscriptionService sub, IMemoryCache cache)
        {
            subService = sub;
            memoryCache = cache;
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            if (memoryCache.TryGetValue(cacheKey, out List<Subscription> subList))
            {
                return View(subList);
            }
            else
            {
                subList = await subService.ViewAllAsync();

                var options = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(45))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal);

                memoryCache.Set(cacheKey, subList, options);
            }
                
            return View(subList);
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

            Guid addedId = await subService.AddSubAsync(sub);
            
            if (addedId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, $"Subscription already assocatiated with email {sub.Email}");
                return View("CreateView", sub);
            }
          
            memoryCache.Remove(cacheKey);   
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
        public async Task<IActionResult> Edit(Guid subId, Subscription sub)
        {
            if (subId != sub.Id)
                return BadRequest("ID no match");

            if (!ModelState.IsValid)
            {
                return View("Editview", sub);
            }

            //Service Interaction
            bool updated = await subService.Update(sub);

            if (!updated)
            {
                ModelState.AddModelError(string.Empty, $"Subscription already assocatiated with email {sub.Email}");
                return View("EditView", sub);
            }

            memoryCache.Remove(cacheKey);   
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("delete/{subId}")]
        public async Task<IActionResult> Delete(Guid subId)
        {
            Subscription sub = await subService.FindAsync(subId);

            //Nothing calls this yet
            return View("DeleteView", sub);
        }

		[HttpPost]
		[Route("delete/{subId}")]
		public async Task<IActionResult> Delete(Subscription sub)
		{
			await subService.DeleteSub(sub.Id);

            //Nothing calls this yet
            memoryCache.Remove(cacheKey);
            return RedirectToAction("Index");
		}
	}
}
