using Domain.Models.SetKbnMst;

namespace EmrCloudApi.Responses.SetKbnMst;

public class SetKbnMstDto
{
    public SetKbnMstDto(SetKbnMstModel model)
    {
        SetKbn = model.SetKbn;
        SetKbnEdaNo = model.SetKbnEdaNo;
        SetKbnName = model.SetKbnName;
        KaCd = model.KaCd;
        DocCd = model.DocCd;
        GenerationId = model.GenerationId;
    }

    public int SetKbn { get; private set; }

    public int SetKbnEdaNo { get; private set; }

    public string SetKbnName { get; private set; }

    public int KaCd { get; private set; }

    public int DocCd { get; private set; }

    public int GenerationId { get; private set; }
}
