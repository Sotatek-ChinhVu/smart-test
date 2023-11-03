using Domain.Models.MstItem;
using Domain.Models.SetMst;
using Helper.Extension;
using System.Collections.ObjectModel;

namespace Domain.Models.KensaIrai;

public class KensaMstModel
{

    public KensaMstModel(string kensaItemCd, int kensaItemSeqNo, string centerCd, string kensaName, string kensaKana, string unit, int materialCd, int containerCd, string maleStd, string maleStdLow, string maleStdHigh, string femaleStd, string femaleStdLow, string femaleStdHigh, string formula, int digit, string oyaItemCd, int oyaItemSeqNo, long sortNo, string centerItemCd1, string centerItemCd2)
    {
        KensaItemCd = kensaItemCd;
        KensaItemSeqNo = kensaItemSeqNo;
        CenterCd = centerCd;
        KensaName = kensaName;
        KensaKana = kensaKana;
        Unit = unit;
        MaterialCd = materialCd;
        ContainerCd = containerCd;
        MaleStd = maleStd;
        MaleStdLow = maleStdLow;
        MaleStdHigh = maleStdHigh;
        FemaleStd = femaleStd;
        FemaleStdLow = femaleStdLow;
        FemaleStdHigh = femaleStdHigh;
        Formula = formula;
        Digit = digit;
        OyaItemCd = oyaItemCd;
        OyaItemSeqNo = oyaItemSeqNo;
        SortNo = sortNo;
        CenterItemCd1 = centerItemCd1;
        CenterItemCd2 = centerItemCd2;
        TenMsts = new();
        ChildKensaMsts = new();
        TenItemModels = new();
        ParentKensaMst = new();
        CenterName = string.Empty;
    }

    public KensaMstModel()
    {
        KensaItemCd = string.Empty;
        CenterCd = string.Empty;
        KensaName = string.Empty;
        KensaKana = string.Empty;
        Unit = string.Empty;
        MaleStd = string.Empty;
        MaleStdLow = string.Empty;
        MaleStdHigh = string.Empty;
        FemaleStd = string.Empty;
        FemaleStdLow = string.Empty;
        FemaleStdHigh = string.Empty;
        Formula = string.Empty;
        OyaItemCd = string.Empty;
        CenterItemCd1 = string.Empty;
        CenterItemCd2 = string.Empty;
        TenMsts = new();
        ChildKensaMsts = new();
        TenItemModels = new();
        ParentKensaMst = new();
        CenterName = string.Empty;
    }

    public KensaMstModel(string kensaItemCd, int kensaItemSeqNo, string centerCd, string kensaName, string kensaKana, string unit, int materialCd, int containerCd, string maleStd, string maleStdLow, string maleStdHigh, string femaleStd, string femaleStdLow, string femaleStdHigh, string formula, int digit, string oyaItemCd, int oyaItemSeqNo, long sortNo, string centerItemCd1, string centerItemCd2, List<TenItemModel> tenMsts, List<TenItemModel> tenItemModels, List<KensaMstModel> kensaMstModels, object parentKensaMst, string centerName )
    {
        KensaItemCd = kensaItemCd;
        KensaItemSeqNo = kensaItemSeqNo;
        CenterCd = centerCd;
        KensaName = kensaName;
        KensaKana = kensaKana;
        Unit = unit;
        MaterialCd = materialCd;
        ContainerCd = containerCd;
        MaleStd = maleStd;
        MaleStdLow = maleStdLow;
        MaleStdHigh = maleStdHigh;
        FemaleStd = femaleStd;
        FemaleStdLow = femaleStdLow;
        FemaleStdHigh = femaleStdHigh;
        Formula = formula;
        Digit = digit;
        OyaItemCd = oyaItemCd;
        OyaItemSeqNo = oyaItemSeqNo;
        SortNo = sortNo;
        CenterItemCd1 = centerItemCd1;
        CenterItemCd2 = centerItemCd2;
        TenMsts = tenMsts;
        TenItemModels = tenItemModels;
        ChildKensaMsts = kensaMstModels;
        ParentKensaMst = parentKensaMst;
        CenterName = centerName;
    }

    public KensaMstModel(string kensaItemCd, int kensaItemSeqNo, string centerCd, string kensaName, string kensaKana, string unit, int materialCd, int containerCd, string maleStd, string maleStdLow, string maleStdHigh, string femaleStd, string femaleStdLow, string femaleStdHigh, string formula, int digit, string oyaItemCd, int oyaItemSeqNo, long sortNo, string centerItemCd1, string centerItemCd2, int isDeleted)
    {
        KensaItemCd = kensaItemCd;
        KensaItemSeqNo = kensaItemSeqNo;
        CenterCd = centerCd;
        KensaName = kensaName;
        KensaKana = kensaKana;
        Unit = unit;
        MaterialCd = materialCd;
        ContainerCd = containerCd;
        MaleStd = maleStd;
        MaleStdLow = maleStdLow;
        MaleStdHigh = maleStdHigh;
        FemaleStd = femaleStd;
        FemaleStdLow = femaleStdLow;
        FemaleStdHigh = femaleStdHigh;
        Formula = formula;
        Digit = digit;
        OyaItemCd = oyaItemCd;
        OyaItemSeqNo = oyaItemSeqNo;
        SortNo = sortNo;
        CenterItemCd1 = centerItemCd1;
        CenterItemCd2 = centerItemCd2;
        IsDeleted = isDeleted;
        TenMsts = new();
        ChildKensaMsts = new();
        TenItemModels = new();
        ParentKensaMst = new();
        CenterName = string.Empty;
    }

    public void SetCenterName(string newCenterName)
    {
        CenterName = newCenterName;
    }

    public void SetCenterItemCd1(string newCenterItemCd1)
    {
        CenterItemCd1 = newCenterItemCd1;
    }

    public string KensaItemCd { get; private set; }

    public int KensaItemSeqNo { get; private set; }

    public string CenterCd { get; private set; }

    public string KensaName { get; private set; }

    public string KensaKana { get; private set; }

    public string Unit { get; private set; }

    public int MaterialCd { get; private set; }

    public int ContainerCd { get; private set; }

    public string MaleStd { get; private set; }

    public string MaleStdLow { get; private set; }

    public string MaleStdHigh { get; private set; }

    public string FemaleStd { get; private set; }

    public string FemaleStdLow { get; private set; }

    public string FemaleStdHigh { get; private set; }

    public string Formula { get; private set; }

    public int Digit { get; private set; }

    public string OyaItemCd { get; private set; }

    public int OyaItemSeqNo { get; private set; }

    public long SortNo { get; private set; }

    public string CenterItemCd1 { get; private set; }

    public string CenterItemCd2 { get; private set; }

    public string CenterItemCd
    {
        get { return CenterItemCd1; }
    }

    public List<TenItemModel> TenMsts { get; private set; }

    public List<TenItemModel> TenItemModels { get; private set; }

    public List<KensaMstModel> ChildKensaMsts { get; private set; }

    public object ParentKensaMst { get; private set; }

    public string CenterName { get; private set; }

    public int SeqNo
    {
        get { return KensaItemSeqNo; }
    }

    public string ItemCd
    {
        get { return KensaItemCd; }
    }

    public string DisplaySeqNo => string.IsNullOrEmpty(ItemCd) ? string.Empty : SeqNo.AsString();

    public string ItemName
    {
        get { return KensaName; }
    }

    public int IsDeleted { get; private set; }
}
