using KMS.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Common.Constants
{
    public static class ConstTitle
    {
        public static string User = "Người dùng";
        public static string Role = "Phân quyền";
        public static string Company = "Công ty";
        public static string Application = "Ứng dụng";
        public static string Key = "Key";
    }

    public static class ConstPathUrl
    {
        public static string User = "/" + ConstTitle.User.ToSlugUrl();
        public static string Role = "/" + ConstTitle.Role.ToSlugUrl();
        public static string Company = "/" + ConstTitle.Company.ToSlugUrl();
        public static string Application = "/" + ConstTitle.Application.ToSlugUrl();
        public static string Key = "/" + ConstTitle.Key.ToSlugUrl();
    }
}
