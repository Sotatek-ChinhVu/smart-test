using Domain.Constant;
using Domain.Converter;
using Domain.Models.Insurance;
using Domain.Models.Online;
using Domain.Models.Online.QualificationConfirmation;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
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
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IReceptionRepository _receptionRepository;
    private readonly ISystemConfRepository _systemConfig;

    public OnlineRepository(ITenantProvider tenantProvider, IInsuranceRepository insuranceRepository, IReceptionRepository receptionRepository, ISystemConfRepository systemConfig) : base(tenantProvider)
    {
        _insuranceRepository = insuranceRepository;
        _receptionRepository = receptionRepository;
        _systemConfig = systemConfig;
    }

    public List<long> InsertOnlineConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> onlineList)
    {
        List<long> idList = new();
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
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
                    foreach (var online in onlineInsertList)
                    {
                        TrackingDataContext.AddRange(online);
                        TrackingDataContext.SaveChanges();
                        idList.Add(online.ID);
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });
        return idList;
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
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
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
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
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
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
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
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
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

    public List<OnlineConfirmationHistoryModel> GetListOnlineConfirmationHistoryModel(int userId, Dictionary<string, string> onlQuaResFileDict, Dictionary<string, (int confirmationType, string infConsFlg)> onlQuaConfirmationTypeDict)
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
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
            });
        }
        var result = listOnlineConfirmationHistory.Select(item => ConvertToModel(item))
                                                  .OrderByDescending(item => item.OnlineConfirmationDate)
                                                  .ToList();
        return result;
    }

    public List<OnlineConsentModel> GetOnlineConsentModel(long ptId)
    {
        var systemDate = CIUtil.GetJapanDateTimeNow();
        var onlineConsentList = NoTrackingDataContext.OnlineConsents.Where(item => item.PtId == ptId
                                                                                   && new List<int>() { 1, 2, 3 }.Contains(item.ConsKbn))
                                                                    .ToList();
        onlineConsentList = onlineConsentList.Where(item => item.LimitDate >= systemDate).ToList();
        return onlineConsentList.Select(item => new OnlineConsentModel(item.PtId, item.ConsKbn, item.ConsDate, item.LimitDate)).ToList();
    }

    public bool UpdateOnlineConsents(int userId, long ptId, List<QCXmlMsgResponse> responseList)
    {
        void UpdateOnlineConsent(int consKbn, DateTime consDate, DateTime limitDate, List<OnlineConsent> onlineConsentList)
        {
            if (consKbn == 0) return;
            var onlineConsent = onlineConsentList.FirstOrDefault(x => x.PtId == ptId && x.ConsKbn == consKbn);
            if (onlineConsent != null)
            {
                onlineConsent.ConsDate = consDate;
                onlineConsent.LimitDate = limitDate;
                onlineConsent.UpdateDate = CIUtil.GetJapanDateTimeNow();
                onlineConsent.UpdateId = userId;
            }
            else
            {
                TrackingDataContext.OnlineConsents.Add(new OnlineConsent()
                {
                    PtId = ptId,
                    ConsKbn = consKbn,
                    ConsDate = consDate,
                    LimitDate = limitDate,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                });
            }
            TrackingDataContext.SaveChanges();
        }

        bool successed = true;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    foreach (var confirmation in from response in responseList
                                                 where response.MessageBody.ResultList != null && response.MessageBody.ResultList.ResultOfQualificationConfirmation != null
                                                 from confirmation in response.MessageBody.ResultList.ResultOfQualificationConfirmation
                                                 select confirmation)
                    {
                        var onlineConsentList = TrackingDataContext.OnlineConsents.Where(item => item.PtId == ptId && new List<int>() { 1, 2, 3, 4 }.Contains(item.ConsKbn)).ToList();
                        if (confirmation.SpecificHealthCheckupsInfoConsFlg.AsInteger() == 1)
                        {
                            int consKbn = 2;
                            DateTime consDate = DateTime.SpecifyKind(CIUtil.StrDateToDate(confirmation.SpecificHealthCheckupsInfoConsTime), DateTimeKind.Utc);
                            DateTime limitDate = DateTime.SpecifyKind(CIUtil.StrDateToDate(confirmation.SpecificHealthCheckupsInfoAvailableTime), DateTimeKind.Utc);
                            UpdateOnlineConsent(consKbn, consDate, limitDate, onlineConsentList);
                        }

                        if (confirmation.PharmacistsInfoConsFlg.AsInteger() == 1)
                        {
                            int consKbn = 1;
                            DateTime consDate = DateTime.SpecifyKind(CIUtil.StrDateToDate(confirmation.PharmacistsInfoConsTime), DateTimeKind.Utc);
                            DateTime limitDate = DateTime.SpecifyKind(CIUtil.StrDateToDate(confirmation.PharmacistsInfoAvailableTime), DateTimeKind.Utc);
                            UpdateOnlineConsent(consKbn, consDate, limitDate, onlineConsentList);
                        }

                        if (confirmation.DiagnosisInfoConsFlg.AsInteger() == 1)
                        {
                            int consKbn = 3;
                            DateTime consDate = DateTime.SpecifyKind(CIUtil.StrDateToDate(confirmation.DiagnosisInfoConsTime), DateTimeKind.Utc);
                            DateTime limitDate = DateTime.SpecifyKind(CIUtil.StrDateToDate(confirmation.DiagnosisInfoAvailableTime), DateTimeKind.Utc);
                            UpdateOnlineConsent(consKbn, consDate, limitDate, onlineConsentList);
                        }

                        if (confirmation.OperationInfoConsFlg.AsInteger() == 1)
                        {
                            int consKbn = 4;
                            DateTime consDate = DateTime.SpecifyKind(CIUtil.StrDateToDate(confirmation.OperationInfoConsTime), DateTimeKind.Utc);
                            DateTime limitDate = DateTime.SpecifyKind(CIUtil.StrDateToDate(confirmation.OperationInfoAvailableTime), DateTimeKind.Utc);
                            UpdateOnlineConsent(consKbn, consDate, limitDate, onlineConsentList);
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            });
        return successed;
    }


    public List<QualificationInfModel> GetListQualificationInf()
    {
        var listOnlineConfirmation = NoTrackingDataContext.OnlineConfirmations
                                        .OrderByDescending(u => u.CreateDate)
                                        .Select(x => new QualificationInfModel(
                                                        x.ReceptionNo,
                                                        x.ReceptionDateTime,
                                                        x.YoyakuDate,
                                                        x.SegmentOfResult ?? string.Empty,
                                                        x.ErrorMessage ?? string.Empty
                                            ))
                                        .Take(50)
                                        .ToList();
        return listOnlineConfirmation;
    }

    public bool SaveOnlineConfirmation(int userId, QualificationInfModel qualificationInf, ModelStatus status)
    {
        if (status == ModelStatus.Added)
        {
            var onlConfirm = new OnlineConfirmation();
            onlConfirm.ReceptionNo = qualificationInf.ReceptionNo;
            onlConfirm.ReceptionDateTime = CIUtil.SetKindUtc(qualificationInf.ReceptionDateTime);
            onlConfirm.YoyakuDate = qualificationInf.YoyakuDate;
            onlConfirm.SegmentOfResult = qualificationInf.SegmentOfResult;
            onlConfirm.ErrorMessage = qualificationInf.ErrorMessage;
            onlConfirm.CreateDate = CIUtil.GetJapanDateTimeNow();
            onlConfirm.CreateId = userId;
            onlConfirm.UpdateDate = CIUtil.GetJapanDateTimeNow();
            onlConfirm.UpdateId = userId;

            TrackingDataContext.OnlineConfirmations.Add(onlConfirm);
            return TrackingDataContext.SaveChanges() > 0;
        }
        else if (status == ModelStatus.Modified)
        {
            var onlConfirm = TrackingDataContext.OnlineConfirmations.FirstOrDefault(x => x.ReceptionNo == qualificationInf.ReceptionNo);

            if (onlConfirm != null)
            {
                onlConfirm.SegmentOfResult = qualificationInf.SegmentOfResult;
                onlConfirm.ErrorMessage = qualificationInf.ErrorMessage;
                onlConfirm.UpdateDate = CIUtil.GetJapanDateTimeNow();
                onlConfirm.UpdateId = userId;

                return TrackingDataContext.SaveChanges() > 0;
            }
        }

        return true;
    }

    public bool InsertListOnlConfirmHistory(int userId, List<OnlineConfirmationHistoryModel> listOnlineConfirmationHistoryModel)
    {
        foreach (var item in listOnlineConfirmationHistoryModel)
        {
            TrackingDataContext.Add(new OnlineConfirmationHistory()
            {
                PtId = item.PtId,
                OnlineConfirmationDate = item.OnlineConfirmationDate,
                InfoConsFlg = item.InfoConsFlg,
                ConfirmationType = item.ConfirmationType,
                PrescriptionIssueType = item.PrescriptionIssueType,
                ConfirmationResult = item.ConfirmationResult,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                UketukeStatus = item.UketukeStatus,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId,
            });
        }

        return TrackingDataContext.SaveChanges() > 0;

    }

    public (bool, List<ReceptionRowModel> receptions) UpdateRaiinInfByResResult(int hpId, int userId, List<ConfirmResultModel> listResResult)
    {
        var raiinInfsChange = new List<RaiinInf>();

        listResResult = listResResult.Where(u => u.PtId > 0).ToList();
        if (listResResult.Count == 0)
        {
            return (true, new());
        }
        foreach (var resResult in listResResult)
        {
            var raiinInfToUpdate = GetRaiinInfToUpdateByPtId(hpId, resResult.PtId, resResult.SinDate);
            if (raiinInfToUpdate.raiinInfs == null)
            {
                continue;
            }

            if (raiinInfToUpdate.referenceNo != resResult.ReferenceNo && resResult.ReferenceNo != 0)
            {
                resResult.ChangeConfirmationStatus(97);
                resResult.ChangeConfirmationResult("照会番号不一致");
            }
            else
            {
                if (resResult.ConfirmationStatus != 4)
                {
                    var ptHokenCheckModel = TrackingDataContext.PtHokenChecks.FirstOrDefault(u => u.HpId == hpId &&
                                                                                                    u.PtID == resResult.PtId &&
                                                                                                    u.HokenId == resResult.HokenId &&
                                                                                                    u.HokenGrp == resResult.HokenGrp &&
                                                                                                    u.CheckDate == CIUtil.IntToDate(resResult.CheckDate) &&
                                                                                                    u.IsDeleted == DeleteStatus.None);
                    if (ptHokenCheckModel != null)
                    {
                        ptHokenCheckModel.CheckCmt = resResult.PtHokenCheckModel.CheckComment;
                        ptHokenCheckModel.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        ptHokenCheckModel.UpdateId = userId;
                    }
                    else
                    {
                        TrackingDataContext.PtHokenChecks.Add(new PtHokenCheck()
                        {
                            HpId = hpId,
                            PtID = resResult.PtHokenCheckModel.PtId,
                            HokenGrp = resResult.PtHokenCheckModel.HokenGrp,
                            HokenId = resResult.PtHokenCheckModel.HokenId,
                            CheckDate = CIUtil.IntToDate(resResult.PtHokenCheckModel.ConfirmDate),
                            CheckId = resResult.PtHokenCheckModel.CheckId,
                            CheckCmt = resResult.PtHokenCheckModel.CheckComment,
                            IsDeleted = 0,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = userId,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = userId,
                        });
                    }
                }
            }

            foreach (var raiinInf in raiinInfToUpdate.raiinInfs)
            {
                raiinInf.ConfirmationResult = GetConfirmationResult(hpId, resResult, raiinInf.SinDate);
                raiinInf.ConfirmationState = resResult.ConfirmationStatus;
                raiinInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                raiinInf.UpdateId = userId;
            }

            foreach (var raiinInf in raiinInfToUpdate.raiinInfs)
            {
                raiinInfsChange.Add(raiinInf);
            }
        }

        var saveChanges = TrackingDataContext.SaveChanges() > 0;
        return (saveChanges, saveChanges ? GetListRaiinInf(raiinInfsChange) : new());
    }

    private List<ReceptionRowModel> GetListRaiinInf(List<RaiinInf> raiinInfs)
    {
        var result = new List<ReceptionRowModel>();
        foreach (var raiinInf in raiinInfs)
        {
            result.AddRange(_receptionRepository.GetList(raiinInf.HpId, raiinInf.SinDate, CommonConstants.InvalidId, raiinInf.PtId, isDeleted: 0));
        }

        return result;
    }

    private (List<RaiinInf>? raiinInfs, long referenceNo) GetRaiinInfToUpdateByPtId(int hpId, long ptId, int sinDate)
    {
        var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(u => u.HpId == hpId &&
                                                                           u.PtId == ptId &&
                                                                           u.IsDelete == DeleteStatus.None &&
                                                                           u.IsTester == 0);
        if (ptInf == null)
        {
            return (null, 0);
        }
        var listRaiinInf = TrackingDataContext.RaiinInfs.Where(u => u.HpId == hpId &&
                                                                   u.PtId == ptId &&
                                                                   u.SinDate == sinDate &&
                                                                   u.Status == RaiinState.Reservation &&
                                                                   u.IsDeleted == DeleteStatus.None).ToList();
        if (listRaiinInf.Count > 0)
        {
            return (listRaiinInf, ptInf.ReferenceNo);
        }
        return (null, 0);
    }

    private string GetConfirmationResult(int hpId, ConfirmResultModel resResult, int sinDate)
    {
        var hokenInfs = _insuranceRepository.FindHokenInfByPtId(hpId, resResult.PtId);
        var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == resResult.PtId && p.IsDelete != 1);
        var matchPtInf = new List<PtInfConfirmationModel>();
        var resultOfQC = new ResultOfQualificationConfirmation()
        {
            NameKana = resResult.NameKana,
            Name = resResult.Name,
            Sex1 = resResult.Sex1,
            Sex2 = resResult.Sex2,
            PostNumber = resResult.PostNumber,
            Birthdate = resResult.Birthday,
            Address = resResult.Address,
            InsuredName = resResult.InsuredName,
            InsurerNumber = resResult.InsurerNumber,
            InsuredCardSymbol = resResult.InsuredCardSymbol,
            InsuredBranchNumber = resResult.InsuredBranchNumber,
            PersonalFamilyClassification = resResult.PersonalFamilyClassification,
            InsuredCertificateIssuanceDate = resResult.InsuredCertificateIssuanceDate,
            InsuredCardValidDate = resResult.InsuredCardValidDate,
            InsuredCardExpirationDate = resResult.InsuredCardExpirationDate,
        };
        var matchHokenInfs = hokenInfs.FindAll(p => PatientInfoConverter.GetHokenConfirmationModels(p, resultOfQC, resResult.Birthday.AsInteger(), sinDate).All(x => x.IsReflect));

        var systemConfigList = _systemConfig.GetList(hpId, new List<int> { 100029 });

        int nameBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 1)?.Val ?? 1);
        int kanaNameBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 2)?.Val ?? 1);
        int genderBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 3)?.Val ?? 1);
        int birthDayBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 4)?.Val ?? 1);
        int addressBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 5)?.Val ?? 1);
        int postcodeBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 6)?.Val ?? 1);
        int seitaiNushiBasicInfoCheck = (int)(systemConfigList.FirstOrDefault(item => item.GrpEdaNo == 7)?.Val ?? 1);

        if (ptInf != null)
        {
            var ptInfModel = ConvertToPatientInfoModel(ptInf);

            matchPtInf = new List<PtInfConfirmationModel>
            {
                new PtInfConfirmationModel(PtInfOQConst.KANA_NAME, 
                                           ptInfModel.KanaName, 
                                           resultOfQC.NameKana ,
                                           nameBasicInfoCheck,
                                           kanaNameBasicInfoCheck,
                                           genderBasicInfoCheck,
                                           birthDayBasicInfoCheck,
                                           addressBasicInfoCheck,
                                           postcodeBasicInfoCheck,
                                           seitaiNushiBasicInfoCheck),
                new PtInfConfirmationModel(PtInfOQConst.KANJI_NAME, 
                                           ptInfModel.Name, 
                                           resultOfQC.Name, 
                                           nameBasicInfoCheck,
                                           kanaNameBasicInfoCheck,
                                           genderBasicInfoCheck,
                                           birthDayBasicInfoCheck,
                                           addressBasicInfoCheck,
                                           postcodeBasicInfoCheck,
                                           seitaiNushiBasicInfoCheck),
                new PtInfConfirmationModel(PtInfOQConst.SEX, 
                                           ptInfModel.Sex.AsString(), 
                                           string.IsNullOrEmpty(resultOfQC.Sex2) ? resultOfQC.Sex1: resultOfQC.Sex2, 
                                           nameBasicInfoCheck,
                                           kanaNameBasicInfoCheck,
                                           genderBasicInfoCheck,
                                           birthDayBasicInfoCheck,
                                           addressBasicInfoCheck,
                                           postcodeBasicInfoCheck,
                                           seitaiNushiBasicInfoCheck),
                new PtInfConfirmationModel(PtInfOQConst.BIRTHDAY, 
                                           ptInfModel.Birthday.AsString(), 
                                           resultOfQC.Birthdate.AsString(),
                                           nameBasicInfoCheck,
                                           kanaNameBasicInfoCheck,
                                           genderBasicInfoCheck,
                                           birthDayBasicInfoCheck,
                                           addressBasicInfoCheck,
                                           postcodeBasicInfoCheck,
                                           seitaiNushiBasicInfoCheck),
                new PtInfConfirmationModel(PtInfOQConst.SETANUSI, ptInfModel.Setanusi, resultOfQC.InsuredName, nameBasicInfoCheck, kanaNameBasicInfoCheck, genderBasicInfoCheck, birthDayBasicInfoCheck, addressBasicInfoCheck, postcodeBasicInfoCheck, seitaiNushiBasicInfoCheck),
                new PtInfConfirmationModel(PtInfOQConst.HOME_ADDRESS, ptInfModel.HomeAddress1, resultOfQC.Address, nameBasicInfoCheck, kanaNameBasicInfoCheck, genderBasicInfoCheck, birthDayBasicInfoCheck, addressBasicInfoCheck, postcodeBasicInfoCheck, seitaiNushiBasicInfoCheck),
                new PtInfConfirmationModel(PtInfOQConst.HOME_POST, ptInfModel.HomePost, resultOfQC.PostNumber, nameBasicInfoCheck, kanaNameBasicInfoCheck, genderBasicInfoCheck, birthDayBasicInfoCheck, addressBasicInfoCheck, postcodeBasicInfoCheck, seitaiNushiBasicInfoCheck),
            };
            matchPtInf = matchPtInf.Where(x => x.IsVisible).ToList();
        }
        if (matchPtInf.Any(x => !x.IsReflect) && matchHokenInfs.Count <= 0)
        {
            return "[基本・保険差異あり] " + resResult.ProcessingResultMessage;
        }
        else if (!matchPtInf.Any(x => !x.IsReflect) && matchHokenInfs.Count <= 0)
        {
            return "[保険差異あり] " + resResult.ProcessingResultMessage;
        }
        else if (matchPtInf.Any(x => !x.IsReflect) && matchHokenInfs.Count > 0)
        {
            return "[基本差異あり] " + resResult.ProcessingResultMessage;
        }
        return resResult.ProcessingResultMessage;
    }

    private static PatientInforModel ConvertToPatientInfoModel(PtInf ptInf)
    {
        return new PatientInforModel(
                    ptInf.KanaName ?? string.Empty,
                    ptInf.Name ?? string.Empty,
                    ptInf.Sex,
                    ptInf.Birthday,
                    ptInf.HomePost ?? string.Empty,
                    ptInf.HomeAddress1 ?? string.Empty,
                    ptInf.Setanusi ?? string.Empty
                );
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
        TrackingDataContext.SaveChanges();
    }

    private string ReplaceAt(string input, int index, char newChar)
    {
        if (input == null)
        {
            return string.Empty;
        }
        StringBuilder builder = new StringBuilder(input);
        builder[index] = newChar;
        return builder.ToString();
    }
    #endregion

    public void ReleaseResource()
    {
        DisposeDataContext();
        _insuranceRepository.ReleaseResource();
        _receptionRepository.ReleaseResource();
    }
}
