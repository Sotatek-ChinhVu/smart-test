using Domain.Models.PatientInfor;
using Helper.Common;
using System.Text.Json.Serialization;

namespace EmrCloudApi.Responses.MedicalExamination;

public class Karte2HeaderModel
{
    public Karte2HeaderModel(PatientInforModel patientInfo, int startDate, int endDate)
    {
        HpId = patientInfo.HpId;
        PtId = patientInfo.PtId;
        KanaName = patientInfo.KanaName;
        Name = patientInfo.Name;
        Sex = patientInfo.Sex == 1 ? "（男）" : "（女）";
        Birthday = CIUtil.SDateToShowWDate2(patientInfo.Birthday);
        CurrentTime = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm");
        StartDate = CIUtil.SDateToShowSDate2(startDate);
        EndDate = CIUtil.SDateToShowSDate2(endDate);
    }

    [JsonPropertyName("hpId")]
    public int HpId { get; set; }

    [JsonPropertyName("ptId")]
    public long PtId { get; set; }

    [JsonPropertyName("kanaName")]
    public string KanaName { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("sex")]
    public string Sex { get; set; }

    [JsonPropertyName("birthday")]
    public string Birthday { get; set; }

    [JsonPropertyName("currentTime")]
    public string CurrentTime { get; set; }

    [JsonPropertyName("startDate")]
    public string StartDate { get; set; }

    [JsonPropertyName("endDate")]
    public string EndDate { get; set; }
}
