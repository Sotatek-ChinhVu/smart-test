using System.Text.Json.Serialization;
using UseCase.Core.Sync.Core;
using UseCase.MedicalExamination.GetHistory;

namespace UseCase.MedicalExamination.GetDataPrintKarte2;

public class GetDataPrintKarte2InputData : IInputData<GetMedicalExaminationHistoryOutputData>
{
    [JsonConstructor]
    public GetDataPrintKarte2InputData(long ptId, int hpId, int sinDate, long raiinNo, int startDate, int endDate, bool isCheckedHoken, bool isCheckedJihi, bool isCheckedHokenJihi, bool isCheckedJihiRece, bool isCheckedHokenRousai, bool isCheckedHokenJibai, bool isCheckedDoctor, bool isCheckedStartTime, bool isCheckedVisitingTime, bool isCheckedEndTime, bool isUketsukeNameChecked, bool isCheckedSyosai, bool isIncludeTempSave, bool isCheckedApproved, bool isCheckedInputDate, bool isCheckedSetName, int deletedOdrVisibilitySetting, bool isIppanNameChecked, bool isCheckedHideOrder, bool emptyMode)
    {
        PtId = ptId;
        HpId = hpId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
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
        EmptyMode = emptyMode;
    }

    public GetDataPrintKarte2InputData()
    {
    }

    [JsonPropertyName("ptId")]
    public long PtId { get; private set; }

    [JsonPropertyName("hpId")]
    public int HpId { get; private set; }

    [JsonPropertyName("sinDate")]
    public int SinDate { get; private set; }

    [JsonPropertyName("raiinNo")]
    public long RaiinNo { get; private set; }

    //KanInfo
    [JsonPropertyName("startDate")]
    public int StartDate { get; private set; }

    [JsonPropertyName("endDate")]
    public int EndDate { get; private set; }


    //Hoken
    [JsonPropertyName("isCheckedHoken")]
    public bool IsCheckedHoken { get; private set; }

    [JsonPropertyName("isCheckedJihi")]
    public bool IsCheckedJihi { get; private set; }

    [JsonPropertyName("isCheckedHokenJihi")]
    public bool IsCheckedHokenJihi { get; private set; }

    [JsonPropertyName("isCheckedJihiRece")]
    public bool IsCheckedJihiRece { get; private set; }

    [JsonPropertyName("isCheckedHokenRousai")]
    public bool IsCheckedHokenRousai { get; private set; }

    [JsonPropertyName("isCheckedHokenJibai")]
    public bool IsCheckedHokenJibai { get; private set; }


    //Raiin
    [JsonPropertyName("isCheckedDoctor")]
    public bool IsCheckedDoctor { get; private set; }

    [JsonPropertyName("isCheckedStartTime")]
    public bool IsCheckedStartTime { get; private set; }

    [JsonPropertyName("isCheckedVisitingTime")]
    public bool IsCheckedVisitingTime { get; private set; }

    [JsonPropertyName("isCheckedEndTime")]
    public bool IsCheckedEndTime { get; private set; }

    [JsonPropertyName("isUketsukeNameChecked")]
    public bool IsUketsukeNameChecked { get; private set; }

    [JsonPropertyName("isCheckedSyosai")]
    public bool IsCheckedSyosai { get; private set; }

    [JsonPropertyName("isIncludeTempSave")]
    public bool IsIncludeTempSave { get; private set; }

    [JsonPropertyName("isCheckedApproved")]
    public bool IsCheckedApproved { get; private set; }


    //Rp
    [JsonPropertyName("isCheckedInputDate")]
    public bool IsCheckedInputDate { get; private set; }

    [JsonPropertyName("isCheckedSetName")]
    public bool IsCheckedSetName { get; private set; }

    [JsonPropertyName("deletedOdrVisibilitySetting")]
    public int DeletedOdrVisibilitySetting { get; private set; }


    //Karute
    [JsonPropertyName("isIppanNameChecked")]
    public bool IsIppanNameChecked { get; private set; }


    //Order
    [JsonPropertyName("isCheckedHideOrder")]
    public bool IsCheckedHideOrder { get; private set; }

    // Orther
    [JsonPropertyName("emptyMode")]
    public bool EmptyMode { get; private set; }
}
