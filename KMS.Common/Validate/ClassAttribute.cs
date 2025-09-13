namespace KMS.Common.Validate
{
    public sealed class CssClassAttribute : Attribute
    {
        public CssClassAttribute(string cssClassName) : base()
        {
            CssClassName = cssClassName;
        }

        public string? CssClassName { set; get; }
    }
}