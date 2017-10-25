using System.Web.Mvc;
using Alloy.Oktober2017.Web.Models.Pages;
using Alloy.Oktober2017.Web.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Mvc;

namespace Alloy.Oktober2017.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Security;
	using EPiServer;
	using EPiServer.Cms.Shell.UI.Rest.ContentQuery.Internal;
	using EPiServer.Core;
	using EPiServer.DataAbstraction;
	using EPiServer.DataAbstraction.RuntimeModel;
	using EPiServer.Personalization;
	using EPiServer.Security;
	using EPiServer.Shell.ContentQuery;
	using EPiServer.Validation;
	using EPiServer.Web.Routing;

	public class StartPageController : PageControllerBase<StartPage>
	{
		private readonly IValidationService validationService;
		private readonly IContentProviderManager _contentProviderManager;
		private readonly ILanguageBranchRepository _languageBranchRepository;

		// Can be used to convert ContentReference to parmeant link. Not the other way around
		private readonly IPermanentLinkMapper _permanentLinkMapper;

		private readonly IContentLanguageSettingsResolver contentLanguageSettingsResolver;

		private readonly IContentSecurityRepository contentSecurityRepository;

		private readonly ISiteDefinitionRepository _siteDefinitionRepository;

		public StartPageController(
			IValidationService validationService,
			IContentProviderManager contentProviderManager,
			ILanguageBranchRepository languageBranchRepository,
			IPermanentLinkMapper ermanentLinkMapper,
			IContentLanguageSettingsResolver contentLanguageSettingsResolver,
			IContentSecurityRepository contentSecurityRepository,
			ISiteDefinitionRepository siteDefinitionRepository)
		{
			this.validationService = validationService;
			this._contentProviderManager = contentProviderManager;
			this._languageBranchRepository = languageBranchRepository;
			this._permanentLinkMapper = ermanentLinkMapper;
			this.contentLanguageSettingsResolver = contentLanguageSettingsResolver;
			this.contentSecurityRepository = contentSecurityRepository;
			this._siteDefinitionRepository = siteDefinitionRepository;
		}

		public ActionResult Index(StartPage currentPage)
		{
			// Content Model stuff
			// return guid perma link
			var linkUrl = currentPage.LinkURL;
			// Whats the diffeent
			var name = currentPage.Name; // Name of page
			var pageName = currentPage.PageName; // DisplayName


			// -- Other stuff --------
			var providerMap = this._contentProviderManager.ProviderMap;
			var isWastebasket = this._contentProviderManager.IsWastebasket(currentPage.ContentLink);

			var securityDescriptor = this.contentSecurityRepository.Get(currentPage.ContentLink);

			var allSiteDefinitions = this._siteDefinitionRepository.List();

			// check access rights on content with ACL = Access Control List
			var creator = currentPage.ACL.Creator;

			// WIll check if the current use have access or not to this content
			bool iHaveReadAccess = currentPage.ACL.QueryDistinctAccess(AccessLevel.Read);

			// How to validate a page using IValtidate
			this.validationService.Validate(currentPage);



			// -------------- Users -----------------
			var user = HttpContext.User;

			// Get current user - Da fuck is this?? Never seen before
			EPiServerProfile currentUser = EPiServer.Personalization.EPiServerProfile.Current;
			// or
			string currentUser2 = EPiServer.Security.PrincipalInfo.CurrentPrincipal.Identity.Name;
			// or
			string currentUser3 = this.HttpContext.User.Identity.Name;

			EPiServerProfile someUser = EPiServer.Personalization.EPiServerProfile.Get("tobias");

			#region ------ Roles -------

			// Crash because The Role Manager feature has not been enabled.
			//string[] allRoles = System.Web.Security.Roles.GetAllRoles();

			var rolename = "webadmin";
			//bool currentUserIsInRole = System.Web.Security.Roles.IsUserInRole(rolename);
			// or
			bool currentUserIsInRole2 = HttpContext.User.IsInRole(rolename);
			// or
			bool currentUserIsInRole3 = EPiServer.Security.PrincipalInfo.CurrentPrincipal.IsInRole(rolename);


			// or - This crash: because it's not specific 
			// MembershipUser someUser2 = System.Web.Security.Membership.GetUser("tobias");
			// Add extra property - this works: but not for anonymous user ofc
			// currentUser.TrySetProfileValue("TobbesProperty", "hejsan");
			// currentUser.Save();

			//      object theValue;
			//      var gotIt = currentUser.TryGetProfileValue("TobbesProperty", out  theValue);
			//currentUser.Company

			#endregion

			// ----- LANGUAGE STUFF ----------
			var allLanguage = this._languageBranchRepository.ListAll();

			var contentLanguageSettings = this.contentLanguageSettingsResolver.Resolve(currentPage.ContentLink);


			var currLang = currentPage.Language;
			var languages = currentPage.ExistingLanguages;
			var languages3 = currentPage.GetPageLanguage("en"); // get a page in another language

			var languages2 = currentPage.LanguageBranch; // Obsolete, use the on in ILocalizable interface



			// Connect the view models logotype property to the start page's to make it editable
			var editHints = this.ViewData.GetEditHints<PageViewModel<StartPage>, StartPage>();

			editHints.AddConnection(m => m.Layout.Logotype, p => p.SiteLogotype);
			editHints.AddConnection(m => m.Layout.ProductPages, p => p.ProductPageLinks);
			editHints.AddConnection(m => m.Layout.CompanyInformationPages, p => p.CompanyInformationPageLinks);

			// Either you change name to be same in VM adn Page or you do this stuff
			//editHints.AddConnection(m => m.Layout.NewsPages, p => p.NewsPageLinks);
			editHints.AddConnection(m => m.Layout.CustomerZonePages, p => p.CustomerZonePageLinks);


			var model = PageViewModel.Create(currentPage);

			return View(model);
		}

	}

	public class MyValidator : IValidate<StartPage>
	{
		public IEnumerable<ValidationError> Validate(StartPage instance)
		{
			// throw new System.NotImplementedException();

			return new List<ValidationError>();
		}
	}
}
