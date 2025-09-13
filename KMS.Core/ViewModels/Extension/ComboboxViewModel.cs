namespace KMS.Core.ViewModels.Extension
{
    public class ComboboxViewModel
    {
        public Guid Value { set; get; }
        public string? Name { set; get; }
        public string? Description { set; get; }
        public string? Avatar { set; get; }
    }

    // Thêm ViewModel mới cho tree structure
    public class ComboboxTreeViewModel
    {
        public Guid Value { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Guid ParentId { get; set; }
        public int Level { get; set; }
        public string DisplayName { get; set; }
    }

    public class AttachmentViewModel
    {
        public string? Url { get; set; }
        public string? NameFile { get; set; }
        public double Size { get; set; }
        public string? Extension { get; set; }
    }

    public static class AttachmentExtension
    {
        public static List<AttachmentViewModel> GetAttachment(this string? value, string url)
        {
            List<AttachmentViewModel> result = new();
            if (string.IsNullOrEmpty(value)) return result;
            var attachments = value.Split(",");
            foreach (var attachment in attachments)
            {
                var nameFile = attachment.Replace(url, "").Split(" ").LastOrDefault()?.Trim();
                result.Add(new AttachmentViewModel()
                {
                    Url = attachment,
                    NameFile = nameFile
                });
            }

            return result;
        }
    }
}