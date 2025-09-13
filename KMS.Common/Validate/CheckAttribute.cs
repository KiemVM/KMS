namespace KMS.Common.Validate
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class CheckAttribute : Attribute
    {
        public CheckAttribute(bool isCompare = true) : base()
        {
            IsCompare = isCompare;
        }

        public bool? IsCompare { set; get; }
    }
}