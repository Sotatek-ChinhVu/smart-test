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
using System.Text;


namespace Infrastructure.Repositories.SpecialNote
{
    public class SpecialNoteRepository : RepositoryBase, ISpecialNoteRepository
    {
        public SpecialNoteRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public bool SaveSpecialNote(int hpId, long ptId, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId)
        {
            if (!IsInvalidInputId(hpId, ptId)) return false;

            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();

            var result = executionStrategy.Execute(
                () =>
                {
                    // execute your logic here
                    using (var transaction = TrackingDataContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (summaryInfModel != null && summaryInfModel.Id == hpId && summaryInfModel.PtId == ptId)
                            {
                                SaveSummaryInf(hpId, ptId, summaryInfModel, userId);
                                TrackingDataContext.SaveChanges();
                            }
                            if (importantNoteModel != null)
                            {
                                SaveImportantNote(hpId, ptId, importantNoteModel);
                                TrackingDataContext.SaveChanges();
                            }
                            if (patientInfoModel != null)
                            {
                                SavePatientInfo(hpId, ptId, patientInfoModel, userId);
                                TrackingDataContext.SaveChanges();
                            }
                            TrackingDataContext.SaveChanges();
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }
                });
            return result;
        }
        private bool IsInvalidInputId(int hpId, long ptId)
        {
            if (!NoTrackingDataContext.HpInfs.Any(x => x.HpId == hpId)) return false;
            if (!NoTrackingDataContext.PtInfs.Any(x => x.PtId == ptId)) return false;
            return true;
        }
        #region SaveSummaryInf
        private void SaveSummaryInf(int hpId, long ptId, SummaryInfModel summaryInfModel, int userId)
        {
            var summaryInf = TrackingDataContext.SummaryInfs.FirstOrDefault(x => x.PtId == ptId && x.HpId == hpId);
            if (summaryInf != null)
            {
                summaryInf.Id = summaryInfModel.Id;
                summaryInf.HpId = summaryInfModel.HpId;
                summaryInf.PtId = summaryInfModel.PtId;
                summaryInf.SeqNo = summaryInfModel.SeqNo;
                summaryInf.Text = summaryInfModel.Text;
                summaryInf.Rtext = Encoding.ASCII.GetBytes(summaryInfModel.Rtext);
                summaryInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                summaryInf.UpdateId = userId;
            }
            else
            {
                TrackingDataContext.SummaryInfs.Add(new SummaryInf()
                {
                    HpId = summaryInfModel.HpId,
                    PtId = summaryInfModel.PtId,
                    SeqNo = summaryInfModel.SeqNo,
                    Text = summaryInfModel.Text,
                    Rtext = Encoding.ASCII.GetBytes(summaryInfModel.Rtext),
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    CreateId = userId
                });
            }
        }
        #endregion

