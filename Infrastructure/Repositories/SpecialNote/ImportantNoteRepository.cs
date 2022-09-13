using Domain.Models.SpecialNote.ImportantNote;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.SpecialNote
{
    public class ImportantNoteRepository : IImportantNoteRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public ImportantNoteRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<PtAlrgyDrugModel> GetAlrgyDrugList(long ptId)
        {
            var ptAlrgyDrugs = _tenantDataContext.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyDrugModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ItemCd ?? String.Empty,
               x.DrugName ?? String.Empty,
               x.StartDate,
               x.EndDate,
               x.Cmt ?? String.Empty,
               x.IsDeleted
            ));
            return ptAlrgyDrugs.ToList();
        }

        public List<PtAlrgyElseModel> GetAlrgyElseList(long ptId)
        {
            var ptAlrgyElses = _tenantDataContext.PtAlrgyElses.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyElseModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.AlrgyName ?? String.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? String.Empty,
                x.IsDeleted
            ));
            return ptAlrgyElses.ToList();
        }

        public List<PtAlrgyFoodModel> GetAlrgyFoodList(long ptId)
        {
            var aleFoodKbns = _tenantDataContext.M12FoodAlrgyKbn.ToList();
            var ptAlrgyFoods = _tenantDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0).ToList();
            var query = from ale in ptAlrgyFoods
                        join mst in aleFoodKbns on ale.AlrgyKbn equals mst.FoodKbn
                        select new PtAlrgyFoodModel
                        (
                              ale.HpId,
                              ale.PtId,
                              ale.SeqNo,
                              ale.SortNo,
                              ale.AlrgyKbn ?? String.Empty,
                              ale.StartDate,
                              ale.EndDate,
                              ale.Cmt ?? String.Empty,
                              ale.IsDeleted,
                              mst.FoodName
                        );

            return query.ToList();
        }

        public List<PtInfectionModel> GetInfectionList(long ptId)
        {
            var ptInfections = _tenantDataContext.PtInfection.Where(x => x.PtId == ptId && x.IsDeleted == 0).OrderBy(x => x.SortNo).Select(x => new PtInfectionModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd ?? String.Empty,
               x.ByotaiCd ?? String.Empty,
               x.Byomei ?? String.Empty,
               x.StartDate,
               x.Cmt ?? String.Empty,
               x.IsDeleted
            ));
            return ptInfections.ToList();
        }

        public List<PtKioRekiModel> GetKioRekiList(long ptId)
        {
            var ptKioRekis = _tenantDataContext.PtKioRekis.Where(x => x.PtId == ptId && x.IsDeleted == 0).OrderBy(p => p.SortNo).Select(x => new PtKioRekiModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd ?? String.Empty,
               x.ByotaiCd ?? String.Empty,
               x.Byomei ?? String.Empty,
               x.StartDate,
               x.Cmt ?? String.Empty,
               x.IsDeleted
            ));

            return ptKioRekis.ToList();
        }

        public List<PtOtcDrugModel> GetOtcDrugList(long ptId)
        {
            var ptOtcDrugs = _tenantDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtcDrugModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.SerialNum,
                x.TradeName ?? String.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? String.Empty,
                x.IsDeleted
            ));
            return ptOtcDrugs.ToList();
        }

        public List<PtOtherDrugModel> GetOtherDrugList(long ptId)
        {
            var ptOtherDrugs = _tenantDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtherDrugModel(
              x.HpId,
              x.PtId,
              x.SeqNo,
              x.SortNo,
              x.ItemCd ?? String.Empty,
              x.DrugName ?? String.Empty,
              x.StartDate,
              x.EndDate,
              x.Cmt ?? String.Empty,
              x.IsDeleted
            ));
            return ptOtherDrugs.ToList();
        }

        public List<PtSuppleModel> GetSuppleList(long ptId)
        {
            var ptSupples = _tenantDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtSuppleModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.IndexCd ?? String.Empty,
                x.IndexWord ?? String.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? String.Empty,
                x.IsDeleted
            ));
            return ptSupples.ToList();
        }
    }
}

