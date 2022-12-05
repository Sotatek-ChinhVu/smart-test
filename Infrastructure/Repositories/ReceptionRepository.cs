using Domain.Constant;
using Domain.Models.Reception;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Infrastructure.Repositories
{
    public class ReceptionRepository : IReceptionRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;
        public ReceptionRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public ReceptionModel Get(long raiinNo)
        {
            var receptionEntity = _tenantNoTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.RaiinNo == raiinNo);
            var raiinCommentInf = _tenantNoTrackingDataContext.RaiinCmtInfs.FirstOrDefault(r => r.RaiinNo == raiinNo);

            return new ReceptionModel
                (
                    receptionEntity?.HpId ?? 0,
                    receptionEntity?.PtId ?? 0,
                    receptionEntity?.SinDate ?? 0,
                    receptionEntity?.RaiinNo ?? 0,
                    receptionEntity?.OyaRaiinNo ?? 0,
                    receptionEntity?.HokenPid ?? 0,
                    receptionEntity?.SanteiKbn ?? 0,
                    receptionEntity?.Status ?? 0,
                    receptionEntity?.IsYoyaku ?? 0,
                    receptionEntity?.YoyakuTime ?? string.Empty,
                    receptionEntity?.YoyakuId ?? 0,
                    receptionEntity?.UketukeSbt ?? 0,
                    receptionEntity?.UketukeTime ?? string.Empty,
                    receptionEntity?.UketukeId ?? 0,
                    receptionEntity?.UketukeNo ?? 0,
                    receptionEntity?.SinStartTime ?? string.Empty,
                    receptionEntity?.SinEndTime ?? string.Empty,
                    receptionEntity?.KaikeiTime ?? string.Empty,
                    receptionEntity?.KaikeiId ?? 0,
                    receptionEntity?.KaId ?? 0,
                    receptionEntity?.TantoId ?? 0,
                    receptionEntity?.SyosaisinKbn ?? 0,
                    receptionEntity?.JikanKbn ?? 0,
                    raiinCommentInf?.Text ?? string.Empty
                );
        }

        public long Insert(ReceptionSaveDto dto, int hpId, int userId)
        {
            var executionStrategy = _tenantTrackingDataContext.Database.CreateExecutionStrategy();
            return executionStrategy.Execute(() =>
            {
                using var transaction = _tenantTrackingDataContext.Database.BeginTransaction();

                // Insert RaiinInf
                var raiinInf = CreateNewRaiinInf(dto.Reception, hpId, userId);
                _tenantTrackingDataContext.RaiinInfs.Add(raiinInf);
                _tenantTrackingDataContext.SaveChanges();

                if (raiinInf.OyaRaiinNo == 0)
                {
                    raiinInf.OyaRaiinNo = raiinInf.RaiinNo;
                }

                // Insert RaiinCmtInf
                if (!string.IsNullOrWhiteSpace(dto.ReceptionComment))
                {
                    var raiinCmtInf = CreateNewRaiinCmtInf(raiinInf, dto.ReceptionComment, hpId, userId);
                    _tenantTrackingDataContext.RaiinCmtInfs.Add(raiinCmtInf);
                }

                // Insert RaiinKbnInfs
                var raiinKbnInfs = dto.KubunInfs
                    .Where(model => model.KbnCd != CommonConstants.KbnCdDeleteFlag)
                    .Select(dto => CreateNewRaiinKbnInf(dto, raiinInf, hpId, userId));
                _tenantTrackingDataContext.RaiinKbnInfs.AddRange(raiinKbnInfs);

                // Update insurances and diseases
                SaveInsuraceConfirmationHistories(dto.Insurances, raiinInf.PtId, hpId, userId);
                UpdateDiseaseTenkis(dto.Diseases, raiinInf.PtId, hpId, userId);
                _tenantTrackingDataContext.SaveChanges();

                transaction.Commit();
                return raiinInf.RaiinNo;
            });

            #region Helper methods

            RaiinInf CreateNewRaiinInf(ReceptionModel model, int hpId, int userId)
            {
                return new RaiinInf
                {
                    HpId = hpId,
                    PtId = model.PtId,
                    SinDate = model.SinDate,
                    OyaRaiinNo = model.OyaRaiinNo,
                    HokenPid = model.HokenPid,
                    SanteiKbn = model.SanteiKbn,
                    Status = model.Status,
                    IsYoyaku = model.IsYoyaku,
                    YoyakuTime = model.YoyakuTime,
                    YoyakuId = model.YoyakuId,
                    UketukeSbt = model.UketukeSbt,
                    UketukeTime = model.UketukeTime,
                    UketukeId = model.UketukeId,
                    UketukeNo = model.UketukeNo,
                    SinStartTime = model.SinStartTime,
                    SinEndTime = model.SinEndTime,
                    KaikeiTime = model.KaikeiTime,
                    KaikeiId = model.KaikeiId,
                    KaId = model.KaId,
                    TantoId = model.TantoId,
                    SyosaisinKbn = model.SyosaisinKbn,
                    JikanKbn = model.JikanKbn,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    CreateId = userId
                };
            }

            RaiinCmtInf CreateNewRaiinCmtInf(RaiinInf raiinInf, string text, int hpId, int userId)
            {
                return new RaiinCmtInf
                {
                    HpId = hpId,
                    PtId = raiinInf.PtId,
                    SinDate = raiinInf.SinDate,
                    RaiinNo = raiinInf.RaiinNo,
                    CmtKbn = CmtKbns.Comment,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    Text = text,
                    CreateDate = DateTime.UtcNow,
                    CreateId = userId
                };
            }

            RaiinKbnInf CreateNewRaiinKbnInf(RaiinKbnInfDto dto, RaiinInf raiinInf, int hpId, int userId)
            {
                return new RaiinKbnInf
                {
                    HpId = hpId,
                    PtId = raiinInf.PtId,
                    SinDate = raiinInf.SinDate,
                    RaiinNo = raiinInf.RaiinNo,
                    UpdateDate = DateTime.UtcNow,
                    UpdateId = userId,
                    GrpId = dto.GrpId,
                    KbnCd = dto.KbnCd,
                    CreateDate = DateTime.UtcNow,
                    CreateId = userId
                };
            }

            #endregion
        }

        public int GetMaxUketukeNo(int hpId, int sindate, int infKbn, int kaId, int uketukeMode)
        {
            var query = _tenantNoTrackingDataContext.RaiinInfs.Where
                (
                    p => p.HpId == hpId && p.SinDate == sindate
                    && (!(uketukeMode == 1 || uketukeMode == 3) || p.UketukeSbt == infKbn)
                    && (!(uketukeMode == 2 || uketukeMode == 3) || p.KaId == kaId)
                    && p.IsDeleted == DeleteTypes.None
                ).OrderByDescending(p => p.UketukeNo).FirstOrDefault();
            if (query != null)
            {
                return query.UketukeNo;
            }
            return 0;
        }

        public bool Update(ReceptionSaveDto dto, int hpId, int userId)
        {
            var raiinInf = _tenantTrackingDataContext.RaiinInfs
                .FirstOrDefault(r => r.HpId == hpId
                    && r.PtId == dto.Reception.PtId
                    && r.SinDate == dto.Reception.SinDate
                    && r.RaiinNo == dto.Reception.RaiinNo
                    && r.IsDeleted == DeleteTypes.None);
            if (raiinInf is null)
            {
                return false;
            }

            UpdateRaiinInfIfChanged(raiinInf, dto.Reception);
            UpsertRaiinCmtInf(raiinInf, dto.ReceptionComment);
            SaveRaiinKbnInfs(raiinInf, dto.KubunInfs);
            // Update insurances and diseases
            SaveInsuraceConfirmationHistories(dto.Insurances, raiinInf.PtId, hpId, userId);
            UpdateDiseaseTenkis(dto.Diseases, raiinInf.PtId, hpId, userId);

            _tenantTrackingDataContext.SaveChanges();
            return true;

            #region Helper methods

            void UpdateRaiinInfIfChanged(RaiinInf entity, ReceptionModel model)
            {
                // Detect changes
                if (entity.OyaRaiinNo != model.OyaRaiinNo ||
                    entity.KaId != model.KaId ||
                    entity.UketukeSbt != model.UketukeSbt ||
                    entity.UketukeNo != model.UketukeNo ||
                    entity.TantoId != model.TantoId ||
                    entity.SyosaisinKbn != model.SyosaisinKbn ||
                    entity.JikanKbn != model.JikanKbn ||
                    entity.SanteiKbn != model.SanteiKbn ||
                    entity.HokenPid != model.HokenPid)
                {
                    entity.OyaRaiinNo = model.OyaRaiinNo;
                    entity.KaId = model.KaId;
                    entity.UketukeSbt = model.UketukeSbt;
                    entity.UketukeNo = model.UketukeNo;
                    entity.TantoId = model.TantoId;
                    entity.HokenPid = model.HokenPid;
                    entity.SyosaisinKbn = model.SyosaisinKbn;
                    entity.JikanKbn = model.JikanKbn;
                    entity.SanteiKbn = model.SanteiKbn;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.UpdateId = userId;
                }
            }

            void UpsertRaiinCmtInf(RaiinInf raiinInf, string text)
            {
                var raiinCmtInf = _tenantTrackingDataContext.RaiinCmtInfs
                   .FirstOrDefault(x => x.HpId == hpId
                        && x.RaiinNo == raiinInf.RaiinNo
                        && x.CmtKbn == CmtKbns.Comment
                        && x.IsDelete == DeleteTypes.None);
                if (raiinCmtInf is null)
                {
                    _tenantTrackingDataContext.RaiinCmtInfs.Add(new RaiinCmtInf
                    {
                        HpId = hpId,
                        PtId = raiinInf.PtId,
                        SinDate = raiinInf.SinDate,
                        RaiinNo = raiinInf.RaiinNo,
                        CmtKbn = CmtKbns.Comment,
                        Text = text,
                        CreateDate = DateTime.UtcNow,
                        CreateId = userId,
                        UpdateDate = DateTime.UtcNow,
                        UpdateId = userId
                    });
                }
                else if (raiinCmtInf.Text != text)
                {
                    raiinCmtInf.Text = text;
                    raiinCmtInf.UpdateDate = DateTime.UtcNow;
                    raiinCmtInf.UpdateId = userId;
                }
            }

            void SaveRaiinKbnInfs(RaiinInf raiinInf, IEnumerable<RaiinKbnInfDto> kbnInfDtos)
            {
                var existingEntities = _tenantTrackingDataContext.RaiinKbnInfs
                    .Where(x => x.HpId == hpId
                        && x.PtId == raiinInf.PtId
                        && x.SinDate == raiinInf.SinDate
                        && x.RaiinNo == raiinInf.RaiinNo
                        && x.IsDelete == DeleteTypes.None)
                    .ToList();

                foreach (var kbnInfDto in kbnInfDtos)
                {
                    var existingEntity = existingEntities.Find(x => x.GrpId == kbnInfDto.GrpId);
                    if (kbnInfDto.KbnCd == CommonConstants.KbnCdDeleteFlag)
                    {
                        if (existingEntity is not null)
                        {
                            // Soft-delete
                            existingEntity.IsDelete = DeleteTypes.Deleted;
                        }
                    }
                    else
                    {
                        if (existingEntity is null)
                        {
                            // Insert
                            _tenantTrackingDataContext.RaiinKbnInfs.Add(new RaiinKbnInf
                            {
                                HpId = hpId,
                                PtId = raiinInf.PtId,
                                SinDate = raiinInf.SinDate,
                                RaiinNo = raiinInf.RaiinNo,
                                GrpId = kbnInfDto.GrpId,
                                KbnCd = kbnInfDto.KbnCd,
                                CreateDate = DateTime.UtcNow,
                                UpdateDate = DateTime.UtcNow,
                                UpdateId = userId,
                                CreateId = userId
                            });
                        }
                        else if (existingEntity.KbnCd != kbnInfDto.KbnCd)
                        {
                            // Update
                            existingEntity.KbnCd = kbnInfDto.KbnCd;
                            existingEntity.UpdateDate = DateTime.UtcNow;
                            existingEntity.UpdateId = userId;
                        }
                    }
                }
            }

            #endregion
        }

        private void SaveInsuraceConfirmationHistories(IEnumerable<InsuranceDto> insurances, long ptId, int hpId, int userId)
        {
            if (insurances.Any())
            {
                List<PtHokenCheck> listHokenCheckAddNew = new();

                var hokenIds = insurances.Select(i => i.HokenId).Distinct();
                var oldHokenCheckDB = _tenantTrackingDataContext.PtHokenChecks
                                            .Where(item =>
                                                            hokenIds.Contains(item.HokenId)
                                                            && item.HpId == hpId
                                                            && item.PtID == ptId
                                                            && item.IsDeleted == 0
                                            ).ToList();

                foreach (var insuranceItem in insurances)
                {
                    var hokenGrp = insuranceItem.IsHokenGroupKohi ? HokenGroupConstant.HokenGroupKohi : HokenGroupConstant.HokenGroupHokenPattern;
                    var listCheckTime = oldHokenCheckDB.Where(item => 
                                                                    item.HokenId == insuranceItem.HokenId
                                                                    && item.HokenGrp == hokenGrp)
                                                        .OrderByDescending(item => item.CheckDate)
                                                        .ToList();

                    var listHokenCheckInsertInput = insuranceItem.ConfirmDateList.Where(item => item.SeqNo == 0).ToList();
                    var listHokenCheckUpdateInput = insuranceItem.ConfirmDateList.Where(item => item.SeqNo != 0).ToList();

                    // Update PtHokenCheck
                    var listUpdateItemDB = _tenantTrackingDataContext.PtHokenChecks
                                 .Where(item =>
                                             listHokenCheckUpdateInput.Select(item => item.SeqNo).Contains(item.SeqNo)
                                             && item.HpId == hpId
                                             && item.PtID == ptId
                                             && item.HokenId == insuranceItem.HokenId
                                             && item.HokenGrp == hokenGrp
                                             && item.IsDeleted == 0)
                                 .ToList();

                    // Update PtHokenCheck
                    foreach (var update in listHokenCheckUpdateInput)
                    {
                        var checkDatetimeInput = DateTime.ParseExact(update.SinDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToUniversalTime();
                        var hokenCheckItem = listUpdateItemDB.FirstOrDefault(item => item.SeqNo == update.SeqNo);
                        if (hokenCheckItem != null)
                        {
                            hokenCheckItem.UpdateDate = DateTime.UtcNow;
                            hokenCheckItem.UpdateId = userId;

                            /// <summary>
                            /// Update check date:
                            /// If checkDate of item in database is not equal checkDate input,
                            /// and checkDate input not equal other checkDate in database
                            /// then update checkDate
                            /// </summary>
                            if (!hokenCheckItem.CheckDate.ToString("yyyyMMdd").Equals(update.SinDate.ToString())
                                && !listCheckTime.Select(item => item.CheckDate.ToString("yyyyMMdd")).ToList().Contains(update.SinDate.ToString()))
                            {
                                hokenCheckItem.CheckDate = checkDatetimeInput;
                                var removeItem = listCheckTime.FirstOrDefault(item => item.SeqNo == update.SeqNo);
                                if (removeItem != null)
                                {
                                    listCheckTime.Remove(removeItem);
                                }
                            }

                            // update isDelete
                            if (update.IsDelete)
                            {
                                hokenCheckItem.IsDeleted = 1;
                                var removeItem = listCheckTime.FirstOrDefault(item => item.SeqNo == update.SeqNo);
                                if (removeItem != null)
                                {
                                    listCheckTime.Remove(removeItem);
                                }
                            }
                            else
                            {
                                hokenCheckItem.CheckCmt = update.Comment;
                                hokenCheckItem.CheckId = userId;
                            }
                        }
                    }

                    // Add new PtHokenCheck
                    foreach (var item in listHokenCheckInsertInput)
                    {
                        var checkDatetime = DateTime.ParseExact(item.SinDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture).ToUniversalTime();
                        if (listCheckTime == null || !listCheckTime.Select(item => item.CheckDate.ToString("yyyyMMdd")).ToList().Contains(item.SinDate.ToString()))
                        {
                            listHokenCheckAddNew.Add(new PtHokenCheck
                            {
                                HpId = hpId,
                                PtID = ptId,
                                HokenGrp = hokenGrp,
                                HokenId = insuranceItem.HokenId,
                                CheckDate = checkDatetime,
                                CheckCmt = item.Comment,
                                CheckId = userId,
                                CreateDate = DateTime.UtcNow,
                                CreateId = userId,
                                UpdateDate = DateTime.UtcNow,
                                UpdateId = userId
                            });
                        }
                    }
                }
                _tenantTrackingDataContext.PtHokenChecks.AddRange(listHokenCheckAddNew);
            }
        }

        private void UpdateDiseaseTenkis(IEnumerable<DiseaseDto> diseases, long ptId, int hpId, int userId)
        {
            var ptByomeiIds = diseases.Select(d => d.Id);
            var ptByomeis = _tenantTrackingDataContext.PtByomeis.AsTracking()
                .Where(x => x.HpId == hpId && x.PtId == ptId && ptByomeiIds.Contains(x.Id))
                .ToList();

            foreach (var disease in diseases)
            {
                var ptByomei = ptByomeis.Find(x => x.Id == disease.Id);
                if (ptByomei is not null
                    && (ptByomei.TenkiKbn != disease.TenkiKbn || ptByomei.TenkiDate != disease.TenkiDate))
                {
                    ptByomei.TenkiKbn = disease.TenkiKbn;
                    ptByomei.TenkiDate = disease.TenkiDate;
                    ptByomei.UpdateDate = DateTime.UtcNow;
                    ptByomei.UpdateId = userId;
                }
            }
        }

        public List<ReceptionRowModel> GetList(int hpId, int sinDate, long raiinNo, long ptId, [Optional] bool isGetAccountDue)
        {
            return GetReceptionRowModels(hpId, sinDate, raiinNo, ptId, isGetAccountDue);
        }

        public IEnumerable<ReceptionModel> GetList(int hpId, long ptId, int karteDeleteHistory)
        {
            var result = _tenantNoTrackingDataContext.RaiinInfs.Where
                                (r =>
                                    r.HpId == hpId && r.PtId == ptId && r.Status >= 3 &&
                                 (r.IsDeleted == DeleteTypes.None || karteDeleteHistory == 1 || (r.IsDeleted != DeleteTypes.Confirm && karteDeleteHistory == 2)));
            return result.Select(r => new ReceptionModel(
                        r.HpId,
                        r.PtId,
                        r.SinDate,
                        r.RaiinNo,
                        r.OyaRaiinNo,
                        r.HokenPid,
                        r.SanteiKbn,
                        r.Status,
                        r.IsYoyaku,
                        r.YoyakuTime ?? String.Empty,
                        r.YoyakuId,
                        r.UketukeSbt,
                        r.UketukeTime ?? String.Empty,
                        r.UketukeId,
                        r.UketukeNo,
                        r.SinStartTime ?? string.Empty,
                        r.SinEndTime ?? String.Empty,
                        r.KaikeiTime ?? String.Empty,
                        r.KaikeiId,
                        r.KaId,
                        r.TantoId,
                        r.SyosaisinKbn,
                        r.JikanKbn,
                        string.Empty
                   ));

        }

        public List<ReceptionModel> GetLastRaiinInfs(int hpId, long ptId, int sinDate)
        {
            var result = _tenantNoTrackingDataContext.RaiinInfs.Where(p =>
                                                                        p.HpId == hpId
                                                                        && p.PtId == ptId
                                                                        && p.IsDeleted == DeleteTypes.None
                                                                        && p.SinDate < sinDate && p.Status >= RaiinState.TempSave);
            return result.Select(r => new ReceptionModel(
                    r.HpId,
                    r.PtId,
                    r.SinDate,
                    r.RaiinNo,
                    r.OyaRaiinNo,
                    r.HokenPid,
                    r.SanteiKbn,
                    r.Status,
                    r.IsYoyaku,
                    r.YoyakuTime ?? String.Empty,
                    r.YoyakuId,
                    r.UketukeSbt,
                    r.UketukeTime ?? String.Empty,
                    r.UketukeId,
                    r.UketukeNo,
                    r.SinStartTime ?? string.Empty,
                    r.SinEndTime ?? String.Empty,
                    r.KaikeiTime ?? String.Empty,
                    r.KaikeiId,
                    r.KaId,
                    r.TantoId,
                    r.SyosaisinKbn,
                    r.JikanKbn,
                    string.Empty
               )).ToList();
        }

        public IEnumerable<ReceptionModel> GetList(int hpId, long ptId, List<long> raiinNos)
        {
            var result = _tenantNoTrackingDataContext.RaiinInfs.Where
                                (r =>
                                    r.HpId == hpId && r.PtId == ptId && r.Status >= 3 && r.IsDeleted == 0 && raiinNos.Contains(r.RaiinNo));
            return result.Select(r => new ReceptionModel(
                        r.HpId,
                        r.PtId,
                        r.SinDate,
                        r.RaiinNo,
                        r.OyaRaiinNo,
                        r.HokenPid,
                        r.SanteiKbn,
                        r.Status,
                        r.IsYoyaku,
                        r.YoyakuTime ?? String.Empty,
                        r.YoyakuId,
                        r.UketukeSbt,
                        r.UketukeTime ?? String.Empty,
                        r.UketukeId,
                        r.UketukeNo,
                        r.SinStartTime ?? String.Empty,
                        r.SinEndTime ?? String.Empty,
                        r.KaikeiTime ?? String.Empty,
                        r.KaikeiId,
                        r.KaId,
                        r.TantoId,
                        r.SyosaisinKbn,
                        r.JikanKbn,
                        string.Empty
                   ));

        }

        public bool CheckListNo(List<long> raininNos)
        {
            var check = _tenantNoTrackingDataContext.RaiinInfs.Any(r => raininNos.Contains(r.RaiinNo) && r.IsDeleted != 1);
            return check;
        }

        private List<ReceptionRowModel> GetReceptionRowModels(int hpId, int sinDate, long raiinNo, long ptId, bool isGetAccountDue)
        {
            // 1. Prepare all the necessary collections for the join operation
            // Raiin (Reception)
            var raiinInfs = _tenantNoTrackingDataContext.RaiinInfs.Where(x => x.IsDeleted == DeleteTypes.None);
            var raiinCmtInfs = _tenantNoTrackingDataContext.RaiinCmtInfs.Where(x => x.IsDelete == DeleteTypes.None);
            var raiinKbnInfs = _tenantNoTrackingDataContext.RaiinKbnInfs.Where(x => x.IsDelete == DeleteTypes.None);
            var raiinKbnDetails = _tenantNoTrackingDataContext.RaiinKbnDetails.Where(x => x.IsDeleted == DeleteTypes.None);
            // Pt (Patient)
            var ptInfs = _tenantNoTrackingDataContext.PtInfs.Where(x => x.IsDelete == DeleteTypes.None);
            var ptCmtInfs = _tenantNoTrackingDataContext.PtCmtInfs.Where(x => x.IsDeleted == DeleteTypes.None);
            var ptHokenPatterns = _tenantNoTrackingDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteTypes.None);
            var ptKohis = _tenantNoTrackingDataContext.PtKohis.Where(x => x.IsDeleted == DeleteTypes.None);
            // Rsv (Reservation)
            var rsvInfs = _tenantNoTrackingDataContext.RsvInfs;
            var rsvFrameMsts = _tenantNoTrackingDataContext.RsvFrameMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // User (Doctor)
            var userMsts = _tenantNoTrackingDataContext.UserMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // Ka (Department)
            var kaMsts = _tenantNoTrackingDataContext.KaMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // Lock (Function lock)
            var lockInfs = _tenantNoTrackingDataContext.LockInfs.Where(x =>
                x.FunctionCd == FunctionCode.MedicalExaminationCode || x.FunctionCd == FunctionCode.TeamKarte);
            // Uketuke
            var uketukeSbtMsts = _tenantNoTrackingDataContext.UketukeSbtMsts.Where(x => x.IsDeleted == DeleteTypes.None);

            // 2. Filter collections by parameters
            var filteredRaiinInfs = raiinInfs;
            if (!isGetAccountDue)
            {
                filteredRaiinInfs = filteredRaiinInfs.Where(x => x.HpId == hpId && x.SinDate == sinDate);
            }
            if (raiinNo != CommonConstants.InvalidId)
            {
                filteredRaiinInfs = filteredRaiinInfs.Where(x => x.RaiinNo == raiinNo);
            }

            var filteredPtInfs = ptInfs;
            if (ptId != CommonConstants.InvalidId)
            {
                filteredPtInfs = filteredPtInfs.Where(x => x.PtId == ptId);
            }

            // 3. Perform the join operation
            var raiinQuery =
                from raiinInf in filteredRaiinInfs
                join ptInf in filteredPtInfs on
                    new { raiinInf.HpId, raiinInf.PtId } equals
                    new { ptInf.HpId, ptInf.PtId }
                join raiinCmtInfComment in raiinCmtInfs.Where(x => x.CmtKbn == CmtKbns.Comment) on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo } equals
                    new { raiinCmtInfComment.HpId, raiinCmtInfComment.PtId, raiinCmtInfComment.SinDate, raiinCmtInfComment.RaiinNo } into relatedRaiinCmtInfComments
                from relatedRaiinCmtInfComment in relatedRaiinCmtInfComments.DefaultIfEmpty()
                join raiinCmtInfRemark in raiinCmtInfs.Where(x => x.CmtKbn == CmtKbns.Remark) on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo } equals
                    new { raiinCmtInfRemark.HpId, raiinCmtInfRemark.PtId, raiinCmtInfRemark.SinDate, raiinCmtInfRemark.RaiinNo } into relatedRaiinCmtInfRemarks
                from relatedRaiinCmtInfRemark in relatedRaiinCmtInfRemarks.DefaultIfEmpty()
                from ptCmtInf in ptCmtInfs.Where(x => x.PtId == ptInf.PtId).OrderByDescending(x => x.UpdateDate).Take(1).DefaultIfEmpty()
                join rsvInf in rsvInfs on raiinInf.RaiinNo equals rsvInf.RaiinNo into relatedRsvInfs
                from relatedRsvInf in relatedRsvInfs.DefaultIfEmpty()
                join rsvFrameMst in rsvFrameMsts on relatedRsvInf.RsvFrameId equals rsvFrameMst.RsvFrameId into relatedRsvFrameMsts
                from relatedRsvFrameMst in relatedRsvFrameMsts.DefaultIfEmpty()
                join lockInf in lockInfs on raiinInf.RaiinNo equals lockInf.RaiinNo into relatedLockInfs
                from relatedLockInf in relatedLockInfs.DefaultIfEmpty()
                join uketukeSbtMst in uketukeSbtMsts on raiinInf.UketukeSbt equals uketukeSbtMst.KbnId into relatedUketukeSbtMsts
                from relatedUketukeSbtMst in relatedUketukeSbtMsts.DefaultIfEmpty()
                join tanto in userMsts on
                    new { raiinInf.HpId, UserId = raiinInf.TantoId } equals
                    new { tanto.HpId, tanto.UserId } into relatedTantos
                from relatedTanto in relatedTantos.DefaultIfEmpty()
                join primaryDoctor in userMsts on
                    new { raiinInf.HpId, UserId = ptInf.PrimaryDoctor } equals
                    new { primaryDoctor.HpId, primaryDoctor.UserId } into relatedPrimaryDoctors
                from relatedPrimaryDoctor in relatedPrimaryDoctors.DefaultIfEmpty()
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into relatedKaMsts
                from relatedKaMst in relatedKaMsts.DefaultIfEmpty()
                join ptHokenPattern in ptHokenPatterns on
                    new { raiinInf.HpId, raiinInf.PtId, raiinInf.HokenPid } equals
                    new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenPid } into relatedPtHokenPatterns
                from relatedPtHokenPattern in relatedPtHokenPatterns.DefaultIfEmpty()
                from ptKohi1 in ptKohis.Where(x => x.PtId == ptInf.PtId && x.HokenId == relatedPtHokenPattern.Kohi1Id).Take(1).DefaultIfEmpty()
                from ptKohi2 in ptKohis.Where(x => x.PtId == ptInf.PtId && x.HokenId == relatedPtHokenPattern.Kohi2Id).Take(1).DefaultIfEmpty()
                from ptKohi3 in ptKohis.Where(x => x.PtId == ptInf.PtId && x.HokenId == relatedPtHokenPattern.Kohi3Id).Take(1).DefaultIfEmpty()
                from ptKohi4 in ptKohis.Where(x => x.PtId == ptInf.PtId && x.HokenId == relatedPtHokenPattern.Kohi4Id).Take(1).DefaultIfEmpty()
                select new
                {
                    raiinInf,
                    ptInf,
                    ptCmtInf,
                    primaryDoctorName = relatedPrimaryDoctor.Sname,
                    relatedUketukeSbtMst,
                    relatedTanto,
                    relatedKaMst,
                    relatedRaiinCmtInfComment,
                    relatedRaiinCmtInfRemark,
                    relatedRsvFrameMst,
                    relatedLockInf,
                    relatedPtHokenPattern,
                    ptKohi1,
                    ptKohi2,
                    ptKohi3,
                    ptKohi4,
                    raiinKbnDetails = (
                        from inf in raiinKbnInfs
                        join detail in raiinKbnDetails on
                            new { inf.HpId, inf.GrpId, inf.KbnCd } equals
                            new { detail.HpId, GrpId = detail.GrpCd, detail.KbnCd }
                        where inf.HpId == hpId
                            && inf.PtId == raiinInf.PtId
                            && inf.SinDate == sinDate
                            && inf.RaiinNo == raiinInf.RaiinNo
                        select detail
                    ).ToList(),
                    parentRaiinNo = (
                        from r in raiinInfs
                        where r.HpId == hpId
                            && r.PtId == raiinInf.PtId
                            && r.SinDate == raiinInf.SinDate
                            && r.OyaRaiinNo == raiinInf.OyaRaiinNo
                            && r.RaiinNo != r.OyaRaiinNo
                        select r.OyaRaiinNo
                    ).FirstOrDefault(),
                    lastVisitDate = (
                        from x in raiinInfs
                        where x.HpId == hpId
                            && x.PtId == raiinInf.PtId
                            && x.SinDate < sinDate
                            && x.Status >= RaiinState.TempSave
                        orderby x.SinDate descending
                        select x.SinDate
                    ).FirstOrDefault(),
                    firstVisitDate = (
                        from x in raiinInfs
                        where x.HpId == hpId
                            && x.PtId == raiinInf.PtId
                            && x.SinDate < sinDate
                            && x.Status >= RaiinState.TempSave
                            && x.SyosaisinKbn == SyosaiConst.Syosin
                        orderby x.SinDate descending
                        select x.SinDate
                    ).FirstOrDefault()
                };

            var raiins = raiinQuery.ToList();
            var grpIds = _tenantNoTrackingDataContext.RaiinKbnMsts.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None).Select(x => x.GrpCd).ToList();
            var models = raiins.Select(r => new ReceptionRowModel(
                r.raiinInf.RaiinNo,
                r.raiinInf.PtId,
                r.parentRaiinNo,
                r.raiinInf.UketukeNo,
                r.relatedLockInf is not null,
                r.raiinInf.Status,
                r.ptInf.PtNum,
                r.ptInf.KanaName,
                r.ptInf.Name,
                r.ptInf.Sex,
                r.ptInf.Birthday,
                r.raiinInf.YoyakuTime ?? string.Empty,
                r.relatedRsvFrameMst?.RsvFrameName ?? string.Empty,
                r.relatedUketukeSbtMst?.KbnId ?? CommonConstants.InvalidId,
                r.raiinInf.UketukeTime ?? string.Empty,
                r.raiinInf.SinStartTime ?? string.Empty,
                r.raiinInf.SinEndTime ?? string.Empty,
                r.raiinInf.KaikeiTime ?? string.Empty,
                r.relatedRaiinCmtInfComment?.Text ?? string.Empty,
                r.ptCmtInf?.Text ?? string.Empty,
                r.relatedTanto?.UserId ?? CommonConstants.InvalidId,
                r.relatedKaMst?.KaId ?? CommonConstants.InvalidId,
                r.lastVisitDate,
                r.firstVisitDate,
                r.primaryDoctorName ?? string.Empty,
                r.relatedRaiinCmtInfRemark?.Text ?? string.Empty,
                r.raiinInf.ConfirmationState,
                r.raiinInf.ConfirmationResult ?? string.Empty,
                grpIds,
                dynamicCells: r.raiinKbnDetails.Select(d => new DynamicCell(d.GrpCd, d.KbnCd, d.KbnName, d.ColorCd ?? string.Empty)).ToList(),
                sinDate,
                // Fields needed to create Hoken name
                r.relatedPtHokenPattern?.HokenPid ?? CommonConstants.InvalidId,
                r.relatedPtHokenPattern?.StartDate ?? 0,
                r.relatedPtHokenPattern?.EndDate ?? 0,
                r.relatedPtHokenPattern?.HokenSbtCd ?? CommonConstants.InvalidId,
                r.relatedPtHokenPattern?.HokenKbn ?? CommonConstants.InvalidId,
                r.ptKohi1?.HokenSbtKbn ?? CommonConstants.InvalidId,
                r.ptKohi1?.Houbetu ?? string.Empty,
                r.ptKohi2?.HokenSbtKbn ?? CommonConstants.InvalidId,
                r.ptKohi2?.Houbetu ?? string.Empty,
                r.ptKohi3?.HokenSbtKbn ?? CommonConstants.InvalidId,
                r.ptKohi3?.Houbetu ?? string.Empty,
                r.ptKohi4?.HokenSbtKbn ?? CommonConstants.InvalidId,
                r.ptKohi4?.Houbetu ?? string.Empty
            )).ToList();

            return models;
        }

        public bool UpdateStatus(int hpId, long raiinNo, int status, int userId)
        {
            return Update(hpId, raiinNo, r => r.Status = status, userId);
        }

        public bool UpdateUketukeNo(int hpId, long raiinNo, int uketukeNo, int userId)
        {
            return Update(hpId, raiinNo, r => r.UketukeNo = uketukeNo, userId);
        }

        public bool UpdateUketukeTime(int hpId, long raiinNo, string uketukeTime, int userId)
        {
            return Update(hpId, raiinNo, r => r.UketukeTime = uketukeTime, userId);
        }

        public bool UpdateSinStartTime(int hpId, long raiinNo, string sinStartTime, int userId)
        {
            return Update(hpId, raiinNo, r => r.SinStartTime = sinStartTime, userId);
        }

        public bool UpdateUketukeSbt(int hpId, long raiinNo, int uketukeSbt, int userId)
        {
            return Update(hpId, raiinNo, r => r.UketukeSbt = uketukeSbt, userId);
        }

        public bool UpdateTantoId(int hpId, long raiinNo, int tantoId, int userId)
        {
            return Update(hpId, raiinNo, r => r.TantoId = tantoId, userId);
        }

        public bool UpdateKaId(int hpId, long raiinNo, int kaId, int userId)
        {
            return Update(hpId, raiinNo, r => r.KaId = kaId, userId);
        }

        private bool Update(int hpId, long raiinNo, Action<RaiinInf> updateEntity, int userId)
        {
            var raiinInf = _tenantNoTrackingDataContext.RaiinInfs.AsTracking().Where(r =>
                r.HpId == hpId
                && r.RaiinNo == raiinNo
                && r.IsDeleted == DeleteTypes.None).FirstOrDefault();
            if (raiinInf is null)
            {
                return false;
            }

            updateEntity(raiinInf);
            raiinInf.UpdateDate = DateTime.UtcNow;
            raiinInf.UpdateId = userId;
            _tenantNoTrackingDataContext.SaveChanges();
            return true;
        }

        public ReceptionModel GetReceptionComments(int hpId, long raiinNo)
        {
            var receptionComment = _tenantNoTrackingDataContext.RaiinCmtInfs
                .FirstOrDefault(x => x.RaiinNo == raiinNo && x.IsDelete == 0 && x.CmtKbn == 1);
            if (receptionComment is null)
                return new ReceptionModel();
            return new ReceptionModel(
                receptionComment.HpId,
                receptionComment.PtId,
                receptionComment.RaiinNo,
                receptionComment.Text
                );
        }

        public ReceptionModel GetReceptionVisiting(int hpId, long raiinNo)
        {
            var DataRaiinInf = _tenantNoTrackingDataContext.RaiinInfs
                .FirstOrDefault(x => x.HpId == hpId && x.RaiinNo == raiinNo);
            if (DataRaiinInf is null)
                return new ReceptionModel();
            return new ReceptionModel(
                DataRaiinInf.RaiinNo,
                DataRaiinInf.UketukeId,
                DataRaiinInf.KaId,
                DataRaiinInf.UketukeTime ?? string.Empty,
                DataRaiinInf.SinStartTime ?? string.Empty,
                DataRaiinInf.Status,
                DataRaiinInf.YoyakuId,
                DataRaiinInf.TantoId);
        }

        public bool CheckExistReception(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var check = _tenantNoTrackingDataContext.RaiinInfs
                .Any(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == sinDate && x.RaiinNo == raiinNo && x.IsDeleted == 0);
            return check;
        }

        public ReceptionModel GetDataDefaultReception(int hpId, int ptId, int sinDate, int defaultSettingDoctor)
        {
            var tantoId = 0;
            var kaId = 0;
            // Tanto Id
            var mainDoctor = _tenantNoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.IsDelete != 1);
            if (mainDoctor != null)
            {
                var userMst = _tenantNoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == mainDoctor.PrimaryDoctor && (sinDate <= 0 || u.StartDate <= sinDate && u.EndDate >= sinDate));
                if (userMst?.JobCd == 1)
                {
                    tantoId = mainDoctor.PrimaryDoctor;
                }

                // if DefaultDoctorSetting = 1 get doctor from last visit
                if (defaultSettingDoctor == 1)
                {
                    var lastRaiinInf = _tenantNoTrackingDataContext.RaiinInfs.Where(p => p.HpId == hpId &&
                                                           p.PtId == ptId &&
                                                           p.IsDeleted == DeleteTypes.None &&
                                                           p.Status >= RaiinState.TempSave &&
                                                           (sinDate <= 0 || p.SinDate < sinDate))
                                                            .OrderByDescending(p => p.SinDate)
                                                            .ThenByDescending(p => p.RaiinNo).FirstOrDefault();
                    if (lastRaiinInf != null && lastRaiinInf.TantoId > 0)
                    {
                        tantoId = lastRaiinInf.TantoId;
                    }
                }

                // if DefaultDoctorSetting = 2 get doctor from last reception
                if (defaultSettingDoctor == 2)
                {
                    var lastRaiinInf = _tenantNoTrackingDataContext.RaiinInfs.Where(p => p.HpId == hpId &&
                                                           p.IsDeleted == DeleteTypes.None &&
                                                           p.SinDate <= sinDate)
                                                            .OrderByDescending(p => p.SinDate)
                                                            .ThenByDescending(p => p.RaiinNo).FirstOrDefault();
                    if (lastRaiinInf != null && lastRaiinInf.TantoId > 0)
                    {
                        tantoId = lastRaiinInf.TantoId;
                    }
                }
            }

            // KaId
            var getKaIdDefault = _tenantNoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == tantoId && u.IsDeleted == 0);
            if (getKaIdDefault != null)
            {
                kaId = getKaIdDefault.KaId;
            }
            return new ReceptionModel(tantoId, kaId);
        }

        public long InitDoctorCombobox(int userId, int tantoId, long ptId, int hpId, int sinDate)
        {
            var isDoctor = _tenantNoTrackingDataContext.UserMsts.Any(u => u.UserId == userId && u.IsDeleted == DeleteTypes.None && u.JobCd == 1);
            var doctors = _tenantNoTrackingDataContext.UserMsts.Where(p => p.StartDate <= sinDate && p.EndDate >= sinDate && p.JobCd == 1).OrderBy(p => p.SortNo).ToList();
            if (tantoId <= 0 || !doctors.Any(p => p.Id == tantoId))
            {
                // if have only 1 doctor in user list
                if (doctors.Count == 2)
                {
                    return doctors[1].Id;
                }

                if (isDoctor)
                {
                    return userId;
                }
                else
                {
                    var mainDoctor = _tenantNoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.IsDelete != 1);

                    if (mainDoctor != null)
                    {
                        var userMst = _tenantNoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == mainDoctor.PrimaryDoctor && (sinDate <= 0 || u.StartDate <= sinDate && u.EndDate >= sinDate));
                        if (userMst?.JobCd == 1)
                        {
                            return mainDoctor.PrimaryDoctor;
                        }
                    }
                    var defaultDoctorSetting = _tenantNoTrackingDataContext.SystemConfs.FirstOrDefault(p =>
                            p.HpId == hpId && p.GrpCd == 1009 && p.GrpEdaNo == 0)?.Val ?? 0;

                    // if DefaultDoctorSetting = 1 get doctor from last visit
                    if (defaultDoctorSetting == 1)
                    {
                        var lastRaiinInf = _tenantNoTrackingDataContext.RaiinInfs
                                .Where(p => p.HpId == hpId &&
                                            p.PtId == ptId &&
                                            p.IsDeleted == DeleteTypes.None &&
                                            p.Status >= RaiinState.TempSave &&
                                            (sinDate <= 0 || p.SinDate < sinDate))
                                .OrderByDescending(p => p.SinDate)
                                .ThenByDescending(p => p.RaiinNo)
                                .FirstOrDefault(); ;

                        if (lastRaiinInf != null && lastRaiinInf.TantoId > 0)
                        {
                            return lastRaiinInf.TantoId;
                        }
                    }

                    // if DefaultDoctorSetting = 2 get doctor from last reception
                    if (defaultDoctorSetting == 2)
                    {
                        var lastRaiinInf = _tenantNoTrackingDataContext.RaiinInfs
                                .Where(p => p.HpId == hpId &&
                                            p.IsDeleted == DeleteTypes.None &&
                                            p.SinDate <= sinDate)
                                .OrderByDescending(p => p.SinDate)
                                .ThenByDescending(p => p.RaiinNo)
                                .FirstOrDefault();

                        if (lastRaiinInf != null && lastRaiinInf.TantoId > 0)
                        {
                            return lastRaiinInf.TantoId;
                        }
                    }
                }

                //if DefaultDoctorSetting = 0
                return doctors.Count > 0 ? doctors[0].Id : 0;
            }

            return 0;
        }

        public int GetFirstVisitWithSyosin(int hpId, long ptId, int sinDate)
        {
            int firstDate = 0;
            var syosinBi = _tenantNoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId
                                                                           && x.PtId == ptId
                                                                           && x.SinDate < sinDate
                                                                           && x.SyosaisinKbn == SyosaiConst.Syosin
                                                                           && x.Status >= RaiinState.TempSave
                                                                           && x.IsDeleted == DeleteTypes.None
                )
                .OrderByDescending(x => x.SinDate)
                .FirstOrDefault();

            if (syosinBi != null)
            {
                firstDate = syosinBi.SinDate;
            }

            return firstDate;
        }
    }
}