namespace EmrCloudApi.Tenant.Requests.MedicalExamination
{
    public class Karte2ExportRequest
    {
        public long PtId { get;  set; }
        public int HpId { get;  set; }
        public int UserId { get;  set; }
        public int SinDate { get;  set; } = 0;
        public long RaiinNo { get;  set; } = 0;
        public bool EmptyMode { get;  set; } = false;

        //FormInfo
        public string FDisp { get;  set; } = string.Empty;
        public int FDojitu { get;  set; } = 0;
        public int FSijiType { get;  set; } = -1;

        //KanInfo
        public int StartDate { get;  set; } = 0;
        public int EndDate { get;  set; } = 0;

        //Hoken
        public bool IsCheckedHoken { get;  set; } = true;
        public bool IsCheckedJihi { get;  set; } = true;
        public bool IsCheckedHokenJihi { get;  set; } = true;
        public bool IsCheckedJihiRece { get;  set; } = true;
        public bool IsCheckedHokenRousai { get;  set; } = true;
        public bool IsCheckedHokenJibai { get;  set; } = true;

        //Raiin
        public bool IsCheckedDoctor { get;  set; } = true;
        public bool IsCheckedStartTime { get;  set; } = true;
        public bool IsCheckedVisitingTime { get;  set; } = true;
        public bool IsCheckedEndTime { get;  set; } = true;
        public bool IsUketsukeNameChecked { get;  set; } = true;
        public bool IsCheckedSyosai { get;  set; } = true;
        public bool IsIncludeTempSave { get;  set; } = true;
        public bool IsCheckedApproved { get;  set; } = true;

        //Rp
        public bool IsCheckedInputDate { get;  set; } = true;
        public bool IsCheckedSetName { get;  set; } = true;
        public int DeletedOdrVisibilitySetting { get;  set; } = 1;

        //Karute
        public bool IsIppanNameChecked { get;  set; } = true;

        //Order
        public bool IsCheckedHideOrder { get;  set; } = false;

        //Other
        public bool ChkDummy { get;  set; } = false;
        public bool Chk_Gairaikanri { get;  set; } = false;
        public bool ChkIppan { get;  set; } = false;
        public bool ChkPrtDate { get;  set; } = false;
        public int RaiinTermDelKbn { get;  set; } = 1;
    } 
}
