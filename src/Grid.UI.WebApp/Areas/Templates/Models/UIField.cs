namespace Grid.UI.WebApp.Areas.Templates.Models
{
    public enum UIFieldTypes
    {
        Text,
        TextArea,
        DateTime,
        DropDown,
        CheckBox,
        Password,
        MultiSelectDropDown,
        HtmlEditor
    }

    public class UIField
    {
        // Title is used for the Table Heading
        public string Title { get; set; }

        //Property Name
        public string Name { get; set; }

        // Key is used as viewbag key
        public string ViewBagKey { get; set; }

        public UIFieldTypes FieldType  { get; set; }

        // Knockout Binding
        public string GetReadOnlyBinding()
        {
            switch (FieldType)
            {
                case UIFieldTypes.Text:
                    return "truncatedText";
                case UIFieldTypes.TextArea:
                    return "truncatedText";
                case UIFieldTypes.DateTime:
                    return "date";
                default:
                    return "truncatedText";
            }
        }
        public string GetEditableBinding()
        {
            switch (FieldType)
            {
                case UIFieldTypes.Text:
                    return "value";
                case UIFieldTypes.TextArea:
                    return "value";
                case UIFieldTypes.DateTime:
                    return "date";
                default:
                    return "value";
            }
        }

        public static UIField GetField(string name)
        {
            return new UIField
            {
                Name = name,
                Title = name,
                FieldType = UIFieldTypes.Text,
                ViewBagKey = name
            };
        }
        public static UIField GetField(string name, string title)
        {
            return new UIField
            {
                Name = name,
                Title = title,
                FieldType = UIFieldTypes.Text,
                ViewBagKey = name
            };
        }
        public static UIField GetField(string name, string title, UIFieldTypes fieldType)
        {
            return new UIField
            {
                Name = name,
                Title = title,
                FieldType = fieldType,
                ViewBagKey = name
            };
        }

        public static UIField GetField(string name, string title, UIFieldTypes fieldType, string key)
        {
            return new UIField
            {
                Name = name,
                Title = title,
                FieldType = fieldType,
                ViewBagKey = key
            };
        }
    }
}