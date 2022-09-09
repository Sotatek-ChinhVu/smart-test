namespace Domain.Models.SuperSetDetail;

public interface ISuperSetDetailRepository
{
    List<SetByomeiModel> GetSetByoMeis(int hpId, int setCd);
}