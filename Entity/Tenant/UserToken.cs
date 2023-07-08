using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "USER_TOKEN")]
    public class UserToken
    {
        [Required]
        [Column(name: "USER_ID", Order = 1)]
        public int UserId { get; set; }

        [Required]
        [Column(name: "REFRESH_TOKEN", Order = 2)]
        public string RefreshToken { get; set; } = string.Empty;

        [Column(name: "TOKEN_EXPIRY_TIME", Order = 3)]
        public DateTime RefreshTokenExpiryTime { get; set; }

        [Column(name: "REFRESH_TOKEN_IS_USED", Order = 4)]
        public bool RefreshTokenIsUsed { get; set; }
    }
}
