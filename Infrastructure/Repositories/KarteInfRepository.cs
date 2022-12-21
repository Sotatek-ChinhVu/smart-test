using Domain.Models.KarteInf;
using Domain.Models.KarteInfs;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Text;

namespace Infrastructure.Repositories
{
    public class KarteInfRepository : RepositoryBase, IKarteInfRepository
    {
        public KarteInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<KarteInfModel> GetList(long ptId, long rainNo, long sinDate, bool isDeleted)
        {
            var karteInfEntity = NoTrackingDataContext.KarteInfs.Where(k => k.PtId == ptId && k.RaiinNo == rainNo && k.SinDate == sinDate && (isDeleted || k.IsDeleted == 0)).ToList();

            if (karteInfEntity == null)
            {
                return new List<KarteInfModel>();
            }
            return karteInfEntity.Select(k => ConvertToModel(k)).ToList();
        }

        public List<KarteInfModel> GetList(long ptId, int hpId, int deleteCondition, List<long> raiinNos)
        {
            var karteInfEntities = NoTrackingDataContext.KarteInfs.Where(k => k.PtId == ptId && k.HpId == hpId && raiinNos.Contains(k.RaiinNo)).AsEnumerable();

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
                            join user in NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId)
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
                        TrackingDataContext.KarteImgInfs.AddRange(listFileInsert);
                    }
                }
                else
                {
                    UpdateSeqNoKarteFile(hpId, ptId, raiinNo, listFileName);
                }
                return TrackingDataContext.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetSinDate(long ptId, int hpId, int searchType, int sinDate, List<long> listRaiiNoSameSinDate, string searchText)
        {
            if (searchType == 1)
                return NoTrackingDataContext.KarteInfs.OrderBy(k => k.SinDate).LastOrDefault(k => k.HpId == hpId && k.PtId == ptId && (k.Text != null && k.Text.Contains(searchText)) && k.SinDate <= sinDate && !listRaiiNoSameSinDate.Contains(k.SinDate) && k.KarteKbn == 1)?.SinDate ?? -1;
            else
                return NoTrackingDataContext.KarteInfs.OrderBy(k => k.SinDate).FirstOrDefault(k => k.HpId == hpId && k.PtId == ptId && (k.Text != null && k.Text.Contains(searchText)) && k.SinDate >= sinDate && !listRaiiNoSameSinDate.Contains(k.SinDate) && k.KarteKbn == 1)?.SinDate ?? -1;
        }

        public long GetLastSeqNo(int hpId, long ptId, long rainNo)
        {
            var lastItem = NoTrackingDataContext.KarteImgInfs.Where(item => item.HpId == hpId && item.PtId == ptId && item.RaiinNo == rainNo).ToList()?.MaxBy(item => item.SeqNo);
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
            var listOldFile = TrackingDataContext.KarteImgInfs.Where(item =>
                                               item.HpId == hpId
                                               && item.PtId == ptId
                                               && item.RaiinNo == raiinNo
                                               && item.SeqNo == lastSeqNo
                                               && item.FileName != null
                                               && listFileName.Contains(item.FileName)
                                               ).OrderBy(item => item.Position)
                                               .ToList();

            var listUpdateFiles = TrackingDataContext.KarteImgInfs.Where(item =>
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
                TrackingDataContext.KarteImgInfs.Add(newFile);
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
            var result = NoTrackingDataContext.KarteImgInfs.Where(item =>
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

        public List<FileInfModel> GetListKarteFile(int hpId, long ptId, List<long> listRaiinNo, bool isGetAll)
        {

            var listFileKarte = NoTrackingDataContext.KarteImgInfs.Where(item =>
                                                                                item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && listRaiinNo.Contains(item.RaiinNo)
                                                                                )
                                                                    .OrderBy(item => item.Position)
                                                                    .ToList();
            if (listFileKarte.Any())
            {
                List<FileInfModel> result = new();
                foreach (var karte in listFileKarte)
                {
                    var lastSeqNo = listFileKarte.Max(item => item.SeqNo);
                    if (!isGetAll)
                    {
                        if (karte.SeqNo == lastSeqNo)
                        {
                            result.Add(new FileInfModel(
                                        karte.RaiinNo,
                                        karte.SeqNo,
                                        karte.FileName ?? string.Empty,
                                        karte.SeqNo != lastSeqNo
                                    ));
                        }
                    }
                    else
                    {
                        result.Add(new FileInfModel(
                                        karte.RaiinNo,
                                        karte.SeqNo,
                                        karte.FileName ?? string.Empty,
                                        karte.SeqNo != lastSeqNo
                                    ));
                    }
                }
                return result;
            }
            return new();
        }

        public bool ClearTempData(int hpId, long ptId, List<string> listFileNames)
        {
            var listDeletes = NoTrackingDataContext.KarteImgInfs.Where(item => item.HpId == hpId
                                                                && item.PtId == ptId
                                                                && item.SeqNo == 0
                                                                && item.RaiinNo == 0
                                                                && item.FileName != null
                                                                && listFileNames.Contains(item.FileName)
                                                            ).ToList();
            TrackingDataContext.KarteImgInfs.RemoveRange(listDeletes);
            return TrackingDataContext.SaveChanges() > 0;
        }
    }
}
