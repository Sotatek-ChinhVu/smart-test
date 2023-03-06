using Domain.Models.InsuranceMst;
using Domain.Models.MstItem;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.SystemConf;
using Infrastructure.Converter;
using System.Text;

namespace Infrastructure.Interfaces;

public interface IRecalculationService
{
    RecalculationConverter.ForLoop GetDataForLoop(int hpId, int sinYm, List<long> ptIdList);

    RecalculationConverter.InsideLoop GetDataInsideLoop(int hpId, int sinYm, long ptId, int hokenId);

    (List<ReceCheckErrModel>, StringBuilder) CheckError(int hpId, int sinYm, ReceRecalculationModel recalculationItem, List<ReceCheckOptModel> receCheckOptList, List<ReceRecalculationModel> receRecalculationList, List<ReceCheckErrModel> allReceCheckErrList, List<SystemConfModel> allSystemConfigList, List<SyobyoKeikaModel> allSyobyoKeikaList, List<IsKantokuCdValidModel> allIsKantokuCdValidList, List<ReceSinKouiCountModel> sinKouiCountList, List<TenItemModel> tenMstByItemCdList, List<string> itemCdList);

    bool SaveReceCheckErrList(int hpId, int userId, List<ReceCheckErrModel> newReceCheckErrList);

    void ReleaseResource();
}
