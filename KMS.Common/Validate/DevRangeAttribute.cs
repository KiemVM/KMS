namespace KMS.Common.Validate
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DevRangeAttribute:Attribute
    {
        public DevRangeAttribute(double min = 0, double max = 0) : base()
        {
            Min = min;
            Max = max;
        }

        public DevRangeAttribute(int min = 0, int max = 0) : base()
        {
            Min = Convert.ToDouble(min);
            Max = Convert.ToDouble(max);
        }

        public double? Min { set; get; }
        public double? Max { set; get; }
    }
}
