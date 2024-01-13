using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Tenant
{
    [Table(name: "user_token")]
    public class UserToken
    {
        [Required]
        [Column(name: "user_id", Order = 1)]
        public int UserId { get; set; }

        [Required]
        [Column(name: "refresh_token", Order = 2)]
        public string RefreshToken { get; set; } = string.Empty;

        [Column(name: "token_expiry_time", Order = 3)]
        public DateTime RefreshTokenExpiryTime { get; set; }

        [Column(name: "refresh_token_is_used", Order = 4)]
        public bool RefreshTokenIsUsed { get; set; }
    }
}
