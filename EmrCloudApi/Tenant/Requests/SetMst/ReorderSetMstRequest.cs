using System.ComponentModel.DataAnnotations;
using UseCase.SetMst.ReorderSetMst;

namespace EmrCloudApi.Tenant.Requests.SetMst;

public class ReorderSetMstRequest
{
    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int HpId { get; set; }

    [Required]
    [RegularExpression(@"^[1-9][0-9]*$", ErrorMessage = "{0} > 0")]
    public int DragSetCd { get; set; }

    [Required]
    [RegularExpression(@"^[0-9]*$", ErrorMessage = "{0} >= 0")]
    public int DropSetCd { get; set; }
}