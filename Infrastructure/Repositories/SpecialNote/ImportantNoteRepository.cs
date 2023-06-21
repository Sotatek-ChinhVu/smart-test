﻿using Domain.Models.SpecialNote.ImportantNote;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories.SpecialNote
{
    public class ImportantNoteRepository : RepositoryBase, IImportantNoteRepository
    {
        public ImportantNoteRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public void AddAlrgyDrugList(List<PtAlrgyDrugModel> inputDatas, int hpId, int userId)
        {
            var ptId = inputDatas.FirstOrDefault()?.PtId ?? 0;
            var alrgyDrugs = NoTrackingDataContext.PtAlrgyDrugs.Where(a => a.HpId == hpId && a.PtId == ptId).ToList();
            var maxSortNo = !(alrgyDrugs?.Count > 0) ? 0 : alrgyDrugs.Max(a => a.SortNo);
            foreach (var item in inputDatas)
            {
                TrackingDataContext.Add(
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
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId
                    }
               );
            }

            TrackingDataContext.SaveChanges();
        }

        public List<PtAlrgyDrugModel> GetAlrgyDrugList(long ptId)
        {
            var ptAlrgyDrugs = NoTrackingDataContext.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyDrugModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ItemCd ?? string.Empty,
               x.DrugName ?? string.Empty,
               x.StartDate,
               x.EndDate,
               x.Cmt ?? string.Empty,
               x.IsDeleted
            ));
            return ptAlrgyDrugs.ToList();
        }

        public List<PtAlrgyDrugModel> GetAlrgyDrugList(long ptId, int sinDate)
        {
            var ptAlrgyDrugs = NoTrackingDataContext.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyDrugModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ItemCd ?? string.Empty,
               x.DrugName ?? string.Empty,
               x.StartDate,
               x.EndDate,
               x.Cmt ?? string.Empty,
               x.IsDeleted
            ));
            return ptAlrgyDrugs.AsEnumerable().Where(x => x.FullStartDate <= sinDate && sinDate <= x.FullEndDate).OrderBy(x => x.SortNo).ToList();
        }

        public List<PtAlrgyElseModel> GetAlrgyElseList(long ptId)
        {
            var ptAlrgyElses = NoTrackingDataContext.PtAlrgyElses.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyElseModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.AlrgyName ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptAlrgyElses.ToList();
        }

        public List<PtAlrgyElseModel> GetAlrgyElseList(long ptId, int sinDate)
        {
            var ptAlrgyElses = NoTrackingDataContext.PtAlrgyElses.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyElseModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.AlrgyName ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptAlrgyElses.AsEnumerable().Where(x => x.FullStartDate <= sinDate && sinDate <= x.FullEndDate).OrderBy(x => x.SortNo).ToList();
        }

        public List<PtAlrgyFoodModel> GetAlrgyFoodList(long ptId)
        {
            var aleFoodKbns = NoTrackingDataContext.M12FoodAlrgyKbn.ToList();
            var ptAlrgyFoods = NoTrackingDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0).ToList();
            var query = from ale in ptAlrgyFoods
                        join mst in aleFoodKbns on ale.AlrgyKbn equals mst.FoodKbn
                        select new PtAlrgyFoodModel
                        (
                              ale.HpId,
                              ale.PtId,
                              ale.SeqNo,
                              ale.SortNo,
                              ale.AlrgyKbn ?? string.Empty,
                              ale.StartDate,
                              ale.EndDate,
                              ale.Cmt ?? string.Empty,
                              ale.IsDeleted,
                              mst.FoodName ?? string.Empty
                        );

            return query.ToList();
        }

        public List<PtAlrgyFoodModel> GetAlrgyFoodList(long ptId, int sinDate)
        {
            var aleFoodKbns = NoTrackingDataContext.M12FoodAlrgyKbn.ToList();
            var ptAlrgyFoods = NoTrackingDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0).ToList();
            var query = from ale in ptAlrgyFoods
                        join mst in aleFoodKbns on ale.AlrgyKbn equals mst.FoodKbn
                        select new PtAlrgyFoodModel
                        (
                              ale.HpId,
                              ale.PtId,
                              ale.SeqNo,
                              ale.SortNo,
                              ale.AlrgyKbn ?? string.Empty,
                              ale.StartDate,
                              ale.EndDate,
                              ale.Cmt ?? string.Empty,
                              ale.IsDeleted,
                              mst.FoodName ?? string.Empty
                        );

            return query.AsEnumerable().Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate)
                                            .OrderBy(p => p.SortNo).ToList();
        }

        public List<PtInfectionModel> GetInfectionList(long ptId)
        {
            var ptInfections = NoTrackingDataContext.PtInfection.Where(x => x.PtId == ptId && x.IsDeleted == 0).OrderBy(x => x.SortNo).Select(x => new PtInfectionModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd ?? string.Empty,
               x.ByotaiCd ?? string.Empty,
               x.Byomei ?? string.Empty,
               x.StartDate,
               x.Cmt ?? string.Empty,
               x.IsDeleted
            ));
            return ptInfections.ToList();
        }

        public List<PtKioRekiModel> GetKioRekiList(long ptId)
        {
            var ptKioRekis = NoTrackingDataContext.PtKioRekis.Where(x => x.PtId == ptId && x.IsDeleted == 0).OrderBy(p => p.SortNo).Select(x => new PtKioRekiModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ByomeiCd ?? string.Empty,
               x.ByotaiCd ?? string.Empty,
               x.Byomei ?? string.Empty,
               x.StartDate,
               x.Cmt ?? string.Empty,
               x.IsDeleted
            ));

            return ptKioRekis.ToList();
        }

        public List<PtOtcDrugModel> GetOtcDrugList(long ptId)
        {
            var ptOtcDrugs = NoTrackingDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtcDrugModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.SerialNum,
                x.TradeName ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptOtcDrugs.ToList();
        }

        public List<PtOtcDrugModel> GetOtcDrugList(long ptId, int sinDate)
        {
            var ptOtcDrugs = NoTrackingDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtcDrugModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.SerialNum,
                x.TradeName ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptOtcDrugs.AsEnumerable().Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate)
                    .OrderBy(p => p.SortNo).ToList();
        }

        public List<PtOtherDrugModel> GetOtherDrugList(long ptId)
        {
            var ptOtherDrugs = NoTrackingDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtherDrugModel(
              x.HpId,
              x.PtId,
              x.SeqNo,
              x.SortNo,
              x.ItemCd ?? string.Empty,
              x.DrugName ?? string.Empty,
              x.StartDate,
              x.EndDate,
              x.Cmt ?? string.Empty,
              x.IsDeleted
            ));
            return ptOtherDrugs.ToList();
        }

        public List<PtOtherDrugModel> GetOtherDrugList(long ptId, int sinDate)
        {
            var ptOtherDrugs = NoTrackingDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtherDrugModel(
              x.HpId,
              x.PtId,
              x.SeqNo,
              x.SortNo,
              x.ItemCd ?? string.Empty,
              x.DrugName ?? string.Empty,
              x.StartDate,
              x.EndDate,
              x.Cmt ?? string.Empty,
              x.IsDeleted
            ));
            return ptOtherDrugs.AsEnumerable().Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.SortNo).ToList();
        }

        public List<PtSuppleModel> GetSuppleList(long ptId)
        {
            var ptSupples = NoTrackingDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtSuppleModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.IndexCd ?? string.Empty,
                x.IndexWord ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptSupples.OrderBy(x => x.SortNo).ToList();
        }

        public List<PtSuppleModel> GetSuppleList(long ptId, int sinDate)
        {
            var ptSupples = NoTrackingDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtSuppleModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.IndexCd ?? string.Empty,
                x.IndexWord ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptSupples.AsEnumerable().Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(x => x.SortNo).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}

