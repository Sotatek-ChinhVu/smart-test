namespace EmrCloudApi.Tenant.Requests.ExportPDF;

public class Karte2ExportRequest
{
    public long PtId { get; set; }

    public int HpId { get; set; }

    public int UserId { get; set; }

    public int SinDate { get; set; }

    public long RaiinNo { get; set; }

    public bool EmptyMode { get; set; }


    //FormInfo
    public string FDisp { get; set; }

    public int FDojitu { get; set; }

    public int FSijiType { get; set; }


    //KanInfo
    public int StartDate { get; set; }

    public int EndDate { get; set; }


    //Hoken
    public bool IsCheckedHoken { get; set; }

    public bool IsCheckedJihi { get; set; }

    public bool IsCheckedHokenJihi { get; set; }

    public bool IsCheckedJihiRece { get; set; }

    public bool IsCheckedHokenRousai { get; set; }

    public bool IsCheckedHokenJibai { get; set; }

    //Raiin
    public bool IsCheckedDoctor { get; set; }

    public bool IsCheckedStartTime { get; set; }

    public bool IsCheckedVisitingTime { get; set; }

    public bool IsCheckedEndTime { get; set; }

    public bool IsUketsukeNameChecked { get; set; }

    public bool IsCheckedSyosai { get; set; }

    public bool IsIncludeTempSave { get; set; }

    public bool IsCheckedApproved { get; set; }


    //Rp
    public bool IsCheckedInputDate { get; set; }

    public bool IsCheckedSetName { get; set; }

    public int DeletedOdrVisibilitySetting { get; set; }


    //Karute
    public bool IsIppanNameChecked { get; set; }


    //Order
    public bool IsCheckedHideOrder { get; set; }


    //Other
    public bool ChkDummy { get; set; }

    public bool Chk_Gairaikanri { get; set; }

    public bool ChkIppan { get; set; }

    public bool ChkPrtDate { get; set; }

    public int RaiinTermDelKbn { get; set; }
}
