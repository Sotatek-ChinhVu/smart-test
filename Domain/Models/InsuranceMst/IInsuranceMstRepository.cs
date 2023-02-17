using Domain.Common;

namespace Domain.Models.InsuranceMst
{
    public interface IInsuranceMstRepository : IRepositoryBase
    {
        InsuranceMstModel GetDataInsuranceMst(int hpId, long ptId, int sinDate);

        IEnumerable<HokensyaMstModel> SearchListDataHokensyaMst(int hpId, int sinDate, string keyword);

        HokenMstModel GetHokenMstByFutansyaNo(int hpId, int sinDate, string futansyaNo);

        bool SaveHokenSyaMst(HokensyaMstModel model, int userId);

        HokensyaMstModel FindHokenSyaMstInf(int hpId, string hokensyaNo, int hokenKbn, string houbetuNo, string hokensyaNoSearch);

        List<InsuranceMasterDetailModel> GetInsuranceMasterDetails(int hpId, int FHokenNo, int FHokenSbtKbn, bool IsJitan, bool IsTaken);

        List<SelectMaintenanceModel> GetSelectMaintenance(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int startDate);

        bool DeleteHokenMaster(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int startDate);

        (int sortNo, int hokenEdaNo) GetInfoCloneInsuranceMst(int hpId, int hokenNo, int prefNo, int startDate);
    }
}
