using System.ComponentModel.DataAnnotations;
using Alloy.Oktober2017.Web.Models.Blocks;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Alloy.Oktober2017.Web.Models.Properties;

namespace Alloy.Oktober2017.Web.Models.Pages
{
	using EPiServer.Framework.Blobs;

	/// <summary>
    /// Used to present a single product
    /// </summary>
    [SiteContentType(
        GUID = "17583DCD-3C11-49DD-A66D-0DEF0DD601FC",
        GroupName = Global.GroupNames.Products)]
    [SiteImageUrl(Global.StaticGraphicsFolderPath + "page-type-thumbnail-product.png")]
    [AvailableContentTypes( 
        Availability = Availability.Specific,
        IncludeOn = new[] { typeof(StartPage) })]
    public class ProductPage : StandardPage, IHasRelatedContent
    {
        [Required]
        [BackingType(typeof(PropertyStringList))]
        [Display(Order = 305)]
        [UIHint(Global.SiteUIHints.Strings)]
        [CultureSpecific]
        public virtual string[] UniqueSellingPoints { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 330)]
        [CultureSpecific]
        [AllowedTypes(new[] { typeof(IContentData) },new[] { typeof(JumbotronBlock) })]
        public virtual ContentArea RelatedContentArea { get; set; }


	 //   [Display(
		//    GroupName = SystemTabNames.Content,
		//    Order = 330)]
		//public virtual FileBlob FilePath { get; set; }

	 //   [Display(
		//    GroupName = SystemTabNames.Content,
		//    Order = 330)]
	 //   public virtual FilePath FilePath { get; set; }

		//public virtual MapLocationPoint MapLocationPoint { get; set; }

		//[Display(GroupName = SystemTabNames.Content,Order = 330)]
		//public virtual FileContent FileContent { get; set; }


	}
}
