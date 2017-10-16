using Alloy.Oktober2017.Web.Business.Rendering;

namespace Alloy.Oktober2017.Web.Models.Pages
{
    /// <summary>
    /// Used to logically group pages in the page tree
    /// </summary>
    [SiteContentType(
        GUID = "D178950C-D20E-4A46-90BD-5338B2424745",
        GroupName = Global.GroupNames.Specialized)]
    [SiteImageUrl]
    public class ContainerPage : SitePageData, IContainerPage
    {
        
    }
}
