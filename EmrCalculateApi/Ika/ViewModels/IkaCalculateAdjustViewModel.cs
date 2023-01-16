using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using EmrCalculateApi.Interface;
using PostgreDataContext;
using Domain.Constant;
using Helper.Common;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    class DelSbtConst
    {
        /// <summary>
        /// 包括
        /// </summary>
        public const int Houkatu = 0;
        /// <summary>
        /// 背反
        /// </summary>
        public const int Haihan = 1;
        /// <summary>
        /// 包括特殊
        /// </summary>
        public const int HoukatuTokusyu = 2;
        /// <summary>
        /// 外来管理加算
        /// </summary>
        public const int GairaiKanri = 3;
        /// <summary>
        /// 優先背反
        /// </summary>
        public const int Yuusen = 4;
        /// <summary>
        /// 注加算対象基本項目なし
        /// </summary>
        public const int ChuKasan = 5;
        /// <summary>
        /// 外来管理加算（同一診療）
        /// </summary>
        public const int GairaiDouitu = 6;
        /// <summary>
        /// ある項目が存在しないために算定できない場合
        /// </summary>
        public const int NoExists = 7;
        /// <summary>
        /// ある項目が存在しないために算定できない場合（警告）
        /// </summary>
        public const int NoExistsWarning = 8;
        /// <summary>
        /// 削除項目に付随して削除される項目
        /// </summary>
        public const int Fuzui = 9;

        /// <summary>
        /// 注加算項目で、同一Rp内に対応する基本項目がない項目
        /// </summary>
        public const int ChuKasanWrn = 10;
        /// <summary>
        /// 背反特殊
        /// </summary>
        public const int HaihanTokusyu = 11;
        /// <summary>
        /// 加算基本項目なし
        /// </summary>
        public const int NoKihon = 12;
        /// <summary>
        /// 加算基本項目なし
        /// </summary>
        public const int NoKihonWrn = 13;
        /// <summary>
        /// 自賠・自費以外の保険で自賠文書料を算定
        /// </summary>
        public const int JibaiBunsyo = 14;
        /// <summary>
        /// 注射手技で薬剤がないため算定できない
        /// </summary>
        public const int ChusyaYakuzai = 15;
        /// <summary>
        /// ログ出力なし
        /// </summary>
        public const int NoLog = 99;
        /// <summary>
        /// 算定回数上限
        /// </summary>
        public const int SanteiCount = 100;
        /// <summary>
        /// 年齢チェック
        /// </summary>
        public const int AgeCheck = 101;
        /// <summary>
        /// 初診から
        /// </summary>
        public const int AfterSyosin = 102;
        /// <summary>
        /// 特殊
        /// </summary>
        public const int Special = 9999;
    }

    /// <summary>
    /// 削除項目Rp情報
    /// </summary>
    class DelItemRowInf
    {
        /// <summary>
        /// Rp番号
        /// </summary>
        public int rpNo;
        /// <summary>
        /// 連番
        /// </summary>
        public int seqNo;
        /// <summary>
        /// 行番号
        /// </summary>
        public int rowNo;
        /// <summary>
        /// 項目連番
        /// </summary>
        public int itemSeqNo;
        /// <summary>
        /// レコード識別
        /// </summary>
        public string recId;
        /// <summary>
        /// 点数
        /// </summary>
        public double ten;
        /// <summary>
        /// 保険区分
        /// </summary>
        public int hokenKbn;
        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string itemCd;
        /// <summary>
        /// 自動算定フラグ
        /// </summary>
        public int isAutoAdd;
    }

    /// <summary>
    /// 包括背反処理クラス
    /// </summary>
    class IkaCalculateAdjustViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;
        private List<int> checkHokenKbn;
        private List<int> checkSanteiKbn;

        private readonly TenantDataContext _tenantDataContext;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateAdjustViewModel(IkaCalculateCommonDataViewModel common, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _common = common;

            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;

            checkHokenKbn = CalcUtils.GetCheckHokenKbns(_common.hokenKbn, _systemConfigProvider.GetHokensyuHandling());
            checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(_common.hokenKbn, _systemConfigProvider.GetHokensyuHandling());
        }

        /// <summary>
        /// 包括背反処理
        /// <param name="first">true - 初回チェック（可能性はなし、ログ展開なし）</param>
        /// </summary>
        public void Adjust(bool first)
        {
            const string conFncName = nameof(Adjust);

            _emrLogger.WriteLogStart( this, conFncName, "");

            // 注加算チェック
            TyuKasan();

            // 小児科外来と小児かかりつけと外来腫瘍化学療法診療料による初再診項目の削除（ログに出さない）
            TokusyuSyouniGairaiSyosai();
            TokusyuSyouniKakaritukeSyosai();
            TokusyuGairaiSyuyoSyosai();

            // 小児科外来診療料・かかりつけ診療料と、在宅療養指導管理料
            // ※包括より先に処理する
            TokusyuSyouniGairaiKakarituke();

            // 包括処理
            Houkatu(first);

            // 背反処理
            Haihan(first);

            // 優先順背反
            PriorityHaihan(first);

            // 特殊包括
            Tokusyu(first);

            // 加算項目
            Kasan();

            // 検査判断料調整
            if (first == false)
            {
                Handan();
            }

            // 削除をワークに反映(IS_DELETED)
            WrkDelete();

            if (GairaiKanri())
            {
                // 削除をワークに反映(IS_DELETED)
                WrkDelete();
            }

            if (first == false)
            {

                Saiketu();

                // 最後に検査まるめの処理を行う
                Marume();

                // 逓減項目チェック
                Teigen();

                // 同日再診チェック
                DoujituSaisin();

                // 労災電子化加算
                RousaiDensika();

                // 削除をワークに反映(IS_DELETED)
                WrkDelete();
                // ログに展開する
                OutputLog();
            }

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }

        private void Handan()
        {
            //List<(int[] kbn, string santeiItem)> handanls =
            //    new List<(int[], string)>
            //    {
            //        // 尿・糞便等検査判断料
            //        (new int[]{1}, ItemCdConst.KensaHandanNyou),
            //        // 血液学的検査判断料
            //        (new int[]{2}, ItemCdConst.KensaHandanKetueki),
            //        // 生化学的検査（１）判断料
            //        (new int[]{3}, ItemCdConst.KensaHandanSeika1),
            //        // 生化学的検査（２）判断料
            //        (new int[]{4}, ItemCdConst.KensaHandanSeika2),
            //        // 免疫学的検査判断料
            //        (new int[]{5}, ItemCdConst.KensaHandanMeneki),
            //        // 微生物学的検査判断料
            //        (new int[]{6}, ItemCdConst.KensaHandanBiseibutu),
            //        // 呼吸機能検査等判断料
            //        (new int[]{11}, ItemCdConst.KensaHandanKokyu),
            //        // 脳波検査判断料１
            //        (new int[]{13}, ItemCdConst.KensaHandanNoha1),
            //        // 脳波検査判断料２
            //        (new int[]{13}, ItemCdConst.KensaHandanNoha2),
            //        // 神経・筋検査判断料
            //        (new int[]{14}, ItemCdConst.KensaHandanSinkei),
            //        // ラジオアイソトープ検査判断料
            //        (new int[]{15}, ItemCdConst.KensaHandanRadio),
            //        // 遺伝子関連・染色体検査判断料
            //        (new int[]{17}, ItemCdConst.KensaHandanIdensi),
            //        // 病理判断料
            //        (new int[]{40,41,42}, ItemCdConst.KensaHandanByori)
            //    };

            for (int i = 0; i < KensaHandanConst.KensaHandanList.Count(); i++)
            {
                List<WrkSinKouiDetailModel> handanDtls =
                    _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                    .FindAll(p => p.ItemCd == KensaHandanConst.KensaHandanList[i].santeiItem && p.IsAutoAdd == 1);

                if (handanDtls.Any())
                {
                    // 自動追加された判断料が存在する場合、そのもととなる項目があるかチェックする
                    var wrkDtls =
                        _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly).FindAll(p =>
                            p.ItemCd != KensaHandanConst.KensaHandanList[i].santeiItem &&
                            p.TenMst != null &&
                            KensaHandanConst.KensaHandanList[i].kbn.Contains(p.TenMst.HandanGrpKbn));
                    var wrkDtlDels =
                        _common.Wrk.wrkSinKouiDetailDels.FindAll(p =>
                            p.IsWarning == 0);

                    var _join = (
                        from wrkDtl in wrkDtls
                        join wrkDtlDel in wrkDtlDels on
                            new { rpNo = wrkDtl.RpNo, seqNo = wrkDtl.SeqNo, rowNo = wrkDtl.RowNo } equals
                            new { rpNo = wrkDtlDel.RpNo, seqNo = wrkDtlDel.SeqNo, rowNo = wrkDtlDel.RowNo } into subs
                        from sub in subs.DefaultIfEmpty()
                        select
                            new {
                                wrkDtl,
                                sub
                            }
                        );

                    if (_join.Any(p => p.sub == null) == false)
                    {
                        // 有効な検査がない
                        foreach (WrkSinKouiDetailModel delDtl in handanDtls)
                        {
                            // 削除フラグを立てる
                            delDtl.IsDeleted = DeleteStatus.DeleteFlag;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 注加算チェック
        /// 率加減算（TEN_ID=5,6）の項目は削除
        /// その他の加算は警告
        /// 注加算コードの設定があるのに同一Rpに同じ注加算コードを持つ基本項目が存在しない場合、算定しない
        /// </summary>
        private void TyuKasan()
        {
            List<WrkSinKouiDetailModel> tyukasanDtls =
                _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                    .FindAll(p =>
                        p.TenMst != null &&
                        string.IsNullOrEmpty(p.TenMst.TyuCd.Trim()) == false &&
                        p.TenMst.TyuCd.Trim() != "0" &&
                        p.TenMst.TyuCd.Trim() != "9999" &&
                        string.IsNullOrEmpty(p.TenMst.TyuSeq) == false &&
                        p.TenMst.TyuSeq != "0" &&
                        new List<string> { "7", "9" }.Contains(p.Kokuji1));

            foreach (WrkSinKouiDetailModel chukasanDtl in tyukasanDtls)
            {
                if (_common.Wrk.FindWrkSinKouiDetailByRp(chukasanDtl.RpNo, 0)
                    .Any(p =>
                        p.TenMst != null &&
                        p.TenMst.TyuCd.Trim() == chukasanDtl.TenMst.TyuCd &&
                        new List<string> { "1", "3", "5" }.Contains(p.Kokuji1)) == false)
                {
                    // 同一Rpに加算対象の基本項目が存在しない
                    if (new int[] { 5, 6 }.Contains(chukasanDtl.TenMst.TenId))
                    {
                        // ワーク診療行為詳細削除に追加

                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                (hokenKbn: chukasanDtl.HokenKbn,
                                    rpNo: chukasanDtl.RpNo,
                                    seqNo: chukasanDtl.SeqNo,
                                    rowNo: chukasanDtl.RowNo,
                                    itemCd: chukasanDtl.ItemCd,
                                    delItemCd: "",
                                    santeiDate: 0,
                                    delSbt: DelSbtConst.ChuKasan,
                                    isWarning: 0,
                                    termCnt: 1,
                                    termSbt: 1,
                                    isAutoAdd: chukasanDtl.IsAutoAdd,
                                    hokenId: _common.Wrk.GetWrkKouiHokenId(chukasanDtl.RpNo, chukasanDtl.SeqNo));
                    }
                    else
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                (hokenKbn: chukasanDtl.HokenKbn,
                                    rpNo: chukasanDtl.RpNo,
                                    seqNo: chukasanDtl.SeqNo,
                                    rowNo: chukasanDtl.RowNo,
                                    itemCd: chukasanDtl.ItemCd,
                                    delItemCd: "",
                                    santeiDate: 0,
                                    delSbt: DelSbtConst.ChuKasanWrn,
                                    isWarning: 1,
                                    termCnt: 1,
                                    termSbt: 1,
                                    isAutoAdd: chukasanDtl.IsAutoAdd,
                                    hokenId: _common.Wrk.GetWrkKouiHokenId(chukasanDtl.RpNo, chukasanDtl.SeqNo));
                    }
                }
            }
        }
        /// <summary>
        /// 加算チェック
        /// 率加減算（TEN_ID=5,6）の項目は削除
        /// その他の加算は警告
        /// 同一Rpに基本項目が存在しない場合、警告
        /// </summary>
        private void Kasan()
        {
            List<WrkSinKouiDetailModel> tyukasanDtls =
                _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                    .FindAll(p =>
                        p.TenMst != null &&
                        new List<string> { "7", "9" }.Contains(p.Kokuji1));

            foreach (WrkSinKouiDetailModel chukasanDtl in tyukasanDtls)
            {
                if (_common.Wrk.FindWrkSinKouiDetailByRp(chukasanDtl.RpNo, 0)
                    .Any(p =>
                        p.TenMst != null &&
                        new List<string> { "1", "3", "5" }.Contains(p.Kokuji1)) == false)
                {
                    // 同一Rpに基本項目が存在しない
                    if (new int[] { 5, 6 }.Contains(chukasanDtl.TenMst.TenId))
                    {
                        // ワーク診療行為詳細削除に追加

                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                (hokenKbn: chukasanDtl.HokenKbn,
                                    rpNo: chukasanDtl.RpNo,
                                    seqNo: chukasanDtl.SeqNo,
                                    rowNo: chukasanDtl.RowNo,
                                    itemCd: chukasanDtl.ItemCd,
                                    delItemCd: "",
                                    santeiDate: 0,
                                    delSbt: DelSbtConst.NoKihon,
                                    isWarning: 0,
                                    termCnt: 1,
                                    termSbt: 1,
                                    isAutoAdd: chukasanDtl.IsAutoAdd,
                                    hokenId: _common.Wrk.GetWrkKouiHokenId(chukasanDtl.RpNo, chukasanDtl.SeqNo));
                    }
                    else
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                (hokenKbn: chukasanDtl.HokenKbn,
                                    rpNo: chukasanDtl.RpNo,
                                    seqNo: chukasanDtl.SeqNo,
                                    rowNo: chukasanDtl.RowNo,
                                    itemCd: chukasanDtl.ItemCd,
                                    delItemCd: "",
                                    santeiDate: 0,
                                    delSbt: DelSbtConst.NoKihonWrn,
                                    isWarning: 1,
                                    termCnt: 1,
                                    termSbt: 1,
                                    isAutoAdd: chukasanDtl.IsAutoAdd,
                                    hokenId: _common.Wrk.GetWrkKouiHokenId(chukasanDtl.RpNo, chukasanDtl.SeqNo));
                    }
                }
            }
        }

        /// <summary>
        /// 包括処理        /// 
        /// </summary>
        /// <param name="excludeMaybe">true-可能性なし</param>
        private void Houkatu(bool excludeMaybe)
        {
            const string conFncName = nameof(Houkatu);

            // WRK_SIN_KOUI_DETAIL_DEL.TERM_SBTに記録する値
            // HOUKATU_MSTのTERMは、1:1日につき　2:同一月内　3:同時 99:同一月内（診療日以降）
            int[] termSbts = { 2, 6, 1, 6 };
            const int conHoukatuDay = 1;
            const int conHoukatuMonth = 2;
            const int conHoukatuRaiin = 3;
            const int conHoukatuMonthSin = 99;

            int[] houkatuTerms = { conHoukatuDay, conHoukatuMonth, conHoukatuRaiin, conHoukatuMonthSin };

            _emrLogger.WriteLogStart(this, conFncName, "");

            //電子点数表包括マスタ（すべて）
            List<DensiHoukatuMstModel> densiHoukatuMstAll;

            //ワーク診療情報に登録されている診療行為コードのリストを取得
            //List<WrkSinKouiDetailModel> wrkDtls =
            //    _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly);
            //List<WrkSinKouiDetailModel> wrkDtls = 
            //    _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
            //    .FindAll(p => 
            //        p.TenMst != null && 
            //        (p.TenMst.MasterSbt == "S" || p.TenMst.MasterSbt == "R") && 
            //        (checkSanteiKbn.Contains(_common.Wrk.GetSanteiKbn(p.RpNo)) || 
            //         (_common.Wrk.GetSanteiKbn(p.RpNo) == 2 && p.IsAutoAdd == 1)));
            List<WrkSinKouiDetailModel> wrkDtls =
                _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                .FindAll(p =>
                    p.TenMst != null &&
                    (p.TenMst.MasterSbt == "S" || p.TenMst.MasterSbt == "R"));

            List<string> wrkDtlItemCds =
                wrkDtls.Select(p => p.ItemCd).Distinct().ToList();

            //関連する電子点数表包括マスタを取得
            densiHoukatuMstAll = _common.Mst.GetDensiHoukatu(_common.ptId, wrkDtlItemCds, _common.IsRosai, excludeMaybe);

            if (densiHoukatuMstAll.Any())
            {

                // 算定日情報を取得する
                List<string> allDelItemCds = densiHoukatuMstAll.GroupBy(p => p.DelItemCd).Select(p => p.Key).ToList();

                // とりあえず、1月分
                List<SanteiDaysModel> allSanteiDays = _common.GetSanteiDaysSinYmWithSanteiKbn(allDelItemCds);

                //電子点数表包括マスタ（一部）
                //List<DensiHoukatuMstModel> densiHoukatuMsts;

                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    bool checkDone = false;

                    List<int> checkSanteiKbnsLocal = new List<int>();
                    
                    if (_common.Wrk.GetSanteiKbn(wrkDtl.RaiinNo, wrkDtl.RpNo) == SanteiKbnConst.Jihi)
                    {
                        // 自費分点の場合は自費分点だけ
                        checkSanteiKbnsLocal.Add(SanteiKbnConst.Jihi);
                    }
                    else
                    {
                        checkSanteiKbnsLocal.AddRange(checkSanteiKbn);

                        if(wrkDtl.HokenKbn == HokenKbn.Jihi && checkSanteiKbnsLocal.Any(p=>p == HokenKbn.Jihi) == false)
                        {
                            // 自費保険の場合、自費算定の項目もチェック
                            checkSanteiKbnsLocal.Add(SanteiKbnConst.Jihi);
                        }
                    }

                    // 当該項目に関連する包括マスタが存在するかチェック
                    if (densiHoukatuMstAll.Any(p => p.ItemCd == wrkDtl.ItemCd))
                    {
                        for (int i = 0; i < termSbts.Count(); i++)
                        {
                            int termSbt = termSbts[i];
                            int houkatuTerm = houkatuTerms[i];

                            // 当該項目、期間に関連する包括マスタを抽出
                            List<DensiHoukatuMstModel> densiHoukatuMstTgts =
                                densiHoukatuMstAll.FindAll(p => p.ItemCd == wrkDtl.ItemCd && p.HoukatuTerm == houkatuTerm);

                            // 当該項目、期間に関連する包括マスタが存在するかチェック
                            if (densiHoukatuMstTgts.Any())
                            {
                                // 当該項目を包括する項目の診療行為コードのリスト
                                List<string> delItemCds = densiHoukatuMstTgts.Select(p => p.DelItemCd).ToList();

                                // この来院の中に、自身を包括する診療行為が存在するかチェック
                                List<WrkSinKouiDetailModel> filtredWrkDtls =
                                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                        p.RaiinNo == _common.raiinNo &&
                                        _common.Wrk.ExistWrkSinKouiDetailDel(p) == false &&
                                        //p.HokenKbn == _common.hokenKbn &&
                                        checkHokenKbn.Contains(p.HokenKbn) &&
                                        checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                                        delItemCds.Contains(p.ItemCd) &&
                                        p.IsDummy == false &&
                                        p.IsDeleted == DeleteStatus.None);

                                foreach (WrkSinKouiDetailModel filteredWrkDtl in filtredWrkDtls)
                                {
                                    //if (_common.Wrk.GetSanteiKbn(filteredWrkDtl.RpNo) == SanteiKbnConst.Santei)
                                    if (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(filteredWrkDtl.RaiinNo, filteredWrkDtl.RpNo)))
                                    {
                                        // 算定外は除く
                                        if (_common.Wrk.ExistWrkSinKouiDetailDel(filteredWrkDtl))
                                        {
                                            // 既に登録がある場合は削除された項目なので読み飛ばし
                                            continue;
                                        }

                                        // ワーク診療行為詳細削除に追加
                                        int isWarning = densiHoukatuMstTgts.Find(p => p.DelItemCd == filteredWrkDtl.ItemCd).SpJyoken;
                                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                (hokenKbn: wrkDtl.HokenKbn,
                                                    rpNo: wrkDtl.RpNo,
                                                    seqNo: wrkDtl.SeqNo,
                                                    rowNo: wrkDtl.RowNo,
                                                    itemCd: wrkDtl.ItemCd,
                                                    delItemCd: filteredWrkDtl.ItemCd,
                                                    santeiDate: 0,
                                                    delSbt: DelSbtConst.Houkatu,
                                                    isWarning: isWarning,
                                                    termCnt: 1,
                                                    termSbt: 1,
                                                    isAutoAdd: wrkDtl.IsAutoAdd,
                                                    hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                        if (isWarning == 0)
                                        {
                                            // 削除される項目が削除していた項目があれば、削除しておく
                                            foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                            {
                                                foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                {
                                                    if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                        p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo &&
                                                        !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                    {
                                                        dtl.IsDeleted = 0;

                                                        foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                        {
                                                            koui.IsDeleted = 0;
                                                        }
                                                        foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                        {
                                                            rp.IsDeleted = 0;
                                                        }
                                                    }
                                                }
                                            }
                                            _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);

                                            if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 1)
                                            {
                                                // 最初の1つだけチェックする設定の場合は抜ける
                                                checkDone = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (checkDone)
                                {
                                    // 最初の1つだけチェックする設定の場合で、チェック済みの場合は抜ける
                                    break;
                                }
                                else
                                {
                                    if (new int[] { conHoukatuDay, conHoukatuMonth, conHoukatuMonthSin }.Contains(houkatuTerm))
                                    {
                                        // 同日 or 同月のチェック

                                        // 当日に、自身を包括する診療行為が存在するかチェック(同来院分は先にチェックしているので省く）
                                        if (_common.calcMode != CalcModeConst.Trial)
                                        {
                                            if (houkatuTerm != conHoukatuMonthSin)
                                            {
                                                filtredWrkDtls =
                                                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                                    p.RaiinNo != _common.raiinNo &&
                                                    _common.Wrk.wrkSinKouiDetailDels.Any(d => d.RpNo == p.RpNo && d.SeqNo == p.SeqNo && d.RowNo == p.RowNo && d.IsWarning == 0) == false &&
                                                    //p.HokenKbn == _common.hokenKbn &&
                                                    checkHokenKbn.Contains(p.HokenKbn) &&
                                                    checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                                                    delItemCds.Contains(p.ItemCd) &&
                                                    p.IsDummy == false &&
                                                    p.IsDeleted == DeleteStatus.None);
                                            }
                                            else
                                            {
                                                // 同一月内（診療日以降）の場合、当来院以前を対象とする
                                                filtredWrkDtls =
                                                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                                    p.RaiinNo != _common.raiinNo &&
                                                    ((string.Compare(p.SinStartTime, _common.sinStartTime) < 0) ||
                                                    ((string.Compare(p.SinStartTime, _common.sinStartTime) == 0) && (p.RaiinNo < _common.raiinNo))) &&
                                                    _common.Wrk.wrkSinKouiDetailDels.Any(d => d.RpNo == p.RpNo && d.SeqNo == p.SeqNo && d.RowNo == p.RowNo && d.IsWarning == 0) == false &&
                                                    checkHokenKbn.Contains(p.HokenKbn) &&
                                                    checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                                                    delItemCds.Contains(p.ItemCd) &&
                                                    p.IsDummy == false &&
                                                    p.IsDeleted == DeleteStatus.None);
                                            }

                                            foreach (WrkSinKouiDetailModel filteredWrkDtl in filtredWrkDtls)
                                            {
                                                //if(_common.Wrk.GetSanteiKbn(filteredWrkDtl.RpNo) == SanteiKbnConst.Santei)
                                                if (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(filteredWrkDtl.RaiinNo, filteredWrkDtl.RpNo)))
                                                {
                                                    // 算定外除く

                                                    if (_common.Wrk.ExistWrkSinKouiDetailDel(filteredWrkDtl.RpNo, filteredWrkDtl.ItemCd))
                                                    {
                                                        // 既に登録がある場合は削除された項目なので読み飛ばし
                                                        continue;
                                                    }

                                                    // ワーク診療行為詳細削除に追加
                                                    int isWarning = densiHoukatuMstTgts.Find(p => p.DelItemCd == filteredWrkDtl.ItemCd).SpJyoken;
                                                    _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                            (hokenKbn: wrkDtl.HokenKbn,
                                                                rpNo: wrkDtl.RpNo,
                                                                seqNo: wrkDtl.SeqNo,
                                                                rowNo: wrkDtl.RowNo,
                                                                itemCd: wrkDtl.ItemCd,
                                                                delItemCd: filteredWrkDtl.ItemCd,
                                                                santeiDate: 0,
                                                                delSbt: DelSbtConst.Houkatu,
                                                                isWarning: isWarning,
                                                                termCnt: 1,
                                                                termSbt: 2,
                                                                isAutoAdd: wrkDtl.IsAutoAdd,
                                                                hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                                    if (isWarning == 0)
                                                    {
                                                        // 削除される項目が削除していた項目があれば、削除しておく
                                                        foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                                        {
                                                            foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                            {
                                                                if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                                    p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo &&
                                                                    !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                                {
                                                                    dtl.IsDeleted = 0;

                                                                    foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                    {
                                                                        koui.IsDeleted = 0;
                                                                    }
                                                                    foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                    {
                                                                        rp.IsDeleted = 0;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);

                                                        if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 1)
                                                        {
                                                            // 最初の1つだけチェックする設定の場合は抜ける
                                                            checkDone = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        int startDate = _common.sinDate;
                                        int endDate = _common.sinDate;

                                        if (new int[] { conHoukatuMonth, conHoukatuMonthSin }.Contains(houkatuTerm) ||
                                            (_common.calcMode == CalcModeConst.Trial && houkatuTerm == conHoukatuDay))
                                        {
                                            if (houkatuTerm != conHoukatuDay)
                                            {
                                                // 同月の場合、startDateは1日
                                                startDate = _common.SinFirstDateOfMonth;
                                            }
                                            //endDate = _common.SinLastDateOfMonth;

                                            //}

                                            // 期間内に算定されている対象項目を取得
                                            //List<SanteiDaysModel> santeiDays =
                                            //    _common.GetSanteiDays(startDate, endDate, tgtItemCds);
                                            List<SanteiDaysModel> santeiDays;

                                            if (_common.calcMode == CalcModeConst.Trial && houkatuTerm == conHoukatuDay)
                                            {
                                                // 同日
                                                santeiDays = allSanteiDays.FindAll(p => delItemCds.Contains(p.ItemCd) && p.SinDate == _common.sinDate && checkSanteiKbnsLocal.Contains(p.SanteiKbn));
                                            }
                                            else
                                            {
                                                // 同月
                                                //santeiDays = _common.GetSanteiDaysSinYm(delItemCds);
                                                santeiDays = allSanteiDays.FindAll(p => delItemCds.Contains(p.ItemCd) && checkSanteiKbnsLocal.Contains(p.SanteiKbn));
                                            }
                                            if (new int[] { conHoukatuMonthSin }.Contains(houkatuTerm))
                                            {
                                                santeiDays = santeiDays.FindAll(p => p.SinDate >= startDate && p.SinDate <= endDate);
                                            }


                                            foreach (SanteiDaysModel santeiDay in santeiDays)
                                            {
                                                // ワーク診療行為詳細削除に追加
                                                int isWarning = densiHoukatuMstTgts.Find(p => p.DelItemCd == santeiDay.ItemCd).SpJyoken;
                                                _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                    (hokenKbn: wrkDtl.HokenKbn,
                                                        rpNo: wrkDtl.RpNo,
                                                        seqNo: wrkDtl.SeqNo,
                                                        rowNo: wrkDtl.RowNo,
                                                        itemCd: wrkDtl.ItemCd,
                                                        delItemCd: santeiDay.ItemCd,
                                                        santeiDate: santeiDay.SinDate,
                                                        delSbt: DelSbtConst.Houkatu,
                                                        isWarning: isWarning,
                                                        termCnt: 1,
                                                        termSbt: termSbt,
                                                        isAutoAdd: wrkDtl.IsAutoAdd,
                                                        hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                                if (isWarning == 0)
                                                {
                                                    // 削除される項目が削除していた項目があれば、削除しておく
                                                    foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                                    {
                                                        foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                        {
                                                            if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                                p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo &&
                                                                !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                            {
                                                                dtl.IsDeleted = 0;

                                                                foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                {
                                                                    koui.IsDeleted = 0;
                                                                }
                                                                foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                {
                                                                    rp.IsDeleted = 0;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);

                                                    if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 1)
                                                    {
                                                        // 最初の1つだけチェックする設定の場合は抜ける
                                                        checkDone = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }

        /// <summary>
        /// 背反処理
        /// </summary>
        /// <param name="excludeMaybe">true-可能性なし</param>
        private void Haihan(bool excludeMaybe)
        {
            const string conFncName = nameof(Haihan);

            _emrLogger.WriteLogStart( this, conFncName, "");

            // 電子点数表背反マスタ
            List<DensiHaihanMstModel> densiHaihans = new List<DensiHaihanMstModel>();

            // 今回算定している項目の診療行為コードのリスト
            // ・点数マスタと紐づいている
            // ・マスタ種別がS,Rである or J自費 or Z特材
            // ・算定区分が指定のもの or 算定区分が自費算定で自動算定 or J自費
            //List<WrkSinKouiDetailModel> wrkDtls = _common.Wrk.FindWrkSinKouiDetailHoken();
            //List<WrkSinKouiDetailModel> wrkDtls = 
            //    _common.Wrk.FindWrkSinKouiDetailHoken()
            //    .FindAll(p =>
            //        p.TenMst != null &&
            //        (p.TenMst.MasterSbt == "S" || p.TenMst.MasterSbt == "R" || p.ItemCd.StartsWith("J") || p.OdrItemCd.StartsWith("Z")) &&
            //        (checkSanteiKbn.Contains(_common.Wrk.GetSanteiKbn(p.RpNo)) ||
            //         (_common.Wrk.GetSanteiKbn(p.RpNo) == 2 && p.IsAutoAdd == 1) ||
            //         (string.IsNullOrEmpty(p.ItemCd) == false && p.ItemCd.StartsWith("J"))));
            List<WrkSinKouiDetailModel> wrkDtls =
                _common.Wrk.FindWrkSinKouiDetailHoken()
                .FindAll(p =>
                    p.TenMst != null &&
                    (p.TenMst.MasterSbt == "S" || p.TenMst.MasterSbt == "R" || p.TenMst.MasterSbt == "Y" || p.ItemCd.StartsWith("J") || p.OdrItemCd.StartsWith("Z")));

            // 項目コードをリスト化する
            // ただし、Z特材はOdrItemCdから引っ張るので、下で別に取得する
            List<string> wrkDtlItemCds =
                wrkDtls.FindAll(p => !p.OdrItemCd.StartsWith("Z")).Select(p => p.ItemCd).Distinct().ToList();

            if (wrkDtls.Any(p => p.OdrItemCd.StartsWith("Z")))
            {
                // Z特材（ItemCdは算定用のコードになっているため、OdrItemCdで調べる）を含む場合、
                // OdrItemCdから引っ張る
                wrkDtlItemCds.AddRange(wrkDtls.FindAll(p => p.OdrItemCd.StartsWith("Z")).Select(q => q.OdrItemCd).Distinct().ToList());
            }
            //List<string> wrkDtlItemCds =
            //    wrkDtls.Where(p=>p.TenMst != null && (p.TenMst.MasterSbt == "S" || p.TenMst.MasterSbt == "R")).Select(p => p.ItemCd).Distinct().ToList();

            // 今回算定している項目に関連する電子点数表背反マスタを取得
            List<DensiHaihanMstModel> densiHaihanAll =
                _common.Mst.GetDensiHaihanAll(wrkDtlItemCds, _common.IsRosai, excludeMaybe);

            if (densiHaihanAll.Any())
            {
                // 算定日情報を取得する
                List<string> allHaihanItemCds = densiHaihanAll.GroupBy(p => p.ItemCd2).Select(p => p.Key).ToList();

                // とりあえず、1月分
                List<SanteiDaysModel> allSanteiDays =
                    _common.GetSanteiDaysSinYmWithSanteiKbn(allHaihanItemCds, addJihi: true);

                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    bool checkDone = false;

                    List<int> checkSanteiKbnsLocal = new List<int>();
                    
                    if (_common.Wrk.GetSanteiKbn(wrkDtl.RaiinNo, wrkDtl.RpNo) == SanteiKbnConst.Jihi)
                    {
                        // 自費分点の場合は自費分点だけ
                        checkSanteiKbnsLocal.Add(SanteiKbnConst.Jihi);
                    }
                    else
                    {
                        checkSanteiKbnsLocal.AddRange(checkSanteiKbn);

                        if (wrkDtl.HokenKbn == HokenKbn.Jihi && checkSanteiKbnsLocal.Any(p => p == HokenKbn.Jihi) == false)
                        {
                            // 自費保険の場合、自費算定の項目もチェック
                            checkSanteiKbnsLocal.Add(SanteiKbnConst.Jihi);
                        }
                    }

                    if (densiHaihanAll.Any(p => p.ItemCd1 == wrkDtl.ItemCd || (wrkDtl.OdrItemCd.StartsWith("Z") && p.ItemCd1 == wrkDtl.OdrItemCd)))
                    {
                        /// 0:日  1:月  2:週  3:来院  4:カスタム
                        for (int i = 0; i <= 4; i++)
                        {
                            // 当該項目に関連する電子点数表背反マスタを抽出

                            //densiHaihans = densiHaihanAll.FindAll(p => p.mstSbt == i && p.ItemCd1 == wrkDtl.ItemCd);
                            //if (densiHaihans.Any())
                            //{
                            if (densiHaihanAll.Any(p => p.mstSbt == i && p.ItemCd1 == wrkDtl.ItemCd || (wrkDtl.OdrItemCd.StartsWith("Z") && p.ItemCd1 == wrkDtl.OdrItemCd)))
                            {
                                densiHaihans = densiHaihanAll.FindAll(p => p.mstSbt == i && p.ItemCd1 == wrkDtl.ItemCd || (wrkDtl.OdrItemCd.StartsWith("Z") && p.ItemCd1 == wrkDtl.OdrItemCd));

                                //当該項目を背反する項目の診療行為コードのリストを取得
                                List<string> tgtItemCds = new List<string>();
                                foreach (var data in densiHaihans)
                                {
                                    tgtItemCds.Add(data.ItemCd2);
                                }

                                //まず、同来院をチェック
                                List<WrkSinKouiDetailModel> filtredWrkDtls =
                                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                        p.RaiinNo == _common.raiinNo &&
                                        //p.HokenKbn == _common.hokenKbn &&
                                        _common.Wrk.wrkSinKouiDetailDels.Any(d => d.RpNo == p.RpNo && d.SeqNo == p.SeqNo && d.RowNo == p.RowNo && d.IsWarning == 0) == false &&
                                        checkHokenKbn.Contains(p.HokenKbn) &&
                                        (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) ||
                                           (string.IsNullOrEmpty(p.ItemCd) == false && p.ItemCd.StartsWith("J"))) &&
                                        (tgtItemCds.Contains(p.ItemCd) ||
                                           (string.IsNullOrEmpty(p.ItemCd) == false && p.OdrItemCd.StartsWith("Z") && tgtItemCds.Contains(p.OdrItemCd))) &&
                                        p.IsDummy == false &&
                                        p.IsDeleted == DeleteStatus.None);

                                foreach (WrkSinKouiDetailModel filteredWrkDtl in filtredWrkDtls)
                                {
                                    //if (_common.Wrk.GetSanteiKbn(filteredWrkDtl.RpNo)==SanteiKbnConst.Santei)
                                    if (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(filteredWrkDtl.RaiinNo, filteredWrkDtl.RpNo)) ||
                                        (string.IsNullOrEmpty(filteredWrkDtl.ItemCd) == false &&
                                         filteredWrkDtl.ItemCd.StartsWith("J")))
                                    {
                                        // 算定外除く

                                        // ワーク診療行為詳細削除に追加
                                        DensiHaihanMstModel haihan = densiHaihans.Find(p =>
                                            p.ItemCd2 == filteredWrkDtl.ItemCd ||
                                            (filteredWrkDtl.OdrItemCd.StartsWith("Z") && p.ItemCd2 == filteredWrkDtl.OdrItemCd));

                                        // 警告区分取得
                                        int isWarning = GetIsWarning(haihan.SpJyoken, haihan.HaihanKbn);

                                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                (hokenKbn: wrkDtl.HokenKbn,
                                                 rpNo: wrkDtl.RpNo,
                                                 seqNo: wrkDtl.SeqNo,
                                                 rowNo: wrkDtl.RowNo,
                                                 itemCd: wrkDtl.ItemCd,
                                                 delItemCd: filteredWrkDtl.ItemCd,
                                                 santeiDate: 0,
                                                 delSbt: DelSbtConst.Haihan,
                                                 isWarning: isWarning,
                                                 termCnt: haihan.TermCnt,
                                                 termSbt: haihan.termSbt,
                                                 isAutoAdd: wrkDtl.IsAutoAdd,
                                                 hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                        if (isWarning == 0)
                                        {
                                            // 削除される項目が削除していた項目があれば、削除しておく
                                            foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                            {
                                                foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                {
                                                    if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                        p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo &&
                                                        !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                    {
                                                        dtl.IsDeleted = 0;

                                                        foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                        {
                                                            koui.IsDeleted = 0;
                                                        }
                                                        foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                        {
                                                            rp.IsDeleted = 0;
                                                        }
                                                    }
                                                }
                                            }
                                            _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);

                                            if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 1)
                                            {
                                                // 最初の1つだけチェックする設定の場合は抜ける
                                                checkDone = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (checkDone)
                                {
                                    // 最初の1つだけチェックする設定で、チェック済みの場合は抜ける
                                    break;
                                }
                                else
                                {
                                    // 期間のチェック
                                    if (i <= 2)
                                    {
                                        // 0:同日・1:同月・2:同週の場合

                                        //まず、同日をチェック(当来院分は先にチェックしているので省く）
                                        if (_common.calcMode != CalcModeConst.Trial)
                                        {
                                            //試算の場合は、WRKからではなく、SINから取得
                                            if (i != 1)
                                            {
                                                filtredWrkDtls =
                                                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                                        p.RaiinNo != _common.raiinNo &&
                                                        _common.Wrk.ExistWrkSinKouiDetailDel(p) == false &&
                                                        //p.HokenKbn == _common.hokenKbn &&
                                                        checkHokenKbn.Contains(p.HokenKbn) &&
                                                        (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) ||
                                                           (string.IsNullOrEmpty(p.ItemCd) == false && p.ItemCd.StartsWith("J"))) &&
                                                        (tgtItemCds.Contains(p.ItemCd) ||
                                                           (string.IsNullOrEmpty(p.ItemCd) == false && p.OdrItemCd.StartsWith("Z") && tgtItemCds.Contains(p.OdrItemCd))) &&
                                                        p.IsDummy == false &&
                                                        p.IsDeleted == DeleteStatus.None);
                                            }
                                            else
                                            {
                                                // 同月の場合、当来院以前を対象とする
                                                filtredWrkDtls =
                                                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                                        p.RaiinNo != _common.raiinNo &&
                                                        ((string.Compare(p.SinStartTime, _common.sinStartTime) < 0) ||
                                                        ((string.Compare(p.SinStartTime, _common.sinStartTime) == 0) && (p.RaiinNo < _common.raiinNo))) &&
                                                        _common.Wrk.ExistWrkSinKouiDetailDel(p) == false &&
                                                        checkHokenKbn.Contains(p.HokenKbn) &&
                                                        (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) ||
                                                           (string.IsNullOrEmpty(p.ItemCd) == false && p.ItemCd.StartsWith("J"))) &&
                                                        (tgtItemCds.Contains(p.ItemCd) ||
                                                           (string.IsNullOrEmpty(p.ItemCd) == false && p.OdrItemCd.StartsWith("Z") && tgtItemCds.Contains(p.OdrItemCd))) &&
                                                        p.IsDummy == false &&
                                                        p.IsDeleted == DeleteStatus.None);
                                            }

                                            foreach (WrkSinKouiDetailModel filteredWrkDtl in filtredWrkDtls)
                                            {
                                                //if (_common.Wrk.GetSanteiKbn(filteredWrkDtl.RpNo) == SanteiKbnConst.Santei)
                                                if (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(filteredWrkDtl.RaiinNo, filteredWrkDtl.RpNo)) ||
                                                   (string.IsNullOrEmpty(filteredWrkDtl.ItemCd) == false &&
                                                      filteredWrkDtl.ItemCd.StartsWith("J")))
                                                {
                                                    // 算定外は除く

                                                    // ワーク診療行為詳細削除に追加
                                                    DensiHaihanMstModel haihan = densiHaihans.Find(p =>
                                                      p.ItemCd2 == filteredWrkDtl.ItemCd ||
                                                      (filteredWrkDtl.OdrItemCd.StartsWith("Z") && p.ItemCd2 == filteredWrkDtl.OdrItemCd));

                                                    // 警告区分取得
                                                    int isWarning = GetIsWarning(haihan.SpJyoken, haihan.HaihanKbn);

                                                    _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                            (hokenKbn: wrkDtl.HokenKbn,
                                                             rpNo: wrkDtl.RpNo,
                                                             seqNo: wrkDtl.SeqNo,
                                                             rowNo: wrkDtl.RowNo,
                                                             itemCd: wrkDtl.ItemCd,
                                                             delItemCd: filteredWrkDtl.ItemCd,
                                                             santeiDate: 0,
                                                             delSbt: DelSbtConst.Haihan,
                                                             isWarning: isWarning,
                                                             termCnt: haihan.TermCnt,
                                                             termSbt: haihan.termSbt,
                                                             isAutoAdd: wrkDtl.IsAutoAdd,
                                                             hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                                    if (isWarning == 0)
                                                    {
                                                        // 削除される項目が削除していた項目があれば、削除しておく
                                                        foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                                        {
                                                            foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                            {
                                                                if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                                    p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo &&
                                                                    !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                                {
                                                                    dtl.IsDeleted = 0;

                                                                    foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                    {
                                                                        koui.IsDeleted = 0;
                                                                    }
                                                                    foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                    {
                                                                        rp.IsDeleted = 0;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);

                                                        if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 1)
                                                        {
                                                            // 最初の1つだけチェックする設定の場合は抜ける
                                                            checkDone = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (i != 0 || _common.calcMode == CalcModeConst.Trial)
                                        {
                                            // 日以外、または試算の場合は算定情報をチェック

                                            int startDate = _common.sinDate;
                                            int endDate = _common.sinDate;

                                            if (i == 1)
                                            {
                                                // 同月
                                                startDate = _common.SinFirstDateOfMonth;
                                                //endDate = _common.SinLastDateOfMonth;     // 月末までが正解？
                                            }
                                            else if (i == 2)
                                            {
                                                // 同週
                                                startDate = _common.SinFirstDateOfWeek;
                                                //endDate = _common.SinLastDateOfWeek;      // 週末までが正解？
                                            }

                                            // 期間内に算定されている対象項目を取得
                                            //List<SanteiDaysModel> santeiDays =
                                            //    _common.GetSanteiDays(startDate, endDate, tgtItemCds);
                                            //List<SanteiDaysModel> santeiDays =
                                            //    _common.GetSanteiDaysInSinYM(startDate, endDate, tgtItemCds);
                                            List<SanteiDaysModel> santeiDays = null;

                                            if (startDate / 100 == endDate / 100 && startDate / 100 == _common.sinDate / 100)
                                            {
                                                santeiDays =
                                                    allSanteiDays.FindAll(p =>
                                                        p.SinDate >= startDate && p.SinDate <= endDate && checkSanteiKbnsLocal.Contains(p.SanteiKbn) &&
                                                        (tgtItemCds.Contains(p.ItemCd) || (p.OdrItemCd.StartsWith("Z") && tgtItemCds.Contains(p.OdrItemCd))));
                                            }
                                            else
                                            {
                                                santeiDays =
                                                    _common.GetSanteiDaysWithHaihan(startDate, endDate, tgtItemCds, _common.hokenKbn, true, true, checkSanteiKbnsLocal);
                                            }

                                            //// 当日分をオーダーからも取得する
                                            //List<OdrDtlTenModel> odrDtls = _common.Odr.FilterOdrDetailByItemCdToday(tgtItemCds);
                                            //foreach (OdrDtlTenModel odrDtl in odrDtls)
                                            //{
                                            //    if (santeiDays.Any(p => p.SinDate == odrDtl.SinDate && p.ItemCd == odrDtl.ItemCd) == false)
                                            //    {
                                            //        santeiDays.Add(new SanteiDaysModel(odrDtl.SinDate, odrDtl.ItemCd));
                                            //    }
                                            //}

                                            foreach (SanteiDaysModel santeiDay in santeiDays)
                                            {
                                                // ワーク診療行為詳細削除に追加
                                                DensiHaihanMstModel haihan = densiHaihans.Find(p => p.ItemCd2 == santeiDay.ItemCd);

                                                if (haihan == null)
                                                {
                                                    haihan = densiHaihans.Find(p => p.ItemCd2 == santeiDay.OdrItemCd);
                                                }

                                                // 警告区分取得
                                                int isWarning = GetIsWarning(haihan.SpJyoken, haihan.HaihanKbn);

                                                _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                    (hokenKbn: wrkDtl.HokenKbn,
                                                     rpNo: wrkDtl.RpNo,
                                                     seqNo: wrkDtl.SeqNo,
                                                     rowNo: wrkDtl.RowNo,
                                                     itemCd: wrkDtl.ItemCd,
                                                     delItemCd: santeiDay.ItemCd,
                                                     santeiDate: santeiDay.SinDate,
                                                     delSbt: DelSbtConst.Haihan,
                                                     isWarning: isWarning,
                                                     termCnt: haihan.TermCnt,
                                                     termSbt: haihan.termSbt,
                                                     isAutoAdd: wrkDtl.IsAutoAdd,
                                                     hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                                if (isWarning == 0)
                                                {
                                                    // 削除される項目が削除していた項目があれば、削除しておく
                                                    foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                                    {
                                                        foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                        {
                                                            dtl.IsDeleted = 0;

                                                            if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                                p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo &&
                                                                !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                            {
                                                                foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                {
                                                                    koui.IsDeleted = 0;
                                                                }
                                                                foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                {
                                                                    rp.IsDeleted = 0;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);

                                                    if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 1)
                                                    {
                                                        // 最初の1つだけチェックする設定の場合は抜ける
                                                        checkDone = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (i == 4)
                                    {
                                        // カスタム

                                        foreach (DensiHaihanMstModel densiHaihan in densiHaihans)
                                        {
                                            // 期間を求める
                                            int startDate = -1;
                                            int endDate = _common.sinDate;

                                            switch (densiHaihan.termSbt)
                                            {
                                                case 1: // 来院（処理済み）
                                                    break;
                                                case 2: // 日
                                                    startDate = _common.DaysBefore(_common.sinDate, densiHaihan.TermCnt);
                                                    break;
                                                case 3: // 暦週
                                                    startDate = _common.WeeksBefore(_common.sinDate, densiHaihan.TermCnt);
                                                    endDate = _common.SinLastDateOfWeek;
                                                    break;
                                                case 4: // 暦月
                                                    startDate = _common.MonthsBefore(_common.sinDate, densiHaihan.TermCnt - 1);
                                                    endDate = _common.SinLastDateOfMonth;
                                                    break;
                                                case 5: // 週
                                                    startDate = _common.DaysBefore(_common.sinDate, densiHaihan.TermCnt * 7);
                                                    break;
                                                case 6: // 月
                                                    startDate = _common.MonthsBefore(_common.sinDate, densiHaihan.TermCnt - 1);
                                                    break;
                                                case 7: // 日（前後）
                                                    startDate = _common.DaysBefore(_common.sinDate, densiHaihan.TermCnt);
                                                    break;
                                                case 9: // 患者当たり
                                                    startDate = 0;
                                                    break;
                                            }

                                            if (startDate >= 0)
                                            {
                                                //まず、同日をチェック(当来院分は先にチェックしているので省く）

                                                if (!new int[] { 6, 7 }.Contains(densiHaihan.termSbt))
                                                {
                                                    filtredWrkDtls =
                                                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                                            p.RaiinNo != _common.raiinNo &&
                                                            //p.HokenKbn == _common.hokenKbn &&
                                                            checkHokenKbn.Contains(p.HokenKbn) &&
                                                            (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) ||
                                                             (string.IsNullOrEmpty(p.ItemCd) == false && p.ItemCd.StartsWith("J"))) &&
                                                            (tgtItemCds.Contains(p.ItemCd) ||
                                                             (string.IsNullOrEmpty(p.ItemCd) == false && p.OdrItemCd.StartsWith("Z") && tgtItemCds.Contains(p.OdrItemCd))) &&
                                                            p.IsDummy == false &&
                                                            p.IsDeleted == DeleteStatus.None);
                                                }
                                                else
                                                {
                                                    // 月・日（前後）、の場合、当来院以前を対象とする
                                                    filtredWrkDtls =
                                                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                                            p.RaiinNo != _common.raiinNo &&
                                                            ((string.Compare(p.SinStartTime, _common.sinStartTime) < 0) ||
                                                            ((string.Compare(p.SinStartTime, _common.sinStartTime) == 0) && (p.RaiinNo < _common.raiinNo))) &&
                                                            checkHokenKbn.Contains(p.HokenKbn) &&
                                                            (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) ||
                                                             (string.IsNullOrEmpty(p.ItemCd) == false && p.ItemCd.StartsWith("J"))) &&
                                                            (tgtItemCds.Contains(p.ItemCd) ||
                                                             (string.IsNullOrEmpty(p.ItemCd) == false && p.OdrItemCd.StartsWith("Z") && tgtItemCds.Contains(p.OdrItemCd))) &&
                                                            p.IsDummy == false &&
                                                            p.IsDeleted == DeleteStatus.None);
                                                }

                                                foreach (WrkSinKouiDetailModel filteredWrkDtl in filtredWrkDtls)
                                                {
                                                    //if (_common.Wrk.GetSanteiKbn(filteredWrkDtl.RpNo) == SanteiKbnConst.Santei)
                                                    if (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(filteredWrkDtl.RaiinNo, filteredWrkDtl.RpNo)) ||
                                                        (string.IsNullOrEmpty(filteredWrkDtl.ItemCd) == false &&
                                                         filteredWrkDtl.ItemCd.StartsWith("J")))
                                                    {
                                                        // 算定外は除く

                                                        // ワーク診療行為詳細削除に追加
                                                        DensiHaihanMstModel haihan = densiHaihans.Find(p => p.ItemCd2 == filteredWrkDtl.ItemCd);

                                                        if (haihan == null)
                                                        {
                                                            haihan = densiHaihans.Find(p => p.ItemCd2 == filteredWrkDtl.OdrItemCd);
                                                        }
                                                        // 警告区分取得
                                                        int isWarning = GetIsWarning(haihan.SpJyoken, haihan.HaihanKbn);

                                                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                                (hokenKbn: wrkDtl.HokenKbn,
                                                                 rpNo: wrkDtl.RpNo,
                                                                 seqNo: wrkDtl.SeqNo,
                                                                 rowNo: wrkDtl.RowNo,
                                                                 itemCd: wrkDtl.ItemCd,
                                                                 delItemCd: filteredWrkDtl.ItemCd,
                                                                 santeiDate: 0,
                                                                 delSbt: DelSbtConst.Haihan,
                                                                 isWarning: isWarning,
                                                                 termCnt: haihan.TermCnt,
                                                                 termSbt: haihan.termSbt,
                                                                 isAutoAdd: wrkDtl.IsAutoAdd,
                                                                 hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                                        if (isWarning == 0)
                                                        {
                                                            // 削除される項目が削除していた項目があれば、削除しておく
                                                            foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                                            {
                                                                foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                {
                                                                    dtl.IsDeleted = 0;

                                                                    if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                                        p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd &&
                                                                        !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                                    {
                                                                        foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                        {
                                                                            koui.IsDeleted = 0;
                                                                        }
                                                                        foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                        {
                                                                            rp.IsDeleted = 0;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);

                                                            if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 1)
                                                            {
                                                                // 最初の1つだけチェックする設定の場合は抜ける
                                                                checkDone = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }

                                                if (densiHaihan.termSbt >= 2)
                                                {
                                                    // 期間内に算定されている対象項目を取得
                                                    List<SanteiDaysModel> santeiDays = null;

                                                    if (startDate / 100 == endDate / 100 && startDate / 100 == _common.sinDate / 100)
                                                    {
                                                        santeiDays =
                                                            allSanteiDays.FindAll(p => p.SinDate >= startDate && p.SinDate <= endDate && checkSanteiKbnsLocal.Contains(p.SanteiKbn) &&
                                                            (p.ItemCd == densiHaihan.ItemCd2 || (p.OdrItemCd.StartsWith("Z") && p.OdrItemCd == densiHaihan.ItemCd2)));
                                                    }
                                                    else
                                                    {
                                                        santeiDays =
                                                            _common.GetSanteiDaysWithHaihan(startDate, endDate, densiHaihan.ItemCd2, _common.hokenKbn, true, true, checkSanteiKbnsLocal);
                                                    }

                                                    //// 当日分をオーダーからも取得する
                                                    //List<OdrDtlTenModel> odrDtls = _common.Odr.FilterOdrDetailByItemCdToday(tgtItemCds);
                                                    //foreach (OdrDtlTenModel odrDtl in odrDtls)
                                                    //{
                                                    //    if (santeiDays.Any(p => p.SinDate == odrDtl.SinDate && p.ItemCd == odrDtl.ItemCd) == false)
                                                    //    {
                                                    //        santeiDays.Add(new SanteiDaysModel(odrDtl.SinDate, odrDtl.ItemCd));
                                                    //    }
                                                    //}

                                                    foreach (SanteiDaysModel santeiDay in santeiDays)
                                                    {
                                                        // ワーク診療行為詳細削除に追加
                                                        DensiHaihanMstModel haihan = densiHaihan;

                                                        // 警告区分取得
                                                        int isWarning = GetIsWarning(haihan.SpJyoken, haihan.HaihanKbn);

                                                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                            (hokenKbn: wrkDtl.HokenKbn,
                                                             rpNo: wrkDtl.RpNo,
                                                             seqNo: wrkDtl.SeqNo,
                                                             rowNo: wrkDtl.RowNo,
                                                             itemCd: wrkDtl.ItemCd,
                                                             delItemCd: santeiDay.ItemCd,
                                                             santeiDate: santeiDay.SinDate,
                                                             delSbt: DelSbtConst.Haihan,
                                                             isWarning: isWarning,
                                                             termCnt: haihan.TermCnt,
                                                             termSbt: haihan.termSbt,
                                                             isAutoAdd: wrkDtl.IsAutoAdd,
                                                             hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                                        if (isWarning == 0)
                                                        {
                                                            // 削除される項目が削除していた項目があれば、削除しておく
                                                            foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                                            {
                                                                foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                {
                                                                    if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                                        p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd &&
                                                                        !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                                    {
                                                                        dtl.IsDeleted = 0;

                                                                        foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                        {
                                                                            koui.IsDeleted = 0;
                                                                        }
                                                                        foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                                        {
                                                                            rp.IsDeleted = 0;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);

                                                            if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 1)
                                                            {
                                                                // 最初の1つだけチェックする設定の場合は抜ける
                                                                checkDone = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (checkDone)
                                                    {
                                                        // 最初の1つだけチェックする設定で、チェック済みの場合は抜ける
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (checkDone)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }

        private void PriorityHaihan(bool excludeMaybe)
        {
            const string conFncName = nameof(PriorityHaihan);

            _emrLogger.WriteLogStart(this, conFncName, "");

            // 優先順背反マスタ
            List<PriorityHaihanMstModel> priorityHaihans = new List<PriorityHaihanMstModel>();

            // 今回算定している項目の診療行為コードのリスト
            //List<WrkSinKouiDetailModel> wrkDtls = _common.Wrk.FindWrkSinKouiDetailHoken();
            //List<WrkSinKouiDetailModel> wrkDtls =
            //    _common.Wrk.FindWrkSinKouiDetailHoken()
            //    .FindAll(p =>
            //        p.TenMst != null &&
            //        (p.TenMst.MasterSbt == "S" || p.TenMst.MasterSbt == "R") &&
            //        (checkSanteiKbn.Contains(_common.Wrk.GetSanteiKbn(p.RpNo)) ||
            //         (_common.Wrk.GetSanteiKbn(p.RpNo) == 2 && p.IsAutoAdd == 1)));
            List<WrkSinKouiDetailModel> wrkDtls =
                _common.Wrk.FindWrkSinKouiDetailHoken()
                .FindAll(p =>
                    p.TenMst != null &&
                    (p.TenMst.MasterSbt == "S" || p.TenMst.MasterSbt == "R"));
            List<string> wrkDtlItemCds =
                wrkDtls.Select(p => p.ItemCd).Distinct().ToList();

            // 今回算定している項目に関連する電子点数表背反マスタを取得
            List<PriorityHaihanMstModel> priorityHaihanAll =
                _common.Mst.GetPriorityHaihanAll(wrkDtlItemCds, _common.IsRosai, excludeMaybe);

            for (int userSetting = 2; userSetting >= 1; userSetting--)
            {
                if (priorityHaihanAll.Any(p => p.UserSetting == userSetting))
                {
                    foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                    {
                        bool checkDone = false;

                        List<int> checkSanteiKbnsLocal = new List<int>();
                        if (_common.Wrk.GetSanteiKbn(wrkDtl.RaiinNo, wrkDtl.RpNo) == SanteiKbnConst.Jihi)
                        {
                            // 自費分点の場合は自費分点だけ
                            checkSanteiKbnsLocal.Add(SanteiKbnConst.Jihi);
                        }
                        else
                        {
                            checkSanteiKbnsLocal.AddRange(checkSanteiKbn);

                            if (wrkDtl.HokenKbn == HokenKbn.Jihi && checkSanteiKbnsLocal.Any(p => p == HokenKbn.Jihi) == false)
                            {
                                // 自費保険の場合、自費算定の項目もチェック
                                checkSanteiKbnsLocal.Add(SanteiKbnConst.Jihi);
                            }
                        }

                        if (priorityHaihanAll.Any(p =>
                                (p.ItemCd3 == wrkDtl.ItemCd ||
                                 p.ItemCd4 == wrkDtl.ItemCd ||
                                 p.ItemCd5 == wrkDtl.ItemCd ||
                                 p.ItemCd6 == wrkDtl.ItemCd ||
                                 p.ItemCd7 == wrkDtl.ItemCd ||
                                 p.ItemCd8 == wrkDtl.ItemCd ||
                                 p.ItemCd9 == wrkDtl.ItemCd) &&
                                 p.UserSetting == userSetting))
                        {
                            /// 0:日  1:月  2:週  3:来院  4:カスタム
                            // 当該項目に関連する電子点数表背反マスタを抽出
                            priorityHaihans = priorityHaihanAll.FindAll(p =>
                                (p.ItemCd3 == wrkDtl.ItemCd ||
                                    p.ItemCd4 == wrkDtl.ItemCd ||
                                    p.ItemCd5 == wrkDtl.ItemCd ||
                                    p.ItemCd6 == wrkDtl.ItemCd ||
                                    p.ItemCd7 == wrkDtl.ItemCd ||
                                    p.ItemCd8 == wrkDtl.ItemCd ||
                                    p.ItemCd9 == wrkDtl.ItemCd) &&
                                    p.UserSetting == userSetting)
                                .OrderByDescending(p => p.UserSetting)
                                .ThenBy(p => p.HaihanGrp)
                                .ToList();

                            if (priorityHaihans.Any())
                            {

                                foreach (var data in priorityHaihans)
                                {
                                    //当該項目を背反する項目の診療行為コードのリストを取得
                                    List<string> tgtItemCds = new List<string>();
                                    List<string> _delItemCds = new List<string>();

                                    for (int i = 1; i <= 9; i++)
                                    {

                                        string itemCd = data.ItemCd(i);
                                        if (itemCd != "")
                                        {
                                            if (itemCd == wrkDtl.ItemCd)
                                            {
                                                break;
                                            }
                                            else
                                            {
                                                //チェック
                                                //まず、同来院をチェック
                                                if (
                                                    _common.Wrk.wrkSinKouiDetails.Any(p =>
                                                        p.RaiinNo == _common.raiinNo &&
                                                        //p.HokenKbn == _common.hokenKbn &&
                                                        checkHokenKbn.Contains(p.HokenKbn) &&
                                                        _common.Wrk.ExistWrkSinKouiDetailDel(p) == false &&
                                                        p.ItemCd == itemCd &&
                                                        //_common.Wrk.GetSanteiKbn(p.RpNo) == SanteiKbnConst.Santei
                                                        checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                                                        p.IsDummy == false &&
                                                        p.IsDeleted == DeleteStatus.None
                                                    ))
                                                {
                                                    _delItemCds.Add(itemCd);
                                                }
                                                else
                                                {
                                                    // 期間を求める
                                                    int startDate = -1;
                                                    int endDate = _common.sinDate;

                                                    switch (data.TermSbt)
                                                    {
                                                        case 1: // 来院（処理済み）
                                                            break;
                                                        case 2: // 日
                                                            startDate = _common.DaysBefore(_common.sinDate, data.TermCnt);
                                                            break;
                                                        case 3: // 暦週
                                                            startDate = _common.WeeksBefore(_common.sinDate, data.TermCnt);
                                                            endDate = _common.SinLastDateOfWeek;
                                                            break;
                                                        case 4: // 暦月
                                                            startDate = _common.MonthsBefore(_common.sinDate, data.TermCnt - 1);
                                                            endDate = _common.SinLastDateOfMonth;
                                                            break;
                                                        case 5: // 週
                                                            startDate = _common.DaysBefore(_common.sinDate, data.TermCnt * 7);
                                                            break;
                                                        case 6: // 月
                                                            startDate = _common.MonthsBefore(_common.sinDate, data.TermCnt - 1);
                                                            break;
                                                        case 9: // 患者当たり
                                                            startDate = 0;
                                                            break;
                                                    }

                                                    if (startDate >= 0)
                                                    {
                                                        //まず、同日をチェック(当来院分は先にチェックしているので省く）
                                                        if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                                                                 p.RaiinNo != _common.raiinNo &&
                                                                 //p.HokenKbn == _common.hokenKbn &&
                                                                 checkHokenKbn.Contains(p.HokenKbn) &&
                                                                 p.ItemCd == itemCd &&
                                                                 //_common.Wrk.GetSanteiKbn(p.RpNo) == SanteiKbnConst.Santei
                                                                 checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                                                                 p.IsDummy == false &&
                                                                 p.IsDeleted == DeleteStatus.None
                                                                 ))
                                                        {
                                                            _delItemCds.Add(itemCd);
                                                        }
                                                        else if (data.TermSbt >= 3 ||
                                                            (_common.calcMode == CalcModeConst.Trial && data.TermSbt == 2))
                                                        {
                                                            // 期間内に算定されている対象項目を取得
                                                            List<SanteiDaysModel> santeiDays =
                                                                _common.GetSanteiDays(startDate, endDate, itemCd, _common.hokenKbn, true, santeiKbns: checkSanteiKbnsLocal);

                                                            // 当日分をオーダーからも取得する
                                                            List<OdrDtlTenModel> odrDtls = _common.Odr.FilterOdrDetailByItemCdToday(itemCd);
                                                            foreach (OdrDtlTenModel odrDtl in odrDtls)
                                                            {
                                                                if (santeiDays.Any(p => p.SinDate == odrDtl.SinDate && p.ItemCd == odrDtl.ItemCd) == false)
                                                                {
                                                                    santeiDays.Add(new SanteiDaysModel(odrDtl.SinDate, odrDtl.ItemCd));
                                                                }
                                                            }

                                                            if (santeiDays.Any())
                                                            {
                                                                _delItemCds.Add(itemCd);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (_delItemCds.Count() >= data.Count)
                                        {
                                            // 警告区分取得
                                            int isWarning = data.SpJyoken;

                                            _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                (hokenKbn: wrkDtl.HokenKbn,
                                                 rpNo: wrkDtl.RpNo,
                                                 seqNo: wrkDtl.SeqNo,
                                                 rowNo: wrkDtl.RowNo,
                                                 itemCd: wrkDtl.ItemCd,
                                                 delItemCd: "",
                                                 santeiDate: 0,
                                                 delSbt: DelSbtConst.Yuusen,
                                                 isWarning: isWarning,
                                                 termCnt: data.TermCnt,
                                                 termSbt: data.TermSbt,
                                                 isAutoAdd: wrkDtl.IsAutoAdd,
                                                 hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo),
                                                 delItemCds: _delItemCds);

                                            if (isWarning == 0)
                                            {
                                                // 削除される項目が削除していた項目があれば、削除しておく
                                                foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog))
                                                {
                                                    foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                    {
                                                        if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                            p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd &&
                                                            !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                        {
                                                            dtl.IsDeleted = 0;

                                                            foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                            {
                                                                koui.IsDeleted = 0;
                                                            }
                                                            foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                            {
                                                                rp.IsDeleted = 0;
                                                            }
                                                        }
                                                    }
                                                }
                                                _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && p.TermSbt == 1 && p.DelSbt != DelSbtConst.NoLog);
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            _emrLogger.WriteLogEnd(this, conFncName, "");
        }

        /// <summary>
        /// SP_JYOKENとHAIHAN_KBNからIS_WARNINGを求める
        /// SP_JYOKEN = 0 -> 0:削除
        /// SP_JYOKEN = 1 -> 1:警告
        /// HAIHAN_KBN = 3 -> 2:いずれか一方
        /// HAIHAN_KBN = 3 -> 3:いずれか一方(警告)
        /// </summary>
        /// <param name="spJyoken"></param>
        /// <param name="haihanKbn"></param>
        /// <returns></returns>
        private int GetIsWarning(int spJyoken, int haihanKbn)
        {
            int ret = spJyoken;
            if (haihanKbn == 3)
            {
                ret += 2;
            }
            return ret;
        }

        /// <summary>
        /// 特殊な包括処理
        /// </summary>
        private void Tokusyu(bool excludeMaybe)
        {
            const string conFncName = nameof(Tokusyu);

            _emrLogger.WriteLogStart(this, conFncName, "");

            // 小児科外来診療料
            TokusyuSyouniGairai();

            // 小児かかりつけ診療料
            TokusyuSyouniKakarituke();

            // 地域包括診療料
            TokusyuTiiki();

            // 認知症地域包括診療料
            TokusyuNintiTiiki();

            // 生活習慣病
            TokusyuSeikatu();

            // 在がん医総
            TokusyuZaigan();

            // 在医総、施医総
            TokusyuZaiiso();

            // 在宅患者訪問点滴
            TokusyuZaiKanHoumonTenteki();

            // 在宅患者訪問診療料・訪問看護指示料
            TokusyuZaiHoumon();

            // 慢性疼痛疾患管理料
            TokusyuMaiseiToTu();

            // 慢性維持透析
            TokusyuManeiIjiToseki();

            // 外来管理加算
            //GairaiKanri();

            // 情報通信機器等
            if (excludeMaybe == false && _common.sinDate <= 20220331)
            {
                Online();
            }

            // 外来感染対策向上加算
            TokusyuKansenKojo();

            // 連携強化加算
            TokusyuRenkeiKyoka();

            // サーベイランス強化加算
            TokusyuSurveillance();

            // 初診から一ヶ月
            TokusyuSyosin();

            TokusyuJibaiBunsyo();

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 小児科外来診療料、小児かかりつけ診療料を包括する在宅項目チェック
        /// </summary>
        private void TokusyuSyouniGairaiKakarituke()
        {
            // 在宅患者訪問診療料、および、在宅療養指導管理料を取得する
            List<string> checkItemCds = _common.Mst.GetZaitakuRyoyoList();

            List<string> delItemCds =
                new List<string>
                {
                    //小児科外来診療料（処方箋を交付）初診時   113003510
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                    //小児科外来診療料（処方箋を交付）再診時   113003610
                    ItemCdConst.IgakuSyouniGairaiSaisinKofuAri,
                    //小児科外来診療料（処方箋を交付しない）初診時    113003710   
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi,
                    //小児科外来診療料（処方箋を交付しない）再診時    113003810
                    ItemCdConst.IgakuSyouniGairaiSaisinKofuNasi,
                    //乳幼児時間外加算（初診）（小児科外来診療料）113009670
                    ItemCdConst.IgakuSyouniGairaiSyosinJikangai,
                    //乳幼児時間外加算（再診）（小児科外来診療料）113009770                    
                    ItemCdConst.IgakuSyouniGairaiSaisinJikangai,
                    //機能強化加算（初診）（小児科外来診療料）  113028870
                    ItemCdConst.IgakuSyouniGairaiKinoKyoka,
                    //乳幼児時間外特例医療機関加算（初診）（小児科外来診療料）111010770
                    ItemCdConst.IgakuSyouniGairaiSyosinJikangaiToku,
                    //乳幼児時間外特例医療機関加算（再診）（小児科外来診療料）112006070
                    ItemCdConst.IgakuSyouniGairaiSaisinJikangaiToku,
                    //乳幼児夜間加算（小児科初診）（小児科外来診療料）113007070
                    ItemCdConst.IgakuSyouniGairaiSyosinSyouniYakan,
                    //乳幼児休日加算（小児科初診）（小児科外来診療料）113007170
                    ItemCdConst.IgakuSyouniGairaiSyosinSyouniKyujitu,
                    //乳幼児深夜加算（小児科初診）（小児科外来診療料）113007270
                    ItemCdConst.IgakuSyouniGairaiSyosinSyouniSinya,
                    //乳幼児夜間加算（小児科再診）（小児科外来診療料）113007370
                    ItemCdConst.IgakuSyouniGairaiSaisinSyouniYakan,
                    //乳幼児休日加算（小児科再診）（小児科外来診療料）113007470
                    ItemCdConst.IgakuSyouniGairaiSaisinSyouniKyujitu,
                    //乳幼児深夜加算（小児科再診）（小児科外来診療料）113007570
                    ItemCdConst.IgakuSyouniGairaiSaisinSyouniSinya,
                    //小児抗菌薬適正使用支援加算（小児科外来診療料）113024670
                    ItemCdConst.IgakuSyouniGairaiKokin,
                    //小児かかりつけ診療料（処方箋を交付）初診時 113019710
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                    //小児かかりつけ診療料（処方箋を交付）再診時 113019810
                    ItemCdConst.IgakuSyouniKakaritukeSaisinKofuAri,
                    //小児かかりつけ診療料（処方箋を交付しない）初診時  113019910
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                    //小児かかりつけ診療料（処方箋を交付しない）再診時  113020010
                    ItemCdConst.IgakuSyouniKakaritukeSaisinKofuNasi,
                    //小児かかりつけ診療料１（処方箋を交付）初診時 113037210
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                    //小児かかりつけ診療料１（処方箋を交付）再診時 113037310
                    ItemCdConst.IgakuSyouniKakarituke1SaisinKofuAri,
                    //小児かかりつけ診療料１（処方箋を交付しない）初診時  113037410
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                    //小児かかりつけ診療料１（処方箋を交付しない）再診時  113037510
                    ItemCdConst.IgakuSyouniKakarituke1SaisinKofuNasi,
                    //小児かかりつけ診療料２（処方箋を交付）初診時 113037610
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                    //小児かかりつけ診療料２（処方箋を交付）再診時 113037710
                    ItemCdConst.IgakuSyouniKakarituke2SaisinKofuAri,
                    //小児かかりつけ診療料２（処方箋を交付しない）初診時  113037810
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi,
                    //小児かかりつけ診療料２（処方箋を交付しない）再診時  113037910
                    ItemCdConst.IgakuSyouniKakarituke2SaisinKofuNasi,
                    //乳幼児時間外加算（初診）（小児かかりつけ診療料）  113020170
                    ItemCdConst.IgakuSyouniKakaritukeSyosinNyuJikangai,
                    //乳幼児時間外特例医療機関加算（初診）（小児かかりつけ診療料）113020470
                    ItemCdConst.IgakuSyouniKakaritukeSyosinNyuJikangaiToku,
                    //乳幼児夜間加算（小児科初診）（小児かかりつけ診療料）113020570
                    ItemCdConst.IgakuSyouniKakaritukeSyosinNyuYakan,
                    //乳幼児休日加算（小児科初診）（小児かかりつけ診療料）113020670
                    ItemCdConst.IgakuSyouniKakaritukeSyosinNyuKyujitu,
                    //乳幼児深夜加算（小児科初診）（小児かかりつけ診療料）113020770
                    ItemCdConst.IgakuSyouniKakaritukeSyosinNyuSinya,
                    //乳幼児時間外加算（再診）（小児かかりつけ診療料）  113020870
                    ItemCdConst.IgakuSyouniKakaritukeSaisinNyuJikangai,
                    //乳幼児時間外特例医療機関加算（再診）（小児かかりつけ診療料）113021170
                    ItemCdConst.IgakuSyouniKakaritukeSaisinNyuJikangaiToku,
                    //乳幼児夜間加算（小児科再診）（小児かかりつけ診療料）    113021270
                    ItemCdConst.IgakuSyouniKakaritukeSaisinNyuYakan,
                    //乳幼児休日加算（小児科再診）（小児かかりつけ診療料）    113021370
                    ItemCdConst.IgakuSyouniKakaritukeSaisinNyuKyujitu,
                    //乳幼児深夜加算（小児科再診）（小児かかりつけ診療料）    113021470
                    ItemCdConst.IgakuSyouniKakaritukeSaisinNyuSinya,
                    //時間外加算（初診）（小児かかりつけ診療料） 113026670
                    ItemCdConst.IgakuSyouniKakaritukeSyosinJikangai,
                    //休日加算（初診）（小児かかりつけ診療料）  113026770
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKyujitu,
                    //深夜加算（初診）（小児かかりつけ診療料）  113026870
                    ItemCdConst.IgakuSyouniKakaritukeSyosinSinya,
                    //時間外特例医療機関加算（初診）（小児かかりつけ診療料）   113026970
                    ItemCdConst.IgakuSyouniKakaritukeSyosinJikangaiToku,
                    //時間外加算（再診）（小児かかりつけ診療料）             113027070
                    ItemCdConst.IgakuSyouniKakaritukeSaisinJikangai,
                    //休日加算（再診）（小児かかりつけ診療料）              113027170
                    ItemCdConst.IgakuSyouniKakaritukeSaisinKyujitu,
                    //深夜加算（再診）（小児かかりつけ診療料）              113027270
                    ItemCdConst.IgakuSyouniKakaritukeSaisinSinya,
                    //時間外特例医療機関加算（再診）（小児かかりつけ診療料）   113027370
                    ItemCdConst.IgakuSyouniKakaritukeSaisinJikangaiToku,
                    //機能強化加算（初診）（小児かかりつけ診療料）    113028970
                    ItemCdConst.IgakuSyouniKakaritukeKinoKyoka,
                    //小児抗菌薬適正使用支援加算（小児かかりつけ診療料） 113027870
                    ItemCdConst.IgakuSyouniKakaritukeKokin
                };

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: null,
                delItemCds: delItemCds,
                delCdKbns: null,
                excludeItemCds: null,
                checkTerm: 1,
                delSbt: DelSbtConst.HaihanTokusyu);
        }

        /// <summary>
        /// 小児科外来診療料
        /// </summary>
        private void TokusyuSyouniGairai()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniGairaiSaisinKofuAri,
                    ItemCdConst.IgakuSyouniGairaiSaisinKofuNasi
                };

            // 投薬、注射、処置、手術、検査、画像、その他 包括
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.TouyakuMin, ReceSinId.SonotaMax, 0, false, false, false, false)
                };

            // SARS-Cov関連項目及びその判断料は包括しない
            List<string> excludeItemCds = GetSARSCovItemList();

            // 救急医療管理加算１（診療報酬上臨時的取扱）は、別に算定できない、となってないので算定できる可能性があるらしい
            excludeItemCds.AddRange(
                new List<string>
                {
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinji,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovOhsin,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiOhsinCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovGairai,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiGairaiCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiNyuyojiKasan,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiSyoniKasan
                }
                );
            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: null,
                delCdKbns: null,
                excludeItemCds: excludeItemCds,
                checkTerm: 1);
        }

        /// <summary>
        /// 小児かかりつけ診療料
        /// </summary>
        private void TokusyuSyouniKakarituke()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakaritukeSaisinKofuAri,
                    ItemCdConst.IgakuSyouniKakaritukeSaisinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke1SaisinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke1SaisinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke2SaisinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke2SaisinKofuNasi
                };

            // 投薬、注射、処置、手術、検査、画像、その他 包括
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.TouyakuMin, ReceSinId.SonotaMax, 0, false, false, false, false)
                };

            // SARS-Cov関連項目及びその判断料は包括しない
            List<string> excludeItemCds = GetSARSCovItemList();
            // 救急医療管理加算１（診療報酬上臨時的取扱）は、別に算定できない、となってないので算定できる可能性があるらしい
            excludeItemCds.AddRange(
                new List<string>
                {
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinji,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovOhsin,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiOhsinCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovGairai,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiGairaiCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiNyuyojiKasan,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiSyoniKasan
                }
                );

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: null,
                delCdKbns: null,
                excludeItemCds: excludeItemCds,
                checkTerm: 1);
        }
        /// <summary>
        /// 小児科外来診療料（初再診）
        /// </summary>
        private void TokusyuSyouniGairaiSyosai()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniGairaiSaisinKofuAri,
                    ItemCdConst.IgakuSyouniGairaiSaisinKofuNasi
                };

            // 初再診
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.Syosin, ReceSinId.Saisin, 0, false, false, false, false)
                };

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: null,
                delCdKbns: null,
                excludeItemCds: null,
                checkTerm: 1,
                delSbt: DelSbtConst.NoLog);
        }

        /// <summary>
        /// 小児かかりつけ診療料（初再診）
        /// </summary>
        private void TokusyuSyouniKakaritukeSyosai()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakaritukeSaisinKofuAri,
                    ItemCdConst.IgakuSyouniKakaritukeSaisinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke1SaisinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke1SaisinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke2SaisinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke2SaisinKofuNasi
                };

            // 初再診
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.Syosin, ReceSinId.Saisin, 0, false, false, false, false)
                };

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: null,
                delCdKbns: null,
                excludeItemCds: null,
                checkTerm: 1,
                delSbt: DelSbtConst.NoLog);
        }

        /// <summary>
        /// 外来腫瘍化学療法診療料（初再診）
        /// </summary>
        private void TokusyuGairaiSyuyoSyosai()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuGairaiSyuyo1,
                    ItemCdConst.IgakuGairaiSyuyo1Sonota,
                    ItemCdConst.IgakuGairaiSyuyo2,
                    ItemCdConst.IgakuGairaiSyuyo2Sonota
                };

            // 初再診
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.Syosin, ReceSinId.Saisin, 0, false, false, false, false)
                };

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: null,
                delCdKbns: null,
                excludeItemCds: null,
                checkTerm: 0,
                delSbt: DelSbtConst.NoLog);
        }
        /// <summary>
        /// 地域包括診療料
        /// </summary>
        private void TokusyuTiiki()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuTiikiHoukatu1,
                    ItemCdConst.IgakuTiikiHoukatu2
                };

            //注射、処置、手術、検査、画像、その他
            //※検査、画像は急性増悪の場合、550点未満のもののみ包括
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>();

            if (_common.Odr.odrDtlls.Any(p =>
                     p.RaiinNo == _common.raiinNo &&
                     p.ItemCd == ItemCdConst.KyuseiZoaku
                    ))
            {
                // 急性増悪
                delKoui =
                    new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                    {
                        (ReceSinId.ChusyaMin, ReceSinId.SyujyutuMax, 0, false, false, false, false),
                        (ReceSinId.SonotaMin, ReceSinId.SonotaMax, 0, false, false, false, false),
                        (ReceSinId.KensaMin, ReceSinId.KensaMax, 550, true, false, false, false),
                        (ReceSinId.GazoMin, ReceSinId.GazoMax, 550, true, false, false, false)
                    };
            }
            else
            {
                delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.ChusyaMin, ReceSinId.SyujyutuMax, 0, true, false, false, false),
                    (ReceSinId.SonotaMin, ReceSinId.SonotaMax, 0, true, false, false, false),
                    (ReceSinId.KensaMin, ReceSinId.KensaMax, 0, true, false, false, false),
                    (ReceSinId.GazoMin, ReceSinId.GazoMax, 0, true, false, false, false)
                };
            }

            List<string> delItemCds = new List<string>
            {
                // 乳幼児感染予防策加算（小児科外来診療料等・診療報酬上臨時的取扱）
                ItemCdConst.IgakuNyuyojiKansen
            };

            // SARS-Cov関連項目及びその判断料は包括しない
            List<string> excludeItemCds = GetSARSCovItemList();
            // 救急医療管理加算１（診療報酬上臨時的取扱）は、別に算定できない、となってないので算定できる可能性があるらしい
            excludeItemCds.AddRange(
                new List<string>
                {
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinji,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovOhsin,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiOhsinCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovGairai,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiGairaiCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiNyuyojiKasan,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiSyoniKasan
                }
                );
            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: delItemCds,
                delCdKbns: null,
                excludeItemCds: excludeItemCds,
                checkTerm: 2);
        }

        /// <summary>
        /// 認知症地域包括診療料
        /// </summary>
        private void TokusyuNintiTiiki()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuNintiTiikiHoukatu1,
                    ItemCdConst.IgakuNintiTiikiHoukatu2
                };

            //注射、処置、手術、検査、画像、その他
            //※検査、画像は急性増悪の場合、550点未満のもののみ包括
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>();

            if (_common.Odr.odrDtlls.Any(p =>
                     p.RaiinNo == _common.raiinNo &&
                     p.ItemCd == ItemCdConst.KyuseiZoaku
                    ))
            {
                // 急性増悪
                delKoui =
                    new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                    {
                        (ReceSinId.ChusyaMin, ReceSinId.SyujyutuMax, 0, true, false, false, false),
                        (ReceSinId.SonotaMin, ReceSinId.SonotaMax, 0, true, false, false, false),
                        (ReceSinId.KensaMin, ReceSinId.KensaMax, 550, true, false, false, false),
                        (ReceSinId.GazoMin, ReceSinId.GazoMax, 550, true, false, false, false)
                    };
            }
            else
            {
                delKoui =
                    new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                    {
                        (ReceSinId.ChusyaMin, ReceSinId.SyujyutuMax, 0, true, false, false, false),
                        (ReceSinId.SonotaMin, ReceSinId.SonotaMax, 0, true, false, false, false),
                        (ReceSinId.KensaMin, ReceSinId.KensaMax, 0, true, false, false, false),
                        (ReceSinId.GazoMin, ReceSinId.GazoMax, 0, true, false, false, false)
                    };
            }

            List<string> delItemCds = new List<string>
            {
                // 乳幼児感染予防策加算（小児科外来診療料等・診療報酬上臨時的取扱）
                ItemCdConst.IgakuNyuyojiKansen
            };

            // SARS-Cov関連項目及びその判断料は包括しない
            List<string> excludeItemCds = GetSARSCovItemList();
            // 救急医療管理加算１（診療報酬上臨時的取扱）は、別に算定できない、となってないので算定できる可能性があるらしい
            excludeItemCds.AddRange(
                new List<string>
                {
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinji,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovOhsin,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiOhsinCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovGairai,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiGairaiCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiNyuyojiKasan,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiSyoniKasan
                }
                );
            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: delItemCds,
                delCdKbns: null,
                excludeItemCds: excludeItemCds,
                checkTerm: 2);
        }

        /// <summary>
        /// 生活習慣病管理料
        /// </summary>
        private void TokusyuSeikatu()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuSeikatuKofuAriKouketuatu,
                    ItemCdConst.IgakuSeikatuKofuAriSisitu,
                    ItemCdConst.IgakuSeikatuKofuAriTounyou,
                    ItemCdConst.IgakuSeikatuKofuNasiKouketuatu,
                    ItemCdConst.IgakuSeikatuKofuNasiSisitu,
                    ItemCdConst.IgakuSeikatuKofuNasiTounyou,
                    ItemCdConst.IgakuSeikatuKouketuatu,
                    ItemCdConst.IgakuSeikatuSisitu,
                    ItemCdConst.IgakuSeikatuTounyou
                };

            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui;
            List<string> delCdKbn = new List<string> { };

            if (_common.sinDate < 20220401)
            {
                // 投薬、注射、検査 包括
                delKoui =
                    new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                    {
                    (ReceSinId.TouyakuMin, ReceSinId.ChusyaMax, 0, false, false, false, false),
                    (ReceSinId.KensaMin, ReceSinId.KensaMax, 0, false, false, false, false)
                    };

                // 投薬（その他に入ることもあるので）
                delCdKbn.Add("F");
            }
            else
            {
                //2022/4/1～　投薬を包括対象外にする
                // 注射、検査 包括
                delKoui =
                    new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                    {
                    (ReceSinId.ChusyaMin, ReceSinId.ChusyaMax, 0, false, false, false, false),
                    (ReceSinId.KensaMin, ReceSinId.KensaMax, 0, false, false, false, false)
                    };
            }

            List<string> delItemCds = new List<string>
            {
                // 乳幼児感染予防策加算（小児科外来診療料等・診療報酬上臨時的取扱）
                ItemCdConst.IgakuNyuyojiKansen
            };

            // 削除対象外項目
            List<string> excludeItemCds = new List<string>
            {
                    // 院内トリアージ実施料（診療報酬上臨時的取扱）
                    ItemCdConst.IgakuTriageRinsyo,
            };

            // SARS-Cov関連項目及びその判断料は包括しない
            excludeItemCds.AddRange(GetSARSCovItemList());
            // 救急医療管理加算１（診療報酬上臨時的取扱）は、別に算定できない、となってないので算定できる可能性があるらしい
            excludeItemCds.AddRange(
                new List<string>
                {
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinji,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovOhsin,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiOhsinCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovGairai,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiGairaiCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiNyuyojiKasan,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiSyoniKasan
                }
                );

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: delItemCds,
                delCdKbns: delCdKbn,
                excludeItemCds: excludeItemCds,
                checkTerm: 3);

        }

        /// <summary>
        /// 在がん医総
        /// </summary>
        private void TokusyuZaigan()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.ZaiZaiganByoAriSyohoAri,
                    ItemCdConst.ZaiZaiganByoAriSyohoNasi,
                    ItemCdConst.ZaiZaiganByoNasiSyohoAri,
                    ItemCdConst.ZaiZaiganByoNasiSyohoNasi,
                    ItemCdConst.ZaiZaiganSyohoAri,
                    ItemCdConst.ZaiZaiganSyohoNasi
                };

            // 在宅行為以外 包括
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.SyosaiMin, ReceSinId.IgakuMax, 0, false, false, false, false),
                    (ReceSinId.TouyakuMin, ReceSinId.SonotaMax, 0, false, false, false, false)
                };

            // 削除対象外項目
            List<string> excludeItemCds =
                new List<string>
                {
                    // 在がん医総自身
                    //ItemCdConst.ZaiZaiganByoAriSyohoAri,
                    //ItemCdConst.ZaiZaiganByoAriSyohoNasi,
                    //ItemCdConst.ZaiZaiganByoNasiSyohoAri,
                    //ItemCdConst.ZaiZaiganByoNasiSyohoNasi,
                    //ItemCdConst.ZaiZaiganSyohoAri,
                    //ItemCdConst.ZaiZaiganSyohoNasi,
                    // ダミー
                    ItemCdConst.JituNissuCount,
                    ItemCdConst.JituNissuCountCancel,
                    ItemCdConst.HoumonCommentCancel,
                    // 院内トリアージ実施料（診療報酬上臨時的取扱）
                    ItemCdConst.IgakuTriageRinsyo,
                    ItemCdConst.Igaku2RuiKansen,
                    ItemCdConst.Igaku2RuiKansenJyuten,
                 //   // 死亡診断
                 //   ItemCdConst.SiboSindanSyoHoumon,
                 //   ItemCdConst.SiboSindanSyoZaigan,
                 //   // 在宅ターミナルケア加算
                 //   ItemCdConst.ZaiTerminalRoZai,
                 //   ItemCdConst.ZaiTerminalRoZaiGai,
                 //   ItemCdConst.ZaiTerminalRoKyokaAri,
                 //   ItemCdConst.ZaiTerminalRoKyokaNasi,
                 //   ItemCdConst.ZaiTerminalIKyokaAri,
                 //   ItemCdConst.ZaiTerminalIKyokaNasi,
                 //   ItemCdConst.ZaiTerminalIZai,
                 //   ItemCdConst.ZaiTerminalIZaiGai,
                 //   ItemCdConst.ZaiTerminal2ZaiGai,
                 //   ItemCdConst.ZaiTerminal2KyokaAri,
                 //   ItemCdConst.ZaiTerminal2KyokaNasi,
                 //   ItemCdConst.ZaiTerminal2Zai,
                 //   ItemCdConst.ZaiTerminalSonota,
                 //   ItemCdConst.ZaiTerminalTokuyo,
                 //   // 酸素療法加算
                 //   ItemCdConst.ZaiSansoRyohoKasan1,
                 //   ItemCdConst.ZaiSansoRyohoKasan2,
                 //   // 看取り加算
                 //   ItemCdConst.ZaiMitoriKasan,
                 //   // 訪問看護指示料
                 //   ItemCdConst.ZaiHoumonKango,
                 //   // 在宅緩和ケア充実診療所・病院加算（在がん医総）
                 //   ItemCdConst.ZaiKanwaCareZaigan,
                 //   // 在宅緩和ケア充実診療所・病院加算（施医総管）（１０人～）	
                 //   ItemCdConst.ZaiKanwaCareSiiso10_,
                 //   // 在宅緩和ケア充実診療所・病院加算（施医総管）（１人）	
                 //   ItemCdConst.ZaiKanwaCareSiiso1,
                 //   // 在宅緩和ケア充実診療所・病院加算（施医総管）（２人～９人）	
                 //   ItemCdConst.ZaiKanwaCareSiiso2_9,
                 //   // 在宅緩和ケア充実診療所・病院加算（在医総管）（１人）	
                 //   ItemCdConst.ZaiKanwaCareZaiiso1,
                 //   // 在宅緩和ケア充実診療所・病院加算（在医総管）（２人～９人）
            	    //ItemCdConst.ZaiKanwaCareZaiiso2_9,
                 //   // 在宅緩和ケア充実診療所・病院加算（在医総管）（１０人～）
                 //   ItemCdConst.ZaiKanwaCareZaiiso10,
                 //   // 在宅緩和ケア充実診療所・病院加算（往診）
                 //   ItemCdConst.ZaiKanwaCareOusin,
                 //   // 在宅緩和ケア充実診療所・病院加算(在宅患者訪問診療料(１)１)	
                 //   ItemCdConst.ZaiKanwaCareHoumon1,
                 //   // 在宅緩和ケア充実診療所・病院加算(在宅患者訪問診療料(２)イ)
                 //   ItemCdConst.ZaiKanwaCareHoumon2,
                 //   // 在宅療養実績加算１（在がん医総）
                 //   ItemCdConst.ZaiRyoyoJisseki1Zaigan,
                 //   // 在宅療養実績加算２（在がん医総）
                 //   ItemCdConst.ZaiRyoyoJisseki2Zaigan,
                 //   // 往診
                 //   ItemCdConst.ZaiOusin,
                
                };

            // SARS-Cov関連項目及びその判断料は包括しない
            excludeItemCds.AddRange(GetSARSCovItemList());
            // 救急医療管理加算１（診療報酬上臨時的取扱）は、別に算定できない、となってないので算定できる可能性があるらしい
            excludeItemCds.AddRange(
                new List<string>
                {
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinji,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovOhsin,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiOhsinCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiCovGairai,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiGairaiCyuwa,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiNyuyojiKasan,
                    ItemCdConst.SonotaKyukyuIryoKanriKasanRinjiSyoniKasan
                }
                );

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: null,
                delCdKbns: null,
                excludeItemCds: excludeItemCds,
                checkTerm: 1);
        }

        /// <summary>
        /// 在医総、施医総
        /// </summary>
        private void TokusyuZaiiso()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds = _common.Mst.ZaiisoList;

            // 投薬
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.TouyakuMin, ReceSinId.TouyakuMax, 0, false, false, false, false)
                };

            // 投薬（その他に入ることもあるので）
            List<string> delCdKbn =
                new List<string>
                {
                    "F"
                };

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: null,
                delCdKbns: delCdKbn,
                excludeItemCds: null,
            //    checkTerm: 2);
                checkTerm: 3);  // 月末まで？
        }

        /// <summary>
        /// 在宅患者訪問点滴
        /// </summary>
        private void TokusyuZaiKanHoumonTenteki()
        {
            /*
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.ZaiHoumonTenteki
                };

            // 点滴手技料算定不可
            List<string> delItemCds =
                new List<string>
                {
                    ItemCdConst.ChusyaHoumonTenteki,
                    ItemCdConst.ChusyaTenteki,
                    ItemCdConst.ChusyaTenteki500,
                    ItemCdConst.ChusyaTentekiNyu100,
                    ItemCdConst.ChusyaTentekiSosatuNoDsp,
                    ItemCdConst.ChusyaTentekiSosatuTenAdj
                };
                        
            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: null,
                delItemCds: delItemCds,
                delCdKbns: null,
                excludeItemCds: null, 
                checkTerm: 2);
                */
        }

        /// <summary>
        /// 在宅患者訪問診療料・訪問看護指示料
        /// </summary>
        private void TokusyuZaiHoumon()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    // 在宅患者訪問診療料（１）１（同一建物居住者）
                    ItemCdConst.ZaiHoumon1_1Dou,
                    // 在宅患者訪問診療料（１）１（同一建物居住者以外）
                    ItemCdConst.ZaiHoumon1_1DouIgai,
                    // 在宅患者訪問診療料（１）２（同一建物居住者）
                    ItemCdConst.ZaiHoumon1_2Dou,
                    // 在宅患者訪問診療料（１）２（同一建物居住者以外）
                    ItemCdConst.ZaiHoumon1_2DouIgai,
                    // 在宅患者訪問診療料（２）イ
                    ItemCdConst.ZaiHoumon2i,
                    // // 在宅患者訪問診療料（２）ロ（他の保険医療機関から紹介された患者）
                    ItemCdConst.ZaiHoumon2ro,
                };

            if(_systemConfigProvider.GetHoumonKangoSaisinHokatu() == 0)
            {
                checkItemCds.AddRange(
                    new List<string>
                    {
                        // 在宅患者訪問看護･指導料(保健師、助産師、看護師･週３日目まで)
                        ItemCdConst.ZaiHoumonKangoHoken_3,
                        // 在宅患者訪問看護・指導料（准看護師）（週３日目まで）
                        ItemCdConst.ZaiHoumonKangoJyunkan_3,
                        // 在宅患者訪問看護･指導料(保健師、助産師、看護師･週４日目以降)
                        ItemCdConst.ZaiHoumonKangoHoken4_,
                        // 在宅患者訪問看護・指導料（准看護師）（週４日目以降）
                        ItemCdConst.ZaiHoumonKangoJyunkan4_,
                        // 在宅患者訪問看護･指導料(緩和、褥瘡、人工肛門ケア等専門看護師
                        ItemCdConst.ZaiHoumonKangoKanwa
                    });
            }

            // 再診
            List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKoui =
                new List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)>
                {
                    (ReceSinId.Saisin, ReceSinId.Saisin, 0, false, false, false, false)
                };

            DelWrkDetail(
                checkItemCds: checkItemCds,
                delKouis: delKoui,
                delItemCds: null,
                delCdKbns: null,
                excludeItemCds: null,
                checkTerm: 0);
        }

        /// <summary>
        /// 慢性疼痛疾患管理料
        /// </summary>
        private void TokusyuMaiseiToTu()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuManseiTotu
                };

            // 外来管理加算算定不可
            List<string> delItemCds =
                new List<string>
                {
                    ItemCdConst.GairaiKanriKasan,
                    ItemCdConst.GairaiKanriKasanRousai
                };

            DelWrkDetail(checkItemCds, null, null, delItemCds, null, 2);
        }

        /// <summary>
        /// 慢性維持透析
        /// </summary>
        private void TokusyuManeiIjiToseki()
        {
            // 他の項目を削除する項目のリスト
            List<string> checkItemCds =
                new List<string>
                {
                    ItemCdConst.IgakuManseiIji
                };

            // 胸部X線包括
            DelWrkDetailManseiIjiToseki(checkItemCds, null, 3);
        }

        /// <summary>
        /// 情報通信機器等
        /// オンライン診療料が算定されていない月は算定不可
        /// コロナ特例により、算定可能なこともあるらしいので警告に変更
        /// </summary>
        private void Online()
        {
            List<string> JyohoTusin = new List<string>
            {
                ItemCdConst.IgakuTokusituJyohoTusin,        // 特定疾患療養管理料（情報通信機器）113029010
                ItemCdConst.IgakuSyouniRyoyoJyohoTusin,     // 小児科療養指導料（情報通信機器）113029510
                ItemCdConst.IgakuTenkanJyohoTusin,          // てんかん指導料（情報通信機器）113029610
                ItemCdConst.IgakuNanbyoJyohoTusin,          // 難病外来指導管理料（情報通信機器）113029710
                ItemCdConst.IgakuTounyouJyohoTusin,         // 糖尿病透析予防指導管理料（情報通信機器）113030910
                ItemCdConst.IgakuTiikiHoukatuJyohoTusin,    // 地域包括診療料（情報通信機器）113031310
                ItemCdConst.IgakuNintiTiikiJyohoTusin,      // 認知症地域包括診療料（情報通信機器）113031410
                ItemCdConst.IgakuSeikatuJyohoTusin,         // 生活習慣病管理料（情報通信機器）113031510
                ItemCdConst.ZaiJikochuJyohoTusin            // 在宅自己注射指導管理料（情報通信機器）114049910
            };

            if (_common.Wrk.ExistWrkSinKouiDetailByItemCd(JyohoTusin))
            {
                // オンライン診療料が算定されているか？
                if ((_common.Wrk.wrkSinKouiDetails.Any(p =>
                                        p.RaiinNo != _common.raiinNo &&
                                        _common.Wrk.wrkSinKouiDetailDels.Any(
                                            d => d.RpNo == p.RpNo &&
                                            d.SeqNo == p.SeqNo &&
                                            d.RowNo == p.RowNo &&
                                            d.IsWarning == 0) == false &&
                                        checkHokenKbn.Contains(p.HokenKbn) &&
                                        p.ItemCd == ItemCdConst.OnlineSinryo) == false) &&
                    (_common.Sin.CheckSanteiTerm(new List<string> { ItemCdConst.OnlineSinryo }, _common.SinFirstDateOfMonth, _common.sinDate) == false))
                {
                    // オンライン診療料が算定されていない場合

                    // 削除対象外Rpリスト

                    // 削除対象のレコードを探す
                    List<WrkSinKouiDetailModel> delWrkDtls =
                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                            p.RaiinNo == _common.raiinNo &&
                            p.HokenKbn == _common.hokenKbn &&
                            JyohoTusin.Contains(p.ItemCd));

                    // 削除対象の項目リストを生成する
                    List<DelItemRowInf> delRows = MakeDelRows(delWrkDtls);

                    // ワーク診療行為詳細削除に追加
                    AppendWrkDtlDel(
                        delRows: delRows,
                        excludeRows: new List<(int rpNo, int seqNo, int itemSeqNo)>(),
                        stdTen: 0,
                        syotei: false,
                        delItemCd: ItemCdConst.OnlineSinryo,
                        santeiDate: 0,
                        delSbt: DelSbtConst.NoExistsWarning,
                        termCnt: 1,
                        termSbt: 1,
                        isWarning: 1);
                }
            }


        }

        /// <summary>
        /// 外来感染対策向上加算
        /// </summary>
        private void TokusyuKansenKojo()
        {
            List<string> checkItemCds = new List<string>();
            List<string> delItemCds = new List<string>();

            // 先に算定されたものだけ残す
            // 同日に算定された場合、初診、再診、医学管理、在宅、精神の優先順位で算定する
            for (int i = 0; i < 5; i++)
            { 
                switch (i)
                {
                    case 0:
                        // 精神
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinKansenKojo,
                                ItemCdConst.SaisinKansenKojo,
                                ItemCdConst.IgakuKansenKojo,
                                ItemCdConst.ZaitakuKansenKojo
                             };

                        // 精神算定不可
                        delItemCds =
                            new List<string>
                            { 
                                ItemCdConst.SonotaKansenKojo
                            };
                        break;
                    case 1:
                        // 在宅
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinKansenKojo,
                                ItemCdConst.SaisinKansenKojo,
                                ItemCdConst.IgakuKansenKojo,
                                ItemCdConst.SonotaKansenKojo
                             };

                        // 在宅算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.ZaitakuKansenKojo
                            };
                        break;
                    case 2: 
                        // 医学管理
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinKansenKojo,
                                ItemCdConst.SaisinKansenKojo,
                                ItemCdConst.ZaitakuKansenKojo,
                                ItemCdConst.SonotaKansenKojo
                             };

                        // 医学管理算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.IgakuKansenKojo
                            };
                        break;
                    case 3:
                        // 再診
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinKansenKojo,
                                ItemCdConst.IgakuKansenKojo,
                                ItemCdConst.ZaitakuKansenKojo,
                                ItemCdConst.SonotaKansenKojo
                             };

                        // 再診算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.SaisinKansenKojo
                            };
                        break;
                    case 4:
                        // 初診
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SaisinKansenKojo,
                                ItemCdConst.IgakuKansenKojo,
                                ItemCdConst.ZaitakuKansenKojo,
                                ItemCdConst.SonotaKansenKojo
                             };

                        // 初診算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.SyosinKansenKojo
                            };
                        break;
                    default:
                        break;
                }


                DelWrkDetail(
                    checkItemCds: checkItemCds,
                    delKouis: null,
                    delItemCds: delItemCds,
                    delCdKbns: null,
                    excludeItemCds: null,
                    checkTerm: 2);
            }

        }

        /// <summary>
        /// 連携強化加算
        /// </summary>
        private void TokusyuRenkeiKyoka()
        {
            List<string> checkItemCds = new List<string>();
            List<string> delItemCds = new List<string>();

            // 先に算定されたものだけ残す
            // 同日に算定された場合、初診、再診、医学管理、在宅、精神の優先順位で算定する
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        // 精神
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinRenkeiKyoka,
                                ItemCdConst.SaisinRenkeiKyoka,
                                ItemCdConst.IgakuRenkeiKyoka,
                                ItemCdConst.ZaitakuRenkeiKyoka
                             };

                        // 精神算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.SonotaRenkeiKyoka
                            };
                        break;
                    case 1:
                        // 在宅
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinRenkeiKyoka,
                                ItemCdConst.SaisinRenkeiKyoka,
                                ItemCdConst.IgakuRenkeiKyoka,
                                ItemCdConst.SonotaRenkeiKyoka
                             };

                        // 在宅算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.ZaitakuRenkeiKyoka
                            };
                        break;
                    case 2:
                        // 医学管理
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinRenkeiKyoka,
                                ItemCdConst.SaisinRenkeiKyoka,
                                ItemCdConst.ZaitakuRenkeiKyoka,
                                ItemCdConst.SonotaRenkeiKyoka
                             };

                        // 医学管理算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.IgakuRenkeiKyoka
                            };
                        break;
                    case 3:
                        // 再診
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinRenkeiKyoka,
                                ItemCdConst.IgakuRenkeiKyoka,
                                ItemCdConst.ZaitakuRenkeiKyoka,
                                ItemCdConst.SonotaRenkeiKyoka
                             };

                        // 再診算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.SaisinRenkeiKyoka
                            };
                        break;
                    case 4:
                        // 初診
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SaisinRenkeiKyoka,
                                ItemCdConst.IgakuRenkeiKyoka,
                                ItemCdConst.ZaitakuRenkeiKyoka,
                                ItemCdConst.SonotaRenkeiKyoka
                             };

                        // 初診算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.SyosinRenkeiKyoka
                            };
                        break;
                    default:
                        break;
                }
                DelWrkDetail(
                    checkItemCds: checkItemCds,
                    delKouis: null,
                    delItemCds: delItemCds,
                    delCdKbns: null,
                    excludeItemCds: null,
                    checkTerm: 2);
            }

        }

        /// <summary>
        /// サーベイランス強化加算
        /// </summary>
        private void TokusyuSurveillance()
        {
            List<string> checkItemCds = new List<string>();
            List<string> delItemCds = new List<string>();

            // 先に算定されたものだけ残す
            // 同日に算定された場合、初診、再診、医学管理、在宅、精神の優先順位で算定する
            for (int i = 0; i < 5; i++)
            {
                switch (i)
                {
                    case 0:
                        // 精神
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinSurveillance,
                                ItemCdConst.SaisinSurveillance,
                                ItemCdConst.IgakuSurveillance,
                                ItemCdConst.ZaitakuSurveillance
                             };

                        // 精神算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.SonotaSurveillance
                            };
                        break;
                    case 1:
                        // 在宅
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinSurveillance,
                                ItemCdConst.SaisinSurveillance,
                                ItemCdConst.IgakuSurveillance,
                                ItemCdConst.SonotaSurveillance
                             };

                        // 在宅算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.ZaitakuSurveillance
                            };
                        break;
                    case 2:
                        // 医学管理
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinSurveillance,
                                ItemCdConst.SaisinSurveillance,
                                ItemCdConst.ZaitakuSurveillance,
                                ItemCdConst.SonotaSurveillance
                             };

                        // 医学管理算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.IgakuSurveillance
                            };
                        break;
                    case 3:
                        // 再診
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SyosinSurveillance,
                                ItemCdConst.IgakuSurveillance,
                                ItemCdConst.ZaitakuSurveillance,
                                ItemCdConst.SonotaSurveillance
                             };

                        // 再診算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.SaisinSurveillance
                            };
                        break;
                    case 4:
                        // 初診
                        // 他の項目を削除する項目のリスト
                        checkItemCds =
                             new List<string>
                             {
                                ItemCdConst.SaisinSurveillance,
                                ItemCdConst.IgakuSurveillance,
                                ItemCdConst.ZaitakuSurveillance,
                                ItemCdConst.SonotaSurveillance
                             };

                        // 初診算定不可
                        delItemCds =
                            new List<string>
                            {
                                ItemCdConst.SyosinSurveillance
                            };
                        break;
                    default:
                        break;
                }
                DelWrkDetail(
                    checkItemCds: checkItemCds,
                    delKouis: null,
                    delItemCds: delItemCds,
                    delCdKbns: null,
                    excludeItemCds: null,
                    checkTerm: 2);
            }

        }

        /// <summary>
        /// 初診から1ヶ月、当日分のチェック
        /// 背反等で初診が算定不可になった場合に、チェック対象から外したいので、
        /// 背反処理の後のここでチェックする
        /// </summary>
        private void TokusyuSyosin()
        {
            /// <summary>
            /// 初診項目のリスト
            /// </summary>
            List<string> _syosinls =
                new List<string>
                {
                    ItemCdConst.Syosin,
                    ItemCdConst.SyosinCorona,
                    ItemCdConst.SyosinJouhou,
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuAri,
                    ItemCdConst.IgakuSyouniGairaiSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakaritukeSyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke1SyosinKofuNasi,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuAri,
                    ItemCdConst.IgakuSyouniKakarituke2SyosinKofuNasi
                };

            foreach (
                WrkSinKouiDetailModel wrkDtl in
                    _common.Wrk.wrkSinKouiDetails.FindAll(p => p.IsDeleted == DeleteStatus.None))
            {

                List<DensiSanteiKaisuModel> densiSanteiKaisuModels =
                    _common.Mst.FindDensiSanteiKaisuSyosin(_common.sinDate, _common.IsRosai, wrkDtl.ItemCd);

                if (densiSanteiKaisuModels != null && densiSanteiKaisuModels.Any())
                {
                    // チェック期間と表記を取得する
                    foreach (DensiSanteiKaisuModel densiSanteiKaisu in densiSanteiKaisuModels)
                    {
                        // チェック開始日
                        int startDate = 0;
                        // チェック終了日
                        int endDate = _common.sinDate;

                        List<int> checkHokenKbnTmp = new List<int>();
                        checkHokenKbnTmp.AddRange(checkHokenKbn);

                        if (densiSanteiKaisu.TargetKbn == 1)
                        {
                            // 健保のみ対象の場合はすべて対象
                        }
                        else if (densiSanteiKaisu.TargetKbn == 2)
                        {
                            // 労災のみ対象の場合、健保は抜く
                            checkHokenKbnTmp.RemoveAll(p => new int[] { 0 }.Contains(p));
                        }

                        List<int> checkSanteiKbnTmp = new List<int>();
                        checkSanteiKbnTmp.AddRange(checkSanteiKbn);

                        if (_common.Wrk.GetSanteiKbn(wrkDtl.RaiinNo, wrkDtl.RpNo) == 2 && wrkDtl.IsAutoAdd == 1)
                        {
                            // 自費算定、自動算定の場合、自費も含める
                            checkSanteiKbnTmp.Add(2);
                        }

                        //初診から1カ月
                        List<WrkSinKouiDetailModel> tgtWrkDtls =
                            _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                _syosinls.Contains(p.ItemCd) && checkHokenKbnTmp.Contains(p.HokenKbn) && checkSanteiKbnTmp.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)));
                        foreach (WrkSinKouiDetailModel tgtWrkDtl in tgtWrkDtls)
                        {
                            if(_common.Wrk.ExistWrkSinKouiDetailDel(tgtWrkDtl) == false)
                            {
                                // 初診関連項目を算定している場合、算定不可
                                endDate = 99999999;
                                break;
                            }
                        }

                        if (new int[] { 997, 998 }.Contains(densiSanteiKaisu.UnitCd))
                        {
                            //初診から1カ月
                            if (endDate > _common.sinDate)
                            {
                                //算定不可
                                int ret = 0;
                                int isWarning = 0;
                                if (densiSanteiKaisu.SpJyoken == 1)
                                {
                                    // 注意
                                    isWarning = 1;
                                }

                                if(isWarning == 0)
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetailDel
                                    (hokenKbn: wrkDtl.HokenKbn,
                                        rpNo: wrkDtl.RpNo,
                                        seqNo: wrkDtl.SeqNo,
                                        rowNo: wrkDtl.RowNo,
                                        itemCd: wrkDtl.ItemCd,
                                        delItemCd: "9999999998",
                                        santeiDate: _common.sinDate,
                                        delSbt: DelSbtConst.AfterSyosin,
                                        isWarning: isWarning,
                                        termCnt: 1,
                                        termSbt: 1,
                                        isAutoAdd: wrkDtl.IsAutoAdd,
                                        hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                                    if (ret == 2)
                                    {
                                        //// 削除される項目が削除していた項目があれば、削除しておく
                                        foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && (p.TermSbt == 1 || p.TermSbt == 6) && p.DelSbt != DelSbtConst.NoLog))
                                        {
                                            foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                            {
                                                if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                    p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd &&
                                                    !(p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && (p.TermSbt == 1 || p.TermSbt == 6) && p.DelSbt != DelSbtConst.NoLog)) == false)
                                                {
                                                    dtl.IsDeleted = 0;

                                                    foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                    {
                                                        koui.IsDeleted = 0;
                                                    }
                                                    foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                    {
                                                        rp.IsDeleted = 0;
                                                    }
                                                }
                                            }
                                        }
                                        _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == wrkDtl.ItemCd && p.TermCnt == 1 && (p.TermSbt == 1 || p.TermSbt == 6) && p.DelSbt != DelSbtConst.NoLog);
                                    }
                                }
                            }
                        }                        
                    }
                }
            }
        }
        private void TokusyuJibaiBunsyo()
        {
            if (new int[] { 0, 1, 2 }.Contains(_common.hokenKbn))
            {
                // 削除対象のレコードを探す
                List<WrkSinKouiDetailModel> delWrkDtls =
                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                        p.RaiinNo == _common.raiinNo &&
                        p.HokenKbn == _common.hokenKbn &&
                        (p.TenMst != null && new string[] { "ZZ0", "ZZ1", "A18" }.Contains(p.TenMst.SyukeiSaki)));

                // 削除対象の項目リストを生成する
                List<DelItemRowInf> delRows = MakeDelRows(delWrkDtls);

                // ワーク診療行為詳細削除に追加
                foreach (DelItemRowInf delRow in delRows)
                {
                    if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                             p.RpNo == delRow.rpNo && p.SeqNo == delRow.seqNo && p.RowNo == delRow.rowNo &&
                             p.ItemCd == delRow.itemCd && p.DelItemCd == "9999999998"))
                    {
                        // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                    }
                    else
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                        (hokenKbn: delRow.hokenKbn,
                            rpNo: delRow.rpNo,
                            seqNo: delRow.seqNo,
                            rowNo: delRow.rowNo,
                            itemCd: delRow.itemCd,
                            delItemCd: "9999999998",
                            santeiDate: _common.sinDate,
                            delSbt: DelSbtConst.JibaiBunsyo,
                            isWarning: 0,
                            termCnt: 0,
                            termSbt: 0,
                            isAutoAdd: delRow.isAutoAdd,
                            hokenId: _common.Wrk.GetWrkKouiHokenId(delRow.rpNo, delRow.seqNo));
                    }

                }
            }
        }

        /// <summary>
        /// SARS-Cov関連で小児科外来診療料等に包括されない項目のリストを返す
        /// </summary>
        /// <returns></returns>
        private List<string> GetSARSCovItemList()
        {
            List<string> kakusanItems = new List<string>
            {
                // ＳＡＲＳ－ＣｏＶ－２核酸検出（検査委託）        
                ItemCdConst.KensaSARSCov2Itaku,
                ItemCdConst.KensaSARSCov2Itaku20211231,
                // ＳＡＲＳ－ＣｏＶ－２核酸検出（検査委託以外）
                ItemCdConst.KensaSARSCov2ItakuGai,
                ItemCdConst.KensaSARSCov2ItakuGai20211231,
                // ＳＡＲＳ－ＣｏＶ－２・インフルエンザ核酸同時検出（検査委託）
                ItemCdConst.KensaSARSInfulItaku,
                ItemCdConst.KensaSARSInfulItaku20211231,
                // ＳＡＲＳ－ＣｏＶ－２・インフルエンザ核酸同時検出（検査委託以外）
                ItemCdConst.KensaSARSInfulItakugai,
                ItemCdConst.KensaSARSInfulItakugai20211231,
                ItemCdConst.KensaVirusKakusaninSARSItaku,
                ItemCdConst.KensaVirusKakusaninSARSItakuGai,
                //ＳＡＲＳ－ＣｏＶ－２・ＲＳウイルス核酸同時検出（検査委託） 
                ItemCdConst.KensaSARSRsVirusItaku,
                //ウイルスＳＡＲＳ－ＣｏＶ－２・ＲＳウイルス核酸同時検出（検査委託以外） 
                ItemCdConst.KensaSARSRsVirusItakuIgai,
                //ＳＡＲＳ－ＣｏＶ－２・インフルエンザ・ＲＳ核酸同時検出（委託）
                ItemCdConst.KensaSARSInfluRsVirusItaku,
                //ＳＡＲＳ－ＣｏＶ－２・インフルエンザ・ＲＳ核酸同時検出（委託外）
                ItemCdConst.KensaSARSInfluRsVirusItakuIgai
            };
            List<string> kogenItems = new List<string>
            {
                // ＳＡＲＳ－ＣｏＶ－２抗原検出
                ItemCdConst.KensaSARSCov2Kogen,
                ItemCdConst.KensaSARSCov2Kogen20211231,
                // ＳＡＲＳ－ＣｏＶ－２抗原検出（定量）
                ItemCdConst.KensaSARSCov2KogenTeiryo,
                ItemCdConst.KensaSARSCov2KogenTeiryo20211231,
                // ＳＡＲＳ－ＣｏＶ－２・インフルエンザウイルス抗原同時検出
                ItemCdConst.KensaSARSInfulKogenDouji,
                ItemCdConst.KensaSARSInfulKogenDouji20211231,
                // ＳＡＲＳ－ＣｏＶ－２・ＲＳウイルス抗原同時検出（定性）
                ItemCdConst.KensaSARSRsVirusKougen,
                // ＳＡＲＳ－ＣｏＶ－２・インフルエンザ・ＲＳ抗原同時検出（定性）
                ItemCdConst.KensaSARSInfluRsVirusKougen
            };

            List<string> result = new List<string>();
            result.AddRange(kakusanItems);
            result.AddRange(kogenItems);

            if (_common.Odr.odrDtlls.Any(p =>
                p.RaiinNo == _common.raiinNo &&
                kakusanItems.Contains(p.ItemCd)
            ))
            {
                // 核酸検出の場合、微生物学的検査判断料を算定できる
                result.Add(ItemCdConst.KensaHandanBiseibutu);
            }

            if (_common.Odr.odrDtlls.Any(p =>
                 p.RaiinNo == _common.raiinNo &&
                 kogenItems.Contains(p.ItemCd)
                ))
            {
                // 抗原検出の場合、免疫学的検査判断料を算定できる
                result.Add(ItemCdConst.KensaHandanMeneki);
            }

            return result;
        }
        /// <summary>
        /// 項目削除処理
        /// </summary>
        /// <param name="checkItemCds">他の項目を削除する診療行為の診療行為コード</param>
        /// <param name="delKouis">
        /// 削除対象行為のリスト
        ///     min - 行為番号(SIN_ID)下限
        ///     max - 行為番号(SIN_ID)上限
        ///     stdTen - 0以外の場合、この点数未満の診療行為、または診療行為以外を削除する
        /// </param>
        /// <param name="excludeItemCds">削除対象外とする項目の診療行為コード</param>
        /// <param name="checkTerm">0:同来院　1:同日 2:月(診療日以前) 3:月(月末まで)</param>
        private void DelWrkDetail
            (List<string> checkItemCds, List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKouis, List<string> delItemCds, List<string> delCdKbns,
             List<string> excludeItemCds, int checkTerm, int delSbt = DelSbtConst.HoukatuTokusyu)
        {
            // 今回の算定から、他の行為を包括する診療行為(checkItemCds)を探す
            List<WrkSinKouiDetailModel> wrkDtls = GetWrkDtlRaiin(checkItemCds);

            if (wrkDtls.Any())
            {
                // 見つかった場合、包括する診療行為を削除(WRK_SIN_KOUI_DETAIL_DELに追加)していく
                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    List<int> santeiKbns = new List<int>();
                    if (_common.Wrk.GetSanteiKbn(wrkDtl.RaiinNo, wrkDtl.RpNo) == SanteiKbnConst.Jihi)
                    {
                        // 自費算定の項目は自費算定の項目のみ包括する
                        santeiKbns.Add(SanteiKbnConst.Jihi);
                    }
                    else
                    {
                        // 自費算定以外の場合、自費算定は包括しない
                        santeiKbns.AddRange(CalcUtils.GetCheckSanteiKbns(wrkDtl.HokenKbn, _systemConfigProvider.GetHokensyuHandling()));
                        santeiKbns.RemoveAll(p => p == SanteiKbnConst.Jihi);
                    }
                    
                    //if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                    //         p.RaiinNo == wrkDtl.RaiinNo &&
                    //         p.RpNo == wrkDtl.RpNo &&
                    //         p.SeqNo == wrkDtl.SeqNo &&
                    //         p.RowNo == wrkDtl.RowNo &&
                    //         p.IsWarning == 0) == false)
                    //{
                    // 削除対象項目に入ってない場合、削除する
                    DelRows(
                        delKouis: delKouis,
                        delItemCds: delItemCds,
                        delCdKbns: delCdKbns,
                        excludeItemCds: excludeItemCds,
                        exRpNo: wrkDtl.RpNo,
                        exSeqNo: wrkDtl.SeqNo,
                        exItemSeqNo: wrkDtl.ItemSeqNo,
                        delItemCd: wrkDtl.ItemCd,
                        santeiDay: 0,
                        termCnt: 1,
                        termSbt: 1,
                        delSbt: delSbt,
                        santeiKbns: santeiKbns
                        );
                    //}
                }
            }

            if (checkTerm == 1 || checkTerm == 2 || checkTerm == 3)
            {
                // 同日分を先にチェック
                if (_common.calcMode != CalcModeConst.Trial)
                {
                    wrkDtls = GetWrkDtlToday(checkItemCds, (checkTerm == 2));

                    if (wrkDtls.Any())
                    {
                        // 見つかった場合、包括する診療行為を削除(WRK_SIN_KOUI_DETAIL_DELに追加)していく
                        foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                        {
                            List<int> santeiKbns = new List<int>();
                            if (_common.Wrk.GetSanteiKbn(wrkDtl.RaiinNo, wrkDtl.RpNo) == SanteiKbnConst.Jihi)
                            {
                                // 自費算定の項目は自費算定の項目のみ包括する
                                santeiKbns.Add(SanteiKbnConst.Jihi);
                            }
                            else
                            {
                                // 自費算定以外の場合、自費算定は包括しない
                                santeiKbns.AddRange(CalcUtils.GetCheckSanteiKbns(wrkDtl.HokenKbn, _systemConfigProvider.GetHokensyuHandling()));
                                santeiKbns.RemoveAll(p => p == SanteiKbnConst.Jihi);
                            }
                            
                            if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                     p.RpNo == wrkDtl.RpNo && p.SeqNo == wrkDtl.SeqNo && p.RowNo == wrkDtl.RowNo &&
                                     p.ItemCd == wrkDtl.ItemCd && p.IsWarning == 0) == false)
                            {
                                // 当該項目が削除される項目ではない場合
                                DelRows(
                                delKouis: delKouis,
                                delItemCds: delItemCds,
                                delCdKbns: delCdKbns,
                                excludeItemCds: excludeItemCds,
                                exRpNo: wrkDtl.RpNo,
                                exSeqNo: wrkDtl.SeqNo,
                                exItemSeqNo: wrkDtl.ItemSeqNo,
                                delItemCd: wrkDtl.ItemCd,
                                santeiDay: 0,
                                termCnt: 1,
                                termSbt: 2,
                                delSbt: delSbt,
                                santeiKbns: santeiKbns
                                );
                            }
                        }
                    }
                }

                //List<SanteiDaysModel> santeiDays = _common.GetSanteiDaysSinDate(checkItemCds);
                //if (santeiDays.Any())
                //{
                //    foreach (SanteiDaysModel santeiDay in santeiDays)
                //    {
                //        DelRows(
                //            delKouis: delKouis,
                //            delItemCds: delItemCds,
                //            delCdKbns: delCdKbns,
                //            excludeItemCds: excludeItemCds,
                //            exRpNo: 0,
                //            exSeqNo: 0,
                //            exItemSeqNo: 0,
                //            delItemCd: santeiDay.ItemCd,
                //            santeiDay: santeiDay.SinDate,
                //            termCnt: 1,
                //            termSbt: 6
                //            );
                //    }
                //}
                //}
                //else if (checkTerm == 2)
                //{
                if (checkTerm == 2 || checkTerm == 3 || (_common.calcMode == CalcModeConst.Trial && checkTerm == 1))
                {
                    // 同月チェック
                    //List<SanteiDaysModel> santeiDays = _common.GetSanteiDays(_common.SinFirstDateOfMonth, _common.sinDate, checkItemCds);
                    List<SanteiDaysModel> santeiDays = _common.GetSanteiDaysSinYmWithHokenKbn(checkItemCds);
                    
                    if (checkTerm == 1)
                    {
                        // 診療日のみ
                        santeiDays = santeiDays.FindAll(p => p.SinDate == _common.sinDate);
                    }
                    else if (checkTerm == 2)
                    {
                        // 診療日まで
                        santeiDays = santeiDays.FindAll(p => p.SinDate <= _common.sinDate);
                    }

                    if (santeiDays.Any())
                    {
                        foreach (SanteiDaysModel santeiDay in santeiDays)
                        {
                            List<int> santeiKbns = new List<int>();
                            if (santeiDay.SanteiKbn == SanteiKbnConst.Jihi)
                            {
                                // 自費算定の項目は自費算定の項目のみ包括する
                                santeiKbns.Add(SanteiKbnConst.Jihi);
                            }
                            else
                            {
                                // 自費算定以外の場合、自費算定は包括しない
                                santeiKbns.AddRange(CalcUtils.GetCheckSanteiKbns(santeiDay.HokenKbn, _systemConfigProvider.GetHokensyuHandling()));
                                santeiKbns.RemoveAll(p => p == SanteiKbnConst.Jihi);
                            }
                            DelRows(
                                delKouis: delKouis,
                                delItemCds: delItemCds,
                                delCdKbns: delCdKbns,
                                excludeItemCds: excludeItemCds,
                                exRpNo: 0,
                                exSeqNo: 0,
                                exItemSeqNo: 0,
                                delItemCd: santeiDay.ItemCd,
                                santeiDay: santeiDay.SinDate,
                                termCnt: 1,
                                termSbt: 6,
                                delSbt: delSbt,
                                santeiKbns: santeiKbns
                                );
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 慢性維持透析（胸部X線の包括）用
        /// </summary>
        /// <param name="checkItemCds"></param>
        /// <param name="excludeItemCds"></param>
        /// <param name="checkTerm">0:同来院　1:同日 2:月(診療日以前) 3:月(月末まで)</param>
        private void DelWrkDetailManseiIjiToseki
            (List<string> checkItemCds, List<string> excludeItemCds, int checkTerm)
        {
            // 今回の算定から、他の行為を包括する診療行為(checkItemCds)を探す
            List<WrkSinKouiDetailModel> wrkDtls = GetWrkDtlRaiin(checkItemCds);

            if (wrkDtls.Any())
            {
                // 見つかった場合、包括する診療行為を削除(WRK_SIN_KOUI_DETAIL_DELに追加)していく
                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    DelKyobuXLay(
                        exRpNo: wrkDtl.RpNo,
                        exSeqNo: wrkDtl.SeqNo,
                        exItemSeqNo: wrkDtl.ItemSeqNo,
                        excludeItemCds: excludeItemCds,
                        delItemCd: wrkDtl.ItemCd,
                        santeiDay: 0,
                        termCnt: 1,
                        termSbt: 1);
                }
            }

            if (checkTerm == 1 || checkTerm == 2 || checkTerm == 3)
            {
                // 同日分を先にチェック
                wrkDtls = GetWrkDtlToday(checkItemCds, (checkTerm == 2));

                if (wrkDtls.Any())
                {
                    // 見つかった場合、包括する診療行為を削除(WRK_SIN_KOUI_DETAIL_DELに追加)していく
                    foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                    {
                        DelKyobuXLay(
                            exRpNo: wrkDtl.RpNo,
                            exSeqNo: wrkDtl.SeqNo,
                            exItemSeqNo: wrkDtl.ItemSeqNo,
                            excludeItemCds: excludeItemCds,
                            delItemCd: wrkDtl.ItemCd,
                            santeiDay: 0,
                            termCnt: 1,
                            termSbt: 6);
                    }
                }

                if (checkTerm == 2 || checkTerm == 3)
                {
                    // 同月チェック
                    List<SanteiDaysModel> santeiDays = _common.GetSanteiDaysSinYm(checkItemCds);

                    if (checkTerm == 2)
                    {
                        // 診療日まで
                        santeiDays = santeiDays.FindAll(p => p.SinDate <= _common.sinDate);
                    }

                    if (santeiDays.Any())
                    {
                        foreach (SanteiDaysModel santeiDay in santeiDays)
                        {
                            DelKyobuXLay(
                                exRpNo: 0,
                                exSeqNo: 0,
                                exItemSeqNo: 0,
                                excludeItemCds: excludeItemCds,
                                delItemCd: santeiDay.ItemCd,
                                santeiDay: santeiDay.SinDate,
                                termCnt: 1,
                                termSbt: 6);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// ワーク行為詳細リストから既にWrkKouiDetailDelに登録済みのものを取り除く
        /// </summary>
        /// <param name="wrkDtl"></param>
        /// <returns></returns>
        private List<WrkSinKouiDetailModel> ExcludeDtlDel(List<WrkSinKouiDetailModel> wrkDtls)
        {
            List<WrkSinKouiDetailModel> ret = new List<WrkSinKouiDetailModel>();

            foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls.FindAll(p => _common.Wrk.ExistWrkSinKouiDetailDel(p) == false))
            {
                ret.Add(wrkDtl);
            }

            return ret;
        }

        /// <summary>
        /// 今回の算定から、他の行為を包括する診療行為(checkItemCds)を探す
        /// </summary>
        /// <param name="checkItemCds"></param>
        /// <param name="beforeThisRaiin">true-当来院以前の診療行為を探す</param>
        /// <returns></returns>
        private List<WrkSinKouiDetailModel> GetWrkDtlRaiin(List<string> checkItemCds)
        {
            List<WrkSinKouiDetailModel> results = new List<WrkSinKouiDetailModel>();

            if (checkItemCds != null && checkItemCds.Any())
            {
                results = _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                        p.RaiinNo == _common.raiinNo &&
                        _common.Wrk.ExistWrkSinKouiDetailDel(p) == false &&
                        //p.HokenKbn == _common.hokenKbn &&
                        checkHokenKbn.Contains(p.HokenKbn) &&
                        checkSanteiKbn.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                        checkItemCds.Contains(p.ItemCd) &&
                        p.IsDummy == false &&
                        p.IsDeleted == DeleteStatus.None);
            }

            return results;
        }

        /// <summary>
        /// 今日の算定から、他の行為を包括する診療行為(checkItemCds)を探す
        /// </summary>
        /// <param name="checkItemCds"></param>
        /// <returns></returns>
        private List<WrkSinKouiDetailModel> GetWrkDtlToday(List<string> checkItemCds, bool beforeThisRaiin = false)
        {
            List<WrkSinKouiDetailModel> results = new List<WrkSinKouiDetailModel>();

            if (beforeThisRaiin == false)
            {
                results =
                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    //p.HokenKbn == _common.hokenKbn &&
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    _common.Wrk.ExistWrkSinKouiDetailDel(p) == false &&
                    checkItemCds.Contains(p.ItemCd) &&
                    p.IsDeleted == DeleteStatus.None);
            }
            else
            {
                // 当来院以前
                results =
                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    checkSanteiKbn.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) &&
                    _common.Wrk.ExistWrkSinKouiDetailDel(p) == false &&
                    checkItemCds.Contains(p.ItemCd) &&
                    ((string.Compare(p.SinStartTime, _common.sinStartTime) < 0) ||
                     ((string.Compare(p.SinStartTime, _common.sinStartTime) == 0) && (p.RaiinNo < _common.raiinNo))) &&
                    p.IsDeleted == DeleteStatus.None);
            }

            return results;
        }

        /// <summary>
        /// レコードを削除する処理
        /// 診療行為番号範囲指定、診療行為コード指定、コード表用番号指定
        /// </summary>
        /// <param name="delKouis">削除対象診療行為番号範囲のリスト</param>
        /// <param name="delItemCds">削除対象項目の診療行為コードのリスト</param>
        /// <param name="delCdKbns">削除対象項目のコード表用番号のリスト</param>
        /// <param name="excludeItemCds">削除対象外項目の診療行為コードのリスト</param>
        /// <param name="exRpNo">削除対象外レコードのRP_NO</param>
        /// <param name="exSeqNo">削除対象外レコードのSEQ_NO</param>
        /// <param name="exItemSeqNo">削除対象外レコードのITEM_SEQ_NO</param>
        /// <param name="delItemCd">これらの項目を削除する項目の診療行為コード</param>
        /// <param name="santeiDay">delItemCdの算定日</param>
        /// <param name="termCnt">チェック期間数</param>
        /// <param name="termSbt">チェック期間</param>
        private void DelRows(List<(int min, int max, double stdTen, bool syotei, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm)> delKouis, List<string> delItemCds, List<string> delCdKbns, List<string> excludeItemCds,
            int exRpNo, int exSeqNo, int exItemSeqNo, string delItemCd, int santeiDay, int termCnt, int termSbt, int delSbt = DelSbtConst.HoukatuTokusyu, List<int> santeiKbns = null)
        {
            //指定の行為を削除
            if (delKouis != null)
            {
                for (int i = 0; i < delKouis.Count; i++)
                {
                    DelKoui
                        (
                            kouiMin: delKouis[i].min,
                            kouiMax: delKouis[i].max,
                            stdTen: delKouis[i].stdTen,
                            syotei: delKouis[i].syotei,
                            exRpNo: exRpNo,
                            exSeqNo: exSeqNo,
                            exItemSeqNo: exItemSeqNo,
                            excludeItemCds: excludeItemCds,
                            delItemCd: delItemCd,
                            santeiDay: santeiDay,
                            termCnt: termCnt,
                            termSbt: termSbt,
                            excludeYakuzai: delKouis[i].excludeYakuzai,
                            excludeTokuzai: delKouis[i].excludeTokuzai,
                            excludeFilm: delKouis[i].excludeFilm,
                            delSbt: delSbt,
                            santeiKbns: santeiKbns
                        );
                }
            }

            if (delItemCds != null)
            {
                // 指定の項目を削除
                DelItem
                    (
                        delItemCds: delItemCds,
                        stdTen: 0,
                        exRpNo: exRpNo,
                        exSeqNo: exSeqNo,
                        exItemSeqNo: exItemSeqNo,
                        excludeItemCds: excludeItemCds,
                        delItemCd: delItemCd,
                        santeiDay: santeiDay,
                        termCnt: termCnt,
                        termSbt: termSbt,
                        delSbt: delSbt,
                        santeiKbns: santeiKbns
                    );
            }

            if (delCdKbns != null)
            {
                // 指定のコード表用番号の項目を削除
                DelCdKbn
                    (
                        delCdKbns: delCdKbns,
                        stdTen: 0,
                        syotei: false,
                        exRpNo: exRpNo,
                        exSeqNo: exSeqNo,
                        exItemSeqNo: exItemSeqNo,
                        excludeItemCds: excludeItemCds,
                        delItemCd: delItemCd,
                        santeiDay: santeiDay,
                        termCnt: termCnt,
                        termSbt: termSbt,
                        excludeYakuzai: false,
                        delSbt: delSbt,
                        santeiKbns: santeiKbns
                    );
            }
        }

        /// <summary>
        /// 指定の行為の項目を削除する
        /// </summary>
        /// <param name="kouiMin">行為番号(SIN_ID)下限</param>
        /// <param name="kouiMax">行為番号(SIN_ID)上限</param>
        /// <param name="stdTen">0以外のとき、この点数未満の診療行為、または診療行為以外を削除する</param>
        /// <param name="syotei">false-項目別に判断する、true-所定点数で判断する</param>
        /// <param name="exRpNo">削除対象外レコードのRP_NO</param>
        /// <param name="exSeqNo">削除対象外レコードのSEQ_NO</param>
        /// <param name="exItemSeqNo">削除対象外レコードのITEM_SEQ_NO</param>
        /// <param name="excludeItemCds">削除対象外にする項目の診療行為コード</param>
        /// <param name="delItemCd">行為を削除する項目</param>
        /// <param name="santeiDay">行為を削除する項目(delItemCd)を算定した日、0は当来院</param>
        /// <param name="termCnt">チェック期間数</param>
        /// <param name="termSbt">チェック期間種別 1-来院、6-月</param>
        private void DelKoui
            (int kouiMin, int kouiMax, double stdTen, bool syotei, int exRpNo, int exSeqNo, int exItemSeqNo,
             List<string> excludeItemCds, string delItemCd, int santeiDay, int termCnt, int termSbt, bool excludeYakuzai, bool excludeTokuzai, bool excludeFilm, int delSbt = DelSbtConst.HoukatuTokusyu, List<int> santeiKbns = null)
        {
            // 削除対象外Rpリスト
            List<(int rpNo, int seqNo, int itemSeqNo)> excludeRows =
                MakeExcludeRows(exRpNo, exSeqNo, exItemSeqNo, excludeItemCds);

            List<int> checkSanteiKbnsLocal = new List<int>();

            if (santeiKbns != null)
            {
                checkSanteiKbnsLocal.AddRange(santeiKbns);
            }
            else
            {
                checkSanteiKbnsLocal.AddRange(checkSanteiKbn);
            }

            // 対象のRpを探す
            List<WrkSinRpInfModel> delWrkRps =
                _common.Wrk.wrkSinRpInfs.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    //p.HokenKbn == _common.hokenKbn &&
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    //p.SanteiKbn == SanteiKbnConst.Santei &&
                    checkSanteiKbnsLocal.Contains(p.SanteiKbn) &&
                    (p.SinId >= kouiMin && p.SinId <= kouiMax)
                    );

            foreach (WrkSinRpInfModel delWrkRp in delWrkRps)
            {
                List<WrkSinKouiModel> delWrkKouis =
                    _common.Wrk.wrkSinKouis.FindAll(p =>
                        p.RaiinNo == _common.raiinNo &&
                        p.HokenKbn == _common.hokenKbn &&
                        p.RpNo == delWrkRp.RpNo &&
                        p.InoutKbn == 0 &&
                        p.SyukeiSaki != ReceSyukeisaki.SonotaSyohoSenComment);

                if (delWrkRp.SinId == ReceSinId.Sonota)
                {
                    // その他の場合、CD_KBN="SO"（コメントのみのRp）は、削除対象外とする
                    delWrkKouis = delWrkKouis.FindAll(p => p.CdKbn != "SO");
                }

                foreach (WrkSinKouiModel delWrkKoui in delWrkKouis)
                {
                    // 削除対象を探す
                    List<WrkSinKouiDetailModel> delWrkDtls =
                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                            p.RaiinNo == _common.raiinNo &&
                            p.HokenKbn == _common.hokenKbn &&
                            p.RpNo == delWrkKoui.RpNo &&
                            p.SeqNo == delWrkKoui.SeqNo);

                    //if(delWrkKoui.SyukeiSaki == ReceSyukeisaki.SonotaSyohoSen)
                    //{
                    //    delWrkDtls = delWrkDtls.FindAll(p => !(p.RecId == "CO" && p.ItemCd.Length == 9 && p.ItemCd.StartsWith("8")));
                    //}

                    // 削除対象の項目リスト
                    List<DelItemRowInf> delRows = MakeDelRows(delWrkDtls);

                    // ワーク診療行為詳細削除に追加
                    AppendWrkDtlDel(
                        delRows: delRows,
                        excludeRows: excludeRows,
                        stdTen: stdTen,
                        syotei: syotei,
                        delItemCd: delItemCd,
                        santeiDate: santeiDay,
                        delSbt: delSbt,
                        termCnt: termCnt,
                        termSbt: termSbt,
                        isWarning: 0,
                        excludeYakuzai: excludeYakuzai,
                        excludeTokuzai: excludeTokuzai,
                        excludeFilm: excludeFilm);
                }
            }
        }

        /// <summary>
        /// 指定の項目を削除する
        /// </summary>
        /// <param name="kouiMin">行為番号(SIN_ID)下限</param>
        /// <param name="kouiMax">行為番号(SIN_ID)上限</param>
        /// <param name="stdTen">0以外のとき、この点数未満の診療行為、または診療行為以外を削除する</param>
        /// <param name="exRpNo">削除対象外レコードのRP_NO</param>
        /// <param name="exSeqNo">削除対象外レコードのSEQ_NO</param>
        /// <param name="exItemSeqNo">削除対象外レコードのITEM_SEQ_NO</param>
        /// <param name="excludeItemCds">削除対象外にする項目の診療行為コード</param>
        /// <param name="delItemCd">行為を削除する項目</param>
        /// <param name="santeiDay">行為を削除する項目(delItemCd)を算定した日、0は当来院</param>
        /// <param name="termCnt">チェック期間数</param>
        /// <param name="termSbt">チェック期間種別 1-来院、6-月</param>
        private void DelItem
            (List<string> delItemCds, double stdTen, int exRpNo, int exSeqNo, int exItemSeqNo,
             List<string> excludeItemCds, string delItemCd, int santeiDay, int termCnt, int termSbt, int delSbt = DelSbtConst.HoukatuTokusyu, List<int> santeiKbns = null)
        {
            // 削除対象外Rpリスト
            List<(int rpNo, int seqNo, int itemSeqNo)> excludeRows =
                MakeExcludeRows(exRpNo, exSeqNo, exItemSeqNo, excludeItemCds);

            List<int> checkSanteiKbnsLocal = new List<int>();

            if (santeiKbns != null)
            {
                checkSanteiKbnsLocal = santeiKbns;
            }
            else
            {
                checkSanteiKbnsLocal.AddRange(checkSanteiKbn);
            }


            // 削除対象のレコードを探す
            List<WrkSinKouiDetailModel> delWrkDtls =
                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    //p.HokenKbn == _common.hokenKbn &&
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) ||
                    (_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo) == 2 && p.IsAutoAdd == 1)) &&
                    delItemCds.Contains(p.ItemCd));
            // 削除対象の項目リストを生成する
            List<DelItemRowInf> delRows = MakeDelRows(delWrkDtls);

            // ワーク診療行為詳細削除に追加
            AppendWrkDtlDel(
                delRows: delRows,
                excludeRows: excludeRows,
                stdTen: stdTen,
                syotei: false,
                delItemCd: delItemCd,
                santeiDate: santeiDay,
                delSbt: delSbt,
                termCnt: termCnt,
                termSbt: termSbt,
                isWarning: 0,
                excludeYakuzai: false);
        }

        /// <summary>
        /// 指定のコード表用番号の項目を削除する
        /// </summary>
        /// <param name="delCdKbns">削除するコード表用番号</param>
        /// <param name="stdTen">0以外のとき、この点数未満の診療行為、または診療行為以外を削除する</param>
        /// <param name="exRpNo">削除対象外レコードのRP_NO</param>
        /// <param name="exSeqNo">削除対象外レコードのSEQ_NO</param>
        /// <param name="exItemSeqNo">削除対象外レコードのITEM_SEQ_NO</param>
        /// <param name="excludeItemCds">削除対象外にする項目の診療行為コード</param>
        /// <param name="delItemCd">行為を削除する項目</param>
        /// <param name="santeiDay">行為を削除する項目(delItemCd)を算定した日、0は当来院</param>
        /// <param name="termCnt">チェック期間数</param>
        /// <param name="termSbt">チェック期間種別 1-来院、6-月</param>
        private void DelCdKbn
            (List<string> delCdKbns, double stdTen, bool syotei, int exRpNo, int exSeqNo, int exItemSeqNo,
            List<string> excludeItemCds, string delItemCd, int santeiDay, int termCnt, int termSbt, bool excludeYakuzai, int delSbt = DelSbtConst.HoukatuTokusyu, List<int> santeiKbns = null)
        {
            // 削除対象外Rpリスト
            List<(int rpNo, int seqNo, int itemSeqNo)> excludeRows =
                MakeExcludeRows(exRpNo, exSeqNo, exItemSeqNo, excludeItemCds);

            List<int> checkSanteiKbnsLocal = new List<int>();
            if (santeiKbns != null)
            {
                checkSanteiKbnsLocal.AddRange(santeiKbns);
            }
            else
            {
                checkSanteiKbnsLocal.AddRange(checkSanteiKbn);
            }

            // 削除対象のレコードを探す
            List<WrkSinKouiDetailModel> delWrkDtls =
                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    //p.HokenKbn == _common.hokenKbn &&
                    checkHokenKbn.Contains(p.HokenKbn) &&
                    (checkSanteiKbnsLocal.Contains(_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo)) ||
                    (_common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo) == 2 && p.IsAutoAdd == 1)) &&
                    _common.Wrk.GetSyukeiSaki(p.RaiinNo, p.RpNo, p.SeqNo) != ReceSyukeisaki.SonotaSyohoSenComment &&
                    delCdKbns.Contains(p.CdKbn));

            // 削除対象の項目リスト
            List<DelItemRowInf> delRows = MakeDelRows(delWrkDtls);

            // ワーク診療行為詳細削除に追加
            AppendWrkDtlDel(
                delRows: delRows,
                excludeRows: excludeRows, stdTen: stdTen, syotei: syotei, delItemCd: delItemCd, santeiDate: santeiDay, delSbt: delSbt, termCnt: termCnt, termSbt: termSbt, isWarning: 0, excludeYakuzai: excludeYakuzai);
        }

        /// <summary>
        /// 胸部X線の撮影料、診断料を削除する
        /// </summary>
        /// <param name="stdTen"></param>
        /// <param name="exRpNo"></param>
        /// <param name="exSeqNo"></param>
        /// <param name="exItemSeqNo"></param>
        /// <param name="excludeItemCds"></param>
        /// <param name="delItemCd"></param>
        /// <param name="santeiDay"></param>
        /// <param name="termCnt"></param>
        /// <param name="termSbt"></param>
        private void DelKyobuXLay
            (int exRpNo, int exSeqNo, int exItemSeqNo,
             List<string> excludeItemCds, string delItemCd, int santeiDay, int termCnt, int termSbt)
        {
            // 削除対象外Rpリスト
            List<(int rpNo, int seqNo, int itemSeqNo)> excludeRows =
                MakeExcludeRows(exRpNo, exSeqNo, exItemSeqNo, excludeItemCds);

            // 胸部を探す（部位区分=5の項目、または、コメント項目の"単純撮影（撮影部位）胸部（肩を除く）"）
            var kyobuDtls =
                _common.Wrk.wrkSinKouiDetails.Where(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    (
                        (p.TenMst != null && p.TenMst.BuiKbn == 5) ||
                        (p.ItemCd == ItemCdConst.CommentTanjyunSatueiKyobu)
                    )
                    )
                    .GroupBy(p => new { rpNo = p.RpNo, seqNo = p.SeqNo });

            if (kyobuDtls != null && kyobuDtls.Any())
            {
                // 胸部がオーダーされているRpの中で撮影料、診断料を抽出
                foreach (var key in kyobuDtls)
                {
                    List<WrkSinKouiDetailModel> delWrkDtls =
                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                            p.RpNo == key.Key.rpNo &&
                            p.SeqNo == key.Key.seqNo &&
                            (p.IsSatuei == true || p.IsSindan == true)
                            );

                    // 削除対象の項目リスト
                    List<DelItemRowInf> delRows = MakeDelRows(delWrkDtls);

                    // ワーク診療行為詳細削除に追加
                    AppendWrkDtlDel(
                        delRows: delRows,
                        excludeRows: excludeRows,
                        stdTen: 0,
                        syotei: false,
                        delItemCd: delItemCd,
                        santeiDate: santeiDay,
                        delSbt: DelSbtConst.HoukatuTokusyu,
                        termCnt: termCnt,
                        termSbt: termSbt,
                        isWarning: 0,
                        excludeYakuzai: false,
                        excludeTokuzai: true,
                        excludeFilm: true);
                }
            }
        }

        /// <summary>
        /// 削除対象外レコードのリストを生成する
        /// </summary>
        /// <param name="exRpNo">削除対象外レコードのRP_NO</param>
        /// <param name="exSeqNo">削除対象外レコードのSEQ_NO</param>
        /// <param name="exItemSeqNo">削除対象外レコードのITEM_SEQ_NO</param>
        /// <param name="excludeItemCds">削除対象外の項目の診療行為コードのリスト</param>
        /// <returns></returns>
        private List<(int rpNo, int seqNo, int itemSeqNo)> MakeExcludeRows(int exRpNo, int exSeqNo, int exItemSeqNo, List<string> excludeItemCds)
        {
            List<(int rpNo, int seqNo, int itemSeqNo)> excludeRows =
                new List<(int rpNo, int seqNo, int itemSeqNo)>();

            excludeRows.Add((exRpNo, exSeqNo, exItemSeqNo));

            // 削除対象外項目コードの指定がある場合、このRpにその項目が含まれているかチェックする
            if (excludeItemCds != null && excludeItemCds.Any())
            {
                List<WrkSinKouiDetailModel> exWrkDtls =
                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    excludeItemCds.Contains(p.ItemCd));

                foreach (WrkSinKouiDetailModel exWrkDtl in exWrkDtls)
                {
                    excludeRows.Add((exWrkDtl.RpNo, exWrkDtl.SeqNo, exWrkDtl.ItemSeqNo));
                }
            }

            return excludeRows;
        }

        /// <summary>
        /// 削除対象レコードのリストを生成する
        /// </summary>
        /// <param name="delWrkDtls">削除対象のWRK_SIN_KOUI_DETAIL</param>
        /// <returns></returns>
        private List<DelItemRowInf> MakeDelRows(List<WrkSinKouiDetailModel> delWrkDtls)
        {
            List<DelItemRowInf> delRows = new List<DelItemRowInf>();

            foreach (WrkSinKouiDetailModel delWrkDtl in delWrkDtls)
            {
                // 自費算定は除く
                if (_common.Wrk.wrkSinRpInfs.Any(p =>
                         p.RaiinNo == _common.raiinNo &&
                         //p.HokenKbn == _common.hokenKbn &&
                         checkHokenKbn.Contains(p.HokenKbn) &&
                         p.RpNo == delWrkDtl.RpNo &&
                         //p.SanteiKbn == SanteiKbnConst.Santei
                         checkSanteiKbn.Contains(p.SanteiKbn)
                         ))
                {
                    if (_common.Wrk.wrkSinKouis.Any(p =>
                             p.RaiinNo == _common.raiinNo &&
                             p.HokenKbn == _common.hokenKbn &&
                             p.RpNo == delWrkDtl.RpNo &&
                             p.SeqNo == delWrkDtl.SeqNo &&
                             p.InoutKbn == 0))
                    {
                        DelItemRowInf delrow = new DelItemRowInf();

                        delrow.rpNo = delWrkDtl.RpNo;
                        delrow.seqNo = delWrkDtl.SeqNo;
                        delrow.rowNo = delWrkDtl.RowNo;
                        delrow.itemSeqNo = delWrkDtl.ItemSeqNo;
                        delrow.recId = delWrkDtl.RecId;
                        delrow.ten = delWrkDtl.Ten;
                        delrow.hokenKbn = delWrkDtl.HokenKbn;
                        delrow.itemCd = delWrkDtl.ItemCd;
                        delrow.isAutoAdd = delWrkDtl.IsAutoAdd;

                        delRows.Add(delrow);
                    }
                }
            }

            return delRows;
        }

        /// <summary>
        /// 外来管理加算の削除
        /// </summary>
        private bool GairaiKanri()
        {
            bool ret = false;

            List<string> Saisinls =
                new List<string>
                {
                    ItemCdConst.Saisin,
                    ItemCdConst.SaisinRousai,
                    ItemCdConst.SaisinJouhou,
                    ItemCdConst.Saisin2,
                    ItemCdConst.Saisin2Rousai,
                    ItemCdConst.Saisin2Jouhou,
                    ItemCdConst.SaisinDojitu,
                    ItemCdConst.SaisinDojituRousai,
                    ItemCdConst.SaisinJouhouDojitu
                };

            List<WrkSinKouiDetailModel> wrkDtls =
                _common.Wrk.FindWrkSinKouiDetailByItemCd(FindWrkDtlMode.HokenOnly, ItemCdConst.GairaiKanriKasan);
            if (wrkDtls.Any())
            {
                //if(_common.Wrk.wrkSinKouiDetails.Any(p =>
                //   p.TenMst != null &&
                //   p.IsDeleted == 0 &&
                //    (
                //        (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 215 && p.TenMst.CdKbnno <= 217) ||
                //        (p.TenMst.CdKbn == "D" && (p.TenMst.CdKbnno >= 235 && p.TenMst.CdKbnno <= 236) || (p.TenMst.CdKbnno == 237 && p.TenMst.CdKouno <= 2)) ||
                //        (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 239 && p.TenMst.CdKbnno <= 242) ||
                //        (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 243 && p.TenMst.CdKbnno <= 254) ||
                //        (p.TenMst.CdKbn == "D" && (p.TenMst.CdKbnno >= 255 && p.TenMst.CdKbnno <= 281) || (p.TenMst.CdKbnno == 282 && p.TenMst.CdKouno <= 3)) ||
                //        (p.TenMst.CdKbn == "D" && (p.TenMst.CdKbnno >= 286 && p.TenMst.CdKbnno <= 290) || (p.TenMst.CdKbnno == 291 && p.TenMst.CdKouno <= 3)) ||
                //        (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 292 && p.TenMst.CdKbnno <= 294) ||
                //        (p.TenMst.CdKbn == "D" && p.TenMst.CdKbnno >= 295 && p.TenMst.CdKbnno <= 325) ||
                //        (p.TenMst.CdKbn == "H") ||
                //        (p.TenMst.CdKbn == "I") ||
                //        (p.TenMst.CdKbn == "J") ||
                //        (p.TenMst.CdKbn == "K") ||
                //        (p.TenMst.CdKbn == "L") ||
                //        (p.TenMst.CdKbn == "M")
                //        )
                //   ) == true)
                if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                        p.TenMst != null &&
                        (p.RaiinNo == _common.raiinNo || p.OyaRaiinNo == _common.oyaRaiinNo) &&
                        //p.IsDeleted == 0 &&               // 包括されていても実施していれば算定不可
                        (p.TenMst.GairaiKanriKbn == 1 ||
                         (new string[] { "H", "I", "J", "K", "L", "M" }.Contains(p.TenMst.CdKbn) && p.ItemCd.StartsWith("1") && p.ItemCd.Length == 9)) &&
                        p.TenMst.CdKbn != "A"
                    ) == true ||
                    _common.Wrk.wrkSinKouiDetails.Any(p =>
                        p.TenMst != null &&
                        p.RaiinNo == _common.raiinNo &&
                        p.IsDeleted == DeleteStatus.None &&
                        Saisinls.Contains(p.ItemCd)
                    ) == false
                    )
                {
                    foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                (hokenKbn: wrkDtl.HokenKbn,
                                                 rpNo: wrkDtl.RpNo,
                                                 seqNo: wrkDtl.SeqNo,
                                                 rowNo: wrkDtl.RowNo,
                                                 itemCd: wrkDtl.ItemCd,
                                                 delItemCd: "9999999998",
                                                 santeiDate: 0,
                                                 delSbt: DelSbtConst.GairaiKanri,
                                                 isWarning: 0,
                                                 termCnt: 1,
                                                 termSbt: 1,
                                                 isAutoAdd: wrkDtl.IsAutoAdd,
                                                 hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));
                    }

                    ret = true;
                }
                else
                {
                    // 同一来院で外来管理加算を算定済みの時は算定しない
                    // 診察開始時間が小さい方で算定
                    // 診察開始時間が同じ場合、来院番号が小さい方で算定
                    if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                             p.RaiinNo != _common.raiinNo &&
                             p.OyaRaiinNo == _common.oyaRaiinNo &&
                             ((string.Compare(p.SinStartTime, _common.sinStartTime) < 0) ||
                              ((string.Compare(p.SinStartTime, _common.sinStartTime) == 0) && (p.RaiinNo < _common.raiinNo))) &&
                             p.ItemCd == ItemCdConst.GairaiKanriKasan &&
                             p.IsDeleted == DeleteStatus.None))
                    {
                        foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetailDel
                                                    (hokenKbn: wrkDtl.HokenKbn,
                                                     rpNo: wrkDtl.RpNo,
                                                     seqNo: wrkDtl.SeqNo,
                                                     rowNo: wrkDtl.RowNo,
                                                     itemCd: wrkDtl.ItemCd,
                                                     delItemCd: "9999999998",
                                                     santeiDate: 0,
                                                     delSbt: DelSbtConst.GairaiDouitu,
                                                     isWarning: 0,
                                                     termCnt: 1,
                                                     termSbt: 1,
                                                     isAutoAdd: wrkDtl.IsAutoAdd,
                                                     hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));
                        }

                        ret = true;
                    }
                }
            }

            return ret;

        }

        /// <summary>
        /// ワーク診療行為詳細削除に削除対象レコードの情報を追加する
        /// </summary>
        /// <param name="delRows">削除対象レコードのリスト</param>
        /// <param name="excludeRows">削除対象外レコードのリスト</param>
        /// <param name="stdTen">基準点</param>
        /// <param name="syotei">false-項目ごとに判断、true-所定点数で判断</param>
        /// <param name="delItemCd">これらの項目を削除する項目の診療行為コード</param>
        /// <param name="santeiDate">delItemCdの算定日</param>
        /// <param name="delSbt">削除種別</param>
        /// <param name="termCnt">チェック期間数</param>
        /// <param name="termSbt">チェック期間</param>
        /// <param name="isWarning">0:削除 1:警告 2:どちらか１つ 3:どちらか1つの可能性</param>
        /// <param name="excludeYakuzai">true-薬剤除く</param>
        private void AppendWrkDtlDel
            (List<DelItemRowInf> delRows, List<(int rpNo, int seqNo, int itemSeqNo)> excludeRows,
            double stdTen, bool syotei, string delItemCd, int santeiDate, int delSbt, int termCnt, int termSbt, int isWarning = 0, bool excludeYakuzai = false, bool excludeTokuzai = false, bool excludeFilm = false)
        {
            foreach (DelItemRowInf delRow in delRows)
            {
                if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                         p.RpNo == delRow.rpNo && p.SeqNo == delRow.seqNo && p.RowNo == delRow.rowNo &&
                         p.ItemCd == delRow.itemCd && p.DelItemCd == delItemCd))
                {
                    // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                }
                else
                {
                    if (IsExcludeRecord(delRow.rpNo, delRow.seqNo, delRow.itemSeqNo) == false)
                    {
                        // 削除対象外レコードではない

                        double targetTen = delRow.ten;

                        // チェック用に詳細取得
                        WrkSinKouiDetailModel chkWrkDtl = null;

                        if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                                 p.RpNo == delRow.rpNo &&
                                 p.SeqNo == delRow.seqNo &&
                                 p.RowNo == delRow.rowNo &&
                                 p.IsDeleted == DeleteStatus.None))
                        {
                            chkWrkDtl = _common.Wrk.wrkSinKouiDetails.Find(p =>
                                p.RpNo == delRow.rpNo &&
                                p.SeqNo == delRow.seqNo &&
                                p.RowNo == delRow.rowNo &&
                                p.IsDeleted == DeleteStatus.None);
                        }

                        #region 所定点数で判断する場合、所定点数を計算する
                        if (syotei && (chkWrkDtl != null && new string[] { "S", "R" }.Contains(chkWrkDtl.MasterSbt)))
                        {
                            // 所定点数で判断する場合は、所定点数を取得

                            if (_common.Wrk.wrkSinKouis.Any(p =>
                                     p.RpNo == delRow.rpNo &&
                                     p.SeqNo == delRow.seqNo &&
                                     p.IsDeleted == DeleteStatus.None))
                            {
                                WrkSinKouiModel wrkKoui =
                                    _common.Wrk.wrkSinKouis.Find(p =>
                                        p.RpNo == delRow.rpNo &&
                                        p.SeqNo == delRow.seqNo &&
                                        p.IsDeleted == DeleteStatus.None);

                                List<WrkSinKouiDetailModel> wrkDtls =
                                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                        p.RpNo == delRow.rpNo &&
                                        p.SeqNo == delRow.seqNo &&
                                        p.IsDeleted == DeleteStatus.None);

                                if (!(wrkKoui.CdKbn == "E" && wrkDtls.Any(p => p.CdKbnno > 0 && p.CdKbnno < 100)))
                                {
                                    // エックス線診断料は並び変えない
                                    wrkDtls =
                                        wrkDtls
                                            .OrderBy(p => p.TyuCd)
                                            .ThenBy(p => p.Kokuji1CalcSort)
                                            .ThenBy(p => p.Kokuji2)
                                            .ThenBy(p => p.TusokuAge)   // 通則年齢加算
                                            .ThenBy(p => p.TyuSeq)
                                            .ThenBy(p => p.ItemSeqNo)
                                            .ThenBy(p => p.ItemEdaNo)
                                            .ToList();
                                }

                                if (wrkDtls.Any(p =>
                                    p.RpNo == delRow.rpNo &&
                                    p.SeqNo == delRow.seqNo &&
                                    p.RowNo == delRow.rowNo &&
                                    p.IsDeleted == DeleteStatus.None))
                                {
                                    WrkSinKouiDetailModel wrkDtl = wrkDtls.Find(p =>
                                        p.RpNo == delRow.rpNo &&
                                        p.SeqNo == delRow.seqNo &&
                                        p.RowNo == delRow.rowNo &&
                                        p.IsDeleted == DeleteStatus.None);

                                    if (new string[] { "7", "9" }.Contains(wrkDtl.Kokuji2))
                                    {
                                        // 加算項目の場合、上にある基本項目を取得する
                                        int index = wrkDtls.FindIndex(p =>
                                            p.RpNo == delRow.rpNo &&
                                            p.SeqNo == delRow.seqNo &&
                                            p.RowNo == delRow.rowNo &&
                                            p.IsDeleted == DeleteStatus.None);

                                        if (index >= 0)
                                        {
                                            targetTen = 0;
                                            for (int i = index; i >= 0; i--)
                                            {
                                                if (wrkDtls[i].TenMst != null)
                                                {
                                                    if (wrkDtls[i].TenMst.MasterSbt != "S" && wrkDtls[i].TenMst.MasterSbt != "C")
                                                    {
                                                        break;
                                                    }

                                                    targetTen += wrkDtls[i].Ten;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        if (stdTen == 0 || delRow.recId != ReceRecId.Sinryo || targetTen < stdTen)
                        {
                            if (excludeYakuzai && (chkWrkDtl != null && chkWrkDtl.DrugKbn > 0))
                            {
                                // 薬剤を除く指定の時、薬剤項目は除く
                            }
                            else if (excludeTokuzai && (chkWrkDtl != null && chkWrkDtl.MasterSbt == "T"))
                            {
                                // 特材を除く指定の時、特材項目は除く
                            }
                            else if (excludeFilm && (chkWrkDtl != null && chkWrkDtl.SinKouiKbn == 77))
                            {
                                // フィルムを除く指定の時、フィルム項目は除く
                            }
                            else if (stdTen > 0 && delRow.recId == ReceRecId.Comment)
                            {
                                // 基準点の指定があって、コメントの場合は削除対象外とする
                                // 例）地域包括では、急性増悪時、検査・画像は550点未満の項目のみ包括される
                                //     同一Rp内で、削除されない項目もあるかもしれないので、この時点ではコメントを残しておく
                                //     最終的に、算定項目がなくなった場合は、項目削除時に、
                                //     削除項目に付随するコメント削除の処理があるのでそこで消えてくれるはず
                            }
                            else
                            {
                                // 基準点の指定がない(0) or 
                                // 基準点の指定はあるが、診療行為(SI)ではない or 点数が基準点未満
                                _common.Wrk.AppendNewWrkSinKouiDetailDel
                                (hokenKbn: delRow.hokenKbn,
                                    rpNo: delRow.rpNo,
                                    seqNo: delRow.seqNo,
                                    rowNo: delRow.rowNo,
                                    itemCd: delRow.itemCd,
                                    delItemCd: delItemCd,
                                    santeiDate: santeiDate,
                                    delSbt: delSbt,
                                    isWarning: isWarning,
                                    termCnt: termCnt,
                                    termSbt: termSbt,
                                    isAutoAdd: delRow.isAutoAdd,
                                    hokenId: _common.Wrk.GetWrkKouiHokenId(delRow.rpNo, delRow.seqNo));

                                if (isWarning == 0)
                                {
                                    //// 削除される項目が削除していた項目があれば、削除しておく
                                    foreach (var del in _common.Wrk.wrkSinKouiDetailDels.FindAll(p => p.DelItemCd == delRow.itemCd && p.TermCnt == 1 && (p.TermSbt == 1 || p.TermSbt == 6) && p.DelSbt != DelSbtConst.NoLog))
                                    {
                                        foreach (var dtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd && p.IsDeleted == DeleteStatus.DeleteFlag))
                                        {
                                            if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                p.RaiinNo == del.RaiinNo && p.RpNo == del.RpNo && p.SeqNo == del.SeqNo && p.RowNo == del.RowNo && p.ItemCd == del.ItemCd &&
                                                !(p.DelItemCd == delRow.itemCd && p.TermCnt == 1 && (p.TermSbt == 1 || p.TermSbt == 6) && p.DelSbt != DelSbtConst.NoLog)) == false)
                                            {
                                                dtl.IsDeleted = 0;

                                                foreach (var koui in _common.Wrk.wrkSinKouis.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.SeqNo == dtl.SeqNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                {
                                                    koui.IsDeleted = 0;
                                                }
                                                foreach (var rp in _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == dtl.RaiinNo && p.RpNo == dtl.RpNo && p.IsDeleted == DeleteStatus.DeleteFlag))
                                                {
                                                    rp.IsDeleted = 0;
                                                }
                                            }
                                        }
                                    }
                                    _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.DelItemCd == delRow.itemCd && p.TermCnt == 1 && (p.TermSbt == 1 || p.TermSbt == 6) && p.DelSbt != DelSbtConst.NoLog);
                                }
                            }
                        }
                    }
                }

            }

            #region Local Method

            // 削除対象外のレコードかどうかチェックする
            bool IsExcludeRecord(int rpNo, int seqNo, int itemSeqNo)
            {
                bool ret = false;

                for (int i = 0; i < excludeRows.Count; i++)
                {
                    if (excludeRows[i].rpNo == rpNo &&
                       excludeRows[i].seqNo == seqNo &&
                       excludeRows[i].itemSeqNo == itemSeqNo)
                    {
                        ret = true;
                        break;
                    }
                }

                return ret;
            }
            #endregion
        }

        // 自動発生コメントのリスト
        private List<string> autoAddCommentls = new List<string>()
                    {
                        ItemCdConst.CommentJissiRekkyoDummy,
                        ItemCdConst.CommentJissiRekkyoItemNameDummy,
                        ItemCdConst.CommentJissiNissuDummy,
                        ItemCdConst.CommentJissiNissuItemNameDummy
                    };

        // 注射手技項目のリスト
        private List<string> chusyaSyugiItemCds = new List<string>
            {
                ItemCdConst.ChusyaTenteki,
                ItemCdConst.ChusyaTenteki500,
                ItemCdConst.ChusyaTentekiNyu100,
                ItemCdConst.ChusyaJyomyaku,
                ItemCdConst.ChusyaHikaKin
            };
        /// <summary>
        /// 削除した項目に付随する項目の削除
        /// </summary>
        private void WrkDelete()
        {
            List<WrkSinKouiDetailModel> wrkDtlGrpItems;

            #region localmethod

            void _delDetail(WrkSinKouiDetailModel wrkDtl)
            {

                if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                            p.RaiinNo == wrkDtl.RaiinNo &&
                            p.HokenKbn == wrkDtl.HokenKbn &&
                            p.RpNo == wrkDtl.RpNo &&
                            p.SeqNo == wrkDtl.SeqNo &&
                            p.RowNo == wrkDtl.RowNo &&
                            p.IsWarning == 0))
                {
                    wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;

                    // 特定の注コードを持つ基本項目が削除される場合、別Rpの同一注コードを持つ項目も削除
                    List<string> delTyuCds = new List<string>
                    {
                        "1101", // 初診
                        "1201"  // 再診
                    };

                    if (wrkDtl.IsKihonKoumoku && delTyuCds.Contains(wrkDtl.TyuCd))
                    {
                        // 付随する加算、コメント削除する
                        wrkDtlGrpItems =
                            _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                p.RaiinNo == wrkDtl.RaiinNo &&
                                p.HokenKbn == wrkDtl.HokenKbn &&
                                p.TenMst != null &&
                                p.IsKihonKoumoku2 == false &&
                                p.TenMst.TyuCd == wrkDtl.TenMst.TyuCd);

                        List<string> syosaiDelItems = new List<string>();
                        if (wrkDtl.TyuCd == "1101")
                        {
                            // 初診のコメントがあれば削除
                            syosaiDelItems.AddRange(new List<string>
                            {
                                ItemCdConst.CommentSyosinJikanNai
                            });

                            // TYU_CDは未設定だが、算定不可になるはずの項目
                            syosaiDelItems.AddRange(new List<string>
                            {
                                // 乳幼児感染予防策加算（初診料・診療報酬上臨時的取扱）
                                ItemCdConst.SyosinNyuyojiKansen,
                                // 医科外来等感染症対策実施加算（初診料）
                                ItemCdConst.SyosinKansenTaisaku,
                            });
                        }
                        else if (wrkDtl.TyuCd == "1201")
                        {
                            // 再診のコメントがあれば削除
                            syosaiDelItems.AddRange(new List<string>
                            {
                                ItemCdConst.CommentSaisinDojitu,
                                ItemCdConst.CommentDenwaSaisin,
                                ItemCdConst.CommentDenwaSaisinDojitu
                            });

                            // TYU_CDは未設定だが、算定不可になるはずの項目
                            syosaiDelItems.AddRange(new List<string>
                            {
                                // 乳幼児感染予防策加算（再診料・外来診療料・診療報酬上臨時的取扱）
                                ItemCdConst.SaisinNyuyojiKansen,
                                // 医科外来等感染症対策実施加算（再診料・外来診療料）
                                ItemCdConst.SaisinKansenTaisaku,
                            });
                        }

                        if (syosaiDelItems.Any())
                        {
                            wrkDtlGrpItems.AddRange(
                                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                    p.RaiinNo == wrkDtl.RaiinNo &&
                                    p.HokenKbn == wrkDtl.HokenKbn &&
                                    syosaiDelItems.Contains(p.ItemCd)));
                        }

                        foreach (WrkSinKouiDetailModel wrkDtlGrpItem in wrkDtlGrpItems)
                        {
                            wrkDtlGrpItem.IsDeleted = DeleteStatus.DeleteFlag;

                            if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                        p.RpNo == wrkDtlGrpItem.RpNo && p.SeqNo == wrkDtlGrpItem.SeqNo && p.RowNo == wrkDtlGrpItem.RowNo &&
                                        p.ItemCd == wrkDtlGrpItem.ItemCd && p.IsWarning == 0))
                            {
                                // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetailDel(
                                hokenKbn: wrkDtlGrpItem.HokenKbn,
                                rpNo: wrkDtlGrpItem.RpNo,
                                seqNo: wrkDtlGrpItem.SeqNo,
                                rowNo: wrkDtlGrpItem.RowNo,
                                itemCd: wrkDtlGrpItem.ItemCd,
                                delItemCd: ItemCdConst.NoSantei,
                                santeiDate: 0,
                                delSbt: DelSbtConst.Fuzui,
                                isWarning: 0,
                                termCnt: 0,
                                termSbt: 0,
                                isAutoAdd: wrkDtlGrpItem.IsAutoAdd,
                                hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtlGrpItem.RpNo, wrkDtlGrpItem.SeqNo)
                                );
                            }
                        }
                    }

                    // 付随するコメント削除する
                    if (string.IsNullOrEmpty(wrkDtl.BaseItemCd))
                    {
                        wrkDtlGrpItems =
                            _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                p.RaiinNo == wrkDtl.RaiinNo &&
                                p.HokenKbn == wrkDtl.HokenKbn &&
                                p.RpNo == wrkDtl.RpNo &&
                                p.SeqNo == wrkDtl.SeqNo &&
                                p.ItemSeqNo == wrkDtl.ItemSeqNo &&
                                (string.IsNullOrEmpty(p.BaseItemCd) || p.BaseItemCd == wrkDtl.ItemCd) &&  // ベース項目コードが未指定（空）、またはベース項目コードが自身と同じ（異なる項目に紐づくコメントは削除しないようにする）
                                p.BuiKbn == 0 &&  // 部位以外（部位は削除しない）
                                p.ItemSbt == 1 &&
                                !(new int[] { CmtSbtConst.SatueiBui, CmtSbtConst.SatueiBuiFukubu, CmtSbtConst.SatueiBuiKyobu }.Contains(p.CmtSbt)) &&  // 部位コメント以外（部位は削除しない）
                                _common.Wrk.GetSyukeiSaki(wrkDtl.RaiinNo, wrkDtl.RpNo, wrkDtl.SeqNo) != ReceSyukeisaki.SonotaSyohoSenComment    // 処方箋コメント以外
                                );

                        foreach (WrkSinKouiDetailModel wrkDtlGrpItem in wrkDtlGrpItems)
                        {
                            wrkDtlGrpItem.IsDeleted = DeleteStatus.DeleteFlag;

                            if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                        p.RpNo == wrkDtlGrpItem.RpNo && p.SeqNo == wrkDtlGrpItem.SeqNo && p.RowNo == wrkDtlGrpItem.RowNo &&
                                        p.ItemCd == wrkDtlGrpItem.ItemCd && p.IsWarning == 0))
                            {
                                // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetailDel(
                                hokenKbn: wrkDtlGrpItem.HokenKbn,
                                rpNo: wrkDtlGrpItem.RpNo,
                                seqNo: wrkDtlGrpItem.SeqNo,
                                rowNo: wrkDtlGrpItem.RowNo,
                                itemCd: wrkDtlGrpItem.ItemCd,
                                delItemCd: ItemCdConst.NoSantei,
                                santeiDate: 0,
                                delSbt: DelSbtConst.Fuzui,
                                isWarning: 0,
                                termCnt: 0,
                                termSbt: 0,
                                isAutoAdd: wrkDtlGrpItem.IsAutoAdd,
                                hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtlGrpItem.RpNo, wrkDtlGrpItem.SeqNo)
                                );
                            }
                        }

                        // 付随する年齢加算なども削除する
                        if (wrkDtl.TenMst != null)
                        {
                            for (int i = 0; i <= 4; i++)
                            {
                                if (wrkDtl.TenMst.AgekasanCd(i) != "" && wrkDtl.TenMst.AgekasanCd(i) != "0")
                                {

                                    wrkDtlGrpItems =
                                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                            p.RaiinNo == wrkDtl.RaiinNo &&
                                            p.HokenKbn == wrkDtl.HokenKbn &&
                                            p.RpNo == wrkDtl.RpNo &&
                                            p.SeqNo == wrkDtl.SeqNo &&
                                            p.ItemCd == wrkDtl.TenMst.AgekasanCd(i) &&
                                            p.IsAutoAdd == 1);

                                    foreach (WrkSinKouiDetailModel wrkDtlGrpItem in wrkDtlGrpItems)
                                    {
                                        wrkDtlGrpItem.IsDeleted = DeleteStatus.DeleteFlag;

                                        if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                    p.RpNo == wrkDtlGrpItem.RpNo && p.SeqNo == wrkDtlGrpItem.SeqNo && p.RowNo == wrkDtlGrpItem.RowNo &&
                                                    p.ItemCd == wrkDtlGrpItem.ItemCd && p.IsWarning == 0))
                                        {
                                            // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                                        }
                                        else
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetailDel(
                                            hokenKbn: wrkDtlGrpItem.HokenKbn,
                                            rpNo: wrkDtlGrpItem.RpNo,
                                            seqNo: wrkDtlGrpItem.SeqNo,
                                            rowNo: wrkDtlGrpItem.RowNo,
                                            itemCd: wrkDtlGrpItem.ItemCd,
                                            delItemCd: ItemCdConst.NoSantei,
                                            santeiDate: 0,
                                            delSbt: DelSbtConst.Fuzui,
                                            isWarning: 0,
                                            termCnt: 0,
                                            termSbt: 0,
                                            isAutoAdd: wrkDtlGrpItem.IsAutoAdd,
                                            hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtlGrpItem.RpNo, wrkDtlGrpItem.SeqNo)
                                            );
                                        }
                                    }
                                }
                            }

                            // 処置乳幼児加算
                            if (wrkDtl.TenMst != null && wrkDtl.TenMst.SyotiNyuyojiKbn > 0)
                            {
                                //処置乳幼児加算区分を取得する
                                string itemCd = _common.Wrk.GetSyotiNyuyojiKasanItemCd(wrkDtl.TenMst.SyotiNyuyojiKbn);

                                if (itemCd != "")
                                {

                                    wrkDtlGrpItems =
                                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                            p.RaiinNo == wrkDtl.RaiinNo &&
                                            p.HokenKbn == wrkDtl.HokenKbn &&
                                            p.RpNo == wrkDtl.RpNo &&
                                            p.SeqNo == wrkDtl.SeqNo &&
                                            p.ItemCd == itemCd &&
                                            p.IsAutoAdd == 1);

                                    foreach (WrkSinKouiDetailModel wrkDtlGrpItem in wrkDtlGrpItems)
                                    {
                                        wrkDtlGrpItem.IsDeleted = DeleteStatus.DeleteFlag;

                                        if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                                    p.RpNo == wrkDtlGrpItem.RpNo && p.SeqNo == wrkDtlGrpItem.SeqNo && p.RowNo == wrkDtlGrpItem.RowNo &&
                                                    p.ItemCd == wrkDtlGrpItem.ItemCd && p.IsWarning == 0))
                                        {
                                            // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                                        }
                                        else
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetailDel(
                                                hokenKbn: wrkDtlGrpItem.HokenKbn,
                                                rpNo: wrkDtlGrpItem.RpNo,
                                                seqNo: wrkDtlGrpItem.SeqNo,
                                                rowNo: wrkDtlGrpItem.RowNo,
                                                itemCd: wrkDtlGrpItem.ItemCd,
                                                delItemCd: ItemCdConst.NoSantei,
                                                santeiDate: 0,
                                                delSbt: DelSbtConst.Fuzui,
                                                isWarning: 0,
                                                termCnt: 0,
                                                termSbt: 0,
                                                isAutoAdd: wrkDtlGrpItem.IsAutoAdd,
                                                hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtlGrpItem.RpNo, wrkDtlGrpItem.SeqNo)
                                                );
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                // 自動追加コメント（実施日とか）
                //if (string.IsNullOrEmpty(wrkDtl.CmtOpt) == false)
                if (string.IsNullOrEmpty(wrkDtl.BaseItemCd) == false && wrkDtl.BaseSeqNo > 0)
                {
                    if (//autoAddCommentls.Contains(wrkDtl.ItemCd) &&
                        _common.Wrk.wrkSinKouiDetails.Any(p =>
                        p.RaiinNo == wrkDtl.RaiinNo &&
                        p.HokenKbn == wrkDtl.HokenKbn &&
                        p.RpNo == wrkDtl.RpNo &&
                        p.SeqNo == wrkDtl.BaseSeqNo &&
                        //p.ItemCd == wrkDtl.CmtOpt &&
                        p.ItemCd == wrkDtl.BaseItemCd &&
                        p.IsDeleted == DeleteStatus.None) == false)
                    {
                        // 元の項目が削除されている場合は削除する
                        wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;

                        if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                p.RpNo == wrkDtl.RpNo && p.SeqNo == wrkDtl.SeqNo && p.RowNo == wrkDtl.RowNo &&
                                p.ItemCd == wrkDtl.ItemCd && p.IsWarning == 0))
                        {
                            // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                        }
                        else
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetailDel(
                                hokenKbn: wrkDtl.HokenKbn,
                                rpNo: wrkDtl.RpNo,
                                seqNo: wrkDtl.SeqNo,
                                rowNo: wrkDtl.RowNo,
                                itemCd: wrkDtl.ItemCd,
                                delItemCd: ItemCdConst.NoSantei,
                                santeiDate: 0,
                                delSbt: DelSbtConst.Fuzui,
                                isWarning: 0,
                                termCnt: 0,
                                termSbt: 0,
                                isAutoAdd: wrkDtl.IsAutoAdd,
                                hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo)
                            );
                        }
                    }
                }
                else if (string.IsNullOrEmpty(wrkDtl.BaseItemCd) == false)
                {
                    if (//autoAddCommentls.Contains(wrkDtl.ItemCd) &&
                        _common.Wrk.wrkSinKouiDetails.Any(p =>
                        p.RaiinNo == wrkDtl.RaiinNo &&
                        p.HokenKbn == wrkDtl.HokenKbn &&
                        p.RpNo == wrkDtl.RpNo &&
                        p.SeqNo == wrkDtl.SeqNo &&
                        //p.ItemCd == wrkDtl.CmtOpt &&
                        p.ItemCd == wrkDtl.BaseItemCd &&
                        p.IsDeleted == DeleteStatus.None) == false)
                    {
                        // 元の項目が削除されている場合は削除する
                        wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;

                        if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                p.RpNo == wrkDtl.RpNo && p.SeqNo == wrkDtl.SeqNo && p.RowNo == wrkDtl.RowNo &&
                                p.ItemCd == wrkDtl.ItemCd && p.IsWarning == 0))
                        {
                            // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                        }
                        else
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetailDel(
                                hokenKbn: wrkDtl.HokenKbn,
                                rpNo: wrkDtl.RpNo,
                                seqNo: wrkDtl.SeqNo,
                                rowNo: wrkDtl.RowNo,
                                itemCd: wrkDtl.ItemCd,
                                delItemCd: ItemCdConst.NoSantei,
                                santeiDate: 0,
                                delSbt: DelSbtConst.Fuzui,
                                isWarning: 0,
                                termCnt: 0,
                                termSbt: 0,
                                isAutoAdd: wrkDtl.IsAutoAdd,
                                hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo)
                            );
                        }
                    }
                }

            }

            #endregion

            // まず、詳細から作業する
            foreach (WrkSinKouiDetailModel wrkDtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.IsDeleted == DeleteStatus.None))
            {
                _delDetail(wrkDtl);
            }

            #region 注射手技料と薬剤のチェック

            foreach (WrkSinKouiDetailModel wrkDtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.IsDeleted == DeleteStatus.None && chusyaSyugiItemCds.Contains(p.ItemCd)))
            {
                if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                            p.RaiinNo == wrkDtl.RaiinNo &&
                            p.HokenKbn == wrkDtl.HokenKbn &&
                            p.RpNo == wrkDtl.RpNo &&
                            (p.RecId == "IY" || p.RecId == "TO") &&
                            p.IsDeleted == DeleteStatus.None) == false)
                {
                    wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;
                    _common.Wrk.AppendNewWrkSinKouiDetailDel
                        (hokenKbn: wrkDtl.HokenKbn,
                            rpNo: wrkDtl.RpNo,
                            seqNo: wrkDtl.SeqNo,
                            rowNo: wrkDtl.RowNo,
                            itemCd: wrkDtl.ItemCd,
                            delItemCd: "9999999998",
                            santeiDate: _common.sinDate,
                            delSbt: DelSbtConst.ChusyaYakuzai,
                            isWarning: 0,
                            termCnt: 0,
                            termSbt: 0,
                            isAutoAdd: wrkDtl.IsAutoAdd,
                            hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtl.RpNo, wrkDtl.SeqNo));

                    _delDetail(wrkDtl);
                }
            }
            #endregion

            // KOUIの削除
            foreach (WrkSinKouiModel wrkKoui in _common.Wrk.wrkSinKouis.FindAll(p => p.IsDeleted == DeleteStatus.None))
            {
                if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                            p.RaiinNo == wrkKoui.RaiinNo &&
                            p.HokenKbn == wrkKoui.HokenKbn &&
                            p.RpNo == wrkKoui.RpNo &&
                            p.SeqNo == wrkKoui.SeqNo &&
                            p.IsDeleted == DeleteStatus.DeleteFlag))
                {
                    // 削除されていない項目を探す
                    List<WrkSinKouiDetailModel> wrkDtls =
                        _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                            p.RaiinNo == wrkKoui.RaiinNo &&
                            p.HokenKbn == wrkKoui.HokenKbn &&
                            p.RpNo == wrkKoui.RpNo &&
                            p.SeqNo == wrkKoui.SeqNo &&
                            p.IsDeleted == DeleteStatus.None);

                    if (wrkDtls.Any() == false)
                    {
                        // 削除されていない項目が存在しない場合、行為を削除
                        wrkKoui.IsDeleted = DeleteStatus.DeleteFlag;
                    }
                    else
                    {
                        // 削除項目を含むKOUIの場合、調整

                        if (
                            //wrkDtls.Any(p =>
                            //     p.RecId != "CO" &&
                            //     !(p.RecId == "SI" && new string[] {"7","9"}.Contains(p.Kokuji1))) == false

                            _common.Wrk.wrkSinKouiDetails.Any(p =>
                                p.RaiinNo == wrkKoui.RaiinNo &&
                                p.HokenKbn == wrkKoui.HokenKbn &&
                                p.RpNo == wrkKoui.RpNo &&
                                p.RecId != "CO" &&
                                !(p.RecId == "SI" && new string[] { "7", "9" }.Contains(p.Kokuji1)) &&
                                p.BuiKbn == 0 &&
                                p.IsDeleted == DeleteStatus.None) == false                            
                           )
                        {
                            // 同一Rpに、コメント以外 and 加算項目以外 and 部位以外 の項目がない
                            bool kouiDel = true;
                            for (int i = 0; i < wrkDtls.Count; i++)
                            {
                                if(_common.Wrk.GetSyukeiSaki(wrkKoui.RaiinNo, wrkKoui.RpNo, wrkKoui.SeqNo) == ReceSyukeisaki.SonotaSyohoSenComment)
                                {
                                    // 処方箋のコメントは残す
                                    kouiDel = false;
                                    continue;
                                }
                                wrkDtls[i].IsDeleted = DeleteStatus.DeleteFlag;

                                if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                                         p.RpNo == wrkDtls[i].RpNo && p.SeqNo == wrkDtls[i].SeqNo && p.RowNo == wrkDtls[i].RowNo &&
                                         p.ItemCd == wrkDtls[i].ItemCd && p.IsWarning == 0))
                                {
                                    // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                                }
                                else
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetailDel(
                                        hokenKbn: wrkDtls[i].HokenKbn,
                                        rpNo: wrkDtls[i].RpNo,
                                        seqNo: wrkDtls[i].SeqNo,
                                        rowNo: wrkDtls[i].RowNo,
                                        itemCd: wrkDtls[i].ItemCd,
                                        delItemCd: ItemCdConst.NoSantei,
                                        santeiDate: 0,
                                        delSbt: DelSbtConst.Fuzui,
                                        isWarning: 0,
                                        termCnt: 0,
                                        termSbt: 0,
                                        isAutoAdd: wrkDtls[i].IsAutoAdd,
                                        hokenId: _common.Wrk.GetWrkKouiHokenId(wrkDtls[i].RpNo, wrkDtls[i].SeqNo)
                                        );
                                }
                            }

                            if (kouiDel)
                            {
                                wrkKoui.IsDeleted = DeleteStatus.DeleteFlag;
                            }
                        }
                    }
                }
            }

            // 薬剤特材の依り代になるSIレコードが削除された場合、同一オーダーRp内にあった別のSIが生きていればそちらに統合
            // 今のところ検査だけ対応、OdrRpNoをセットする処理を追加すれば他の行為にも適用可能
            foreach (WrkSinKouiModel wrkKoui in _common.Wrk.wrkSinKouis.FindAll(p => p.IsDeleted == DeleteStatus.None && new string[] { "IY", "TO" }.Contains(p.RecId) && p.OdrRpNo > 0))
            {
                if (_common.Wrk.wrkSinKouis.Any(p => p.RpNo == wrkKoui.RpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.DeleteFlag))
                {
                    // 同一RpNo内に削除されたSIが存在する
                    if (_common.Wrk.wrkSinKouis.Any(p => p.RpNo == wrkKoui.RpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.None) == false)
                    {
                        // 同一RpNo内に削除されていないSIが存在しない
                        if (_common.Wrk.wrkSinKouis.Any(p => p.OdrRpNo == wrkKoui.OdrRpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.None))
                        {
                            // 同一OdrRpNoでSIで削除されていない行為が存在する場合、入れ替え
                            int newRpNo = _common.Wrk.wrkSinKouis.Find(p => p.OdrRpNo == wrkKoui.OdrRpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.None).RpNo;
                            int newSeqNo = _common.Wrk.wrkSinKouis.Where(p => p.OdrRpNo == wrkKoui.OdrRpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.None).Max(p => p.SeqNo) + 1;

                            foreach (WrkSinKouiDetailModel wrkDtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RpNo == wrkKoui.RpNo && p.SeqNo == wrkKoui.SeqNo))
                            {
                                wrkDtl.RpNo = newRpNo;
                                wrkDtl.SeqNo = newSeqNo;
                            }
                            wrkKoui.RpNo = newRpNo;
                            wrkKoui.SeqNo = newSeqNo;
                        }
                    }
                }
            }
            //-----------

            foreach (WrkSinKouiModel wrkKoui in _common.Wrk.wrkSinKouis.FindAll(p => p.IsDeleted == DeleteStatus.DeleteFlag && new string[] { "SI" }.Contains(p.RecId) && p.OdrRpNo > 0))
            {
                if (_common.Wrk.wrkSinKouiDetails.Any(p => p.RpNo == wrkKoui.RpNo && p.SeqNo == wrkKoui.SeqNo && p.RecId == "CO"))
                {
                    //if (_common.Wrk.wrkSinKouis.Any(p => p.RpNo == wrkKoui.RpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.None) == false)
                    //{
                    // 同一RpNo内に削除されていないSIが存在しない
                    if (_common.Wrk.wrkSinKouis.Any(p => p.OdrRpNo == wrkKoui.OdrRpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.None))
                    {
                        // 同一OdrRpNoでSIで削除されていない行為が存在する場合、入れ替え
                        int newRpNo = _common.Wrk.wrkSinKouis.Find(p => p.OdrRpNo == wrkKoui.OdrRpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.None).RpNo;
                        int newSeqNo = _common.Wrk.wrkSinKouis.Where(p => p.OdrRpNo == wrkKoui.OdrRpNo && p.RecId == "SI" && p.IsDeleted == DeleteStatus.None).Max(p => p.SeqNo);
                        int newItemSeqNo = 0;
                        int newItemEdaNo = 0;
                        if (_common.Wrk.wrkSinKouiDetails.Any(p => p.RpNo == newRpNo && p.SeqNo == newSeqNo && p.IsDeleted == DeleteStatus.None && p.RecId == "SI"))
                        {
                            newItemSeqNo = _common.Wrk.wrkSinKouiDetails.FirstOrDefault(p => p.RpNo == newRpNo && p.SeqNo == newSeqNo && p.IsDeleted == DeleteStatus.None && p.RecId == "SI").ItemSeqNo;
                            newItemEdaNo = _common.Wrk.wrkSinKouiDetails.Where(p => p.RpNo == newRpNo && p.SeqNo == newSeqNo && p.ItemSeqNo == newItemSeqNo && p.IsDeleted == DeleteStatus.None).Max(p => p.ItemEdaNo) + 1;
                        }
                        else
                        {
                            newItemSeqNo = _common.Wrk.wrkSinKouiDetails.Where(p => p.RpNo == newRpNo && p.SeqNo == newSeqNo && p.IsDeleted == DeleteStatus.None).Max(p => p.ItemSeqNo);
                            newItemEdaNo = 1;
                        }

                        foreach (WrkSinKouiDetailModel wrkDtl in _common.Wrk.wrkSinKouiDetails.FindAll(p => p.RpNo == wrkKoui.RpNo && p.SeqNo == wrkKoui.SeqNo && p.RecId == "CO"))
                        {
                            _common.Wrk.wrkSinKouiDetailDels.RemoveAll(p => p.RpNo == wrkDtl.RpNo && p.SeqNo == wrkDtl.SeqNo && p.RowNo == wrkDtl.RowNo);
                            wrkDtl.IsDeleted = DeleteStatus.None;

                            wrkDtl.RpNo = newRpNo;
                            wrkDtl.SeqNo = newSeqNo;
                            int newRowNo = _common.Wrk.wrkSinKouiDetails.Where(p => p.RpNo == newRpNo && p.SeqNo == newSeqNo && p.IsDeleted == DeleteStatus.None).Max(p => p.RowNo) + 1;
                            wrkDtl.RowNo = newRowNo;

                            wrkDtl.ItemSeqNo = newItemSeqNo;
                            wrkDtl.ItemEdaNo = newItemEdaNo;

                            newItemEdaNo++;
                        }
                    }
                    //}
                }
            }

            ///--------
            // RPの削除
            //List<WrkSinRpInfModel> wrkRps = _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == _common.raiinNo && p.SanteiKbn == SanteiKbnConst.Santei);
            List<WrkSinRpInfModel> wrkRps = _common.Wrk.wrkSinRpInfs.FindAll(p => p.RaiinNo == _common.raiinNo && checkSanteiKbn.Contains(p.SanteiKbn) && p.IsDeleted == DeleteStatus.None);
            foreach (WrkSinRpInfModel wrkRp in wrkRps)
            {
                if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                         p.RaiinNo == wrkRp.RaiinNo &&
                         p.HokenKbn == wrkRp.HokenKbn &&
                         p.RpNo == wrkRp.RpNo &&
                         p.IsDeleted == DeleteStatus.DeleteFlag))
                {
                    // 削除項目を含むRPの場合、調整
                    List<WrkSinKouiModel> wrkKouis =
                        _common.Wrk.wrkSinKouis.FindAll(p =>
                            p.RaiinNo == wrkRp.RaiinNo &&
                            p.HokenKbn == wrkRp.HokenKbn &&
                            p.RpNo == wrkRp.RpNo &&
                            p.IsDeleted == DeleteStatus.None);

                    if (wrkKouis.Any() == false)
                    {
                        // 削除されていない行為が存在しない場合、Rpを削除
                        wrkRp.IsDeleted = DeleteStatus.DeleteFlag;
                    }
                    else
                    {
                        List<WrkSinKouiDetailModel> wrkDtls =
                            _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                                p.RaiinNo == wrkRp.RaiinNo &&
                                p.HokenKbn == wrkRp.HokenKbn &&
                                p.RpNo == wrkRp.RpNo &&
                                p.IsDeleted == DeleteStatus.None);

                        if (wrkDtls.Any() == false)
                        {
                            // 削除されていない項目が存在しない場合、Rpを削除
                            wrkRp.IsDeleted = DeleteStatus.DeleteFlag;
                        }
                        else
                        {
                            //if (wrkDtls.Any(p =>
                            //         new string[] { "1", "3", "5" }.Contains(p.Kokuji1) ||
                            //         p.RecId == "IY" ||
                            //         p.RecId == "TO") == false)
                            //{
                            //    // 基本項目、薬剤、特材がない
                            //    for (int i = 0; i < wrkDtls.Count; i++)
                            //    {
                            //        wrkDtls[i].IsDeleted = 1;

                            //        if (_common.Wrk.wrkSinKouiDetailDels.Any(p =>
                            //                 p.RpNo == wrkDtls[i].RpNo && p.SeqNo == wrkDtls[i].SeqNo && p.RowNo == wrkDtls[i].RowNo &&
                            //                 p.ItemCd == wrkDtls[i].ItemCd && p.IsWarning == 0))
                            //        {
                            //            // 既にWRK_SIN_KOUI_DETAIL_DELに登録がある場合は登録しない
                            //        }
                            //        else
                            //        {
                            //            _common.Wrk.AppendNewWrkSinKouiDetailDel(
                            //                hokenKbn: wrkDtls[i].HokenKbn,
                            //                rpNo: wrkDtls[i].RpNo,
                            //                seqNo: wrkDtls[i].SeqNo,
                            //                rowNo: wrkDtls[i].RowNo,
                            //                itemCd: wrkDtls[i].ItemCd,
                            //                delItemCd: "9999999999",
                            //                santeiDate: 0,
                            //                delSbt: 3,
                            //                isWarning: 0,
                            //                termCnt: 0,
                            //                termSbt: 0
                            //                );
                            //        }
                            //    }

                            //    wrkRp.IsDeleted = 1;
                            //}
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ログ出力処理
        /// </summary>
        private void OutputLog()
        {
            int logSbt;
            string logText = "";

            // ワーク診療行為詳細削除情報とワーク診療行為詳細を結合
            // IsAutoAddを取得
            var dtldels =
                _common.Wrk.wrkSinKouiDetailDels.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    p.DelItemCd != ItemCdConst.NoSantei
                );

            var dtls = _common.Wrk.wrkSinKouiDetails;

            var joinQuery = (
                from dtldel in dtldels
                join dtl in dtls on
                    new { dtldel.HpId, dtldel.PtId, dtldel.SinDate, dtldel.RaiinNo, dtldel.RpNo, dtldel.SeqNo, dtldel.RowNo } equals
                    new { dtl.HpId, dtl.PtId, dtl.SinDate, dtl.RaiinNo, dtl.RpNo, dtl.SeqNo, dtl.RowNo }
                orderby dtldel.RpNo, dtldel.SeqNo, dtldel.RowNo, dtldel.IsWarning
                select new { wrkDtlDtl = dtldel, isAutoAdd = dtl.IsAutoAdd }
                 );

            if (joinQuery != null)
            {
                List<WrkSinKouiDetailDelModel> wrkDtlDells;

                // ログ出力モードの確認
                if (_systemConfigProvider.GetHoukatuHaihanLogputMode() == 1)
                {
                    // 自動発生項目は省く
                    wrkDtlDells =
                        joinQuery.Where(p => p.isAutoAdd == 0).Select(p => p.wrkDtlDtl).ToList();
                }
                else
                {
                    wrkDtlDells =
                        joinQuery.Select(p => p.wrkDtlDtl).ToList();
                }

                // 条件付きのログを除く
                if (_systemConfigProvider.GetHoukatuHaihanSPJyokenLogputMode() == 0)
                {
                    wrkDtlDells =
                        wrkDtlDells.FindAll(p => !(new int[] { 0, 1, 2, 4 }.Contains(p.DelSbt) && (new int[] { 1,3 }.Contains(p.IsWarning))));
                }

                List<(string itemCd, string delItemcd, int delSbt, int isWarning, int termCnt, int termSbt, int isAutoAdd, int hokenId, List<string> delItemCds)> logData =
                    new List<(string itemCd, string delItemcd, int delSbt, int isWarning, int termCnt, int termSbt, int isAutoAdd, int hokenId, List<string>)>();

                foreach (WrkSinKouiDetailDelModel wrkDtlDel in wrkDtlDells)
                {
                    if (new int[] { 2, 3 }.Contains(wrkDtlDel.IsWarning) &&
                        (logData.Any(p =>
                           p.itemCd == wrkDtlDel.ItemCd &&
                           p.delItemcd == wrkDtlDel.DelItemCd &&
                           p.isWarning == wrkDtlDel.IsWarning &&
                           p.termCnt == wrkDtlDel.TermCnt &&
                           p.termSbt == wrkDtlDel.TermSbt) ||
                          logData.Any(p =>
                            p.delItemcd == wrkDtlDel.ItemCd &&
                            p.itemCd == wrkDtlDel.DelItemCd &&
                            p.isWarning == wrkDtlDel.IsWarning &&
                            p.termCnt == wrkDtlDel.TermCnt &&
                            p.termSbt == wrkDtlDel.TermSbt)))
                    {
                        // どちらか１つ、で、すでに同一組み合わせのメッセージが存在する場合は読み飛ばす
                    }
                    else if (_systemConfigProvider.GetHoukatuHaihanCheckMode() == 0 ||
                        logData.Any(p => p.itemCd == wrkDtlDel.ItemCd) == false)
                    {
                        logData.Add(
                            (
                                wrkDtlDel.ItemCd,
                                wrkDtlDel.DelItemCd,
                                wrkDtlDel.DelSbt,
                                wrkDtlDel.IsWarning,
                                wrkDtlDel.TermCnt,
                                wrkDtlDel.TermSbt,
                                wrkDtlDel.IsAutoAdd,
                                wrkDtlDel.HokenId,
                                wrkDtlDel.DelItemCds
                            ));
                    }
                }

                var unqLogDatas = logData.Distinct();

                if (unqLogDatas != null)
                {
                    foreach ((string itemCd, string delItemCd, int delSbt, int isWarning, int termCnt, int termSbt, int isAutoAdd, int hokenId, List<string> delItemCds) unqLogData in unqLogDatas)
                    {
                        logText = "";

                        if (unqLogData.isWarning == 0)
                        {
                            // 削除
                            logSbt = 2;
                        }
                        else
                        {
                            // 警告
                            logSbt = 1;
                        }

                        List<TenMstModel> tenMsts;

                        if (new int[] { DelSbtConst.Houkatu, DelSbtConst.HoukatuTokusyu }.Contains(unqLogData.delSbt))
                        {
                            // 包括
                            string item1;
                            string item2;
                            // 期間
                            string termText;

                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts == null || tenMsts.Any() == false || tenMsts.FirstOrDefault().MasterSbt != "C")
                            {
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    item1 = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    item1 = unqLogData.itemCd;
                                }
                                tenMsts = _common.Mst.GetTenMst(unqLogData.delItemCd);
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    item2 = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    item2 = unqLogData.delItemCd;
                                }

                                termText = MakeTermText(unqLogData.termSbt, unqLogData.termCnt);

                                if (unqLogData.isAutoAdd == 0)
                                {
                                    logText = String.Format(FormatConst.HokatuLog[unqLogData.isWarning], item1, item2, termText);
                                }
                                else
                                {
                                    logText = String.Format(FormatConst.HokatuLogAuto[unqLogData.isWarning], item1, item2, termText);
                                }
                            }
                        }
                        else if (new int[] { DelSbtConst.Haihan, DelSbtConst.HaihanTokusyu }.Contains(unqLogData.delSbt))
                        {
                            // 背反

                            // 他の項目に削除される項目の名称
                            string vanishItemName;
                            // 他の項目を削除する項目の名称
                            string delItemName;
                            // 期間
                            string termText;

                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts == null || tenMsts.Any() == false || tenMsts.FirstOrDefault().MasterSbt != "C")
                            {
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    vanishItemName = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    vanishItemName = unqLogData.itemCd;
                                }
                                tenMsts = _common.Mst.GetTenMst(unqLogData.delItemCd);
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    delItemName = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    delItemName = unqLogData.delItemCd;
                                }

                                //期間を表す文字列を取得する
                                termText = MakeTermText(unqLogData.termSbt, unqLogData.termCnt);

                                if (unqLogData.isAutoAdd == 0)
                                {
                                    logText = String.Format(FormatConst.HaihanLog[unqLogData.isWarning], vanishItemName, delItemName, termText);
                                }
                                else
                                {
                                    logText = String.Format(FormatConst.HaihanLogAuto[unqLogData.isWarning], vanishItemName, delItemName, termText);
                                }
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.GairaiKanri)
                        {
                            // 外来管理加算
                            if (unqLogData.isAutoAdd == 0)
                            {
                                logText = FormatConst.GairaiKanriLogNotSantei;
                            }
                            else
                            {
                                logText = FormatConst.GairaiKanriLogDontSantei;
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.Yuusen)
                        {
                            // 優先順背反
                            // 他の項目に削除される項目の名称
                            string vanishItemName;
                            // 他の項目を削除する項目の名称
                            string delItemName = "";
                            // 期間
                            string termText;

                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts == null || tenMsts.Any() || tenMsts.FirstOrDefault().MasterSbt != "C")
                            {
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    vanishItemName = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    vanishItemName = unqLogData.itemCd;
                                }

                                foreach (string delItemCd in unqLogData.delItemCds)
                                {
                                    tenMsts = _common.Mst.GetTenMst(delItemCd);
                                    if (tenMsts != null && tenMsts.Any())
                                    {
                                        if (delItemName != "")
                                        {
                                            delItemName += "' と '";
                                        }
                                        delItemName += tenMsts.FirstOrDefault().ReceName;
                                    }
                                }

                                //期間を表す文字列を取得する
                                termText = MakeTermText(unqLogData.termSbt, unqLogData.termCnt);

                                if (unqLogData.isAutoAdd == 0)
                                {
                                    logText = String.Format(FormatConst.HaihanLog[unqLogData.isWarning], vanishItemName, delItemName, termText);
                                }
                                else
                                {
                                    logText = String.Format(FormatConst.HaihanLogAuto[unqLogData.isWarning], vanishItemName, delItemName, termText);
                                }
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.ChuKasan)
                        {
                            // 注加算対象基本項目なし
                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts.Any())
                            {
                                if (unqLogData.isAutoAdd == 0)
                                {
                                    logText = String.Format(FormatConst.TyuKasanNoTargetNotSantei, tenMsts.First().ReceName);
                                }
                                else
                                {
                                    logText = String.Format(FormatConst.TyuKasanNoTargetDontSantei, tenMsts.First().ReceName);
                                }
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.ChuKasanWrn)
                        {
                            // 注加算対象基本項目なし警告
                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts.Any())
                            {
                                logText = String.Format(FormatConst.TyuKasanNoTargetWarning, tenMsts.First().ReceName);
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.GairaiDouitu)
                        {
                            // 外来管理加算（同一診療）
                            if (unqLogData.isAutoAdd == 0)
                            {
                                logText = FormatConst.GairaiKanriLogDouituRaiinNotSantei;
                            }
                            else
                            {
                                logText = FormatConst.GairaiKanriLogDouituRaiinDontSantei;
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.NoExists)
                        {
                            // ある項目が存在しないために算定できない場合
                            string item1;
                            string item2;

                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts == null || tenMsts.Any() || tenMsts.FirstOrDefault().MasterSbt != "C")
                            {
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    item1 = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    item1 = unqLogData.itemCd;
                                }

                                tenMsts = _common.Mst.GetTenMst(unqLogData.delItemCd);
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    item2 = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    item2 = unqLogData.delItemCd;
                                }

                                if (unqLogData.isAutoAdd == 0)
                                {
                                    logText = String.Format(FormatConst.NoExist, item1, item2);
                                }
                                else
                                {
                                    logText = String.Format(FormatConst.NoExistAuto, item1, item2);
                                }
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.NoExistsWarning)
                        {
                            // ある項目が存在しないために算定できない可能性がある場合
                            string item1;
                            string item2;

                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts == null || tenMsts.Any() || tenMsts.FirstOrDefault().MasterSbt != "C")
                            {
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    item1 = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    item1 = unqLogData.itemCd;
                                }

                                tenMsts = _common.Mst.GetTenMst(unqLogData.delItemCd);
                                if (tenMsts != null && tenMsts.Any())
                                {
                                    item2 = tenMsts.FirstOrDefault().ReceName;
                                }
                                else
                                {
                                    item2 = unqLogData.delItemCd;
                                }

                                logText = String.Format(FormatConst.NoExistWarning, item1, item2);
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.NoKihon)
                        {
                            // 加算基本項目なし
                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts.Any())
                            {
                                if (unqLogData.isAutoAdd == 0)
                                {
                                    logText = String.Format(FormatConst.KasanNoKihon, tenMsts.First().ReceName);
                                }
                                else
                                {
                                    logText = String.Format(FormatConst.KasanNoKihonAuto, tenMsts.First().ReceName);
                                }
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.NoKihonWrn)
                        {
                            // 加算基本項目なし警告
                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts.Any())
                            {
                                logText = String.Format(FormatConst.KasanNoKihonWarning, tenMsts.First().ReceName);
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.JibaiBunsyo)
                        {
                            // 自賠文書料を自賠・自費以外の保険で算定しようとした場合、算定不可
                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts.Any())
                            {
                                logText = String.Format(FormatConst.JibaiBunsyoHoken, tenMsts.First().ReceName);
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.ChusyaYakuzai)
                        {
                            // 注射手技、薬剤なし
                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts.Any())
                            {
                                logText = String.Format(FormatConst.NoneChusyaYakuzai, tenMsts.First().ReceName);
                            }
                        }
                        else if (unqLogData.delSbt == DelSbtConst.AfterSyosin)
                        {
                            // 初診から1ヶ月
                            tenMsts = _common.Mst.GetTenMst(unqLogData.itemCd);
                            if (tenMsts.Any())
                            {
                                if (unqLogData.isWarning == 0)
                                {
                                    if (unqLogData.isAutoAdd == 0)
                                    {
                                        logText = String.Format(FormatConst.SanteiSyosin, tenMsts.First().ReceName) + FormatConst.NotSantei;
                                    }
                                    else
                                    {
                                        logText = String.Format(FormatConst.SanteiSyosin, tenMsts.First().ReceName) + FormatConst.DontSantei;
                                    }
                                }
                                else
                                {
                                    logText = String.Format(FormatConst.SanteiSyosin, tenMsts.First().ReceName) + FormatConst.MaybeNotSantei;
                                }
                            }
                        }

                        if (logText != "")
                        {
                            _common.AppendCalcLog(logSbt, logText, unqLogData.itemCd, unqLogData.delItemCd, unqLogData.delSbt, unqLogData.isWarning, unqLogData.termCnt, unqLogData.termSbt, unqLogData.hokenId);
                        }
                    }
                }

            }

            #region Local Method

            // termCnt(期間数)が1の場合、「同」を付けて返す
            // 1以外の場合、(期間数)+期間名を返す
            string GetTermText(int termCnt, string termText)
            {
                string ret = "";

                if (termCnt == 1)
                {
                    ret = "同" + termText;
                }
                else
                {
                    ret = string.Format("{0}{1}", termCnt, termText);
                }

                return ret;
            }

            // 期間種別と期間数から期間を表す文字列を生成する
            string MakeTermText(int termSbt, int termCnt)
            {
                // 期間種別に対応する期間を表す文字列のリスト
                //0:未指定 1:来院 2:日 3:暦週 4:暦月 5:週 6:月 9:患者あたり
                List<string> termls =
                    new List<string>
                    {
                        "",
                        "同来院",
                        "日",
                        "週",
                        "月",
                        "週",
                        "月",
                        "",
                        "",
                        "患者あたり"
                    };

                string ret = "";

                if (new int[] { 1, 9 }.Contains(termSbt))
                {
                    // 1:来院 9:患者当たり
                    ret = termls[termSbt];
                }
                else if (new int[] { 2, 3, 4, 5, 6 }.Contains(termSbt))
                {
                    // 2:日 3:暦週 4:暦月 5:週 6:月
                    ret = GetTermText(termCnt, termls[termSbt]);
                }

                return ret;
            }

            #endregion
        }

        private void Saiketu()
        {
            /// <summary>
            /// 採血料の項目リスト
            /// </summary>
            List<string> Saiketuls =
                new List<string>
                {
                ItemCdConst.KensaBV,
                ItemCdConst.KensaBC
                };

            List<WrkSinKouiDetailModel> wrkDtls =
                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    Saiketuls.Contains(p.ItemCd) &&
                    p.IsAutoAdd == 1 &&
                    p.IsDeleted == DeleteStatus.None);

            if (wrkDtls.Any())
            {
                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                             p.RaiinNo == wrkDtl.RaiinNo &&
                             p.HokenKbn == wrkDtl.HokenKbn &&
                             p.IsDeleted == DeleteStatus.None &&
                             p.TenMst != null &&
                             p.TenMst.SaiketuKbn == 1) == false)
                    {
                        wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;
                    }
                }
            }
        }

        private void Marume()
        {
            if (_common.hokenKbn == HokenSyu.Kenpo && _common.syosaiHokenKbn == HokenKbn.Kokho)
            {
                // 国保
                if (_systemConfigProvider.GetKensaMarumeBuntenKokuho() == 1)
                {
                    MarumeBuntenDevide();
                }
                else
                {
                    MarumeBuntenBundle();
                }
            }
            else
            {
                // 国保以外
                if (_systemConfigProvider.GetKensaMarumeBuntenSyaho() == 1)
                {
                    MarumeBuntenDevide();
                }
                else
                {
                    MarumeBuntenBundle();
                }
            }
        }
        /// <summary>
        /// 項目数による検査まるめ処理
        /// HOKATU_KENSA が 1,2,3,5,6,7,9,10,12 のもの
        /// </summary>
        private void MarumeBuntenBundle()
        {
            #region local method
            // base - minusをして、<0の場合は0を返す
            double _MinusTensu(double baseTensu, double minusTensu)
            {
                double ret = baseTensu;
                ret = ret - minusTensu;
                if (ret < 0)
                {
                    ret = 0;
                }

                return ret;
            }
            #endregion

            for (int santeiKbnIndex = 0; santeiKbnIndex <= 1; santeiKbnIndex++)
            {
                // まるめ検査項目取得
                List<(List<OdrDtlTenModel> odrDtls, int minIndex, int itemCnt)> marumels = new List<(List<OdrDtlTenModel>, int, int)>();

                int findCount = 0;
                string itemCd = "";

                int santeiKbn = 0;
                if (santeiKbnIndex == 1)
                {
                    santeiKbn = 2;
                }

                for (int j = 0; j < HokatuKensaConst.HoukatuKensaList.Count; j++)
                {
                    // 合計点数取得
                    (double totalTen, bool marume) = GetKensaMarumeTensu(j, santeiKbn);

                    // 保険組み合わせのリストを取得し、優先順に並べる
                    HashSet<(int hokenPid, string sortKey)> tmpHokenPidList = new HashSet<(int, string)>();
                    foreach (int hokenPid in _common.Wrk.GetKensaHokenPidList(HokatuKensaConst.HoukatuKensaList[j].kbn, santeiKbn))
                    {
                        tmpHokenPidList.Add((hokenPid, _common.GetSortKey(hokenPid)));
                    }
                    List<(int hokenPid, string sortKey)> hokenPidList = tmpHokenPidList.OrderBy(p => p.sortKey).ToList();

                    int totalCount = 0;
                    int commentRpNo = 0;
                    int commentSeqNo = 0;

                    // 優先度の高い保険組み合わせから順に処理する
                    for (int i = 0; i < hokenPidList.Count; i++)
                    {
                        List<WrkSinKouiModel> wrkKouis =
                        _common.Wrk.wrkSinKouis.FindAll(p =>
                            p.RaiinNo == _common.raiinNo &&
                            p.HokatuKensa == HokatuKensaConst.HoukatuKensaList[j].kbn &&
                            _common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo) == santeiKbn &&
                            p.HokenPid == hokenPidList[i].hokenPid &&
                            p.IsDeleted == DeleteStatus.None);

                        foreach (WrkSinKouiModel wrkKoui in wrkKouis)
                        {
                            findCount = 0;
                            marumels.Clear();

                            // まるめ対象の検査を検索
                            List<WrkSinKouiDetailModel> wrkDtls =
                                _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                                .FindAll(p =>
                                    p.RpNo == wrkKoui.RpNo &&
                                    p.SeqNo == wrkKoui.SeqNo &&
                                    p.TenMst != null &&
                                    p.TenMst.HokatuKensa == HokatuKensaConst.HoukatuKensaList[j].kbn
                                );

                            findCount = wrkDtls.Count();
                            totalCount += findCount;

                            // まるめになる数かどうかチェック
                            itemCd = "";
                            for (int k = 0; k < HokatuKensaConst.HoukatuKensaList[j].ranges.Count; k++)
                            {
                                if (findCount >= HokatuKensaConst.HoukatuKensaList[j].ranges[k].min && findCount <= HokatuKensaConst.HoukatuKensaList[j].ranges[k].max)
                                {
                                    // まるめ条件に合う場合、ITEM_CDを記憶する
                                    itemCd = HokatuKensaConst.HoukatuKensaList[j].ranges[k].item;
                                    break;
                                }
                            }

                            // 検査逓減も含めて検索
                            wrkDtls =
                                _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                                .FindAll(p =>
                                    p.RpNo == wrkKoui.RpNo &&
                                    p.SeqNo == wrkKoui.SeqNo &&
                                    p.TenMst != null &&
                                    (p.TenMst.HokatuKensa == HokatuKensaConst.HoukatuKensaList[j].kbn || p.ItemCd == ItemCdConst.KensaTeigen)
                                );

                            // まるめ項目を追加する
                            if (itemCd != "")
                            {
                                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                                {
                                    wrkDtl.FmtKbn = 1;
                                }

                                _common.Wrk.InsertNewWrkSinKouiDetail(wrkKoui.RpNo, wrkKoui.SeqNo, itemCd, autoAdd: 1, isNodspRece: 1, isNodspRyosyu: 1);
                                if (marume)
                                {
                                    // 全体でまるめの場合の調整処理
                                    //if (i == hokenPidList.Count - 1 || _common.Wrk.wrkSinKouiDetails.Last().Ten > totalTen)
                                    if (i == hokenPidList.Count - 1)
                                    {
                                        // 最後の保険組み合わせで調整
                                        _common.Wrk.wrkSinKouiDetails.Last().Ten = totalTen;
                                        _common.Wrk.wrkSinKouiDetails.Last().AdjustTensu = true;
                                        totalTen = 0;
                                    }
                                    else
                                    {
                                        totalTen = _MinusTensu(totalTen, GetTensu(itemCd));
                                    }
                                }
                                //_common.Wrk.InsertNewWrkSinKouiDetailCommentRecord(wrkKoui.RpNo, wrkKoui.SeqNo, ItemCdConst.CommentFree, "（" + CIUtil.ToWide(findCount.ToString()) + "項目）", fmtKbn: 1);
                            }
                            else if (marume)
                            {
                                // まるめではない場合で、全体ではまるめの場合
                                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                                {
                                    wrkDtl.FmtKbn = 1;

                                    // Rpとしてまるめではない場合、まるめ項目が発生しない
                                    // しかし、WrkToSinで、FmtKbn=1の項目の点数は0に変換される
                                    // なので、0にされないようにここで調整済みのフラグを立てておく
                                    wrkDtl.AdjustTensu = true;
                                }

                                // まるめではない場合で、全体ではまるめの場合、各項目の点数を調整し合計を超えないようにする
                                for (int k = 0; k < wrkDtls.Count(); k++)
                                {
                                    if (wrkDtls[k].Ten >= totalTen ||
                                        ((i == hokenPidList.Count - 1) && (k == wrkDtls.Count() - 1)))
                                    {
                                        // 点数が合計点より高い or 最終レコード
                                        wrkDtls[k].Ten = totalTen;
                                        totalTen = 0;
                                    }
                                    else
                                    {
                                        totalTen = _MinusTensu(totalTen, wrkDtls[k].Ten);
                                    }
                                }
                            }
                            else
                            {
                                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                                {
                                    wrkDtl.FmtKbn = 1;

                                    // Rpとしてまるめではない場合、まるめ項目が発生しない
                                    // しかし、WrkToSinで、FmtKbn=1の項目の点数は0に変換される
                                    // なので、0にされないようにここで調整済みのフラグを立てておく
                                    wrkDtl.AdjustTensu = true;
                                }
                            }

                            commentRpNo = wrkKoui.RpNo;
                            commentSeqNo = wrkKoui.SeqNo;
                        }
                    }

                    if (marume)
                    {
                        _common.Wrk.InsertNewWrkSinKouiDetailCommentRecord(commentRpNo, commentSeqNo, ItemCdConst.CommentFree, "（" + CIUtil.ToWide(totalCount.ToString()) + "項目）", fmtKbn: 0);
                    }
                }
            }
        }
        /// <summary>
        /// 項目数による検査まるめ処理
        /// HOKATU_KENSA が 1,2,3,5,6,7,9,10,12 のもの
        /// </summary>
        private void MarumeBuntenDevide()
        {
            // まるめ検査項目取得
            List<(List<OdrDtlTenModel> odrDtls, int minIndex, int itemCnt)> marumels = new List<(List<OdrDtlTenModel>, int, int)>();
            List<OdrDtlTenModel> retOdrDtl;
            int retMin;
            int retCount;

            int findCount;
            string itemCd;


            for (int j = 0; j < HokatuKensaConst.HoukatuKensaList.Count; j++)
            {
                List<WrkSinKouiModel> wrkKouis =
                    _common.Wrk.wrkSinKouis.FindAll(p =>
                        p.RaiinNo == _common.raiinNo &&
                        p.HokatuKensa == HokatuKensaConst.HoukatuKensaList[j].kbn &&
                        p.IsDeleted == DeleteStatus.None);

                foreach (WrkSinKouiModel wrkKoui in wrkKouis)
                {
                    findCount = 0;
                    marumels.Clear();

                    // まるめ対象の検査を検索
                    List<WrkSinKouiDetailModel> wrkDtls =
                        _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                        .FindAll(p =>
                            p.RpNo == wrkKoui.RpNo &&
                            p.SeqNo == wrkKoui.SeqNo &&
                            p.TenMst != null &&
                            p.TenMst.HokatuKensa == HokatuKensaConst.HoukatuKensaList[j].kbn
                        );

                    findCount = wrkDtls.Count();

                    // まるめになる数かどうかチェック
                    itemCd = "";
                    for (int k = 0; k < HokatuKensaConst.HoukatuKensaList[j].ranges.Count; k++)
                    {
                        if (findCount >= HokatuKensaConst.HoukatuKensaList[j].ranges[k].min && findCount <= HokatuKensaConst.HoukatuKensaList[j].ranges[k].max)
                        {
                            // まるめ条件に合う場合、ITEM_CDを記憶する
                            itemCd = HokatuKensaConst.HoukatuKensaList[j].ranges[k].item;
                            break;
                        }
                    }

                    // 検査逓減も含めて検索
                    wrkDtls =
                        _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                        .FindAll(p =>
                            p.RpNo == wrkKoui.RpNo &&
                            p.SeqNo == wrkKoui.SeqNo &&
                            p.TenMst != null &&
                            (p.TenMst.HokatuKensa == HokatuKensaConst.HoukatuKensaList[j].kbn || p.ItemCd == ItemCdConst.KensaTeigen)
                        );

                    // まるめ項目を追加する
                    if (itemCd != "")
                    {
                        foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                        {
                            wrkDtl.FmtKbn = 1;
                        }

                        _common.Wrk.InsertNewWrkSinKouiDetail(wrkKoui.RpNo, wrkKoui.SeqNo, itemCd, autoAdd: 1, isNodspRece: 1, isNodspRyosyu: 1);
                        _common.Wrk.InsertNewWrkSinKouiDetailCommentRecord(wrkKoui.RpNo, wrkKoui.SeqNo, ItemCdConst.CommentFree, "（" + CIUtil.ToWide(findCount.ToString()) + "項目）", fmtKbn: 1);
                    }

                }

            }

        }
        private (double, bool) GetKensaMarumeTensu(int hokatuKensaIndex, int santeiKbn)
        {
            double totalTen = 0;
            bool marume = false;

            List<WrkSinKouiModel> wrkKouis =
                _common.Wrk.wrkSinKouis.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokatuKensa == HokatuKensaConst.HoukatuKensaList[hokatuKensaIndex].kbn &&
                    _common.Wrk.GetSanteiKbn(p.RaiinNo, p.RpNo) == santeiKbn &&
                    p.IsDeleted == DeleteStatus.None);

            int ItemCount = 0;
            foreach (WrkSinKouiModel wrkKoui in wrkKouis)
            {
                ItemCount +=
                    _common.Wrk.FindWrkSinKouiDetail(FindWrkDtlMode.HokenOnly)
                        .FindAll(p =>
                            p.RpNo == wrkKoui.RpNo &&
                            p.SeqNo == wrkKoui.SeqNo &&
                            p.TenMst != null &&
                            p.TenMst.HokatuKensa == HokatuKensaConst.HoukatuKensaList[hokatuKensaIndex].kbn
                        ).Count();
            }
            // まるめになる数かどうかチェック
            string itemCd = "";
            for (int k = 0; k < HokatuKensaConst.HoukatuKensaList[hokatuKensaIndex].ranges.Count; k++)
            {
                if (ItemCount >= HokatuKensaConst.HoukatuKensaList[hokatuKensaIndex].ranges[k].min && ItemCount <= HokatuKensaConst.HoukatuKensaList[hokatuKensaIndex].ranges[k].max)
                {
                    // まるめ条件に合う場合、ITEM_CDを記憶する
                    itemCd = HokatuKensaConst.HoukatuKensaList[hokatuKensaIndex].ranges[k].item;
                    marume = true;
                    break;
                }
            }
            if (string.IsNullOrEmpty(itemCd) == false)
            {
                List<TenMstModel> tenMsts = _common.masterFinder.FindTenMstByItemCd(_common.hpId, _common.sinDate, itemCd);
                if (tenMsts != null && tenMsts.Any())
                {
                    totalTen = tenMsts.First().Ten;
                }
            }

            return (totalTen, marume);
        }

        private double GetTensu(string itemCd)
        {
            double ret = 0;
            List<TenMstModel> tenMsts = _common.masterFinder.FindTenMstByItemCd(_common.hpId, _common.sinDate, itemCd);
            if (tenMsts != null && tenMsts.Any())
            {
                ret = tenMsts.First().Ten;
            }
            return ret;
        }

        private void Teigen()
        {
            List<WrkSinKouiDetailModel> wrkDtls =
                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    p.TeigenTargetInRaiin &&
                    p.IsAutoAdd == 1 &&
                    p.IsDeleted == DeleteStatus.None);

            if (wrkDtls.Any())
            {
                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    if (wrkDtl.ItemCd == ItemCdConst.KensaTeigen)
                    {
                        if (wrkDtl.TeigenHokatuKbn == 40)
                        {
                            if (_common.Wrk.wrkSinKouiDetails.Count(p =>
                                     p.RaiinNo == wrkDtl.RaiinNo &&
                                     p.HokenKbn == wrkDtl.HokenKbn &&
                                     p.IsDeleted == DeleteStatus.None &&
                                     (p.RpNo != wrkDtl.RpNo || p.SeqNo != wrkDtl.SeqNo || p.RowNo != wrkDtl.RowNo) &&
                                     p.HokatuKbn == wrkDtl.TeigenHokatuKbn &&
                                     p.CdKbn == wrkDtl.TeigenCdKbn &&
                                     p.CdKbnno == wrkDtl.TeigenCdKbnno &&
                                     p.CdEdano == wrkDtl.TeigenCdEdano &&
                                     p.CdKouno == wrkDtl.TeigenCdKouno) <= 1)
                            {
                                wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;
                            }
                        }
                        else
                        {
                            if (_common.Wrk.wrkSinKouiDetails.Count(p =>
                                     p.RaiinNo == wrkDtl.RaiinNo &&
                                     p.HokenKbn == wrkDtl.HokenKbn &&
                                     p.IsDeleted == DeleteStatus.None &&
                                     (p.RpNo != wrkDtl.RpNo || p.SeqNo != wrkDtl.SeqNo || p.RowNo != wrkDtl.RowNo) &&
                                     p.HokatuKbn == wrkDtl.TeigenHokatuKbn) <= 1)
                            {
                                wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;
                            }
                        }
                    }
                    else if (wrkDtl.ItemCd == ItemCdConst.GazoCtMriGensan)
                    {
                        if (_common.Wrk.wrkSinKouiDetails.Count(p =>
                                 p.RaiinNo == wrkDtl.RaiinNo &&
                                 p.HokenKbn == wrkDtl.HokenKbn &&
                                 p.IsDeleted == DeleteStatus.None &&
                                 (p.RpNo != wrkDtl.RpNo || p.SeqNo != wrkDtl.SeqNo || p.RowNo != wrkDtl.RowNo) &&
                                 p.TenMst != null &&
                                 p.TenMst.HokatuKbn == 201) <= 1)
                        {
                            wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;
                        };
                    }
                }
            }
        }

        private void DoujituSaisin()
        {
            List<string> dojituSaisinls = new List<string>
                {
                    ItemCdConst.SaisinDojitu,
                    ItemCdConst.SaisinDojituRousai,
                    ItemCdConst.SaisinDenwaDojitu,
                    ItemCdConst.SaisinDenwaDojituRousai,
                    ItemCdConst.SaisinJouhouDojitu
                };


            List<WrkSinKouiDetailModel> wrkDtls =
                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    dojituSaisinls.Contains(p.ItemCd) &&
                    p.IsAutoAdd == 1 &&
                    p.IsDeleted == DeleteStatus.None);

            if (wrkDtls.Any())
            {
                if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                     p.RaiinNo != _common.raiinNo &&
                     p.HokenKbn == _common.hokenKbn &&
                     ItemCdConst.doujituSaisinCheckitemCds.Contains(p.ItemCd) &&
                     p.IsDeleted == DeleteStatus.None) == false)
                {
                    foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                    {
                        if (wrkDtl.ItemCd == ItemCdConst.SaisinDojitu)
                        {
                            wrkDtl.ItemCd = ItemCdConst.Saisin;
                        }
                        else if (wrkDtl.ItemCd == ItemCdConst.SaisinDojituRousai)
                        {
                            wrkDtl.ItemCd = ItemCdConst.SaisinRousai;
                        }
                        else if (wrkDtl.ItemCd == ItemCdConst.SaisinDenwaDojitu)
                        {
                            wrkDtl.ItemCd = ItemCdConst.SaisinDenwa;
                        }
                        else if (wrkDtl.ItemCd == ItemCdConst.SaisinDenwaDojituRousai)
                        {
                            wrkDtl.ItemCd = ItemCdConst.SaisinDenwaRousai;
                        }
                        else if (wrkDtl.ItemCd == ItemCdConst.SaisinJouhouDojitu)
                        {
                            wrkDtl.ItemCd = ItemCdConst.SaisinJouhou;
                        }
                    }
                }
            }


            void delDojituComment(List<string> commentItems, List<string> targetItems)
            {
                wrkDtls =
                    _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                        p.RaiinNo == _common.raiinNo &&
                        p.HokenKbn == _common.hokenKbn &&
                        commentItems.Contains(p.ItemCd) &&
                        p.IsAutoAdd == 1 &&
                        p.IsDeleted == DeleteStatus.None);

                if (wrkDtls.Any())
                {
                    foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                    {
                        if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                             p.RaiinNo == _common.raiinNo &&
                             p.HokenKbn == _common.hokenKbn &&
                             targetItems.Contains(p.ItemCd) &&
                             p.IsDeleted == DeleteStatus.None) == false)
                        {
                            wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;
                        }
                    }
                }
            }

            delDojituComment(new List<string> { ItemCdConst.CommentSaisinDojitu }, new List<string> { ItemCdConst.SaisinDojitu, ItemCdConst.SaisinDojituRousai });
            delDojituComment(new List<string> { ItemCdConst.CommentDenwaSaisin }, new List<string> { ItemCdConst.SaisinDenwa, ItemCdConst.SaisinDenwa2, ItemCdConst.SaisinDenwaRousai, ItemCdConst.SaisinDenwa2Rousai });
            delDojituComment(new List<string> { ItemCdConst.CommentDenwaSaisinDojitu }, new List<string> { ItemCdConst.SaisinDenwaDojitu, ItemCdConst.SaisinDenwaDojituRousai });
        }

        /// <summary>
        /// 労災電子化加算
        /// </summary>
        private void RousaiDensika()
        {
            List<WrkSinKouiDetailModel> wrkDtls =
                _common.Wrk.wrkSinKouiDetails.FindAll(p =>
                    p.RaiinNo == _common.raiinNo &&
                    p.HokenKbn == _common.hokenKbn &&
                    p.ItemCd == ItemCdConst.SonotaRosaiDensika &&
                    p.IsAutoAdd == 1 &&
                    p.IsDeleted == DeleteStatus.None);

            if (wrkDtls.Any())
            {
                List<string> rosaiDensikals =
                    new List<string>
                    {
                        ItemCdConst.SonotaRosaiDensika,
                        ItemCdConst.SonotaRosaiDensikaCancel
                    };

                foreach (WrkSinKouiDetailModel wrkDtl in wrkDtls)
                {
                    bool santei = false;

                    if (_common.calcMode == CalcModeConst.Trial || _common.Wrk.ExistWrkSinKouiDetailByItemCdRpNo(rosaiDensikals, wrkDtl.RpNo, false) == false)
                    {

                        foreach (WrkSinRpInfModel wrkRp in _common.Wrk.wrkSinRpInfs.FindAll(p =>
                                     p.RaiinNo == wrkDtl.RaiinNo &&
                                     p.HokenKbn == wrkDtl.HokenKbn &&
                                     p.IsDeleted == DeleteStatus.None))
                        {
                            foreach (WrkSinKouiModel wrkKoui in _common.Wrk.wrkSinKouis.FindAll(p =>
                                      p.RaiinNo == wrkRp.RaiinNo &&
                                      p.HokenKbn == wrkRp.HokenKbn &&
                                      p.RpNo == wrkRp.RpNo &&
                                      p.InoutKbn == 0 &&
                                      p.IsDeleted == DeleteStatus.None))
                            {
                                if (_common.Wrk.wrkSinKouiDetails.Any(p =>
                                     p.RaiinNo == wrkKoui.RaiinNo &&
                                     p.RpNo == wrkKoui.RpNo &&
                                     p.SeqNo == wrkKoui.SeqNo &&
                                     p.ItemCd != ItemCdConst.SonotaRosaiDensika &&
                                     p.IsDeleted == DeleteStatus.None &&
                                     p.IsNodspRece == 0))
                                {
                                    santei = true;
                                    break;
                                }
                            }

                            if (santei)
                            {
                                break;
                            }
                        }
                    }

                    if (santei == false)
                    {
                        // 有効なオーダーがない場合は削除
                        wrkDtl.IsDeleted = DeleteStatus.DeleteFlag;
                    }
                }
            }
        }
    }
}
