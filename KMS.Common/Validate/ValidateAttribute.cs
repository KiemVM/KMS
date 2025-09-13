namespace KMS.Common.Validate
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValidateAttribute : Attribute
    {
        public ValidateAttribute(int length = 65000, bool required = true) : base()
        {
            Required = required;
            Length = length;
        }

        public bool? Required { set; get; }
        public int? Length { set; get; }
    }
}