using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.SetMst;

public class ReorderSetMstRequestItem
{
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int HpId { get; set; } = 0;

    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int SetCd { get; set; } = 0;

    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int SetKbn { get; set; }

    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int SetKbnEdaNo { get; set; }

    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int GenerationId { get; set; } = 0;

    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int Level1 { get; set; }

    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int Level2 { get; set; } = 0;

    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int Level3 { get; set; } = 0;
}
