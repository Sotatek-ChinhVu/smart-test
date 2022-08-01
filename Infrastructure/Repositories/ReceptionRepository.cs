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

        public ReceptionModel? Get(long raiinNo)
        {
            var receptionEntity = _tenantDataContext.RaiinInfs.SingleOrDefault(r => r.RaiinNo == raiinNo);

            if (receptionEntity == null)
            {
                return null;
            }

            return new ReceptionModel
                (
                    receptionEntity.HpId,
                    receptionEntity.PtId,
                    receptionEntity.SinDate,
                    receptionEntity.RaiinNo,
                    receptionEntity.OyaRaiinNo,
                    receptionEntity.HokenPid,
                    receptionEntity.SanteiKbn,
                    receptionEntity.Status,
                    receptionEntity.IsYoyaku,
                    receptionEntity.YoyakuTime ?? string.Empty,
                    receptionEntity.YoyakuId,
                    receptionEntity.UketukeSbt,
                    receptionEntity.UketukeTime ?? string.Empty,
                    receptionEntity.UketukeId,
                    receptionEntity.UketukeNo,
                    receptionEntity.SinStartTime,
                    receptionEntity.SinEndTime ?? string.Empty,
                    receptionEntity.KaikeiTime ?? string.Empty,
                    receptionEntity.KaikeiId,
                    receptionEntity.KaId,
                    receptionEntity.TantoId,
                    receptionEntity.SyosaisinKbn,
                    receptionEntity.JikanKbn
                );
        }

        public List<ReceptionRowModel> GetList(int hpId, int sinDate)
        {
            return GetReceptionRowModels(hpId, sinDate);
        }

        public List<ReceptionModel> GetList(int hpId, long ptId, int karteDeleteHistory)
        {
            var result =  _tenantDataContext.RaiinInfs.Where
                                (r =>
                                    r.HpId == hpId && r.PtId == ptId && r.Status >= 3 &&
                                 (r.IsDeleted == DeleteTypes.None || karteDeleteHistory == 1 || (r.IsDeleted != DeleteTypes.Confirm && karteDeleteHistory == 2))).ToList();
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
                        r.JikanKbn
                   )).ToList();

        }

        private List<ReceptionRowModel> GetReceptionRowModels(int hpId, int sinDate)
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

            // 2. Perform the join operation
            var raiinQuery =
                from raiinInf in raiinInfs.Where(x => x.HpId == hpId && x.SinDate == sinDate)
                join ptInf in ptInfs on
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
                    uketukeSbt = relatedUketukeSbtMst,
                    tanto = relatedTanto,
                    primaryDoctorName = relatedPrimaryDoctor.Sname,
                    ka = relatedKaMst,
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
                r.uketukeSbt?.KbnName ?? string.Empty,
                r.uketukeSbt?.KbnId ?? CommonConstants.InvalidId,
                r.raiinInf.UketukeTime ?? string.Empty,
                r.raiinInf.SinStartTime ?? string.Empty,
                r.raiinInf.SinEndTime ?? string.Empty,
                r.raiinInf.KaikeiTime ?? string.Empty,
                r.relatedRaiinCmtInfComment?.Text ?? string.Empty,
                r.ptCmtInf?.Text ?? string.Empty,
                r.tanto?.Sname ?? string.Empty,
                r.tanto?.UserId ?? CommonConstants.InvalidId,
                r.ka?.KaSname ?? string.Empty,
                r.ka?.KaId ?? CommonConstants.InvalidId,
                r.lastVisitDate,
                r.primaryDoctorName ?? string.Empty,
                r.relatedRaiinCmtInfRemark?.Text ?? string.Empty,
                r.raiinInf.ConfirmationState,
                r.raiinInf.ConfirmationResult ?? string.Empty,
                grpIds,
                dynamicCells: r.raiinKbnDetails.Select(d => new DynamicCell(d.GrpCd, d.KbnCd, d.KbnName, d.ColorCd ?? string.Empty)).ToList(),
                sinDate
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
            _tenantDataContext.SaveChanges();
            return true;
        }
    }
}
