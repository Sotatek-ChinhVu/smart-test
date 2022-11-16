using System.Runtime.InteropServices;

namespace Domain.Models.Reception
{
    public interface IReceptionRepository
    {
        long Insert(ReceptionSaveDto dto, int hpId, int userId);

        bool Update(ReceptionSaveDto dto, int hpId, int userId);

        ReceptionModel Get(long raiinNo);

        List<ReceptionRowModel> GetList(int hpId, int sinDate, long raiinNo, long ptId, [Optional] bool isGetAccountDue);

        ReceptionModel GetReceptionComments(int hpId, long raiinNo);

        ReceptionModel GetReceptionVisiting(int hpId, long raiinNo);

        IEnumerable<ReceptionModel> GetList(int hpId, long ptId, int karteDeleteHistory);

        List<ReceptionModel> GetLastRaiinInfs(int hpId, long ptId, int sinDate);

        bool UpdateStatus(int hpId, long raiinNo, int status, int userId);

        bool UpdateUketukeNo(int hpId, long raiinNo, int uketukeNo, int userId);

        bool UpdateUketukeTime(int hpId, long raiinNo, string uketukeTime, int userId);

        bool UpdateSinStartTime(int hpId, long raiinNo, string sinStartTime, int userId);

        bool UpdateUketukeSbt(int hpId, long raiinNo, int uketukeSbt, int userId);

        bool UpdateTantoId(int hpId, long raiinNo, int tantoId, int userId);

        bool UpdateKaId(int hpId, long raiinNo, int kaId, int userId);

        bool CheckListNo(List<long> raininNos);

        bool CheckExistReception(int hpId, long ptId, int sinDate, long raiinNo);

        int GetFirstVisitWithSyosin(int hpId, long ptId, int sinDate);

        ReceptionModel GetDataDefaultReception(int hpId, int ptId, int sinDate, int defaultSettingDoctor);
    }
}
