using Domain.Models.PatientInfor;
using Domain.Models.SpecialNote.PatientInfo;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories.SpecialNote
{
    public class PatientInfoRepository : IPatientInfoRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public PatientInfoRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<PhysicalInfoModel> GetPhysicalList(int hpId, long ptId)
        {
            var physicals = new List<PhysicalInfoModel>();
            var allKensaInfDetails = _tenantDataContext.KensaInfDetails.Where(x => x.PtId == ptId && x.IsDeleted == 0 && (x.KensaItemCd != null && x.KensaItemCd.StartsWith("V")))?.GroupBy(item => new { item.KensaItemCd, item.IraiDate })
               .Select(item => item.OrderByDescending(x => x.SeqNo).FirstOrDefault()).ToList();
            var kensaMsts = _tenantDataContext.KensaMsts.Where(x => x.HpId == hpId && x.IsDelete == 0 && x.KensaItemCd.StartsWith("V")).OrderBy(mst => mst.SortNo);

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
                        kd?.UpdateDate ?? DateTime.MinValue
                      )).ToList()
                    );
                physicals.Add(physical);
            }
            return physicals;
        }

        public List<PtPregnancyModel> GetPregnancyList(long ptId, int hpId)
        {
            var ptPregnancys = _tenantDataContext.PtPregnancies.Where(x => x.PtId == ptId && x.HpId == hpId && x.IsDeleted == 0).Select(x => new PtPregnancyModel(
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
                x.UpdateMachine ?? String.Empty
            ));
            return ptPregnancys.ToList();
        }

        public List<SeikaturekiInfModel> GetSeikaturekiInfList(long ptId, int hpId)
        {
            var seikaturekiInfs = _tenantDataContext.SeikaturekiInfs.Where(x => x.PtId == ptId && x.HpId == hpId).OrderByDescending(x => x.UpdateDate).Select(x => new SeikaturekiInfModel(
                x.Id,
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.Text ?? String.Empty
            ));
            return seikaturekiInfs.ToList();
        }
    }
}
