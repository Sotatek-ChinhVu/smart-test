using Domain.Models.DrugInfor;

namespace EmrCloudApi.Responses.DrugInfor.Dto;

public class SinrekiFilterMstDto
{
    public SinrekiFilterMstDto(SinrekiFilterMstModel model)
    {
        GrpCd = model.GrpCd;
        Name = model.Name;
        SortNo = model.SortNo;
        SinrekiFilterMstKouiList = model.SinrekiFilterMstKouiList.Select(item => new SinrekiFilterMstKouiDto(item)).ToList();
        SinrekiFilterMstDetailList = model.SinrekiFilterMstDetailList.Select(item => new SinrekiFilterMstDetailDto(item)).ToList();
    }

    public int GrpCd { get; private set; }

    public string Name { get; private set; }

    public int SortNo { get; private set; }

    public List<SinrekiFilterMstKouiDto> SinrekiFilterMstKouiList { get; private set; }

    public List<SinrekiFilterMstDetailDto> SinrekiFilterMstDetailList { get; private set; }
}
