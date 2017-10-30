using System.Web.Mvc;
using Alloy.Oktober2017.Web.Models.Pages;
using Alloy.Oktober2017.Web.Models.ViewModels;
using EPiServer.Web;
using EPiServer.Web.Mvc;

namespace Alloy.Oktober2017.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Web.Security;
	using Alloy.Oktober2017.Web.Business;
	using EPiServer;
	using EPiServer.AddOns.Helpers;
	using EPiServer.Cms.Shell.UI.Rest.ContentQuery.Internal;
	using EPiServer.Core;
	using EPiServer.DataAbstraction;
	using EPiServer.DataAbstraction.RuntimeModel;
	using EPiServer.Filters;
	using EPiServer.Personalization;
	using EPiServer.Security;
	using EPiServer.Shell.ContentQuery;
	using EPiServer.Validation;
	using EPiServer.Web.Routing;

	public class StartPageController : PageControllerBase<StartPage>
	{
		private readonly SearchService _searchService;

		private readonly IContentRepository contentRepository;

		private readonly IValidationService validationService;
		private readonly IContentProviderManager _contentProviderManager;
		private readonly ILanguageBranchRepository _languageBranchRepository;

		// Can be used to convert ContentReference to parmeant link. Not the other way around
		private readonly IPermanentLinkMapper permanentLinkMapper;

		private readonly IContentLanguageSettingsResolver contentLanguageSettingsResolver;

		private readonly IContentSecurityRepository contentSecurityRepository;

		private readonly ISiteDefinitionRepository siteDefinitionRepository;

		private readonly ProjectRepository projectRepository;

		private readonly IPageSource pageSource;



		public StartPageController(
			IValidationService validationService,
			IContentProviderManager contentProviderManager,
			ILanguageBranchRepository languageBranchRepository,
			IPermanentLinkMapper ermanentLinkMapper,
			IContentLanguageSettingsResolver contentLanguageSettingsResolver,
			IContentSecurityRepository contentSecurityRepository,
			ISiteDefinitionRepository siteDefinitionRepository,
			ProjectRepository projectRepository, IPageSource pageSource, IContentRepository contentRepository, SearchService searchService)
		{
			this.validationService = validationService;
			this._contentProviderManager = contentProviderManager;
			this._languageBranchRepository = languageBranchRepository;
			this.permanentLinkMapper = ermanentLinkMapper;
			this.contentLanguageSettingsResolver = contentLanguageSettingsResolver;
			this.contentSecurityRepository = contentSecurityRepository;
			this.siteDefinitionRepository = siteDefinitionRepository;
			this.projectRepository = projectRepository;
			this.pageSource = pageSource;
			this.contentRepository = contentRepository;
			this._searchService = searchService;
		}

		public ActionResult Index(StartPage currentPage)
		{
			// serach
			var result = _searchService
				.Search(
				"alloy",
				new[] { SiteDefinition.Current.StartPage },
				this.HttpContext,
				currentPage.Language.Name,
				10
				);

			// IPAgeSource
			PageDataCollection children = pageSource.GetChildren(currentPage.PageLink);
			IEnumerable<IContent> children2 = this.contentRepository.GetChildren<IContent>(currentPage.ContentLink);

			// Static Filter for both Icontent and PageDataCollection
			var filtred = EPiServer.Filters.FilterForVisitor.Filter(children2);

			// FIlter for PageDataCollection:
			var filter = new FilterAccess();
			filter.Filter(children);

			// Content Model stuff
			// return guid perma link
			var linkUrl = currentPage.LinkURL;

			// Whats the diffeent
			string name = currentPage.Name; // Gets or sets the name of the page.
			string pageName = currentPage.PageName; // Gets or sets the display name of page.
			PageReference pageLink = currentPage.PageLink;
			ContentReference contentLink = currentPage.ContentLink;

			// -- Other stuff --------
			var providerMap = this._contentProviderManager.ProviderMap;
			var isWastebasket = this._contentProviderManager.IsWastebasket(currentPage.ContentLink);

			var securityDescriptor = this.contentSecurityRepository.Get(currentPage.ContentLink);

			var allSiteDefinitions = this.siteDefinitionRepository.List();
			// check access rights on content with ACL = Access Control List
			var creator = currentPage.ACL.Creator;

			// WIll check if the current use have access or not to this content
			bool iHaveReadAccess = currentPage.ACL.QueryDistinctAccess(AccessLevel.Read);

			// How to validate a page using IValtidate
			this.validationService.Validate(currentPage);

			// --  WORKING with proeject programattiaclly ---

			// this creates a new project and saves it.
			Project project = new Project { Name = "Tobes code project" };
			// this._projectRepository.Save(project);


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

			var readAccess = currentPage.QueryDistinctAccess(AccessLevel.Read);
			var readAccess2 = currentPage.ACL.QueryDistinctAccess(AccessLevel.Read);

			var readAccess3 = currentPage.ACL.QueryAccess() == AccessLevel.Read;

			var readAccess4 = currentPage.QueryAccess() == AccessLevel.Read;

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
			var allLanguageInWebSite = this._languageBranchRepository.ListAll();

			var contentLanguageSettings = this.contentLanguageSettingsResolver.Resolve(currentPage.ContentLink);

			PageData pageInNewLang = currentPage.GetPageLanguage("en"); // get a page in another language

			// The new ones Page langauge
			IEnumerable<CultureInfo> languages = currentPage.ExistingLanguages;
			CultureInfo currLang = currentPage.Language;

			// The old ones
			string languages2 = currentPage.LanguageBranch; // Obsolete, use the on in ILocalizable interface ExistingLange
			ReadOnlyStringList oldGetLanague = currentPage.PageLanguages;

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
