using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace EmrCloudApi.Responses.MedicalExamination;

public class GetDataPrintKarte2Response
{
    public GetDataPrintKarte2Response(List<HistoryKarteOdrRaiinItem>? karteOrdRaiins, GetDataPrintKarte2InputData input)
    {
        KarteOrdRaiins = karteOrdRaiins;
        DisplaySetting = new DisplaySetting(input);
    }

    public List<HistoryKarteOdrRaiinItem>? KarteOrdRaiins { get; private set; }

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


    //Karute
    public bool IsIppanNameChecked { get; private set; }


    //Order
    public bool IsCheckedHideOrder { get; private set; }
}