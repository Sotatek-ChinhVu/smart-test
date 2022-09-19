using Domain.Constant;
using Domain.Models.SwapHoken;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class SwapHokenRepository : ISwapHokenRepository
    {
        private readonly TenantDataContext _tenantDataContext;

        public SwapHokenRepository(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        public long CountOdrInf(int hpId,long ptId, int hokenPid, int startDate, int endDate)
        {
            return _tenantDataContext.OdrInfs.Count((x) =>
                            x.HpId == hpId &&
                            x.PtId == ptId &&
                            x.HokenPid == hokenPid &&
                            x.SinDate >= startDate &&
                            x.SinDate <= endDate &&
                            x.IsDeleted == 0);

        }

        public List<int> GetListSeikyuYms(int hpId, long ptId, int hokenPid, int startDate, int endDate)
        {
            var listRaiin = _tenantDataContext.RaiinInfs.Where(x =>
                        x.HpId == hpId &&
                        x.PtId == ptId &&
                        x.HokenPid == hokenPid &&
                        x.Status >= RaiinState.TempSave &&
                        x.IsDeleted == DeleteTypes.None &&
                        x.SinDate >= startDate &&
                        x.SinDate <= endDate);

            return listRaiin.GroupBy(x => x.SinDate / 100).Select(x => x.Key).ToList();
        }

        public List<int> GetSeikyuYmsInPendingSeikyu(int hpId,long ptId, List<int> sinYms, int hokenId)
        {
            return _tenantDataContext.ReceSeikyus.Where(p => p.HpId == hpId &&
                                                        p.PtId == ptId &&
                                                        p.IsDeleted == DeleteTypes.None &&
                                                        sinYms.Contains(p.SinYm) &&
                                                        p.SeikyuYm != 999999 &&
                                                        p.HokenId == hokenId)
                                                 .Select(p => p.SeikyuYm).ToList();
        }

        public bool SwapHokenParttern(int hpId,long PtId, int OldHokenPid, int NewHokenPid, int StartDate, int EndDate)
        {
            var updateDate = DateTime.Now;
            var updateMachine = TempIdentity.ComputerName;
            var updateId = TempIdentity.UserId;

            #region UpdateHokenPatternInRaiin
            var raiinInfs = _tenantDataContext.RaiinInfs.Where((x) =>
                x.HpId == hpId &&
                x.PtId == PtId &&
                x.HokenPid == OldHokenPid &&
                x.Status >= RaiinState.TempSave &&
                x.SinDate >= StartDate &&
                x.SinDate <= EndDate &&
                x.IsDeleted == DeleteTypes.None);
            var raiinInfList = raiinInfs.Select(p => p).ToList();
            foreach (var raiinInf in raiinInfList)
            {
                raiinInf.HokenPid = NewHokenPid;
                raiinInf.UpdateDate = updateDate;
                raiinInf.UpdateMachine = updateMachine;
                raiinInf.UpdateId = updateId;
            }
            #endregion

            #region UpdateHokenPatternInOdrInf
            var odrInfs = _tenantDataContext.OdrInfs.Where((x) =>
                    x.HpId == hpId &&
                    x.PtId == PtId &&
                    x.SinDate >= StartDate &&
                    x.SinDate <= EndDate &&
                    x.HokenPid == OldHokenPid);
            var odrInfList = odrInfs.Select((p) => p).ToList();
            odrInfList.ForEach((x) =>
            {
                x.HokenPid = NewHokenPid;
                x.UpdateDate = updateDate;
                x.UpdateMachine = updateMachine;
                x.UpdateId = updateId;
            });
            #endregion

            return _tenantDataContext.SaveChanges() > 0;
        }

        public bool ExistRaiinInfUsedOldHokenId(int hpId,long ptId, List<int> sinYms, int oldHokenPId)
        {
            return _tenantDataContext.RaiinInfs.Any(p => p.HpId == hpId &&
                                                                              p.PtId == ptId &&
                                                                              sinYms.Contains(p.SinDate / 100) &&
                                                                              p.HokenPid == oldHokenPId &&
                                                                              p.Status >= RaiinState.TempSave &&
                                                                              p.IsDeleted == DeleteTypes.None);
        }

        public bool UpdateReceSeikyu(int hpId,long ptId, List<int> seiKyuYms, int oldHokenId, int newHokenId)
        {
            var receSeiKyus = _tenantDataContext.ReceSeikyus.Where(p => p.HpId == hpId && p.PtId == ptId && seiKyuYms.Contains(p.SeikyuYm));
            foreach (var receSeiKyu in receSeiKyus)
            {
                if (oldHokenId != newHokenId && receSeiKyu.HokenId == newHokenId)
                {
                    receSeiKyu.IsDeleted = DeleteTypes.Deleted;
                }
                receSeiKyu.HokenId = newHokenId;
                receSeiKyu.UpdateDate = DateTime.Now;
                receSeiKyu.UpdateId = TempIdentity.UserId;
                receSeiKyu.UpdateMachine = TempIdentity.ComputerName;
            }
            _tenantDataContext.SaveChanges();
            return true;
        }

        public bool ReceCalcProgress(int hpId,bool isReCalculation, bool isReceCalculation, bool isReceCheckError,List<long> ptIds,List<int> seikyuYms)
        {
            for (int i = 0; i < seikyuYms.Count; i++)
            {
                if (isReCalculation)
                {
                    RunCalculateMonth(hpId, seikyuYms[i], ptIds);
                }

                if(isReceCalculation)
                {
                    ReceFutanCalculateMain(hpId,ptIds, seikyuYms[i]);
                }

                if (isCheckError)
                {
                    recalculationVM.CheckErrorInMonth(seikyuYms[i], _ptIds);
                }

            }
            return true;

            void RunCalculateMonth(int hpId, int seikyuYm, List<long> ptIds, string preFix = "")
            {
                preFix = preFix + "MON_";

                //要求登録
                List<RaiinDayModel> raiinDays = FindRaiinInfDaysInMonth(hpId, seikyuYm, ptIds);
                List<CalcStatus> CalcStatusies = raiinDays.Select(r => new CalcStatus()
                {
                    HpId = r.HpId,
                    PtId = r.PtId,
                    SinDate = r.SinDate,
                    CalcMode = CalcModeConst.Continuity
                }).ToList();

                AddCalcStatus(CalcStatusies, preFix);

                int AllCalcCount = _tenantDataContext.CalcStatus
                                .Count(p => p.CreateMachine == preFix + TempIdentity.ComputerName &&
                                                               p.Status == 0);


                //計算処理
                RunCalculate(hpId, 0, 0, 0, preFix);

                void RunCalculate(int hpId, long ptId, int sinDate, int seikyuUp, string preFix = "")
                {
                    int CalculatedCount = 0;

                    
                    CalcStatus? calcStatus = null;

                    // 要求登録           
                    AddCalcStatusC(hpId, ptId, sinDate, seikyuUp, preFix);


                    // 要求がある限りループ
                    while (GetCalcStatus(hpId, ptId, sinDate, ref calcStatus, preFix))
                    {
                        if (calcStatus?.SinDate <= 20180331)
                        {
                            List<CalcStatus> calcStatusies = new List<CalcStatus>();

                            if (calcStatus.PtId == ptId && calcStatus.SinDate == sinDate)
                            {
                                calcStatusies.AddRange(GetSameCalcStatus(calcStatus.CalcId, preFix));
                            }
                            else
                            {
                                calcStatusies.AddRange(GetSameCalcStatusInputModel(calcStatus, preFix));
                            }
                            foreach (CalcStatus updCalcStatus in calcStatusies)
                            {
                                updCalcStatus.Status = 8;
                            }

                            if (UpdateCalcStatus(calcStatusies) == false)
                            {
                                // falseのまま、放置するわけにいかないのでリトライする
                                List<long> calcIds = calcStatusies.Select(p => p.CalcId).ToList();

                                List<CalcStatus> updCalcStatusies = GetCalcStatusies(calcIds, preFix);

                                foreach (CalcStatus updCalcStatus in updCalcStatusies)
                                {
                                    updCalcStatus.Status = 8;
                                }

                            }
                        }
                        else
                        {
                            int i = 0;

                            if (CheckCalcStatus(calcStatus))
                            {
                                // チェック
                                // 他端末で当該患者の当該診療日が属する月の計算中の場合、待機する
                                while (CheckCalcStatusOther(calcStatus) && i < 300)
                                {
                                    i++;

                                    int ret = -1;

                                    if(calcStatus!= null)
                                    {
                                        var entities = _tenantDataContext.CalcStatus.Where(p =>
                                        p.CalcId == calcStatus.CalcId);

                                        if (entities != null && entities.Any())
                                        {
                                            ret = entities.First().Status;
                                        }
                                    }
                                    
                                    if (ret != 0)
                                    {
                                        // 待機中にステータスが変更された場合、この要求は他のプロセスが処理しているので処理しない
                                        break;
                                    }
                                }

                                // 自端末で当該患者の当該診療日が属する月の計算中の場合、待機する
                                i = 0;
                                while (CheckCalcStatusSelf(calcStatus) && i < 30)
                                {
                                    i++;
                                    
                                    if(calcStatus != null)
                                    {
                                        if (GetCalcStatusint(calcStatus.CalcId) != 0)
                                        {
                                            // 待機中にステータスが変更された場合、この要求は他のプロセスが処理しているので処理しない
                                            break;
                                        }
                                    }
                                }

                                if (calcStatus != null)
                                {
                                    if (GetCalcStatusint(calcStatus.CalcId) != 0)
                                    {
                                        continue;
                                    }
                                }
                            }

                            
                            // 要求ロック
                            List<CalcStatus> calcStatusies = new List<CalcStatus>();

                            if (calcStatus != null && calcStatus.PtId == ptId && calcStatus.SinDate == sinDate)
                            {
                                calcStatusies.AddRange(GetSameCalcStatus(calcStatus.CalcId, preFix));
                            }
                            else
                            {
                                calcStatusies.AddRange(GetSameCalcStatusInputModel(calcStatus, preFix));
                            }
                            foreach (CalcStatus updCalcStatus in calcStatusies)
                            {
                                updCalcStatus.Status = 1;
                            }

                            if(calcStatus != null && GetCalcStatusint(calcStatus.CalcId) != 0)
                            {
                                continue;
                            }

                            if (UpdateCalcStatus(calcStatusies) == false)
                            {
                                // falseのまま、放置するわけにいかないので、0に戻すようリトライする
                                List<long> calcIds = calcStatusies.Select(p => p.CalcId).ToList();

                                List<CalcStatus> updCalcStatusies = GetCalcStatusies(calcIds, preFix);

                                foreach (CalcStatus updCalcStatus in updCalcStatusies)
                                {
                                    updCalcStatus.Status = 0;
                                }

                            }
                            else
                            {
                                //計算準備
                                StartCalculate(calcStatus.HpId, calcStatus.PtId, calcStatus.SinDate, calcStatus.SeikyuUp, calcStatus.CalcMode, preFix);

                                //計算処理
                                bool ret = MainCalculate();

                                // 点数マスタのキャッシュを受け取り（次回の計算に引き継ぐため）
                                _cacheTenMst = _common.CacheTenMst;

                                if (ret == true)
                                {
                                    // 負担金計算用引数データ取得
                                    (List<Calculate.Futan.Models.SinKouiCountModel> argSinKouiCountModels,
                                        List<Calculate.Futan.Models.SinKouiModel> argSinKouiModels,
                                        List<Calculate.Futan.Models.SinKouiDetailModel> argSinKouiDetailModels,
                                        List<Calculate.Futan.Models.SinRpInfModel> argSinRpInfModels) =
                                            GetArgSinData();

                                    FutancalcViewModel FutanCalcVM = new FutancalcViewModel();
                                    try
                                    {
                                        FutanCalcVM.FutanCalculation(_common.ptId, _common.sinDate, argSinKouiCountModels, argSinKouiModels, argSinKouiDetailModels, argSinRpInfModels, calcStatus.SeikyuUp);
                                    }
                                    catch (Exception e)
                                    {
                                       ret = false;

                                        _common.AppendCalcLog(9, "負担金計算で問題が発生したため、計算できません。");
                                        UpdateCalcLog();
                                    }
                                    finally
                                    {
                                        FutanCalcVM.Dispose();
                                    }

                                }

                                // 要求更新
                                if (ret == true)
                                {
                                    // 正常終了
                                    //calcStatus.Status = 9;
                                    foreach (CalcStatusModel updCalcStatus in calcStatusies)
                                    {
                                        updCalcStatus.Status = 9;
                                    }
                                }
                                else
                                {
                                    // エラー
                                    //calcStatus.Status = 8;
                                    foreach (CalcStatusModel updCalcStatus in calcStatusies)
                                    {
                                        updCalcStatus.Status = 8;
                                    }
                                }
                                //_saveIkaCalculateCommandHandler.UpdateCalcStatus(calcStatus);
                                if (_saveIkaCalculateCommandHandler.UpdateCalcStatus(calcStatusies) == false)
                                {
                                    // falseのまま、放置するわけにいかないのでリトライする
                                    List<long> calcIds = calcStatusies.Select(p => p.CalcId).ToList();

                                    List<CalcStatusModel> updCalcStatusies = _ikaCalculateFinder.GetCalcStatusies(calcIds, preFix);

                                    foreach (CalcStatusModel updCalcStatus in updCalcStatusies)
                                    {
                                        updCalcStatus.Status = 8;
                                    }

                                    if (_saveIkaCalculateCommandHandler.UpdateCalcStatus(updCalcStatusies) == false)
                                    {
                                        Log.WriteLogError(ModuleName, this, conFncName, null, "update calcstatus error (8)");
                                    }
                                }

                                // レセプト状態情報のSTATUS_KBN=8を0に戻す
                                if (ikaCalculateArgumentViewModel.clearReceChk == 1)
                                {
                                    List<int> hokenIds = _common.Sin.SinKouis.FindAll(p => p.IsDeleted == DeleteStatus.None).GroupBy(p => p.HokenId).Select(p => p.Key).ToList();
                                    List<Emr.Calculate.Ika.Models.ReceStatusModel> receStatusies = _ikaCalculateFinder.FindReceStatus(hpId, ptId, sinDate / 100, hokenIds);
                                    _saveIkaCalculateCommandHandler.UpdateReceStatus(receStatusies);
                                }
                            }
                        }
                        CalculatedCount++;
                    }

                }

                bool UpdateCalcStatus(List<CalcStatus> calcStatusies)
                {
                    bool ret = true;
                    foreach (CalcStatus calcStatus in calcStatusies)
                    {
                        calcStatus.UpdateDate = DateTime.Now;
                        calcStatus.UpdateId = TempIdentity.UserId;
                        calcStatus.UpdateMachine = TempIdentity.ComputerName;
                    }
                    _tenantDataContext.SaveChanges();
                    return ret;
                }

                int GetCalcStatusint(long calcId)
                {
                    int ret = -1;

                    var entities = _tenantDataContext.CalcStatus.Where(p =>
                        p.CalcId == calcId);

                    if (entities != null && entities.Any())
                    {
                        ret = entities.First().Status;
                    }

                    return ret;
                }

                bool CheckCalcStatusSelf(CalcStatus? calcStatus)
                {
                    string computerName = TempIdentity.ComputerName.ToUpper();
                    DateTime dtCheck = DateTime.Now.AddMinutes(-5);

                    if(calcStatus != null)
                    {
                        var entities = _tenantDataContext.CalcStatus.Where(p =>
                        p.HpId == calcStatus.HpId &&
                        p.PtId == calcStatus.PtId &&
                        p.SinDate >= calcStatus.SinDate / 100 * 100 + 1 &&
                        p.SinDate <= calcStatus.SinDate / 100 * 100 + 31 &&
                        (p.Status == 1) &&
                        p.CalcId != calcStatus.CalcId &&
                        p.CreateMachine != null && p.CreateMachine.ToUpper() == computerName &&
                        p.UpdateDate >= dtCheck);

                        return (entities != null && entities.Any());
                    }

                    return false;
                }

                /// <summary>
                /// 指定の計算要求が他端末で処理中ではないかチェック
                /// </summary>
                /// <param name="calcStatus"></param>
                /// <returns></returns>
                bool CheckCalcStatusOther(CalcStatus? calcStatus)
                {
                    string computerName = TempIdentity.ComputerName.ToUpper();
                    DateTime dtCheck = DateTime.Now.AddMinutes(-5);

                    if(calcStatus != null)
                    {
                        var entities = _tenantDataContext.CalcStatus.Where(p =>
                        p.HpId == calcStatus.HpId &&
                        p.PtId == calcStatus.PtId &&
                        p.SinDate >= calcStatus.SinDate / 100 * 100 + 1 &&
                        p.SinDate <= calcStatus.SinDate / 100 * 100 + 31 &&
                        (p.Status == 1) &&
                        p.CalcId != calcStatus.CalcId &&
                        p.CreateMachine != null && p.CreateMachine.ToUpper() != computerName &&
                        p.UpdateDate >= dtCheck);
                        return (entities != null && entities.Any());
                    }
                    return false;
                    
                }

                bool CheckCalcStatus(CalcStatus? calcStatus)
                {
                    string computerName = TempIdentity.ComputerName.ToUpper();
                    DateTime dtCheck = DateTime.Now.AddMinutes(-5);

                    if(calcStatus != null)
                    {
                        var entities = _tenantDataContext.CalcStatus.Where(p =>
                        p.HpId == calcStatus.HpId &&
                        p.PtId == calcStatus.PtId &&
                        p.SinDate >= calcStatus.SinDate / 100 * 100 + 1 &&
                        p.SinDate <= calcStatus.SinDate / 100 * 100 + 31 &&
                        //(p.Status == 0 || p.Status == 1) &&
                        (p.Status == 1) &&
                        p.CalcId != calcStatus.CalcId &&
                        p.UpdateDate >= dtCheck);

                        return (entities != null && entities.Any());
                    }
                    return false;
                }

                List<CalcStatus> GetCalcStatusies(List<long> calcIds, string preFix)
                {
                    List<CalcStatus> calcStatusies = new List<CalcStatus>();
                    string computerName = (preFix + TempIdentity.ComputerName).ToUpper();

                    if (calcIds != null && calcIds.Any())
                    {
                        var entities = _tenantDataContext.CalcStatus.Where(p =>
                            p.CreateMachine == computerName &&
                            calcIds.Contains(p.CalcId) &&
                            !(new int[] { 8, 9 }.Contains(p.Status)))
                            .OrderBy(p => p.CalcId)
                            .ToList();

                        calcStatusies = entities;
                    }
                    return calcStatusies;
                }

                bool UpdateCalcStatus(List<CalcStatus> calcStatusies)
                {
                    foreach (CalcStatus calcStatus in calcStatusies)
                    {
                        calcStatus.UpdateDate = DateTime.Now;
                        calcStatus.UpdateId = TempIdentity.UserId;
                        calcStatus.UpdateMachine = TempIdentity.ComputerName;
                    }
                    _tenantDataContext.SaveChanges();
                    return true;
                }

                List<CalcStatus> GetSameCalcStatusInputModel(CalcStatus? calcStatus, string preFix)
                {
                    string computerName = (preFix + TempIdentity.ComputerName).ToUpper();

                    if(calcStatus!=null)
                    {
                        var entities = _tenantDataContext.CalcStatus.Where(p =>
                        p.CreateMachine == computerName &&
                        p.HpId == calcStatus.HpId &&
                        p.PtId == calcStatus.PtId &&
                        p.SinDate == calcStatus.SinDate &&
                        p.Status == 0).ToList();

                        return entities;
                    }
                    return new List<CalcStatus>();
                }

                List<CalcStatus> GetSameCalcStatus(long CalcId, string preFix)
                {
                    string computerName = (preFix + TempIdentity.ComputerName).ToUpper();

                    var entities = _tenantDataContext.CalcStatus.Where(p =>
                        p.CreateMachine == computerName &&
                        p.CalcId == CalcId).ToList();

                    return entities;
                }

                void AddCalcStatusC(int hpId, long ptId, int sinDate, int seikyuUp, string preFix)
                {
                    List<CalcStatus> calcStatusies = new List<CalcStatus>();

                    CalcStatus calcStatus;
                    // 患者ID、診療日の指定がある場合は当月分追加
                    if (hpId > 0 && ptId > 0 && sinDate > 0)
                    {
                        calcStatus = new CalcStatus();
                        calcStatus.HpId = hpId;
                        calcStatus.PtId = ptId;
                        calcStatus.SinDate = sinDate;
                        calcStatus.SeikyuUp = seikyuUp;
                        calcStatus.CalcMode = CalcModeConst.Normal;
                        calcStatusies.Add(calcStatus);

                        List<RaiinDayModel> raiinDays = FindRaiinInfDays(hpId, ptId, sinDate);
                        foreach (RaiinDayModel raiinDay in raiinDays)
                        {
                            calcStatus = new CalcStatus();
                            calcStatus.HpId = raiinDay.HpId;
                            calcStatus.PtId = raiinDay.PtId;
                            calcStatus.SinDate = raiinDay.SinDate;
                            calcStatus.CalcMode = CalcModeConst.Continuity;
                            calcStatusies.Add(calcStatus);
                        }
                    }
                    AddCalcStatus(calcStatusies, preFix);
                }

                bool GetCalcStatus(int hpId, long ptId, int sinDate, ref CalcStatus? calcStatus, string preFix)
                {
                    bool ret = false;

                    string computerName = (preFix + TempIdentity.ComputerName).ToUpper();

                    if (hpId >= 0 && ptId >= 0)
                    {
                        var entity = _tenantDataContext.CalcStatus.Where(p =>
                            p.CreateMachine == computerName &&
                            p.HpId == hpId &&
                            p.PtId == ptId &&
                            p.SinDate == sinDate &&
                            p.CalcMode == 0 &&
                            p.Status == 0)
                            .OrderBy(p => p.CalcId)
                            .FirstOrDefault();

                        if (entity != null)
                        {
                            calcStatus = entity;
                        }
                    }

                    if (calcStatus == null)
                    {
                        var entity3 = _tenantDataContext.CalcStatus.Where(p =>
                        p.HpId == hpId &&
                        p.CreateMachine == computerName &&
                        p.Status == 0)
                        .OrderBy(p => p.CalcMode)
                        .ThenByDescending(p => p.SeikyuUp)
                        .ThenBy(p => p.CalcId)
                        .FirstOrDefault();
                        if (entity3 != null)
                        {
                            calcStatus = entity3;
                        }
                    }
                    if (calcStatus != null)
                    {
                        ret = true;
                    }
                    return ret;
                }

                List<RaiinDayModel> FindRaiinInfDays(int hpId, long ptId, int sinDate)
                {
                    var raiinInfs = _tenantDataContext.RaiinInfs.Where(p =>
                        p.HpId == hpId &&
                        p.PtId == ptId &&
                        p.SinDate >= sinDate / 100 * 100 + 1 &&
                        p.SinDate <= sinDate / 100 * 100 + 31 &&
                        p.Status >= RaiinState.Calculate &&
                        p.IsDeleted == DeleteTypes.None);

                    var joinQuery = (
                        from raiinInf in raiinInfs
                        where
                            raiinInf.HpId == hpId &&
                            raiinInf.PtId == ptId &&
                            raiinInf.SinDate >= sinDate / 100 * 100 + 1 &&
                            raiinInf.SinDate <= sinDate / 100 * 100 + 31 &&
                            raiinInf.IsDeleted == DeleteTypes.None
                        group raiinInf by
                            new { HpId = raiinInf.HpId, PtId = raiinInf.PtId, SinDate = raiinInf.SinDate } into A
                        orderby
                            A.Key.HpId, A.Key.PtId, A.Key.SinDate
                        select new
                        {
                            A
                        }
                    );
                    return
                        joinQuery.AsEnumerable().Select(
                            data =>
                                new RaiinDayModel(data.A.Key.HpId, data.A.Key.PtId, data.A.Key.SinDate)
                            )
                            .ToList();
                }

                long AddCalcStatus(List<CalcStatus> CalcStatusies, string preFix)
                {
                    long calcId = 0;

                    CalcStatusies.ForEach(p =>
                    {
                        p.CreateDate = DateTime.Now;
                        p.CreateId = TempIdentity.UserId;
                        p.CreateMachine = (preFix + TempIdentity.ComputerName).ToUpper();
                    });
                    _tenantDataContext.CalcStatus.AddRange(CalcStatusies);
                    _tenantDataContext.SaveChanges();
                    return calcId;
                }

                //来院情報取得
                /// <summary>
                /// 請求年月に属する当該患者の全来院日を取得する
                /// </summary>
                /// <param name="hpId">医療機関識別ID</param>
                /// <param name="seikyuYm">請求年月</param>
                /// <param name="ptIds">患者ID</param>
                /// <returns>
                /// 指定の請求年月に属する月の来院日情報
                /// </returns>
                List<RaiinDayModel> FindRaiinInfDaysInMonth(int hpId, int seikyuYm, List<long> ptIds)
                {
                    int fromSinDate = seikyuYm * 100 + 1;
                    int toSinDate = seikyuYm * 100 + 99;

                    var receSeikyus = _tenantDataContext.ReceSeikyus.Where(r => r.IsDeleted == DeleteStatus.None);

                    var maxReceSeikyus = _tenantDataContext.ReceSeikyus.Where(
                        r => r.IsDeleted == DeleteStatus.None
                    ).GroupBy(
                        r => new { r.HpId, r.SinYm, r.PtId, r.HokenId }
                    ).Select(
                        r => new
                        {
                            r.Key.HpId,
                            r.Key.SinYm,
                            r.Key.PtId,
                            r.Key.HokenId,
                            SeikyuYm = r.Max(x => x.SeikyuYm)
                        }
                    );

                    var raiinInfs = _tenantDataContext.RaiinInfs.AsQueryable();
                    if (ptIds?.Count >= 1)
                    {
                        raiinInfs = raiinInfs.Where(r => ptIds.Contains(r.PtId));
                    }

                    var joinQuery = (
                        from raiinInf in raiinInfs
                        join rs in receSeikyus on
                            new { raiinInf.HpId, raiinInf.PtId, SinYm = (int)Math.Floor((double)raiinInf.SinDate / 100) } equals
                            new { rs.HpId, rs.PtId, rs.SinYm } into rsJoin
                        from receSeikyu in rsJoin.DefaultIfEmpty()
                        where
                                    raiinInf.HpId == hpId &&
                                    raiinInf.Status >= RaiinState.Calculate &&
                                    raiinInf.IsDeleted == DeleteTypes.None &&
                                    (
                                        //当月分
                                        (raiinInf.SinDate >= fromSinDate && raiinInf.SinDate <= toSinDate) ||
                                        //月遅れ・返戻分
                                        (
                                            (
                                                from rs1 in maxReceSeikyus
                                                where
                                                    rs1.HpId == hpId &&
                                                    rs1.SeikyuYm == seikyuYm
                                                select rs1
                                            ).Any(
                                                r =>
                                                    r.HpId == raiinInf.HpId &&
                                                    r.PtId == raiinInf.PtId &&
                                                    r.SinYm == raiinInf.SinDate / 100
                                            )
                                        )
                                    )

                        group raiinInf by
                            new { raiinInf.HpId, raiinInf.PtId, raiinInf.SinDate } into A
                        orderby
                            A.Key.HpId, A.Key.PtId, A.Key.SinDate
                        select new
                        {
                            A
                        }

                    );

                    return
                        joinQuery.AsEnumerable().Select(
                            data =>
                                new RaiinDayModel(data.A.Key.HpId, data.A.Key.PtId, data.A.Key.SinDate)
                        ).ToList();
                }
            }

            void ReceFutanCalculateMain(int hpId,List<long> ptIds, int seikyuYm)
            {

                ClearCalculate(hpId, ptIds, seikyuYm);





                #region function support
                void ClearCalculate(long hpId, List<long> ptIds, int seikyuYm)
                {
                    //レセデータ初期化
                    if (ptIds?.Count >= 1)
                    {
                        _tenantDataContext.ReceInfs.RemoveRange(_tenantDataContext.ReceInfs.Where(p =>
                            p.HpId == hpId &&
                            ptIds.Contains(p.PtId) &&
                            p.SeikyuYm == seikyuYm
                        ));
                        _tenantDataContext.ReceInfPreEdits.RemoveRange(_tenantDataContext.ReceInfPreEdits.Where(p =>
                            p.HpId == hpId &&
                            ptIds.Contains(p.PtId) &&
                            p.SeikyuYm == seikyuYm
                        ));
                        _tenantDataContext.ReceFutanKbns.RemoveRange(_tenantDataContext.ReceFutanKbns.Where(p =>
                            p.HpId == hpId &&
                            ptIds.Contains(p.PtId) &&
                            p.SeikyuYm == seikyuYm
                        ));
                        _tenantDataContext.ReceInfJds.RemoveRange(_tenantDataContext.ReceInfJds.Where(p =>
                            p.HpId == hpId &&
                            ptIds.Contains(p.PtId) &&
                            p.SeikyuYm == seikyuYm
                        ));
                    }
                    else
                    {
                        _tenantDataContext.ReceInfs.RemoveRange(_tenantDataContext.ReceInfs.Where(p =>
                            p.HpId == hpId &&
                            p.SeikyuYm == seikyuYm
                        ));
                        _tenantDataContext.ReceInfPreEdits.RemoveRange(_tenantDataContext.ReceInfPreEdits.Where(p =>
                            p.HpId == hpId &&
                            p.SeikyuYm == seikyuYm
                        ));
                        _tenantDataContext.ReceFutanKbns.RemoveRange(_tenantDataContext.ReceFutanKbns.Where(p =>
                            p.HpId == hpId &&
                            p.SeikyuYm == seikyuYm
                        ));
                        _tenantDataContext.ReceInfJds.RemoveRange(_tenantDataContext.ReceInfJds.Where(p =>
                            p.HpId == hpId &&
                            p.SeikyuYm == seikyuYm
                        ));
                    }
                }
                #endregion
            }
        }
    }
}
