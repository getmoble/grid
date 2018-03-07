using System.Collections.Generic;
using System.Linq;

namespace Grid.Providers.Email
{
    public class SimpleTemplateProcessor
    {
        public static string Process(string template, List<PlaceHolder> placeHolders)
        {
            if (!string.IsNullOrEmpty(template) && placeHolders != null && placeHolders.Any())
                template = placeHolders.Aggregate(template, (current, placeHolder) => current.Replace(placeHolder.Key, placeHolder.Value));

            return template;
        }
    }
}
