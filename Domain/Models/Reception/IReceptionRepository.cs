﻿using Domain.Common;
using System.Runtime.InteropServices;

namespace Domain.Models.Reception
{
    public interface IReceptionRepository : IRepositoryBase
    {
        long Insert(ReceptionSaveDto dto, int hpId, int userId);

        bool Update(ReceptionSaveDto dto, int hpId, int userId);

        ReceptionModel Get(long raiinNo, bool flag = false);

        List<ReceptionRowModel> GetList(int hpId, int sinDate, long raiinNo, long ptId, [Optional] bool isGetAccountDue, [Optional] bool isGetFamily, int isDeleted = 2, bool searchSameVisit = false);

        IEnumerable<ReceptionModel> GetList(int hpId, long ptId, int karteDeleteHistory);

        ReceptionModel GetReceptionComments(int hpId, long raiinNo);

        ReceptionModel GetReceptionVisiting(int hpId, long raiinNo);

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

        int GetMaxUketukeNo(int hpId, int sindate, int infKbn, int kaId, int uketukeMode);

        long InitDoctorCombobox(int userId, int tantoId, long ptId, int hpId, int sinDate);

        bool CheckExistRaiinNo(int hpId, long ptId, long raiinNo);

        List<ReceptionModel> GetListRaiinInf(int hpId, long ptId, int pageIndex, int pageSize, int isDeleted, bool isAll = false);

        ReceptionModel? GetLastKarute(int hpId, long ptNum);

        List<Tuple<int, long, long>> Delete(bool flag, int hpId, long ptId, int userId, int sinDate, List<Tuple<long, long, int>> receptions);

        bool CheckExistOfRaiinNos(List<long> raininNos);

        List<ReceptionModel> GetRaiinListWithKanInf(int hpId, long ptId);

        ReceptionModel GetLastVisit(int hpId, long ptId, int sinDate);

        List<SameVisitModel> GetListSameVisit(int hpId, long ptId, int sinDate);

        bool UpdateIsDeleted(int hpId, long raiinNo);

        List<RaiinInfToPrintModel> GetOutDrugOrderList(int hpId, int fromDate, int toDate);

        int GetStatusRaiinInf(int hpId, long raiinNo, long ptId);
    }
}
