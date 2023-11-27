using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text;


namespace Infrastructure.Repositories.SpecialNote
{
    public class SpecialNoteRepository : RepositoryBase, ISpecialNoteRepository
    {
        public SpecialNoteRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public bool SaveSpecialNote(int hpId, long ptId, int sinDate, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId)
        {
            if (!IsInvalidInputId(hpId, ptId)) return false;

            if (summaryInfModel != null && summaryInfModel.HpId == hpId && summaryInfModel.PtId == ptId)
            {
                SaveSummaryInf(summaryInfModel, userId);
            }
            if (importantNoteModel != null)
            {
                SaveImportantNote(userId, hpId, ptId, importantNoteModel);
            }
            if (patientInfoModel != null)
            {
                SavePatientInfo(hpId, ptId, sinDate, patientInfoModel, userId);
            }

            TrackingDataContext.SaveChanges();
            return true;
        }
        private bool IsInvalidInputId(int hpId, long ptId)
        {
            if (!NoTrackingDataContext.HpInfs.Any(x => x.HpId == hpId)) return false;
            if (!NoTrackingDataContext.PtInfs.Any(x => x.PtId == ptId)) return false;
            return true;
        }
        #region SaveSummaryInf
        private void SaveSummaryInf(SummaryInfModel summaryInfModel, int userId)
        {
            var summaryInf = TrackingDataContext.SummaryInfs.FirstOrDefault(x => x.Id == summaryInfModel.Id);
            if (summaryInf != null)
            {
                summaryInf.HpId = summaryInfModel.HpId;
                summaryInf.PtId = summaryInfModel.PtId;
                summaryInf.SeqNo = summaryInfModel.SeqNo;
                summaryInf.Text = summaryInfModel.Text;
                summaryInf.Rtext = Encoding.UTF8.GetBytes(summaryInfModel.Rtext);
                summaryInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                summaryInf.UpdateId = userId;
            }
            else
            {
                if (summaryInfModel.HpId != 0 && summaryInfModel.PtId != 0)
                {
                    TrackingDataContext.SummaryInfs.Add(new SummaryInf()
                    {
                        HpId = summaryInfModel.HpId,
                        PtId = summaryInfModel.PtId,
                        SeqNo = summaryInfModel.SeqNo,
                        Text = summaryInfModel.Text,
                        Rtext = Encoding.UTF8.GetBytes(summaryInfModel.Rtext),
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        CreateId = userId
                    });
                }
            }
        }
        #endregion

