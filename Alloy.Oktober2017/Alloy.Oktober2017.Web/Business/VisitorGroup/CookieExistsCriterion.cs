using System.Web;

namespace Alloy.Oktober2017.Web.Business.VisitorGroup
{
	using System.Security.Principal;
	using EPiServer.Personalization.VisitorGroups;

	[VisitorGroupCriterion(
		Category = "Technical",
		DisplayName = "Cookie Exists tob",
		Description = "Checks if a specific cookie exists")]
	public class CookieExistsCriterion : CriterionBase<CookieExistsCriterionSettings>
	{
		public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
		{
			var isThis = this.Model.CookieName;

			return httpContext.Request.Cookies[isThis as string] != null;
		}

		public override void Subscribe(ICriterionEvents criterionEvents)
		{
			criterionEvents.StartRequest += criterionEvents_StartRequest;
			criterionEvents.EndRequest += criterionEvents_StartRequest;
			criterionEvents.StartSession += criterionEvents_StartRequest;
			criterionEvents.VisitedPage += criterionEvents_StartRequest;


		}

		private void criterionEvents_StartRequest(object sender, CriterionEventArgs e)
		{
			// just curiousa


		}
	}
}