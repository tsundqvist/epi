using System;
using System.Web.Mvc;

namespace Alloy.Oktober2017.Web
{
	using System.Web.Routing;

	public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }

	    protected override void RegisterRoutes(RouteCollection routes)
	    {
		    base.RegisterRoutes(routes);
		    routes.MapRoute("myadminplugin", "MyAdminPlugin/{action}",
			    new { controller = "MyAdminPlugin", action = "Index" });
	    }
	}
}