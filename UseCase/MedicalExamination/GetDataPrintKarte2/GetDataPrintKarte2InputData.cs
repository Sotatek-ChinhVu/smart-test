using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetHistory;

namespace UseCase.MedicalExamination.GetDataPrintKarte2;

public class GetDataPrintKarte2InputData : IInputData<GetMedicalExaminationHistoryOutputData>
{
    public GetDataPrintKarte2InputData(long ptId, int hpId, int sinDate, int startDate, int endDate, bool isCheckedHoken, bool isCheckedJihi, bool isCheckedHokenJihi, bool isCheckedJihiRece, bool isCheckedHokenRousai, bool isCheckedHokenJibai, bool isCheckedDoctor, bool isCheckedStartTime, bool isCheckedVisitingTime, bool isCheckedEndTime, bool isUketsukeNameChecked, bool isCheckedSyosai, bool isIncludeTempSave, bool isCheckedApproved, bool isCheckedInputDate, bool isCheckedSetName, int deletedOdrVisibilitySetting, bool isIppanNameChecked, bool isCheckedHideOrder)
    {
        PtId = ptId;
        HpId = hpId;
        SinDate = sinDate;
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
    }

    public long PtId { get; private set; }
    public int HpId { get; private set; }
    public int SinDate { get; private set; }

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
}
