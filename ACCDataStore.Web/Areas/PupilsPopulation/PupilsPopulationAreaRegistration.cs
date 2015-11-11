using System.Web.Mvc;

namespace ACCDataStore.Web.Areas.PupilsPopulation
{
    public class PupilsPopulationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PupilsPopulation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PupilsPopulation_default",
                "PupilsPopulation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}