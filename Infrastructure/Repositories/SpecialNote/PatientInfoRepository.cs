using Domain.Models.SpecialNote.PatientInfo;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Repositories.SpecialNote
{
    public class PatientInfoRepository : RepositoryBase, IPatientInfoRepository
    {
        public PatientInfoRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<KensaInfDetailModel> GetListKensaInfModel(int hpId, long ptId, int sinDate)
        {
            var listKensaInfDetail = NoTrackingDataContext.KensaInfDetails.Where(u => u.HpId == hpId
                                                                                && u.PtId == ptId
                                                                                && u.IsDeleted == 0
                                                                                && (u.KensaItemCd == "V0001"
                                                                                || u.KensaItemCd == "V0002"
                                                                                || u.KensaItemCd == "V0003"))
                                                                        .OrderByDescending(u => u.IraiDate);
            if (listKensaInfDetail.Any())
            {
                int maxIraiDate = listKensaInfDetail.Max(item => item.IraiDate);
                return listKensaInfDetail.Where(item => item.IraiDate == maxIraiDate)
                                         .Select(item => new KensaInfDetailModel(
                                            item.HpId,
                                            item.PtId,
                                            item.IraiCd,
                                            item.SeqNo,
                                            item.IraiDate,
                                            item.RaiinNo,
                                            item.KensaItemCd ?? string.Empty,
                                            item.ResultVal ?? string.Empty,
                                            item.ResultType ?? string.Empty,
                                            item.AbnormalKbn ?? string.Empty,
                                            item.IsDeleted,
                                            item.CmtCd1 ?? string.Empty,
                                            item.CmtCd2 ?? string.Empty,
                                            item.UpdateDate,
                                            string.Empty,
                                            string.Empty,
                                            0
                                        )).ToList();
            }
            return new();
        }

        public List<PhysicalInfoModel> GetPhysicalList(int hpId, long ptId)
        {
            var physicals = new List<PhysicalInfoModel>();
            var allKensaInfDetails = NoTrackingDataContext.KensaInfDetails.Where(x => x.PtId == ptId && x.IsDeleted == 0 && (x.KensaItemCd != null && x.KensaItemCd.StartsWith("V")))?.GroupBy(item => new { item.KensaItemCd, item.IraiDate })
               .Select(item => item.OrderByDescending(x => x.SeqNo).FirstOrDefault()).ToList();
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(x => x.HpId == hpId && x.IsDelete == 0 && x.KensaItemCd.StartsWith("V")).OrderBy(mst => mst.SortNo);

            foreach (var kensaMst in kensaMsts)
            {
                var kensaInfDetails = allKensaInfDetails?.Where(k => k != null && k.KensaItemCd == kensaMst.KensaItemCd).ToList();
                var physical = new PhysicalInfoModel(
                  kensaMst.HpId,
                  kensaMst.KensaItemCd,
                  kensaMst.KensaItemSeqNo,
                  kensaMst.CenterCd ?? String.Empty,
                  kensaMst.KensaName ?? String.Empty,
                  kensaMst.KensaKana ?? String.Empty,
                  kensaMst.Unit ?? String.Empty,
                  kensaMst.MaterialCd,
                  kensaMst.ContainerCd,
                  kensaMst.MaleStd ?? String.Empty,
                  kensaMst.MaleStdLow ?? String.Empty,
                  kensaMst.FemaleStdHigh ?? String.Empty,
                  kensaMst.FemaleStd ?? String.Empty,
                  kensaMst.FemaleStdLow ?? String.Empty,
                  kensaMst.FemaleStdHigh ?? String.Empty,
                  kensaMst.Formula ?? String.Empty,
                  kensaMst.OyaItemCd ?? String.Empty,
                  kensaMst.OyaItemSeqNo,
                  kensaMst.SortNo,
                  kensaMst.CenterItemCd1 ?? String.Empty,
                  kensaMst.CenterItemCd2 ?? String.Empty,
                  kensaMst.IsDelete,
                  kensaMst.Digit,
                  kensaInfDetails == null ? new List<KensaInfDetailModel>() : kensaInfDetails.Select(kd =>
                      new KensaInfDetailModel(
                        kd?.HpId ?? 0,
                        kd?.PtId ?? 0,
                        kd?.IraiCd ?? 0,
                        kd?.SeqNo ?? 0,
                        kd?.IraiDate ?? 0,
                        kd?.RaiinNo ?? 0,
                        kd?.KensaItemCd ?? String.Empty,
                        kd?.ResultVal ?? String.Empty,
                        kd?.ResultType ?? String.Empty,
                        kd?.AbnormalKbn ?? String.Empty,
                        kd?.IsDeleted ?? 0,
                        kd?.CmtCd1 ?? String.Empty,
                        kd?.CmtCd2 ?? String.Empty,
                        kd?.UpdateDate ?? DateTime.MinValue,
                        string.Empty,
                        string.Empty,
                        0
                      )).ToList()
                    );
                physicals.Add(physical);
            }
            return physicals;
        }

        public List<PtPregnancyModel> GetPregnancyList(long ptId, int hpId)
        {
            var ptPregnancys = NoTrackingDataContext.PtPregnancies.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0)
              .Select(x => new PtPregnancyModel(
                x.Id,
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.StartDate,
                x.EndDate,
                x.PeriodDate,
                x.PeriodDueDate,
                x.OvulationDate,
                x.OvulationDueDate,
                x.IsDeleted,
                x.UpdateDate,
                x.UpdateId,
                x.UpdateMachine ?? String.Empty,
                0
            ));
            return ptPregnancys.ToList();
        }

        public List<PtPregnancyModel> GetPregnancyList(long ptId, int hpId, int sinDate)
        {
            var ptPregnancys = NoTrackingDataContext.PtPregnancies.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0 && x.StartDate <= sinDate && x.EndDate >= sinDate)
              .Select(x => new PtPregnancyModel(
                x.Id,
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.StartDate,
                x.EndDate,
                x.PeriodDate,
                x.PeriodDueDate,
                x.OvulationDate,
                x.OvulationDueDate,
                x.IsDeleted,
                x.UpdateDate,
                x.UpdateId,
                x.UpdateMachine ?? String.Empty,
                sinDate
            ));
            return ptPregnancys.AsEnumerable().OrderByDescending(item => item.StartDate).ToList();
        }

        public List<SeikaturekiInfModel> GetSeikaturekiInfList(long ptId, int hpId)
        {
            var seikaturekiInfs = NoTrackingDataContext.SeikaturekiInfs.Where(x => x.PtId == ptId && x.HpId == hpId).OrderByDescending(x => x.UpdateDate).Select(x => new SeikaturekiInfModel(
                x.Id,
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.Text ?? String.Empty
            ));
            return seikaturekiInfs.ToList();
        }

        public List<KensaInfDetailModel> GetListKensaInfDetailModel(int hpId, long ptId, int sinDate)
        {
            var result = new List<KensaInfDetailModel>();
            var KensaMstRepos = NoTrackingDataContext.KensaMsts.Where(k => k.HpId == hpId && k.IsDelete == 0 && k.KensaItemCd.StartsWith("V"))
                .Select(u => new
                {
                    KensaName = u.KensaName,
                    KensaItemCd = u.KensaItemCd,
                    Unit = u.Unit,
                    SortNo = u.SortNo
                });
            var kensaInfDetailRepos = NoTrackingDataContext.KensaInfDetails.Where(d => d.HpId == hpId
                                                                                                    && d.PtId == ptId
                                                                                                    && d.IsDeleted == 0
                                                                                                    && d.IraiDate <= sinDate
                                                                                                    && d.KensaItemCd != null && d.KensaItemCd.StartsWith("V")
                                                                                                    && !string.IsNullOrEmpty(d.ResultVal));
            var query = from KensaMst in KensaMstRepos
                        join kensaInfDetail in kensaInfDetailRepos on
                        KensaMst.KensaItemCd equals kensaInfDetail.KensaItemCd into listDetail
                        select new
                        {
                            KensaMst = KensaMst,
                            KensaInfDetail = listDetail.OrderByDescending(item => item.IraiDate).ThenByDescending(item => item.UpdateDate).FirstOrDefault()
                        };

            result = query.AsEnumerable().Select(u => new KensaInfDetailModel(
                u.KensaMst.KensaItemCd,
                u.KensaMst.Unit,
                u.KensaMst.KensaName,
                u.KensaMst.SortNo
            )).ToList();

            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public bool SaveKensaInfWeightedConfirmation(int hpId, long ptId, long raiinNo, double weight, int sinDate , int userId)
        {
            EntityEntry<KensaInf> entryKensaInf = TrackingDataContext.KensaInfs.Add(new KensaInf()
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
            bool successKensa = TrackingDataContext.SaveChanges() > 0;
            if (!successKensa)
                return false;
            else
            {
                TrackingDataContext.KensaInfDetails.Add(new KensaInfDetail()
                {
                    HpId = Session.HospitalID,
                    PtId = ptId,
                    IraiDate = sinDate,
                    RaiinNo = raiinNo,
                    IraiCd = entryKensaInf.Entity.IraiCd,
                    KensaItemCd = IraiCodeConstant.WEIGHT_CODE,
                    ResultVal = weight.ToString(),
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId
                });
                return TrackingDataContext.SaveChanges() > 0;
            }
        }
    }
}
