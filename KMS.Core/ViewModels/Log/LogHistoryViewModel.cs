using KMS.Common.Constants;
using KMS.Core.AbstractClass;
using KMS.Core.ViewModels.Identity;
using System.ComponentModel.DataAnnotations;

namespace KMS.Core.ViewModels.Log
{
    public class LogHistoryViewModel : DateTracking
    {
        public Guid Id { set; get; }
        public Guid ObjectId { set; get; }
        public Guid? UserId { set; get; }

        [MaxLength(30)]
        public string? Action { set; get; }

        public string? Content { set; get; }
        public string? Url { set; get; }

        [UIHint(UiHint.Avatar)]
        public AvatarViewModel? UserViewModel { get; set; }
    }
}