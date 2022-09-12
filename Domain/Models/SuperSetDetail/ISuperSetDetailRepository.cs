namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository
{
    List<SetByomeiModel> GetSetByomeiList(int hpId, int setCd);
}