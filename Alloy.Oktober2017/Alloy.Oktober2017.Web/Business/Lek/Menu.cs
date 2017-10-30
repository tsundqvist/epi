using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alloy.Oktober2017.Web.Business.Lek
{
	using System.Web.Mvc;
	using EPiServer.Core;
	using EPiServer.Shell.Navigation;

	public class MyController : Controller
	{
		// 1 is menu path?
		[MenuItem("/global/dashboard", Text = "Start")] // , Url= "/path/to/edit/default.aspx"
		public ActionResult Index()
		{
			return Content("Hi there!");

		}
	}
}