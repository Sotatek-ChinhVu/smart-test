using Domain.Models.MstItem;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Linq;
using static Domain.Models.MstItem.SearchOTCModel;

namespace Infrastructure.Repositories
{
    public class MstItemRepository : IMstItemRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        private readonly TenantDataContext _tenantDataContextTracking;
        public MstItemRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<DosageDrugModel> GetDosages(List<string> yjCds)
        {
            var result = _tenantDataContext.DosageDrugs.Where(d => yjCds.Contains(d.YjCd));
            return result == null ? new List<DosageDrugModel>() : result.Select(
                    r => new DosageDrugModel(
                            r.YjCd,
                            r.DoeiCd,
                            r.DgurKbn,
                            r.KikakiUnit,
                            r.YakkaiUnit,
                            r.RikikaRate,
                            r.RikikaUnit,
                            r.YoukaiekiCd
                   )).ToList();
        }

        public SearchOTCModel SearchOTCModels(string searchValue, int pageIndex, int pageSize)
        {
            searchValue = searchValue.Trim();
            var OtcFormCodes = _tenantDataContext.M38OtcFormCode.AsQueryable();
            var OtcMakerCodes = _tenantDataContext.M38OtcMakerCode.AsQueryable();
            var OtcMains = _tenantDataContext.M38OtcMain.AsQueryable();
            var UsageCodes = _tenantDataContext.M56UsageCode.AsQueryable();
            var OtcClassCodes = _tenantDataContext.M38ClassCode.AsQueryable();
            var query = from main in OtcMains
                        join classcode in OtcClassCodes on main.ClassCd equals classcode.ClassCd into classLeft
                        from clas in classLeft.DefaultIfEmpty()
                        join makercode in OtcMakerCodes on main.CompanyCd equals makercode.MakerCd into makerLeft
                        from maker in makerLeft.DefaultIfEmpty()
                        join formcode in OtcFormCodes on main.DrugFormCd equals formcode.FormCd into formLeft
                        from form in formLeft.DefaultIfEmpty()
                        join usagecode in UsageCodes on main.YohoCd equals usagecode.YohoCd into usageLeft
                        from usage in usageLeft.DefaultIfEmpty()
                        where (main.TradeKana.Contains(searchValue)
                                || main.TradeName.Contains(searchValue)
                                || maker.MakerKana.Contains(searchValue)
                                || maker.MakerName.Contains(searchValue))
                        select new SearchOTCBaseModel()
                        {
                            SerialNum = main.SerialNum,
                            OtcCd = main.OtcCd,
                            TradeName = main.TradeName,
                            TradeKana = main.TradeKana,
                            ClassCd = main.ClassCd,
                            CompanyCd = main.CompanyCd,
                            TradeCd = main.TradeCd,
                            DrugFormCd = main.DrugFormCd,
                            YohoCd = main.YohoCd,
                            Form = form.Form,
                            MakerName = maker.MakerName,
                            MakerKana = maker.MakerKana,
                            Yoho = usage.Yoho,
                            ClassName = clas.ClassName,
                            MajorDivCd = clas.MajorDivCd
                        };
            var total = query.Count();
            var models = query.OrderBy(u => u.TradeKana).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new SearchOTCModel(models,total);
        }
        public List<FoodAlrgyKbnModel> GetFoodAlrgyMasterData()
        {
            List<FoodAlrgyKbnModel> m12FoodAlrgies = new List<FoodAlrgyKbnModel>();
            int i = 0;
            var aleFoodKbns = _tenantDataContext.M12FoodAlrgyKbn.Select(x => new FoodAlrgyKbnModel()
            {
                FoodKbn = x.FoodKbn,
                FoodName = x.FoodName,
                IsDrugAdditives = int.TryParse(x.FoodKbn, out i) && int.Parse(x.FoodKbn) > 50
            }).OrderBy(x=>x.FoodKbn).ToList();
            return aleFoodKbns;
        }
    }
}
