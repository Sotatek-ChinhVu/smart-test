using System.ComponentModel.DataAnnotations;

namespace EmrCloudApi.Tenant.Requests.SetMst;

public class SaveSetMstRequest
{
    [Required]
    [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "{0} is Required")]
    public int SinDate { get; set; }

    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int SetCd { get; set; } = 0;

    [Required]
    [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
    public int SetKbn { get; set; }

    [Required]
    [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
    public int SetKbnEdaNo { get; set; }

    public int GenerationId { get; set; } = 0;

    [Required]
    [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
    public int Level1 { get; set; }

    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int Level2 { get; set; } = 0;

    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int Level3 { get; set; } = 0;

    [MaxLength(60)]
    public string SetName { get; set; } = string.Empty;

    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int WeightKbn { get; set; } = 0;

    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int Color { get; set; } = 0;

    [RegularExpression(@"^[0-1]$", ErrorMessage = "{0} = 0 or 1")]
    public int IsDeleted { get; set; } = 0;

    public bool IsGroup { get; set; } = false;
}
