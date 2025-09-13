namespace KMS.Common.Models
{
    public class InputDateRangeViewModel
    {
        public string? Id { get; set; }
        public bool IsRequire { set; get; } = false;
        public bool IsLabel { set; get; } = true;
        /// <summary>
        /// Hiển thị nhãn label
        /// </summary>
        public string? Text { get; set; } = "Khoảng thời gian";
        public DateTime DateStart { get; set; } = new DateTime(DateTime.Now.Year, 1, 1);
        public DateTime DateEnd { get; set; } = DateTime.Now.Date;
        public string? FuncJsOnchange { get; set; }
        /// <summary>
        /// Có phải là input search hay không
        /// </summary>
        public bool IsSearch { get; set; } = false;
    }
}
