using Domain.Common;

namespace Domain.Models.DrugInfor;

public interface IDrugInforRepository : IRepositoryBase
{
    DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd);

    List<SinrekiFilterMstModel> GetSinrekiFilterMstList(int hpId);

    SinrekiFilterMstModel GetSinrekiFilterMst(int hpId, int grpCd);

    bool SaveSinrekiFilterMstList(int hpId, int userId, List<SinrekiFilterMstModel> sinrekiFilterMstList);

    bool CheckExistGrpCd(int hpId, List<int> grpCdList);

    bool CheckExistKouiKbn(int hpId, List<int> kouiKbnIdList);

    bool CheckExistSinrekiFilterMstKoui(int hpId, List<long> kouiSeqNoList);

    bool CheckExistSinrekiFilterMstDetail(int hpId, List<long> detailIdList);

    List<DrugUsageHistoryModel> GetDrugUsageHistoryList(int hpId, long ptId);
}
