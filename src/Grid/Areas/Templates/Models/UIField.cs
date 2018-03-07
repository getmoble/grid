namespace Grid.Areas.Templates.Models
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
        HtmlEditor,
        TimeAgo
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
        // DropdownDetailsBind is used as Dropdown value
       public string DropdownDetailsBind { get; set; }   

        // DropdownResult is used to return the Dropdown selected value
        public string DropdownResult { get; set; }

        //Caption is used to give caption for Dropdown caption
        public string Caption { get; set; }

        public string Binding { get; set; }
        // Knockout Binding
        public string GetReadOnlyBinding()
        {
            switch (FieldType)
            {
                case UIFieldTypes.Text:
                    return "text";
                case UIFieldTypes.TextArea:
                    return "text";
                case UIFieldTypes.DateTime:
                    return "date";
                case UIFieldTypes.TimeAgo:
                    return "timeago";
                default:
                    return "text";
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
        public static UIField GetField( string name, string title,string dropdownLabelBinding, string dropdownResultBinding,string caption, UIFieldTypes fieldType)
        {
            return new UIField
            {
                Name = name,
                Title = title,
                FieldType = fieldType,              
                DropdownDetailsBind = dropdownLabelBinding,
                DropdownResult = dropdownResultBinding,
                Caption = caption
            };
        }
    }
}