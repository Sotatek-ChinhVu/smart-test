using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using Helper.Extension;
using Domain.Constant;
using EmrCalculateApi.Constants;
using Helper.Common;

namespace EmrCalculateApi.Ika.ViewModels
{
    /// <summary>
    /// 読み替えを行うレコードの情報管理クラス
    /// </summary>
    class YomikaeRecInf
    {
        /// <summary>
        /// Rp番号
        /// </summary>
        public int _rpNo;
        /// <summary>
        /// 連番
        /// </summary>
        public int _seqNo;
        /// <summary>
        /// 行番号
        /// </summary>
        public int _rowNo;

        /// <summary>
        /// 読み替えを行うレコードの情報管理クラス生成
        /// </summary>
        /// <param name="rpNo">Rp番号</param>
        /// <param name="seqNo">連番</param>
        /// <param name="rowNo">行番号</param>
        public YomikaeRecInf(int rpNo, int seqNo, int rowNo)
        {
            _rpNo = rpNo;
            _seqNo = seqNo;
            _rowNo = rowNo;
        }

        /// <summary>
        /// Rp番号
        /// </summary>
        public int RpNo
        {
            get { return _rpNo; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public int SeqNo
        {
            get { return _seqNo; }
        }

        /// <summary>
        /// 行番号
        /// </summary>
        public int RowNo
        {
            get { return _rowNo; }
        }
    }

    /// <summary>
    /// 薬剤の逓減点数を記憶するためのクラス
    /// </summary>
    class YakuzaiTeigen
    {
        /// <summary>
        /// 向精神薬の逓減点数
        /// </summary>
        private double _kousei;
        /// <summary>
        /// 多剤投与の逓減点数
        /// </summary>
        private double _tazai;
        /// <summary>
        /// 向精神薬の逓減前点数
        /// </summary>
        private double _kouseiOrg;
        /// <summary>
        /// 多剤投与の逓減前点数
        /// </summary>
        private double _tazaiOrg;
        /// <summary>
        /// 薬剤の逓減点数を記憶するためのクラス生成
        /// </summary>
        public YakuzaiTeigen()
        {
            _kousei = 0;
            _tazai = 0;
            _kouseiOrg = 0;
            _tazaiOrg = 0;
        }

        /// <summary>
        /// 向精神薬の逓減に関係する項目の合計点数
        /// </summary>
        public double Kousei
        {
            get { return _kousei; }
            set { _kousei = value; }
        }

        /// <summary>
        /// 多剤投与の逓減に関係する項目の合計点数
        /// </summary>
        public double Tazai
        {
            get { return _tazai; }
            set { _tazai = value; }
        }

        /// <summary>
        /// 向精神薬逓減前点数
        /// </summary>
        public double KouseiOrg
        {
            get { return _kouseiOrg; }
            set { _kouseiOrg = value; }
        }

        /// <summary>
        /// 多剤投与逓減前点数
        /// </summary>
        public double TazaiOrg
        {
            get { return _tazaiOrg; }
            set { _tazaiOrg = value; }
        }

        /// <summary>
        /// 減点数をセット
        /// </summary>
        /// <param name="kouseiGentensu"></param>
        /// <param name="tazaiGentensu"></param>
        public void SetGentensu(double kouseiGentensu, double tazaiGentensu)
        {
            _kousei = kouseiGentensu;
            _tazai = tazaiGentensu;
        }
    }

    /// <summary>
    /// ワーク情報から診療情報に変換するクラス
    /// </summary>
    class IkaCalculateWrkToSinViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        /// <summary>
        /// 共通データ
        /// </summary>
        private IkaCalculateCommonDataViewModel _common;

        /// <summary>
        /// 内分泌検査の合計最大値
        /// </summary>
        private double NaibunpituMax = 0;
        /// <summary>
        /// IGE/HRTの合計最大値
        /// </summary>
        private double IgeHrtTenMax = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateWrkToSinViewModel(IkaCalculateCommonDataViewModel common)
        {
            _common = common;

            #region 複数種類の項目を合算して最大値がある項目の診療日時点の最大値を求める

            // 内分泌
            NaibunpituMax = GetNaibunpituMax();
            // IGE/HRT
            IgeHrtTenMax = GetIgeHrtTenMax();

            #endregion
        }

        /// <summary>
        /// 内分泌検査の診療日に応じた合計最大点数を返す
        /// </summary>
        double GetNaibunpituMax()
        {
            double ret = 3600;

            //if(_common.sinDate >= xxxx)
            //{
            //    ret = xxxx;
            //}

            return ret;
        }

        /// <summary>
        /// IgE, HRT検査の診療日に応じた合計最大点数を返す
        /// </summary>        
        double GetIgeHrtTenMax()
        {
            double ret = 1430;

            //if(_common.sinDate >= xxxx)
            //{
            //    ret = xxxx;
            //}

            return ret;
        }

        /// <summary>
        /// 点数計算処理
        /// </summary>
        public void Calculate()
        {
            // 読み替え加算対象レコード
            List<YomikaeRecInf> yomikaeDtl =
                new List<YomikaeRecInf>();

            // 自費算定分の合計金額
            double jihiSanteiTotal = 0;
            // 自費項目分の合計金額
            double jihiKoumokuTotal = 0;
            // 外税対象の合計金額
            double jihiSotozeiTotal = 0;
            // 軽減税率対象の合計金額
            double jihiKeigenzeiTotal = 0;
            // 内税対象の合計金額
            double jihiUchizeiTotal = 0;
            // 内税軽減税率対象の合計金額
            double jihiUchiKeigenzeiTotal = 0;

            //内分泌検査の合計
            double naibunpitu = -1;
            List<double> naibunpituTens = new List<double>
            {
                0, 0, 0
            };
            //IgeHrtの合計
            double igeHrtTen = 0;
            List<double> igeHrtTens = new List<double>
            {
                0, 0, 0
            };

            // 薬剤逓減情報
            YakuzaiTeigen yakuzaiTeigen = new YakuzaiTeigen();

            if (_common.IsRosai && _common.hokenKbn != HokenSyu.After && SyosaiConst.Saisin == _common.syosai)
            {
                // 労災で再診の場合、読み替え加算が可能かどうかチェックする
                yomikaeDtl = CheckRosaiGairaiKanriYomikae();
            }

            List<WrkSinRpInfModel> wrkSinRps = 
                _common.Wrk.wrkSinRpInfs.FindAll(p => 
                    p.RaiinNo == _common.raiinNo && 
                    p.HokenKbn == _common.hokenKbn &&
                    p.IsDeleted == 0);

            #region local method 領収証に表示する算定項目があるかチェックする関数
            bool _existDspMeisai()
            {
                bool ret = false;

                foreach (WrkSinRpInfModel wrkSinRp in wrkSinRps)
                {
                    foreach(WrkSinKouiModel wrkSinKoui in
                        _common.Wrk.wrkSinKouis.FindAll(p =>
                            p.RaiinNo == _common.raiinNo &&
                            p.RpNo == wrkSinRp.RpNo &&
                            p.IsDeleted == 0 &&
                            p.InoutKbn == 0 
                            ))
                    {
                        if(_common.Wrk.wrkSinKouiDetails.Any(p=>
                            p.RaiinNo == wrkSinKoui.RaiinNo &&
                            p.RpNo == wrkSinKoui.RpNo &&
                            p.SeqNo == wrkSinKoui.SeqNo &&
                            p.IsDeleted == DeleteStatus.None &&
                            p.IsNodspRyosyu == 0))
                        {
                            ret = true;
                            break;
                        }

                    }                  
                }

                return ret;
            }
            #endregion

            if (wrkSinRps.Any() == false)
            {
                // 算定項目がない場合
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, SanteiKbnConst.Santei);
                _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.Sonota, isNodspRece: 1);
                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.NoMeisai);

