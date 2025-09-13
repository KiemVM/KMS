namespace KMS.Core.AbstractClass
{
    public abstract class DateTracking
    {
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
