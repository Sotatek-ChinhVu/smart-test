using Domain.Models.KarteInfs;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories
{
    public class KarteInfRepository : IKarteInfRepository
    {
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private readonly TenantDataContext _tenantTrackingDataContext;

        public KarteInfRepository(ITenantProvider tenantProvider)
        {
            _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantTrackingDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted)
        {
            var karteInfEntity = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.PtId == ptId && k.RaiinNo == rainNo && k.SinDate == sinDate && (isDeleted || k.IsDeleted == 0)).ToList();

            if (karteInfEntity == null)
            {
                return new List<KarteInfModel>();
            }
            return karteInfEntity.Select(k => ConvertToModel(k)).ToList();
        }

        public List<KarteInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long> raiinNos)
        {
            var karteInfEntities = _tenantNoTrackingDataContext.KarteInfs.Where(k => k.PtId == ptId && k.HpId == hpId && raiinNos.Contains(k.RaiinNo)).AsEnumerable();

            if (deleteCondition == 0)
            {
                karteInfEntities = karteInfEntities.Where(r => r.IsDeleted == DeleteTypes.None);
            }
            else if (deleteCondition == 1)
            {
                karteInfEntities = karteInfEntities.Where(r => r.IsDeleted == DeleteTypes.None || r.IsDeleted == DeleteTypes.Deleted);
            }
            else
            {
                karteInfEntities = karteInfEntities.Where(r => r.IsDeleted == DeleteTypes.None || r.IsDeleted == DeleteTypes.Deleted || r.IsDeleted == DeleteTypes.Confirm);
            }

            if (karteInfEntities == null)
            {
                return new List<KarteInfModel>();
            }

            var karteInfs = from karte in karteInfEntities
                            join user in _tenantNoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId)
                          on karte.CreateId equals user.UserId into odrUsers
                            from odrUser in odrUsers.DefaultIfEmpty()
                            select ConvertToModel(karte, odrUser?.Sname ?? string.Empty);

            return karteInfs.ToList();
        }

        private static KarteInfModel ConvertToModel(KarteInf itemData, string updateName = "")
        {
            return new KarteInfModel(
                itemData.HpId,
                itemData.RaiinNo,
                itemData.KarteKbn,
                itemData.SeqNo,
                itemData.PtId,
                itemData.SinDate,
                itemData.Text ?? string.Empty,
                itemData.IsDeleted,
                itemData.RichText == null ? string.Empty : Encoding.UTF8.GetString(itemData.RichText),
                itemData.CreateDate,
                itemData.UpdateDate,
                updateName
                );
        }

        public long SaveListFileKarte(int hpId, long ptId, long raiinNo, long lastSeqNo, List<KarteImgInfModel> listModel, List<long> listFileDeletes)
        {
            try
            {
                var listOldFiles = _tenantTrackingDataContext.KarteImgInfs.Where(item =>
                                                        item.HpId == hpId
                                                        && item.PtId == ptId
                                                        && item.RaiinNo == raiinNo
                                                        && item.SeqNo == lastSeqNo).ToList();

                var listUpdateSeqNo = listOldFiles.Where(item => !listFileDeletes.Contains(item.Id)).ToList();
                var listFileInsert = ConvertListKarteImgInf(lastSeqNo, listUpdateSeqNo, listModel);
                if (listFileInsert.Any())
                {
                    _tenantTrackingDataContext.KarteImgInfs.AddRange(listFileInsert);
                }
                _tenantTrackingDataContext.SaveChanges();
                if (listFileInsert.Any())
                {
                    return listFileInsert.First().SeqNo;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int GetSinDate(long ptId, int hpId, int searchType, int sinDate, List<long> listRaiiNoSameSinDate, string searchText)
        {
            if (searchType == 1)
                return _tenantNoTrackingDataContext.KarteInfs.OrderBy(k => k.SinDate).LastOrDefault(k => k.HpId == hpId && k.PtId == ptId && (k.Text != null && k.Text.Contains(searchText)) && k.SinDate <= sinDate && !listRaiiNoSameSinDate.Contains(k.SinDate) && k.KarteKbn == 1)?.SinDate ?? -1;
            else
                return _tenantNoTrackingDataContext.KarteInfs.OrderBy(k => k.SinDate).FirstOrDefault(k => k.HpId == hpId && k.PtId == ptId && (k.Text != null && k.Text.Contains(searchText)) && k.SinDate >= sinDate && !listRaiiNoSameSinDate.Contains(k.SinDate) && k.KarteKbn == 1)?.SinDate ?? -1;
        }

        public bool CheckExistListFile(int hpId, long ptId, long seqNo, long rainNo, List<long> listFileDeletes)
        {
            var fileExists = _tenantNoTrackingDataContext.KarteImgInfs.Count(item =>
                                                                                item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.SeqNo == seqNo
                                                                                && item.RaiinNo == rainNo
                                                                                && listFileDeletes.Distinct().Contains(item.Id));
            return fileExists == listFileDeletes?.Count();
        }

        public long GetLastSeqNo(int hpId, long ptId, long rainNo)
        {
            var lastItem = _tenantNoTrackingDataContext.KarteImgInfs.Where(item => item.HpId == hpId && item.PtId == ptId && item.RaiinNo == rainNo).ToList()?.MaxBy(item => item.SeqNo);
            return lastItem != null ? lastItem.SeqNo : 0;
        }

        private List<KarteImgInf> ConvertListKarteImgInf(long lastSeqNo, List<KarteImgInf> listKarteImgInfUpdateSeqNo, List<KarteImgInfModel> listModel)
        {
            List<KarteImgInf> result = new();
            int position = 1;

            // update seqNo
            foreach (var updateItem in listKarteImgInfUpdateSeqNo)
            {
                KarteImgInf entity = new();
                entity.HpId = updateItem.HpId;
                entity.PtId = updateItem.PtId;
                entity.RaiinNo = updateItem.RaiinNo;
                entity.Position = position;
                entity.SeqNo = lastSeqNo + 1;
                entity.FileName = updateItem.FileName;
                result.Add(entity);
                position += 1;
            }

            // insert new entity
            foreach (var model in listModel)
            {
                KarteImgInf entity = new();
                entity.HpId = model.HpId;
                entity.PtId = model.PtId;
                entity.RaiinNo = model.RaiinNo;
                entity.Position = position;
                entity.SeqNo = lastSeqNo + 1;
                entity.FileName = model.FileName;
                result.Add(entity);
                position += 1;
            }
            return result;
        }

        public List<KarteImgInfModel> GetListKarteFile(int hpId, long ptId, long rainNo)
        {
            var lastSeqNo = GetLastSeqNo(hpId, ptId, rainNo);
            var result = _tenantNoTrackingDataContext.KarteImgInfs.Where(item =>
                                                                                item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.RaiinNo == rainNo
                                                                                && item.SeqNo == lastSeqNo
                                                                                )
                                                                    .OrderBy(item => item.Position)
                                                                    .Select(item => new KarteImgInfModel(
                                                                            item.Id,
                                                                            item.HpId,
                                                                            item.PtId,
                                                                            item.RaiinNo,
                                                                            item.FileName ?? string.Empty
                                                                    )).ToList();
            return result;
        }
    }
}
