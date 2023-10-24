using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MstItem.Dto;

public class RenkeiPathConfDto
{
    public RenkeiPathConfDto(RenkeiPathConfModel model)
    {
        Id = model.Id;
        RenkeiId = model.RenkeiId;
        SeqNo = model.SeqNo;
        EdaNo = model.EdaNo;
        Path = model.Path;
        Machine = model.Machine;
        CharCd = model.CharCd;
        WorkPath = model.WorkPath;
        Interval = model.Interval;
        Param = model.Param;
        User = model.User;
        PassWord = model.PassWord;
        IsInvalid = model.IsInvalid;
        Biko = model.Biko;
        IsDeleted = model.IsDeleted;
        PassWordDisplay = model.PassWordDisplay;
        CharCdName = model.CharCdName;
    }

    public long Id { get; private set; }

    public int RenkeiId { get; private set; }

    public int SeqNo { get; private set; }

    public int EdaNo { get; private set; }

    public string Path { get; private set; }

    public string Machine { get; private set; }

    public int CharCd { get; private set; }

    public string WorkPath { get; private set; }

    public int Interval { get; private set; }

    public string Param { get; private set; }

    public string User { get; private set; }

    public string PassWord { get; private set; }

    public int IsInvalid { get; private set; }

    public string Biko { get; private set; }

    public bool IsDeleted { get; private set; }

    public string PassWordDisplay { get; private set; }

    public string CharCdName { get; private set; }
}
