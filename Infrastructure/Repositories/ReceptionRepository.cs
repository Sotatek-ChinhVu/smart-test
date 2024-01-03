using Domain.Constant;
using Domain.Models.Family;
using Domain.Models.Reception;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Infrastructure.Repositories
{
    public class ReceptionRepository : RepositoryBase, IReceptionRepository
    {
        public ReceptionRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public ReceptionModel Get(long raiinNo, bool flag = false)
        {
            var receptionEntity = NoTrackingDataContext.RaiinInfs.FirstOrDefault(r => r.RaiinNo == raiinNo && (!flag || r.IsDeleted == 0));
            var raiinCommentInf = NoTrackingDataContext.RaiinCmtInfs.FirstOrDefault(r => r.RaiinNo == raiinNo);

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
            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            return executionStrategy.Execute(() =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();

                // Insert RaiinInf
                var raiinInf = CreateNewRaiinInf(new ReceptionModel(dto.Reception), hpId, userId);
                UpdateConfirmationInfo(hpId, raiinInf.SinDate, raiinInf.PtId, ref raiinInf);
                TrackingDataContext.RaiinInfs.Add(raiinInf);
                TrackingDataContext.SaveChanges();

                if (raiinInf.OyaRaiinNo == 0)
                {
                    raiinInf.OyaRaiinNo = raiinInf.RaiinNo;
                }

                // Insert RaiinCmtInf
                if (!string.IsNullOrWhiteSpace(dto.ReceptionComment))
                {
                    var raiinCmtInf = CreateNewRaiinCmtInf(raiinInf, dto.ReceptionComment, hpId, userId);
                    TrackingDataContext.RaiinCmtInfs.Add(raiinCmtInf);
                }

                // Insert RaiinKbnInfs
                var raiinKbnInfs = dto.KubunInfs
                    .Where(model => model.KbnCd != CommonConstants.KbnCdDeleteFlag)
                    .Select(dto => CreateNewRaiinKbnInf(dto, raiinInf, hpId, userId));
                TrackingDataContext.RaiinKbnInfs.AddRange(raiinKbnInfs);

                // Update insurances and diseases
                SaveInsuraceConfirmationHistories(dto.Insurances, raiinInf.PtId, hpId, userId);
                UpdateDiseaseTenkis(dto.Diseases, raiinInf.PtId, hpId, userId);
                TrackingDataContext.SaveChanges();

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
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
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
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    Text = text,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
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
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    GrpId = dto.GrpId,
                    KbnCd = dto.KbnCd,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId
                };
            }

            void UpdateConfirmationInfo(int hpId, int sinDate, long ptId, ref RaiinInf raiinInf)
            {
                int confirmationType = 0;
                var raiinInfsInSameday = TrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                                     && item.SinDate == sinDate
                                                                                     && item.PtId == ptId)
                                                                      .ToList();

                var onlineConfirmationHistoryInSameday = TrackingDataContext.OnlineConfirmationHistories.Where(item => item.PtId == ptId)
                                                                                                        .AsEnumerable()
                                                                                                        .Where(item => CIUtil.DateTimeToInt(item.OnlineConfirmationDate) == sinDate)
                                                                                                        .ToList();

                #region update ConfirmationType
                if (raiinInfsInSameday.Any())
                {
                    var confirmedRaiinInfs = raiinInfsInSameday.Where(x => x.ConfirmationType > 0);
                    confirmationType = confirmedRaiinInfs.Any() ? confirmedRaiinInfs.Min(x => x.ConfirmationType) : 0;
                    raiinInf.ConfirmationType = confirmationType;
                }
                if (onlineConfirmationHistoryInSameday.Any())
                {
                    var confirmedOnlineConfirmationHistorys = onlineConfirmationHistoryInSameday.Where(x => x.ConfirmationType > 0);
                    confirmationType = confirmedOnlineConfirmationHistorys.Any() ? confirmedOnlineConfirmationHistorys.Min(x => x.ConfirmationType) : 0;
                    raiinInf.ConfirmationType = confirmationType;
                }
                #endregion

                #region update InfConsFlg
                string infoConsFlg = "    ";
                if (raiinInfsInSameday.Any())
                {
                    void UpdateFlgValue(int flgIdx)
                    {
                        char flgToChar(int flg)
                        {
                            if (flg == 1)
                            {
                                return '1';
                            }
                            else if (flg == 2)
                            {
                                return '2';
                            }
                            return ' ';
                        }

                        var confirmedFlgRaiinInfs = raiinInfsInSameday.Where(x => !string.IsNullOrEmpty(x.InfoConsFlg) && x.InfoConsFlg.Length > flgIdx && x.InfoConsFlg[flgIdx] != ' ');
                        int infConsFlg = !confirmedFlgRaiinInfs.Any() ? 0 : confirmedFlgRaiinInfs.Min(x => x.InfoConsFlg![flgIdx].AsInteger());
                        infoConsFlg = ReplaceAt(infoConsFlg, flgIdx, flgToChar(infConsFlg));
                    }
                    //Update PharmacistsInfoConsFlg
                    UpdateFlgValue(0);
                    //Update SpecificHealthCheckupsInfoConsFlg
                    UpdateFlgValue(1);
                    //Update DiagnosisInfoConsFlg
                    UpdateFlgValue(2);
                    //Update OperationInfoConsFlg
                    UpdateFlgValue(3);
                }
                if (onlineConfirmationHistoryInSameday.Any())
                {
                    void UpdateFlgValue(int flgIdx)
                    {
                        char flgToChar(int flg)
                        {
                            if (flg == 1)
                            {
                                return '1';
                            }
                            else if (flg == 2)
                            {
                                return '2';
                            }
                            return ' ';
                        }

                        var confirmedFlgRaiinInfs = onlineConfirmationHistoryInSameday.Where(x => !string.IsNullOrEmpty(x.InfoConsFlg) && x.InfoConsFlg.Length > flgIdx && x.InfoConsFlg[flgIdx] != ' ');
                        int infConsFlg = !confirmedFlgRaiinInfs.Any() ? 0 : confirmedFlgRaiinInfs.Min(x => x.InfoConsFlg![flgIdx].AsInteger());
                        infoConsFlg = ReplaceAt(infoConsFlg, flgIdx, flgToChar(infConsFlg));
                    }
                    //Update PharmacistsInfoConsFlg
                    UpdateFlgValue(0);
                    //Update SpecificHealthCheckupsInfoConsFlg
                    UpdateFlgValue(1);
                    //Update DiagnosisInfoConsFlg
                    UpdateFlgValue(2);
                    //Update OperationInfoConsFlg
                    UpdateFlgValue(3);
                }
                if (string.IsNullOrWhiteSpace(infoConsFlg))
                {
                    infoConsFlg = "";
                }
                raiinInf.InfoConsFlg = infoConsFlg;
                #endregion
            }

            string ReplaceAt(string input, int index, char newChar)
            {
                if (input == null)
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder(input);
                builder[index] = newChar;
                return builder.ToString();
            }
            #endregion
        }

        public int GetMaxUketukeNo(int hpId, int sindate, int infKbn, int kaId, int uketukeMode)
        {
            var query = NoTrackingDataContext.RaiinInfs.Where
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
            bool resetOyaRaiinNo = false;
            var raiinInf = TrackingDataContext.RaiinInfs
                .FirstOrDefault(r => r.HpId == hpId
                    && r.PtId == dto.Reception.PtId
                    && r.SinDate == dto.Reception.SinDate
                    && r.RaiinNo == dto.Reception.RaiinNo
                    && r.IsDeleted == DeleteTypes.None);
            if (raiinInf is null)
            {
                return false;
            }

            UpdateRaiinInfIfChanged(raiinInf, new ReceptionModel(dto.Reception));
            if (resetOyaRaiinNo)
            {
                ResetOyaRaiinNo(hpId, userId, raiinInf.PtId, raiinInf.RaiinNo);
            }
            UpsertRaiinCmtInf(raiinInf, dto.ReceptionComment);
            SaveRaiinKbnInfs(raiinInf, dto.KubunInfs);
            // Update insurances and diseases
            SaveInsuraceConfirmationHistories(dto.Insurances, raiinInf.PtId, hpId, userId);
            UpdateDiseaseTenkis(dto.Diseases, raiinInf.PtId, hpId, userId);

            TrackingDataContext.SaveChanges();
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
                    if (model.OyaRaiinNo == 0)
                    {
                        resetOyaRaiinNo = true;
                        entity.OyaRaiinNo = entity.RaiinNo;
                    }
                    else
                    {
                        entity.OyaRaiinNo = model.OyaRaiinNo;
                    }
                    entity.KaId = model.KaId;
                    entity.UketukeSbt = model.UketukeSbt;
                    entity.UketukeNo = model.UketukeNo;
                    entity.TantoId = model.TantoId;
                    entity.HokenPid = model.HokenPid;
                    entity.SyosaisinKbn = model.SyosaisinKbn;
                    entity.JikanKbn = model.JikanKbn;
                    entity.SanteiKbn = model.SanteiKbn;
                    entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    entity.UpdateId = userId;
                }

                if (entity.IsYoyaku == 1 && entity.Status == RaiinState.Reservation)
                {
                    if (string.IsNullOrEmpty(entity.UketukeTime) || entity.UketukeTime.Equals("0"))
                    {
                        entity.UketukeTime = model.UketukeTime;
                    }
                    entity.Status = model.Status;
                }
            }

            void ResetOyaRaiinNo(int hpId, int userId, long ptId, long raiinNo)
            {
                var raiinListSameVisit = TrackingDataContext.RaiinInfs.Where(item => item.RaiinNo != raiinNo
                                                                                     && item.OyaRaiinNo == raiinNo
                                                                                     && item.PtId == ptId
                                                                                     && item.HpId == hpId
                                                                                     && item.IsDeleted == 0
                                                                      ).ToList();
                var newOyaRaiinNo = raiinListSameVisit.FirstOrDefault()?.RaiinNo ?? 0;
                if (raiinListSameVisit.Any() && newOyaRaiinNo > 0)
                {
                    foreach (var raiinInf in raiinListSameVisit)
                    {
                        raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        raiinInf.UpdateId = userId;
                        raiinInf.OyaRaiinNo = newOyaRaiinNo;
                    }
                }
            }

            void UpsertRaiinCmtInf(RaiinInf raiinInf, string text)
            {
                var raiinCmtInf = TrackingDataContext.RaiinCmtInfs
                   .FirstOrDefault(x => x.HpId == hpId
                        && x.RaiinNo == raiinInf.RaiinNo
                        && x.CmtKbn == CmtKbns.Comment
                        && x.IsDelete == DeleteTypes.None);
                if (raiinCmtInf is null)
                {
                    TrackingDataContext.RaiinCmtInfs.Add(new RaiinCmtInf
                    {
                        HpId = hpId,
                        PtId = raiinInf.PtId,
                        SinDate = raiinInf.SinDate,
                        RaiinNo = raiinInf.RaiinNo,
                        CmtKbn = CmtKbns.Comment,
                        Text = text,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId
                    });
                }
                else if (raiinCmtInf.Text != text)
                {
                    raiinCmtInf.Text = text;
                    raiinCmtInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    raiinCmtInf.UpdateId = userId;
                }
            }

            void SaveRaiinKbnInfs(RaiinInf raiinInf, IEnumerable<RaiinKbnInfDto> kbnInfDtos)
            {
                var existingEntities = TrackingDataContext.RaiinKbnInfs
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
                            TrackingDataContext.RaiinKbnInfs.Add(new RaiinKbnInf
                            {
                                HpId = hpId,
                                PtId = raiinInf.PtId,
                                SinDate = raiinInf.SinDate,
                                RaiinNo = raiinInf.RaiinNo,
                                GrpId = kbnInfDto.GrpId,
                                KbnCd = kbnInfDto.KbnCd,
                                CreateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                UpdateId = userId,
                                CreateId = userId
                            });
                        }
                        else if (existingEntity.KbnCd != kbnInfDto.KbnCd)
                        {
                            // Update
                            existingEntity.KbnCd = kbnInfDto.KbnCd;
                            existingEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            existingEntity.UpdateId = userId;
                        }
                    }
                }
            }

            #endregion
        }

        private bool Update(int hpId, long raiinNo, Action<RaiinInf> updateEntity)
        {
            var raiinInf = NoTrackingDataContext.RaiinInfs.AsTracking().Where(r =>
                r.HpId == hpId
                && r.RaiinNo == raiinNo).FirstOrDefault();
            if (raiinInf is null)
            {
                return false;
            }

            updateEntity(raiinInf);
            return NoTrackingDataContext.SaveChanges() > 0;
        }

        private bool Update(int hpId, long raiinNo, Action<RaiinInf> updateEntity, int userId)
        {
            var raiinInf = NoTrackingDataContext.RaiinInfs.AsTracking().Where(r =>
                r.HpId == hpId
                && r.RaiinNo == raiinNo
                && r.IsDeleted == DeleteTypes.None).FirstOrDefault();
            if (raiinInf is null)
            {
                return false;
            }

            updateEntity(raiinInf);
            raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            raiinInf.UpdateId = userId;
            NoTrackingDataContext.SaveChanges();
            return true;
        }

        private void SaveInsuraceConfirmationHistories(IEnumerable<InsuranceDto> insurances, long ptId, int hpId, int userId)
        {
            if (insurances.Any())
            {
                List<PtHokenCheck> listHokenCheckAddNew = new();

                foreach (var insuranceItem in insurances)
                {
                    var hokenGrp = insuranceItem.IsHokenGroupKohi ? HokenGroupConstant.HokenGroupKohi : HokenGroupConstant.HokenGroupHokenPattern;

                    var listHokenCheckInsertInput = insuranceItem.ConfirmDateList.Where(item => item.SeqNo == 0).ToList();
                    var listHokenCheckUpdateInput = insuranceItem.ConfirmDateList.Where(item => item.SeqNo != 0).ToList();

                    // Update PtHokenCheck
                    var listUpdateItemDB = TrackingDataContext.PtHokenChecks
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
                        var checkDatetimeInput = DateTime.ParseExact(update.SinDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                        var utcCheckDateTime = DateTime.SpecifyKind(checkDatetimeInput, DateTimeKind.Utc);

                        var hokenCheckItem = listUpdateItemDB.FirstOrDefault(item => item.SeqNo == update.SeqNo);
                        if (hokenCheckItem != null)
                        {
                            hokenCheckItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            hokenCheckItem.UpdateId = userId;

                            // update isDelete
                            if (update.IsDelete)
                            {
                                hokenCheckItem.IsDeleted = 1;
                            }
                            else
                            {
                                hokenCheckItem.CheckDate = utcCheckDateTime;
                                hokenCheckItem.CheckCmt = update.Comment;
                                hokenCheckItem.CheckId = userId;
                            }
                        }
                    }

                    // Add new PtHokenCheck
                    foreach (var item in listHokenCheckInsertInput)
                    {
                        var checkDatetime = DateTime.ParseExact(item.SinDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                        var utcCheckDateTime = DateTime.SpecifyKind(checkDatetime, DateTimeKind.Utc);

                        listHokenCheckAddNew.Add(new PtHokenCheck
                        {
                            HpId = hpId,
                            PtID = ptId,
                            HokenGrp = hokenGrp,
                            HokenId = insuranceItem.HokenId,
                            CheckDate = utcCheckDateTime,
                            CheckCmt = item.Comment,
                            CheckId = userId,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId
                        });
                    }
                }
                TrackingDataContext.PtHokenChecks.AddRange(listHokenCheckAddNew);
            }
        }

        private void UpdateDiseaseTenkis(IEnumerable<DiseaseDto> diseases, long ptId, int hpId, int userId)
        {
            var ptByomeiIds = diseases.Select(d => d.Id);
            var ptByomeis = TrackingDataContext.PtByomeis.AsTracking()
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
                    ptByomei.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    ptByomei.UpdateId = userId;
                }
            }
        }

        public List<ReceptionRowModel> GetList(int hpId, int sinDate, long raiinNo, long ptId, [Optional] bool isGetAccountDue, [Optional] bool isGetFamily, int isDeleted = 2, bool searchSameVisit = false)
        {
            return GetReceptionRowModels(hpId, sinDate, raiinNo, ptId, isGetAccountDue, isGetFamily, isDeleted, searchSameVisit);
        }

        public IEnumerable<ReceptionModel> GetList(int hpId, long ptId, int karteDeleteHistory)
        {
            var result = NoTrackingDataContext.RaiinInfs.Where
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
                        r.YoyakuTime ?? string.Empty,
                        r.YoyakuId,
                        r.UketukeSbt,
                        r.UketukeTime ?? string.Empty,
                        r.UketukeId,
                        r.UketukeNo,
                        r.SinStartTime ?? string.Empty,
                        r.SinEndTime ?? string.Empty,
                        r.KaikeiTime ?? string.Empty,
                        r.KaikeiId,
                        r.KaId,
                        r.TantoId,
                        r.SyosaisinKbn,
                        r.JikanKbn,
                        string.Empty
                   ));

        }

        public ReceptionModel GetYoyakuRaiinInf(int hpId, long ptId, int sinDate)
        {
            var entity = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                       && item.SinDate == sinDate
                                                                       && item.PtId == ptId
                                                                       && item.IsDeleted == DeleteTypes.None
                                                                       && item.Status == RaiinState.Reservation)
                                                        .OrderByDescending(p => p.RaiinNo)
                                                        .FirstOrDefault();
            if (entity == null)
            {
                return new();
            }
            return new ReceptionModel(
                        entity.HpId,
                        entity.PtId,
                        entity.SinDate,
                        entity.RaiinNo,
                        entity.OyaRaiinNo,
                        entity.HokenPid,
                        entity.SanteiKbn,
                        entity.Status,
                        entity.IsYoyaku,
                        entity.YoyakuTime ?? string.Empty,
                        entity.YoyakuId,
                        entity.UketukeSbt,
                        entity.UketukeTime ?? string.Empty,
                        entity.UketukeId,
                        entity.UketukeNo,
                        entity.SinStartTime ?? string.Empty,
                        entity.SinEndTime ?? string.Empty,
                        entity.KaikeiTime ?? string.Empty,
                        entity.KaikeiId,
                        entity.KaId,
                        entity.TantoId,
                        entity.SyosaisinKbn,
                        entity.JikanKbn,
                        string.Empty
                   );
        }

        public List<ReceptionModel> GetLastRaiinInfs(int hpId, long ptId, int sinDate)
        {
            var result = NoTrackingDataContext.RaiinInfs.Where(p =>
                                                                        p.HpId == hpId
                                                                        && p.PtId == ptId
                                                                        && p.IsDeleted == DeleteTypes.None
                                                                        && p.SinDate < sinDate && p.Status >= RaiinState.TempSave)
                                                        .OrderByDescending(p => p.SinDate)
                                                        .ThenByDescending(p => p.RaiinNo);
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
                    r.YoyakuTime ?? string.Empty,
                    r.YoyakuId,
                    r.UketukeSbt,
                    r.UketukeTime ?? string.Empty,
                    r.UketukeId,
                    r.UketukeNo,
                    r.SinStartTime ?? string.Empty,
                    r.SinEndTime ?? string.Empty,
                    r.KaikeiTime ?? string.Empty,
                    r.KaikeiId,
                    r.KaId,
                    r.TantoId,
                    r.SyosaisinKbn,
                    r.JikanKbn,
                    string.Empty
               )).ToList();
        }

        public ReceptionModel GetLastVisit(int hpId, long ptId, int sinDate)
        {
            var result = NoTrackingDataContext.RaiinInfs
                            .Where(p => p.HpId == hpId &&
                                        p.PtId == ptId &&
                                        p.IsDeleted == DeleteTypes.None &&
                                        p.Status >= RaiinState.TempSave &&
                                        (sinDate <= 0 || p.SinDate < sinDate))
                            .OrderByDescending(p => p.SinDate)
                            .ThenByDescending(p => p.RaiinNo)
                            .FirstOrDefault();
            if (result == null)
                return new();

            return new ReceptionModel(
                    result.HpId,
                    result.PtId,
                    result.SinDate,
                    result.RaiinNo,
                    result.OyaRaiinNo,
                    result.HokenPid,
                    result.SanteiKbn,
                    result.Status,
                    result.IsYoyaku,
                    result.YoyakuTime ?? string.Empty,
                    result.YoyakuId,
                    result.UketukeSbt,
                    result.UketukeTime ?? string.Empty,
                    result.UketukeId,
                    result.UketukeNo,
                    result.SinStartTime ?? string.Empty,
                    result.SinEndTime ?? string.Empty,
                    result.KaikeiTime ?? string.Empty,
                    result.KaikeiId,
                    result.KaId,
                    result.TantoId,
                    result.SyosaisinKbn,
                    result.JikanKbn,
                    string.Empty
               );
        }

        public List<SameVisitModel> GetListSameVisit(int hpId, long ptId, int sinDate)
        {
            List<SameVisitModel> result = new();
            var raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                             && item.PtId == ptId
                                                                             && (sinDate == 0 || item.SinDate == sinDate)
                                                                             && item.IsDeleted == 0)
                                                              .ToList();

            foreach (var raiinInf in raiinInfList)
            {
                string sameVisit = raiinInfList.FirstOrDefault(item => item.OyaRaiinNo == raiinInf.OyaRaiinNo && item.RaiinNo != raiinInf.RaiinNo)?.OyaRaiinNo.ToString() ?? string.Empty;
                var sameItem = new SameVisitModel(raiinInf.SinDate, raiinInf.PtId, raiinInf.RaiinNo, raiinInf.OyaRaiinNo, sameVisit);
                result.Add(sameItem);
            }
            return result;
        }

        public bool CheckListNo(List<long> raininNos)
        {
            var check = NoTrackingDataContext.RaiinInfs.Any(r => raininNos.Contains(r.RaiinNo) && r.IsDeleted != 1);
            return check;
        }

        public bool CheckExistOfRaiinNos(List<long> raininNos)
        {
            raininNos = raininNos.Distinct().ToList();
            var raiinInfCount = NoTrackingDataContext.RaiinInfs.Count(r => raininNos.Contains(r.RaiinNo) && r.IsDeleted != 1);
            return raininNos.Count == raiinInfCount;
        }

        private List<ReceptionRowModel> GetReceptionRowModels(int hpId, int sinDate, long raiinNo, long ptId, bool isGetAccountDue, bool isGetFamily, int isDeleted, bool searchSameVisit)
        {
            // 1. Prepare all the necessary collections for the join operation
            // Raiin (Reception)
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(x => (isDeleted == 2 || x.IsDeleted == isDeleted));
            var raiinCmtInfs = NoTrackingDataContext.RaiinCmtInfs.Where(x => x.IsDelete == DeleteTypes.None);
            var raiinKbnInfs = NoTrackingDataContext.RaiinKbnInfs.Where(x => x.IsDelete == DeleteTypes.None);
            var raiinKbnDetails = NoTrackingDataContext.RaiinKbnDetails.Where(x => x.IsDeleted == DeleteTypes.None);
            // Pt (Patient)
            var ptInfs = NoTrackingDataContext.PtInfs.Where(x => (isDeleted == 2 || x.IsDelete == isDeleted));
            var ptCmtInfs = NoTrackingDataContext.PtCmtInfs.Where(x => x.IsDeleted == DeleteTypes.None);
            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteTypes.None);
            var ptKohis = NoTrackingDataContext.PtKohis.Where(x => x.IsDeleted == DeleteTypes.None);
            // Rsv (Reservation)
            var rsvInfs = NoTrackingDataContext.RsvInfs;
            var rsvFrameMsts = NoTrackingDataContext.RsvFrameMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // User (Doctor)
            var userMsts = NoTrackingDataContext.UserMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // Ka (Department)
            var kaMsts = NoTrackingDataContext.KaMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // Lock (Function lock)
            var lockInfs = NoTrackingDataContext.LockInfs.Where(x =>
                x.FunctionCd == FunctionCode.MedicalExaminationCode || x.FunctionCd == FunctionCode.TeamKarte || x.FunctionCd == FunctionCode.SwitchOrderCode);
            // Uketuke
            var uketukeSbtMsts = NoTrackingDataContext.UketukeSbtMsts.Where(x => x.IsDeleted == DeleteTypes.None);

            // 2. Filter collections by parameters
            var filteredRaiinInfs = raiinInfs;
            if (!isGetAccountDue)
            {
                filteredRaiinInfs = filteredRaiinInfs.Where(x => x.HpId == hpId && x.SinDate == sinDate);
            }
            if (raiinNo != CommonConstants.InvalidId && !searchSameVisit)
            {
                filteredRaiinInfs = filteredRaiinInfs.Where(x => x.RaiinNo == raiinNo);
            }
            if (searchSameVisit && ptId != CommonConstants.InvalidId)
            {
                filteredRaiinInfs = filteredRaiinInfs.Where(item => item.PtId == ptId);
            }

            var filteredPtInfs = ptInfs;
            if (ptId != CommonConstants.InvalidId && !isGetFamily)
            {
                filteredPtInfs = filteredPtInfs.Where(x => x.PtId == ptId);
            }
            else if (ptId != CommonConstants.InvalidId && isGetFamily)
            {
                filteredRaiinInfs = filteredRaiinInfs.Where(item => item.Status >= 3);
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
                        orderby x.SinDate descending, x.RaiinNo descending
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

            var raiins = raiinQuery.AsEnumerable().DistinctBy(r => r.raiinInf.RaiinNo).ToList();
            var grpIds = NoTrackingDataContext.RaiinKbnMsts.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None).Select(x => x.GrpCd).ToList();
            var models = raiins.Select(r => new ReceptionRowModel(
                r.raiinInf.RaiinNo,
                r.raiinInf.PtId,
                r.parentRaiinNo,
                r.raiinInf.UketukeNo,
                r.relatedLockInf is not null,
                r.raiinInf.Status,
                r.raiinInf.IsDeleted,
                r.ptInf.PtNum,
                r.ptInf.KanaName ?? string.Empty,
                r.ptInf.Name ?? string.Empty,
                r.ptInf.Sex,
                r.ptInf.Birthday,
                r.raiinInf.YoyakuTime ?? string.Empty,
                r.raiinInf.ConfirmationType,
                r.relatedRsvFrameMst?.RsvFrameName ?? string.Empty,
                r.relatedUketukeSbtMst?.KbnId ?? CommonConstants.InvalidId,
                r.raiinInf.UketukeTime ?? string.Empty,
                r.raiinInf.SinStartTime ?? string.Empty,
                r.raiinInf.SinEndTime ?? string.Empty,
                r.raiinInf.KaikeiTime ?? string.Empty,
                r.relatedRaiinCmtInfComment?.Text ?? string.Empty,
                r.ptCmtInf?.Text ?? string.Empty,
                r.relatedTanto?.UserId ?? CommonConstants.InvalidId,
                string.IsNullOrEmpty(r.relatedTanto?.DrName) ? r.relatedTanto?.Name ?? string.Empty : r.relatedTanto?.DrName ?? string.Empty,
                r.relatedTanto?.KanaName ?? string.Empty,
                r.relatedKaMst?.KaId ?? CommonConstants.InvalidId,
                r.relatedKaMst?.KaName ?? string.Empty,
                r.lastVisitDate,
                r.firstVisitDate,
                r.primaryDoctorName ?? string.Empty,
                r.relatedRaiinCmtInfRemark?.Text ?? string.Empty,
                r.raiinInf.ConfirmationState,
                r.raiinInf.ConfirmationResult ?? string.Empty,
                grpIds,
                dynamicCells: r.raiinKbnDetails.Select(d => new DynamicCell(d.GrpCd, d.KbnCd, d.KbnName ?? string.Empty, d.ColorCd?.Length > 0 ? "#" + d.ColorCd : string.Empty)).ToList(),
                r.raiinInf.SinDate,
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

            foreach (var model in models)
            {
                var kanaName = model.KanaName?.Replace("　", " ") ?? "";
                var list = models
                    .Where(vs => vs.KanaName?.Replace("　", " ") == kanaName && vs.PtId != model.PtId && model.PtNum != vs.PtNum);
                if (!string.IsNullOrWhiteSpace(kanaName) && list != null && list.Any())
                {
                    model.IsNameDuplicate = true;
                }
                else
                {
                    model.IsNameDuplicate = false;
                }
            }

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

        public bool UpdateIsDeleted(int hpId, long raiinNo)
        {
            return Update(hpId, raiinNo, r => r.IsDeleted = 0);
        }

        public ReceptionModel GetReceptionComments(int hpId, long raiinNo)
        {
            var receptionComment = NoTrackingDataContext.RaiinCmtInfs
                .FirstOrDefault(x => x.RaiinNo == raiinNo && x.IsDelete == 0 && x.CmtKbn == 1);
            if (receptionComment is null)
                return new ReceptionModel();
            return new ReceptionModel(
                receptionComment.HpId,
                receptionComment.PtId,
                receptionComment.RaiinNo,
                receptionComment.Text ?? string.Empty
                );
        }

        public ReceptionModel GetReceptionVisiting(int hpId, long raiinNo)
        {
            var DataRaiinInf = NoTrackingDataContext.RaiinInfs
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

        public ReceptionModel GetDataDefaultReception(int hpId, int ptId, int sinDate, int defaultSettingDoctor)
        {
            var tantoId = 0;
            var kaId = 0;
            // Tanto Id
            var mainDoctor = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.IsDelete != 1);
            if (mainDoctor != null)
            {
                var userMst = NoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == mainDoctor.PrimaryDoctor && (sinDate <= 0 || u.StartDate <= sinDate && u.EndDate >= sinDate));
                if (userMst?.JobCd == 1)
                {
                    tantoId = mainDoctor.PrimaryDoctor;
                }

                // if DefaultDoctorSetting = 1 get doctor from last visit
                if (defaultSettingDoctor == 1)
                {
                    var lastRaiinInf = NoTrackingDataContext.RaiinInfs.Where(p => p.HpId == hpId &&
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
                    var lastRaiinInf = NoTrackingDataContext.RaiinInfs.Where(p => p.HpId == hpId &&
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
            var getKaIdDefault = NoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == tantoId && u.IsDeleted == 0);
            if (getKaIdDefault != null)
            {
                kaId = getKaIdDefault.KaId;
            }
            return new ReceptionModel(tantoId, kaId);
        }

        public long InitDoctorCombobox(int userId, int tantoId, long ptId, int hpId, int sinDate)
        {
            var isDoctor = NoTrackingDataContext.UserMsts.Any(u => u.UserId == userId && u.IsDeleted == DeleteTypes.None && u.JobCd == 1);
            var doctors = NoTrackingDataContext.UserMsts.Where(p => p.StartDate <= sinDate && p.EndDate >= sinDate && p.JobCd == 1 && p.IsDeleted == DeleteTypes.None).OrderBy(p => p.SortNo).ToList();
            // if have only 1 doctor in user list
            if (doctors.Count == 1)
            {
                return doctors[0].Id;
            }

            if (isDoctor)
            {
                return userId;
            }
            else
            {
                var mainDoctor = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.IsDelete != 1);

                if (mainDoctor != null)
                {
                    var userMst = NoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == mainDoctor.PrimaryDoctor && (sinDate <= 0 || u.StartDate <= sinDate && u.EndDate >= sinDate));
                    if (userMst?.JobCd == 1)
                    {
                        return mainDoctor.PrimaryDoctor;
                    }
                }
                var defaultDoctorSetting = NoTrackingDataContext.SystemConfs.FirstOrDefault(p =>
                        p.HpId == hpId && p.GrpCd == 1009 && p.GrpEdaNo == 0)?.Val ?? 0;

                // if DefaultDoctorSetting = 1 get doctor from last visit
                if (defaultDoctorSetting == 1)
                {
                    var lastRaiinInf = NoTrackingDataContext.RaiinInfs
                            .Where(p => p.HpId == hpId &&
                                        p.PtId == ptId &&
                                        p.IsDeleted == DeleteTypes.None &&
                                        p.Status >= RaiinState.TempSave &&
                                        (sinDate <= 0 || p.SinDate < sinDate))
                            .OrderByDescending(p => p.SinDate)
                            .ThenByDescending(p => p.RaiinNo)
                            .FirstOrDefault();

                    if (lastRaiinInf != null && lastRaiinInf.TantoId > 0)
                    {
                        return lastRaiinInf.TantoId;
                    }
                }

                // if DefaultDoctorSetting = 2 get doctor from last reception
                if (defaultDoctorSetting == 2)
                {
                    var lastRaiinInf = NoTrackingDataContext.RaiinInfs
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

            return doctors.Count > 0 ? doctors[0].Id : 0;
        }

        public int GetFirstVisitWithSyosin(int hpId, long ptId, int sinDate)
        {
            int firstDate = 0;
            var syosinBi = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId
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

        public bool CheckExistRaiinNo(int hpId, long ptId, long raiinNo)
        {
            return NoTrackingDataContext.RaiinInfs.Any(item => item.HpId == hpId && item.PtId == ptId && item.RaiinNo == raiinNo);
        }

        /// <summary>
        /// Delete reception
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="hpId"></param>
        /// <param name="ptId"></param>
        /// <param name="userId"></param>
        /// <param name="sinDate"></param>
        /// <param name="receptions"></param>
        /// Item1: RaiinNo
        /// Item2: OyaRaiinNo
        /// Item3: Status
        /// <returns></returns>
        /// Item1: SinDate
        /// Item2: RaiinNo
        /// Item3: ptId
        public List<Tuple<int, long, long>> Delete(bool flag, int hpId, long ptId, int userId, int sinDate, List<Tuple<long, long, int>> receptions)
        {
            if (flag)
            {
                var raiinNos = receptions.Select(r => r.Item1);
                raiinNos = raiinNos.Distinct().ToList();
                var raiinInfs = TrackingDataContext.RaiinInfs.Where(r => raiinNos.Contains(r.RaiinNo)).ToList();
                foreach (var raiinInf in raiinInfs)
                {
                    raiinInf.IsDeleted = DeleteTypes.Deleted;
                    raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    raiinInf.UpdateId = userId;
                }

                TrackingDataContext.SaveChanges();

                var result = raiinInfs.Select(r => new Tuple<int, long, long>(r.SinDate, r.RaiinNo, r.PtId)).ToList();

                return result;
            }
            else
            {
                List<Tuple<int, long, long>> result = new();
                foreach (var raiinNoAndOya in receptions)
                {
                    var deletedItem = DeleteKarute(hpId, ptId, raiinNoAndOya.Item1, raiinNoAndOya.Item2, raiinNoAndOya.Item3, sinDate, userId);
                    if (deletedItem.Item1 != 0 && deletedItem.Item2 != 0 && deletedItem.Item3 != 0)
                        result.Add(deletedItem);
                }

                return result;
            }
        }

        private Tuple<int, long, long> DeleteKarute(int hpId, long ptId, long raiinNo, long oyaRaiinNo, int status, int sinDate, int userId)
        {
            int deleteFlag = 1;
            if (status == RaiinState.Reservation || status == RaiinState.Receptionist || status == RaiinState.TempSave)
            {
                deleteFlag = 2;
            }
            else if (status == RaiinState.Calculate || status == RaiinState.Waiting || status == RaiinState.Settled)
            {
                deleteFlag = 1;
            }

            //delete raiinInf
            var raiinInf = TrackingDataContext.RaiinInfs.FirstOrDefault(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == raiinNo && r.SinDate == sinDate);
            if (raiinInf == null) return new(0, 0, 0);

            raiinInf.UpdateId = userId;
            raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            raiinInf.IsDeleted = deleteFlag;

            // Update oyaRaiinNo of other raiinInf
            var listRaiinInf = TrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.OyaRaiinNo == raiinNo && r.RaiinNo != raiinNo && r.IsDeleted == DeleteTypes.None).ToList();
            if (listRaiinInf.Count > 0)
            {
                long minRaiinNo = listRaiinInf.Min(r => r.RaiinNo);
                listRaiinInf.ForEach((r) =>
                {
                    r.OyaRaiinNo = minRaiinNo;
                });
            }
            // No.6512 commit raiininf first for trigger
            TrackingDataContext.SaveChanges();

            // Delete reservation info
            var rsvInf = TrackingDataContext.RsvInfs.FirstOrDefault(r => r.HpId == hpId && r.RaiinNo == raiinNo && r.SinDate == sinDate && r.PtId == ptId);
            if (rsvInf != null) TrackingDataContext.RsvInfs.Remove(rsvInf);

            var rsvFrameInf = TrackingDataContext.RsvFrameInfs.FirstOrDefault(r => r.Number == raiinNo);
            if (rsvFrameInf != null) TrackingDataContext.RsvFrameInfs.Remove(rsvFrameInf);

            //delete order
            var odrInfs = TrackingDataContext.OdrInfs.Where(odr => odr.HpId == hpId
                                                                   && odr.PtId == ptId
                                                                   && odr.RaiinNo == raiinNo
                                                                   && odr.SinDate == sinDate);
            if (odrInfs != null)
            {
                var updateId = userId;
                var updateDate = CIUtil.GetJapanDateTimeNow();

                foreach (var odrInf in odrInfs)
                {
                    odrInf.IsDeleted = deleteFlag;
                    odrInf.UpdateId = updateId;
                    odrInf.UpdateDate = updateDate;
                }
            }

            //delete karte
            var karteInfs = NoTrackingDataContext.KarteInfs.Where(k => k.HpId == hpId
                                                                       && k.PtId == ptId
                                                                       && k.RaiinNo == raiinNo
                                                                       && k.SinDate == sinDate);
            if (karteInfs != null)
            {
                var updateId = userId;
                var updateDate = CIUtil.GetJapanDateTimeNow();

                foreach (var karteInf in karteInfs)
                {
                    karteInf.IsDeleted = deleteFlag;
                    karteInf.UpdateId = updateId;
                    karteInf.UpdateDate = updateDate;
                }
            }

            // Delete KENSA_INF,KENSA_INF_DETAIL
            var listKendaInf = TrackingDataContext.KensaInfs.Where(k => k.HpId == hpId
                                                                        && k.PtId == ptId
                                                                        && k.RaiinNo == raiinNo)
                                                            .ToList();
            listKendaInf.ForEach((k) =>
            {
                k.IsDeleted = DeleteTypes.Deleted;
                k.UpdateDate = CIUtil.GetJapanDateTimeNow();
                k.UpdateId = userId;
            });

            var listKendaInfDetail = TrackingDataContext.KensaInfs.Where(k => k.HpId == hpId
                                                                              && k.PtId == ptId
                                                                              && k.RaiinNo == raiinNo)
                                                                  .ToList();
            listKendaInfDetail.ForEach((k) =>
            {
                k.IsDeleted = DeleteTypes.Deleted;
                k.UpdateDate = CIUtil.GetJapanDateTimeNow();
                k.UpdateId = userId;
            });

            // Delete LIMIT_LIST_INF、LIMIT_CNT_LIST_INF
            var listLimitListInf = TrackingDataContext.LimitListInfs.Where(k => k.HpId == hpId
                                                                                && k.PtId == ptId
                                                                                && k.RaiinNo == raiinNo)
                                                                    .ToList();
            listLimitListInf.ForEach((k) =>
            {
                k.IsDeleted = DeleteTypes.Deleted;
                k.UpdateDate = CIUtil.GetJapanDateTimeNow();
                k.UpdateId = userId;
            });

            var listLimitCntListInf = TrackingDataContext.LimitCntListInfs.Where(k => k.HpId == hpId
                                                                                      && k.PtId == ptId
                                                                                      && k.OyaRaiinNo == oyaRaiinNo)
                                                                          .ToList();
            listLimitCntListInf.ForEach((k) =>
            {
                k.IsDeleted = DeleteTypes.Deleted;
                k.UpdateDate = CIUtil.GetJapanDateTimeNow();
                k.UpdateId = userId;
            });

            // Delete Monshin
            var listMonshinInf = TrackingDataContext.MonshinInfo.Where(m => m.HpId == hpId
                                                                            && m.PtId == ptId
                                                                            && m.RaiinNo == raiinNo
                                                                            && m.SinDate == sinDate)
                                                                .ToList();
            listMonshinInf.ForEach((m) =>
            {
                m.IsDeleted = DeleteTypes.Deleted;
                m.UpdateDate = CIUtil.GetJapanDateTimeNow();
                m.UpdateId = userId;
            });
            var result = new Tuple<int, long, long>(raiinInf.SinDate, raiinInf.RaiinNo, raiinInf.PtId);

            TrackingDataContext.SaveChanges();

            return result;
        }

        public ReceptionModel? GetLastKarute(int hpId, long ptNum)
        {
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtNum == ptNum && p.IsDelete == DeleteTypes.None);

            if (ptInf != null)
            {
                var raiinInf = NoTrackingDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.PtId == ptInf.PtId && r.IsDeleted == DeleteTypes.None
                                                                                    && r.Status >= RaiinState.TempSave).OrderByDescending(r => r.SinDate).FirstOrDefault();
                if (raiinInf != null)
                {
                    return new ReceptionModel(raiinInf.HpId,
                                              raiinInf.PtId,
                                              raiinInf.RaiinNo,
                                              raiinInf.SinDate);
                }
            }

            return null;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<ReceptionModel> GetListRaiinInf(int hpId, long ptId, int pageIndex, int pageSize, int isDeleted, bool isAll = false)
        {
            List<ReceptionModel> result = new();
            List<RaiinInf> raiinInfs;
            if (isAll)
            {
                raiinInfs = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId &&
                                                        x.PtId == ptId && (x.IsDeleted == DeleteTypes.None || isDeleted == 1 || (x.IsDeleted != DeleteTypes.Confirm && isDeleted == 2)))
                                            .OrderByDescending(x => x.SinDate)
                                            .ToList();
            }
            else
            {
                raiinInfs = NoTrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId &&
                                                           x.PtId == ptId && (x.IsDeleted == DeleteTypes.None || isDeleted == 1 || (x.IsDeleted != DeleteTypes.Confirm && isDeleted == 2)))
                                               .OrderByDescending(x => x.SinDate)
                                               .Skip((pageIndex - 1) * pageSize)
                                               .Take(pageSize)
                                               .ToList();
            }

            var tantoIdList = raiinInfs.Select(item => item.TantoId).Distinct().ToList();
            var kaIdIdList = raiinInfs.Select(item => item.KaId).Distinct().ToList();

            var userMsts = NoTrackingDataContext.UserMsts.Where(x => x.HpId == hpId &&
                                                                     x.IsDeleted == 0 &&
                                                                     tantoIdList.Contains(x.UserId))
                                                         .ToList();

            var kaMsts = NoTrackingDataContext.KaMsts.Where(x => x.HpId == hpId &&
                                                                x.IsDeleted == 0 &&
                                                                kaIdIdList.Contains(x.KaId))
                                                     .ToList();

            var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(x => x.HpId == hpId &&
                                                                           x.IsDeleted == 0 &&
                                                                           x.PtId == ptId)
                                                                .ToList();
            var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(x => x.HpId == hpId &&
                                                                                   x.IsDeleted == 0 &&
                                                                                   x.PtId == ptId)
                                                                       .ToList();

            foreach (var raiinInf in raiinInfs)
            {
                var kaMst = kaMsts.FirstOrDefault(item => item.KaId == raiinInf.KaId);
                var userMst = userMsts.FirstOrDefault(item => item.UserId == raiinInf.TantoId);
                var ptHokenPattern = ptHokenPatterns.FirstOrDefault(item => item.HokenPid == raiinInf.HokenPid);
                var ptHokenInf = ptHokenInfs.FirstOrDefault(item => ptHokenPattern != null && item.HokenId == ptHokenPattern.HokenId);
                var item = new ReceptionModel(
                            raiinInf.HpId,
                            raiinInf.PtId,
                            raiinInf.SinDate,
                            raiinInf.UketukeNo,
                            raiinInf.Status,
                            kaMst?.KaSname ?? string.Empty,
                            userMst?.Sname ?? string.Empty,
                            ptHokenInf?.Houbetu ?? string.Empty,
                            ptHokenInf?.HokensyaNo ?? string.Empty,
                            ptHokenInf?.HokenKbn ?? 0,
                            ptHokenInf?.HokenId ?? 0,
                            raiinInf.HokenPid,
                            raiinInf.RaiinNo,
                            raiinInf.IsDeleted == 1);
                result.Add(item);
            }

            return result;
        }

        public List<ReceptionModel> GetRaiinListWithKanInf(int hpId, long ptId)
        {
            List<ReceptionModel> result = new();
            var raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId &&
                                                                             item.IsDeleted == DeleteTypes.None &&
                                                                             item.PtId == ptId
                                                               ).OrderByDescending(p => p.SinDate)
                                                               .ToList();

            var kaIdList = raiinInfList.Select(item => item.KaId).Distinct().ToList();
            var tantoIdList = raiinInfList.Select(item => item.TantoId).Distinct().ToList();
            var hokenPIdList = raiinInfList.Select(item => item.HokenPid).Distinct().ToList();

            var kaMstList = NoTrackingDataContext.KaMsts.Where(item => item.HpId == hpId &&
                                                                       item.IsDeleted == 0 &&
                                                                       kaIdList.Contains(item.KaId)
                                                        ).ToList();

            var userMstList = NoTrackingDataContext.UserMsts.Where(item => item.HpId == hpId &&
                                                                           item.IsDeleted == 0 &&
                                                                           tantoIdList.Contains(item.UserId)
                                                            ).ToList();

            var ptHokenPatternList = NoTrackingDataContext.PtHokenPatterns.Where(item => item.HpId == hpId &&
                                                                                         item.IsDeleted == 0 &&
                                                                                         item.PtId == ptId &&
                                                                                         hokenPIdList.Contains(item.HokenPid)
                                                                          ).ToList();

            var hokenIdList = ptHokenPatternList.Select(item => item.HokenId).Distinct().ToList();

            var ptHokenInfList = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId &&
                                                                                 item.IsDeleted == 0 &&
                                                                                 item.PtId == ptId &&
                                                                                 hokenIdList.Contains(item.HokenId)
                                                                  ).ToList();

            foreach (var raiinInf in raiinInfList)
            {
                var kaSName = kaMstList.FirstOrDefault(item => item.KaId == raiinInf.KaId)?.KaSname ?? string.Empty;
                var sName = userMstList.FirstOrDefault(item => item.UserId == raiinInf.TantoId)?.Sname ?? string.Empty;
                var ptHokenPattern = ptHokenPatternList.FirstOrDefault(item => item.HokenPid == raiinInf.HokenPid);
                var ptHokenInf = ptHokenInfList.FirstOrDefault(item => item.HokenId == ptHokenPattern?.HokenId);
                var hokenKbnName = GetHokenKbnName(ptHokenPattern, ptHokenInf);
                result.Add(new ReceptionModel(
                               raiinInf.PtId,
                               raiinInf.SinDate,
                               raiinInf.RaiinNo,
                               raiinInf.TantoId,
                               raiinInf.KaId,
                               sName,
                               kaSName,
                               hokenKbnName));
            }
            return result;
        }

        private string GetHokenKbnName(PtHokenPattern? ptHokenPattern, PtHokenInf? ptHokenInf)
        {
            string result = string.Empty;
            if (ptHokenPattern == null || ptHokenPattern.PtId == 0 && ptHokenPattern.HokenPid == 0 && ptHokenPattern.HpId == 0)
            {
                return string.Empty;
            }

            if (ptHokenInf == null)
            {
                result = "公費";
                return result;
            }

            if (ptHokenInf.Houbetu == HokenConstant.HOUBETU_NASHI)
            {
                result = "公費";
                return result;
            }

            string hokensyaNo = ptHokenInf.HokensyaNo ?? string.Empty;
            switch (ptHokenInf.HokenKbn)
            {
                case 0:
                    result = "自費";
                    break;
                case 1:
                    result = "社保";
                    break;
                case 2:
                    if (hokensyaNo.Length == 8 &&
                        hokensyaNo.StartsWith("39"))
                    {
                        result = "後期";
                    }
                    else if (hokensyaNo.Length == 8 &&
                        hokensyaNo.StartsWith("67"))
                    {
                        result = "退職";
                    }
                    else
                    {
                        result = "国保";
                    }
                    break;
                case 11:
                case 12:
                case 13:
                    result = "労災";
                    break;
                case 14:
                    result = "自賠";
                    break;
            }
            return result;
        }

        public List<RaiinInfToPrintModel> GetOutDrugOrderList(int hpId, int fromDate, int toDate)
        {
            var raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && item.SinDate >= fromDate
                                                                             && item.SinDate <= toDate
                                                                             && item.Status >= RaiinState.TempSave)
                                                              .ToList();

            var raiinNoList = raiinInfList.Select(item => item.RaiinNo).Distinct().ToList();
            var ptIdList = raiinInfList.Select(item => item.PtId).Distinct().ToList();
            var kaIdList = raiinInfList.Select(item => item.KaId).Distinct().ToList();
            var tantoIdList = raiinInfList.Select(item => item.TantoId).Distinct().ToList();
            var uketukeSbtList = raiinInfList.Select(item => item.UketukeSbt).Distinct().ToList();
            var hokenPidList = raiinInfList.Select(item => item.HokenPid).Distinct().ToList();

            var ordInfList = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                         && item.SinDate >= fromDate
                                                                         && item.SinDate <= toDate
                                                                         && item.IsDeleted == 0
                                                                         && raiinNoList.Contains(item.RaiinNo)
                                                                         && item.InoutKbn == 1// コメント（処方箋備考）
                                                                         && ((item.OdrKouiKbn >= 20 && item.OdrKouiKbn <= 29) // 処方
                                                                              || item.OdrKouiKbn == 100 // コメント（処方箋）
                                                                              || item.OdrKouiKbn == 101))
                                                          .GroupBy(item => new { item.RaiinNo })
                                                          .Select(item => item.FirstOrDefault())
                                                          .ToList();

            var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                       && item.IsDelete == 0
                                                                       && ptIdList.Contains(item.PtId))
                                                        .ToList();

            var kaMstList = NoTrackingDataContext.KaMsts.Where(item => item.HpId == hpId
                                                                       && item.IsDeleted == 0
                                                                       && kaIdList.Contains(item.KaId))
                                                        .ToList();

            var userMstList = NoTrackingDataContext.UserMsts.Where(item => item.HpId == hpId
                                                                           && item.IsDeleted == 0
                                                                           && item.StartDate <= fromDate
                                                                           && toDate <= item.EndDate
                                                                           && tantoIdList.Contains(item.UserId))
                                                            .ToList();

            var uketsukeSbtMstList = NoTrackingDataContext.UketukeSbtMsts.Where(item => item.HpId == hpId
                                                                                        && item.IsDeleted == 0
                                                                                        && uketukeSbtList.Contains(item.KbnId));

            #region Get HokenPatternName
            var ptHokenPatternList = NoTrackingDataContext.PtHokenPatterns.Where(item => item.HpId == hpId
                                                                                         && item.IsDeleted == 0
                                                                                         && hokenPidList.Contains(item.HokenPid)
                                                                                         && ptIdList.Contains(item.PtId))
                                                                          .ToList();

            var hokenIdList = ptHokenPatternList.Select(item => item.HokenId).Distinct().ToList();

            var ptHokenInfList = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId
                                                                                 && item.IsDeleted == 0
                                                                                 && ptIdList.Contains(item.PtId)
                                                                                 && hokenIdList.Contains(item.HokenId))
                                                                  .ToList();

            var ptHokenPatternResult = (from ptHokenPattern in ptHokenPatternList
                                        join ptHokenInf in ptHokenInfList on
                                            new { ptHokenPattern.PtId, ptHokenPattern.HokenId } equals
                                            new { ptHokenInf.PtId, ptHokenInf.HokenId } into ptHokenInf1List
                                        from ptHokenInfItem in ptHokenInf1List.DefaultIfEmpty()
                                        select new
                                        {
                                            ptHokenPattern.HokenPid,
                                            ptHokenPattern.PtId,
                                            HokenHobetu = ptHokenInfItem == null ? "" : ptHokenInfItem.Houbetu,
                                            HokensyaNo = ptHokenInfItem == null ? "" : ptHokenInfItem.HokensyaNo,
                                            PtHokenPattern = ptHokenPattern,
                                            HokenInfHokenId = ptHokenInfItem == null ? 0 : ptHokenInfItem.HokenId,
                                            HokenInfStartDate = ptHokenInfItem == null ? 0 : ptHokenInfItem.StartDate,
                                            HokenInfEndDate = ptHokenInfItem == null ? 0 : ptHokenInfItem.EndDate,
                                        }).ToList();

            #endregion

            var query = from odr in ordInfList
                        join raiin in raiinInfList
                            on new { odr.HpId, odr.PtId, odr.SinDate, odr.RaiinNo }
                            equals new { raiin.HpId, raiin.PtId, raiin.SinDate, raiin.RaiinNo }
                        join pt in ptInfList
                            on new { raiin.PtId }
                            equals new { pt.PtId } into ptLeft
                        from pt in ptLeft
                        join ka in kaMstList
                            on new { raiin.KaId }
                            equals new { ka.KaId } into kaLeft
                        from ka in kaLeft
                        join user in userMstList
                             on new { raiin.TantoId }
                             equals new { TantoId = user.UserId } into userLeft
                        from user in userLeft.DefaultIfEmpty()
                        join uketsuke in uketsukeSbtMstList
                            on new { raiin.UketukeSbt }
                            equals new { UketukeSbt = uketsuke.KbnId } into uketsukeLeft
                        from uketsuke in uketsukeLeft.DefaultIfEmpty()
                        join hokenPattern in ptHokenPatternResult
                            on new { raiin.PtId, raiin.HokenPid, }
                            equals new { hokenPattern.PtId, hokenPattern.HokenPid } into PtHokenPatternLeft
                        from hokenPattern in PtHokenPatternLeft.DefaultIfEmpty()
                        select new
                        {
                            Raiin = raiin,
                            Pt = pt,
                            Ka = ka,
                            User = user,
                            Uketsuke = uketsuke,
                            PtHokenPatternItem = hokenPattern
                        };
            var result = query.Select(data => new RaiinInfToPrintModel(PrintMode.PrintPrescription,
                                                                       data.Pt?.Name ?? string.Empty,
                                                                       data.User?.Name ?? string.Empty,
                                                                       0,
                                                                       data.Raiin?.KaId ?? 0,
                                                                       data.Pt?.PtId ?? 0,
                                                                       data.Pt?.PtNum ?? 0,
                                                                       data.PtHokenPatternItem?.HokenHobetu ?? string.Empty,
                                                                       data.PtHokenPatternItem?.PtHokenPattern.HokenKbn ?? 0,
                                                                       string.Empty,
                                                                       data.PtHokenPatternItem?.HokensyaNo ?? string.Empty,
                                                                       data.Raiin?.UketukeNo ?? 0,
                                                                       0,
                                                                        data.Raiin?.SinDate ?? 0,
                                                                       0,
                                                                       data.Raiin?.TantoId ?? 0,
                                                                       data.Ka?.KaName ?? string.Empty,
                                                                       data.Raiin?.UketukeSbt ?? 0,
                                                                       data.Uketsuke?.KbnName ?? string.Empty,
                                                                       0,
                                                                       data.Raiin?.HokenPid ?? 0,
                                                                       0,
                                                                       -1,
                                                                       -1,
                                                                       string.Empty,
                                                                       string.Empty,
                                                                       0, 0, 0, 0, 0, 0, 0, 0, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                       data.Raiin?.Status ?? 0, data.Raiin?.RaiinNo ?? 0))
                          .ToList();

            return result;
        }

        public int GetStatusRaiinInf(int hpId, long raiinNo, long ptId)
        {
            var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(item => item.HpId == hpId
                                                                                  && item.PtId == ptId
                                                                                  && item.RaiinNo == raiinNo);
            if (raiinInf == null)
            {
                return 0;
            }
            return raiinInf.Status;
        }

        public ReceptionModel GetRaiinInfBySinDate(int hpId, long ptId, int sinDate)
        {
            var raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                             && item.SinDate == sinDate
                                                                             && item.PtId == ptId
                                                                             && item.IsDeleted == DeleteTypes.None)
                                                              .ToList();
            if (!raiinInfList.Any())
            {
                return new();
            }

            RaiinInf raiinInf;

            if (raiinInfList.Any(item => item.Status == RaiinState.Reservation))
            {
                raiinInf = raiinInfList.OrderByDescending(item => item.IsYoyaku).ThenBy(item => item.YoyakuTime).ThenBy(item => item.RaiinNo).First();
            }
            else
            {
                raiinInf = raiinInfList.OrderBy(item => item.UketukeTime).ThenBy(item => item.RaiinNo).First();
            }

            return new ReceptionModel(raiinInf.HpId,
                                      raiinInf.PtId,
                                      raiinInf.SinDate,
                                      raiinInf.RaiinNo,
                                      raiinInf.OyaRaiinNo,
                                      raiinInf.HokenPid,
                                      raiinInf.SanteiKbn,
                                      raiinInf.Status,
                                      raiinInf.IsYoyaku,
                                      raiinInf.YoyakuTime ?? string.Empty,
                                      raiinInf.YoyakuId,
                                      raiinInf.UketukeSbt,
                                      raiinInf.UketukeTime ?? string.Empty,
                                      raiinInf.UketukeId,
                                      raiinInf.UketukeNo,
                                      raiinInf.SinStartTime ?? string.Empty,
                                      raiinInf.SinEndTime ?? string.Empty,
                                      raiinInf.KaikeiTime ?? string.Empty,
                                      raiinInf.KaikeiId,
                                      raiinInf.KaId,
                                      raiinInf.TantoId,
                                      raiinInf.SyosaisinKbn,
                                      raiinInf.JikanKbn,
                                      string.Empty
                               );
        }

        public int GetNextUketukeNoBySetting(int hpId, int sindate, int infKbn, int kaId, int uketukeMode, int defaultUkeNo)
        {
            var raiinInf = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                         && item.SinDate == sindate
                                                                         && (!(uketukeMode == 1 || uketukeMode == 3) || item.UketukeSbt == infKbn)
                                                                         && (!(uketukeMode == 2 || uketukeMode == 3) || item.KaId == kaId)
                                                                         && item.IsDeleted == DeleteTypes.None
                                                         ).OrderByDescending(p => p.UketukeNo)
                                                          .FirstOrDefault();
            if (raiinInf != null)
            {
                return raiinInf.UketukeNo + 1 < defaultUkeNo ? defaultUkeNo : raiinInf.UketukeNo + 1;
            }
            return defaultUkeNo > 0 ? defaultUkeNo : 1;
        }

        public RaiinInfModel? GetRaiinInf(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p =>
                p.HpId == hpId &&
                p.PtId == ptId &&
                p.SinDate == sinDate &&
                p.RaiinNo == raiinNo &&
                p.IsDeleted == DeleteTypes.None
            );

            var kaMsts = NoTrackingDataContext.KaMsts.Where(p =>
                p.HpId == hpId &&
                p.IsDeleted == DeleteStatus.None
            );

            var userMsts = NoTrackingDataContext.UserMsts.Where(p =>
                p.HpId == hpId &&
                p.IsDeleted == DeleteStatus.None
            );

            var join = (
                from raiinInf in raiinInfs
                join kaMst in kaMsts on
                    new { raiinInf.HpId, raiinInf.KaId } equals
                    new { kaMst.HpId, kaMst.KaId } into joinKaMsts
                from joinKaMst in joinKaMsts.DefaultIfEmpty()
                join userMst in userMsts on
                    new { raiinInf.HpId, raiinInf.TantoId } equals
                    new { userMst.HpId, TantoId = userMst.UserId } into joinUserMsts
                from joinUserMst in joinUserMsts.DefaultIfEmpty()
                select new
                {
                    raiinInf,
                    joinKaMst,
                    joinUserMst
                }
                ).ToList();

            RaiinInfModel? result = null;

            if (join != null && join.Any())
            {
                //return RaiinInf with User and Ka info
                result = new RaiinInfModel(join.First().raiinInf.PtId,
                                           join.First().raiinInf.SinDate,
                                           join.First().raiinInf.RaiinNo,
                                           join.First().raiinInf.KaId,
                                           join.First().joinKaMst != null ? join.First().joinKaMst.KaName ?? string.Empty : string.Empty,
                                           join.First().raiinInf.TantoId,
                                           join.First().joinUserMst != null ? join.First().joinUserMst.DrName ?? string.Empty : string.Empty,
                                           join.First().joinUserMst != null ? join.First().joinUserMst.Name ?? string.Empty : string.Empty,
                                           join.First().joinUserMst != null ? join.First().joinUserMst.KanaName ?? string.Empty : string.Empty,
                                           join.First().joinKaMst != null ? join.First().joinKaMst.KaSname ?? string.Empty : string.Empty
                                        );
            }

            return result;
        }

    }
}
