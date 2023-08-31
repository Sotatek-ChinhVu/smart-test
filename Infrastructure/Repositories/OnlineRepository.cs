using Domain.Models.Online;
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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

    public void UpdateInfConsFlgInRaiinInf(int userId, List<RaiinInf> raiinInfsInSameday, string infConsFlg)
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

    public string ReplaceAt(string input, int index, char newChar)
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
    }
}
