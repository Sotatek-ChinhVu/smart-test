using Domain.Models.InsuranceMst;
using Domain.Models.MstItem;
using Domain.Models.Receipt;
using Domain.Models.Receipt.Recalculation;
using Domain.Models.SystemConf;

namespace Infrastructure.Converter;

public class RecalculationConverter
{
    #region Get data list to for loop
    public class ForLoop
    {
        public ForLoop(List<ReceCheckOptModel> receCheckOptList, List<ReceRecalculationModel> receRecalculationList, List<ReceCheckErrModel> allReceCheckErrList, List<SystemConfModel> allSystemConfigList, List<SyobyoKeikaModel> allSyobyoKeikaList, List<IsKantokuCdValidModel> allIsKantokuCdValidList)
        {
            ReceCheckOptList = receCheckOptList;
            ReceRecalculationList = receRecalculationList;
            AllReceCheckErrList = allReceCheckErrList;
            AllSystemConfigList = allSystemConfigList;
            AllSyobyoKeikaList = allSyobyoKeikaList;
            AllIsKantokuCdValidList = allIsKantokuCdValidList;
        }

        public List<ReceCheckOptModel> ReceCheckOptList { get; private set; }

        public List<ReceRecalculationModel> ReceRecalculationList { get; private set; }

        public List<ReceCheckErrModel> AllReceCheckErrList { get; private set; }

        public List<SystemConfModel> AllSystemConfigList { get; private set; }

        public List<SyobyoKeikaModel> AllSyobyoKeikaList { get; private set; }

        public List<IsKantokuCdValidModel> AllIsKantokuCdValidList { get; private set; }
    }
    #endregion

    #region Get list data inside for loop
    public class InsideLoop
    {
        public InsideLoop(List<ReceSinKouiCountModel> sinKouiCountList, List<TenItemModel> tenMstByItemCdList, List<string> itemCdList)
        {
            SinKouiCountList = sinKouiCountList;
            TenMstByItemCdList = tenMstByItemCdList;
            ItemCdList = itemCdList;
        }

        public List<ReceSinKouiCountModel> SinKouiCountList { get; private set; }

        public List<TenItemModel> TenMstByItemCdList { get; private set; }

        public List<string> ItemCdList { get; private set; }
    }
    #endregion
}
