using Domain.Common;

namespace Domain.Models.Yousiki;

public interface IYousikiRepository : IRepositoryBase
{
    List<Yousiki1InfModel> GetYousiki1InfModelWithCommonInf(int hpId, int sinYm, long ptNum, int dataTypes, int status = -1);

    List<Yousiki1InfDetailModel> GetYousiki1InfDetails(int hpId, int sinYm, long ptId, int dataType, int seqNo);
}
