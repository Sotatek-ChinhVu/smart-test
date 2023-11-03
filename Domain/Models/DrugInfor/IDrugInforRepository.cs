using Domain.Common;

namespace Domain.Models.DrugInfor;

public interface IDrugInforRepository : IRepositoryBase
{
    DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd);

    List<SinrekiFilterMstModel> GetSinrekiFilterMstList(int hpId, int sinDate);
}
