namespace Domain.Models.SeikaturekiInf;

public interface ISeikaturekiInfRepository
{
    List<SeikaturekiInfModel> GetList(long ptId, int hpId);
}
