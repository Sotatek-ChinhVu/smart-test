using System.Text.Json.Serialization;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Responses.MedicalExamination;

public class GetDataPrintKarte2Response
{
    public GetDataPrintKarte2Response(List<HistoryKarteOdrRaiinItem> karteOrdRaiins, GetDataPrintKarte2InputData input)
    {
        KarteOrdRaiins = karteOrdRaiins;
        DisplaySetting = new DisplaySetting(input);
    }

    [JsonPropertyName("karteOrdRaiins")]
    public List<HistoryKarteOdrRaiinItem> KarteOrdRaiins { get; private set; }

    [JsonPropertyName("displaySetting")]
    public DisplaySetting DisplaySetting { get; private set; }
}

public class DisplaySetting
{
    public DisplaySetting(GetDataPrintKarte2InputData input)
    {
        IsCheckedDoctor = input.IsCheckedDoctor;
        IsCheckedStartTime = input.IsCheckedStartTime;
        IsCheckedVisitingTime = input.IsCheckedVisitingTime;
        IsCheckedEndTime = input.IsCheckedEndTime;
        IsUketsukeNameChecked = input.IsUketsukeNameChecked;
        IsCheckedSyosai = input.IsCheckedSyosai;
        IsIncludeTempSave = input.IsIncludeTempSave;
        IsCheckedApproved = input.IsCheckedApproved;
        IsCheckedInputDate = input.IsCheckedInputDate;
        IsCheckedSetName = input.IsCheckedSetName;
        IsIppanNameChecked = input.IsIppanNameChecked;
        IsCheckedHideOrder = input.IsCheckedHideOrder;
    }

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


    //Karute
    [JsonPropertyName("isIppanNameChecked")]
    public bool IsIppanNameChecked { get; private set; }


    //Order
    [JsonPropertyName("isCheckedHideOrder")]
    public bool IsCheckedHideOrder { get; private set; }
}