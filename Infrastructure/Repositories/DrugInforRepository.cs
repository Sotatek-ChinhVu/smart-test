using Domain.Models.DrugInfor;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DrugInforRepository : IDrugInforRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public DrugInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public DrugInforModel GetDrugInfor(int hpId, int sinDate, string itemCd)
        {
            var queryItems = _tenantDataContext.TenMsts.Where(
                     item => item.HpId == hpId && new[] { 20, 30 }.Contains(item.SinKouiKbn)
                     && item.StartDate <= sinDate && item.EndDate >= sinDate
                     ).ToList();

            var queryDrugInfs = _tenantDataContext.PiProductInfs.ToList();
            var queryM28DrugMsts = _tenantDataContext.M28DrugMst.ToList();
            var queryM34DrugInfoMains = _tenantDataContext.M34DrugInfoMains.ToList();
            //Join
            var joinQuery = from m28DrugMst in queryM28DrugMsts
                            join tenItem in queryItems
                            on m28DrugMst.KikinCd equals tenItem.ItemCd
                            join m34DrugInfoMain in queryM34DrugInfoMains
                            on m28DrugMst.YjCd equals m34DrugInfoMain.YjCd
                            join drugInf in queryDrugInfs
                            on m28DrugMst.YjCd equals drugInf.YjCd
                            select new { m28DrugMst, tenItem, m34DrugInfoMain, drugInf };

            if (!string.IsNullOrEmpty(itemCd))
            {
                joinQuery = joinQuery.Where(
                       item =>
                       item.tenItem.ItemCd == itemCd);
            }

            var result = joinQuery.AsEnumerable().Select(d => new DrugInforModel(
                                                        d.tenItem != null ? d.tenItem.Name : string.Empty,
                                                        d.drugInf != null ? d.drugInf.GenericName : string.Empty,
                                                        d.drugInf != null ? d.drugInf.Unit : string.Empty,
                                                        d.drugInf != null ? d.drugInf.Marketer : string.Empty,
                                                        d.drugInf != null ? d.drugInf.Vender : string.Empty,
                                                        d.tenItem != null ? d.tenItem.KohatuKbn : 0,
                                                        d.tenItem != null ? d.tenItem.Ten : 0,
                                                        d.tenItem != null ? (d.tenItem.ReceUnitName ?? string.Empty) : string.Empty,
                                                        d.m34DrugInfoMain != null ? d.m34DrugInfoMain.Mark : string.Empty
                                                    )).FirstOrDefault();
            if (result != null)
            {
                return result;
            }
            else
            {
                return new DrugInforModel("", "", "", "", "", 0, 0, "", "");
            }
        }
    }
}
