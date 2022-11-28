using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem.DiseaseSearch;

public class DiseaseSearchModel
{
    public DiseaseSearchModel(ByomeiMstModel model)
    {
        ByomeiCd = model.ByomeiCd;
        ByomeiType = model.ByomeiType;
        Sbyomei = model.Sbyomei;
        KanaName1 = model.KanaName1;
        Sikkan = model.Sikkan;
        NanByo = model.NanByo;
        Icd10 = model.Icd10;
        Icd102013 = model.Icd102013;
        IsAdopted = model.IsAdopted;
    }

    public string ByomeiCd { get; private set; }

    public string ByomeiType { get; private set; }

    public string Sbyomei { get; private set; }

    public string KanaName1 { get; private set; }

    public string Sikkan { get; private set; }

    public string NanByo { get; private set; }

    public string Icd10 { get; private set; }

    public string Icd102013 { get; private set; }

    public bool IsAdopted { get; private set; }
}
