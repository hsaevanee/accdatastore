using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.ScrumHub
{
    public class ScrumHubAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ScrumHub";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ScrumHub_default",
                "ScrumHub/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}