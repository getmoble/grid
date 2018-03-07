using System;
using System.ComponentModel;

namespace Grid.Api.Models
{
    public class ApiModelBase
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }


        public static string GetEnumDescription(Enum value)
        {
            if (value != null)
            {
                var fi = value.GetType().GetField(value.ToString());
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes.Length > 0 ? attributes[0].Description : value.ToString();
            }

            return string.Empty;
        }
    }
}
