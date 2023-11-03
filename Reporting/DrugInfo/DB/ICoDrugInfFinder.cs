using Domain.Common;
using Entity.Tenant;
using Reporting.DrugInfo.Model;

namespace Reporting.DrugInfo.DB;

public interface ICoDrugInfFinder : IRepositoryBase
{
    PathConf GetPathConf(int grpCode);

    DrugInfoModel GetBasicInfo(int hpId, long ptId, int orderDate = 0);

    List<OrderInfoModel> GetOrderByRaiinNo(long raiinNo);

    string GetYJCode(string itemCd);

    List<SingleDosageMstModel> GetSingleDosageMstCollection(int hpId, string unitName);

    TenMstModel GetTenMstModel(string itemCd);

    List<PiImage> GetProductImages(int hpId, string itemCd);

    List<DrugInf> GetDrugInfo(int hpId, string itemCd, int age, int gender);

    PathPicture GetDefaultPathPicture();
}
