using Domain.Models.KarteKbnMst;
using UseCase.Core.Sync.Core;

namespace UseCase.MedicalExamination.Karte2Print
{
    public class Karte2ExportInputData :  IInputData<Karte2ExportOutputData>
    {
        public Karte2ExportInputData(long ptId, int hpId, int userId, int sinDate, long raiinNo, bool emptyMode, string fDisp, int fDojitu, int fSijiType, int startDate, int endDate, bool isCheckedHoken, bool isCheckedJihi, bool isCheckedHokenJihi, bool isCheckedJihiRece, bool isCheckedHokenRousai, bool isCheckedHokenJibai, bool isCheckedDoctor, bool isCheckedStartTime, bool isCheckedVisitingTime, bool isCheckedEndTime, bool isUketsukeNameChecked, bool isCheckedSyosai, bool isIncludeTempSave, bool isCheckedApproved, bool isCheckedInputDate, bool isCheckedSetName, int deletedOdrVisibilitySetting, bool isIppanNameChecked, bool isCheckedHideOrder, bool chkDummy, bool chk_Gairaikanri, bool chkIppan, bool chkPrtDate, int raiinTermDelKbn)
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
        public int SinDate { get; private set; } = 0;
        public long RaiinNo { get; private set; } = 0;

        public bool EmptyMode { get; private set; } = false;

        //FormInfo
        public string FDisp { get; private set; } = string.Empty;
        public int FDojitu { get; private set; } = 0;
        public int FSijiType { get; private set; } = -1;

        //KanInfo
        public int StartDate { get; private set; } = 0;
        public int EndDate { get; private set; } = 0;

        //Hoken
        public bool IsCheckedHoken { get; private set; } = true;

        public bool IsCheckedJihi { get; private set; } = true;

        public bool IsCheckedHokenJihi { get; private set; } = true;

        public bool IsCheckedJihiRece { get; private set; } = true;

        public bool IsCheckedHokenRousai { get; private set; } = true;

        public bool IsCheckedHokenJibai { get; private set; } = true;

        //Raiin
        public bool IsCheckedDoctor { get; private set; } = true;


        public bool IsCheckedStartTime { get; private set; } = true;


        public bool IsCheckedVisitingTime { get; private set; } = true;


        public bool IsCheckedEndTime { get; private set; } = true;


        public bool IsUketsukeNameChecked { get; private set; } = true;


        public bool IsCheckedSyosai { get; private set; } = true;


        public bool IsIncludeTempSave { get; private set; } = true;


        public bool IsCheckedApproved { get; private set; } = true;

        //Rp
        public bool IsCheckedInputDate { get; private set; } = true;

        public bool IsCheckedSetName { get; private set; } = true;

        public int DeletedOdrVisibilitySetting { get; private set; } = 1;

        //Karute
        public bool IsIppanNameChecked { get; private set; } = true;

        //Order
        public bool IsCheckedHideOrder { get; private set; } = false;

        //Other
        public bool ChkDummy { get; private set; } = false;

        public bool Chk_Gairaikanri { get; private set; } = false;

        public bool ChkIppan { get; private set; } = false;

        public bool ChkPrtDate { get; private set; } = false;

        public int RaiinTermDelKbn { get; private set; } = 1;
    }
}
