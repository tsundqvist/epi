namespace Alloy.Oktober2017.Web.Business.Plugin
{
	using System.IO;
	using System.Web.Mvc;
	using EPiServer.DynamicContent;
	using EPiServer.PlugIn;

	// What the fuck is thos -> deprecated in favour of Blocks!
	// This can now be added into XHtml properties
	[DynamicContentPlugIn(
		DisplayName = "ClassDynamicContentPlugin",
		Description = "Example of a Dynamic Content plugin as a simple class.")]
	public class ClassDynamicContentPlugin : DynamicContentBase, IDynamicContentView
	{
		public ClassDynamicContentPlugin()
		{
		}

		public void Render(TextWriter writer)
		{	
			writer.Write("<div>Hello World</div>");
		}
	}

	[EPiServer.PlugIn.GuiPlugIn(
		//Area = EPiServer.PlugIn.PlugInArea.AdminConfigMenu,
		Area = EPiServer.PlugIn.PlugInArea.ReportMenu,

		Url = "/MyAdminPlugin",
		DisplayName = "My MVC Admin Plugin")]
	public class MyAdminPluginController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
	}
}

