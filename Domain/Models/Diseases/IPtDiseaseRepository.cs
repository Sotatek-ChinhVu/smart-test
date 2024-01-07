using Domain.Common;
using Domain.Enum;
using Domain.Models.ListSetMst;

namespace Domain.Models.Diseases
{
    public interface IPtDiseaseRepository : IRepositoryBase
    {
        List<PtDiseaseModel> GetPatientDiseaseList(int hpId, long ptId, int sinDate, int hokenId, DiseaseViewType openFrom, bool isContiFiltered, bool isInMonthFiltered);

        List<PtDiseaseModel> GetListPatientDiseaseForReport(int hpId, long ptId, int hokenPid, int sinDate, bool tenkiByomei);

        List<PtDiseaseModel> GetByomeiInThisMonth(int hpId, int sinYm, long ptId, int hokenId);

        List<PtDiseaseModel> GetPtByomeisByHokenId(int hpId, long ptId, int hokenId);

        List<long> Upsert(List<PtDiseaseModel> inputDatas, int hpId, int userId);

        List<ByomeiSetMstModel> GetDataTreeSetByomei(int hpId, int sinDate);

        List<PtDiseaseModel> GetTekiouByomeiByOrder(int hpId, List<string> itemCds);

        List<PtDiseaseModel> GetAllByomeiByPtId(int hpId, long ptId, int pageIndex, int pageSize);

        bool UpdateByomeiSetMst(int userId, int hpId, List<ByomeiSetMstUpdateModel> listData);

        Dictionary<string, string> GetByomeiMst(int hpId, List<string> byomeiCds);

        bool IsHokenInfInUsed(int hpId, long ptId, int hokenId);
    }
}
