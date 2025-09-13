using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace KMS.Core.ViewModels.ConfigOptions
{
    public static class AttributeHelper
    {
        public static string? DisplayName(this object value)
        {
            try
            {
                return value.GetType().GetMember(value?.ToString() ?? "").First().GetCustomAttribute<CommentAttribute>()?.Comment;
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
