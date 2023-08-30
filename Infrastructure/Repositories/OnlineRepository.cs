﻿using Domain.Models.Insurance;
using Domain.Models.Online;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace Infrastructure.Repositories;

public class OnlineRepository : RepositoryBase, IOnlineRepository
{
    public OnlineRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public bool InsertOnlineConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> onlineList)
    {
        var onlineInsertList = onlineList.Select(item => new OnlineConfirmationHistory()
        {
            ID = 0,
            PtId = item.PtId,
            OnlineConfirmationDate = item.OnlineConfirmationDate,
            ConfirmationType = item.ConfirmationType,
            ConfirmationResult = item.ConfirmationResult,
            UketukeStatus = item.UketukeStatus,
            CreateDate = CIUtil.GetJapanDateTimeNow(),
            UpdateDate = CIUtil.GetJapanDateTimeNow(),
            CreateId = userId,
            UpdateId = userId,
        }).ToList();
        TrackingDataContext.AddRange(onlineInsertList);
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<OnlineConfirmationHistoryModel> GetRegisterdPatientsFromOnline(int confirmDate, int id = 0, int confirmType = 1)
    {
        List<OnlineConfirmationHistory> onlineList;
        if (id == 0)
        {
            onlineList = NoTrackingDataContext.OnlineConfirmationHistories.Where(item => item.ConfirmationType == confirmType)
                                                                          .ToList();
            onlineList = onlineList.Where(item => CIUtil.DateTimeToInt(item.OnlineConfirmationDate) == confirmDate).ToList();
        }
        else
        {
            onlineList = NoTrackingDataContext.OnlineConfirmationHistories.Where(item => item.ID == id).ToList();
        }

        var result = onlineList.OrderBy(item => item.OnlineConfirmationDate)
                               .Select(item => ConvertToModel(item))
                               .ToList();
        return result;
    }

    public bool UpdateOnlineConfirmationHistory(int uketukeStatus, int id, int userId)
    {
        string updateDate = CIUtil.GetJapanDateTimeNow().ToString("yyyy-MM-dd HH:mm:ss.fff");

        string updateQuery = $"UPDATE \"ONLINE_CONFIRMATION_HISTORY\" SET \"UKETUKE_STATUS\" = {uketukeStatus}, \"UPDATE_DATE\" = '{updateDate}'"
                             + $", \"UPDATE_ID\" = {userId}"
                             + $" WHERE \"ID\" = {id} AND \"UKETUKE_STATUS\" = 0";

        return TrackingDataContext.Database.ExecuteSqlRaw(updateQuery) > 0;
    }

    public long UpdateRefNo(int hpId, long ptId)
    {
        var nextRefNo = TrackingDataContext.Database.SqlQueryRaw<long>("SELECT NEXTVAL(' \"PT_INF_REFERENCE_NO_seq\"')").ToList().FirstOrDefault();
        string updateQuery = $"UPDATE \"PT_INF\" SET \"REFERENCE_NO\" = {nextRefNo} WHERE \"HP_ID\" = {hpId} AND \"PT_ID\" = {ptId}";
        TrackingDataContext.Database.ExecuteSqlRaw(updateQuery);
        return nextRefNo;
    }

    public bool UpdateOnlineHistoryById(int userId, long id, long ptId, int uketukeStatus, int confirmationType)
    {
        var onlineHistory = TrackingDataContext.OnlineConfirmationHistories.FirstOrDefault(item => item.ID == id);
        if (onlineHistory == null)
        {
            return false;
        }
        onlineHistory.UpdateDate = CIUtil.GetJapanDateTimeNow();
        onlineHistory.UpdateId = userId;
        if (uketukeStatus != -1)
        {
            onlineHistory.UketukeStatus = uketukeStatus;
        }
        if (ptId != -1)
        {
            onlineHistory.PtId = ptId;
        }
        if (confirmationType != -1)
        {
            onlineHistory.ConfirmationType = confirmationType;
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public bool CheckExistIdList(List<long> idList)
    {
        idList = idList.Distinct().ToList();
        var countId = NoTrackingDataContext.OnlineConfirmationHistories.Count(x => idList.Contains(x.ID));
        return idList.Count == countId;
    }

    public bool UpdateOQConfirmation(int hpId, int userId, long onlineHistoryId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict)
    {
        if (!onlQuaResFileDict.Any())
        {
            return false;
        }
        var history = TrackingDataContext.OnlineConfirmationHistories.FirstOrDefault(x => x.ID == onlineHistoryId);
        if (history == null)
        {
            return false;
        }
        bool success = false;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    var item = onlQuaResFileDict.First();
                    history.ConfirmationType = onlQuaConfirmationTypeDict.ContainsKey(item.Key) ? onlQuaConfirmationTypeDict[item.Key].confirmationType : 1;
                    history.InfoConsFlg = onlQuaConfirmationTypeDict.ContainsKey(item.Key) ? onlQuaConfirmationTypeDict[item.Key].infConsFlg : "    ";
                    history.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    history.UpdateId = userId;
                    TrackingDataContext.SaveChanges();

                    int sindate = CIUtil.DateTimeToInt(history.OnlineConfirmationDate);
                    var raiinInfsInSameday = TrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId && x.SinDate == sindate && x.PtId == history.PtId).ToList();

                    UpdateConfirmationTypeInRaiinInf(userId, raiinInfsInSameday, history.ConfirmationType);
                    if (!string.IsNullOrEmpty(history.InfoConsFlg))
                    {
                        UpdateInfConsFlgInRaiinInf(userId, raiinInfsInSameday, history.InfoConsFlg);
                    }
                    transaction.Commit();
                    success = true;
                }
                catch
                {
                    transaction.Rollback();
                }
            });
        return success;
    }

    public bool SaveAllOQConfirmation(int hpId, int userId, long ptId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict)
    {
        bool success = false;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    List<OnlineConfirmationHistory> historyList = new();
                    foreach (var item in onlQuaResFileDict)
                    {
                        historyList.Add(new OnlineConfirmationHistory()
                        {
                            PtId = ptId,
                            OnlineConfirmationDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(item.Key, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)),
                            ConfirmationType = onlQuaConfirmationTypeDict.ContainsKey(item.Key) ? onlQuaConfirmationTypeDict[item.Key].confirmationType : 1,
                            InfoConsFlg = onlQuaConfirmationTypeDict.ContainsKey(item.Key) ? onlQuaConfirmationTypeDict[item.Key].infConsFlg : "    ",
                            ConfirmationResult = item.Value,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId,
                        });
                    }
                    TrackingDataContext.OnlineConfirmationHistories.AddRange(historyList);
                    TrackingDataContext.SaveChanges();

                    var ptIdList = historyList.Select(item => item.PtId).Distinct().ToList();
                    var sinDateList = historyList.Select(item => CIUtil.DateTimeToInt(item.OnlineConfirmationDate)).Distinct().ToList();
                    var raiinInfList = TrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId && sinDateList.Contains(item.SinDate) && ptIdList.Contains(item.PtId)).ToList();
                    foreach (var historyItem in historyList)
                    {
                        int sindate = CIUtil.DateTimeToInt(historyItem.OnlineConfirmationDate);
                        var raiinInfsInSameday = raiinInfList.Where(item => item.SinDate == sindate && item.PtId == historyItem.PtId).ToList();
                        UpdateConfirmationTypeInRaiinInf(userId, raiinInfsInSameday, historyItem.ConfirmationType);
                        if (!string.IsNullOrEmpty(historyItem.InfoConsFlg))
                        {
                            UpdateInfConsFlgInRaiinInf(userId, raiinInfsInSameday, historyItem.InfoConsFlg);
                        }
                    }
                    transaction.Commit();
                    success = true;
                }
                catch
                {
                    transaction.Rollback();
                }
            });
        return success;
    }

    public bool SaveOQConfirmation(int hpId, int userId, long onlineHistoryId, long ptId, string confirmationResult, string onlineConfirmationDateString, int confirmationType, string infConsFlg, int uketukeStatus = 0, bool isUpdateRaiinInf = true)
    {
        bool success = false;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    var onlineConfirmationDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(onlineConfirmationDateString, "yyyyMMddHHmmss", CultureInfo.InvariantCulture));
                    if (onlineHistoryId == 0)
                    {
                        TrackingDataContext.OnlineConfirmationHistories.Add(new OnlineConfirmationHistory()
                        {
                            PtId = ptId,
                            OnlineConfirmationDate = onlineConfirmationDate,
                            ConfirmationType = confirmationType,
                            ConfirmationResult = confirmationResult,
                            InfoConsFlg = infConsFlg,
                            UketukeStatus = uketukeStatus,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId,
                        });
                    }
                    else
                    {
                        var onlineHistory = TrackingDataContext.OnlineConfirmationHistories.FirstOrDefault(p => p.ID == onlineHistoryId);
                        if (onlineHistory != null)
                        {
                            if (uketukeStatus > 0)
                            {
                                onlineHistory.UketukeStatus = uketukeStatus;
                            }
                            onlineHistory.PtId = ptId;
                            onlineHistory.ConfirmationType = confirmationType;
                            onlineHistory.ConfirmationResult = confirmationResult;
                            onlineHistory.InfoConsFlg = infConsFlg;
                            onlineHistory.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            onlineHistory.UpdateId = userId;
                        }
                    }
                    TrackingDataContext.SaveChanges();
                    if (isUpdateRaiinInf)
                    {
                        UpdateOnlineInRaiinInf(hpId, userId, ptId, onlineConfirmationDate, confirmationType, infConsFlg);
                    }
                    transaction.Commit();
                    success = true;
                }
                catch
                {
                    transaction.Rollback();
                }
            });
        return success;
    }

    public bool UpdateOnlineInRaiinInf(int hpId, int userId, long ptId, DateTime onlineConfirmationDate, int confirmationType, string infConsFlg)
    {
        bool success = false;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    int sindate = CIUtil.DateTimeToInt(onlineConfirmationDate);
                    var raiinInfsInSameday = TrackingDataContext.RaiinInfs.Where(x => x.HpId == hpId && x.SinDate == sindate && x.PtId == ptId).ToList();
                    UpdateConfirmationTypeInRaiinInf(userId, raiinInfsInSameday, confirmationType);
                    if (!string.IsNullOrEmpty(infConsFlg))
                    {
                        UpdateInfConsFlgInRaiinInf(userId, raiinInfsInSameday, infConsFlg);
                    }
                    transaction.Commit();
                    success = true;
                }
                catch
                {
                    transaction.Rollback();
                }
            });
        return success;
    }

    public bool UpdatePtInfOnlineQualify(int hpId, int userId, long ptId, List<PtInfConfirmationModel> resultList)
    {
        if (resultList == null || resultList.Count == 0)
        {
            return false;
        }
        if (!resultList.Any(item => !item.IsReflect))
        {
            return false;
        }

        var ptInf = TrackingDataContext.PtInfs.FirstOrDefault(item => item.HpId == hpId && item.PtId == ptId);
        if (ptInf == null)
        {
            return false;
        }

        foreach (var model in resultList)
        {
            if (model.IsReflect)
            {
                continue;
            }
            switch (model.AttributeName)
            {
                case PtInfOQConst.KANJI_NAME:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        ptInf.Name = model.XmlValue;
                    }
                    break;
                case PtInfOQConst.KANA_NAME:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        ptInf.KanaName = model.XmlValue;
                    }
                    break;
                case PtInfOQConst.SEX:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        ptInf.Sex = model.XmlValue.AsInteger();
                    }
                    break;
                case PtInfOQConst.BIRTHDAY:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        ptInf.Birthday = model.XmlValue.AsInteger();
                    }
                    break;
                case PtInfOQConst.HOME_ADDRESS:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        ptInf.HomeAddress1 = model.XmlValue;
                    }
                    break;
                case PtInfOQConst.HOME_POST:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        string value = model.XmlValue;
                        if (!string.IsNullOrEmpty(value) && value.Contains("-"))
                        {
                            value = value.Replace("-", string.Empty);
                        }
                        ptInf.HomePost = value;
                    }
                    break;
                case PtInfOQConst.SETANUSI:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        ptInf.Setanusi = model.XmlValue;
                    }
                    break;
            }
        }
        var isUpdated = resultList.Any(item => !item.IsReflect && !string.IsNullOrEmpty(item.XmlValue));
        if (isUpdated)
        {
            ptInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            ptInf.UpdateId = userId;
            return TrackingDataContext.SaveChanges() > 0;
        }
        return true;
    }

    public bool UpdateConfirmationTypeInRaiinInf(int userId, List<RaiinInf> raiinInfsInSameday, int confirmationType)
    {
        var unConfirmedRaiinInfs = raiinInfsInSameday.Where(x => x.ConfirmationType == 0);
        var confirmedRaiininfs = raiinInfsInSameday.Where(x => x.ConfirmationType > 0);
        if (!confirmedRaiininfs.Any())
        {
            foreach (var raiinInf in unConfirmedRaiinInfs)
            {
                raiinInf.ConfirmationType = confirmationType;
                raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                raiinInf.UpdateId = userId;
            }
        }
        else
        {
            int minConfirmationType = confirmedRaiininfs.Min(x => x.ConfirmationType);
            int newConfirmationType = minConfirmationType > confirmationType ? confirmationType : minConfirmationType;
            foreach (var raiinInf in confirmedRaiininfs)
            {
                raiinInf.ConfirmationType = newConfirmationType;
                raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                raiinInf.UpdateId = userId;
            }
            foreach (var raiinInf in unConfirmedRaiinInfs)
            {
                raiinInf.ConfirmationType = newConfirmationType;
                raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                raiinInf.UpdateId = userId;
            }
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    public List<OnlineConfirmationHistoryModel> GetListOnlineConfirmationHistoryModel(long ptId)
    {
        var listOnlineConfirmationHistory = NoTrackingDataContext.OnlineConfirmationHistories.Where(item => item.PtId == ptId).ToList();
        var result = listOnlineConfirmationHistory.Select(item => ConvertToModel(item))
                                                  .OrderByDescending(item => item.OnlineConfirmationDate)
                                                  .ToList();
        return result;
    }

    public List<OnlineConfirmationHistoryModel> GetListOnlineConfirmationHistoryModel(Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict)
    {
        var listOnlineConfirmationHistory = new List<OnlineConfirmationHistory>();
        foreach (var item in onlQuaResFileDict)
        {
            listOnlineConfirmationHistory.Add(new OnlineConfirmationHistory()
            {
                PtId = 0,
                OnlineConfirmationDate = TimeZoneInfo.ConvertTimeToUtc(DateTime.ParseExact(item.Key, "yyyyMMddHHmmss", CultureInfo.InvariantCulture)),
                ConfirmationType = onlQuaConfirmationTypeDict.ContainsKey(item.Key) ? onlQuaConfirmationTypeDict[item.Key].confirmationType : 1,
                InfoConsFlg = onlQuaConfirmationTypeDict.ContainsKey(item.Key) ? onlQuaConfirmationTypeDict[item.Key].infConsFlg : "    ",
                ConfirmationResult = item.Value,
                CreateDate = DateTime.Now,
                CreateId = Session.UserID,
            });
        }
        var result = listOnlineConfirmationHistory.Select(item => ConvertToModel(item))
                                                  .OrderByDescending(item => item.OnlineConfirmationDate)
                                                  .ToList();
        return result;
    }

    public bool UpdateHokenInfOnlineQualify(int hpId, long ptId, int hokenId, long seqNo, List<HokenConfirmationModel> resultList, bool IsAddNewPatient = false)
    {
        bool result = false;
        if (resultList == null || resultList.Count == 0)
        {
            return result;
        }
        if (!resultList.Any(item => !item.IsReflect))
        {
            return result;
        }
        var hokenInf = TrackingDataContext.PtHokenInfs.FirstOrDefault(item => item.HpId == hpId
                                                                              && item.PtId == ptId
                                                                              && item.HokenId == hokenId
                                                                              && item.SeqNo == seqNo);
        if (hokenInf == null)
        {
            return false;
        }

        foreach (var model in resultList)
        {
            if (model.IsReflect)
            {
                continue;
            }
            switch (model.AttributeName)
            {
                case HokenConfOnlQuaConst.HOKENSYA_NO:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        string oldKigo = hokenInf?.Kigo ?? string.Empty;
                        string oldBango = hokenInf?.Bango ?? string.Empty;
                        hokenInf!.HokensyaNo = model.XmlValue;
                        hokenInf.Kigo = oldKigo;
                        hokenInf.Bango = oldBango;
                    }
                    break;
                case HokenConfOnlQuaConst.KIGO:
                    hokenInf.Kigo = model.XmlValue;
                    break;
                case HokenConfOnlQuaConst.BANGO:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        hokenInf.Bango = model.XmlValue;
                    }
                    break;
                case HokenConfOnlQuaConst.EDANO:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        hokenInf.EdaNo = model.XmlValue;
                    }
                    break;
                case HokenConfOnlQuaConst.HONKE:
                    hokenInf.HonkeKbn = model.XmlValue.AsInteger();
                    break;
                case HokenConfOnlQuaConst.KOFU_DATE:
                    if (CIUtil.CheckSDate(model.XmlValue))
                    {
                        hokenInf.KofuDate = model.XmlValue.AsInteger();
                    }
                    break;
                case HokenConfOnlQuaConst.START_DATE:
                    if (CIUtil.CheckSDate(model.XmlValue))
                    {
                        hokenInf.StartDate = model.XmlValue.AsInteger();
                    }
                    break;
                case HokenConfOnlQuaConst.END_DATE:
                    if (CIUtil.CheckSDate(model.XmlValue))
                    {
                        hokenInf.EndDate = model.XmlValue.AsInteger();
                    }
                    break;
                case HokenConfOnlQuaConst.KOGAKU_KBN:
                    if (!string.IsNullOrEmpty(model.XmlValue))
                    {
                        hokenInf.KogakuKbn = KogakuKbnToLimitConsFlgConverter.ClassificationFlagToKogakuKbn(model.XmlValue);
                    }
                    break;
                case HokenConfOnlQuaConst.CREDENTIAL:
                    break;
            }
        }

        // Update hoken 38 logic
        string xmlHokensyaNo = resultList.FirstOrDefault(item => item.AttributeName == HokenConfOnlQuaConst.HOKENSYA_NO)?.XmlValue ?? string.Empty;
        if ((!string.IsNullOrEmpty(xmlHokensyaNo)
            && xmlHokensyaNo.StartsWith("39")
            && xmlHokensyaNo.Length == 8)
            || IsAddNewPatient)
        {
            if (hokenInf.HonkeKbn == 0)
            {
                hokenInf.HonkeKbn = 1;
            }
            if (string.IsNullOrEmpty(hokenInf.Kigo))
            {
                hokenInf.Kigo = "　";
            }
        }

        // 資格証明
        var creadentialModel = resultList.FirstOrDefault(item => item.AttributeName == HokenConfOnlQuaConst.CREDENTIAL);
        string xmlCreadential = creadentialModel?.XmlValue ?? string.Empty;
        string currentCreadential = creadentialModel?.CurrentValue ?? string.Empty;
        bool isReflect = creadentialModel?.IsReflect ?? false;
        // Set hoken = 068
        if ((!isReflect && !string.IsNullOrEmpty(xmlCreadential))
            || (isReflect && !string.IsNullOrEmpty(currentCreadential)))
        {
            hokenInf.HokenNo = 68;
            hokenInf.HokenEdaNo = 0;
            hokenInf.HokenMasterModel = hokenInf.HokenMstFiltered?.Find(item => item.HokenNumber == hokenInf.HokenNo
                                                                                && item.HokenEdaNo == hokenInf.HokenEdaNo);
        }
        else
        {
            // Set Hoken Master = first hoken
            if (!isReflect && string.IsNullOrEmpty(xmlCreadential) && !string.IsNullOrEmpty(currentCreadential))
            {
                hokenInf.HokenMasterModel = hokenInf.HokenMstFiltered?.FirstOrDefault();
            }
        }

        // Update info
        var isUpdated = resultList.Where(item => !item.IsReflect && !string.IsNullOrEmpty(item.XmlValue)).Any();
        if (isUpdated)
        {
            hokenInf.UpdateDate = DateTime.Now;
            hokenInf.UpdateId = Session.UserID;
        }

        return TrackingDataContext.SaveChanges() > 0;
    }


    #region private function
    private OnlineConfirmationHistoryModel ConvertToModel(OnlineConfirmationHistory entity)
    {
        return new OnlineConfirmationHistoryModel(
                   entity.ID,
                   entity.PtId,
                   entity.OnlineConfirmationDate,
                   entity.ConfirmationType,
                   entity.InfoConsFlg ?? string.Empty,
                   entity.ConfirmationResult ?? string.Empty,
                   entity.PrescriptionIssueType,
                   entity.UketukeStatus
               );
    }

    private void UpdateInfConsFlgInRaiinInf(int userId, List<RaiinInf> raiinInfsInSameday, string infConsFlg)
    {
        var unConfirmedRaiinInfs = raiinInfsInSameday.Where(x => string.IsNullOrEmpty(x.InfoConsFlg));
        var confirmedRaiininfs = raiinInfsInSameday.Except(unConfirmedRaiinInfs).ToList();
        if (!confirmedRaiininfs.Any())
        {
            foreach (var raiinInf in unConfirmedRaiinInfs)
            {
                raiinInf.InfoConsFlg = infConsFlg;
                raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                raiinInf.UpdateId = userId;
            }
        }
        else
        {
            void UpdateFlgValue(int flgIdx)
            {
                char flgToChar(int flg)
                {
                    if (flg == 1)
                    {
                        return '1';
                    }
                    else if (flg == 2)
                    {
                        return '2';
                    }
                    return ' ';
                }
                var unCofirmedFlgRaiinInfs = confirmedRaiininfs.Where(x => x.InfoConsFlg?.Length > flgIdx && x.InfoConsFlg[flgIdx] == ' ');
                var confirmedFlgRaiinInfs = confirmedRaiininfs.Where(x => x.InfoConsFlg?.Length > flgIdx && x.InfoConsFlg[flgIdx] != ' ');
                if (!confirmedFlgRaiinInfs.Any())
                {
                    foreach (var raiinInf in unCofirmedFlgRaiinInfs)
                    {
                        raiinInf.InfoConsFlg = ReplaceAt(raiinInf.InfoConsFlg ?? string.Empty, flgIdx, infConsFlg[flgIdx]);
                        raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        raiinInf.UpdateId = userId;
                    }
                }
                else
                {
                    int minFlg = confirmedFlgRaiinInfs.Min(x => x.InfoConsFlg![flgIdx].AsInteger());
                    int respondedFlg = infConsFlg[flgIdx] == ' ' ? 0 : infConsFlg![flgIdx].AsInteger();
                    int newFlg = respondedFlg == 0 ? minFlg : (minFlg > respondedFlg ? respondedFlg : minFlg);
                    foreach (var raiinInf in confirmedFlgRaiinInfs)
                    {
                        raiinInf.InfoConsFlg = ReplaceAt(raiinInf.InfoConsFlg ?? string.Empty, flgIdx, flgToChar(newFlg));
                        raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        raiinInf.UpdateId = userId;
                    }
                    foreach (var raiinInf in unCofirmedFlgRaiinInfs)
                    {
                        raiinInf.InfoConsFlg = ReplaceAt(raiinInf.InfoConsFlg ?? string.Empty, flgIdx, flgToChar(newFlg));
                        raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        raiinInf.UpdateId = userId;
                    }
                }
            }
            //Update PharmacistsInfoConsFlg
            UpdateFlgValue(0);
            //Update SpecificHealthCheckupsInfoConsFlg
            UpdateFlgValue(1);
            //Update DiagnosisInfoConsFlg
            UpdateFlgValue(2);
            //Update OperationInfoConsFlg
            UpdateFlgValue(3);

            //Apply computed infoconsflg for the new raiininf which has nullable infoconsFlg value
            string newInfoConsFlg = confirmedRaiininfs.FirstOrDefault()?.InfoConsFlg ?? string.Empty;
            foreach (var raiinInf in unConfirmedRaiinInfs)
            {
                raiinInf.InfoConsFlg = newInfoConsFlg;
                raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                raiinInf.UpdateId = userId;

            }
        }
        NoTrackingDataContext.SaveChanges();
    }

    private string ReplaceAt(string input, int index, char newChar)
    {
        if (input == null)
        {
            throw new ArgumentNullException("input");
        }
        StringBuilder builder = new StringBuilder(input);
        builder[index] = newChar;
        return builder.ToString();
    }
    #endregion

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