        #region SaveImportantNote
        private void SaveImportantNote(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            if (importantNoteModel?.AlrgyFoodItems != null && importantNoteModel.AlrgyFoodItems.Any())
            {
                SaveAlrgyFoodItems(userId, hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.AlrgyElseItems != null && importantNoteModel.AlrgyElseItems.Any())
            {
                SaveElseItems(userId, hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.AlrgyDrugItems != null && importantNoteModel.AlrgyDrugItems.Any())
            {
                SaveDrugItems(userId, hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.KioRekiItems != null && importantNoteModel.KioRekiItems.Any())
            {
                SaveKioRekiItems(userId, hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.InfectionsItems != null && importantNoteModel.InfectionsItems.Any())
            {
                SaveInfectionsItems(userId, hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.OtherDrugItems != null && importantNoteModel.OtherDrugItems.Any())
            {
                SaveOtherDrugItems(userId, hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.OtcDrugItems != null && importantNoteModel.OtcDrugItems.Any())
            {
                SaveOtcDrugItems(userId, hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.SuppleItems != null && importantNoteModel.SuppleItems.Any())
            {
                SaveSuppleItems(userId, hpId, ptId, importantNoteModel);
            }
        }
        private void SaveAlrgyFoodItems(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var alrgyFoodItems = NoTrackingDataContext.PtAlrgyFoods.Where(x => x.HpId == hpId && x.PtId == ptId)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo, x.IsDeleted, x.CreateId, x.CreateDate })
                .ToList();
            var alrgyFoodUpdates = alrgyFoodItems.Where(x => x.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateDate, x.CreateId }).ToList();
            var alrgyFoodAdds = alrgyFoodItems.Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
            var alrgyFoodList = importantNoteModel.AlrgyFoodItems.Where(x => x.HpId == hpId && x.PtId == ptId)
                                    .Select(x => new PtAlrgyFood()
                                    {
                                        HpId = x.HpId,
                                        PtId = x.PtId,
                                        SeqNo = x.SeqNo,
                                        SortNo = x.SortNo,
                                        AlrgyKbn = x.AlrgyKbn,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate,
                                        Cmt = x.Cmt,
                                        IsDeleted = x.IsDeleted,
                                    }).ToList();
            var updateAlrgyFoodList = new List<PtAlrgyFood>();
            foreach (var item in alrgyFoodList)
            {
                var updateAlrgyFood = alrgyFoodUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo);
                if (updateAlrgyFood != null)
                {
                    item.CreateId = updateAlrgyFood.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updateAlrgyFood.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updateAlrgyFoodList.Add(item);
                }
            }

            var addAlrgyFoodList = alrgyFoodList.Where(x => !alrgyFoodAdds.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            foreach (var item in addAlrgyFoodList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }
            if (updateAlrgyFoodList.Any())
            {
                TrackingDataContext.PtAlrgyFoods.UpdateRange(updateAlrgyFoodList);
            }
            if (addAlrgyFoodList.Any(x => x.SeqNo == 0))
            {
                TrackingDataContext.PtAlrgyFoods.AddRange(addAlrgyFoodList);
            }
        }
        private void SaveElseItems(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var alrgyElseItems = NoTrackingDataContext.PtAlrgyElses
                .Where(x => x.HpId == hpId && x.PtId == ptId)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate, x.IsDeleted })
                .ToList();
            var alrgyElseItemUpdates = alrgyElseItems.Where(x => x.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate });
            var alrgyElseItemAdds = alrgyElseItems.Select(x => new { x.HpId, x.PtId, x.SeqNo });
            var alrgyElseList = importantNoteModel.AlrgyElseItems.Where(x => x.HpId == hpId && x.PtId == ptId)
                                    .Select(x => new PtAlrgyElse()
                                    {
                                        HpId = x.HpId,
                                        PtId = x.PtId,
                                        SeqNo = x.SeqNo,
                                        SortNo = x.SortNo,
                                        AlrgyName = x.AlrgyName,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate,
                                        Cmt = x.Cmt,
                                        IsDeleted = x.IsDeleted,
                                    }).ToList();
            var updateAlrgyElseList = new List<PtAlrgyElse>();
            foreach (var item in alrgyElseList)
            {
                var updateAlrgyElse = alrgyElseItemUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo);
                if (updateAlrgyElse != null)
                {
                    item.CreateId = updateAlrgyElse.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updateAlrgyElse.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updateAlrgyElseList.Add(item);
                }
            }
            var addAlrgyElseList = alrgyElseList.Where(x => !alrgyElseItemAdds.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            foreach (var item in addAlrgyElseList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            if (updateAlrgyElseList.Any())
            {
                TrackingDataContext.PtAlrgyElses.UpdateRange(updateAlrgyElseList);
            }
            if (addAlrgyElseList.Any(x => x.SeqNo == 0))
            {
                TrackingDataContext.PtAlrgyElses.AddRange(addAlrgyElseList);
            }
        }
        private void SaveDrugItems(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var alrgyDrugItems = NoTrackingDataContext.PtAlrgyDrugs.Where(x => x.HpId == hpId && x.PtId == ptId)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo, x.IsDeleted, x.CreateId, x.CreateDate })
                .ToList();
            var alrgyDrugItemUpdates = alrgyDrugItems.Where(a => a.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate }).ToList();
            var alrgyDrugItemAdds = alrgyDrugItems.Select(x => new { x.HpId, x.PtId, x.SeqNo });

