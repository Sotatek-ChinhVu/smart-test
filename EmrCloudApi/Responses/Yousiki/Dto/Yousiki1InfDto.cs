using Domain.Models.Yousiki;

namespace EmrCloudApi.Responses.Yousiki.Dto;

public class Yousiki1InfDto
{
    public Yousiki1InfDto(Yousiki1InfModel model)
    {
        PtNum = model.PtNum;
        Name = model.Name;
        PtId = model.PtId;
        SinYm = model.SinYm;
        DataType = model.DataType;
        Status = model.Status;
        StatusDic = model.StatusDic;
        SeqNo = model.SeqNo;
        CommonList = model.Yousiki1InfDetailList.Where(item => item.DataType == 0).Select(item => new Yousiki1InfDetailDto(item)).ToList();
        LivingHabitList = model.Yousiki1InfDetailList.Where(item => item.DataType == 1).Select(item => new Yousiki1InfDetailDto(item)).ToList();
        AtHomeList = model.Yousiki1InfDetailList.Where(item => item.DataType == 2).Select(item => new Yousiki1InfDetailDto(item)).ToList();
        RehabilitationList = model.Yousiki1InfDetailList.Where(item => item.DataType == 3).Select(item => new Yousiki1InfDetailDto(item)).ToList();
    }

    public long PtNum { get; private set; }

    public string Name { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int DataType { get; private set; }

    public int Status { get; private set; }

    public Dictionary<int, int> StatusDic { get; private set; }

    public int SeqNo { get; private set; }

    public List<Yousiki1InfDetailDto> CommonList { get; private set; }

    public List<Yousiki1InfDetailDto> LivingHabitList { get; private set; }

    public List<Yousiki1InfDetailDto> AtHomeList { get; private set; }

    public List<Yousiki1InfDetailDto> RehabilitationList { get; private set; }
}
