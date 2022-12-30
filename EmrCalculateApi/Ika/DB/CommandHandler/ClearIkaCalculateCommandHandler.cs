using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.ViewModels;
using PostgreDataContext;
using EmrCalculateApi.Interface;
using Helper.Constants;
using EmrCalculateApi.Extensions;

namespace EmrCalculateApi.Ika.DB.CommandHandler
{
    public class ClearIkaCalculateCommandHandler
    {
        private readonly string _moduleName = ModuleNameConst.EmrCalculateIka;
        private readonly TenantDataContext _tenantDataContext;
        private readonly IEmrLogger _emrLogger;
        public ClearIkaCalculateCommandHandler(TenantDataContext tenantDataContext, IEmrLogger emrLogger)
        {
            _tenantDataContext = tenantDataContext;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// ワークテーブルの削除
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者番号</param>
        /// <param name="sinDate">診療日</param>
        public void ClearWrkData(int hpId, long ptId, int sinDate)
        {
            const string conFncName = nameof(ClearWrkData);
            try
            {
                //if (ICDebugConf.SaveWrk)
                //{
                //    //ワーク診療行為Rp情報
                //    _tenantDataContext.WrkSinRpInfs.RemoveRange(p =>
                //        p.HpId == hpId &&
                //        p.PtId == ptId &&
                //        p.SinDate == sinDate
                //    );
                //    //ワーク診療行為情報
                //    _tenantDataContext.WrkSinKouis.RemoveRange(p =>
                //        p.HpId == hpId &&
                //        p.PtId == ptId &&
                //        p.SinDate == sinDate
                //    );
                //    //ワーク診療行為詳細情報
                //    _tenantDataContext.WrkSinKouiDetails.RemoveRange(p =>
                //        p.HpId == hpId &&
                //        p.PtId == ptId &&
                //        p.SinDate == sinDate
                //    );
                //    //ワーク診療行為詳細削除情報
                //    _tenantDataContext.WrkSinKouiDetailDelRepository.RemoveRange(p =>
                //        p.HpId == hpId &&
                //        p.PtId == ptId &&
                //        p.SinDate == sinDate
                //    );
                //}
                ////算定ログ
                //_tenantDataContext.CalcLogs.RemoveRange(p =>
                //    p.HpId == hpId &&
                //    p.PtId == ptId &&
                //    p.SinDate == sinDate
                //);

                ////診療行為RP情報
                //_tenantDataContext.SinRpNoInfs.RemoveRange(p =>
                //    p.HpId == hpId &&
                //    p.PtId == ptId &&
                //    p.SinYm == sinDate / 100 &&
                //    p.SinDay == sinDate % 100
                //);

                ////診療行為回数情報
                //_tenantDataContext.SinKouiCounts.RemoveRange(p =>
                //    p.HpId == hpId &&
                //    p.PtId == ptId &&
                //    p.SinYm == sinDate / 100 &&
                //    p.SinDay == sinDate % 100
                //);
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
                throw;
            }
        }

        /// <summary>
        /// 算定ログの削除
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者番号</param>
        /// <param name="sinDate">診療日</param>
        public void ClearCalcLog(int hpId, long ptId, int sinDate)
        {
            const string conFncName = nameof(ClearCalcLog);
            try
            {
                //算定ログ
                _tenantDataContext.CalcLogs.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate == sinDate
                );
            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
                throw;
            }
        }

        /// <summary>
        /// 算定情報の削除
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者番号</param>
        /// <param name="sinDate">診療日</param>
        public void ClearSanteiInf
            (int hpId, long ptId, int sinDate)
        {            
            const string conFncName = nameof(ClearSanteiInf);

            int sinYm = sinDate / 100;
            int sinDay = sinDate % 100;

            try
            {
                //診療Rp番号情報
                _tenantDataContext.SinRpNoInfs.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.SinDay == sinDay
                );

                //診療行為回数情報
                _tenantDataContext.SinKouiCounts.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.SinDay == sinDay
                );

                //// 診療Rp情報
                //foreach (SinRpInfModel sinRpInf in delSinRpInfs)
                //{
                //    _tenantDataContext.SinRpInfs.RemoveRange(p =>
                //        p.HpId == sinRpInf.HpId &&
                //        p.PtId == sinRpInf.PtId &&
                //        p.SinYm == sinRpInf.SinYm &&
                //        p.RpNo == sinRpInf.RpNo
                //    );
                //}

                ////診療行為
                //foreach (SinKouiModel sinKoui in delSinKouis)
                //{
                //    _tenantDataContext.SinKouis.RemoveRange(p =>
                //        p.HpId == sinKoui.HpId &&
                //        p.PtId == sinKoui.PtId &&
                //        p.SinYm == sinKoui.SinYm &&
                //        p.RpNo == sinKoui.RpNo &&
                //        p.SeqNo == sinKoui.SeqNo
                //    );
                //}

                ////診療行為詳細
                //foreach (SinKouiDetailModel sinKouiDetail in delSinKouiDetails)
                //{
                //    _tenantDataContext.SinKouiDetails.RemoveRange(p =>
                //        p.HpId == sinKouiDetail.HpId &&
                //        p.PtId == sinKouiDetail.PtId &&
                //        p.SinYm == sinKouiDetail.SinYm &&
                //        p.RpNo == sinKouiDetail.RpNo &&
                //        p.SeqNo == sinKouiDetail.SeqNo
                //    );
                //}

                // 診療Rp情報
                _tenantDataContext.SinRpInfs.RemoveRange(p =>                    
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.IsDeleted == 1
                );

                //診療行為
                _tenantDataContext.SinKouis.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.IsDeleted == 1
                );

                //診療行為詳細
                _tenantDataContext.SinKouiDetails.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.IsDeleted == 1
                );

            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError(this, conFncName, E);
                throw;
            }
        }

        /// <summary>
        /// 診療データの削除
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者番号</param>
        /// <param name="sinDate">診療日</param>
        public void ClearSinData
            (int hpId, long ptId, int sinDate)
        {
            const string conFncName = nameof(ClearSinData);

            int sinYm = sinDate / 100;
            //int sinDay = sinDate % 100;

            try
            {

                // 診療Rp情報
                _tenantDataContext.SinRpInfs.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.IsDeleted == 1 
                );

                //診療行為
                _tenantDataContext.SinKouis.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.IsDeleted == 1
                );

                //診療行為詳細
                _tenantDataContext.SinKouiDetails.RemoveRange(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinYm == sinYm &&
                    p.IsDeleted == 1
                );

            }
            catch (Exception E)
            {
                _emrLogger.WriteLogError( this, conFncName, E);
                throw;
            }
        }
    }
}
