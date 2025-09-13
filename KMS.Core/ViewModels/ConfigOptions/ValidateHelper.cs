using Microsoft.EntityFrameworkCore;
using KMS.Common.Validate;
using System.Reflection;

namespace KMS.Core.ViewModels.ConfigOptions
{
    public static class ValidateHelper
    {
        public static List<string> Validate(this object? obj)
        {
            List<string> msgs = new List<string>();
            if (obj is null)
            {
                msgs.Add("Đối tượng không hợp lệ!");
                return msgs;
            }
            Type type = obj.GetType();
            if (type != (Type)null!)
            {
                PropertyInfo[] properties = type.GetProperties();
                if (properties != (PropertyInfo[])null!)
                {
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        if (!propertyInfo.PropertyType.FullName!.StartsWith("System.")) continue;
                        if (!Attribute.IsDefined(propertyInfo, typeof(ValidateAttribute)) && !Attribute.IsDefined(propertyInfo, typeof(DevRangeAttribute))) continue;
                        var comment = propertyInfo.GetCustomAttribute<CommentAttribute>()?.Comment;
                        var text = !string.IsNullOrWhiteSpace(comment) ? comment : propertyInfo.Name;
                        var value = propertyInfo.GetValue(obj, null)?.ToString()?.Trim() ?? "";
                        if (Attribute.IsDefined(propertyInfo, typeof(DevRangeAttribute)))
                        {
                            double number;
                            var check = Double.TryParse(value, out number);
                            if (!check)
                            {
                                msgs.Add($"<b>{text}</b> không hợp lệ!");
                            }
                            else
                            {
                                var min = propertyInfo.GetCustomAttribute<DevRangeAttribute>()?.Min;
                                var max = propertyInfo.GetCustomAttribute<DevRangeAttribute>()?.Max;
                                if (min != null && number < min.Value) msgs.Add($"<b>{text}</b> không được nhỏ hơn <b>{min}</b> !");
                                if (max != null && number > max.Value) msgs.Add($"<b>{text}</b> không được lớn hơn <b>{max}</b> !");
                            }
                        }
                        else
                        {
                            var isRequire = propertyInfo.GetCustomAttribute<ValidateAttribute>()?.Required ?? false;
                            if (isRequire && (value == "0" || string.IsNullOrWhiteSpace(value) || value.Equals(Guid.Empty.ToString())))
                                msgs.Add($"<b>{text}</b> không được để trống!");
                            var length = propertyInfo.GetCustomAttribute<ValidateAttribute>()?.Length;

                            if (isRequire && value.Length > length)
                                msgs.Add($"<b>{text}</b> không được vượt quá <b>{length}</b> kí tự !");
                        }

                    }
                }
            }
            return msgs;
        }
    }
}