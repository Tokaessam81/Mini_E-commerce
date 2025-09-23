using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Core.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; } = null!;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsExpired;
    }

}
