using Domain.Constant;
using Domain.Models.Reception;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class ReceptionRepository : IReceptionRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public ReceptionRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public ReceptionModel Get(long raiinNo)
        {
            var receptionEntity = _tenantDataContext.RaiinInfs.SingleOrDefault(r => r.RaiinNo == raiinNo);
            var raiinCommentInf = _tenantDataContext.RaiinCmtInfs.SingleOrDefault(r => r.RaiinNo == raiinNo);

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

        public long Insert(ReceptionSaveDto dto)
        {
            var executionStrategy = _tenantDataContext.Database.CreateExecutionStrategy();
            return executionStrategy.Execute(() =>
            {
                using var transaction = _tenantDataContext.Database.BeginTransaction();

                // Insert RaiinInf
                var raiinInf = CreateNewRaiinInf(dto.Reception);
                _tenantDataContext.RaiinInfs.Add(raiinInf);
                _tenantDataContext.SaveChanges();

                if (raiinInf.OyaRaiinNo == 0)
                {
                    raiinInf.OyaRaiinNo = raiinInf.RaiinNo;
                }

                // Insert RaiinCmtInf
                if (!string.IsNullOrWhiteSpace(dto.ReceptionComment))
                {
                    var raiinCmtInf = CreateNewRaiinCmtInf(raiinInf, dto.ReceptionComment);
                    _tenantDataContext.RaiinCmtInfs.Add(raiinCmtInf);
                }

                // Insert RaiinKbnInfs
                var raiinKbnInfs = dto.KubunInfs
                    .Where(model => model.KbnCd != CommonConstants.KbnCdDeleteFlag)
                    .Select(dto => CreateNewRaiinKbnInf(dto, raiinInf));
                _tenantDataContext.RaiinKbnInfs.AddRange(raiinKbnInfs);

                // Update insurances and diseases
                AddInsuraceConfirmationHistories(dto.Insurances, raiinInf.PtId);
                UpdateDiseaseTenkis(dto.Diseases, raiinInf.PtId);
                _tenantDataContext.SaveChanges();

                transaction.Commit();
                return raiinInf.RaiinNo;
            });

            #region Helper methods

            RaiinInf CreateNewRaiinInf(ReceptionModel model)
            {
                return new RaiinInf
                {
                    HpId = TempIdentity.HpId,
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
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName
                };
            }

            RaiinCmtInf CreateNewRaiinCmtInf(RaiinInf raiinInf, string text)
            {
                return new RaiinCmtInf
                {
                    HpId = TempIdentity.HpId,
                    PtId = raiinInf.PtId,
                    SinDate = raiinInf.SinDate,
                    RaiinNo = raiinInf.RaiinNo,
                    CmtKbn = CmtKbns.Comment,
                    Text = text,
                    CreateDate = DateTime.UtcNow,
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName
                };
            }

            RaiinKbnInf CreateNewRaiinKbnInf(RaiinKbnInfDto dto, RaiinInf raiinInf)
            {
                return new RaiinKbnInf
                {
                    HpId = TempIdentity.HpId,
                    PtId = raiinInf.PtId,
                    SinDate = raiinInf.SinDate,
                    RaiinNo = raiinInf.RaiinNo,
                    GrpId = dto.GrpId,
                    KbnCd = dto.KbnCd,
                    CreateDate = DateTime.UtcNow,
                    CreateId = TempIdentity.UserId,
                    CreateMachine = TempIdentity.ComputerName
                };
            }

            #endregion
        }

        public bool Update(ReceptionSaveDto dto)
        {
            var raiinInf = _tenantDataContext.RaiinInfs.AsTracking()
                .FirstOrDefault(r => r.HpId == TempIdentity.HpId
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
            AddInsuraceConfirmationHistories(dto.Insurances, raiinInf.PtId);
            UpdateDiseaseTenkis(dto.Diseases, raiinInf.PtId);

            _tenantDataContext.SaveChanges();
            return true;

            #region Helper methods

            void UpdateRaiinInfIfChanged(RaiinInf entity, ReceptionModel model)
            {
                // Detect changes
                if (entity.OyaRaiinNo != model.OyaRaiinNo
                    || entity.KaId != model.KaId
                    || entity.UketukeSbt != model.UketukeSbt
                    || entity.UketukeNo != model.UketukeNo
                    || entity.TantoId != model.TantoId)
                {
                    entity.OyaRaiinNo = model.OyaRaiinNo;
                    entity.KaId = model.KaId;
                    entity.UketukeSbt = model.UketukeSbt;
                    entity.UketukeNo = model.UketukeNo;
                    entity.TantoId = model.TantoId;
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.UpdateId = TempIdentity.UserId;
                    entity.UpdateMachine = TempIdentity.ComputerName;
                }
            }

            void UpsertRaiinCmtInf(RaiinInf raiinInf, string text)
            {
                var raiinCmtInf = _tenantDataContext.RaiinCmtInfs.AsTracking()
                   .FirstOrDefault(x => x.HpId == TempIdentity.HpId
                        && x.RaiinNo == raiinInf.RaiinNo
                        && x.CmtKbn == CmtKbns.Comment
                        && x.IsDelete == DeleteTypes.None);
                if (raiinCmtInf is null)
                {
                    _tenantDataContext.RaiinCmtInfs.Add(new RaiinCmtInf
                    {
                        HpId = TempIdentity.HpId,
                        PtId = raiinInf.PtId,
                        SinDate = raiinInf.SinDate,
                        RaiinNo = raiinInf.RaiinNo,
                        CmtKbn = CmtKbns.Comment,
                        Text = text,
                        CreateDate = DateTime.UtcNow,
                        CreateId = TempIdentity.UserId,
                        CreateMachine = TempIdentity.ComputerName
                    });
                }
                else if (raiinCmtInf.Text != text)
                {
                    raiinCmtInf.Text = text;
                    raiinCmtInf.UpdateDate = DateTime.UtcNow;
                    raiinCmtInf.UpdateId = TempIdentity.UserId;
                    raiinCmtInf.UpdateMachine = TempIdentity.ComputerName;
                }
            }

            void SaveRaiinKbnInfs(RaiinInf raiinInf, IEnumerable<RaiinKbnInfDto> kbnInfDtos)
            {
                var existingEntities = _tenantDataContext.RaiinKbnInfs.AsTracking()
                    .Where(x => x.HpId == TempIdentity.HpId
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
                            _tenantDataContext.RaiinKbnInfs.Add(new RaiinKbnInf
                            {
                                HpId = TempIdentity.HpId,
                                PtId = raiinInf.PtId,
                                SinDate = raiinInf.SinDate,
                                RaiinNo = raiinInf.RaiinNo,
                                GrpId = kbnInfDto.GrpId,
                                KbnCd = kbnInfDto.KbnCd,
                                CreateDate = DateTime.UtcNow,
                                CreateId = TempIdentity.UserId,
                                CreateMachine = TempIdentity.ComputerName
                            });
                        }
                        else if (existingEntity.KbnCd != kbnInfDto.KbnCd)
                        {
                            // Update
                            existingEntity.KbnCd = kbnInfDto.KbnCd;
                            existingEntity.UpdateDate = DateTime.UtcNow;
                            existingEntity.UpdateId = TempIdentity.UserId;
                            existingEntity.UpdateMachine = TempIdentity.ComputerName;
                        }
                    }
                }
            }

            #endregion
        }

        private void AddInsuraceConfirmationHistories(IEnumerable<InsuranceDto> insurances, long ptId)
        {
            var hokenIds = insurances.Select(i => i.HokenId).Distinct();
            var latestPtHokenChecks = (
                from phc in _tenantDataContext.PtHokenChecks.AsTracking()
                where phc.HpId == TempIdentity.HpId
                    && phc.PtID == ptId
                    && hokenIds.Contains(phc.HokenId)
                    && phc.HokenGrp == HokenGroupConstant.HokenGroupHokenPattern
                    && phc.IsDeleted == DeleteTypes.None
                group phc by phc.HokenId into phcGroup
                select phcGroup.OrderByDescending(x => x.CheckDate).FirstOrDefault()
            ).ToList();

            var newPhcs = new List<PtHokenCheck>();
            foreach (var insurance in insurances)
            {
                var latestPhc = latestPtHokenChecks.Find(x => x.HokenId == insurance.HokenId);
                if (latestPhc is not null && latestPhc.CheckDate.ToUniversalTime() != insurance.UtcCheckDate)
                {
                    newPhcs.Add(new PtHokenCheck
                    {
                        HpId = TempIdentity.HpId,
                        PtID = ptId,
                        HokenGrp = HokenGroupConstant.HokenGroupHokenPattern,
                        HokenId = insurance.HokenId,
                        CheckDate = insurance.UtcCheckDate,
                        CheckCmt = string.Empty,
                        CheckId = TempIdentity.UserId,
                        CheckMachine = TempIdentity.ComputerName,
                        CreateDate = DateTime.UtcNow,
                        CreateId = TempIdentity.UserId,
                        CreateMachine = TempIdentity.ComputerName
                    });
                }
            }
            _tenantDataContext.PtHokenChecks.AddRange(newPhcs);
        }

        private void UpdateDiseaseTenkis(IEnumerable<DiseaseDto> diseases, long ptId)
        {
            var ptByomeiIds = diseases.Select(d => d.Id);
            var ptByomeis = _tenantDataContext.PtByomeis.AsTracking()
                .Where(x => x.HpId == TempIdentity.HpId && x.PtId == ptId && ptByomeiIds.Contains(x.Id))
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
                    ptByomei.UpdateId = TempIdentity.UserId;
                    ptByomei.UpdateMachine = TempIdentity.ComputerName;
                }
            }
        }

        public List<ReceptionRowModel> GetList(int hpId, int sinDate, long raiinNo, long ptId)
        {
            return GetReceptionRowModels(hpId, sinDate, raiinNo, ptId);
        }

        public IEnumerable<ReceptionModel> GetList(int hpId, long ptId, int karteDeleteHistory)
        {
            var result = _tenantDataContext.RaiinInfs.Where
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
                        r.SinStartTime,
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
            var check = _tenantDataContext.RaiinInfs.Any(r => raininNos.Contains(r.RaiinNo) && r.IsDeleted != 1);
            return check;
        }

        private List<ReceptionRowModel> GetReceptionRowModels(int hpId, int sinDate, long raiinNo, long ptId)
        {
            // 1. Prepare all the necessary collections for the join operation
            // Raiin (Reception)
            var raiinInfs = _tenantDataContext.RaiinInfs.Where(x => x.IsDeleted == DeleteTypes.None);
            var raiinCmtInfs = _tenantDataContext.RaiinCmtInfs.Where(x => x.IsDelete == DeleteTypes.None);
            var raiinKbnInfs = _tenantDataContext.RaiinKbnInfs.Where(x => x.IsDelete == DeleteTypes.None);
            var raiinKbnDetails = _tenantDataContext.RaiinKbnDetails.Where(x => x.IsDeleted == DeleteTypes.None);
            // Pt (Patient)
            var ptInfs = _tenantDataContext.PtInfs.Where(x => x.IsDelete == DeleteTypes.None);
            var ptCmtInfs = _tenantDataContext.PtCmtInfs.Where(x => x.IsDeleted == DeleteTypes.None);
            var ptHokenPatterns = _tenantDataContext.PtHokenPatterns.Where(x => x.IsDeleted == DeleteTypes.None);
            var ptKohis = _tenantDataContext.PtKohis.Where(x => x.IsDeleted == DeleteTypes.None);
            // Rsv (Reservation)
            var rsvInfs = _tenantDataContext.RsvInfs;
            var rsvFrameMsts = _tenantDataContext.RsvFrameMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // User (Doctor)
            var userMsts = _tenantDataContext.UserMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // Ka (Department)
            var kaMsts = _tenantDataContext.KaMsts.Where(x => x.IsDeleted == DeleteTypes.None);
            // Lock (Function lock)
            var lockInfs = _tenantDataContext.LockInfs.Where(x =>
                x.FunctionCd == FunctionCode.MedicalExaminationCode || x.FunctionCd == FunctionCode.TeamKarte);
            // Uketuke
            var uketukeSbtMsts = _tenantDataContext.UketukeSbtMsts.Where(x => x.IsDeleted == DeleteTypes.None);

            // 2. Filter collections by parameters
            var filteredRaiinInfs = raiinInfs.Where(x => x.HpId == hpId && x.SinDate == sinDate);
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
            var grpIds = _tenantDataContext.RaiinKbnMsts.Where(x => x.HpId == hpId && x.IsDeleted == DeleteTypes.None).Select(x => x.GrpCd).ToList();
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

        public bool UpdateStatus(int hpId, long raiinNo, int status)
        {
            return Update(hpId, raiinNo, r => r.Status = status);
        }

        public bool UpdateUketukeNo(int hpId, long raiinNo, int uketukeNo)
        {
            return Update(hpId, raiinNo, r => r.UketukeNo = uketukeNo);
        }

        public bool UpdateUketukeTime(int hpId, long raiinNo, string uketukeTime)
        {
            return Update(hpId, raiinNo, r => r.UketukeTime = uketukeTime);
        }

        public bool UpdateSinStartTime(int hpId, long raiinNo, string sinStartTime)
        {
            return Update(hpId, raiinNo, r => r.SinStartTime = sinStartTime);
        }

        public bool UpdateUketukeSbt(int hpId, long raiinNo, int uketukeSbt)
        {
            return Update(hpId, raiinNo, r => r.UketukeSbt = uketukeSbt);
        }

        public bool UpdateTantoId(int hpId, long raiinNo, int tantoId)
        {
            return Update(hpId, raiinNo, r => r.TantoId = tantoId);
        }

        public bool UpdateKaId(int hpId, long raiinNo, int kaId)
        {
            return Update(hpId, raiinNo, r => r.KaId = kaId);
        }

        private bool Update(int hpId, long raiinNo, Action<RaiinInf> updateEntity)
        {
            var raiinInf = _tenantDataContext.RaiinInfs.AsTracking().Where(r =>
                r.HpId == hpId
                && r.RaiinNo == raiinNo
                && r.IsDeleted == DeleteTypes.None).FirstOrDefault();
            if (raiinInf is null)
            {
                return false;
            }

            updateEntity(raiinInf);
            raiinInf.UpdateDate = DateTime.UtcNow;
            raiinInf.UpdateId = TempIdentity.UserId;
            raiinInf.UpdateMachine = TempIdentity.ComputerName;
            _tenantDataContext.SaveChanges();
            return true;
        }

        public List<ReceptionModel> GetReceptionVisiting(long raiinNo)
        {
            var listDataRaiinInf = _tenantDataContext.RaiinInfs
                .Where(x => x.RaiinNo == raiinNo)
                .Select(x => new ReceptionModel(
                x.RaiinNo,
                x.UketukeId,
                x.KaId,
                x.UketukeTime ?? String.Empty,
                x.SinStartTime ?? String.Empty,
                x.Status, x.YoyakuId, x.TantoId))
                .ToList();
            return listDataRaiinInf;
        }
    }
}
