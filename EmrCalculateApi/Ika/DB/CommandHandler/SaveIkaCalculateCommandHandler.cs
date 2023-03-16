using EmrCalculateApi.Constants;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using PostgreDataContext;
using Helper.Constants;
using EmrCalculateApi.Ika.Models;
using Helper.Common;
using EmrCalculateApi.Ika.Constants;
using EmrCalculateApi.Ika.ViewModels;
using Infrastructure.CommonDB;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmrCalculateApi.Ika.DB.CommandHandler
{
    public class SaveIkaCalculateCommandHandler 
    {
        private readonly string ModuleName = ModuleNameConst.EmrCalculateIka;
        private readonly TenantDataContext _tenantDataContext;
        private readonly IEmrLogger _emrLogger;
        private readonly ITenantProvider _tenantProvider;
        public SaveIkaCalculateCommandHandler(ITenantProvider tenantProvider, TenantDataContext tenantDataContext, IEmrLogger emrLogger)
        {
            _tenantProvider = tenantProvider;
            _tenantDataContext = tenantDataContext;
            _emrLogger = emrLogger;
        }

        public long AddCalcStatus(List<CalcStatusModel> calcStatusModels, string preFix)
        {
            const string conFncName = nameof(AddCalcStatus);

            long calcId = 0;

            try
            {
                List<CalcStatus> CalcStatusies = calcStatusModels.Select(p => p.CalcStatus).ToList();
                CalcStatusies.ForEach(p =>
                {
                    p.CreateDate = CIUtil.GetJapanDateTimeNow();
                    p.CreateId = Hardcode.UserID;
                    p.CreateMachine = (preFix + Hardcode.ComputerName).ToUpper();

                }
                );
                _tenantDataContext.CalcStatus.AddRange(CalcStatusies);
                //calcId = calcStatusModel.CalcId;

                _tenantDataContext.SaveChanges();
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError( this, conFncName, E);
            }
            return calcId;
        }

        public void AddWrkTalbes
            (TenantDataContext tenantDataContext,
            List<WrkSinRpInfModel> wrkSinRpInfModels, 
             List<WrkSinKouiModel> wrkSinKouiModels, 
             List<WrkSinKouiDetailModel> wrkSinKouiDetailModels, 
             List<WrkSinKouiDetailDelModel> wrkSinKouiDetailDelModels
             )
        {
            const string conFncName = nameof(AddWrkTalbes);
            try
            {

                //wrkSinRpInfModels.ForEach(p =>
                //    {
                //        p.CreateDate = CIUtil.GetJapanDateTimeNow();
                //        p.CreateId = 1; // todo get user from session
                //        p.CreateMachine = Hardcode.ComputerName;
                //        _tenantDataContext.WrkSinRpInfs.Add(p.WrkSinRpInf);
                //    }
                //);
                List<WrkSinRpInf> wrkSinRps = wrkSinRpInfModels.Select(p => p.WrkSinRpInf).ToList();
                wrkSinRps.ForEach(p =>
                    {
                        p.CreateDate = CIUtil.GetJapanDateTimeNow();
                        p.CreateId = Hardcode.UserID;
                        p.CreateMachine = Hardcode.ComputerName;

                    }
                );
                tenantDataContext.WrkSinRpInfs.AddRange(wrkSinRps);

                //wrkSinKouiModels.ForEach(p =>
                //{
                //    p.CreateDate = CIUtil.GetJapanDateTimeNow();
                //    p.CreateId = 1; // todo get user from session
                //    p.CreateMachine = Hardcode.ComputerName;
                //    _tenantDataContext.WrkSinKouis.Add(p.WrkSinKoui);
                //}
                //);

                List<WrkSinKoui> wrkSinKouis = wrkSinKouiModels.Select(p => p.WrkSinKoui).ToList();
                wrkSinKouis.ForEach(p =>
                {
                    p.CreateDate = CIUtil.GetJapanDateTimeNow();
                    p.CreateId = Hardcode.UserID;
                p.CreateMachine = Hardcode.ComputerName;

                }
                );
                tenantDataContext.WrkSinKouis.AddRange(wrkSinKouis);

                //wrkSinKouiDetailModels.ForEach(p =>
                //{
                //    _tenantDataContext.WrkSinKouiDetails.Add(p.WrkSinKouiDetail);
                //}
                //);

                List<WrkSinKouiDetail> wrkSinDtls = wrkSinKouiDetailModels.Select(p => p.WrkSinKouiDetail).ToList();
                wrkSinDtls?.ForEach(p =>
                    {
                        if(string.IsNullOrEmpty(p.TyuCd)==false && p.TyuCd.Length >= 5 && p.TyuCd.EndsWith("D"))
                        {
                            p.TyuCd = p.TyuCd.Substring(0, p.TyuCd.Length - 1);
                        }
                    }
                );
                tenantDataContext.WrkSinKouiDetails.AddRange(wrkSinDtls);

                List<WrkSinKouiDetailDel> wrkSinDtlDels = wrkSinKouiDetailDelModels.Select(p => p.WrkSinKouiDetailDel).ToList();
                tenantDataContext.WrkSinKouiDetailDels.AddRange(wrkSinDtlDels);

            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError( this, conFncName, E);
            }
        }

        public void AddCalcLog(TenantDataContext newDbContext, List<CalcLogModel> calcLogModels)
        {
            const string conFncName = nameof(AddCalcLog);
            try
            {
                if (calcLogModels.Any())
                {
                    string MachinName = Hardcode.ComputerName;
                    //calcLogModels.ForEach(p =>
                    //    {
                    //        p.CreateDate = CIUtil.GetJapanDateTimeNow();
                    //        p.CreateId = 1; // todo get user from session
                    //        p.CreateMachine = Hardcode.ComputerName;
                    //        _tenantDataContext.CalcLogs.Add(p.CalcLog);
                    //    }
                    //);
                    List<CalcLog> calcLogs = calcLogModels.Select(p => p.CalcLog).ToList();
                    calcLogs.ForEach(p =>
                    {
                        p.CreateDate = CIUtil.GetJapanDateTimeNow();
                        p.CreateId = Hardcode.UserID;
                        p.CreateMachine = MachinName;
                        p.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        p.UpdateId = Hardcode.UserID;
                        p.UpdateMachine = MachinName;
                    }
                    );
                    newDbContext.CalcLogs.AddRange(calcLogs);
                }
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError( this, conFncName, E);
            }
        }

        public void AddCalcLog(List<CalcLogModel> calcLogModels)
        {
            const string conFncName = nameof(AddCalcLog);
            try
            {
                AddCalcLog(_tenantDataContext, calcLogModels);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError( this, conFncName, E);
            }
        }

        public void AddErrorCalcLog(List<CalcLogModel> calcLogModels)
        {
            const string conFncName = nameof(AddCalcLog);
            using (var newDbService = _tenantProvider.GetTrackingTenantDataContext())
            {
                using (var transaction = _tenantDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        AddCalcLog(newDbService, calcLogModels);
                        transaction.Commit();
                    }
                    catch (Exception E)
                    {
                        transaction.Rollback();
                        _emrLogger.WriteLogError(this, conFncName, E);
                    }
                }
            }
        }

        public void AddSin(
         TenantDataContext newDbContext,
         List<SinRpInfModel> sinRpInfModels,
         List<SinKouiModel> sinKouiModels,
         List<SinKouiDetailModel> sinKouiDetailModels,
         List<SinKouiCountModel> sinKouiCountModels)
        {
            string MachineName = Hardcode.ComputerName;

            List<SinRpInfModel> addSinRpInfs = new List<SinRpInfModel>();

            foreach (SinRpInfModel sinRpInf in sinRpInfModels.FindAll(q => q.UpdateState == UpdateStateConst.Add))
            {
                addSinRpInfs.Add(
                    new SinRpInfModel(
                        new SinRpInf
                        {
                            HpId = sinRpInf.HpId,
                            PtId = sinRpInf.PtId,
                            SinYm = sinRpInf.SinYm,

                            FirstDay = sinRpInf.FirstDay,
                            HokenKbn = sinRpInf.HokenKbn,
                            SinKouiKbn = sinRpInf.SinKouiKbn,
                            SinId = sinRpInf.SinId,
                            CdNo = sinRpInf.CdNo,
                            SanteiKbn = sinRpInf.SanteiKbn,
                            KouiData = sinRpInf.KouiData,
                            IsDeleted = sinRpInf.IsDeleted,
                            CreateDate = CIUtil.GetJapanDateTimeNow(),
                            CreateId = Hardcode.UserID,
                            CreateMachine = MachineName,
                            UpdateDate = CIUtil.GetJapanDateTimeNow(),
                            UpdateId = Hardcode.UserID,
                            UpdateMachine = MachineName,
                        }
                        )
                    {
                        UpdateState = sinRpInf.UpdateState,
                        KeyNo = sinRpInf.KeyNo
                    });
                //sinRpInf.CreateDate = CIUtil.GetJapanDateTimeNow();
                //sinRpInf.CreateId = Hardcode.UserID;
                //sinRpInf.CreateMachine = MachineName;
                //sinRpInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                //sinRpInf.UpdateId = Hardcode.UserID;
                //sinRpInf.UpdateMachine = MachineName;
            }
            //List<SinRpInf> sinRpInfs = sinRpInfModels.Where(p => p.UpdateState == UpdateStateConst.Add).Select(p => p.SinRpInf).ToList();
            List<SinRpInf> sinRpInfs = addSinRpInfs.Where(p => p.UpdateState == UpdateStateConst.Add).Select(p => p.SinRpInf).ToList();

            newDbContext.SinRpInfs.AddRange(sinRpInfs);
            newDbContext.SaveChanges();

            //foreach (SinRpInfModel sinRpInf in sinRpInfModels.FindAll(q => q.UpdateState == UpdateStateConst.Add))
            foreach (SinRpInfModel sinRpInf in addSinRpInfs.FindAll(q => q.UpdateState == UpdateStateConst.Add))
            {
                foreach (SinRpInfModel sinRpInfModel in sinRpInfModels.FindAll(q => q.KeyNo == sinRpInf.KeyNo))
                {
                    sinRpInfModel.RpNo = sinRpInf.SinRpInf.RpNo;
                }

                foreach (SinKouiModel sinKoui in sinKouiModels.FindAll(q => q.KeyNo == sinRpInf.KeyNo))
                {
                    sinKoui.RpNo = sinRpInf.SinRpInf.RpNo;
                    sinKoui.CreateDate = CIUtil.GetJapanDateTimeNow();
                    sinKoui.CreateId = Hardcode.UserID;
                    sinKoui.CreateMachine = MachineName;
                    sinKoui.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    sinKoui.UpdateId = Hardcode.UserID;
                    sinKoui.UpdateMachine = MachineName;
                }

                foreach (SinKouiDetailModel sinDtl in sinKouiDetailModels.FindAll(q => q.KeyNo == sinRpInf.KeyNo))
                {
                    sinDtl.RpNo = sinRpInf.SinRpInf.RpNo;
                }

                foreach (SinKouiCountModel sinCount in sinKouiCountModels.FindAll(q => q.KeyNo == sinRpInf.KeyNo))
                {
                    sinCount.RpNo = sinRpInf.SinRpInf.RpNo;
                    // 作成日/更新日等の情報は、SinRpのKeyNoに紐づかない追加データもあるので後で更新する
                }

                //foreach (SinRpNoInfModel sinRpNo in sinRpNoInfModels.FindAll(q => q.KeyNo == sinRpInf.KeyNo))
                //{
                //    sinRpNo.RpNo = sinRpInf.SinRpInf.RpNo;
                //    // 作成日/更新日等の情報は、SinRpのKeyNoに紐づかない追加データもあるので後で更新する
                //}
            };

            List<SinKoui> sinKouis = sinKouiModels.Where(p => p.UpdateState == UpdateStateConst.Add).Select(p => p.SinKoui).ToList();
            newDbContext.SinKouis.AddRange(sinKouis);

            List<SinKouiDetail> sinDtls = sinKouiDetailModels.Where(p => p.UpdateState == UpdateStateConst.Add).Select(p => p.SinKouiDetail).ToList();
            newDbContext.SinKouiDetails.AddRange(sinDtls);

            List<SinKouiCount> sinKouiCounts = sinKouiCountModels.Where(p => p.UpdateState == UpdateStateConst.Add).Select(p => p.SinKouiCount).ToList();
            foreach(SinKouiCount sinKouiCount in sinKouiCounts)
            {
                sinKouiCount.CreateDate = CIUtil.GetJapanDateTimeNow();
                sinKouiCount.CreateId = Hardcode.UserID;
                sinKouiCount.CreateMachine = MachineName;
                sinKouiCount.UpdateDate = CIUtil.GetJapanDateTimeNow();
                sinKouiCount.UpdateId = Hardcode.UserID;
                sinKouiCount.UpdateMachine = MachineName;
            }
            newDbContext.SinKouiCounts.AddRange(sinKouiCounts);

            //List<SinRpNoInf> sinRpNoInfs = sinRpNoInfModels.Where(p => p.UpdateState == UpdateStateConst.Add).Select(p => p.SinRpNoInf).ToList();
            //foreach(SinRpNoInf sinRpNoInf in sinRpNoInfs)
            //{
            //    sinRpNoInf.CreateDate = CIUtil.GetJapanDateTimeNow();
            //    sinRpNoInf.CreateId = Session.UserID;
            //    sinRpNoInf.CreateMachine = MachineName;
            //    sinRpNoInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            //    sinRpNoInf.UpdateId = Session.UserID;
            //    sinRpNoInf.UpdateMachine = MachineName;
            //}
            //newDbContext.SinRpNoInfs.AddRange(sinRpNoInfs);

            try
            {
                newDbContext.SaveChanges();
            }
            //catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            catch(Exception ex)
            {
                //foreach (var errors in ex.EntityValidationErrors)
                //{
                //    foreach (var error in errors.ValidationErrors)
                //    {
                //        // VisualStudioの出力に表示
                //        System.Diagnostics.Trace.WriteLine(error.ErrorMessage);
                //    }
                //}
                throw;
            }
        }

        public void DelSin(
         TenantDataContext newDbContext,
         List<SinRpInfModel> sinRpInfModels,
         List<SinKouiModel> sinKouiModels,
         List<SinKouiDetailModel> sinKouiDetailModels,
         List<SinKouiCountModel> sinKouiCountModels,
         List<SinRpNoInfModel> sinRpNoInfModels)
        {
            //string MachineName = Hardcode.ComputerName;
                        
            var delSinRpInf = sinRpInfModels.FindAll(p => p.IsDeleted == 1).ToList();
            delSinRpInf?.ForEach(p =>
                newDbContext.SinRpInfs.Remove(p.SinRpInf)
            );

            var delSinKouis = sinKouiModels.FindAll(p => p.IsDeleted == 1).ToList();
            delSinKouis?.ForEach(p =>
                newDbContext.SinKouis.Remove(p.SinKoui)
            );

            var delSinKouiDtls = sinKouiDetailModels.FindAll(p => p.IsDeleted == 1).ToList();
            delSinKouiDtls?.ForEach(p =>
                newDbContext.SinKouiDetails.Remove(p.SinKouiDetail)
            );

        }

        public void UpdateData(IkaCalculateCommonDataViewModel _common)
        {
            string conFncName = nameof(UpdateData);

            _emrLogger.WriteLogStart( this, conFncName, "");

            // 先に更新/削除分を反映
            _tenantDataContext.SaveChanges();

            using (var new_tenantDataContext = _tenantProvider.CreateNewTrackingDataContext())
            {
                var executionStrategy = new_tenantDataContext.Database.CreateExecutionStrategy();
                executionStrategy.Execute(
                    () =>
                    {
                        using (var transaction = new_tenantDataContext.Database.BeginTransaction())
                        {
                            try
                            {
                                //_emrLogger.WriteLogMsg( this, conFncName, "AddWrkTalbes");
                                //if (ICDebugConf.SaveWrk)
                                //{
                                //    AddWrkTalbes
                                //        (newDbService, _common.Wrk.wrkSinRpInfs, _common.Wrk.wrkSinKouis, _common.Wrk.wrkSinKouiDetails, _common.Wrk.wrkSinKouiDetailDels);
                                //}
                                //_emrLogger.WriteLogMsg( this, conFncName, "AddCalcLog");  
                                AddCalcLog(new_tenantDataContext, _common.calcLogs);
                                //new_tenantDataContext.SaveChanged();
                                //_emrLogger.WriteLogMsg( this, conFncName, "AddSin");
                                AddSin(
                                    new_tenantDataContext, _common.Sin.SinRpInfs, _common.Sin.SinKouis, _common.Sin.SinKouiDetails, _common.Sin.SinKouiCounts);

                                //DelSin(
                                //    newDbService, _common.Sin.SinRpInfs, _common.Sin.SinKouis, _common.Sin.SinKouiDetails, _common.Sin.SinKouiCounts, _common.Sin.SinRpNoInfs);

                                new_tenantDataContext.SaveChanges();

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                _emrLogger.WriteLogError(this, nameof(UpdateData), e);
                                throw;
                            }
                        }
                    });
            }
            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        public void UpdateCalcStatus(CalcStatusModel calcStatus)
        {
            calcStatus.UpdateDate = CIUtil.GetJapanDateTimeNow();
            calcStatus.UpdateId = Hardcode.UserID;
            calcStatus.UpdateMachine = Hardcode.ComputerName;
            Console.WriteLine("Start UpdateCalcStatus One");
            _tenantDataContext.SaveChanges();
            Console.WriteLine("End UpdateCalcStatus One");
        }

        public bool UpdateCalcStatus(List<CalcStatusModel> calcStatusies)
        {
            const string conFncName = nameof(UpdateCalcStatus);

            bool ret = true;
            try
            {
                foreach (CalcStatusModel calcStatus in calcStatusies)
                {
                    calcStatus.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    calcStatus.UpdateId = Hardcode.UserID;
                    calcStatus.UpdateMachine = Hardcode.ComputerName;
                }
                Console.WriteLine("Start UpdateCalcStatus list");
                _tenantDataContext.SaveChanges();
                Console.WriteLine("End UpdateCalcStatus list");
            }
            catch (Exception e)
            {
                ret = false;
                _emrLogger.WriteLogError( this, conFncName, e);
            }

            return ret;
        }
        public void UpdateCalcStatusError(List<CalcStatusModel> calcStatusies)
        {
            foreach (CalcStatusModel calcStatus in calcStatusies)
            {
                calcStatus.UpdateDate = CIUtil.GetJapanDateTimeNow();
                calcStatus.UpdateId = Hardcode.UserID;
                calcStatus.UpdateMachine = Hardcode.ComputerName;
            }

            _tenantDataContext.SaveChanges();
        }

        /// <summary>
        /// レセプト状態情報の状態区分を0に戻す
        /// </summary>
        /// <param name="receStatusies"></param>
        public void UpdateReceStatus(List<ReceStatusModel> receStatusies)
        {
            foreach (ReceStatusModel receStatus in receStatusies)
            {
                receStatus.StatusKbn = 0;
                receStatus.UpdateDate = CIUtil.GetJapanDateTimeNow();
                receStatus.UpdateId = Hardcode.UserID;
                receStatus.UpdateMachine = Hardcode.ComputerName;
            }

            _tenantDataContext.SaveChanges();
        }
    }
}