                wrkSinRps =
                _common.Wrk.wrkSinRpInfs.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    p.IsDeleted == 0);
            }
            else if(_existDspMeisai() == false)
            {
                // 領収証等に表示する算定項目がない場合（
                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Sonota, ReceSinId.Sonota, SanteiKbnConst.Santei);
                _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.Sonota, isNodspRece: 1);
                _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.NoMeisai);

                wrkSinRps =
                _common.Wrk.wrkSinRpInfs.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    p.IsDeleted == 0);
            }

            // 消費税項目の保険PID,ID
            int sotozeiHokenPid = 0;
            int sotozeiHokenId = 0;
            int sotogenHokenPid = 0;
            int sotogenHokenId = 0;
            int uchizeiHokenPid = 0;
            int uchizeiHokenId = 0;
            int uchigenHokenPid = 0;
            int uchigenHokenId = 0;

            foreach (WrkSinRpInfModel wrkSinRp in wrkSinRps)
            {
                // 詳細追加
                _common.Sin.AppendNewSinRpInf(wrkSinRp);

                // 行為抽出
                List<WrkSinKouiModel> wrkSinKouis = 
                    _common.Wrk.wrkSinKouis.FindAll(p => 
                        p.RaiinNo == _common.raiinNo && 
                        p.RpNo == wrkSinRp.RpNo && 
                        p.IsDeleted == 0);

                // 合計点数
                double totalTen = 0;
                // 円点区分
                int enTenKbn = 0;
                // 点数回数に追加するかどうか
                bool addTenCount = false;
                // 自費項目の金額
                double jihiKoumokuTmp = 0;
                // 自費内税分（戻り値受け用）
                double jihiUchizeiTmp = 0;
                // 自費外税分（戻り値受け用）
                double jihiSotozeiTmp = 0;
                // 自費軽減税率分（戻り値受け用）
                double jihiKeigenzeiTmp = 0;
                // 自費内税軽減税率分（戻り値受け用）
                double jihiUchiKeigenzeiTmp = 0;

                foreach (WrkSinKouiModel wrkSinKoui in wrkSinKouis)
                {
                    _common.Sin.AppendNewSinKoui(wrkSinKoui);

                    // 詳細計算
                    (totalTen, enTenKbn, addTenCount, jihiKoumokuTmp, jihiUchizeiTmp, jihiSotozeiTmp, jihiUchiKeigenzeiTmp, jihiKeigenzeiTmp) = 
                        CalcTen(wrkSinRp, wrkSinKoui, yomikaeDtl, ref yakuzaiTeigen, ref naibunpitu, ref igeHrtTen, ref naibunpituTens, ref igeHrtTens);

                    if (_common.Sin.FilterSinKouiDetail(_common.Sin.KeyNo, _common.Sin.SeqNo).Any() == false)
                    {
                        // 属する詳細がない場合、行為削除
                        _common.Sin.RemoveLastKoui();
                    }
                    else
                    {

                        if (_common.Sin.FilterSinKouiDetail(_common.Sin.KeyNo, _common.Sin.SeqNo).Any(p => p.IsNodspRece == 0) == false)
                        {
                            // すべての詳細がレセ非表示の場合
                            _common.Sin.SinKouis.Last().IsNodspRece = 1;
                        }

                        // 自費分の金額を記憶
                        jihiKoumokuTotal += jihiKoumokuTmp;

                        jihiSotozeiTotal += jihiSotozeiTmp;
                        jihiKeigenzeiTotal += jihiKeigenzeiTmp;
                        jihiUchizeiTotal += jihiUchizeiTmp;
                        jihiUchiKeigenzeiTotal += jihiUchiKeigenzeiTmp;

                        if (jihiSotozeiTmp > 0)
                        {
                            if (sotozeiHokenPid < wrkSinKoui.HokenPid)
                            {
                                sotozeiHokenPid = wrkSinKoui.HokenPid;
                                sotozeiHokenId = wrkSinKoui.HokenId;
                            }
                        }

                        if (jihiKeigenzeiTmp > 0)
                        {
                            if (sotogenHokenPid < wrkSinKoui.HokenPid)
                            {
                                sotogenHokenPid = wrkSinKoui.HokenPid;
                                sotogenHokenId = wrkSinKoui.HokenId;
                            }
                        }

                        if (jihiUchizeiTmp > 0)
                        {
                            if (uchizeiHokenPid < wrkSinKoui.HokenPid)
                            {
                                uchizeiHokenPid = wrkSinKoui.HokenPid;
                                uchizeiHokenId = wrkSinKoui.HokenId;
                            }
                        }
                    
                        if (jihiUchiKeigenzeiTmp > 0)
                        {
                            if (uchigenHokenPid < wrkSinKoui.HokenPid)
                            {
                                uchigenHokenPid = wrkSinKoui.HokenPid;
                                uchigenHokenId = wrkSinKoui.HokenId;
                            }
                        }

                        _common.Sin.SinKouis.Last().TotalTen = totalTen * _common.Sin.SinKouis.Last().Count;
                        _common.Sin.SinKouis.Last().Ten = totalTen;
                        if (addTenCount)
                        {
                            _common.Sin.SinKouis.Last().TenColCount = _common.Sin.SinKouis.Last().Count;
                        }
                        _common.Sin.SinKouis.Last().EntenKbn = enTenKbn;

                        if(wrkSinRp.SanteiKbn == SanteiKbnConst.Jihi && enTenKbn == 0)
                        {
                            // 自費算定で円点区分が点数の場合、x10して金額に換算する
                            _common.Sin.SinKouis.Last().TotalTen = _common.Sin.SinKouis.Last().TotalTen * 10;
                            _common.Sin.SinKouis.Last().Ten = _common.Sin.SinKouis.Last().Ten * 10;
                            _common.Sin.SinKouis.Last().EntenKbn = 1;
                        }

                        if (wrkSinRp.SinId != ReceSinId.Jihi && wrkSinRp.SanteiKbn == SanteiKbnConst.Jihi)
                        {
                            // 自費算定分（保険外行為以外）の課税区分をセットする
                            _common.Sin.SinKouis.Last().KazeiKbn = _common.Mst.GetJihisanteiKazei();
                        }

                        // 日情報の設定

                        if (wrkSinKoui.WeekCalcAppendDays.Any())
                        {
                            // 在宅週単位計算項目を含むRpの場合、日情報を調整
                            // 週単位計算する場合、当週すべての日情報に「1」をセット
                            // ただし、調整用項目と本項目それぞれ守備範囲があるので注意（WeekCalcAppendDaysに従えばOK）

                            for (int i = 0; i < wrkSinKoui.WeekCalcAppendDays.Count; i++)
                            {
                                var property = typeof(SinKouiModel).GetProperty("Day" + (wrkSinKoui.WeekCalcAppendDays[i] % 100).ToString());
                                property.SetValue(_common.Sin.SinKouis.Last(), 1);
                            }
                        }
                        else
                        {
                            var property = typeof(SinKouiModel).GetProperty("Day" + (_common.sinDate % 100).ToString());
                            property.SetValue(_common.Sin.SinKouis.Last(), _common.Sin.SinKouis.Last().Count);
                        }

                        // 行為回数
                        _common.Sin.AppendNewSinKouiCount(_common.Sin.SinKouis.Last().RpNo, _common.Sin.SinKouis.Last().SeqNo, _common.Sin.SinKouis.Last().KeyNo, _common.Sin.SinKouis.Last().Count);

                        // 算定区分が自費の場合
                        if (wrkSinRp.SanteiKbn == SanteiKbnConst.Jihi)
                        {
                            // 自費算定分の金額 = 行為の合計点数 - 自費項目の合計金額（課税・非課税）
                            if (_common.Sin.SinKouis.Last().EntenKbn == 0)
                            {
                                jihiSanteiTotal += _common.Sin.SinKouis.Last().Ten * 10;
                            }
                            else
                            {
                                jihiSanteiTotal += _common.Sin.SinKouis.Last().Ten;
                            }

                            if (_common.Mst.GetJihisanteiKazei() == 1)
                            {
                                if (sotozeiHokenPid < wrkSinKoui.HokenPid)
                                {
                                    sotozeiHokenPid = wrkSinKoui.HokenPid;
                                    sotozeiHokenId = wrkSinKoui.HokenId;
                                }
                            }
                        }
                    }
                }

                _common.Sin.CommitSinRpInf();
            }

            // 消費税
            Syohizei(jihiSanteiTotal, jihiKoumokuTotal, jihiUchizeiTotal, jihiSotozeiTotal, jihiUchiKeigenzeiTotal, jihiKeigenzeiTotal, 
                sotozeiHokenPid, sotozeiHokenId, sotogenHokenPid, sotogenHokenId, uchizeiHokenPid, uchizeiHokenId, uchigenHokenPid, uchigenHokenId);

            // 行為データ生成とチェック
            foreach (SinRpInfModel sinRpInf in _common.Sin.SinRpInfs.FindAll(p=>p.UpdateState == UpdateStateConst.Add))
            {
                // 更新ステータスが追加のRp
                foreach(SinKouiModel sinKoui in _common.Sin.SinKouis.FindAll(p=>p.KeyNo == sinRpInf.KeyNo))
                {
                    // Rpの行為

                    // 行為の詳細データを取得
                    sinKoui.DetailData = _common.Sin.GetDetailData(sinKoui);
                }

                if (_common.calcMode != CalcModeConst.Trial)
                {
                    // Rpの行為データを取得
                    sinRpInf.KouiData = _common.Sin.GetKouiData(sinRpInf);

                    // Rp内容が一致するＲｐが存在するかチェック
                    List<SinRpInfModel> sinRpinfs =
                        _common.Sin.SinRpInfs.FindAll(p =>
                            p.KouiData == sinRpInf.KouiData &&
                            p.UpdateState == UpdateStateConst.None);
                    if (sinRpinfs.Any())
                    {
                        // 存在する場合、RpNoを更新
                        // UpdateStateを更新に変更し、DBに追加されないようにしておく
                        sinRpInf.RpNo = sinRpinfs.First().RpNo;
                        sinRpInf.UpdateState = UpdateStateConst.Update;

                        // Rpに紐づく行為カウントに対し、RpNoの更新を行う（行為カウントは来院に紐づくデータなので、すべて追加する必要がある）
                        foreach (SinKouiCountModel sinCount in _common.Sin.SinKouiCounts.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                        {
                            List<SinKouiCountModel> sinCountAdds =
                                _common.Sin.SinKouiCounts.FindAll(p =>
                                    p.RpNo == sinRpinfs.First().RpNo &&
                                    p.SeqNo == sinCount.SeqNo &&        //←これかな？
                                    p.RaiinNo == _common.raiinNo &&
                                    p.UpdateState == UpdateStateConst.Add &&
                                    p.KeyNo != sinRpInf.KeyNo);

                            if (sinCountAdds.Any())
                            {
                                foreach (SinKouiCountModel sinCountAdd in sinCountAdds)
                                {
                                    sinCountAdd.Count += sinCount.Count;
                                }
                                sinCount.UpdateState = UpdateStateConst.Update;
                                _common.Sin.SinKouiCounts.RemoveAll(p => p.KeyNo == sinRpInf.KeyNo);
                            }

                            // RpNoを合わせる
                            sinCount.RpNo = sinRpinfs.First().RpNo;
                            
                        }

                        // Rpに紐づく行為に対し、RpNoと更新ステータスの更新を行う
                        foreach (SinKouiModel sinKoui in _common.Sin.SinKouis.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                        {
                            sinKoui.RpNo = sinRpinfs.First().RpNo;
                            sinKoui.UpdateState = UpdateStateConst.Update;

                            // 行為の回数調整
                            foreach (SinKouiModel sinKouiUp in _common.Sin.SinKouis.FindAll(p => p.RpNo == sinRpinfs.First().RpNo && p.UpdateState == UpdateStateConst.None))
                            {
                                //sinKouiUp.Count += sinKoui.Count;
                                //sinKouiUp.TenColCount += sinKoui.TenColCount;

                                // 加減算方式では間違う可能性もあるので、直に取得する
                                sinKouiUp.Count =
                                    _common.Sin.SinKouiCounts.Where(p =>
                                        p.HpId == _common.hpId &&
                                        p.PtId == _common.ptId &&
                                        p.SinYm == _common.sinDate / 100 &&
                                        p.RpNo == sinKouiUp.RpNo &&
                                        p.SeqNo == sinKouiUp.SeqNo &&
                                        p.UpdateState != UpdateStateConst.Delete &&
                                        p.UpdateState != UpdateStateConst.Update)
                                    .Sum(p =>
                                        p.Count);
                                //if (sinKouiUp.TenColCount > 0)
                                if (sinKouiUp.TenColCount > 0 || sinKoui.TenColCount > 0)
                                {
                                    sinKouiUp.TenColCount = sinKouiUp.Count;
                                }

                                // 同日再診、電話再診のコメントについて、HokenPidを更新
                                // これらの項目はSIN_KOUI.HOKEN_PID不問で結合される項目であるため、
                                // ここで更新しないと最初に算定したときのPIDのままで固定されてしまう
                                if (sinRpInf.KouiData.Contains(ItemCdConst.CommentSaisinDojitu) ||
                                    sinRpInf.KouiData.Contains(ItemCdConst.CommentDenwaSaisin) ||
                                    sinRpInf.KouiData.Contains(ItemCdConst.CommentDenwaSaisinDojitu) ||
                                    sinRpInf.KouiData.Contains(ItemCdConst.CommentSyosinJikanNai))
                                {
                                    sinKouiUp.HokenPid = sinKoui.HokenPid;
                                }

                                // 日情報
                                if (_common.Sin.SinKouiDetails.Any(p =>
                                     p.KeyNo == sinKoui.KeyNo &&
                                     p.SeqNo == sinKoui.SeqNo &&
                                     _common.Mst.ZaiWeekCalcList.Contains(p.ItemCd) &&
                                     p.IsNodspRece == 1))
                                {
                                    // for (int checkDate = _common.SinFirstDateOfWeek; checkDate <= _common.SinLastDateOfWeek; checkDate++)
                                    for (int checkDate = _common.SinFirstDateOfMonth; checkDate <= _common.SinLastDateOfMonth; checkDate++)
                                    {
                                        var property = typeof(SinKouiModel).GetProperty("Day" + (checkDate % 100).ToString());
                                        var count = property.GetValue(sinKouiUp);
                                        var count2 = property.GetValue(sinKoui);

                                        if (count != null && count2 != null)
                                        {
                                            if (count.AsInteger() + count2.AsInteger() > 0)
                                            {
                                                property.SetValue(sinKouiUp, 1);
                                            }
                                            else
                                            {
                                                property.SetValue(sinKouiUp, 0);
                                            }
                                            //property.SetValue(sinKouiUp, count.AsInteger() + count2.AsInteger());
                                        }
                                    }
                                }
                                else
                                {
                                    var property = typeof(SinKouiModel).GetProperty("Day" + (_common.sinDate % 100).ToString());
                                    property.SetValue(sinKouiUp,
                                            _common.Sin.SinKouiCounts.Where(p =>
                                                p.HpId == _common.hpId &&
                                                p.PtId == _common.ptId &&
                                                p.SinDate == _common.sinDate &&
                                                //p.SinYm == _common.sinDate / 100 &&
                                                //p.SinDay == _common.sinDate % 100 &&
                                                p.RpNo == sinKouiUp.RpNo &&
                                                p.SeqNo == sinKouiUp.SeqNo &&
                                                p.UpdateState != UpdateStateConst.Delete &&
                                                p.UpdateState != UpdateStateConst.Update)
                                            .Sum(p =>
                                                p.Count));
                                }
                                sinKouiUp.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                sinKouiUp.UpdateId = 0;
                                sinKouiUp.UpdateMachine = Hardcode.ComputerName;
                            }
                        }

                        // Rpに紐づく詳細に対し、RpNoと更新ステータスの更新を行う
                        foreach (SinKouiDetailModel sinDtl in _common.Sin.SinKouiDetails.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                        {
                            sinDtl.RpNo = sinRpinfs.First().RpNo;
                            sinDtl.UpdateState = UpdateStateConst.Update;
                        }

                        //// Rpに紐づく行為カウントに対し、RpNoの更新を行う
                        //foreach (SinKouiCountModel sinCount in _common.Sin.SinKouiCounts.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                        //{
                        //    sinCount.RpNo = sinRpinfs.First().RpNo;
                        //}

                        // 初回算定日の更新
                        foreach (SinRpInfModel dbSinRpInf in sinRpinfs)
                        {
                            dbSinRpInf.FirstDay =
                            _common.Sin.SinKouiCounts.Where(p =>
                               p.HpId == _common.hpId &&
                               p.PtId == _common.ptId &&
                               p.SinYm == _common.sinDate / 100 &&
                               p.RpNo == sinRpInf.RpNo &&
                               p.UpdateState != UpdateStateConst.Delete)
                            .Select(p => p.SinDay)
                            .Min();
                        }

                    }
                    else
                    {
                        bool addData = true;

                        //if (sinRpInf.SinKouiKbn == ReceKouiKbn.Syosin || sinRpInf.SinKouiKbn == ReceKouiKbn.Saisin || 
                        //    sinRpInf.SinKouiKbn == ReceKouiKbn.ChusyaSonota ||
                        //    sinRpInf.SinKouiKbn == ReceKouiKbn.Sonota || sinRpInf.SinKouiKbn == ReceKouiKbn.Kensa ||
                        //    (sinRpInf.SinKouiKbn >= ReceKouiKbn.TouyakuMin && sinRpInf.SinKouiKbn <= ReceKouiKbn.TouyakuMax ||
                        //    sinRpInf.SinKouiKbn == ReceKouiKbn.Jihi)
                        //    )
                        //{
                            sinRpinfs =
                                _common.Sin.SinRpInfs.FindAll(p =>
                                    p.KouiData == sinRpInf.KouiData &&
                                    p.UpdateState == UpdateStateConst.Add &&
                                    p.KeyNo != sinRpInf.KeyNo);

                            if (sinRpinfs.Any())
                            {
                                // 存在する場合、RpNoを更新
                                // UpdateStateを更新に変更し、DBに追加されないようにしておく
                                sinRpInf.RpNo = sinRpinfs.First().RpNo;
                                sinRpInf.UpdateState = UpdateStateConst.Update;

                                // Rpに紐づく行為カウントに対し、RpNoの更新を行う（行為カウントは来院に紐づくデータなので、すべて追加する必要がある）
                                foreach (SinKouiCountModel sinCount in _common.Sin.SinKouiCounts.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                                {
                                    // 同来院内で同一RpNo,SeqNoでKeyNo違いのCountがあれば、合体させる
                                    List<SinKouiCountModel> sinCountAdds =
                                        _common.Sin.SinKouiCounts.FindAll(p =>
                                            p.RpNo == sinRpinfs.First().RpNo &&
                                            p.UpdateState == UpdateStateConst.Add &&
                                            p.SeqNo == sinCount.SeqNo &&    // <=これが必要？
                                            p.KeyNo != sinRpInf.KeyNo &&
                                            p.RaiinNo == _common.raiinNo);  // 来院で絞っておく必要がある？

                                    if (sinCountAdds.Any())
                                    {
                                        // 回数を加算しておく
                                        foreach (SinKouiCountModel sinCountAdd in sinCountAdds)
                                        {
                                            sinCountAdd.Count += sinCount.Count;
                                        }
                                        //sinCount.UpdateState = UpdateStateConst.Update;
                                        //_common.Sin.SinKouiCounts.RemoveAll(p => p.KeyNo == sinRpInf.KeyNo);
                                        // 加算済みKeyNoは削除
                                        _common.Sin.SinKouiCounts.RemoveAll(p => p.KeyNo == sinRpInf.KeyNo);

                                        //}
                                        sinCount.UpdateState = UpdateStateConst.Update;
                                    }
                                    else
                                    {
                                        // 見つからなかった場合、KeyNoを合わせる
                                        // ※上で同様の処理があるが、こちらは、今から追加するデータの中で同じものがあった場合の処理
                                        // 　なので、RpNoは仮採番のものであり、KeyNoを結合することで、更新時にRpInfに対して採番されるRpNoに更新される
                                        sinCount.KeyNo = sinRpinfs.First().KeyNo;
                                    }
                                    sinCount.RpNo = sinRpinfs.First().RpNo;
                                    
                                }

                                //_common.Sin.SinKouiCounts.RemoveAll(p => p.KeyNo == sinRpInf.KeyNo);

                                // Rpに紐づく行為に対し、RpNoと更新ステータスの更新を行う
                                foreach (SinKouiModel sinKoui in _common.Sin.SinKouis.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                                {
                                    sinKoui.RpNo = sinRpinfs.First().RpNo;
                                    sinKoui.UpdateState = UpdateStateConst.Update;

                                    // 行為の回数調整
                                    foreach (SinKouiModel sinKouiUp in 
                                        _common.Sin.SinKouis.FindAll(p => 
                                            p.RpNo == sinRpinfs.First().RpNo && 
                                            (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add)))
                                    {
                                        //sinKouiUp.Count += sinKoui.Count;
                                        //sinKouiUp.TenColCount += sinKoui.TenColCount;

                                        // 加減算方式では間違う可能性もあるので、直に取得する
                                        sinKouiUp.Count =
                                            _common.Sin.SinKouiCounts.Where(p =>
                                                p.HpId == _common.hpId &&
                                                p.PtId == _common.ptId &&
                                                p.SinYm == _common.sinDate / 100 &&
                                                p.RpNo == sinKouiUp.RpNo &&
                                                p.SeqNo == sinKouiUp.SeqNo &&
                                                p.UpdateState != UpdateStateConst.Delete &&
                                                p.UpdateState != UpdateStateConst.Update)
                                            .Sum(p =>
                                                p.Count);

                                        if (sinKouiUp.TenColCount > 0)
                                        {
                                            sinKouiUp.TenColCount = sinKouiUp.Count;
                                        }

                                        // 日情報
                                        if (_common.Sin.SinKouiDetails.Any(p =>
                                             p.KeyNo == sinKoui.KeyNo &&
                                             p.SeqNo == sinKoui.SeqNo &&
                                             _common.Mst.ZaiWeekCalcList.Contains(p.ItemCd) &&
                                             p.IsNodspRece == 1))
                                        {
                                            for (int checkDate = _common.SinFirstDateOfWeek; checkDate <= _common.SinLastDateOfWeek; checkDate++)
                                            {
                                            //var property = typeof(SinKouiModel).GetProperty("Day" + (checkDate % 100 + checkDate).ToString());
                                            var property = typeof(SinKouiModel).GetProperty("Day" + (checkDate % 100).ToString());
                                            var count = property.GetValue(sinKouiUp);
                                                var count2 = property.GetValue(sinKoui);

                                                if (count != null && count2 != null)
                                                {
                                                    if (count.AsInteger() + count2.AsInteger() > 0)
                                                    {
                                                        property.SetValue(sinKouiUp, 1);
                                                    }
                                                    else
                                                    {
                                                        property.SetValue(sinKouiUp, 0);
                                                    }
                                                    //property.SetValue(sinKouiUp, count.AsInteger() + count2.AsInteger());
                                                }

                                            }
                                        }
                                        else
                                        {
                                            var property = typeof(SinKouiModel).GetProperty("Day" + (_common.sinDate % 100).ToString());
                                            property.SetValue(sinKouiUp,
                                                    _common.Sin.SinKouiCounts.Where(p =>
                                                        p.HpId == _common.hpId &&
                                                        p.PtId == _common.ptId &&
                                                        p.SinDate == _common.sinDate &&
                                                        //p.SinYm == _common.sinDate / 100 &&
                                                        //p.SinDay == _common.sinDate % 100 &&
                                                        p.RpNo == sinKouiUp.RpNo &&
                                                        p.SeqNo == sinKouiUp.SeqNo &&
                                                        p.UpdateState != UpdateStateConst.Delete &&
                                                        p.UpdateState != UpdateStateConst.Update)
                                                    .Sum(p =>
                                                        p.Count));
                                        }

                                    }
                                }

                                // Rpに紐づく詳細に対し、RpNoと更新ステータスの更新を行う
                                foreach (SinKouiDetailModel sinDtl in _common.Sin.SinKouiDetails.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                                {
                                    sinDtl.RpNo = sinRpinfs.First().RpNo;
                                    sinDtl.UpdateState = UpdateStateConst.Update;
                                }

                                // 初回算定日の更新
                                foreach (SinRpInfModel dbSinRpInf in sinRpinfs)
                                {
                                    dbSinRpInf.FirstDay =
                                    _common.Sin.SinKouiCounts.Where(p =>
                                       p.HpId == _common.hpId &&
                                       p.PtId == _common.ptId &&
                                       p.SinYm == _common.sinDate / 100 &&
                                       p.RpNo == sinRpInf.RpNo &&
                                       p.UpdateState != UpdateStateConst.Delete)
                                    .Select(p => p.SinDay)
                                    .Min();
                                }

                                addData = false;

                        //    }
                        }

                        if (addData)
                        {
                            // 存在しない場合、UpdateStateを追加に変更
                            // 付随する行為・行為詳細も更新しておく
                            sinRpInf.UpdateState = UpdateStateConst.Add;

                            // 初回算定日の更新
                            sinRpInf.FirstDay = _common.sinDate % 100;

                            // 行為の更新
                            foreach (SinKouiModel sinKoui in _common.Sin.SinKouis.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                            {
                                sinKoui.UpdateState = UpdateStateConst.Add;

                                foreach (SinKouiDetailModel sinDtl in _common.Sin.SinKouiDetails.FindAll(p => p.KeyNo == sinKoui.KeyNo))
                                {
                                    sinDtl.UpdateState = UpdateStateConst.Add;
                                }
                            }
                        }

                    }
                }
                else
                {
                    sinRpInf.UpdateState = UpdateStateConst.Add;

                    // 初回算定日の更新
                    sinRpInf.FirstDay = _common.sinDate % 100;

                    // 行為の更新
                    foreach (SinKouiModel sinKoui in _common.Sin.SinKouis.FindAll(p => p.KeyNo == sinRpInf.KeyNo))
                    {
                        sinKoui.UpdateState = UpdateStateConst.Add;

                        foreach (SinKouiDetailModel sinDtl in _common.Sin.SinKouiDetails.FindAll(p => p.KeyNo == sinKoui.KeyNo))
                        {
                            sinDtl.UpdateState = UpdateStateConst.Add;
                        }
                    }
                }

                //　行為ＲｐＮｏ
                if (sinRpInf.UpdateState != UpdateStateConst.Delete)
                {
                   // _common.Sin.AppendNewSinRpNo(sinRpInf.RpNo, sinRpInf.KeyNo);
                }

            }

            // Update削除
            //_common.Sin.SinRpInfs.RemoveAll(p => p.UpdateState == UpdateStateConst.Update);
            //_common.Sin.SinKouis.RemoveAll(p => p.UpdateState == UpdateStateConst.Update);
            //_common.Sin.SinKouiDetails.RemoveAll(p => p.UpdateState == UpdateStateConst.Update);
        }

        /// <summary>
        /// 行為に付随する詳細レコードの生成と計算処理
        /// </summary>
        /// <param name="wrkSinKoui">ワーク診療行為情報</param>
        /// <param name="yomikaeDtl">読み替え対象項目情報</param>
        /// <param name="yakuzaiTeigen">薬剤逓減情報</param>
        /// <param name="naibunpitu">内分泌検査の合計点数</param>
        /// <returns>
        /// 当該Rpの合計点数
        /// 円点区分
        /// 点数欄の回数に加えるかどうか  true: 加える   false: 加えない
        /// 自費項目の合計金額
        /// 自費項目（内税）の合計金額
        /// 自費項目（課税）の合計金額
        /// 自費項目（内税軽減税率）の合計金額
        /// 自費項目（軽減税率分）の合計金額
        /// </returns>
        private (double, int, bool, double, double, double, double, double) 
            CalcTen(
            WrkSinRpInfModel wrkSinRpInf, WrkSinKouiModel wrkSinKoui, List<YomikaeRecInf> yomikaeDtl, 
            ref YakuzaiTeigen yakuzaiTeigen, ref double naibunpitu, ref double igeHrtTen, ref List<double> naibunpituTens, ref List<double> igeHrtTens)
        {
            // このRpの合計点数
            double totalTen = 0;

            // 円点区分
            int retEnTen = 0;
            int enTenKbn = 0;
            // 点数欄回数に含めるべきRpかどうか
            bool addTenCount = false;

            // 自費項目の金額
            double jihiKoumokuKingaku = 0;
            // 外税
            double jihiSotozeiKingaku = 0;
            // 軽減税率分
            double jihiKeigenzeiKingaku = 0;
            // 内税
            double jihiUchizeiKingaku = 0;
            // 内税軽減税率分
            double jihiUchiKeigenzeiKingaku = 0;

            // 点滴点数調整項目を含むかどうか
            bool tentekiTenAdj = false;

            // 詳細
            IEnumerable<WrkSinKouiDetailModel> wrkSinDtls =
                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    p.RpNo == wrkSinKoui.RpNo &&
                    p.SeqNo == wrkSinKoui.SeqNo && 
                    p.IsDeleted == 0);
            if (!(wrkSinKoui.CdKbn == "E" && wrkSinDtls.Any(p=>p.CdKbnno > 0 && p.CdKbnno < 100)))
            {
                // エックス線診断料は並び変えない
                wrkSinDtls =
                    wrkSinDtls
                        .OrderBy(p => p.TyuCd)
                        .ThenBy(p => p.Kokuji1CalcSort)
                        .ThenBy(p => p.Kokuji2CalcSort)
                        .ThenBy(p => p.TusokuAge)   // 通則年齢加算
                        .ThenBy(p => p.TyuSeqCalcSort)
                        .ThenBy(p => p.ItemSeqNo)
                        .ThenBy(p => p.ItemEdaNo);

                if(wrkSinKoui.CdKbn == "C" && 
                   wrkSinDtls.Any(p=>p.MasterSbt == "S" && new string[] { "1", "3", "5"}.Contains(p.Kokuji1)) &&
                   new string[] { "7", "9", "A" }.Contains(wrkSinDtls.First(p => p.MasterSbt == "S").Kokuji1))
                {
                    // 在宅の場合で、基本項目が存在していて、先頭のS項目が加算項目の場合、
                    // レセプト電算でエラーになるので、注コードの条件を外す
                    // ※もしかすると、在宅だけではないかもしれないが、影響を最小にするため、在宅に限定する
                    wrkSinDtls =
                        wrkSinDtls
                            .OrderBy(p => p.Kokuji1CalcSort)
                            .ThenBy(p => p.Kokuji2)
                            .ThenBy(p => p.TusokuAge)   // 通則年齢加算
                            .ThenBy(p => p.TyuSeq)
                            .ThenBy(p => p.ItemSeqNo)
                            .ThenBy(p => p.ItemEdaNo);                    
                }
            }
            //.OrderBy(p => p.Kokuji1)
            //.ThenBy(p => p.Kokuji2)
            //.ThenBy(p => p.TyuCd)
            //.ThenBy(p => p.TyuSeq)
            //.ThenBy(p => p.ItemSeqNo)
            //.ThenBy(p => p.ItemEdaNo);

            if (wrkSinDtls.Any())
            {
                foreach (WrkSinKouiDetailModel wrkSinDtl in wrkSinDtls)
                {
                    // 点滴点数調整項目の場合、フラグを立てておく
                    if (wrkSinDtl.ItemCd == ItemCdConst.ChusyaTentekiSosatuTenAdj)
                    {
                        tentekiTenAdj = true;
                    }

                    // 点数欄回数に含めるかどうか判定
                    // 該当項目が１つでもあれば含める
                    if (CheckAddTenCount(wrkSinKoui, wrkSinDtl))
                    {
                        addTenCount = true;
                    }

                    // 点数計算
                    SinKouiDetailModel sinDtl =
                        _common.Sin.AppendNewSinKouiDetail(wrkSinDtl);

                    if(wrkSinDtl.AdjustTensu)
                    {
                        // 既に調整済み点数である場合はwrkからセットする
                        sinDtl.Ten = wrkSinDtl.Ten;
                    }
                    else if (sinDtl.TenMst != null)
                    {
                        if ((wrkSinKoui.HokatuKensa > 0) && sinDtl.FmtKbn == 1)
                        {
                            // 検査包括の場合、FMT_KBN=1は点数計算しない
                            sinDtl.Ten = 0;
                        }
                        else if (wrkSinKoui.InoutKbn == 1 && wrkSinRpInf.SanteiKbn != SanteiKbnConst.Jihi)
                        {
                            // 院外処方の場合、点数計算しない
                            sinDtl.Ten = 0;
                        }
                        else
                        {
                            // 点数を求める
                            (sinDtl.Ten, retEnTen) = CalcDtl(sinDtl, wrkSinKoui.SyukeiSaki);

                            if (sinDtl.ItemCd == ItemCdConst.TouyakuTeigenKousei)
                            {
                                // 向精神薬多剤投与逓減項目の場合、向精神薬多剤投与逓減点数をセット
                                //sinDtl.Ten = yakuzaiTeigen.Kousei;
                                sinDtl.Ten = (yakuzaiTeigen.KouseiOrg - Math.Round(yakuzaiTeigen.KouseiOrg * 0.8, MidpointRounding.AwayFromZero)) * -1;
                                yakuzaiTeigen.Kousei = 0;
                                yakuzaiTeigen.KouseiOrg = 0;
                            }
                            else if (sinDtl.ItemCd == ItemCdConst.TouyakuTeigenNaifuku)
                            {
                                // 多剤投与逓減項目の場合、多剤投与逓減点数をセット
                                //sinDtl.Ten = yakuzaiTeigen.Tazai;
                                sinDtl.Ten = (yakuzaiTeigen.TazaiOrg - Math.Round(yakuzaiTeigen.TazaiOrg * 0.9, MidpointRounding.AwayFromZero)) * -1;

                                yakuzaiTeigen.Tazai = 0;
                                yakuzaiTeigen.TazaiOrg = 0;
                            }

                            // 円点区分を設定
                            if (retEnTen == 1)
                            {
                                enTenKbn = 1;
                            }
                        }

                        if (wrkSinKoui.HokatuKensa == HokatuKensaConst.NaibunpituFuka)
                        {
                            //// 内分泌負荷試験(HOKATU_KENSA = 8)の場合、月トータルで計算する必要がある
                            //if (naibunpitu <= 0)
                            //{
                            //    // 当月のトータル点数を取得する
                            //    naibunpitu = _common.Sin.GetNaibunpituTotalCost();
                            //}

                            //if (naibunpitu >= NaibunpituMax)
                            //{
                            //    // 既に超える場合は0
                            //    sinDtl.Ten = 0;
                            //}
                            //else if (sinDtl.Ten + naibunpitu > NaibunpituMax)
                            //{
                            //    // 内分泌の月最大点数を超えないようにする
                            //    sinDtl.Ten = NaibunpituMax - naibunpitu;
                            //}

                            //naibunpitu += sinDtl.Ten;

                            // 内分泌負荷試験(HOKATU_KENSA = 8)の場合、月トータルで計算する必要がある
                            if (naibunpituTens[wrkSinRpInf.SanteiKbn] <= 0)
                            {
                                // 当月のトータル点数を取得する
                                naibunpituTens[wrkSinRpInf.SanteiKbn] = _common.Sin.GetNaibunpituTotalCost();
                            }

                            if (naibunpituTens[wrkSinRpInf.SanteiKbn] >= NaibunpituMax)
                            {
                                // 既に超える場合は0
                                sinDtl.Ten = 0;
                            }
                            else if (sinDtl.Ten + naibunpituTens[wrkSinRpInf.SanteiKbn] > NaibunpituMax)
                            {
                                // 内分泌の月最大点数を超えないようにする
                                sinDtl.Ten = NaibunpituMax - naibunpituTens[wrkSinRpInf.SanteiKbn];
                            }

                            naibunpituTens[wrkSinRpInf.SanteiKbn] += sinDtl.Ten;
                        }
                        else if (wrkSinKoui.HokatuKensa == HokatuKensaConst.IgeHrt)
                        {
                            //// Ige/Hrt(HOKATU_KENSA = 11)の場合、最大1430点
                            //if (igeHrtTen >= IgeHrtTenMax)
                            //{
                            //    // 既に超える場合は0
                            //    sinDtl.Ten = 0;
                            //}
                            //else if (sinDtl.Ten + igeHrtTen > IgeHrtTenMax)
                            //{
                            //    // Ige/Hrtの最大点数を超えないようにする
                            //    sinDtl.Ten = IgeHrtTenMax - igeHrtTen;
                            //}

                            //igeHrtTen += sinDtl.Ten;

                            // Ige/Hrt(HOKATU_KENSA = 11)の場合、最大1430点
                            if (igeHrtTens[wrkSinRpInf.SanteiKbn] >= IgeHrtTenMax)
                            {
                                // 既に超える場合は0
                                sinDtl.Ten = 0;
                            }
                            else if (sinDtl.Ten + igeHrtTens[wrkSinRpInf.SanteiKbn] > IgeHrtTenMax)
                            {
                                // Ige/Hrtの最大点数を超えないようにする
                                sinDtl.Ten = IgeHrtTenMax - igeHrtTens[wrkSinRpInf.SanteiKbn];
                            }

                            igeHrtTen += sinDtl.Ten;
                        }

                        // 自費項目の場合
                        if (sinDtl.TenMst.JihiSbt > 0)
                        {
                            jihiKoumokuKingaku += sinDtl.Ten;

                            if (sinDtl.TenMst.KazeiKbn == KazeiKbnConst.Kazei)
                            {
                                // 外税
                                jihiSotozeiKingaku += sinDtl.Ten;
                            }
                            else if (sinDtl.TenMst.KazeiKbn == KazeiKbnConst.Keigen)
                            {
                                // 軽減税率分
                                jihiKeigenzeiKingaku += sinDtl.Ten;
                            }
                            else if (sinDtl.TenMst.KazeiKbn == KazeiKbnConst.Uchizei)
                            {
                                // 内税
                                jihiUchizeiKingaku += sinDtl.Ten;
                            }
                            else if (sinDtl.TenMst.KazeiKbn == KazeiKbnConst.UchiKeigenzei)
                            {
                                // 内税軽減税率分
                                jihiUchiKeigenzeiKingaku += sinDtl.Ten;
                            }
                        }

                        // 労災読み替え加算の計算
                        if (yomikaeDtl.Any(p =>
                                p.RpNo == wrkSinDtl.RpNo &&
                                p.SeqNo == wrkSinDtl.SeqNo &&
                                p.RowNo == wrkSinDtl.RowNo))
                        {
                            // 労災読み替え対象の場合、読み替え項目を追加
                            sinDtl.Ten = 0;

                            // 労災読み替え加算の対象である場合、読み替え加算を追加する
                            string addItemCd = GetRosaiYomikaeKasan(wrkSinKoui.CdKbn);

                            if (addItemCd != "")
                            {
                                sinDtl = _common.Sin.AppendNewSinKouiDetail(addItemCd, recId: sinDtl.RecId);
                            }
                        }
                    }
                }

                // 合計点数を求める
                List<SinKouiDetailModel> sinDtls = _common.Sin.FilterSinKouiDetail(_common.Sin.KeyNo, _common.Sin.SeqNo);

                // 薬剤の合計点数
                double totalYakuzaiTen = 0;
                // 向精神薬の合計点数
                double totalKouseiTen = 0;

                // 向精神薬逓減項目を含むかどうか
                bool teigenKousei = false;
                // 内服逓減項目を含むかどうか
                bool teigenNaifuku = false;
                // 酸素ボンベを含むかどうか
                bool isSansoRp = false;

                foreach (SinKouiDetailModel sinDtl in sinDtls)
                {
                    if(sinDtl.IsSanso)
                    {
                        isSansoRp = true;
                    }

                    // 投薬の逓減項目かチェック
                    if (sinDtl.ItemCd == ItemCdConst.TouyakuYakuGenKousei)
                    {
                        //（精減）
                        teigenKousei = true;
                    }
                    else if (sinDtl.ItemCd == ItemCdConst.TouyakuYakuGenNaifuku)
                    {
                        // （減）
                        teigenNaifuku = true;
                    }

                    if (sinDtl.TenMst != null && sinDtl.TenMst.DrugKbn > 0)
                    {
                        // 薬剤の点数
                        totalYakuzaiTen += sinDtl.Ten;

                        if (wrkSinKoui.CdKbn == "F" && sinDtl.TenMst.KouseisinKbn > 0)
                        {
                            // 向精神薬
                            totalKouseiTen += sinDtl.Ten;
                        }
                    }
                    else
                    {
                        // 薬剤以外
                        totalTen += sinDtl.Ten;
                    }
                }

                // 薬剤点数の計算
                if (wrkSinKoui.InoutKbn == 0 && 
                    !(wrkSinRpInf.SinId >= ReceSinId.TouyakuMin && wrkSinRpInf.SinId <= ReceSinId.ChusyaMax) && 
                    totalYakuzaiTen <= 1.5 )
                {
                    // 薬剤が15円以下の場合、薬剤は算定しない（処方、注射行為除く）
                    _common.Sin.RemoveYakuzai(_common.Sin.KeyNo, _common.Sin.SeqNo);
                }
                else if(totalYakuzaiTen > 0)
                {
                    // 薬剤点数は、五捨六入
                    totalYakuzaiTen = Math.Floor(totalYakuzaiTen + 0.4999);
                    if (wrkSinKoui.InoutKbn == 0 && totalYakuzaiTen < 1.5)
                    {
                        //1.5点（15円）未満は1点
                        totalYakuzaiTen = 1;
                    }
                }

                // 薬剤逓減計算
                CalcYakuzaiTeigen(teigenKousei, teigenNaifuku, totalYakuzaiTen, totalKouseiTen, wrkSinKoui.Count, ref yakuzaiTeigen);

                if (isSansoRp)
                {
                    // 酸素の場合
                    totalTen = Math.Round(totalTen * 10, MidpointRounding.AwayFromZero) / 10;
                }

                // 合計点数に薬剤合計点数を加える
                totalTen = Math.Round(totalTen + totalYakuzaiTen, MidpointRounding.AwayFromZero);

                if(tentekiTenAdj)
                {
                    // 点滴の点数調整項目の場合、マイナスに変換
                    totalTen = totalTen * -1;
                }

                if (totalTen <= 0)
                {
                    addTenCount = false;
                }                
            }

            return (totalTen, enTenKbn, addTenCount, jihiKoumokuKingaku, jihiUchizeiKingaku, jihiSotozeiKingaku, jihiUchiKeigenzeiKingaku, jihiKeigenzeiKingaku);
        }

        /// <summary>
        /// 点数欄の回数に加えるかどうか判断する
        /// </summary>
        /// <param name="wrkSinKoui">診療行為情報</param>
        /// <param name="wrkSinDtl">診療行為詳細情報</param>
        /// <returns>true: 点数欄の回数に加える</returns>
        private bool CheckAddTenCount(WrkSinKouiModel wrkSinKoui, WrkSinKouiDetailModel wrkSinDtl)
        {
            bool ret = false;

            // 注射手技なし項目のリスト
            List<string> chusyaSyugiNasils =
                new List<string>
                {
                    ItemCdConst.ChusyaSyuginasi31,
                    ItemCdConst.ChusyaSyuginasi32,
                    ItemCdConst.ChusyaSyuginasi33
                };
            List<string> saisinSyukeiSakils =
                new List<string>
                {
                    ReceSyukeisaki.Saisin,
                    ReceSyukeisaki.SaisinGairai,
                    ReceSyukeisaki.SaisinGairaiRousai,
                    ReceSyukeisaki.SaisinJikangai,
                    ReceSyukeisaki.SaisinKyujitu,
                    ReceSyukeisaki.SaisinSinya
                };

            if (wrkSinDtl.IsKihonKoumoku2 && !chusyaSyugiNasils.Contains(wrkSinDtl.ItemCd))
            {
                // 基本項目（注射手技なし以外）
                ret = true;
            }
            else if (wrkSinKoui.CdKbn != "G" && wrkSinDtl.RecId == "IY" && wrkSinDtl.IsYakuzai)
            {
                // 注射行為以外で、IY薬剤
                ret = true;
            }
            else if (wrkSinKoui.CdKbn != "G" && wrkSinDtl.RecId == "TO" && wrkSinDtl.IsTokuzai)
            {
                // 注射行為以外で、TO特材
                ret = true;
            }
            else if (saisinSyukeiSakils.Contains(wrkSinKoui.SyukeiSaki))
            {
                // 外来管理加算
                ret = true;
            }
            else if (wrkSinKoui.SyukeiSaki == ReceSyukeisaki.TouyakuMadoku)
            {
                // 麻毒加算
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 薬剤逓減数を求める
        /// </summary>
        /// <param name="teigenKousei">true: 向精神薬多剤投与の逓減がある</param>
        /// <param name="teigenNaifuku">多剤投与の逓減がある</param>
        /// <param name="totalYakuzaiTen">このRpの薬剤料の合計</param>
        /// <param name="totalKouiseiTen">このRpの向精神薬の薬剤料の合計</param>
        /// <param name="yakuzaiTeigen">薬剤逓減情報</param>
        private void CalcYakuzaiTeigen(bool teigenKousei, bool teigenNaifuku, double totalYakuzaiTen, double totalKouseiTen, int count, ref YakuzaiTeigen yakuzaiTeigen)
        {
            double allYakuzaiTen = totalYakuzaiTen * count;

            if (teigenKousei)
            {
                // 向精神薬多剤投与の場合

                // 向精神薬合計点数を五捨六入
                double calcKouseiTen = Math.Floor(totalKouseiTen + 0.4999);
                if (calcKouseiTen < 1.5)
                {
                    //1.5点（15円）未満は1点
                    calcKouseiTen = 1;
                }
                double allKouseiTen = calcKouseiTen * count;
                
                // 向精神薬以外の点数を求める
                double allKouseiGaiTen = allYakuzaiTen - allKouseiTen;

                // 向精神薬の逓減後の点数を求める
                //yakuzaiTeigen.Kousei += GetYakuzaiTeigensu(allKouseiTen, ItemCdConst.TouyakuTeigenKousei);
                yakuzaiTeigen.KouseiOrg += allKouseiTen;

                if (teigenNaifuku)
                {
                    // 多剤投与にも該当する場合
                    //yakuzaiTeigen.Tazai += GetYakuzaiTeigensu(allKouseiGaiTen, ItemCdConst.TouyakuTeigenNaifuku);
                    yakuzaiTeigen.TazaiOrg += allKouseiGaiTen;
                }
            }
            else if (teigenNaifuku)
            {
                // 多剤投与の場合
                //yakuzaiTeigen.Tazai += GetYakuzaiTeigensu(allYakuzaiTen, ItemCdConst.TouyakuTeigenNaifuku);
                yakuzaiTeigen.TazaiOrg += allYakuzaiTen;
            }
        }

        /// <summary>
        /// 薬剤逓減点数を求める
        /// </summary>
        /// <param name="totalTen">合計薬剤点数</param>
        /// <param name="itemCd">逓減項目の診療行為コード</param>
        /// <returns>逓減点数</returns>
        private double GetYakuzaiTeigensu(double totalTen, string itemCd)
        {
            double ret = 0;

            // 倍率を求める
            double rate = 0;
            if(itemCd == ItemCdConst.TouyakuTeigenKousei)
            {
                // 向精神薬多剤投与
                rate = 0.8;
            }
            else if(itemCd == ItemCdConst.TouyakuTeigenNaifuku)
            {
                // 多剤投与
                rate = 0.9;
            }

            if (rate > 0)
            {
                // 逓減後の点数を求める
                double totalTenTeigen = Math.Round(totalTen * rate, MidpointRounding.AwayFromZero);
                ret = totalTenTeigen - totalTen;
            }

            return ret;
        }

        /// <summary>
        /// 詳細レコード単位の計算
        /// </summary>
        /// <param name="sinDtl">診療詳細情報</param>
        /// <returns>
        ///     点数 or 金額
        ///     円点区分
        /// </returns>
        private (double, int) CalcDtl(SinKouiDetailModel sinDtl, string syukeisaki)
        {
            double ret = 0;
            int enTenKbn = 0;
            double syoteiTen = 0;

            if (sinDtl.TenMst != null && sinDtl.RecId != "CO")
            {
                if (sinDtl.TenMst.KizamiId > 0)
                {
                    // きざみ計算
                    ret = CalcDtlKizami(sinDtl);
                }
                else if (sinDtl.ItemCd == ItemCdConst.SansoHoseiRitu)
                {
                    // 酸素補正率

                    // 所定点数を取得
                    syoteiTen = GetSyoteiTen(sinDtl);

                    // 金額で計算、補正率をかけ、四捨五入
                    double tmpTen = Math.Round(syoteiTen * 10 * (1 + sinDtl.ItemTen / 100), MidpointRounding.AwayFromZero);
                    // 点数に変換(/10)、四捨五入
                    tmpTen = Math.Round(tmpTen / 10, MidpointRounding.AwayFromZero);

                    // 四捨五入で誤差が出る可能性があるので、ここで調整
                    ret = tmpTen - syoteiTen;
                }
                else if (sinDtl.ItemCd == ItemCdConst.SonotaRosaiSisiKasan)
                {
                    // その他四肢加算
                    ret = GetRosaiSonotaSisiKasanSyoteiTen(sinDtl);
                }
                else if (sinDtl.TenMst.TenId == 3)
                {
                    // 3: 点数（プラス）
                    ret = (double)((decimal)(sinDtl.Suryo * sinDtl.ItemTen));
                }
                else if (new int[] { 1, 2, 4, 99 }.Contains(sinDtl.TenMst.TenId))
                {
                    // 1: 金額（整数部7桁、小数部2桁）
                    // 2: 都道府県購入価格                
                    // 4: 都道府県購入価格（点数）、金額（整数部のみ）
                    // 99: 労災円項目
                    List<string> kingakuSyukeiCd =
                        new List<string>
                        {
                            ReceSyukeisaki.EnKyukyu,
                            ReceSyukeisaki.EnSaisin,
                            ReceSyukeisaki.EnSido,
                            ReceSyukeisaki.EnSonota,
                            ReceSyukeisaki.EnSyosin
                        };

                    if (
                        (new string[] { "Y", "T" }.Contains(sinDtl.TenMst.MasterSbt) ||
                        (sinDtl.OdrItemCd.StartsWith("Z") && 
                         sinDtl.TenMst.MasterSbt == "S" && 
                         !kingakuSyukeiCd.Contains(syukeisaki))) && 
                        sinDtl.TenMst.TenId != 99)
                    {
                        // (薬剤 or 特材 or (Z特材でS)) and TenId <> 99 の場合は点数で表示
                        ret = (double)((decimal)(sinDtl.Suryo * sinDtl.ItemTen) / 10);
                    }
                    else if (sinDtl.ItemTen == 0 && sinDtl.ItemCd.StartsWith("J") == false && sinDtl.ItemCd.StartsWith("Z") == false)
                    {
                        // J自費、Z特材項目以外で点数=0の場合、何もしない
                        // enTenKbnも0(点数)のまま
                    }
                    else
                    {
                        ret = (double)((decimal)(sinDtl.Suryo * sinDtl.ItemTen));
                        // 円単位
                        enTenKbn = 1;
                    }
                }
                else if (sinDtl.TenMst.TenId == 5)
                {
                    // 5: %加算

                    // 所定点数を求める
                    syoteiTen = GetSyoteiTen(sinDtl);

                    ret = (double)((decimal)(syoteiTen * (sinDtl.ItemTen / 100)));

                    if (sinDtl.ItemCd == ItemCdConst.GazoSinseijiKasan ||
                        sinDtl.ItemCd == ItemCdConst.GazoNyuyojiKasan ||
                        sinDtl.ItemCd == ItemCdConst.GazoYojiKasan )
                    {
                        // 画像の乳幼児加算は同一Rpにフィルムが入ることがあり、ここで端数を処理しないと都合が悪い
                        ret = Math.Round(ret, MidpointRounding.AwayFromZero);
                    }
                }
                else if (sinDtl.TenMst.TenId == 6)
                {
                    // 6: %減算
                    syoteiTen = GetSyoteiTen(sinDtl);
                    ret = (double)((decimal)(syoteiTen * (sinDtl.ItemTen / 100)) * -1);
                }
                else if (sinDtl.TenMst.TenId == 7)
                {
                    // 7: 減点診療行為
                }
                else if (sinDtl.TenMst.TenId == 8)
                {
                    // 8: 点数（マイナス）
                    ret = (double)((decimal)(sinDtl.Suryo * (sinDtl.ItemTen * -1)));
                }
                else if (sinDtl.TenMst.TenId == 9)
                {
                    // 9: 乗算割合
                }
                else if (sinDtl.TenMst.TenId == 10)
                {
                    // 10: 除算金額（金額を10で除す。） ※ベントナイト用
                    if (new string[] { "Y", "T" }.Contains(sinDtl.TenMst.MasterSbt))
                    {
                        ret = (double)((decimal)(sinDtl.Suryo * sinDtl.ItemTen)) / 10 / 10;
                    }
                    else
                    {
                        ret = (double)((decimal)(sinDtl.Suryo * sinDtl.ItemTen)) / 10;
                        // 円単位
                        enTenKbn = 1;
                    }
                }
                else if (sinDtl.TenMst.TenId == 11)
                {
                    // 11: 乗算金額（金額を10で乗ずる。） ※ステミラック注用
                    if (new string[] { "Y", "T" }.Contains(sinDtl.TenMst.MasterSbt))
                    {
                        ret = (double)((decimal)(sinDtl.Suryo * sinDtl.ItemTen)) * 10 / 10;
                    }
                    else
                    {
                        ret = (double)((decimal)(sinDtl.Suryo * sinDtl.ItemTen)) * 10;
                        // 円単位
                        enTenKbn = 1;
                    }
                }      
                
                //if(sinDtl.TenMst.JihiSbt > 0)
                //{
                //    // 自費項目の場合、税の計算
                //    if(sinDtl.TenMst.KazeiKbn == 1)
                //    {
                //        // 外税
                //        ret = sinDtl.Suryo * sinDtl.TenMst.Ten * (100 + _common.GetZei()) / 100;
                //    }
                //    else if(sinDtl.TenMst.KazeiKbn == 2)
                //    {
                //        // 内税
                //    }
                //}
            }

            return (ret, enTenKbn);
        }

        /// <summary>
        /// 所定点数を求める
        /// 
        /// 基本的に所定点数には注加算通則加算は含めないようだが、
        /// なぜか、率加算の対象となるものは注加算を含めるケースが多い
        /// なので基本的に注加算を含めるものとし、
        /// 除外されるケースを除外するイメージのロジックにしてある
        /// </summary>
        /// <param name="sinDtlKasan"></param>
        /// <param name="incRiha">リハビリ項目を含むかどうか（四肢加算用）</param>
        /// <returns>所定点数</returns>
        private double GetSyoteiTen(SinKouiDetailModel sinDtlKasan)
        {
            double syoteiTen = 0;

            // 同一Rp内の診療行為詳細のリストを取得する
            List<SinKouiDetailModel> sinDtls =
                _common.Sin.SinKouiDetails.FindAll(p => p.KeyNo == sinDtlKasan.KeyNo && p.SeqNo == sinDtlKasan.SeqNo);

            if (sinDtlKasan.ItemCd == ItemCdConst.SansoHoseiRitu)
            {
                // 酸素補正率の場合、上にある酸素に対して加算する
                for (int i = sinDtls.Count - 2; i >= 0; i--)
                {
                    if (sinDtls[i].IsSanso)
                    {
                        syoteiTen += sinDtls[i].Ten;
                        break;
                    }
                    else if (sinDtls[i].TenMst.MasterSbt != "C")
                    {
                        break;
                    }
                }
            }
            else if (sinDtlKasan.MstItemCd == ItemCdConst.GazoFilmNyu)
            {
                // フィルムの乳幼児加算の場合、上にあるフィルムに対して加算する
                for (int i = sinDtls.Count - 2; i >= 0; i--)
                {
                    if (sinDtls[i].SinKouiKbn == 77)
                    {
                        syoteiTen += sinDtls[i].Ten;
                    }
                    else if (sinDtls[i].MasterSbt != "C")
                    {
                        break;
                    }
                }
            }
            else
            {
                for (int i = sinDtls.Count - 2; i >= 0; i--)
                {
                    if (new string[] { "1", "3", "5" }.Contains(sinDtls[i].Kokuji1))
                    {
                        // 加算項目ではない場合、所定点数に加える（ただし、KOKUJI1=1の場合、加算のこともあるのでKOKUJI2もチェック）

                        if (new string[] { "7", "9" }.Contains(sinDtls[i].Kokuji2) &&
                            sinDtls[i].TenMst.CdKbn == "E")
                        {
                            // 画像で告２が基本料以外の場合は足さない
                        }
                        else if(sinDtlKasan.TenId == 6 && sinDtlKasan.SinKouiKbn == 50 && new string[] { "7", "9" }.Contains(sinDtls[i].Kokuji2))
                        {
                            // 手術の減算の場合、加算項目は足さない
                        }
                        else if(sinDtlKasan.IsSisiKasanSyujyutu && sinDtlKasan.SinKouiKbn == 50 && sinDtls[i].MasterSbt == "R")
                        {
                            // 四肢加算（手術）の場合、R（労災マスタの項目）は対象外とする
                        }
                        else
                        { 
                            syoteiTen += sinDtls[i].Ten;
                        }

                        if (new string[] { "1", "3", "5" }.Contains(sinDtls[i].Kokuji2) && 
                           sinDtls[i].TenMst.CdKbn == "E")
                        {
                            // 画像の場合、加算項目ではない場合はここまで　もしかすると、撮影料診断料のときだけかも？
                            break;
                        }
                    }
                    else if(sinDtlKasan.SinKouiKbn == 62)
                    {
                        // 生体検査は、加算項目も含める
                        syoteiTen += sinDtls[i].Ten;
                    }
                    else if (sinDtlKasan.Kokuji1 == "9" &&
                        (sinDtls[i].TusokuTargetKbn == 0 && new string[] { "7", "A" }.Contains(sinDtls[i].Kokuji1)))
                    {
                        // 通則加算の場合、注加算含む（手術だけかもしれない？）
                        if (!((sinDtls[i].Kokuji1 == "9") &&
                            (sinDtls[i].TenId == 5 || sinDtls[i].TenId == 6)))
                        {
                            // でも、率加算は除く
                            syoteiTen += sinDtls[i].Ten;
                        }
                    }
                    else if (sinDtlKasan.Kokuji1 == "7" && 
                            (sinDtls[i].Kokuji1 == "7" && (sinDtls[i].SinKouiKbn >= 60 && sinDtls[i].SinKouiKbn <= 69)))
                    {
                        // 検査の注加算の場合、注加算含む（注加算含める、TUSOKU_AGE=1の加算だけかも？）
                        if (!((sinDtls[i].Kokuji1 == "9") &&
                            (sinDtls[i].TenId == 5 || sinDtls[i].TenId == 6)))
                        {
                            syoteiTen += sinDtls[i].Ten;
                        }
                    }
                    else if (sinDtlKasan.Kokuji1 == "7" &&
                            (sinDtls[i].Kokuji1 == "7" && (sinDtls[i].SinKouiKbn >= 70 && sinDtls[i].SinKouiKbn <= 79)))
                    {
                        // 画像の注加算の場合、注加算含む
                        // CT/MRIの減算は特別扱い
                        if (!(new int[] { 5, 6 }.Contains(sinDtls[i].TenId)) || sinDtls[i].ItemCd == ItemCdConst.GazoCtMriGensan)                            
                        {
                            syoteiTen += sinDtls[i].Ten;
                        }
                    }
                    else if (new List<string> { "7", "A" }.Contains(sinDtlKasan.Kokuji1) && 
                            (sinDtls[i].Kokuji1 == "7" && !(new List<int> { 5,6 }.Contains(sinDtls[i].TenId))) &&
                            !(sinDtlKasan.TenId == 6 && sinDtlKasan.SinKouiKbn == 50))
                    {
                        // 注加算の場合、率加算以外の注加算含む（手術だけかもしれない？）
                        // 2020/06/09 手術はむしろ違うかも？少なくとも併施減算は加算項目を含めないらしい とりあえず、手術併施減算の時は対象外にしておく
                        if (!((sinDtls[i].Kokuji1 == "9") &&
                            (sinDtls[i].TenId == 5 || sinDtls[i].TenId == 6)))
                        {
                            syoteiTen += sinDtls[i].Ten;
                        }
                    }
                    else if (sinDtls[i].MasterSbt == "Y" || sinDtls[i].MasterSbt == "T")
                    {
                        // 薬剤、特材が間にあったら抜ける
                        break;
                    }
                }
            }

            return syoteiTen;
        }

        /// <summary>
        /// 労災四肢加算（その他）１．５倍の点数を求める
        /// </summary>
        /// <param name="sinDtlKasan"></param>
        /// <returns>所定点数</returns>
        private double GetRosaiSonotaSisiKasanSyoteiTen(SinKouiDetailModel sinDtlKasan)
        {
            double gokei = 0;
            double syoteiTen = 0;

            // 同一Rp内の診療行為詳細のリストを取得する
            List<SinKouiDetailModel> sinDtls =
                _common.Sin.SinKouiDetails.FindAll(p => p.KeyNo == sinDtlKasan.KeyNo);
 
            for (int i = sinDtls.Count - 2; i >= 0; i--)
            {

                if (new string[] { "1", "3", "5" }.Contains(sinDtls[i].Kokuji1))
                {
                    // 加算項目ではない場合、所定点数に加える
                    if (sinDtls[i].IsRihabiri)
                    {
                        gokei +=
                           Math.Round(sinDtls[i].ItemTen * (1 + sinDtlKasan.TenMst.Ten / 100), MidpointRounding.AwayFromZero) * sinDtls[i].Suryo;
                        gokei -= sinDtls[i].Ten;
                    }
                    else
                    {
                        syoteiTen += sinDtls[i].Ten;
                    }
                }
                else if (sinDtlKasan.Kokuji1 == "9" &&
                    (sinDtls[i].TusokuTargetKbn == 0 || sinDtls[i].Kokuji1 == "7"))
                {
                    // 通則加算の場合、注加算含む（手術だけかもしれない？）
                    if (!((sinDtls[i].Kokuji1 == "9") &&
                        (sinDtls[i].TenId == 5 || sinDtls[i].TenId == 6)))
                    {
                        syoteiTen += sinDtls[i].Ten;
                    }
                }
                else if (sinDtlKasan.Kokuji1 == "7" &&
                        (sinDtls[i].Kokuji1 == "7" && !(new List<int> { 5, 6 }.Contains(sinDtls[i].TenId))))
                {
                    // 注加算の場合、率加算以外の注加算含む（手術だけかもしれない？）
                    if (!((sinDtls[i].Kokuji1 == "9") &&
                        (sinDtls[i].TenId == 5 || sinDtls[i].TenId == 6)))
                    {
                        syoteiTen += sinDtls[i].Ten;
                    }
                }
                else if (sinDtls[i].MasterSbt == "Y" || sinDtls[i].MasterSbt == "T")
                {
                    // 薬剤、特材が間にあったら抜ける
                    break;
                }
            }
            
            return syoteiTen * (sinDtlKasan.ItemTen / 100) + gokei;
        }
        /// <summary>
        /// きざみ計算
        /// </summary>
        /// <param name="sinDtl"></param>
        /// <returns></returns>
        private double CalcDtlKizami(SinKouiDetailModel sinDtl)
        {
            double ret = 0;

            // きざみ計算
            if (sinDtl.TenMst != null && sinDtl.Suryo <= sinDtl.TenMst.KizamiMin - sinDtl.TenMst.KizamiVal)
            {
                // 数量 <= きざみ下限 - きざみ数量 の場合、基本点数
                ret = sinDtl.ItemTen;

                if (sinDtl.TenMst.KizamiErr == 2 ||
                   sinDtl.TenMst.KizamiErr == 3)
                {
                    // 算定せず
                    ret = 0;
                    sinDtl.IsDeleted = 1;
                    _common.AppendCalcLog(2, String.Format(FormatConst.LowSuryo, sinDtl.TenMst.ReceName, sinDtl.Suryo, sinDtl.TenMst.KizamiMin));
                }
            }
            else if (sinDtl.TenMst != null && 
                     sinDtl.Suryo > sinDtl.TenMst.KizamiMin - sinDtl.TenMst.KizamiVal &&
                     sinDtl.Suryo <= sinDtl.TenMst.KizamiMin)
            {
                // きざみ下限-きざみ数量 < 数量 <= きざみ下限 の場合、きざみ計算
                ret = sinDtl.ItemTen;
            }
            else if (sinDtl.TenMst != null && 
                     sinDtl.TenMst.KizamiMin < sinDtl.Suryo &&
                     sinDtl.Suryo <= sinDtl.TenMst.KizamiMax)
            {
                // きざみ下限 < 数量 <= きざみ上限 の場合、きざみ計算

                ret = KizamiCalc1();                
            }
            else if (sinDtl.TenMst != null && 
                     sinDtl.TenMst.KizamiMax < sinDtl.Suryo)
            {
                // きざみ上限 < 数量 の場合、きざみ計算

                
                if (new int[] { 0, 2 }.Contains(sinDtl.TenMst.KizamiErr))
                {
                    ret = KizamiCalc1();
                    _common.AppendCalcLog(1, String.Format(FormatConst.HighSuryo, sinDtl.TenMst.ReceName, sinDtl.Suryo, sinDtl.TenMst.KizamiMax));
                }
                else
                {
                    // 上限値で計算
                    ret = KizamiCalc2();
                    _common.AppendCalcLog(1, String.Format(FormatConst.HighSuryo2, sinDtl.TenMst.ReceName, sinDtl.Suryo, sinDtl.TenMst.KizamiMax));
                }
            }

            // 四捨五入して返す？
            //return ret;
            return Math.Round(ret, MidpointRounding.AwayFromZero);

            #region Local Method

            // きざみ計算
            //点数計算式１
            //点数＝基本点数＋（数量ーTEN_MST.KIZAMI_MIN / TEN_MST.KIZAMI_VAL ※切り上げ)×TEN_MST.KIZAMI_TEN ※四捨五入
            double KizamiCalc1()
            {
                // 基本点数 + ((数量 - きざみ下限) / きざみ数量) * きざみ点数
                double retVal = 0;

                if (sinDtl.TenMst != null)
                {
                    retVal = Math.Round(
                        sinDtl.ItemTen +
                        Math.Ceiling((sinDtl.Suryo - sinDtl.TenMst.KizamiMin) / sinDtl.TenMst.KizamiVal) *
                        sinDtl.TenMst.KizamiTen
                        , 3, MidpointRounding.AwayFromZero);
                }
                return retVal;
            }

            //点数計算式２
            //点数＝基本点数＋（TEN_MST.KIZAMI_MAXーTEN_MST.KIZAMI_MIN / TEN_MST.KIZAMI_VAL ※切り上げ)×TEN_MST.KIZAMI_TEN ※四捨五入
            double KizamiCalc2()
            {
                // 基本点数 + ((数量 - きざみ下限) / きざみ数量) * きざみ点数
                double retVal = 0;

                if (sinDtl.TenMst != null)
                {
                    retVal = Math.Round(
                        sinDtl.ItemTen +
                        ((sinDtl.TenMst.KizamiMax - sinDtl.TenMst.KizamiMin) / sinDtl.TenMst.KizamiVal) *
                        sinDtl.TenMst.KizamiTen
                        , 3, MidpointRounding.AwayFromZero);
                }
                return retVal;
            }
            #endregion

        }

        /// <summary>
        /// 労災時、消炎鎮痛湿布の合計点数を取得する
        /// </summary>
        /// <param name="filteredOdrInf"></param>
        /// <returns>合計点数</returns>
        private double GetRosaiSippuTotal()
        {
            double totalSippuTen = 0;

            List<WrkSinRpInfModel> syotiRpInfs = 
                _common.Wrk.wrkSinRpInfs.FindAll(p => 
                    p.RaiinNo == _common.raiinNo && 
                    p.HokenKbn == _common.hokenKbn && 
                    p.SinKouiKbn == ReceKouiKbn.Syoti && 
                    p.IsDeleted == 0);

            foreach (WrkSinRpInfModel wrkRpInf in syotiRpInfs)
            {
                // 労災の場合、湿布の合算をチェック
                List<WrkSinKouiModel> wrkKouis = 
                    _common.Wrk.wrkSinKouis.FindAll(p => 
                        p.RaiinNo == _common.raiinNo && 
                        p.HokenKbn == _common.hokenKbn && 
                        p.RpNo == wrkRpInf.RpNo && 
                        p.IsDeleted == 0);

                foreach (WrkSinKouiModel wrkKoui in wrkKouis)
                {
                    double bairitu = 1;
                    int bui = 0;

                    List<WrkSinKouiDetailModel> wrkDtls = 
                        _common.Wrk.wrkSinKouiDetails.FindAll(p => 
                            p.RaiinNo == _common.raiinNo && 
                            p.HokenKbn == _common.hokenKbn && 
                            p.RpNo == wrkKoui.RpNo && 
                            p.SeqNo == wrkKoui.SeqNo);

                    if (wrkDtls.Any(p => p.ItemCd == ItemCdConst.SyotiSyoenSippu))
                    {
                        // このRpに倍率項目が存在するかチェック
                        if (wrkDtls.Any(p => p.ItemCd == ItemCdConst.SyotiRosaiSisiKasan))
                        {
                            bairitu = 1.5;
                        }
                        else if (wrkDtls.Any(p => p.ItemCd == ItemCdConst.SyotiRosaiSisiKasan2))
                        {
                            bairitu = 2;
                        }
                        else
                        {
                            // 四肢加算項目のオーダーはない
                            if (wrkDtls.Any(p => p.BuiKbn == 10))
                            {
                                // 部位あり
                                bui = 10;
                            }
                            else if (wrkDtls.Any(p => p.BuiKbn == 3))
                            {
                                // 部位あり
                                bui = 3;
                            }
                        }

                        foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                        {
                            if (wrkDtl.ItemCd == ItemCdConst.SyotiSyoenSippu)
                            {
                                if (bui == 10)
                                {
                                    if (wrkDtl.TenMst.SisiKbn == 1 || wrkDtl.TenMst.SisiKbn == 3)
                                    {
                                        // 四肢が存在する場合、２倍を自動算定
                                        bairitu = 2;
                                    }
                                    else if (wrkDtl.TenMst.SisiKbn == 2)
                                    {
                                        // 四肢が存在する場合、１．５倍を自動算定
                                        bairitu = 1.5;
                                    }
                                }
                                else if (bui == 3)
                                {
                                    if (wrkDtl.TenMst.SisiKbn == 1 || wrkDtl.TenMst.SisiKbn == 2)
                                    {
                                        // 四肢が存在する場合、１．５倍を自動算定
                                        bairitu = 1.5;
                                    }
                                }

                                totalSippuTen += wrkDtl.TenMst.Ten * bairitu;
                            }
                        }
                    }
                }
            }

            return totalSippuTen;
        }

        /// <summary>
        /// 労災読み替えが発生するかどうかチェック
        /// 読み替えが発生する場合、外来管理加算特例を自動算定
        /// </summary>
        /// <returns>読み替え対象のレコード情報</returns>
        private List<YomikaeRecInf> CheckRosaiGairaiKanriYomikae()
        {
            #region
            (double, int) _getBairitu(WrkSinKouiDetailModel wrkDtl)
            {
                double bairitu = 1;
                int bui = 0;

                List<WrkSinKouiDetailModel> wrkTgtDtls =
                                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                            p.RaiinNo == _common.raiinNo &&
                                            p.HokenKbn == _common.hokenKbn &&
                                            p.RpNo == wrkDtl.RpNo &&
                                            p.SeqNo == wrkDtl.SeqNo &&
                                            p.IsDeleted == DeleteStatus.None);

                if (wrkTgtDtls.Any(p => p.ItemCd == ItemCdConst.SyotiRosaiSisiKasan))
                {
                    bairitu = 1.5;
                }
                else if (wrkTgtDtls.Any(p => p.ItemCd == ItemCdConst.SyotiRosaiSisiKasan2))
                {
                    bairitu = 2;
                }
                else
                {
                    // 四肢加算項目のオーダーはない
                    if (wrkTgtDtls.Any(p => p.TenMst != null && p.TenMst.BuiKbn == 10))
                    {
                        // 部位あり
                        bui = 10;
                    }
                    else if (wrkTgtDtls.Any(p => p.TenMst != null && p.TenMst.BuiKbn == 3))
                    {
                        // 部位あり
                        bui = 3;
                    }
                }

                if (bui == 10)
                {
                    if (wrkDtl.TenMst.SisiKbn == 1 || wrkDtl.TenMst.SisiKbn == 3)
                    {
                        // 四肢が存在する場合、２倍を自動算定
                        bairitu = 2;
                    }
                    else if (wrkDtl.TenMst.SisiKbn == 2)
                    {
                        // 四肢が存在する場合、１．５倍を自動算定
                        bairitu = 1.5;
                    }
                }
                else if (bui == 3)
                {
                    if (wrkDtl.TenMst.SisiKbn == 1 || wrkDtl.TenMst.SisiKbn == 2)
                    {
                        // 四肢が存在する場合、１．５倍を自動算定
                        bairitu = 1.5;
                    }
                }

                return (bairitu, bui);
            }
            #endregion

            List<YomikaeRecInf> yomikaeDtl =
                new List<YomikaeRecInf>();
            YomikaeRecInf yomikaeTaisyoGai = null;

            var rosaiYomikaels = new List<string>()
                                {
                                    {ItemCdConst.RosaiYomikaeKensa },
                                    {ItemCdConst.RosaiYomikaeSyoti },
                                    {ItemCdConst.RosaiYomikaeMasui },
                                    {ItemCdConst.RosaiYomikaeSonota }
                                };

            // 外来管理加算の点数取得
            var tenMst = _common.Mst.GetTenMst(ItemCdConst.GairaiKanriKasan).FirstOrDefault();

            if (tenMst != null)
            {
                double gairaiKanriTen = tenMst.Ten;

                // 外来管理加算の点数未満の項目があるかチェック

                // 消炎鎮痛（湿布）が存在するかチェック
                // 合算
                double totalSippuTen = GetRosaiSippuTotal();
                

                List<(int RpNo, int SeqNo, int RowNo)> ret =
                    new List<(int RpNo, int SeqNo, int RowNo)>();
                // 厚生労働省が定める検査
                //① 超音波検査等 D215~D217
                //② 脳波検査等 D235~D237 - 2
                //③ 神経・筋検査 D239~D242
                //④ 耳鼻咽喉科学的検査 D243~D254
                //⑤ 眼科学的検査 D255~D282 - 3
                //⑥ 負荷試験等 D286~D291 - 3
                //⑦ ラジオアイソトープを用いた諸検査 D292~D294
                //⑧ 内視鏡検査 D295~D325

                // 健保点数表第２章第７部リハビリテーション H
                //・第８部精神科専門療法 I
                //・第９部処置 J
                //・第10部手術 K
                //・第11部麻酔 L
                //・第12部放射線治療 M
                
                List<WrkSinKouiDetailModel> wrkDtls = _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo ==_common.raiinNo &&
                    p.TenMst != null && 
                    p.IsDeleted == 0 &&
                    (p.TenMst.GairaiKanriKbn == 1 ||
                    (new string[] { "H", "I", "J", "K", "L", "M" }.Contains(p.TenMst.CdKbn) && p.ItemCd.StartsWith("1") && p.ItemCd.Length == 9)) &&
                    p.TenMst.CdKbn != "A"
                     //(
                     //    (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 215 && p.TenMst.CdKbnno <= 217) ||
                     //    (p.TenMst.CdKbn == "D" && (p.TenMst.CdKbnno >= 235 && p.TenMst.CdKbnno <= 236) || (p.TenMst.CdKbnno == 237 && p.TenMst.CdKouno <= 2)) ||
                     //    (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 239 && p.TenMst.CdKbnno <= 242) ||
                     //    (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 243 && p.TenMst.CdKbnno <= 254) ||
                     //    (p.TenMst.CdKbn == "D" && (p.TenMst.CdKbnno >= 255 && p.TenMst.CdKbnno <= 281) || (p.TenMst.CdKbnno == 282 && p.TenMst.CdKouno <= 3)) ||
                     //    (p.TenMst.CdKbn == "D" && (p.TenMst.CdKbnno >= 286 && p.TenMst.CdKbnno <= 290) || (p.TenMst.CdKbnno == 291 && p.TenMst.CdKouno <= 3)) ||
                     //    (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 292 && p.TenMst.CdKbnno <= 294) ||
                     //    (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 295 && p.TenMst.CdKbnno <= 325) ||
                     //    (p.TenMst.CdKbn == "H") ||
                     //    (p.TenMst.CdKbn == "I") ||
                     //    (p.TenMst.CdKbn == "J") ||
                     //    (p.TenMst.CdKbn == "K") ||
                     //    (p.TenMst.CdKbn == "L") ||
                     //    (p.TenMst.CdKbn == "M")
                     //    )
                    );

                if (wrkDtls.Any())
                {
                    #region 労災読み替え加算
                    if (
                            (_common.Mst.ExistAutoSantei(ItemCdConst.GairaiKanriKasan) && 
                             _common.Odr.ExistOdrDetailByItemCd(ItemCdConst.GairaiKanriKasanCancel) == false) ||
                            _common.Wrk.wrkSinKouiDetails.Any(p =>
                                p.RaiinNo == _common.raiinNo &&
                                p.HokenKbn == _common.hokenKbn &&
                                new string[] { ItemCdConst.GairaiKanriKasan, ItemCdConst.GairaiKanriKasanRousai}.Contains(p.ItemCd))                          
                    )
                    {
                        // 自動算定ありで取消がない or 手オーダーあり の場合、労災読み替え加算のチェック
                        double minTen = gairaiKanriTen + 1;

                        foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                        {
                            if (wrkDtl.ItemCd == ItemCdConst.SyotiSyoenSippu && totalSippuTen > gairaiKanriTen)
                            {

                            }
                            else if (wrkDtl.TenMst != null && new string[] { "1", "3", "5" }.Contains(wrkDtl.TenMst.Kokuji2))
                            {
                                // この項目が属するRpに労災四肢加算に値する項目が存在するかチェック
                                // このRpに倍率項目が存在するかチェック
                                (double bairitu, int bui) = _getBairitu(wrkDtl);

                                double chkTen = wrkDtl.TenMst.Ten * bairitu;
                                if (chkTen < gairaiKanriTen)
                                {
                                    // 点数が外来管理加算の点数未満の場合、記憶する
                                    yomikaeDtl.Add(new YomikaeRecInf(wrkDtl.RpNo, wrkDtl.SeqNo, wrkDtl.RowNo));

                                    // 最低点数を更新する場合、読み替え対象外の可能性があるので記憶する
                                    if (chkTen < minTen)
                                    {
                                        if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                                            p.RaiinNo == _common.raiinNo &&
                                            p.HokenKbn == _common.hokenKbn && 
                                            p.RpNo == wrkDtl.RpNo && 
                                            p.SeqNo == wrkDtl.SeqNo && 
                                            rosaiYomikaels.Contains(p.ItemCd)) == false)
                                        {
                                            // 同一Rp内に、外来管理加算（読み替え加算）のオーダーがない場合、外来管理加算特例の対象候補
                                            yomikaeTaisyoGai = new YomikaeRecInf(wrkDtl.RpNo, wrkDtl.SeqNo, wrkDtl.RowNo);
                                            minTen = chkTen;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        // 自動算定なし or 手オーダーなしの場合
                        // 外来管理加算（読み替え加算）がオーダーされている場合、
                        // 外来管理加算（読み替え加算）を自動算定に振り替え
                        double minTen = gairaiKanriTen + 1;

                        foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                        {
                            if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                                p.RaiinNo == _common.raiinNo &&
                                p.HokenKbn == _common.hokenKbn &&
                                p.RpNo == wrkDtl.RpNo && 
                                p.SeqNo == wrkDtl.SeqNo && 
                                rosaiYomikaels.Contains(p.ItemCd)))
                            {
                                // 同一Rp内に、外来管理加算（読み替え加算）のオーダーがある
                                if (wrkDtl.ItemCd == ItemCdConst.SyotiSyoenSippu && totalSippuTen > gairaiKanriTen)
                                {

                                }
                                else if (wrkDtl.TenMst != null && new string[] { "1", "3", "5" }.Contains(wrkDtl.TenMst.Kokuji2))
                                {
                                    // この項目が属するRpに労災四肢加算に値する項目が存在するかチェック
                                    // このRpに倍率項目が存在するかチェック
                                    (double bairitu, int bui) = _getBairitu(wrkDtl);

                                    double chkTen = wrkDtl.TenMst.Ten * bairitu;
                                    if (chkTen < gairaiKanriTen)
                                    {
                                        // 点数が外来管理加算の点数未満の場合、記憶する
                                        yomikaeDtl.Add(new YomikaeRecInf(wrkDtl.RpNo, wrkDtl.SeqNo, wrkDtl.RowNo));
                                    }
                                }
                            }
                        }
                    }

                    if (yomikaeDtl.Any())
                    {
                        // 読み替え対象が存在する場合、外来管理加算算定

                        // 同一Rp内に、外来管理加算（読み替え加算）がある場合は削除しておく
                        foreach (YomikaeRecInf yomikae in yomikaeDtl)
                        {
                            _common.Wrk.wrkSinKouiDetails.RemoveAll(p =>
                                p.RaiinNo == _common.raiinNo &&
                                p.HokenKbn == _common.hokenKbn &&
                                p.RpNo == yomikae.RpNo &&
                                p.SeqNo == yomikae.SeqNo &&
                                rosaiYomikaels.Contains(p.ItemCd));
                        }

                        //// 外来管理加算の項目がDeleteされている場合は、包括なので算定しない
                        //if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                        //         p.RaiinNo == _common.raiinNo &&
                        //         p.HokenKbn == _common.hokenKbn &&
                        //         (p.ItemCd == ItemCdConst.GairaiKanriKasan || p.ItemCd == ItemCdConst.GairaiKanriKasanRousai) &&
                        //         p.IsDeleted == 1) == false)
                        //{
                        if (_common.Mst.ExistAutoSantei(ItemCdConst.GairaiKanriKasan) ||
                            _common.Wrk.wrkSinKouiDetails.Any(p =>
                                         p.RaiinNo == _common.raiinNo &&
                                        p.HokenKbn == _common.hokenKbn &&
                                        p.ItemCd == ItemCdConst.GairaiKanriKasan)
                            )
                        {
                            // 外来管理加算の自動算定設定がある、またはオーダーがあるとき

                            // 外来管理加算の特例がすでに存在する場合は算定しない
                            if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                                 p.RaiinNo == _common.raiinNo &&
                                 p.HokenKbn == _common.hokenKbn &&
                                 p.ItemCd == ItemCdConst.GairaiKanriKasanRousai &&
                                 p.IsDeleted == DeleteStatus.None) == false)
                            {
                                List<WrkSinKouiDetailModel> wrkGairaiKanriDtls =
                                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                         p.RaiinNo == _common.raiinNo &&
                                        p.HokenKbn == _common.hokenKbn &&
                                        p.ItemCd == ItemCdConst.GairaiKanriKasan &&
                                        p.IsDeleted == DeleteStatus.None);
                                if (wrkGairaiKanriDtls.Any())
                                {
                                    // 外来管理加算が存在する場合は置き換える
                                    wrkGairaiKanriDtls.First().ItemCd = ItemCdConst.GairaiKanriKasanRousai;
                                    var gairaiKanriTenMst = _common.Mst.GetTenMst(ItemCdConst.GairaiKanriKasanRousai).FirstOrDefault();
                                    if (gairaiKanriTenMst != null)
                                    {
                                        wrkGairaiKanriDtls.First().TenMst = gairaiKanriTenMst;
                                        wrkGairaiKanriDtls.First().ItemName = gairaiKanriTenMst.ReceName;

                                        List<WrkSinKouiModel> wrkGairaiKanriKouis =
                                            _common.Wrk.wrkSinKouis.FindAll(p =>
                                                 p.RaiinNo == _common.raiinNo &&
                                                p.HokenKbn == _common.hokenKbn &&
                                                p.RpNo == wrkGairaiKanriDtls.First().RpNo &&
                                                p.SeqNo == wrkGairaiKanriDtls.First().SeqNo &&
                                                p.IsDeleted == DeleteStatus.None);
                                        if (wrkGairaiKanriKouis.Any())
                                        {
                                            wrkGairaiKanriKouis.First().SyukeiSaki = ReceSyukeisaki.SaisinGairaiRousai;
                                        }
                                    }
                                }
                                else
                                {
                                    // 外来管理加算が存在しない場合はRpを追加する
                                    //Rp
                                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Saisin, ReceSinId.Saisin, _common.syosaiSanteiKbn);
                                    //行為
                                    _common.Wrk.AppendNewWrkSinKoui(_common.syosaiPid, _common.syosaiHokenId, ReceSyukeisaki.SaisinGairaiRousai, cdKbn: "A");
                                    //詳細
                                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GairaiKanriKasanRousai, autoAdd: 1);

                                    //コミット
                                    _common.Wrk.CommitWrkSinRpInf();
                                }
                                //}

                                // 外来管理加算の包括メッセージを削除
                                _common.calcLogs.RemoveAll(p => p.Text.StartsWith(FormatConst.GairaiKanriLog));
                            }
                        }

                        // 対象外を削除
                        if (yomikaeTaisyoGai != null)
                        {
                            yomikaeDtl.RemoveAll(p =>
                                p.RpNo == yomikaeTaisyoGai.RpNo &&
                                p.SeqNo == yomikaeTaisyoGai.SeqNo &&
                                p.RowNo == yomikaeTaisyoGai.RowNo);
                        }
                    }
                    else
                    {
                        // 読み替え対象がない
                        if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                                 p.RaiinNo == _common.raiinNo &&
                                 p.HokenKbn == _common.hokenKbn &&
                                 p.ItemCd == ItemCdConst.GairaiKanriKasanRousai &&
                                 p.IsDeleted == DeleteStatus.None))
                        {
                            // 労災外来管理加算特例が存在する
                            _common.AppendCalcLog(2, "'外来管理加算（特例）' は、特例対象項目がないため、算定できません。");

                            foreach (WrkSinKouiDetailModel wrkDtl in
                                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                             p.RaiinNo == _common.raiinNo &&
                                             p.HokenKbn == _common.hokenKbn &&
                                             p.ItemCd == ItemCdConst.GairaiKanriKasanRousai &&
                                             p.IsDeleted == DeleteStatus.None))
                            {
                                wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;

                                List<WrkSinKouiDetailModel> wrkDtlGrpItems;
                                wrkDtlGrpItems =
                                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                        p.RaiinNo == wrkDtl.RaiinNo &&
                                        p.HokenKbn == wrkDtl.HokenKbn &&
                                        p.RpNo == wrkDtl.RpNo &&
                                        p.SeqNo == wrkDtl.SeqNo &&
                                        p.ItemSeqNo == wrkDtl.ItemSeqNo);

                                foreach (WrkSinKouiDetailModel wrkDtlGrpItem in wrkDtlGrpItems)
                                {
                                    wrkDtlGrpItem.IsDeleted = DeleteStatus.DeleteFlag;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    // 外来管理加算算定不可項目がない場合
                    List<WrkSinKouiDetailModel> wrkGairaiKanriDtls =
                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                             p.RaiinNo == _common.raiinNo &&
                             p.HokenKbn == _common.hokenKbn &&
                             p.ItemCd == ItemCdConst.GairaiKanriKasanRousai &&
                             p.IsDeleted == DeleteStatus.None);

                    if (wrkGairaiKanriDtls.Any())
                    {                        
                        // 外来管理加算が存在する場合は置き換える
                        wrkGairaiKanriDtls.First().ItemCd = ItemCdConst.GairaiKanriKasan;
                        var gairaiKanriTenMst = _common.Mst.GetTenMst(ItemCdConst.GairaiKanriKasan).FirstOrDefault();
                        if (gairaiKanriTenMst != null)
                        {
                            wrkGairaiKanriDtls.First().TenMst = gairaiKanriTenMst;
                            wrkGairaiKanriDtls.First().ItemName = gairaiKanriTenMst.ReceName;

                            List<WrkSinKouiModel> wrkGairaiKanriKouis =
                                _common.Wrk.wrkSinKouis.FindAll(p =>
                                     p.RaiinNo == _common.raiinNo &&
                                    p.HokenKbn == _common.hokenKbn &&
                                    p.RpNo == wrkGairaiKanriDtls.First().RpNo &&
                                    p.SeqNo == wrkGairaiKanriDtls.First().SeqNo &&
                                    p.IsDeleted == DeleteStatus.None);
                            if (wrkGairaiKanriKouis.Any())
                            {
                                wrkGairaiKanriKouis.First().SyukeiSaki = ReceSyukeisaki.SaisinGairai;
                            }
                        }
                    }
                }
            }

            return yomikaeDtl;
        }

        /// <summary>
        /// コード区分に応じた労災読み替え加算項目を返す
        /// </summary>
        /// <param name="cdKbn">コード区分</param>
        /// <returns></returns>
        private string GetRosaiYomikaeKasan(string cdKbn)
        {
            var rosaiYomikaels = new Dictionary<string, string>()
                                {
                                    {"D", ItemCdConst.RosaiYomikaeKensa },
                                    {"J", ItemCdConst.RosaiYomikaeSyoti },
                                    {"L", ItemCdConst.RosaiYomikaeMasui },
                                    {"H", ItemCdConst.RosaiYomikaeSonota }
                                };

            string addItemCd = "";

            if (rosaiYomikaels.ContainsKey(cdKbn))
            {
                addItemCd = rosaiYomikaels[cdKbn];
            }

            return addItemCd;
        }

        /// <summary>
        /// 消費税計算
        /// </summary>
        /// <param name="jihiSantei">自費算定分の合計金額</param>
        /// <param name="jihiKoumoku">自費項目分の合計金額</param>
        /// <param name="jihiUchizei">内税対象分の合計金額</param>
        /// <param name="jihiSotozei">外税対象分の合計金額</param>
        /// <param name="jihiUchiKeigenzei">内税軽減税率対象分の合計金額</param>
        /// <param name="jihiKeigen">軽減税率対象分の合計金額</param>
        private void Syohizei(double jihiSantei, double jihiKoumoku, double jihiUchizei, double jihiSotozei, double jihiUchiKeigenzei, double jihiKeigen,
            int sotozeiHokenPid, int sotozeiHokenId, int sotogenHokenPid, int sotogenHokenId, int uchizeiHokenPid, int uchizeiHokenId, int uchigenHokenPid, int uchigenHokenId)
        {
            double syouhiZei = 0;
            double keigenZei = 0;
            double uchiZei = 0;
            double uchiKeigenZei = 0;

            if (jihiSotozei > 0)
            {
                // 外税対象金額がある場合、消費税分を求める（金額 * 税率）
                syouhiZei = jihiSotozei * (_common.Mst.GetZei()) / 100;
            }

            if (jihiKeigen > 0)
            {
                if (_common.Mst.GetKeigenZei() > 0)
                {
                    // 軽減税率対象金額がある場合、消費税を求める（金額 * 軽減税率）
                    keigenZei = jihiKeigen * (_common.Mst.GetKeigenZei()) / 100;
                }
                else
                {
                    // 軽減税率の設定がない場合は、外税で処理
                    syouhiZei = jihiKeigen * (_common.Mst.GetZei()) / 100;
                }
            }

            if (jihiUchizei > 0)
            {
                // 内税対象金額がある場合、内税分を求める（金額 - (金額 / (1+税率)) * 100
                uchiZei = jihiUchizei - ((jihiUchizei / (100 + _common.Mst.GetZei())) * 100);
            }

            if (jihiUchiKeigenzei > 0)
            {
                if (_common.Mst.GetKeigenZei() > 0)
                {
                    // 内税軽減税率対象金額がある場合、内税分を求める（金額 - (金額 / (1+税率)) * 100
                    uchiKeigenZei = jihiUchiKeigenzei - ((jihiUchiKeigenzei / (100 + _common.Mst.GetKeigenZei())) * 100);
                }
                else
                {
                    // 軽減税率の設定がない場合は、内税で処理
                    uchiZei = jihiUchiKeigenzei - ((jihiUchiKeigenzei / (100 + _common.Mst.GetZei())) * 100);
                }
            }

            // 自費算定分を求める（自費算定分合計 - 自費分）　
            jihiSantei = jihiSantei - jihiKoumoku;

            if (_common.Mst.GetJihisanteiKazei() == 1)
            {
                if (jihiSantei > 0)
                {
                    //内税はないと考える
                    if (_common.Mst.GetJihisanteiKazei() == 1)
                    {
                        // 課税
                        syouhiZei += jihiSantei * (_common.Mst.GetZei()) / 100;
                    }
                }
            }

            // 端数処理
            syouhiZei = Hasu(syouhiZei);
            keigenZei = Hasu(keigenZei);
            uchiZei = Hasu(uchiZei);
            uchiKeigenZei = Hasu(uchiKeigenZei);

            if (syouhiZei > 0)
            {
                // 消費税追加
                AppendZeiKoumoku(ItemCdConst.Syohizei, syouhiZei, 0, "SZ", 1, sotozeiHokenPid, sotozeiHokenId);
            }

            if (keigenZei > 0)
            {
                // 軽減税追加
                AppendZeiKoumoku(ItemCdConst.Keigenzei, keigenZei, 0, "SZ", 2, sotogenHokenPid, sotogenHokenId);
            }

            if (uchiZei > 0)
            {
                // 内税追加
                AppendZeiKoumoku(ItemCdConst.Uchizei, 0, uchiZei, "UZ", 3, uchizeiHokenPid, uchizeiHokenId);
            }

            if (uchiKeigenZei > 0)
            {
                // 内税軽減税率対象追加
                AppendZeiKoumoku(ItemCdConst.UchiKeigenzei, 0, uchiKeigenZei, "UZ", 4, uchigenHokenPid, uchigenHokenId);
            }
            #region Local Method

            // 端数処理
            double Hasu(double kingaku)
            {
                double ret = 0;

                if (_common.Mst.GetZeiHasu() == 0)
                {
                    // 四捨五入
                    ret = Math.Round(kingaku, MidpointRounding.AwayFromZero);
                }
                else if (_common.Mst.GetZeiHasu() == 1)
                {
                    // 切り捨て
                    ret = Math.Floor(kingaku);
                }
                else if (_common.Mst.GetZeiHasu() == 2)
                {
                    // 切り上げ
                    ret = Math.Ceiling(kingaku);
                }

                return ret;
            }

            // 税項目追加
            void AppendZeiKoumoku(string itemCd, double kingaku, double zei, string cdKbn, int kazeiKbn, int hokenPid, int hokenId)
            {
                _common.Sin.AppendNewSinRpInf(sinKouiKbn: ReceKouiKbn.Jihi, sinId: ReceSinId.Jihi, santeiKbn: SanteiKbnConst.Santei, cdNo: "JS9999999");
                SinKouiModel sinKoui =
                    _common.Sin.AppendNewSinKoui(hokenPid: hokenPid, hokenId: hokenId, syukeiSaki: ReceSyukeisaki.Jihi, cdKbn: cdKbn, kazeiKbn: kazeiKbn);
                sinKoui.Ten = kingaku;
                sinKoui.Zei = zei;
                sinKoui.TotalTen = sinKoui.Ten;
                sinKoui.Count = 1;
                sinKoui.EntenKbn = 1;

                SinKouiDetailModel sinDtl =
                    _common.Sin.AppendNewSinKouiDetail(itemCd);
                sinDtl.Ten = kingaku;
                sinDtl.Zei = zei;

                _common.Sin.AppendNewSinKouiCount(_common.Sin.SinKouis.Last().RpNo, _common.Sin.SinKouis.Last().SeqNo, _common.Sin.SinKouis.Last().KeyNo, _common.Sin.SinKouis.Last().Count);
            }
            #endregion
        }
    }
}
