using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMS.Core.ViewModels.Identity
{
    public class PermissionViewModel
    {
        public Guid? RoleId { get; set; }
        public string? DisplayName { get; set; }
        public IList<RoleClaimsViewModel>? RoleClaimsViewModels { get; set; }
    }
}
