using Domain.Models.HpInf;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class HpInfRepository : RepositoryBase, IHpInfRepository
    {
        public HpInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public bool CheckHpId(int hpId)
        {
            var check = NoTrackingDataContext.HpInfs.Any(hp => hp.HpId == hpId);

            return check;

        }

        public HpInfModel GetHpInf(int hpId)
        {
            var hpInf = NoTrackingDataContext.HpInfs.FirstOrDefault(item => item.HpId == hpId);
            return hpInf != null ? new HpInfModel(hpId,
                                                    hpInf.StartDate,
                                                    hpInf.HpCd ?? string.Empty,
                                                    hpInf.RousaiHpCd ?? string.Empty,
                                                    hpInf.HpName ?? string.Empty,
                                                    hpInf.ReceHpName ?? string.Empty,
                                                    hpInf.KaisetuName ?? string.Empty,
                                                    hpInf.PostCd ?? string.Empty,
                                                    hpInf.PrefNo,
                                                    hpInf.Address1 ?? string.Empty,
                                                    hpInf.Address2 ?? string.Empty,
                                                    hpInf.Tel ?? string.Empty,
                                                    hpInf.FaxNo ?? string.Empty,
                                                    hpInf.OtherContacts ?? string.Empty
                                                ) : new HpInfModel();
        }

        public List<HpInfModel> GetListHpInf(int hpId)
        {
            var hpInfs = NoTrackingDataContext.HpInfs.Where(u => u.HpId == hpId).OrderBy(u => u.StartDate).ToList();
            if (hpInfs == null)
            {
                return new();
            }

            return hpInfs.Select(h => new HpInfModel(h.HpId,
                                                     h.StartDate,
                                                     h.HpCd ?? string.Empty,
                                                     h.RousaiHpCd ?? string.Empty,
                                                     h.HpName ?? string.Empty,
                                                     h.ReceHpName ?? string.Empty,
                                                     h.KaisetuName ?? string.Empty,
                                                     h.PostCd ?? string.Empty,
                                                     h.PrefNo,
                                                     h.Address1 ?? string.Empty,
                                                     h.Address2 ?? string.Empty,
                                                     h.Tel ?? string.Empty,
                                                     h.FaxNo ?? string.Empty,
                                                     h.OtherContacts ?? string.Empty))
                          .ToList();
        }
        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public bool SaveHpInf(int userId, List<HpInfModel> hpInfModels)
        {
            var addedModels = hpInfModels.Where(k => k.HpInfModelStatus == ModelStatus.Added);
            var updatedModels = hpInfModels.Where(k => k.HpInfModelStatus == ModelStatus.Modified);
            var deletedModels = hpInfModels.Where(k => k.HpInfModelStatus == ModelStatus.Deleted);

            if (deletedModels.Any())
            {
                var modelsToDelete = TrackingDataContext.HpInfs.Where(x => deletedModels.Any(d => d.HpId == x.HpId && d.StartDate == x.StartDate));
                TrackingDataContext.HpInfs.RemoveRange(modelsToDelete);
            }

            if (updatedModels.Any())
            {
                foreach (var model in updatedModels)
                {
                    TrackingDataContext.HpInfs.Update(new HpInf()
                    {
                        HpId = model.HpId,
                        StartDate = model.StartDate,
                        HpCd = model.HpCd,
                        RousaiHpCd = model.RousaiHpCd,
                        HpName = model.HpName,
                        ReceHpName = model.ReceHpName,
                        KaisetuName = model.KaisetuName,
                        PostCd = model.PostCd,
                        PrefNo = model.PrefNo,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        Tel = model.Tel,
                        UpdateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        FaxNo = model.FaxNo,
                        OtherContacts = model.OtherContacts
                    });
                }
            }

            if (addedModels.Any())
            {
                foreach (var model in addedModels)
                {
                    TrackingDataContext.HpInfs.Add(new HpInf()
                    {
                        HpId = model.HpId,
                        StartDate = model.StartDate,
                        HpCd = model.HpCd,
                        RousaiHpCd = model.RousaiHpCd,
                        HpName = model.HpName,
                        ReceHpName = model.ReceHpName,
                        KaisetuName = model.KaisetuName,
                        PostCd = model.PostCd,
                        PrefNo = model.PrefNo,
                        Address1 = model.Address1,
                        Address2 = model.Address2,
                        Tel = model.Tel,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        FaxNo = model.FaxNo,
                        OtherContacts = model.OtherContacts
                    });
                }
            }

            return TrackingDataContext.SaveChanges() > 0;
        }
    }
}
