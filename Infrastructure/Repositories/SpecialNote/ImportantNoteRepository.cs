using Domain.Models.SpecialNote.ImportantNote;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories.SpecialNote
{
    public class ImportantNoteRepository : IImportantNoteRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;

        public ImportantNoteRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public void AddAlrgyDrugList(List<PtAlrgyDrugModel> inputDatas, int hpId, int userId)
        {
            var ptId = inputDatas.FirstOrDefault()?.PtId ?? 0;
            var alrgyDrugs = _tenantNoTrackingDataContext.PtAlrgyDrugs.Where(a => a.HpId == hpId && a.PtId == ptId).ToList();
            var maxSortNo = !(alrgyDrugs?.Count > 0) ? 0 : alrgyDrugs.Max(a => a.SortNo);
            foreach (var item in inputDatas)
            {
                _tenantTrackingDataContext.Add(
                    new PtAlrgyDrug
                    {
                        HpId = hpId,
                        PtId = item.PtId,
                        SortNo = maxSortNo++,
                        ItemCd = item.ItemCd,
                        DrugName = item.DrugName,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Cmt = item.Cmt,
                        CreateDate = DateTime.UtcNow,
                        CreateId = userId,
                        UpdateDate = DateTime.UtcNow,
                        UpdateId = userId
                    }
               );
            }

            _tenantTrackingDataContext.SaveChanges();
        }

        public List<PtAlrgyDrugModel> GetAlrgyDrugList(long ptId)
        {
            var ptAlrgyDrugs = _tenantNoTrackingDataContext.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyDrugModel(
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
            var ptAlrgyElses = _tenantNoTrackingDataContext.PtAlrgyElses.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyElseModel(
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
            var aleFoodKbns = _tenantNoTrackingDataContext.M12FoodAlrgyKbn.ToList();
            var ptAlrgyFoods = _tenantNoTrackingDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0).ToList();
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
                              mst.FoodName ?? String.Empty
                        );

            return query.ToList();
        }

        public List<PtInfectionModel> GetInfectionList(long ptId)
        {
            var ptInfections = _tenantNoTrackingDataContext.PtInfection.Where(x => x.PtId == ptId && x.IsDeleted == 0).OrderBy(x => x.SortNo).Select(x => new PtInfectionModel(
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
            var ptKioRekis = _tenantNoTrackingDataContext.PtKioRekis.Where(x => x.PtId == ptId && x.IsDeleted == 0).OrderBy(p => p.SortNo).Select(x => new PtKioRekiModel(
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
            var ptOtcDrugs = _tenantNoTrackingDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtcDrugModel(
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
            var ptOtherDrugs = _tenantNoTrackingDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtherDrugModel(
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
            var ptSupples = _tenantNoTrackingDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtSuppleModel(
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

