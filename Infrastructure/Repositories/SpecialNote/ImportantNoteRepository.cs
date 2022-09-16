using Domain.Models.SpecialNote.ImportantNote;
using Entity.Tenant;
using Helper.Constants;
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

        public void AddAlrgyDrugList(List<PtAlrgyDrugModel> inputDatas)
        {
            _tenantTrackingDataContext.AddRange(
                inputDatas.Select(
                        i => new PtAlrgyDrug
                        {
                            HpId = TempIdentity.HpId,
                            PtId = i.PtId,
                            SortNo = i.SortNo,
                            ItemCd = i.ItemCd,
                            DrugName = i.DrugName,
                            StartDate = i.StartDate,
                            EndDate = i.EndDate,
                            Cmt = i.Cmt,
                            CreateDate = DateTime.UtcNow,
                            CreateId = TempIdentity.UserId,
                            CreateMachine = TempIdentity.ComputerName,
                            UpdateDate = DateTime.UtcNow,
                            UpdateId = TempIdentity.UserId,
                            UpdateMachine = TempIdentity.ComputerName,
                        }
                    )
             );
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
                              mst.FoodName
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

