using Domain.Models.SetGenerationMst;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class SetGenerationMstRepository : RepositoryBase, ISetGenerationMstRepository
    {
        private readonly StackExchange.Redis.IDatabase _cache;
        private readonly string key;
        public SetGenerationMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
            key = GetCacheKey() + "SetGenerationMst";
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        private IEnumerable<SetGenerationMstModel> ReloadCache()
        {
            var setGenerationMstList = NoTrackingDataContext.SetGenerationMsts.Where(s => s.HpId == 1 && s.IsDeleted == 0).Select(s =>
                    new SetGenerationMstModel(
                        s.HpId,
                        s.GenerationId,
                        s.StartDate,
                        s.IsDeleted
                    )
                  ).ToList();

            var json = JsonSerializer.Serialize(setGenerationMstList);
            _cache.StringSet(key, json);

            return setGenerationMstList;
        }

        public IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate)
        {
            IEnumerable<SetGenerationMstModel>? setGenerationMstList =

Enumerable.Empty<SetGenerationMstModel>();
            if (!_cache.KeyExists(key))
            {
                setGenerationMstList = ReloadCache();
            }
            else
            {
                setGenerationMstList = ReadCache();
            }

            return setGenerationMstList!.Where(s => s.StartDate <= sinDate).OrderByDescending(x => x.StartDate).ToList();
        }

        private List<SetGenerationMstModel> ReadCache()
        {
            var results = _cache.StringGet(key);
            var json = results.AsString();
            var datas = JsonSerializer.Deserialize<List<SetGenerationMstModel>>(json);
            return datas ?? new();
        }

        public int GetGenerationId(int hpId, int sinDate)
        {
            int generationId = 0;
            try
            {
                var setGenerationMstList = GetList(hpId, sinDate);
                var generation = setGenerationMstList.OrderByDescending(x => x.StartDate).FirstOrDefault();
                if (generation != null)
                {
                    generationId = generation.GenerationId;
                }
            }
            catch
            {
                return 0;
            }
            return generationId;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
