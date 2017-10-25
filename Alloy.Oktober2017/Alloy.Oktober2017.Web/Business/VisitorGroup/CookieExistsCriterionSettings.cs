namespace Alloy.Oktober2017.Web.Business.VisitorGroup
{
	using System.ComponentModel.DataAnnotations;
	using EPiServer.Personalization.VisitorGroups;

	public class CookieExistsCriterionSettings : CriterionModelBase
	{
		[Required]
		public string CookieName { get; set; }

		public override ICriterionModel Copy()
		{
			return ShallowCopy();
		}
	}
}