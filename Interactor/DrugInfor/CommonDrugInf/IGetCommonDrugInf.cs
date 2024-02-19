using Domain.Models.DrugInfor;

namespace Interactor.DrugInfor.CommonDrugInf;

public interface IGetCommonDrugInf
{
    DrugInforModel GetDrugInforModel(int hpId, int sinDate, string itemCd);

    string ShowProductInf(int hpId, int sinDate, string itemCd, int level, string drugName, string yJCode);

    string ShowKanjaMuke(int hpId, string itemCd, int level, string drugName, string yJCode);

    string ShowMdbByomei(int hpId, string itemCd, int level, string drugName, string yJCode);

    void ReleaseResources();
}
