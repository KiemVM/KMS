using Microsoft.EntityFrameworkCore;
using KMS.Common.Helper;
using KMS.Common.Models;
using KMS.Common.Validate;
using System.Reflection;

namespace KMS.Core.ViewModels.ConfigOptions
{
    public static class InputFormHelper
    {
        public static List<InputFormViewModel> GetInputForms(this object? obj)
        {
            List<InputFormViewModel> inputFormViewModels = new List<InputFormViewModel>();
            if (obj is null) return inputFormViewModels;
            Type type = obj.GetType();
            if (type != (Type)null!)
            {
                PropertyInfo[] properties = type.GetProperties();
                if (properties != (PropertyInfo[])null!)
                {
                    foreach (PropertyInfo propertyInfo in properties)
                    {
                        if (!propertyInfo.PropertyType.FullName!.StartsWith("System.")) continue;
                        var comment = propertyInfo.GetCustomAttribute<CommentAttribute>()?.Comment;
                        var isDateTime = propertyInfo.PropertyType.FullName.Contains("System.DateTime");
                        var isNumber = propertyInfo.PropertyType.FullName.Contains("System.Double") || propertyInfo.PropertyType.FullName.Contains("System.Int") || propertyInfo.PropertyType.FullName.Contains("System.Float");
                        var propertyType = propertyInfo.PropertyType.FullName;
                        var value = "";
                        var typeInput = "";
                        if (isDateTime)
                        {
                            value = string.IsNullOrEmpty((propertyInfo.GetValue(obj, null)?.ToString()?.Trim() ?? ""))
                                ? DateTime.Now.ConvertDateTimeToString()
                                : ((DateTime)propertyInfo.GetValue(obj, null)!).ConvertDateTimeToString();
                            typeInput = "input-date";
                        }
                        else if (isNumber)
                        {
                            value = propertyInfo.GetValue(obj, null).ConvertMoney()?.ToString()?.Trim() ?? "";
                        }
                        else
                        {
                            value = propertyInfo.GetValue(obj, null)?.ToString()?.Trim() ?? "";
                        }
                        inputFormViewModels.Add(new InputFormViewModel()
                        {
                            Id = propertyInfo.Name,
                            IsNumber = isNumber,
                            Type = typeInput,
                            Text = !string.IsNullOrWhiteSpace(comment) ? comment : propertyInfo.Name,
                            Value = value,
                            IsRequire = propertyInfo.GetCustomAttribute<ValidateAttribute>()?.Required ?? false
                        });
                    }
                }
            }
            return inputFormViewModels;
        }
    }
}