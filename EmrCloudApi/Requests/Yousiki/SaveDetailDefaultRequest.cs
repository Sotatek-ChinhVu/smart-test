using EmrCloudApi.Requests.Yousiki.RequestItem;

namespace EmrCloudApi.Requests.Yousiki;

public class SaveDetailDefaultRequest
{
    /// <summary>
    /// 1 - BarthelIndexList
    /// 2 - FIMList
    /// 3 - Yousiki1InfDetailList
    /// </summary>
    public int Mode { get; set; }

    public int DataType { get; set; }

    public List<PatientStatusRequest> BarthelIndexList { get; set; } = new();

    public List<PatientStatusRequest> FIMList { get; set; } = new();

    public List<Yousiki1InfDetailRequest> Yousiki1InfDetailList { get; set; } = new();
}
