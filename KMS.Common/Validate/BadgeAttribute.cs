namespace KMS.Common.Validate
{
    public sealed class BadgeAttribute : Attribute
    {
        public BadgeAttribute(string badgeName) : base()
        {
            BadgeName = badgeName;
        }

        public string? BadgeName { set; get; }
    }
}