using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspSubscriptionTracker.Components
{
    public class AllSubscriptionsViewComponent : ViewComponent
    {
        public AllSubscriptionsViewComponent() { }

        //Use method call types as needed

        //For async calling
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }


        //For sync calling
        //public IViewComponentResult Invoke()
        //{
        //    return View();
        //}
    }
}
