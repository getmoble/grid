using System;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Grid.Infrastructure.Helpers
{
    public static class GridHtmlHelper
    {
        private static MvcHtmlString GridLabelFor<TModel, TValue>(HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.LabelFor(expression, new { @class = "control-label col-md-2" });
        }

        private static MvcHtmlString GridEditorFor<TModel, TValue>(HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.EditorFor(expression, new { htmlAttributes = new { @class = "form-control" }});
        }

        private static MvcHtmlString GridValidationMessageFor<TModel, TValue>(HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return html.ValidationMessageFor(expression, "", new { @class = "text-danger" });
        }

        public static MvcHtmlString GridFieldFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var formgroupContainer = new TagBuilder("div");
            formgroupContainer.AddCssClass("form-group");
                var label = GridLabelFor(html, expression);

                var colgroup10 = new TagBuilder("div");
                colgroup10.AddCssClass("col-md-10");

                    var editor = GridEditorFor(html, expression);
                    var validationMessage = GridValidationMessageFor(html, expression);
                    var colgroupInnerHtml = new StringBuilder();
                    colgroupInnerHtml.Append(editor);
                    colgroupInnerHtml.Append(validationMessage);

                colgroup10.InnerHtml = colgroupInnerHtml.ToString();

            var formGroupInnerHtml = new StringBuilder();
            formGroupInnerHtml.Append(label);
            formGroupInnerHtml.Append(colgroup10);

            formgroupContainer.InnerHtml = formGroupInnerHtml.ToString();
            return MvcHtmlString.Create(formgroupContainer.ToString());
        }
    }
}
