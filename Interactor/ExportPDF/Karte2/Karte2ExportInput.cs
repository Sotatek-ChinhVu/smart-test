namespace Interactor.ExportPDF.Karte2;

public class Karte2ExportInput
{
    public Karte2ExportInput(long ptId, int hpId, int userId, int sinDate, long raiinNo, bool emptyMode, string fDisp, int fDojitu, int fSijiType, int startDate, int endDate, bool isCheckedHoken, bool isCheckedJihi, bool isCheckedHokenJihi, bool isCheckedJihiRece, bool isCheckedHokenRousai, bool isCheckedHokenJibai, bool isCheckedDoctor, bool isCheckedStartTime, bool isCheckedVisitingTime, bool isCheckedEndTime, bool isUketsukeNameChecked, bool isCheckedSyosai, bool isIncludeTempSave, bool isCheckedApproved, bool isCheckedInputDate, bool isCheckedSetName, int deletedOdrVisibilitySetting, bool isIppanNameChecked, bool isCheckedHideOrder, bool chkDummy, bool chk_Gairaikanri, bool chkIppan, bool chkPrtDate, int raiinTermDelKbn)
    {
        PtId = ptId;
        HpId = hpId;
        UserId = userId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        EmptyMode = emptyMode;
        FDisp = fDisp;
        FDojitu = fDojitu;
        FSijiType = fSijiType;
        StartDate = startDate;
        EndDate = endDate;
        IsCheckedHoken = isCheckedHoken;
        IsCheckedJihi = isCheckedJihi;
        IsCheckedHokenJihi = isCheckedHokenJihi;
        IsCheckedJihiRece = isCheckedJihiRece;
        IsCheckedHokenRousai = isCheckedHokenRousai;
        IsCheckedHokenJibai = isCheckedHokenJibai;
        IsCheckedDoctor = isCheckedDoctor;
        IsCheckedStartTime = isCheckedStartTime;
        IsCheckedVisitingTime = isCheckedVisitingTime;
        IsCheckedEndTime = isCheckedEndTime;
        IsUketsukeNameChecked = isUketsukeNameChecked;
        IsCheckedSyosai = isCheckedSyosai;
        IsIncludeTempSave = isIncludeTempSave;
        IsCheckedApproved = isCheckedApproved;
        IsCheckedInputDate = isCheckedInputDate;
        IsCheckedSetName = isCheckedSetName;
        DeletedOdrVisibilitySetting = deletedOdrVisibilitySetting;
        IsIppanNameChecked = isIppanNameChecked;
        IsCheckedHideOrder = isCheckedHideOrder;
        ChkDummy = chkDummy;
        Chk_Gairaikanri = chk_Gairaikanri;
        ChkIppan = chkIppan;
        ChkPrtDate = chkPrtDate;
        RaiinTermDelKbn = raiinTermDelKbn;
    }

    public long PtId { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public bool EmptyMode { get; private set; }


    //FormInfo
    public string FDisp { get; private set; }

    public int FDojitu { get; private set; }

    public int FSijiType { get; private set; }


    //KanInfo
    public int StartDate { get; private set; }

    public int EndDate { get; private set; }


    //Hoken
    public bool IsCheckedHoken { get; private set; }

    public bool IsCheckedJihi { get; private set; }

    public bool IsCheckedHokenJihi { get; private set; }

    public bool IsCheckedJihiRece { get; private set; }

    public bool IsCheckedHokenRousai { get; private set; }

    public bool IsCheckedHokenJibai { get; private set; }

    //Raiin
    public bool IsCheckedDoctor { get; private set; }

    public bool IsCheckedStartTime { get; private set; }

    public bool IsCheckedVisitingTime { get; private set; }

    public bool IsCheckedEndTime { get; private set; }

    public bool IsUketsukeNameChecked { get; private set; }

    public bool IsCheckedSyosai { get; private set; }

    public bool IsIncludeTempSave { get; private set; }

    public bool IsCheckedApproved { get; private set; }


    //Rp
    public bool IsCheckedInputDate { get; private set; }

    public bool IsCheckedSetName { get; private set; }

    public int DeletedOdrVisibilitySetting { get; private set; }


    //Karute
    public bool IsIppanNameChecked { get; private set; }


    //Order
    public bool IsCheckedHideOrder { get; private set; }


    //Other
    public bool ChkDummy { get; private set; }

    public bool Chk_Gairaikanri { get; private set; }

    public bool ChkIppan { get; private set; }

    public bool ChkPrtDate { get; private set; }

    public int RaiinTermDelKbn { get; private set; }
}
