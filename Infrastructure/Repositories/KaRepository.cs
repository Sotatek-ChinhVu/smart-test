using Domain.Models.Ka;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KaRepository : IKaRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantDataContext;
    public KaRepository(ITenantProvider tenantProvider)
    {
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public bool CheckKaId(int KaId)
    {
        var check = _tenantDataContext.KaMsts.Any(k => k.KaId == KaId && k.IsDeleted == 0);
        return check;
    }
    public bool CheckKaId0(List<int> KaIds)
    {
        var countKaMsts = _tenantNoTrackingDataContext.KaMsts.Count(u => KaIds.Contains(u.KaId));
        return KaIds.Count == countKaMsts;
    }

    public KaMstModel GetByKaId(int kaId)
    {
        var entity = _tenantNoTrackingDataContext.KaMsts
            .Where(k => k.KaId == kaId && k.IsDeleted == DeleteTypes.None).FirstOrDefault();
        return entity is null ? new KaMstModel() : ConvertToKaMstModel(entity);
    }

    public List<KaMstModel> GetByKaIds(List<int> kaIds)
    {
        var entities = _tenantNoTrackingDataContext.KaMsts
           .Where(k => kaIds.Contains(k.KaId) && k.IsDeleted == DeleteTypes.None).AsEnumerable();
        return entities is null ? new List<KaMstModel>() : entities.Select(e => ConvertToKaMstModel(e)).ToList();
    }

    public List<KaMstModel> GetList()
    {
        return _tenantNoTrackingDataContext.KaMsts
            .Where(k => k.IsDeleted == DeleteTypes.None)
            .OrderBy(k => k.SortNo).AsEnumerable()
            .Select(k => ConvertToKaMstModel(k)).ToList();
    }

    public List<KaCodeMstModel> GetListKacode()
    {
        return _tenantNoTrackingDataContext.KacodeMsts
                                            .OrderBy(u => u.ReceKaCd)
                                            .Select(ka => new KaCodeMstModel(
                                                        ka.ReceKaCd,
                                                        ka.SortNo,
                                                        ka.KaName
                                             )).ToList();
    }

    public bool SaveKaMst(int hpId, int userId, List<KaMstModel> kaMstModels)
    {
        bool status = false;
        try
        {
            var listKaMsts = _tenantDataContext.KaMsts.Where(item => item.IsDeleted != 1).ToList();
            int sortNo = 1;
            List<KaMst> listAddNews = new();
            foreach (var model in kaMstModels)
            {
                var entity = listKaMsts.FirstOrDefault(mst => mst.Id == model.Id && mst.HpId == hpId);
                if (entity == null)
                {
                    entity = new KaMst();
                    entity.HpId = hpId;
                    entity.Id = 0;
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreateId = userId;
                }
                entity.KaId = model.KaId;
                entity.SortNo = sortNo;
                entity.ReceKaCd = model.ReceKaCd;
                entity.KaSname = model.KaSname;
                entity.KaName = model.KaSname;
                entity.IsDeleted = 0;
                entity.UpdateDate = DateTime.UtcNow;
                entity.UpdateId = userId;
                if (entity.Id == 0)
                {
                    listAddNews.Add(entity);
                }
                sortNo++;
            }
            _tenantDataContext.KaMsts.AddRange(listAddNews);

            var listKaIdModel = kaMstModels.Select(model => model.Id).ToList();
            var listKaDeletes = listKaMsts.Where(model => !listKaIdModel.Contains(model.Id)).ToList();
            foreach (var mst in listKaDeletes)
            {
                mst.IsDeleted = 1;
                mst.UpdateDate = DateTime.UtcNow;
                mst.UpdateId = userId;
            }
            _tenantDataContext.SaveChanges();
            status = true;
            return status;
        }
        catch (Exception)
        {
            return status;
        }
    }



    private KaMstModel ConvertToKaMstModel(KaMst k)
    {
        return new KaMstModel(
            k.Id,
            k.KaId,
            k.SortNo,
            k.ReceKaCd ?? string.Empty,
            k.KaSname ?? string.Empty,
            k.KaName ?? string.Empty);
    }
}
