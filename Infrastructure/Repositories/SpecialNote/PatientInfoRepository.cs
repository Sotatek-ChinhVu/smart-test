using Domain.Models.SpecialNote.PatientInfo;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Repositories.SpecialNote
{
    public class PatientInfoRepository : RepositoryBase, IPatientInfoRepository
    {
        private const string WEIGHT_CD = "V0002";

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
                return listKensaInfDetail.Where(item => item.IraiDate == maxIraiDate).AsEnumerable()
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
            var ptPregnancys = NoTrackingDataContext.PtPregnancies.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0).AsEnumerable()
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
                x.UpdateMachine ?? string.Empty,
                0
            ));
            return ptPregnancys.ToList();
        }

        public List<PtPregnancyModel> GetPregnancyList(long ptId, int hpId, int sinDate)
        {
            var ptPregnancys = NoTrackingDataContext.PtPregnancies.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0 && x.StartDate <= sinDate && x.EndDate >= sinDate).AsEnumerable()
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
            var seikaturekiInfs = NoTrackingDataContext.SeikaturekiInfs.Where(x => x.PtId == ptId && x.HpId == hpId).AsEnumerable().OrderByDescending(x => x.UpdateDate).Select(x => new SeikaturekiInfModel(
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

            var result = query.AsEnumerable().Select(u => new KensaInfDetailModel(
                 u.KensaMst.KensaItemCd,
                 u.KensaMst.Unit,
                 u.KensaMst.KensaName,
                 u.KensaMst.SortNo
             )).ToList();

            return result;
        }

        public KensaInfDetailModel GetPtWeight(long ptId, int sinDate)
        {
            var kensaInf = NoTrackingDataContext.KensaInfDetails.Where(k => k.PtId == ptId && k.IraiDate <= sinDate && k.KensaItemCd == WEIGHT_CD).OrderByDescending(p => p.IraiDate).FirstOrDefault();
            return kensaInf == null ? new() : new KensaInfDetailModel(kensaInf.HpId, kensaInf.PtId, kensaInf.IraiCd, kensaInf.SeqNo, kensaInf.IraiDate, kensaInf.RaiinNo, kensaInf.KensaItemCd ?? string.Empty, kensaInf.ResultVal ?? string.Empty, kensaInf.ResultType ?? string.Empty, kensaInf.AbnormalKbn ?? string.Empty, kensaInf.IsDeleted, kensaInf.CmtCd1 ?? string.Empty, kensaInf.CmtCd2 ?? string.Empty, kensaInf.UpdateDate, string.Empty, string.Empty, 0);
        }

        public List<GcStdInfModel> GetStdPoint(int hpId, int sex)
        {
            var list = NoTrackingDataContext.GcStdMsts.Where(item => item.HpId == hpId && (sex == 0 || item.Sex == sex)).AsEnumerable()
                .Select(item => new GcStdInfModel(item.HpId, item.StdKbn, item.Sex, item.Point, item.SdM25, item.SdM20, item.SdM10, item.SdAvg, item.SdP10, item.SdP20, item.SdP25, item.Per03, item.Per10, item.Per25, item.Per50, item.Per75, item.Per90, item.Per97)).ToList();
            return list;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
