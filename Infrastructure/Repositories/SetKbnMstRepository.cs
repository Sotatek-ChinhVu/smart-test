using Domain.Constant;
using Domain.Models.SetKbnMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Data;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class SetKbnMstRepository : RepositoryBase, ISetKbnMstRepository
    {
        private readonly string key;
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;
        public SetKbnMstRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            key = GetCacheKey() + "SetKbn";
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        public void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != null && RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }
        private IEnumerable<SetKbnMstModel> ReloadCache(int hpId)
        {
            var setKbnMstList = NoTrackingDataContext.SetKbnMsts.Where(s => s.HpId == hpId && s.IsDeleted == 0).Select(s =>
                    new SetKbnMstModel(
                        s.HpId,
                        s.SetKbn,
                        s.SetKbnEdaNo,
                        string.IsNullOrEmpty(s.SetKbnName) ? String.Empty : s.SetKbnName,
                        s.KaCd,
                        s.DocCd,
                        s.IsDeleted,
                        s.GenerationId
                    )
                  ).ToList();
            var json = JsonSerializer.Serialize(setKbnMstList);
            _cache.StringSet(key, json);

            return setKbnMstList;
        }

        private IEnumerable<SetKbnMstModel> ReadCache()
        {
            var results = _cache.StringGet(key);
            var json = results.AsString();
            var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<SetKbnMstModel>>(json) : new();
            return datas ?? new();
        }

        public IEnumerable<SetKbnMstModel> GetList(int hpId, int setKbnFrom, int setKbnTo)
        {
            IEnumerable<SetKbnMstModel> setKbnMstList;
            if (!_cache.KeyExists(key))
            {
                setKbnMstList = ReloadCache(hpId);
            }
            else
            {
                setKbnMstList = ReadCache();
            }

            return setKbnMstList!.Where(s => s.HpId == hpId && s.SetKbn >= setKbnFrom && s.SetKbn <= setKbnTo && s.IsDeleted == 0).OrderBy(s => s.SetKbn).ToList();
        }

        public bool Upsert(int hpId, int userId, int generationId, List<SetKbnMstModel> setKbnMstModels)
        {
            int maxSetKbn = NoTrackingDataContext.SetKbnMsts.Where(s => s.GenerationId == generationId && s.HpId == hpId).Select(s => s.SetKbn).ToList().DefaultIfEmpty(0).Max();
            foreach (var model in setKbnMstModels)
            {
                maxSetKbn++;
                var setKbnMst = TrackingDataContext.SetKbnMsts.FirstOrDefault(x => x.HpId == hpId &&
                                                                                     x.IsDeleted == 0 &&
                                                                                     x.SetKbn == model.SetKbn &&
                                                                                     x.GenerationId == generationId);
                if (setKbnMst != null)
                {
                    if (setKbnMst.IsDeleted == DeleteTypes.Deleted)
                    {
                        setKbnMst.SetKbn = DeleteTypes.Deleted;
                        setKbnMst.UpdateId = userId;
                        setKbnMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        setKbnMst.SetKbnName = model.SetKbnName;
                        setKbnMst.UpdateId = userId;
                        setKbnMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }

                }
                else
                {
                    var newSetKbnMst = new SetKbnMst
                    {
                        HpId = model.HpId,
                        IsDeleted = 0,
                        SetKbnName = model.SetKbnName,
                        GenerationId = generationId,
                        KaCd = model.KaCd,
                        DocCd = model.DocCd,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        SetKbnEdaNo = 0,
                        SetKbn = maxSetKbn
                    };
                    TrackingDataContext.SetKbnMsts.Add(newSetKbnMst);
                }
            }
            var check = TrackingDataContext.SaveChanges() > 0;
            if (check)
            {
                ReloadCache(hpId);
            }
            return check;
        }

        public List<SetKbnMstModel> GetSetKbnMstListByGenerationId(int hpId, int generationId)
        {
            var listSetKbnMst = NoTrackingDataContext.SetKbnMsts.Where(item => item.HpId == hpId
                                                                               && item.IsDeleted == 0
                                                                               && item.GenerationId == generationId
                                                                               && (item.SetKbn >= SetNameConst.SetKbn1 && item.SetKbn <= SetNameConst.SetKbn9
                                                                                   || item.SetKbn == SetNameConst.SetKbn10))
                                                                .ToList();
            return listSetKbnMst.Select(item => new SetKbnMstModel(
                                                    item.HpId,
                                                    item.SetKbn,
                                                    item.SetKbnEdaNo,
                                                    item.SetKbnName ?? string.Empty,
                                                    item.KaCd,
                                                    item.DocCd,
                                                    item.IsDeleted,
                                                    item.GenerationId))
                                .OrderBy(s => s.SetKbn)
                                .ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
