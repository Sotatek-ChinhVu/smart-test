using Domain.Models.MstItem;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Linq;

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
        public SearchSupplementModel GetListSupplement(string searchValue, int pageIndex, int pageSize)
        {
            searchValue = searchValue.Trim();
            try
            {
                var listSuppleIndexCode = _tenantDataContext.M41SuppleIndexcodes.AsQueryable();
                var listSuppleIndexDef = _tenantDataContext.M41SuppleIndexdefs.AsQueryable()
                    .Where(u => string.IsNullOrEmpty(searchValue.Trim()) ? true : u.IndexWord.Contains(searchValue.Trim()))
                        .OrderBy(u => u.SeibunCd)
                            .ThenBy(u => u.IndexWord)
                                .ThenBy(u => u.TokuhoFlg);

                var listSuppleIngre = _tenantDataContext.M41SuppleIngres.AsQueryable();
                var indexDefJoinIngreQueryList = from indexCode in listSuppleIndexCode
                                                 join ingre in listSuppleIngre on indexCode.SeibunCd equals ingre.SeibunCd into suppleIngreList
                                                 from ingreItem in suppleIngreList.DefaultIfEmpty()
                                                 select new
                                                 {
                                                     IndexCode = indexCode,
                                                     Ingre = ingreItem,
                                                 };

                var query = from indexDef in listSuppleIndexDef
                            join indexDefJoinIngreQuery in indexDefJoinIngreQueryList on indexDef.SeibunCd equals indexDefJoinIngreQuery.IndexCode.IndexCd into supplementList
                            from supplementItem in supplementList.DefaultIfEmpty()
                            select new SearchSupplementBaseModel()
                            {
                                SeibunCd = supplementItem.Ingre.SeibunCd,
                                Seibun = supplementItem.Ingre.Seibun,
                                IndexWord = indexDef.IndexWord,
                                TokuhoFlg = indexDef.TokuhoFlg,
                                IndexCd = supplementItem.IndexCode.IndexCd
                            };
                var total = query.Count();
                var result = query.OrderBy(x => x.IndexWord).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                if (result == null || !result.Any())
                {
                    return new SearchSupplementModel(new List<SearchSupplementBaseModel>(),0);
                }
                return new SearchSupplementModel(result, total);
            }
            catch (Exception e)
            {
                return new SearchSupplementModel(new List<SearchSupplementBaseModel>(), 0);
            }
        }
    }
}
