using Domain.Models.DrugInfor;

namespace EmrCloudApi.Responses.DrugInfor.Dto;

public class SinrekiFilterMstKouiDto
{
    public SinrekiFilterMstKouiDto(SinrekiFilterMstKouiModel model)
    {
        GrpCd = model.GrpCd;
        SeqNo = model.SeqNo;
        KouiKbnId = model.KouiKbnId;
        IsChecked = model.IsChecked;
    }

    public int GrpCd { get; private set; }

    public long SeqNo { get; private set; }

    public int KouiKbnId { get; private set; }

    public bool IsChecked { get; private set; }
}
