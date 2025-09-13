using KMS.Common.Validate;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace KMS.Common.Helper
{
    public static class EnumHelper
    {
        public static string? Display(this Enum value)
        {
            try
            {
                return value?.GetType()?.GetMember(value.ToString())?.First()?.GetCustomAttribute<DisplayAttribute>()?.GetName() ?? "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string? CssClass(this Enum value)
        {
            try
            {
                return value.GetType().GetMember(value.ToString()).First().GetCustomAttribute<CssClassAttribute>()?.CssClassName;
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static string? Badge(this Enum value,string? className = "")
        {
            try
            {
                var badge = value.GetType().GetMember(value.ToString()).First().GetCustomAttribute<BadgeAttribute>()?.BadgeName;
                return $"<span class=\"badge {badge} {className} fw-semibold me-1\">{Display(value)}</span>";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        public static int Value(this Enum value)
        {
            return Convert.ToInt32(value);
        }
    }
}