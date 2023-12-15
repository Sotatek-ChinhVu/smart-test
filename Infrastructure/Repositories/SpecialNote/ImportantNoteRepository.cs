using Domain.Models.SpecialNote.ImportantNote;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Repositories.SpecialNote
{
    public class ImportantNoteRepository : RepositoryBase, IImportantNoteRepository
    {
        private readonly string key;
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;
        public ImportantNoteRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
        {
            key = GetDomainKey();
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        public void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public void AddAlrgyDrugList(List<PtAlrgyDrugModel> inputDatas, int hpId, int userId)
        {
            var ptId = inputDatas.FirstOrDefault()?.PtId ?? 0;
            var alrgyDrugs = NoTrackingDataContext.PtAlrgyDrugs.Where(a => a.HpId == hpId && a.PtId == ptId).ToList();
            var maxSortNo = !(alrgyDrugs?.Count > 0) ? 0 : alrgyDrugs.Max(a => a.SortNo);
            foreach (var item in inputDatas)
            {
                TrackingDataContext.Add(
                    new PtAlrgyDrug
                    {
                        HpId = hpId,
                        PtId = item.PtId,
                        SortNo = maxSortNo++,
                        ItemCd = item.ItemCd,
                        DrugName = item.DrugName,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Cmt = item.Cmt,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId
                    }
               );
            }

            TrackingDataContext.SaveChanges();
        }

        public List<PtAlrgyDrugModel> GetAlrgyDrugList(long ptId)
        {
            List<PtAlrgyDrugModel> ptAlrgyDrugs;

            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.PtAlrgyDrugGetList + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                ptAlrgyDrugs = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtAlrgyDrugModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                ptAlrgyDrugs = NoTrackingDataContext.PtAlrgyDrugs.AsEnumerable()
                                                                 .Where(x => x.PtId == ptId && x.IsDeleted == 0)
                                                                 .Select(x => new PtAlrgyDrugModel(
                                                                                  x.HpId,
                                                                                  x.PtId,
                                                                                  x.SeqNo,
                                                                                  x.SortNo,
                                                                                  x.ItemCd ?? string.Empty,
                                                                                  x.DrugName ?? string.Empty,
                                                                                  x.StartDate,
                                                                                  x.EndDate,
                                                                                  x.Cmt ?? string.Empty,
                                                                                  x.IsDeleted
                                                                 )).ToList();
                // Set data to new cache
                var json = JsonSerializer.Serialize(ptAlrgyDrugs);
                _cache.StringSet(finalKey, json);
            }
            return ptAlrgyDrugs;
        }

        public List<PtAlrgyDrugModel> GetAlrgyDrugList(long ptId, int sinDate)
        {
            var ptAlrgyDrugs = NoTrackingDataContext.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0).AsEnumerable().Select(x => new PtAlrgyDrugModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ItemCd ?? string.Empty,
               x.DrugName ?? string.Empty,
               x.StartDate,
               x.EndDate,
               x.Cmt ?? string.Empty,
               x.IsDeleted
            ));
            return ptAlrgyDrugs.AsEnumerable().Where(x => x.FullStartDate <= sinDate && sinDate <= x.FullEndDate).OrderBy(x => x.SortNo).ToList();
        }

        public List<PtAlrgyElseModel> GetAlrgyElseList(long ptId)
        {
            List<PtAlrgyElseModel> ptAlrgyElses;

            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.AlrgyElseGetList + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                ptAlrgyElses = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtAlrgyElseModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                ptAlrgyElses = NoTrackingDataContext.PtAlrgyElses.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                                                             .AsEnumerable()
                                                             .Select(x => new PtAlrgyElseModel(
                                                                              x.HpId,
                                                                              x.PtId,
                                                                              x.SeqNo,
                                                                              x.SortNo,
                                                                              x.AlrgyName ?? string.Empty,
                                                                              x.StartDate,
                                                                              x.EndDate,
                                                                              x.Cmt ?? string.Empty,
                                                                              x.IsDeleted
                                                             )).ToList();
                // Set data to new cache
                var json = JsonSerializer.Serialize(ptAlrgyElses);
                _cache.StringSet(finalKey, json);
            }
            return ptAlrgyElses;
        }

        public List<PtAlrgyElseModel> GetAlrgyElseList(long ptId, int sinDate)
        {
            var ptAlrgyElses = NoTrackingDataContext.PtAlrgyElses.Where(x => x.PtId == ptId && x.IsDeleted == 0).AsEnumerable().Select(x => new PtAlrgyElseModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.AlrgyName ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptAlrgyElses.AsEnumerable().Where(x => x.FullStartDate <= sinDate && sinDate <= x.FullEndDate).OrderBy(x => x.SortNo).ToList();
        }

        public List<PtAlrgyFoodModel> GetAlrgyFoodList(long ptId)
        {
            List<PtAlrgyFoodModel> result;

            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.AlrgyFoodGetList + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                result = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtAlrgyFoodModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                var aleFoodKbns = NoTrackingDataContext.M12FoodAlrgyKbn.ToList();
                var ptAlrgyFoods = NoTrackingDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0).ToList();
                result = (from ale in ptAlrgyFoods
                          join mst in aleFoodKbns on ale.AlrgyKbn equals mst.FoodKbn
                          select new PtAlrgyFoodModel
                          (
                                ale.HpId,
                                ale.PtId,
                                ale.SeqNo,
                                ale.SortNo,
                                ale.AlrgyKbn ?? string.Empty,
                                ale.StartDate,
                                ale.EndDate,
                                ale.Cmt ?? string.Empty,
                                ale.IsDeleted,
                                mst.FoodName ?? string.Empty
                          )).ToList();
                // Set data to new cache
                var json = JsonSerializer.Serialize(result);
                _cache.StringSet(finalKey, json);
            }
            return result;
        }

        public List<PtAlrgyFoodModel> GetAlrgyFoodList(long ptId, int sinDate)
        {
            var aleFoodKbns = NoTrackingDataContext.M12FoodAlrgyKbn.ToList();
            var ptAlrgyFoods = NoTrackingDataContext.PtAlrgyFoods.Where(x => x.PtId == ptId && x.IsDeleted == 0).ToList();
            var query = from ale in ptAlrgyFoods
                        join mst in aleFoodKbns on ale.AlrgyKbn equals mst.FoodKbn
                        select new PtAlrgyFoodModel
                        (
                              ale.HpId,
                              ale.PtId,
                              ale.SeqNo,
                              ale.SortNo,
                              ale.AlrgyKbn ?? string.Empty,
                              ale.StartDate,
                              ale.EndDate,
                              ale.Cmt ?? string.Empty,
                              ale.IsDeleted,
                              mst.FoodName ?? string.Empty
                        );

            return query.AsEnumerable().Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate)
                                            .OrderBy(p => p.SortNo).ToList();
        }

        public List<PtInfectionModel> GetInfectionList(long ptId)
        {
            List<PtInfectionModel> ptInfections;

            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.InfectionGetList + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                ptInfections = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtInfectionModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                ptInfections = NoTrackingDataContext.PtInfection.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                                                                .AsEnumerable()
                                                                .OrderBy(x => x.SortNo)
                                                                .Select(x => new PtInfectionModel(
                                                                                 x.HpId,
                                                                                 x.PtId,
                                                                                 x.SeqNo,
                                                                                 x.SortNo,
                                                                                 x.ByomeiCd ?? string.Empty,
                                                                                 x.ByotaiCd ?? string.Empty,
                                                                                 x.Byomei ?? string.Empty,
                                                                                 x.StartDate,
                                                                                 x.Cmt ?? string.Empty,
                                                                                 x.IsDeleted
                                                                 )).ToList();
                // Set data to new cache
                var json = JsonSerializer.Serialize(ptInfections);
                _cache.StringSet(finalKey, json);
            }
            return ptInfections;
        }

        public List<PtKioRekiModel> GetKioRekiList(long ptId)
        {
            List<PtKioRekiModel> ptKioRekis;
            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.KioRekiGetList + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                ptKioRekis = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtKioRekiModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                ptKioRekis = NoTrackingDataContext.PtKioRekis.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                                                             .AsEnumerable()
                                                             .OrderBy(p => p.SortNo)
                                                             .Select(x => new PtKioRekiModel(
                                                                              x.HpId,
                                                                              x.PtId,
                                                                              x.SeqNo,
                                                                              x.SortNo,
                                                                              x.ByomeiCd ?? string.Empty,
                                                                              x.ByotaiCd ?? string.Empty,
                                                                              x.Byomei ?? string.Empty,
                                                                              x.StartDate,
                                                                              x.Cmt ?? string.Empty,
                                                                              x.IsDeleted
                                                             )).ToList();
                // Set data to new cache
                var json = JsonSerializer.Serialize(ptKioRekis);
                _cache.StringSet(finalKey, json);
            }
            return ptKioRekis;
        }

        public List<PtOtcDrugModel> GetOtcDrugList(long ptId)
        {
            List<PtOtcDrugModel> ptOtcDrugs;
            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.OtcDrugGetList + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                ptOtcDrugs = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtOtcDrugModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                ptOtcDrugs = NoTrackingDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                                                            .AsEnumerable()
                                                            .Select(x => new PtOtcDrugModel(
                                                                             x.HpId,
                                                                             x.PtId,
                                                                             x.SeqNo,
                                                                             x.SortNo,
                                                                             x.SerialNum,
                                                                             x.TradeName ?? string.Empty,
                                                                             x.StartDate,
                                                                             x.EndDate,
                                                                             x.Cmt ?? string.Empty,
                                                                             x.IsDeleted
                                                            )).ToList();
                // Set data to new cache
                var json = JsonSerializer.Serialize(ptOtcDrugs);
                _cache.StringSet(finalKey, json);
            }
            return ptOtcDrugs;
        }

        public List<PtOtcDrugModel> GetOtcDrugList(long ptId, int sinDate)
        {
            var ptOtcDrugs = NoTrackingDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).AsEnumerable().Select(x => new PtOtcDrugModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.SerialNum,
                x.TradeName ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptOtcDrugs.AsEnumerable().Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate)
                    .OrderBy(p => p.SortNo).ToList();
        }

        public List<PtOtherDrugModel> GetOtherDrugList(long ptId)
        {
            List<PtOtherDrugModel> ptOtherDrugs;

            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.OtherDrugGetList + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                ptOtherDrugs = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtOtherDrugModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                ptOtherDrugs = NoTrackingDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                                                                .AsEnumerable()
                                                                .Select(x => new PtOtherDrugModel(
                                                                                 x.HpId,
                                                                                 x.PtId,
                                                                                 x.SeqNo,
                                                                                 x.SortNo,
                                                                                 x.ItemCd ?? string.Empty,
                                                                                 x.DrugName ?? string.Empty,
                                                                                 x.StartDate,
                                                                                 x.EndDate,
                                                                                 x.Cmt ?? string.Empty,
                                                                                 x.IsDeleted
                                                                )).ToList();
                // Set data to new cache
                var json = JsonSerializer.Serialize(ptOtherDrugs);
                _cache.StringSet(finalKey, json);
            }
            return ptOtherDrugs;
        }

        public List<PtOtherDrugModel> GetOtherDrugList(long ptId, int sinDate)
        {
            var ptOtherDrugs = NoTrackingDataContext.PtOtherDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).AsEnumerable().Select(x => new PtOtherDrugModel(
              x.HpId,
              x.PtId,
              x.SeqNo,
              x.SortNo,
              x.ItemCd ?? string.Empty,
              x.DrugName ?? string.Empty,
              x.StartDate,
              x.EndDate,
              x.Cmt ?? string.Empty,
              x.IsDeleted
            ));
            return ptOtherDrugs.AsEnumerable().Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(p => p.SortNo).ToList();
        }

        public List<PtSuppleModel> GetSuppleList(long ptId)
        {
            List<PtSuppleModel> ptSupples;
            // If exist cache, get data from cache then return data
            var finalKey = key + CacheKeyConstant.SuppleGetList + "_" + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).AsString();
                ptSupples = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<PtSuppleModel>>(cacheString) ?? new() : new();
            }
            else
            {
                // If not, get data from database
                ptSupples = NoTrackingDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0)
                                                           .AsEnumerable()
                                                           .OrderBy(x => x.SortNo)
                                                           .Select(x => new PtSuppleModel(
                                                                            x.HpId,
                                                                            x.PtId,
                                                                            x.SeqNo,
                                                                            x.SortNo,
                                                                            x.IndexCd ?? string.Empty,
                                                                            x.IndexWord ?? string.Empty,
                                                                            x.StartDate,
                                                                            x.EndDate,
                                                                            x.Cmt ?? string.Empty,
                                                                            x.IsDeleted
                                                           )).ToList();
                // Set data to new cache
                var json = JsonSerializer.Serialize(ptSupples);
                _cache.StringSet(finalKey, json);
            }
            return ptSupples;
        }

        public List<PtSuppleModel> GetSuppleList(long ptId, int sinDate)
        {
            var ptSupples = NoTrackingDataContext.PtSupples.Where(x => x.PtId == ptId && x.IsDeleted == 0).AsEnumerable().Select(x => new PtSuppleModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.IndexCd ?? string.Empty,
                x.IndexWord ?? string.Empty,
                x.StartDate,
                x.EndDate,
                x.Cmt ?? string.Empty,
                x.IsDeleted
            ));
            return ptSupples.AsEnumerable().Where(p => p.FullStartDate <= sinDate && sinDate <= p.FullEndDate).OrderBy(x => x.SortNo).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}

