namespace ACCDataStore.Web.Areas.SchoolRollForecast.Controllers
{
    internal class OleDBConnection
    {
        private string sConnectionString;

        public OleDBConnection(string sConnectionString)
        {
            this.sConnectionString = sConnectionString;
        }
    }
}