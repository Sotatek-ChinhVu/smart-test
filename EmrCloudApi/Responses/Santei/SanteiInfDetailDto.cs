using UseCase.Santei.GetListSanteiInf;

namespace EmrCloudApi.Responses.Santei;

public class SanteiInfDetailDto
{
    public SanteiInfDetailDto(SanteiInfDetailOutputItem detailModel)
    {
        Id = detailModel.Id;
        KisanSbt = detailModel.KisanSbt;
        KisanDate = detailModel.KisanDate;
        Byomei = detailModel.Byomei;
        HosokuComment = detailModel.HosokuComment;
        EndDate = detailModel.EndDate;
        Comment = detailModel.Comment;
        Comment = detailModel.Comment;
    }

    public long Id { get; private set; }

    public int KisanSbt { get; private set; }

    public int KisanDate { get; private set; }

    public string Byomei { get; private set; }

    public string HosokuComment { get; private set; }

    public int EndDate { get; private set; }

    public string Comment { get; private set; }
}
