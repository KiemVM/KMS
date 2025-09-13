namespace KMS.Common.Models
{
    public class ButtonViewModel
    {
        public Guid Id { set; get; }
        public string? Text { set; get; }
        public string? Url { set; get; } = "";
        public bool IsRole { set; get; } = false;
        public string? ControllerName { set; get; } = "";
        public string? Icon { set; get; } = "";
        public string? FuncJs { set; get; } = "";
        /// <summary>
        /// Background Btn
        /// </summary>
        public string? ClassBg { set; get; } = "";
    }
}