using Domain.Models.Ka;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Repositories;

public class KaRepository : RepositoryBase, IKaRepository
{
    public KaRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public bool CheckKaId(int kaId)
    {
        var check = NoTrackingDataContext.KaMsts.Any(k => k.KaId == kaId && k.IsDeleted == 0);
        return check;
    }
    public bool CheckKaId(List<int> kaIds)
    {
        kaIds = kaIds.Distinct().ToList();
        var countKaMsts = NoTrackingDataContext.KaMsts.Count(u => kaIds.Contains(u.KaId));
        return kaIds.Count == countKaMsts;
    }

    public KaMstModel GetByKaId(int kaId)
    {
        var entity = NoTrackingDataContext.KaMsts
            .Where(k => k.KaId == kaId && k.IsDeleted == DeleteTypes.None).FirstOrDefault();
        return entity is null ? new KaMstModel() : ConvertToKaMstModel(entity);
    }

    public List<KaMstModel> GetByKaIds(List<int> kaIds)
    {
        var entities = NoTrackingDataContext.KaMsts
           .Where(k => kaIds.Contains(k.KaId) && k.IsDeleted == DeleteTypes.None).AsEnumerable();
        return entities is null ? new List<KaMstModel>() : entities.Select(e => ConvertToKaMstModel(e)).ToList();
    }

    public List<KaMstModel> GetList(int isDeleted)
    {
        return NoTrackingDataContext.KaMsts
            .Where(k => (isDeleted == 2 || k.IsDeleted == isDeleted))
            .OrderBy(k => k.SortNo).AsEnumerable()
            .Select(k => ConvertToKaMstModel(k)).ToList();
    }

    public List<KaCodeMstModel> GetListKacode()
    {
        return NoTrackingDataContext.KacodeMsts
                                            .OrderBy(u => u.ReceKaCd)
                                            .Select(ka => new KaCodeMstModel(
                                                        ka.ReceKaCd,
                                                        ka.SortNo,
                                                        ka.KaName ?? string.Empty,
                                                        string.Empty
                                             )).ToList();
    }

    public bool SaveKaMst(int hpId, int userId, List<KaMstModel> kaMstModels)
    {
        var listKaMsts = TrackingDataContext.KaMsts.Where(item => item.IsDeleted != 1).ToList();
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
                entity.CreateDate = CIUtil.GetJapanDateTimeNow();
                entity.CreateId = userId;
            }
            entity.KaId = model.KaId;
            entity.SortNo = sortNo;
            entity.ReceKaCd = model.ReceKaCd;
            entity.KaSname = model.KaSname;
            entity.KaName = model.KaName;
            entity.YousikiKaCd = model.YousikiKaCd;
            entity.IsDeleted = 0;
            entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
            entity.UpdateId = userId;
            if (entity.Id == 0)
            {
                listAddNews.Add(entity);
            }
            sortNo++;
        }
        TrackingDataContext.KaMsts.AddRange(listAddNews);

        var listKaIdModel = kaMstModels.Select(model => model.Id).ToList();
        var listKaDeletes = listKaMsts.Where(model => !listKaIdModel.Contains(model.Id)).ToList();
        foreach (var mst in listKaDeletes)
        {
            mst.IsDeleted = 1;
            mst.UpdateDate = CIUtil.GetJapanDateTimeNow();
            mst.UpdateId = userId;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    private static KaMstModel ConvertToKaMstModel(KaMst k)
    {
        return new KaMstModel(
            k.Id,
            k.KaId,
            k.SortNo,
            k.ReceKaCd ?? string.Empty,
            k.KaSname ?? string.Empty,
            k.KaName ?? string.Empty,
            k.YousikiKaCd ?? string.Empty
            );
    }

    public List<KaCodeMstModel> GetKacodeMstYossi()
    {
        var kacodeMsts = NoTrackingDataContext.KacodeMsts.AsQueryable();

        var kacodeReceYousikis = NoTrackingDataContext.KacodeReceYousikis.AsQueryable();

        var query = from kacodeMst in kacodeMsts
                    join kacodeReceYousiki in kacodeReceYousikis
                    on kacodeMst.ReceKaCd equals kacodeReceYousiki.ReceKaCd into TempKacodeReceYousikis
                    from tempKacodeReceYousiki in TempKacodeReceYousikis.DefaultIfEmpty()
                    select new
                    {
                        KacodeMst = kacodeMst,
                        KacodeReceYousiki = tempKacodeReceYousiki
                    };
        return query.AsEnumerable().Select(p => new KaCodeMstModel(p.KacodeMst?.ReceKaCd ?? string.Empty, p.KacodeMst?.SortNo ?? 0, p.KacodeMst?.KaName ?? string.Empty, p.KacodeReceYousiki?.YousikiKaCd ?? string.Empty)).OrderBy(p => p.ReceKaCd).ToList();
    }

    public List<KacodeYousikiMstModel> GetKacodeYousikiMst()
    {
        var kacodeMsts = NoTrackingDataContext.KacodeYousikiMsts.AsEnumerable().OrderBy(u => u.YousikiKaCd);
        return kacodeMsts.Select(p => new KacodeYousikiMstModel(p.YousikiKaCd, p.SortNo, p.KaName)).ToList();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
