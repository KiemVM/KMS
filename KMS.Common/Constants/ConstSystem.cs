using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Common.Constants
{
    public static class ConstSystem
    {
        public static string Update = "Cập nhật";
        public static string Insert = "Thêm mới";
        public static string Delete = "Xóa";
        public static string View = "Xem";
        public static string RoleAdmin = "Admin";
        public static string ProjectName = "KMS";
        public static Guid GuidAll = Guid.Parse("5c120683-4802-40b6-805e-3b54d492a2a3");
        public static Guid UserConfirmJob = Guid.Parse("68096520-5B01-47BA-9E58-013BEC6C2CAE");
    }

    public static class InputForm
    {
        public const string TextBox = "~/Pages/Shared/Components/InputForm/TextBox.cshtml";
        public const string Combobox = "~/Pages/Shared/Components/InputForm/Combobox.cshtml";
        public const string ComboboxSearch = "~/Pages/Shared/Components/InputForm/ComboboxSearch.cshtml";
        public const string DateRange = "~/Pages/Shared/Components/InputForm/DateRange.cshtml";
        public const string TextArea = "~/Pages/Shared/Components/InputForm/TextArea.cshtml";
    }

    public class UiHint
    {
        public const string Avatar = nameof(Avatar);
    }

    public static class ConstUrl
    {
        public const string ApiErrorCrm = "https://crm.phudev.com/Api/AppError/Add";
        public const string WebFile = "https://file.phudev.com";
        public const string WebCdn = "https://cdn.phudev.com";
        public const string WebCrm = "https://crm.phudev.com";
    }
}