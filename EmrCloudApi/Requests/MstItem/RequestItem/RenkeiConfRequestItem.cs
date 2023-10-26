using Domain.Models.MstItem;

namespace EmrCloudApi.Requests.MstItem.RequestItem;

public class RenkeiConfRequestItem
{
    public long Id { get; set; }

    public int RenkeiId { get; set; }

    public string Param { get; set; } = string.Empty;

    public int PtNumLength { get; set; }

    public int TemplateId { get; set; }

    public int IsInvalid { get; set; }

    public string Biko { get; set; } = string.Empty;

    public int SortNo { get; set; }

    public bool IsDeleted { get; set; }

    public List<RenkeiPathConfRequestItem> RenkeiPathConfModelList { get; set; } = new();

    public List<RenkeiTimingRequestItem> RenkeiTimingModelList { get; set; } = new();
}
