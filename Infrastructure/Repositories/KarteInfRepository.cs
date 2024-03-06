using Domain.Models.KarteInf;
using Domain.Models.KarteInfs;
using Domain.Models.User;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Text;

namespace Infrastructure.Repositories
{
    public class KarteInfRepository : RepositoryBase, IKarteInfRepository
    {
        private readonly IUserRepository _userRepository;
        public KarteInfRepository(ITenantProvider tenantProvider, IUserRepository userRepository) : base(tenantProvider)
        {
            _userRepository = userRepository;
        }

        public List<KarteInfModel> GetList(int hpId, long ptId, long rainNo, int sinDate, bool isDeleted, int userId)
        {
            var karteInfEntity = NoTrackingDataContext.KarteInfs.Where(k => k.HpId == hpId && k.PtId == ptId && k.KarteKbn == 1 && k.RaiinNo == rainNo && k.SinDate == sinDate).ToList();

            if (!karteInfEntity.Any())
            {
                var isEnableMode = _userRepository.CheckLockMedicalExamination(hpId, ptId, rainNo, sinDate, userId);
                var raiinInf = NoTrackingDataContext.RaiinInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.SinDate == sinDate
                                                                                     && p.RaiinNo == rainNo && p.IsDeleted == DeleteTypes.None);

                if (raiinInf == null)
                {
                    return new();
                }
                if (!(isEnableMode && raiinInf.Status < RaiinState.TempSave))
                {
                    return new();
                }
                var karteKbnInsert = NoTrackingDataContext.SystemConfs.FirstOrDefault(item => item.HpId == hpId && item.GrpCd == 2003 && item.GrpEdaNo == 0)?.Val ?? 0;
                if (karteKbnInsert > 0 && ptId > 0)
                {
                    var ptPregnancy = NoTrackingDataContext.PtPregnancies.Where(item => item.HpId == hpId && item.PtId == ptId && item.IsDeleted == 0
                                                                                        && item.StartDate <= sinDate && item.EndDate >= sinDate)
                                                                         .OrderByDescending(item => item.StartDate)
                                                                         .FirstOrDefault();


                    if (ptPregnancy == null)
                    {
                        return new();
                    }

                    int startDate = ptPregnancy.PeriodDate;
                    int endDate = ptPregnancy.EndDate;

                    if (!CIUtil.SDateToDateTime(ptPregnancy.PeriodDate).HasValue && CIUtil.SDateToDateTime(ptPregnancy.PeriodDueDate).HasValue)
                    {
                        startDate = CIUtil.IntToDate(ptPregnancy.PeriodDueDate).AddDays(-280).ToString("yyyyMMdd").AsInteger();
                    }
                    string periodWeek = GetPeriodWeek(sinDate, startDate, 0, endDate);
                    if (ptPregnancy != null)
                    {
                        if (!string.IsNullOrEmpty(periodWeek))
                        {
                            return new() {
                                         new KarteInfModel(hpId, rainNo, ptId, sinDate, periodWeek, true),
                                         };
                        }
                        else
                        {
                            startDate = ptPregnancy.OvulationDate;
                            endDate = ptPregnancy.EndDate;
                            if (!CIUtil.SDateToDateTime(ptPregnancy.PeriodDate).HasValue && CIUtil.SDateToDateTime(ptPregnancy.PeriodDueDate).HasValue)
                            {
                                startDate = CIUtil.IntToDate(ptPregnancy.PeriodDueDate).AddDays(-266).ToString("yyyyMMdd").AsInteger();
                            }
                            string ovulationWeek = GetPeriodWeek(sinDate, startDate, 1, endDate);
                            return new() {
                                         new KarteInfModel(hpId, rainNo, ptId, sinDate, ovulationWeek, true),
                                         };
                        }
                    }
                }
            }
            karteInfEntity = karteInfEntity.Where(item => isDeleted || item.IsDeleted == 0).OrderByDescending(i => i.SeqNo).ToList();
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

