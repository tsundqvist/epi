using EPiServer.Core;

namespace Alloy.Oktober2017.Web.Models.Pages
{
    public interface IHasRelatedContent
    {
        ContentArea RelatedContentArea { get; }
    }
}
