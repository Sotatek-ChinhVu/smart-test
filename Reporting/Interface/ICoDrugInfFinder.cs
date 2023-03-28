using Entity.Tenant;
using Reporting.DrugInfo.Model;

namespace Reporting.Interface
{
    public interface ICoDrugInfFinder
    {
        PathConf GetPathConf(int grpCode);
        DrugInfoModel GetBasicInfo(long ptId, int orderDate = 0);
        List<OrderInfoModel> GetOrderByRaiinNo(long raiinNo);
        string GetYJCode(string ItemCd);
        List<SingleDosageMstModel> GetSingleDosageMstCollection(int hpId, string unitName);
        TenMstModel GetTenMstModel(string ItemCd);
        List<PiImage> GetProductImages(int hpId, string itemCd);
        List<DrugInf> GetDrugInfo(int hpId, string itemCd, int age, int gender);

    }
}
