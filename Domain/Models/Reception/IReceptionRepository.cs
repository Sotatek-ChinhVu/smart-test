namespace Domain.Models.Reception
{
    public interface IReceptionRepository
    {
        long Insert(ReceptionSaveDto dto);

        bool Update(ReceptionSaveDto dto);

        ReceptionModel Get(long raiinNo);

        List<ReceptionRowModel> GetList(int hpId, int sinDate, long raiinNo, long ptId);

        ReceptionModel GetReceptionComments(int hpId, long raiinNo);
        
        ReceptionModel GetReceptionVisiting(int hpId, long raiinNo);

        IEnumerable<ReceptionModel> GetList(int hpId, long ptId, int karteDeleteHistory);

        bool UpdateStatus(int hpId, long raiinNo, int status);

        bool UpdateUketukeNo(int hpId, long raiinNo, int uketukeNo);

        bool UpdateUketukeTime(int hpId, long raiinNo, string uketukeTime);

        bool UpdateSinStartTime(int hpId, long raiinNo, string sinStartTime);

        bool UpdateUketukeSbt(int hpId, long raiinNo, int uketukeSbt);

        bool UpdateTantoId(int hpId, long raiinNo, int tantoId);

        bool UpdateKaId(int hpId, long raiinNo, int kaId);

        bool CheckListNo(List<long> raininNos);

        bool CheckExistReception(int hpId, long ptId, int sinDate, long raiinNo);

        ReceptionModel GetDataDefaultReception(int hpId, int ptId, int sinDate, int defaultSettingDoctor);
    }
}
