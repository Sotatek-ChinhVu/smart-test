using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Repositories
{
    public class HpInfRepository : RepositoryBase, IHpInfRepository
    {
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IInsuranceMstRepository _insuranceMstRepository;
        private readonly IDatabase _cache;
        public HpInfRepository(ITenantProvider tenantProvider, IInsuranceRepository insuranceRepository, IInsuranceMstRepository insuranceMstRepository) : base(tenantProvider)
        {
            _insuranceRepository = insuranceRepository;
            _insuranceMstRepository = insuranceMstRepository;
            _cache = RedisConnectorHelper.Connection.GetDatabase();
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
                var modelsToDelete = TrackingDataContext.HpInfs.AsEnumerable().Where(x => deletedModels.Any(d => d.HpId == x.HpId && d.StartDate == x.StartDate)).ToList();
                TrackingDataContext.HpInfs.RemoveRange(modelsToDelete);
            }

            if (updatedModels.Any())
            {
                foreach (var model in updatedModels)
                {
                    var hpInfUpdate = TrackingDataContext.HpInfs.FirstOrDefault(x => x.HpId == model.HpId && x.StartDate == model.StartDate);
                    if (hpInfUpdate != null)
                    {
                        hpInfUpdate.HpCd = model.HpCd;
                        hpInfUpdate.RousaiHpCd = model.RousaiHpCd;
                        hpInfUpdate.HpName = model.HpName;
                        hpInfUpdate.ReceHpName = model.ReceHpName;
                        hpInfUpdate.KaisetuName = model.KaisetuName;
                        hpInfUpdate.PostCd = model.PostCd;
                        hpInfUpdate.PrefNo = model.PrefNo;
                        hpInfUpdate.Address1 = model.Address1;
                        hpInfUpdate.Address2 = model.Address2;
                        hpInfUpdate.Tel = model.Tel;
                        hpInfUpdate.UpdateId = userId;
                        hpInfUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        hpInfUpdate.FaxNo = model.FaxNo;
                        hpInfUpdate.OtherContacts = model.OtherContacts;
                    }
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
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        FaxNo = model.FaxNo,
                        OtherContacts = model.OtherContacts
                    });
                }
            }

            var result = TrackingDataContext.SaveChanges() > 0;
            if (result)
            {
                var keyInsurance = _insuranceRepository.GetNameKeys(0, 0).FirstOrDefault();
                if (!string.IsNullOrEmpty(keyInsurance))
                {
                    _cache.KeyDelete(keyInsurance);
                }
                var keyInsuranceMst = _insuranceMstRepository.GetNameKeys(0, 0).FirstOrDefault();
                if (!string.IsNullOrEmpty(keyInsuranceMst))
                {
                    _cache.KeyDelete(keyInsuranceMst);
                }
            }
            return result;
        }
    }
}
