using Domain.Common;

namespace Domain.Models.Yousiki;

public interface IYousikiRepository : IRepositoryBase
{
    List<Yousiki1InfModel> GetYousiki1InfModelWithCommonInf(int hpId, int sinYm, long ptNum, int dataType, int status = -1);

    List<Yousiki1InfDetailModel> GetYousiki1InfDetails(int hpId, int sinYm, long ptId, int dataType, int seqNo);

    List<VisitingInfModel> GetVisitingInfs(int hpId, long ptId, int sinYm);

    bool IsYousikiExist(int hpId, int sinYm, long ptId);

    bool IsYousikiExist(int hpId, int sinYm, long ptId, int dataType);

    List<long> GetListPtIdHealthInsuranceAccepted(int hpId, int sinYm, long ptId, int dataType);

    List<Yousiki1InfModel> GetHistoryYousiki(int hpId, int sinYm, long ptId, int dataType);

    List<Yousiki1InfModel> GetYousiki1InfModel(int hpId, int sinYm, long ptNumber, int dataType);

    Dictionary<string, string> GetKacodeYousikiMstDict(int hpId);

    bool AddYousikiInfByMonth(int hpId, int userId, int sinYm, int dataType, List<long> ptIdList);

    bool DeleteYousikiInf(int hpId, int userId, int sinYm, long ptId, int dataType);

    List<Yousiki1InfModel> GetListYousiki1Inf(int hpId, int sinYm, int status = -1);
}
