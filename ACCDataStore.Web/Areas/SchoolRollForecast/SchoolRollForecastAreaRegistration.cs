using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.SchoolRollForecast
{
    public class SchoolRollForecastAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SchoolRollForecast";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SchoolRollForecast_default",
                "SchoolRollForecast/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}