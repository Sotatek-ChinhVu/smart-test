using Entity.Tenant;
using CalculateService.Ika.DB.Finder;
using CalculateService.Ika.DB.CommandHandler;
using CalculateService.Ika.Models;
using CalculateService.Ika.Constants;
using Helper.Constants;
using CalculateService.Utils;
using Infrastructure.Interfaces;
using Domain.Constant;
using CalculateService.Interface;
using CalculateService.Constants;

namespace CalculateService.Ika.ViewModels
{
    class IkaCalculateOdrToWrkJihiViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkJihiViewModel(IkaCalculateCommonDataViewModel common,
            ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _common = common;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// 計算ロジック
        /// </summary>
        /// <param name="hpId">HospitalID</param>
        public void Calculate(int hpId)
        {
            const string conFncName = nameof(Calculate);
            _emrLogger.WriteLogStart(this, conFncName, "");

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.Jihi, OdrKouiKbnConst.Jihi))
            {
                // 保険
                CalculateHoken(hpId);
            }

            _common.Wrk.CommitWrkSinRpInf();

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }

        /// <summary>
        /// 保険分を処理する
        /// </summary>
        /// <param name="hpId">HospitalID</param>
        private void CalculateHoken(int hpId)
        {
            const string conFncName = nameof(CalculateHoken);

            // 通常算定処理
            List<OdrInfModel> filteredOdrInf;
            List<OdrDtlTenModel> filteredOdrDtl;

            // 自費のRpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.Jihi, OdrKouiKbnConst.Jihi);

            if (filteredOdrInf.Any())
            {
                foreach (OdrInfModel odrInf in filteredOdrInf)
                {
                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any())
                    {
                        bool firstSinryoKoui = true;

                        // 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Jihi, ReceSinId.Jihi, 2);

                        // 集計先は、後で内容により変更する
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Jihi, isNodspRece: 1, cdKbn: "JS");

                        foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                        {
                            if (!(_common.IsCommentItemCd(odrDtl.ItemCd)))
                            {
                                if (firstSinryoKoui)
                                {
                                    firstSinryoKoui = false;
                                }
                                else
                                {
                                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Jihi, ReceSinId.Jihi, 2);
                                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Jihi, isNodspRece: 1, cdKbn: "JS");
                                }
                            }
                            _common.Wrk.AppendNewWrkSinKouiDetail(hpId, odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                            if (odrDtl.JihiSbt > 0 && _common.Wrk.wrkSinKouis.Last().JihiSbt <= 0)
                            {
                                _common.Wrk.wrkSinKouis.Last().JihiSbt = odrDtl.JihiSbt;
                            }
                            if (odrDtl.KazeiKbn > 0 && _common.Wrk.wrkSinKouis.Last().KazeiKbn <= 0)
                            {
                                _common.Wrk.wrkSinKouis.Last().KazeiKbn = odrDtl.KazeiKbn;
                            }
                        }
                    }
                }

                _common.Wrk.CommitWrkSinRpInf();
            }
        }
    }
}
