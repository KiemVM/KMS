namespace KMS.Common.Models
{
    public class InputFormViewModel
    {
        public string? Id { set; get; } = "";
        public string? Value { set; get; } = "";
        public string? ValueText { set; get; } = "";
        public string? Text { set; get; } = "";
        public bool IsRequire { set; get; } = false;

        /// <summary>
        /// Đường dẫn nếu là Combobox
        /// </summary>
        public string? Url { set; get; } = "";
        public bool IsDisable { set; get; } = false;
        public bool IsDialog { set; get; } = true;
        public bool IsTime { set; get; } = false;
        /// <summary>
        /// Nếu là Số
        /// </summary>
        public bool IsNumber { set; get; } = false;
        /// <summary>
        /// Có thể hiện Label ở Input hay không
        /// </summary>
        public bool IsLabel { set; get; } = true;
        public string? FuncJsOnchange { set; get; } = "";
        /// <summary>
        /// Mở rộng
        /// </summary>
        public string? Type { set; get; } = "";

        /// <summary>
        /// Có phải là input search hay không
        /// </summary>
        public bool IsSearch { set; get; } = false;


    }
}