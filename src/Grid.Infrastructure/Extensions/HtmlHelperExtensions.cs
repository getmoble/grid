using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Grid.Infrastructure.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static bool IsDebug(this HtmlHelper htmlHelper)
        {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }

        public static IHtmlString GridAddButton(this HtmlHelper helper, EntityType entityType)
        {
            var builder = new StringBuilder();

            var button = new TagBuilder("a");
            button.AddCssClass("float");
            button.Attributes.Add("href", UrlGenerator.CreatePage(entityType));

            var icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-plus fa-2x fab-button");
            button.InnerHtml = icon.ToString();

            builder.AppendLine(button.ToString());
            return new HtmlString(builder.ToString());
        }

        public static IHtmlString GridAddButton(this HtmlHelper helper, string createLink)
        {
            var builder = new StringBuilder();

            var button = new TagBuilder("a");
            button.AddCssClass("float");
            button.Attributes.Add("href", createLink);

            var icon = new TagBuilder("i");
            icon.AddCssClass("fa fa-plus fa-2x fab-button");
            button.InnerHtml = icon.ToString();

            builder.AppendLine(button.ToString());
            return new HtmlString(builder.ToString());
        }
    }
}
