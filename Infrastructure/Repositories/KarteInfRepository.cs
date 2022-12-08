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

        public bool SaveListFileKarte(int hpId, long ptId, long raiinNo, List<string> listFileName, bool saveTempFile)
        {
            try
            {
                if (saveTempFile)
                {
                    var listFileInsert = ConvertListInsertTempKarteFile(hpId, ptId, listFileName);
                    if (listFileInsert.Any())
                    {
                        _tenantTrackingDataContext.KarteImgInfs.AddRange(listFileInsert);
                    }
                }
                else
                {
                    UpdateSeqNoKarteFile(hpId, ptId, raiinNo, listFileName);
                }
                return _tenantTrackingDataContext.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
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

        private List<KarteImgInf> ConvertListInsertTempKarteFile(int hpId, long ptId, List<string> listFileNames)
        {
            List<KarteImgInf> result = new();
            int position = 1;

            // insert new entity
            foreach (var name in listFileNames)
            {
                KarteImgInf entity = new();
                entity.HpId = hpId;
                entity.PtId = ptId;
                entity.RaiinNo = 0;
                entity.Position = position;
                entity.SeqNo = 0;
                entity.FileName = name;
                result.Add(entity);
                position += 1;
            }
            return result;
        }

        private void UpdateSeqNoKarteFile(int hpId, long ptId, long raiinNo, List<string> listFileName)
        {
            int position = 1;
            var lastSeqNo = GetLastSeqNo(hpId, ptId, raiinNo);
            var listOldFile = _tenantTrackingDataContext.KarteImgInfs.Where(item =>
                                               item.HpId == hpId
                                               && item.PtId == ptId
                                               && item.RaiinNo == raiinNo
                                               && item.SeqNo == lastSeqNo
                                               && item.FileName != null
                                               && listFileName.Contains(item.FileName)
                                               ).ToList();

            var listUpdateFiles = _tenantTrackingDataContext.KarteImgInfs.Where(item =>
                                               item.HpId == hpId
                                               && item.PtId == ptId
                                               && item.RaiinNo == 0
                                               && item.SeqNo == 0
                                               && item.FileName != null
                                               && listFileName.Contains(item.FileName)
                                               ).ToList();
            foreach (var item in listOldFile)
            {
                KarteImgInf newFile;
                newFile = item;
                newFile.Id = 0;
                newFile.SeqNo = lastSeqNo + 1;
                newFile.Position = position;
                _tenantTrackingDataContext.KarteImgInfs.Add(newFile);
                position++;
            }

            foreach (var item in listUpdateFiles)
            {
                item.RaiinNo = raiinNo;
                item.SeqNo = lastSeqNo + 1;
                item.Position = position;
                position++;
            }
        }

        public List<string> GetListKarteFile(int hpId, long ptId, long raiinNo, bool searchTempFile)
        {
            var lastSeqNo = searchTempFile ? 0 : GetLastSeqNo(hpId, ptId, raiinNo);
            raiinNo = searchTempFile ? 0 : raiinNo;
            var result = _tenantNoTrackingDataContext.KarteImgInfs.Where(item =>
                                                                                item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.RaiinNo == raiinNo
                                                                                && item.SeqNo == lastSeqNo
                                                                                )
                                                                    .OrderBy(item => item.Position)
                                                                    .Select(item =>
                                                                            item.FileName ?? string.Empty
                                                                    ).ToList();
            return result;
        }

        public bool ClearTempData(int hpId, long ptId, List<string> listFileNames)
        {
            var listDeletes = _tenantTrackingDataContext.KarteImgInfs.Where(item => item.HpId == hpId
                                                                && item.PtId == ptId
                                                                && item.SeqNo == 0
                                                                && item.RaiinNo == 0
                                                                && item.FileName != null
                                                                && listFileNames.Contains(item.FileName)
                                                            ).ToList();
            _tenantTrackingDataContext.KarteImgInfs.RemoveRange(listDeletes);
            return _tenantTrackingDataContext.SaveChanges() > 0;
        }
    }
}