            var alrgyDrugList = importantNoteModel.AlrgyDrugItems.Where(x => x.HpId == hpId && x.PtId == ptId)
                                    .Select(x => new PtAlrgyDrug()
                                    {
                                        HpId = x.HpId,
                                        PtId = x.PtId,
                                        SeqNo = x.SeqNo,
                                        SortNo = x.SortNo,
                                        ItemCd = x.ItemCd,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate,
                                        Cmt = x.Cmt,
                                        IsDeleted = x.IsDeleted,
                                        DrugName = x.DrugName
                                    }).ToList();
            var updateAlrgyDrugList = new List<PtAlrgyDrug>();
            foreach (var item in alrgyDrugList)
            {
                var updateAlrgyDrug = alrgyDrugItemUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo);
                if (updateAlrgyDrug != null)
                {
                    item.CreateId = updateAlrgyDrug.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updateAlrgyDrug.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updateAlrgyDrugList.Add(item);
                }
            }

            var addAlrgyDrugList = alrgyDrugList.Where(x => !alrgyDrugItemAdds.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            foreach (var item in addAlrgyDrugList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            if (updateAlrgyDrugList.Any())
            {
                TrackingDataContext.PtAlrgyDrugs.UpdateRange(updateAlrgyDrugList);
                TrackingDataContext.SaveChanges();
            }
            if (addAlrgyDrugList.Any(a => a.SeqNo == 0))
            {
                TrackingDataContext.PtAlrgyDrugs.AddRange(addAlrgyDrugList);
            }
        }
        private void SaveKioRekiItems(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var kioRekiItems = NoTrackingDataContext.PtKioRekis.Where(x => x.HpId == hpId && x.PtId == ptId)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo, x.IsDeleted, x.CreateDate, x.CreateId }).ToList();
            var kioRekiItemUpdates = kioRekiItems.Where(a => a.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate }).ToList();
            var kioRekiItemAdds = kioRekiItems.Select(x => new { x.HpId, x.PtId, x.SeqNo });
            var kioRekiList = importantNoteModel.KioRekiItems.Where(x => x.HpId == hpId && x.PtId == ptId)
                                    .Select(x => new PtKioReki()
                                    {
                                        HpId = x.HpId,
                                        PtId = x.PtId,
                                        SeqNo = x.SeqNo,
                                        SortNo = x.SortNo,
                                        ByomeiCd = x.ByomeiCd,
                                        ByotaiCd = x.ByotaiCd,
                                        Byomei = x.Byomei,
                                        StartDate = x.StartDate,
                                        Cmt = x.Cmt,
                                        IsDeleted = x.IsDeleted,
                                    }).ToList();
            var updatekioRekiList = new List<PtKioReki>();
            foreach (var item in kioRekiList)
            {
                var updatekioReki = kioRekiItemUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo);
                if (updatekioReki != null)
                {
                    item.CreateId = updatekioReki.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updatekioReki.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updatekioRekiList.Add(item);
                }
            }
            var addkioRekiList = kioRekiList.Where(x => !kioRekiItemAdds.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            foreach (var item in addkioRekiList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            if (updatekioRekiList.Any())
            {
                TrackingDataContext.PtKioRekis.UpdateRange(updatekioRekiList);
            }
            if (addkioRekiList.Any(a => a.SeqNo == 0))
            {
                TrackingDataContext.PtKioRekis.AddRange(addkioRekiList);
            }
        }
        private void SaveInfectionsItems(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var infectionItems = NoTrackingDataContext.PtInfection.Where(x => x.PtId == ptId && x.HpId == hpId)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo, x.IsDeleted, x.CreateId, x.CreateDate }).ToList();
            var infectionItemUpdates = infectionItems.Where(a => a.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate }).ToList();
            var infectionItemAdds = infectionItems.Select(x => new { x.HpId, x.PtId, x.SeqNo });
            var infectionList = importantNoteModel.InfectionsItems.Where(x => x.HpId == hpId && x.PtId == ptId)
                                    .Select(x => new PtInfection()
                                    {
                                        HpId = x.HpId,
                                        PtId = x.PtId,
                                        SeqNo = x.SeqNo,
                                        SortNo = x.SortNo,
                                        ByomeiCd = x.ByomeiCd,
                                        ByotaiCd = x.ByotaiCd,
                                        Byomei = x.Byomei,
                                        StartDate = x.StartDate,
                                        Cmt = x.Cmt,
                                        IsDeleted = x.IsDeleted,
                                    }).ToList();
            var updateInfectionList = new List<PtInfection>();
            foreach (var item in infectionList)
            {
                var updateInfection = infectionItemUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo);
                if (updateInfection != null)
                {
                    item.CreateId = updateInfection.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updateInfection.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updateInfectionList.Add(item);
                }
            }

            var addInfectionList = infectionList.Where(x => !infectionItemAdds.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            foreach (var item in addInfectionList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            if (updateInfectionList.Any())
            {
                TrackingDataContext.PtInfection.UpdateRange(updateInfectionList);
            }
            if (addInfectionList.Any(a => a.SeqNo == 0))
            {
                TrackingDataContext.PtInfection.AddRange(addInfectionList);
            }
        }
        private void SaveOtherDrugItems(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var otherDrugItems = NoTrackingDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.HpId == hpId)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo, x.IsDeleted, x.CreateDate, x.CreateId }).ToList();
            var otherDrugItemUpdates = otherDrugItems.Where(a => a.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate }).ToList();
            var otherDrugItemAdds = otherDrugItems.Select(x => new { x.HpId, x.PtId, x.SeqNo });
            var otherDrugList = importantNoteModel.OtherDrugItems.Where(x => x.HpId == hpId && x.PtId == ptId)
                                    .Select(x => new PtOtherDrug()
                                    {
                                        HpId = x.HpId,
                                        PtId = x.PtId,
                                        SeqNo = x.SeqNo,
                                        SortNo = x.SortNo,
                                        ItemCd = x.ItemCd,
                                        DrugName = x.DrugName,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate,
                                        Cmt = x.Cmt,
                                        IsDeleted = x.IsDeleted,
                                    }).ToList();
            var updateOtherDrugList = new List<PtOtherDrug>();
            foreach (var item in otherDrugList)
            {
                var updateOtherDrug = otherDrugItemUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo);
                if (updateOtherDrug != null)
                {
                    item.CreateId = updateOtherDrug.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updateOtherDrug.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updateOtherDrugList.Add(item);
                }
            }
            var addOtherDrugList = otherDrugList.Where(x => !otherDrugItemAdds.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            foreach (var item in addOtherDrugList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            if (updateOtherDrugList.Any())
            {
                TrackingDataContext.PtOtherDrug.UpdateRange(updateOtherDrugList);
            }
            if (addOtherDrugList.Any(x => x.SeqNo == 0))
            {
                TrackingDataContext.PtOtherDrug.AddRange(addOtherDrugList);
            }
        }
        private void SaveOtcDrugItems(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var otcDrugItems = NoTrackingDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.HpId == hpId)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo, x.IsDeleted, x.CreateId, x.CreateDate }).ToList();
            var otcDrugItemUpdates = otcDrugItems.Where(a => a.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate }).ToList();
            var otcDrugItemAdds = otcDrugItems.Select(x => new { x.HpId, x.PtId, x.SeqNo });
            var otcDrugList = importantNoteModel.OtcDrugItems.Where(x => x.HpId == hpId && x.PtId == ptId)
                                    .Select(x => new PtOtcDrug()
                                    {
                                        HpId = x.HpId,
                                        PtId = x.PtId,
                                        SeqNo = x.SeqNo,
                                        SortNo = x.SortNo,
                                        SerialNum = x.SerialNum,
                                        TradeName = x.TradeName,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate,
                                        Cmt = x.Cmt,
                                        IsDeleted = x.IsDeleted,
                                    }).ToList();
            var updateOtcDrugList = new List<PtOtcDrug>();
            foreach (var item in otcDrugList)
            {
                var updateOtcDrug = otcDrugItemUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo);
                if (updateOtcDrug != null)
                {
                    item.CreateId = updateOtcDrug.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updateOtcDrug.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updateOtcDrugList.Add(item);
                }
            }
            var addOtcDrugList = otcDrugList.Where(x => !otcDrugItemAdds.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            foreach (var item in addOtcDrugList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            if (updateOtcDrugList.Any())
            {
                TrackingDataContext.PtOtcDrug.UpdateRange(updateOtcDrugList);
            }
            if (addOtcDrugList.Any(a => a.SeqNo == 0))
            {
                TrackingDataContext.PtOtcDrug.AddRange(addOtcDrugList);
            }
        }
        private void SaveSuppleItems(int userId, int hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var suppleItems = NoTrackingDataContext.PtSupples.Where(x => x.PtId == ptId && x.HpId == hpId)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo, x.IsDeleted, x.CreateDate, x.CreateId }).ToList();
            var suppleItemUpdates = suppleItems.Where(a => a.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate }).ToList();
            var suppleItemAdds = suppleItems.Select(x => new { x.HpId, x.PtId, x.SeqNo });
            var suppleList = importantNoteModel.SuppleItems.Where(x => x.HpId == hpId && x.PtId == ptId)
                                    .Select(x => new PtSupple()
                                    {
                                        HpId = x.HpId,
                                        PtId = x.PtId,
                                        SeqNo = x.SeqNo,
                                        SortNo = x.SortNo,
                                        IndexCd = x.IndexCd,
                                        IndexWord = x.IndexWord,
                                        StartDate = x.StartDate,
                                        EndDate = x.EndDate,
                                        Cmt = x.Cmt,
                                        IsDeleted = x.IsDeleted,
                                    }).ToList();
            List<PtSupple> updateSuppleList = new();
            foreach (var item in suppleList)
            {
                var updateSupple = suppleItemUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo);
                if (updateSupple != null)
                {
                    item.CreateId = updateSupple.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updateSupple.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updateSuppleList.Add(item);
                }
            }
            var addSuppleList = suppleList.Where(x => !suppleItemAdds.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            foreach (var item in addSuppleList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            if (updateSuppleList.Any())
            {
                TrackingDataContext.PtSupples.UpdateRange(updateSuppleList);
            }
            if (addSuppleList.Any(s => s.SeqNo == 0))
            {
                TrackingDataContext.PtSupples.AddRange(addSuppleList);
            }
        }
        #endregion

        #region SavePatientInfo
        private void SavePatientInfo(int hpId, long ptId, int sinDate, PatientInfoModel patientInfoModel, int userId)
        {
            foreach (var pregnancyItem in patientInfoModel.PregnancyItems)
            {
                if (patientInfoModel?.PregnancyItems != null && pregnancyItem.HpId == hpId && pregnancyItem.PtId == ptId)
                {
                    SavePregnancyItems(pregnancyItem, userId);
                }
            }

            if (patientInfoModel?.PtCmtInfItems != null && patientInfoModel.PtCmtInfItems.HpId == hpId && patientInfoModel.PtCmtInfItems.PtId == ptId)
            {
                SavePtCmtInfItems(patientInfoModel, userId);
            }
            if (patientInfoModel?.SeikatureInfItems != null && patientInfoModel.SeikatureInfItems.HpId == hpId && patientInfoModel.SeikatureInfItems.PtId == ptId)
            {
                SaveSeikatureInfItems(patientInfoModel, userId);
            }
            if (patientInfoModel?.PhysicalInfItems != null && patientInfoModel.PhysicalInfItems.Any())
            {
                SavePhysicalInfItems(hpId, ptId, sinDate, userId, patientInfoModel);
            }
        }
        private void SavePregnancyItems(PtPregnancyModel ptPregnancy, int userId)
        {
            var pregnancyItem = TrackingDataContext.PtPregnancies.FirstOrDefault(x => x.Id == ptPregnancy.Id && x.IsDeleted == 0);
            if (pregnancyItem != null)
            {
                pregnancyItem.StartDate = ptPregnancy.StartDate;
                pregnancyItem.EndDate = ptPregnancy.EndDate;
                pregnancyItem.PeriodDate = ptPregnancy.PeriodDate;
                pregnancyItem.PeriodDueDate = ptPregnancy.PeriodDueDate;
                pregnancyItem.OvulationDate = ptPregnancy.OvulationDate;
                pregnancyItem.OvulationDueDate = ptPregnancy.OvulationDueDate;
                pregnancyItem.IsDeleted = ptPregnancy.IsDeleted;
                pregnancyItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                pregnancyItem.UpdateId = userId;
            }
            else
            {
                var pregnancyObj = new PtPregnancy()
                {
                    HpId = ptPregnancy.HpId,
                    PtId = ptPregnancy.PtId,
                    SeqNo = ptPregnancy.SeqNo,
                    StartDate = ptPregnancy.StartDate,
                    EndDate = ptPregnancy.EndDate,
                    PeriodDate = ptPregnancy.PeriodDate,
                    PeriodDueDate = ptPregnancy.PeriodDueDate,
                    OvulationDate = ptPregnancy.OvulationDate,
                    OvulationDueDate = ptPregnancy.OvulationDueDate,
                    IsDeleted = ptPregnancy.IsDeleted,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    CreateId = userId
                };
                TrackingDataContext.PtPregnancies.Add(pregnancyObj);
            }
        }
        private void SavePtCmtInfItems(PatientInfoModel patientInfoModel, int userId)
        {
            var ptCmtInfItem = TrackingDataContext.PtCmtInfs.FirstOrDefault(x => x.Id == patientInfoModel.PtCmtInfItems.Id && x.IsDeleted == 0);
            if (ptCmtInfItem != null)
            {
                ptCmtInfItem.Text = patientInfoModel.PtCmtInfItems.Text;
                ptCmtInfItem.IsDeleted = patientInfoModel.PtCmtInfItems.IsDeleted;
                ptCmtInfItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                ptCmtInfItem.UpdateId = userId;
            }
            else
            {
                var PtCmtInfObj = new PtCmtInf()
                {
                    HpId = patientInfoModel.PtCmtInfItems.HpId,
                    PtId = patientInfoModel.PtCmtInfItems.PtId,
                    SeqNo = patientInfoModel.PtCmtInfItems.SeqNo,
                    Text = patientInfoModel.PtCmtInfItems.Text,
                    IsDeleted = patientInfoModel.PtCmtInfItems.IsDeleted,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    CreateId = userId
                };
                TrackingDataContext.PtCmtInfs.Add(PtCmtInfObj);
            }
        }
        private void SaveSeikatureInfItems(PatientInfoModel patientInfoModel, int userId)
        {
            var seikatureInfItem = TrackingDataContext.SeikaturekiInfs.FirstOrDefault(x => x.Id == patientInfoModel.SeikatureInfItems.Id);
            if (seikatureInfItem != null)
            {
                seikatureInfItem.HpId = patientInfoModel.SeikatureInfItems.HpId;
                seikatureInfItem.PtId = patientInfoModel.SeikatureInfItems.PtId;
                seikatureInfItem.SeqNo = patientInfoModel.SeikatureInfItems.SeqNo;
                seikatureInfItem.Text = patientInfoModel.SeikatureInfItems.Text;
                seikatureInfItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                seikatureInfItem.UpdateId = userId;
            }
            else
            {
                var seikatureInfObj = new SeikaturekiInf()
                {
                    HpId = patientInfoModel.SeikatureInfItems.HpId,
                    PtId = patientInfoModel.SeikatureInfItems.PtId,
                    SeqNo = patientInfoModel.SeikatureInfItems.SeqNo,
                    Text = patientInfoModel.SeikatureInfItems.Text,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    CreateId = userId
                };
                TrackingDataContext.SeikaturekiInfs.Add(seikatureInfObj);
            }
        }
        private void SavePhysicalInfItems(int hpId, long ptId, int sinDate, int userId, PatientInfoModel patientInfoModel)
        {
            var kensaInfDetailModels = new List<KensaInfDetailModel>();
            foreach (var physicalInfo in patientInfoModel.PhysicalInfItems)
            {
                if (physicalInfo?.KensaInfDetailModels != null && physicalInfo.KensaInfDetailModels.Any())
                {
                    kensaInfDetailModels.AddRange(physicalInfo.KensaInfDetailModels);
                }
            }

            var kensaInfDetailModelIsDeleteds = kensaInfDetailModels.Where(p => p.IsDeleted != DeleteTypes.None).ToList();
            foreach (var kensaInfDetailModelIsDeleted in kensaInfDetailModelIsDeleteds)
            {
                var kensaInfDetail = TrackingDataContext.KensaInfDetails.FirstOrDefault(k => k.HpId == kensaInfDetailModelIsDeleted.HpId && k.PtId == kensaInfDetailModelIsDeleted.PtId && kensaInfDetailModelIsDeleted.IraiCd == k.IraiCd && kensaInfDetailModelIsDeleted.SeqNo == k.SeqNo);
                if (kensaInfDetail != null)
                {
                    TrackingDataContext.KensaInfDetails.Remove(kensaInfDetail);
                }
            }

            var raiinNo = kensaInfDetailModels.Select(k => k.RaiinNo).Distinct().FirstOrDefault();
            TrackingDataContext.KensaInfs.Add(new KensaInf()
            {
                HpId = hpId,
                PtId = ptId,
                RaiinNo = raiinNo,
                IraiDate = sinDate,
                Status = 2,
                InoutKbn = 0,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId
            });
            var kensaInfDetails = NoTrackingDataContext.KensaInfDetails
                .Where(x => x.HpId == hpId && x.PtId == ptId)
                .Select(x => new { x.HpId, x.PtId, x.IraiCd, x.SeqNo, x.CreateId, x.CreateDate, x.IsDeleted })
                .ToList();
            var kensaInfDetailUpdates = kensaInfDetails.Where(a => a.IsDeleted == 0).Select(x => new { x.HpId, x.PtId, x.SeqNo, x.CreateId, x.CreateDate, x.IraiCd }).ToList();
            var kensaInfDetailAdds = kensaInfDetails.Select(x => new { x.HpId, x.PtId, x.IraiCd, x.SeqNo }).ToList();
            var kensaInfDetailListInputs = kensaInfDetailModels.Where(x => x.HpId == hpId && x.PtId == ptId)
                .Select(x => new KensaInfDetail()
                {
                    HpId = x.HpId,
                    PtId = x.PtId,
                    IraiDate = x.IraiDate,
                    RaiinNo = x.RaiinNo,
                    IraiCd = x.IraiCd,
                    SeqNo = x.SeqNo,
                    KensaItemCd = x.KensaItemCd,
                    ResultVal = x.ResultVal,
                    ResultType = x.ResultType,
                    AbnormalKbn = x.AbnormalKbn,
                    IsDeleted = x.IsDeleted,
                    CmtCd1 = x.CmtCd1,
                    CmtCd2 = x.CmtCd2
                });

            var updateKensaInfDetailList = new List<KensaInfDetail>();
            foreach (var item in kensaInfDetailListInputs)
            {
                var updateSupple = kensaInfDetailUpdates.FirstOrDefault(a => a.HpId == item.HpId && a.PtId == item.PtId && a.SeqNo == item.SeqNo && item.IraiCd == a.IraiCd);
                if (updateSupple != null)
                {
                    item.CreateId = updateSupple.CreateId;
                    item.CreateDate = TimeZoneInfo.ConvertTimeToUtc(updateSupple.CreateDate);
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    updateKensaInfDetailList.Add(item);
                }
            }
            var addKensaInfDetailList = kensaInfDetailListInputs.Where(x => !kensaInfDetailAdds.Contains(new { x.HpId, x.PtId, x.IraiCd, x.SeqNo }));
            foreach (var item in addKensaInfDetailList)
            {
                item.CreateDate = CIUtil.GetJapanDateTimeNow();
                item.CreateId = userId;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }
            if (updateKensaInfDetailList.Any())
            {
                TrackingDataContext.KensaInfDetails.UpdateRange(updateKensaInfDetailList);
            }
            if (addKensaInfDetailList.Any(a => a.SeqNo == 0))
            {
                TrackingDataContext.KensaInfDetails.AddRange(addKensaInfDetailList);
            }
            TrackingDataContext.SaveChanges();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        #endregion
    }
}
