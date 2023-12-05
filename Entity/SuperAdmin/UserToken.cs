using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.SuperAdmin
{
    [Table("USER_TOKEN")]
    public class UserToken
    {
        [Column("USER_ID")]
        public int UserId { get; set; }

        [Column("REFRESH_TOKEN")]
        public string RefreshToken { get; set; } = string.Empty;

        [Column("TOKEN_EXPIRY_TIME")]
        public DateTime RefreshTokenExpiryTime { get; set; }

        [Column("REFRESH_TOKEN_IS_USED")]
        public bool RefreshTokenIsUsed { get; set; }
    }
}
