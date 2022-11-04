using Domain.Models.SpecialNote;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System.Text;


namespace Infrastructure.Repositories.SpecialNote
{
    public class SpecialNoteRepository : ISpecialNoteRepository
    {
        private readonly TenantDataContext _tenantDataContextTracking;
        private readonly TenantDataContext _tenantDataContextNoTracking;

        public SpecialNoteRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
            _tenantDataContextNoTracking = tenantProvider.GetNoTrackingDataContext();
        }
        public bool SaveSpecialNote(int hpId, long ptId, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, int userId)
        {
            if (!IsInvalidInputId(hpId, ptId)) return false;

            var executionStrategy = _tenantDataContextTracking.Database.CreateExecutionStrategy();

            var result = executionStrategy.Execute(
                () =>
                {
                    // execute your logic here
                    using (var transaction = _tenantDataContextTracking.Database.BeginTransaction())
                    {
                        try
                        {
                            if (summaryInfModel != null && summaryInfModel.Id == hpId && summaryInfModel.PtId == ptId)
                            {
                                SaveSummaryInf(hpId, ptId, summaryInfModel, userId);
                            }
                            if (importantNoteModel != null)
                            {
                                SaveImportantNote(hpId, ptId, importantNoteModel);
                            }
                            if (patientInfoModel != null)
                            {
                                SavePatientInfo(hpId, ptId, patientInfoModel, userId);
                            }
                            _tenantDataContextTracking.SaveChanges();
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
            if (!_tenantDataContextNoTracking.HpInfs.Any(x => x.HpId == hpId)) return false;
            if (!_tenantDataContextNoTracking.PtInfs.Any(x => x.PtId == ptId)) return false;
            return true;
        }
        #region SaveSummaryInf
        private void SaveSummaryInf(int hpId, long ptId, SummaryInfModel summaryInfModel, int userId)
        {
            var summaryInf = _tenantDataContextNoTracking.SummaryInfs.Where(x => x.PtId == ptId && x.HpId == hpId).FirstOrDefault();
            if (summaryInf != null)
            {
                _tenantDataContextTracking.SummaryInfs.Update(new SummaryInf()
                {
                    Id = summaryInfModel.Id,
                    HpId = summaryInfModel.HpId,
                    PtId = summaryInfModel.PtId,
                    SeqNo = summaryInfModel.SeqNo,
                    Text = summaryInfModel.Text,
                    Rtext = Encoding.ASCII.GetBytes(summaryInfModel.Rtext),
                    CreateDate = summaryInf.CreateDate,
                    CreateId = summaryInf.CreateId,
                    CreateMachine = summaryInf.CreateMachine,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId
                });
            }
            else
            {
                _tenantDataContextTracking.SummaryInfs.Add(new SummaryInf()
                {
                    HpId = summaryInfModel.HpId,
                    PtId = summaryInfModel.PtId,
                    SeqNo = summaryInfModel.SeqNo,
                    Text = summaryInfModel.Text,
                    Rtext = Encoding.ASCII.GetBytes(summaryInfModel.Rtext),
                    CreateDate = DateTime.UtcNow,
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
            var alrgyFoodItems = _tenantDataContextNoTracking.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo })
                .ToList();
            var listAlrgyFood = importantNoteModel.AlrgyFoodItems.Where(x => x.HpId == hpId && x.PtId == ptId)
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
            var updateAlrgyFoodList = listAlrgyFood.Where(x => alrgyFoodItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addAlrgyFoodList = listAlrgyFood.Where(x => !alrgyFoodItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateAlrgyFoodList != null && updateAlrgyFoodList.Any())
            {
                _tenantDataContextTracking.PtAlrgyFoods.UpdateRange(updateAlrgyFoodList);
            }
            if (addAlrgyFoodList != null && addAlrgyFoodList.Any())
            {
                _tenantDataContextTracking.PtAlrgyFoods.AddRange(addAlrgyFoodList);
            }
        }
        private void SaveElseItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var alrgyElseItems = _tenantDataContextNoTracking.PtAlrgyElses
                .Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo })
                .ToList();
            var listAlrgyElse = importantNoteModel.AlrgyElseItems.Where(x => x.HpId == hpId && x.PtId == ptId)
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
            var updateAlrgyElseList = listAlrgyElse.Where(x => alrgyElseItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addAlrgyElseList = listAlrgyElse.Where(x => !alrgyElseItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateAlrgyElseList != null && updateAlrgyElseList.Any())
            {
                _tenantDataContextTracking.PtAlrgyElses.UpdateRange(updateAlrgyElseList);
            }
            if (addAlrgyElseList != null && addAlrgyElseList.Any())
            {
                _tenantDataContextTracking.PtAlrgyElses.AddRange(addAlrgyElseList);
            }
        }
        private void SaveDrugItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var alrgyDrugItems = _tenantDataContextNoTracking.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo })
                .ToList();
            var listAlrgyDrug = importantNoteModel.AlrgyDrugItems.Where(x => x.HpId == hpId && x.PtId == ptId)
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
            var updateAlrgyDrugList = listAlrgyDrug.Where(x => alrgyDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addAlrgyDrugList = listAlrgyDrug.Where(x => !alrgyDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateAlrgyDrugList != null && updateAlrgyDrugList.Any())
            {
                _tenantDataContextTracking.PtAlrgyDrugs.UpdateRange(updateAlrgyDrugList);
            }
            if (addAlrgyDrugList != null && addAlrgyDrugList.Any())
            {
                _tenantDataContextTracking.PtAlrgyDrugs.AddRange(addAlrgyDrugList);
            }
        }
        private void SaveKioRekiItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var kioRekiItems = _tenantDataContextNoTracking.PtKioRekis.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
            var listkioReki = importantNoteModel.KioRekiItems.Where(x => x.HpId == hpId && x.PtId == ptId)
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
            var updatekioRekiList = listkioReki.Where(x => kioRekiItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addkioRekiList = listkioReki.Where(x => !kioRekiItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updatekioRekiList != null && updatekioRekiList.Any())
            {
                _tenantDataContextTracking.PtKioRekis.UpdateRange(updatekioRekiList);
            }
            if (addkioRekiList != null && addkioRekiList.Any())
            {
                _tenantDataContextTracking.PtKioRekis.AddRange(addkioRekiList);
            }
        }
        private void SaveInfectionsItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var infectionItems = _tenantDataContextNoTracking.PtInfection.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
            var listInfection = importantNoteModel.InfectionsItems.Where(x => x.HpId == hpId && x.PtId == ptId)
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
            var updateInfectionList = listInfection.Where(x => infectionItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addInfectionList = listInfection.Where(x => !infectionItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateInfectionList != null && updateInfectionList.Any())
            {
                _tenantDataContextTracking.PtInfection.UpdateRange(updateInfectionList);
            }
            if (addInfectionList != null && addInfectionList.Any())
            {
                _tenantDataContextTracking.PtInfection.AddRange(addInfectionList);
            }
        }
        private void SaveOtherDrugItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var otherDrugItems = _tenantDataContextNoTracking.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
            var listOtherDrug = importantNoteModel.OtherDrugItems.Where(x => x.HpId == hpId && x.PtId == ptId)
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
            var updateOtherDrugList = listOtherDrug.Where(x => otherDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addOtherDrugList = listOtherDrug.Where(x => !otherDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateOtherDrugList != null && updateOtherDrugList.Any())
            {
                _tenantDataContextTracking.PtOtherDrug.UpdateRange(updateOtherDrugList);
            }
            if (addOtherDrugList != null && addOtherDrugList.Any())
            {
                _tenantDataContextTracking.PtOtherDrug.AddRange(addOtherDrugList);
            }
        }
        private void SaveOtcDrugItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var otcDrugItems = _tenantDataContextNoTracking.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
            var listOtcDrug = importantNoteModel.OtcDrugItems.Where(x => x.HpId == hpId && x.PtId == ptId)
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
            var updateOtcDrugList = listOtcDrug.Where(x => otcDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addOtcDrugList = listOtcDrug.Where(x => !otcDrugItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updateOtcDrugList != null && updateOtcDrugList.Any())
            {
                _tenantDataContextTracking.PtOtcDrug.UpdateRange(updateOtcDrugList);
            }
            if (addOtcDrugList != null && addOtcDrugList.Any())
            {
                _tenantDataContextTracking.PtOtcDrug.AddRange(addOtcDrugList);
            }
        }
        private void SaveSuppleItems(long hpId, long ptId, ImportantNoteModel importantNoteModel)
        {
            var suppleItems = _tenantDataContextNoTracking.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                .Select(x => new { x.HpId, x.PtId, x.SeqNo }).ToList();
            var listSupple = importantNoteModel.SuppleItems.Where(x => x.HpId == hpId && x.PtId == ptId)
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
            var updatesuppleList = listSupple.Where(x => suppleItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();
            var addSuppleList = listSupple.Where(x => !suppleItems.Contains(new { x.HpId, x.PtId, x.SeqNo })).ToList();

            if (updatesuppleList != null && updatesuppleList.Any())
            {
                _tenantDataContextTracking.PtSupples.UpdateRange(updatesuppleList);
            }
            if (addSuppleList != null && addSuppleList.Any())
            {
                _tenantDataContextTracking.PtSupples.AddRange(addSuppleList);
            }
        }
        #endregion

        #region SavePatientInfo
        private void SavePatientInfo(int hpId, long ptId, PatientInfoModel patientInfoModel, int userId)
        {
            if (patientInfoModel?.PregnancyItems != null && patientInfoModel.PregnancyItems.HpId == hpId && patientInfoModel.PregnancyItems.PtId == ptId)
            {
                SavePregnancyItems(hpId, ptId, patientInfoModel, userId);
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
        private void SavePregnancyItems(int hpId, long ptId, PatientInfoModel patientInfoModel, int userId)
        {
            var pregnancyItems = _tenantDataContextNoTracking.PtPregnancies.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0).FirstOrDefault();
            if (pregnancyItems != null)
            {
                var pregnancyObj = new PtPregnancy()
                {
                    Id = patientInfoModel.PregnancyItems.Id,
                    HpId = patientInfoModel.PregnancyItems.HpId,
                    PtId = patientInfoModel.PregnancyItems.PtId,
                    SeqNo = patientInfoModel.PregnancyItems.SeqNo,
                    StartDate = patientInfoModel.PregnancyItems.StartDate,
                    EndDate = patientInfoModel.PregnancyItems.EndDate,
                    PeriodDate = patientInfoModel.PregnancyItems.PeriodDate,
                    PeriodDueDate = patientInfoModel.PregnancyItems.PeriodDueDate,
                    OvulationDate = patientInfoModel.PregnancyItems.OvulationDate,
                    OvulationDueDate = patientInfoModel.PregnancyItems.OvulationDueDate,
                    IsDeleted = patientInfoModel.PregnancyItems.IsDeleted,
                    CreateDate = pregnancyItems.CreateDate,
                    CreateId = pregnancyItems.CreateId,
                    CreateMachine = pregnancyItems.CreateMachine,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId
                };
                _tenantDataContextTracking.PtPregnancies.Update(pregnancyObj);
            }
            else
            {
                var pregnancyObj = new PtPregnancy()
                {
                    HpId = patientInfoModel.PregnancyItems.HpId,
                    PtId = patientInfoModel.PregnancyItems.PtId,
                    SeqNo = patientInfoModel.PregnancyItems.SeqNo,
                    StartDate = patientInfoModel.PregnancyItems.StartDate,
                    EndDate = patientInfoModel.PregnancyItems.EndDate,
                    PeriodDate = patientInfoModel.PregnancyItems.PeriodDate,
                    PeriodDueDate = patientInfoModel.PregnancyItems.PeriodDueDate,
                    OvulationDate = patientInfoModel.PregnancyItems.OvulationDate,
                    OvulationDueDate = patientInfoModel.PregnancyItems.OvulationDueDate,
                    IsDeleted = patientInfoModel.PregnancyItems.IsDeleted,
                    CreateDate = DateTime.UtcNow,
                    CreateId = userId
                };
                _tenantDataContextTracking.PtPregnancies.Add(pregnancyObj);
            }
        }
        private void SavePtCmtInfItems(int hpId, long ptId, PatientInfoModel patientInfoModel, int userId)
        {
            var ptCmtInfItems = _tenantDataContextNoTracking.PtCmtInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0).FirstOrDefault();
            if (ptCmtInfItems != null)
            {
                var PtCmtInfObj = new PtCmtInf()
                {
                    Id = ptCmtInfItems.Id,
                    HpId = patientInfoModel.PtCmtInfItems.HpId,
                    PtId = patientInfoModel.PtCmtInfItems.PtId,
                    SeqNo = patientInfoModel.PtCmtInfItems.SeqNo,
                    Text = patientInfoModel.PtCmtInfItems.Text,
                    IsDeleted = patientInfoModel.PtCmtInfItems.IsDeleted,
                    CreateDate = ptCmtInfItems.CreateDate,
                    CreateId = ptCmtInfItems.CreateId,
                    CreateMachine = ptCmtInfItems.CreateMachine,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId
                };
                _tenantDataContextTracking.PtCmtInfs.Update(PtCmtInfObj);
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
                    CreateDate = DateTime.UtcNow,
                    CreateId = userId
                };
                _tenantDataContextTracking.PtCmtInfs.Add(PtCmtInfObj);
            }
        }
        private void SaveSeikatureInfItems(int hpId, long ptId, PatientInfoModel patientInfoModel, int userId)
        {
            var SeikatureInfItems = _tenantDataContextNoTracking.SeikaturekiInfs.Where(x => x.HpId == hpId && x.PtId == ptId).FirstOrDefault();
            if (SeikatureInfItems != null)
            {
                var SeikatureInfObj = new SeikaturekiInf()
                {
                    Id = SeikatureInfItems.Id,
                    HpId = patientInfoModel.SeikatureInfItems.HpId,
                    PtId = patientInfoModel.SeikatureInfItems.PtId,
                    SeqNo = patientInfoModel.SeikatureInfItems.SeqNo,
                    Text = patientInfoModel.SeikatureInfItems.Text,
                    CreateDate = SeikatureInfItems.CreateDate,
                    CreateId = SeikatureInfItems.CreateId,
                    CreateMachine = SeikatureInfItems.CreateMachine,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId
                };
                _tenantDataContextTracking.SeikaturekiInfs.Update(SeikatureInfObj);
            }
            else
            {
                var SeikatureInfObj = new SeikaturekiInf()
                {
                    HpId = patientInfoModel.SeikatureInfItems.HpId,
                    PtId = patientInfoModel.SeikatureInfItems.PtId,
                    SeqNo = patientInfoModel.SeikatureInfItems.SeqNo,
                    Text = patientInfoModel.SeikatureInfItems.Text,
                    CreateDate = DateTime.UtcNow,
                    CreateId = userId
                };
                _tenantDataContextTracking.SeikaturekiInfs.Add(SeikatureInfObj);
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
            var kensaInfDetails = _tenantDataContextNoTracking.KensaInfDetails
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
                _tenantDataContextTracking.KensaInfDetails.UpdateRange(updateKensaInfDetailList);
            }
            if (addKensaInfDetailList != null && addKensaInfDetailList.Any())
            {
                _tenantDataContextTracking.KensaInfDetails.AddRange(addKensaInfDetailList);
            }
        }

        #endregion
    }
}
