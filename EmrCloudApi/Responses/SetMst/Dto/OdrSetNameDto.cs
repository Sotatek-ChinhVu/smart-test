using Domain.Models.SuperSetDetail;

namespace EmrCloudApi.Responses.SetMst.Dto;

public class OdrSetNameDto
{
    public OdrSetNameDto(OdrSetNameModel model)
    {
        SetCd = model.SetCd;
        SetKbn = model.SetKbn;
        SetKbnEdaNo = model.SetKbnEdaNo;
        GenerationId = model.GenerationId;
        Level1 = model.Level1;
        Level2 = model.Level2;
        Level3 = model.Level3;
        SetName = model.SetName;
        RowNo = model.RowNo;
        SortNo = model.SortNo;
        RpName = model.RpName;
        ItemCd = model.ItemCd;
        ItemName = model.ItemName;
        CmtName = model.CmtName;
        CmtOpt = model.CmtOpt;
        Quantity = model.Quantity;
        UnitName = model.UnitName;
        UnitSbt = model.UnitSbt;
        OdrTermVal = model.OdrTermVal;
        KohatuKbn = model.KohatuKbn;
        IpnCd = model.IpnCd;
        IpnName = model.IpnName;
        DrugKbn = model.DrugKbn;
        SinKouiKbn = model.SinKouiKbn;
        SyohoKbn = model.SyohoKbn;
        SyohoLimitKbn = model.SyohoLimitKbn;
        Ten = model.Ten;
        HandanGrpKbn = model.HandanGrpKbn;
        MasterSbt = model.MasterSbt;
        StartDate = model.StartDate;
        EndDate = model.EndDate;
        YohoKbn = model.YohoKbn;
        OdrKouiKbn = model.OdrKouiKbn;
        BuiKbn = model.BuiKbn;
    }

    public int SetCd { get; private set; }

    public int SetKbn { get; private set; }

    public int SetKbnEdaNo { get; private set; }

    public int GenerationId { get; private set; }

    public int Level1 { get; private set; }

    public int Level2 { get; private set; }

    public int Level3 { get; private set; }

    public string SetName { get; private set; }

    public int RowNo { get; private set; }

    public int SortNo { get; private set; }

    public string RpName { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public string CmtName { get; private set; }

    public string CmtOpt { get; private set; }

    public double Quantity { get; private set; }

    public string UnitName { get; private set; }

    public int UnitSbt { get; private set; }

    public double OdrTermVal { get; private set; }

    public int KohatuKbn { get; private set; }

    public string IpnCd { get; private set; }

    public string IpnName { get; private set; }

    public int DrugKbn { get; private set; }

    public int SinKouiKbn { get; private set; }

    public int SyohoKbn { get; private set; }

    public int SyohoLimitKbn { get; private set; }

    public double Ten { get; private set; }

    public int HandanGrpKbn { get; private set; }

    public string MasterSbt { get; private set; }

    public int StartDate { get; private set; }

    public int EndDate { get; private set; }

    public int YohoKbn { get; private set; }

    public int OdrKouiKbn { get; private set; }

    public int BuiKbn { get; private set; }
}
