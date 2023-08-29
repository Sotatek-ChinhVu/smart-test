using Domain.Models.HpInf;

namespace EmrCloudApi.Responses.Reception;

public class HpInfDto
{
    public HpInfDto(HpInfModel model)
    {
        HpId = model.HpId;
        StartDate = model.StartDate;
        HpCd = model.HpCd;
        RousaiHpCd = model.RousaiHpCd;
        HpName = model.HpName;
        ReceHpName = model.ReceHpName;
        KaisetuName = model.KaisetuName;
        PostCd = model.PostCd;
        PrefNo = model.PrefNo;
        Address1 = model.Address1;
        Address2 = model.Address2;
        Tel = model.Tel;
        FaxNo = model.FaxNo;
        OtherContacts = model.OtherContacts;
    }
    public int HpId { get; private set; }

    public int StartDate { get; private set; }

    public string HpCd { get; private set; }

    public string RousaiHpCd { get; private set; }

    public string HpName { get; private set; }

    public string ReceHpName { get; private set; }

    public string KaisetuName { get; private set; }

    public string PostCd { get; private set; }

    public int PrefNo { get; private set; }

    public string Address1 { get; private set; }

    public string Address2 { get; private set; }

    public string Tel { get; private set; }

    public string FaxNo { get; private set; }

    public string OtherContacts { get; private set; }

    public int UpdateId { get; private set; }
}