        public bool SaveListFileKarte(int hpId, int userId, long ptId, long raiinNo, string host, List<FileInfModel> listFiles, bool saveTempFile)
        {
            if (saveTempFile)
            {
                var listFileInsert = ConvertListInsertTempKarteFile(hpId, userId, ptId, host, listFiles);
                if (listFileInsert.Any())
                {
                    TrackingDataContext.KarteImgInfs.AddRange(listFileInsert);
                }
            }
            else
            {
                UpdateSeqNoKarteFile(hpId, userId, ptId, raiinNo, listFiles.Select(item => new FileInfModel(item.IsSchema, item.LinkFile.Replace(host, string.Empty))).ToList());
            }
            return TrackingDataContext.SaveChanges() > 0;
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

        private List<KarteImgInf> ConvertListInsertTempKarteFile(int hpId, int userId, long ptId, string host, List<FileInfModel> listFileNames)
        {
            List<KarteImgInf> result = new();
            int position = 1;

            // insert new entity
            foreach (var item in listFileNames)
            {
                KarteImgInf entity = new();
                entity.HpId = hpId;
                entity.PtId = ptId;
                entity.RaiinNo = 0;
                entity.Position = position;
                entity.SeqNo = 0;
                entity.KarteKbn = 0;
                if (item.IsSchema)
                {
                    entity.KarteKbn = 1;
                }
                entity.FileName = item.LinkFile.Replace(host, string.Empty);
                entity.CreateDate = CIUtil.GetJapanDateTimeNow();
                entity.CreateId = userId;
                entity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                entity.UpdateId = userId;
                result.Add(entity);
                position += 1;
            }
            return result;
        }

        private void UpdateSeqNoKarteFile(int hpId, int userId, long ptId, long raiinNo, List<FileInfModel> fileInfModelList)
        {
            var dateTimeUpdate = CIUtil.GetJapanDateTimeNow();
            var fileNameList = fileInfModelList.Select(item => item.LinkFile).Distinct().ToList();
            int position = 1;
            var lastSeqNo = GetLastSeqNo(hpId, ptId, raiinNo);

            var listOldFile = TrackingDataContext.KarteImgInfs.Where(item =>
                                               item.HpId == hpId
                                               && item.PtId == ptId
                                               && item.RaiinNo == raiinNo
                                               && item.SeqNo == lastSeqNo
                                               && item.FileName != null
                                               && fileNameList.Contains(item.FileName)
                                               ).OrderBy(item => item.Position)
                                               .ToList();

            var listUpdateFiles = TrackingDataContext.KarteImgInfs.Where(item =>
                                               item.HpId == hpId
                                               && item.PtId == ptId
                                               && item.RaiinNo == 0
                                               && item.SeqNo == 0
                                               && item.FileName != null
                                               && fileNameList.Contains(item.FileName)
                                               ).ToList();


            foreach (var fileInf in fileInfModelList)
            {
                var oldItemConvert = listOldFile.FirstOrDefault(item => item.SeqNo == lastSeqNo
                                                                        && item.RaiinNo == raiinNo
                                                                        && item.FileName != null
                                                                        && item.FileName == fileInf.LinkFile);

                if (oldItemConvert != null)
                {
                    KarteImgInf convertItem;
                    convertItem = oldItemConvert;
                    convertItem.Id = 0;
                    convertItem.SeqNo = lastSeqNo + 1;
                    convertItem.Position = position;
                    convertItem.KarteKbn = oldItemConvert.KarteKbn;
                    convertItem.CreateDate = dateTimeUpdate;
                    convertItem.UpdateDate = dateTimeUpdate;
                    convertItem.UpdateId = userId;
                    TrackingDataContext.KarteImgInfs.Add(convertItem);
                    position++;
                    continue;
                }

                var oldItemUpdateSeqNo = listUpdateFiles.FirstOrDefault(item => item.RaiinNo == 0
                                                                                && item.SeqNo == 0
                                                                                && item.FileName != null
                                                                                && item.FileName == fileInf.LinkFile);
                if (oldItemUpdateSeqNo != null)
                {
                    oldItemUpdateSeqNo.RaiinNo = raiinNo;
                    oldItemUpdateSeqNo.SeqNo = lastSeqNo + 1;
                    oldItemUpdateSeqNo.Position = position;
                    oldItemUpdateSeqNo.UpdateDate = dateTimeUpdate;
                    oldItemUpdateSeqNo.CreateDate = dateTimeUpdate;
                    position++;
                    continue;
                }

                KarteImgInf newItem = new();
                newItem.Id = 0;
                newItem.HpId = hpId;
                newItem.PtId = ptId;
                newItem.RaiinNo = raiinNo;
                newItem.FileName = fileInf.LinkFile;
                newItem.SeqNo = lastSeqNo + 1;
                newItem.Position = position;
                newItem.KarteKbn = fileInf.IsSchema ? 1 : 0;
                newItem.CreateDate = dateTimeUpdate;
                newItem.CreateId = userId;
                newItem.UpdateDate = dateTimeUpdate;
                newItem.UpdateId = userId;
                TrackingDataContext.KarteImgInfs.Add(newItem);
                position++;
            }

            if (!fileInfModelList.Any())
            {
                KarteImgInf newFile = new();
                newFile.FileName = string.Empty;
                newFile.Id = 0;
                newFile.HpId = hpId;
                newFile.RaiinNo = raiinNo;
                newFile.PtId = ptId;
                newFile.SeqNo = lastSeqNo + 1;
                newFile.Position = 1;
                newFile.KarteKbn = 0;
                TrackingDataContext.KarteImgInfs.Add(newFile);
            }

            TrackingDataContext.SaveChanges();

            var listUpdateDateFile = TrackingDataContext.KarteImgInfs.Where(item => item.HpId == hpId
                                                                                    && item.PtId == ptId
                                                                                    && item.RaiinNo == raiinNo
                                                                                    && item.SeqNo == lastSeqNo
                                                                                    && item.FileName != null)
                                                                     .OrderBy(item => item.Position)
                                                                     .ToList();

            foreach (var item in listUpdateDateFile)
            {
                item.UpdateDate = dateTimeUpdate;
                item.UpdateId = userId;
            }
        }

        public List<FileInfModel> GetListKarteFile(int hpId, long ptId, long raiinNo, bool searchTempFile)
        {
            var lastSeqNo = searchTempFile ? 0 : GetLastSeqNo(hpId, ptId, raiinNo);
            raiinNo = searchTempFile ? 0 : raiinNo;
            var result = NoTrackingDataContext.KarteImgInfs.Where(item =>
                                                                        item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.RaiinNo == raiinNo
                                                                        && item.SeqNo == lastSeqNo
                                                                        && item.FileName != string.Empty
                                                                        )
                                                            .OrderBy(item => item.Position)
                                                            .Select(item =>
                                                                    new FileInfModel(
                                                                            item.RaiinNo,
                                                                            item.SeqNo,
                                                                            item.KarteKbn > 0,
                                                                            item.FileName ?? string.Empty
                                                                        )
                                                            ).ToList();
            return result;
        }

        public List<FileInfModel> GetListKarteFile(int hpId, long ptId, List<long> listRaiinNo, bool isGetAll)
        {
            var listFileKarte = NoTrackingDataContext.KarteImgInfs.Where(item => item.HpId == hpId
                                                                                 && item.PtId == ptId
                                                                                 && listRaiinNo.Contains(item.RaiinNo))
                                                                    .OrderBy(item => item.Position)
                                                                    .ToList();

            var userIdList = listFileKarte.Select(item => item.CreateId).ToList();
            userIdList.AddRange(listFileKarte.Select(item => item.UpdateId));
            userIdList = userIdList.Distinct().ToList();
            var userMstList = NoTrackingDataContext.UserMsts.Where(item => item.HpId == hpId
                                                                           && userIdList.Contains(item.UserId))
                                                            .ToList();

            if (listFileKarte.Any())
            {
                List<FileInfModel> result = new();
                foreach (var raiinNo in listRaiinNo)
                {
                    var karteFileListByRaiinNo = listFileKarte.Where(item => item.RaiinNo == raiinNo).ToList();
                    foreach (var karte in karteFileListByRaiinNo)
                    {
                        var lastSeqNo = karteFileListByRaiinNo.Max(item => item.SeqNo);
                        if (!isGetAll)
                        {
                            if (karte.SeqNo == lastSeqNo)
                            {
                                result.Add(new FileInfModel(
                                               karte.RaiinNo,
                                               karte.SeqNo,
                                               karte.KarteKbn > 0,
                                               karte.FileName ?? string.Empty,
                                               karte.SeqNo != lastSeqNo,
                                               karte.CreateDate,
                                               karte.UpdateDate,
                                               userMstList.FirstOrDefault(item => item.UserId == karte.CreateId)?.Sname ?? string.Empty,
                                               userMstList.FirstOrDefault(item => item.UserId == karte.UpdateId)?.Sname ?? string.Empty
                                        ));
                            }
                        }
                        else
                        {
                            result.Add(new FileInfModel(
                                           karte.RaiinNo,
                                           karte.SeqNo,
                                           karte.KarteKbn > 0,
                                           karte.FileName ?? string.Empty,
                                           karte.SeqNo != lastSeqNo,
                                           karte.CreateDate,
                                           karte.UpdateDate,
                                           userMstList.FirstOrDefault(item => item.UserId == karte.CreateId)?.Sname ?? string.Empty,
                                           userMstList.FirstOrDefault(item => item.UserId == karte.UpdateId)?.Sname ?? string.Empty
                                        ));
                        }
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

        public Dictionary<string, bool> ListCheckIsSchema(int hpId, long ptId, List<FileMapCopyItem> fileInfUpdateTemp)
        {
            Dictionary<string, bool> result = new();
            var fileNameKeyList = fileInfUpdateTemp.Select(item => item.OldFileName).Distinct().ToList();
            var nextOrderFileList = NoTrackingDataContext.RsvkrtKarteImgInfs.Where(item => item.HpId == hpId
                                                                                           && item.PtId == ptId
                                                                                           && !string.IsNullOrEmpty(item.FileName)
                                                                                           && fileNameKeyList.Contains(item.FileName))
                                                                             .ToList();
            var setFileList = NoTrackingDataContext.SetKarteImgInf.Where(item => item.HpId == hpId
                                                                                 && !string.IsNullOrEmpty(item.FileName)
                                                                                 && fileNameKeyList.Contains(item.FileName))
                                                                   .ToList();
            foreach (var fileInf in fileInfUpdateTemp)
            {
                bool isSchema = false;
                var nextOrderItem = nextOrderFileList.FirstOrDefault(item => item.FileName == fileInf.OldFileName);
                if (nextOrderItem != null)
                {
                    isSchema = nextOrderItem.KarteKbn == 1;
                }
                else
                {
                    var setItem = setFileList.FirstOrDefault(item => item.FileName == fileInf.OldFileName);
                    if (setItem != null)
                    {
                        isSchema = setItem.KarteKbn == 1;
                    }
                }

                result.Add(fileInf.NewFileName, isSchema);
            }
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public long ConvertTextToRichText(int hpId, long ptId)
        {
            var listKarteItems = TrackingDataContext.KarteInfs.Where(item =>
                                                                            item.HpId == hpId
                                                                            && item.PtId == ptId
                                                                            && item.Text != null)
                                                              .ToList();
            if (listKarteItems.Any())
            {
                foreach (var karteItem in listKarteItems)
                {
                    if (karteItem.Text != null)
                    {
                        karteItem.RichText = Encoding.UTF8.GetBytes(karteItem.Text);
                    }
                }
                TrackingDataContext.SaveChanges();
                return ptId;
            }
            return 0;
        }

        private string GetPeriodWeek(int sinDay, int startDay, int ovulation, int endDay = 0)
        {
            if (startDay == 0) return string.Empty;
            if (startDay >= sinDay)
            {
                return "0W0D";
            }
            if (endDay != 0 && endDay < startDay)
            {
                return "0W0D";
            }
            DateTime dtStartDay = CIUtil.IntToDate(startDay);
            dtStartDay = dtStartDay.AddDays(-14 * ovulation);

            DateTime dtToDay;
            if (sinDay > endDay && endDay > 0)
            {
                dtToDay = CIUtil.IntToDate(endDay);
            }
            else
            {
                dtToDay = CIUtil.IntToDate(sinDay);
            }

            int countDays = dtToDay.Subtract(dtStartDay).Days;
            if (countDays < 0)
            {
                countDays *= -1;
            }
            return (countDays / 7) + "W" + (countDays % 7) + "D";
        }

    }
}
