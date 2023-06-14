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
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            RedisConnectorHelper.RedisHost = connection;
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        private IEnumerable<SetKbnMstModel> ReloadCache()
        {
            var setKbnMstList = NoTrackingDataContext.SetKbnMsts.Where(s => s.HpId == 1 && s.IsDeleted == 0).Select(s =>
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
            
            return setKbnMstList;
        }

        private void SetEachFieldForModel(SetKbnMstModel setKbnMstModel)
        {
            NameValueEntry neHpId = new NameValueEntry(nameof(setKbnMstModel.HpId), setKbnMstModel.HpId);
            NameValueEntry neSetKbn = new NameValueEntry(nameof(setKbnMstModel.SetKbn), setKbnMstModel.SetKbn);
            NameValueEntry neSetKbnEdaNo = new NameValueEntry(nameof(setKbnMstModel.SetKbnEdaNo), setKbnMstModel.SetKbnEdaNo);
            NameValueEntry neSetKbnName = new NameValueEntry(nameof(setKbnMstModel.SetKbnName), setKbnMstModel.SetKbnName);
            NameValueEntry neKaCd = new NameValueEntry(nameof(setKbnMstModel.KaCd), setKbnMstModel.KaCd);
            NameValueEntry neDocCd = new NameValueEntry(nameof(setKbnMstModel.DocCd), setKbnMstModel.DocCd);
            NameValueEntry neIsDeleted = new NameValueEntry(nameof(setKbnMstModel.IsDeleted), setKbnMstModel.IsDeleted);
            NameValueEntry neGenerationId = new NameValueEntry(nameof(setKbnMstModel.GenerationId), setKbnMstModel.GenerationId);
            List<NameValueEntry> nameValueEntries = new()
            {
                neHpId,
                neSetKbn,
                neSetKbnEdaNo,
                neSetKbnName,
                neKaCd,
                neDocCd,
                neIsDeleted,
                neGenerationId
            };
            StreamEntry streamEntry = new StreamEntry(key, nameValueEntries.ToArray());

            _cache.StreamAdd(key, streamEntry.Values);
        }

        private IEnumerable<SetKbnMstModel> ReadCache()
        {
            var results = _cache.StreamRange(key);
            List<SetKbnMstModel> datas = new();
            foreach (var result in results)
            {
                var values = result.Values.ToList();
                var hpId = values.FirstOrDefault().Value.AsInteger();
                var setKbn = values.Skip(1).FirstOrDefault().Value.AsInteger();
                var setKbnEdaNo = values.Skip(2).FirstOrDefault().Value.AsInteger();
                var setKbnName = values.Skip(3).FirstOrDefault().Value.AsString();
                var kaCd = values.Skip(4).FirstOrDefault().Value.AsInteger();
                var docCd = values.Skip(5).FirstOrDefault().Value.AsInteger();
                var isDeleted = values.Skip(6).FirstOrDefault().Value.AsInteger();
                var generationId = values.Skip(7).FirstOrDefault().Value.AsInteger();
                var data = new SetKbnMstModel(hpId, setKbn, setKbnEdaNo, setKbnName, kaCd, docCd, isDeleted, generationId);
                datas.Add(data);
            }
            return datas;
        }


        public IEnumerable<SetKbnMstModel> GetList(int hpId, int setKbnFrom, int setKbnTo)
        {
            var setKbnMstList = Enumerable.Empty<SetKbnMstModel>();
            if (!_cache.KeyExists(key))
            {
                setKbnMstList = ReloadCache();
            }
            else
            {
                setKbnMstList = ReadCache();
            }

            return setKbnMstList!.Where(s => s.HpId == hpId && s.SetKbn >= setKbnFrom && s.SetKbn <= setKbnTo && s.IsDeleted == 0).OrderBy(s => s.SetKbn).ToList();
        }

        public bool Upsert(int hpId, int userId, int generationId, List<SetKbnMstModel> setKbnMstModels)
        {
            int maxSetKbn = NoTrackingDataContext.SetKbnMsts.Where(s => s.GenerationId == s.GenerationId && s.HpId == hpId).Select(s => s.SetKbn).ToList().DefaultIfEmpty(0).Max();
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
                _cache.KeyDelete(key);
            }
            return check;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