        #region SaveImportantNote
        private void SaveImportantNote(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            if (importantNoteModel?.AlrgyFoodItems != null && importantNoteModel.AlrgyFoodItems.Any())
            {
                SaveAlrgyFoodItems(hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.AlrgyElseItems != null && importantNoteModel.AlrgyElseItems.Any())
            {
                SaveElseItems(hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.AlrgyDrugItems != null && importantNoteModel.AlrgyDrugItems.Any())
            {
                SaveDrugItems(hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.KioRekiItems != null && importantNoteModel.KioRekiItems.Any())
            {
                SaveKioRekiItems(hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.InfectionsItems != null && importantNoteModel.InfectionsItems.Any())
            {
                SaveInfectionsItems(hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.OtherDrugItems != null && importantNoteModel.OtherDrugItems.Any())
            {
                SaveOtherDrugItems(hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.OtcDrugItems != null && importantNoteModel.OtcDrugItems.Any())
            {
                SaveOtcDrugItems(hpId, ptId, importantNoteModel);
            }
            if (importantNoteModel?.SuppleItems != null && importantNoteModel.SuppleItems.Any())
            {
                SaveSuppleItems(hpId, ptId, importantNoteModel);
            }
        }
        private void SaveAlrgyFoodItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var alrgyFoodItems = NoTrackingDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo })
                .ToList();
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
            var updateAlrgyFoodList = alrgyFoodList.Where(x => alrgyFoodItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addAlrgyFoodList = alrgyFoodList.Where(x => !alrgyFoodItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateAlrgyFoodList != null && updateAlrgyFoodList.Any())
            {
                TrackingDataContext.PtAlrgyFoods.UpdateRange(updateAlrgyFoodList);
            }
            if (addAlrgyFoodList != null && addAlrgyFoodList.Any())
            {
                TrackingDataContext.PtAlrgyFoods.AddRange(addAlrgyFoodList);
            }
        }
        private void SaveElseItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var alrgyElseItems = NoTrackingDataContext.PtAlrgyElses
                .Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo })
                .ToList();
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
            var updateAlrgyElseList = alrgyElseList.Where(x => alrgyElseItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addAlrgyElseList = alrgyElseList.Where(x => !alrgyElseItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateAlrgyElseList != null && updateAlrgyElseList.Any())
            {
                TrackingDataContext.PtAlrgyElses.UpdateRange(updateAlrgyElseList);
            }
            if (addAlrgyElseList != null && addAlrgyElseList.Any())
            {
                TrackingDataContext.PtAlrgyElses.AddRange(addAlrgyElseList);
            }
        }
        private void SaveDrugItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var alrgyDrugItems = NoTrackingDataContext.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo })
                .ToList();
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
                                    }).ToList();
            var updateAlrgyDrugList = alrgyDrugList.Where(x => alrgyDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addAlrgyDrugList = alrgyDrugList.Where(x => !alrgyDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateAlrgyDrugList != null && updateAlrgyDrugList.Any())
            {
                TrackingDataContext.PtAlrgyDrugs.UpdateRange(updateAlrgyDrugList);
            }
            if (addAlrgyDrugList != null && addAlrgyDrugList.Any())
            {
                TrackingDataContext.PtAlrgyDrugs.AddRange(addAlrgyDrugList);
            }
        }
        private void SaveKioRekiItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var kioRekiItems = NoTrackingDataContext.PtKioRekis.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
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
            var updatekioRekiList = kioRekiList.Where(x => kioRekiItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addkioRekiList = kioRekiList.Where(x => !kioRekiItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updatekioRekiList != null && updatekioRekiList.Any())
            {
                TrackingDataContext.PtKioRekis.UpdateRange(updatekioRekiList);
            }
            if (addkioRekiList != null && addkioRekiList.Any())
            {
                TrackingDataContext.PtKioRekis.AddRange(addkioRekiList);
            }
        }
        private void SaveInfectionsItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var infectionItems = NoTrackingDataContext.PtInfection.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
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
            var updateInfectionList = infectionList.Where(x => infectionItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addInfectionList = infectionList.Where(x => !infectionItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateInfectionList != null && updateInfectionList.Any())
            {
                TrackingDataContext.PtInfection.UpdateRange(updateInfectionList);
            }
            if (addInfectionList != null && addInfectionList.Any())
            {
                TrackingDataContext.PtInfection.AddRange(addInfectionList);
            }
        }
        private void SaveOtherDrugItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var otherDrugItems = NoTrackingDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
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
            var updateOtherDrugList = otherDrugList.Where(x => otherDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addOtherDrugList = otherDrugList.Where(x => !otherDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateOtherDrugList != null && updateOtherDrugList.Any())
            {
                TrackingDataContext.PtOtherDrug.UpdateRange(updateOtherDrugList);
            }
            if (addOtherDrugList != null && addOtherDrugList.Any())
            {
                TrackingDataContext.PtOtherDrug.AddRange(addOtherDrugList);
            }
        }
        private void SaveOtcDrugItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var otcDrugItems = NoTrackingDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
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
            var updateOtcDrugList = otcDrugList.Where(x => otcDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addOtcDrugList = otcDrugList.Where(x => !otcDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateOtcDrugList != null && updateOtcDrugList.Any())
            {
                TrackingDataContext.PtOtcDrug.UpdateRange(updateOtcDrugList);
            }
            if (addOtcDrugList != null && addOtcDrugList.Any())
            {
                TrackingDataContext.PtOtcDrug.AddRange(addOtcDrugList);
            }
        }
        private void SaveSuppleItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var suppleItems = NoTrackingDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
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
            var updatesuppleList = suppleList.Where(x => suppleItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addSuppleList = suppleList.Where(x => !suppleItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updatesuppleList != null && updatesuppleList.Any())
            {
                TrackingDataContext.PtSupples.UpdateRange(updatesuppleList);
            }
            if (addSuppleList != null && addSuppleList.Any())
            {
                TrackingDataContext.PtSupples.AddRange(addSuppleList);
            }
        }
        #endregion

        #region SavePatientInfo
        private void SavePatientInfo(int hpId, long ptId, PatientInfoModel patientInfoModel, int userId)
        {
            var ids = patientInfoModel.PregnancyItems.Where(p => p.IsDeleted != DeleteTypes.None).Select(p => p.Id).Distinct();
            var pregnancyItemIsDeleteds = TrackingDataContext.PtPregnancies.Where(p => ids.Contains(p.Id));
            TrackingDataContext.PtPregnancies.RemoveRange(pregnancyItemIsDeleteds);
            foreach (var pregnancyItem in patientInfoModel.PregnancyItems)
            {
                if (patientInfoModel?.PregnancyItems != null && pregnancyItem.HpId == hpId && pregnancyItem.PtId == ptId)
                {
                    SavePregnancyItems(hpId, ptId, pregnancyItem, userId);
                }
            }

            if (patientInfoModel?.PtCmtInfItems != null && patientInfoModel.PtCmtInfItems.HpId == hpId && patientInfoModel.PtCmtInfItems.PtId == ptId)
            {
                SavePtCmtInfItems(hpId, ptId, patientInfoModel, userId);
            }
            if (patientInfoModel?.SeikatureInfItems != null && patientInfoModel.SeikatureInfItems.HpId == hpId && patientInfoModel.SeikatureInfItems.PtId == ptId)
            {
                SaveSeikatureInfItems(hpId, ptId, patientInfoModel, userId);
            }
            if (patientInfoModel?.PhysicalInfItems != null && patientInfoModel.PhysicalInfItems.Any())
            {
                SavePhysicalInfItems(hpId, ptId, patientInfoModel);
            }
        }
        private void SavePregnancyItems(int hpId, long ptId, PtPregnancyModel ptPregnancy, int userId)
        {
            var pregnancyItem = NoTrackingDataContext.PtPregnancies.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0);
            if (pregnancyItem != null)
            {
                pregnancyItem.Id = ptPregnancy.Id;
                pregnancyItem.HpId = ptPregnancy.HpId;
                pregnancyItem.PtId = ptPregnancy.PtId;
                pregnancyItem.SeqNo = ptPregnancy.SeqNo;
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
        private void SavePtCmtInfItems(int hpId, long ptId, PatientInfoModel patientInfoModel, int userId)
        {
            var ptCmtInfItem = NoTrackingDataContext.PtCmtInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0);
            if (ptCmtInfItem != null)
            {
                ptCmtInfItem.HpId = patientInfoModel.PtCmtInfItems.HpId;
                ptCmtInfItem.PtId = patientInfoModel.PtCmtInfItems.PtId;
                ptCmtInfItem.SeqNo = patientInfoModel.PtCmtInfItems.SeqNo;
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
        private void SaveSeikatureInfItems(int hpId, long ptId, PatientInfoModel patientInfoModel, int userId)
        {
            var seikatureInfItem = NoTrackingDataContext.SeikaturekiInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId);
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
        private void SavePhysicalInfItems(int hpId, long ptId, PatientInfoModel patientInfoModel)
        {
            var kensaInfDetailModels = new List<KensaInfDetailModel>();
            foreach (var physicalInfo in patientInfoModel.PhysicalInfItems.Where(x => x.HpId == hpId).ToList())
            {
                if (physicalInfo?.KensaInfDetailModels != null && physicalInfo.KensaInfDetailModels.Any())
                {
                    kensaInfDetailModels.AddRange(physicalInfo.KensaInfDetailModels);
                }
            }
            var kensaInfDetails = NoTrackingDataContext.KensaInfDetails
                .Where(x => x.HpId == hpId && x.PtId == ptId)
                .Select(x => new { x.HpId, x.PtId, x.IraiCd, x.SeqNo })
                .ToList();
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
            var updateKensaInfDetailList = kensaInfDetailListInputs.Where(x => kensaInfDetails.Contains(new { x.HpId, x.PtId, x.IraiCd, x.SeqNo }));
            var addKensaInfDetailList = kensaInfDetailListInputs.Where(x => !kensaInfDetails.Contains(new { x.HpId, x.PtId, x.IraiCd, x.SeqNo }));
            if (updateKensaInfDetailList != null && updateKensaInfDetailList.Any())
            {
                TrackingDataContext.KensaInfDetails.UpdateRange(updateKensaInfDetailList);
            }
            if (addKensaInfDetailList != null && addKensaInfDetailList.Any())
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
