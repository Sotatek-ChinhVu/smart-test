using Domain.Models.Yousiki;

namespace EmrCloudApi.Responses.Yousiki.Dto;

public class VisitingInfDto
{
    public VisitingInfDto(VisitingInfModel model)
    {
        SinDate = model.SinDate;
        UketukeTime = model.UketukeTime;
        RaiinNo = model.RaiinNo;
        Status = model.Status;
        KaName = model.KaName;
        DoctorName = model.DoctorName;
        SyosaisinKbn = model.SyosaisinKbn;
        UketukeSbtName = model.UketukeSbtName;
        PtComment = model.PtComment;
        YousikiKaCd = model.YousikiKaCd;
        RaiinListInfList = model.RaiinListInfList.Select(item => new RaiinListInfDto(item)).ToList();
    }

    public int SinDate { get; private set; }

    public string UketukeTime { get; private set; }

    public long RaiinNo { get; private set; }

    public int Status { get; private set; }

    public string KaName { get; private set; }

    public string DoctorName { get; private set; }

    public int SyosaisinKbn { get; private set; }

    public string UketukeSbtName { get; private set; }

    public string PtComment { get; private set; }

    public string YousikiKaCd { get; private set; }

    public List<RaiinListInfDto> RaiinListInfList { get; private set; }
}
