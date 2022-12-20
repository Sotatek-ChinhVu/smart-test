using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using Infrastructure.Interfaces;
using Helper.Common;
using Domain.Constant;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    /// <summary>
    /// X線撮影診断料算定処理時、項目を整理するためのクラス
    /// </summary>
    class AddItem
    {
        public bool doujiSatuei;
        public List<OdrDtlTenModel> bui;
        public List<OdrDtlTenModel> satuei;
        public List<OdrDtlTenModel> sindan;
        public List<OdrDtlTenModel> sonota;
        public List<OdrDtlTenModel> film;
        public bool isNoKizami;

        public AddItem()
        {
            doujiSatuei = false;
            isNoKizami = false;
            //filmCount = 0;
            bui = new List<OdrDtlTenModel>();
            satuei = new List<OdrDtlTenModel>();
            sindan = new List<OdrDtlTenModel>();
            sonota = new List<OdrDtlTenModel>();
            film = new List<OdrDtlTenModel>();
        }
    }

    /// <summary>
    /// オーダー情報からワーク情報へ変換
    /// 画像
    /// </summary>
    class IkaCalculateOdrToWrkGazoViewModel
    {
        private const string ModuleName = ModuleNameConst.EmrCalculateIka;

        private IkaCalculateCommonDataViewModel _common;

        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="common">共通データ</param>
        public IkaCalculateOdrToWrkGazoViewModel(IkaCalculateCommonDataViewModel common,
            ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _common = common;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        /// <summary>
        /// 計算ロジック
        /// </summary>
        public void Calculate()
        {
            const string conFncName = nameof(Calculate);
            _emrLogger.WriteLogStart( this, conFncName, "");

            if (_common.Odr.ExistOdrKoui(OdrKouiKbnConst.GazoMin, OdrKouiKbnConst.GazoMax))
            { 
                // 保険
                CalculateHoken();

                // 自費
                CalculateJihi();
            }

            _common.Wrk.CommitWrkSinRpInf();

            _emrLogger.WriteLogEnd( this, conFncName, "");
        }

        /// <summary>
        /// 保険分を処理する
        /// </summary>
        private void CalculateHoken()
        {
            const string conFncName = nameof(CalculateHoken);

            // 通常算定処理
            List<OdrInfModel> filteredOdrInf;

            // 画像のRpを取得
            filteredOdrInf = _common.Odr.FilterOdrInfByKouiKbnRange(OdrKouiKbnConst.GazoMin, OdrKouiKbnConst.GazoMax);

            if (filteredOdrInf.Any())
            {
                int santeiKinga = 0;
                int kingaPid = _common.syosaiPid;
                int kingaId = _common.syosaiHokenId;
                int kingaSanteiKbn = _common.syosaiSanteiKbn;

                // 時間外緊急院内画像診断加算算定可能かどうかチェック
                santeiKinga = Kinga();

                bool firstSinryoKoui;
                
                List<OdrDtlTenModel> filteredOdrDtl;

                foreach (OdrInfModel odrInf in filteredOdrInf)
                {

                    // 行為に紐づく詳細を取得
                    filteredOdrDtl = _common.Odr.FilterOdrDetailByRpNo(odrInf.RpNo, odrInf.RpEdaNo);

                    if (filteredOdrDtl.Any())
                    {
                        firstSinryoKoui = true;

                        int firstItem = _common.CheckFirstItemSbt(filteredOdrDtl);
                        // コメントレコードを読み飛ばすフラグ（コメント以外の項目に付随するコメントをくっつけるためのフラグ）
                        bool commentSkipFlg = (firstItem != 0);

                        // 初回、必ずRpと行為のレコードを用意
                        _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Gazo, ReceSinId.Gazo, odrInf.SanteiKbn);

                        // 集計先は、後で内容により変更する
                        _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Gazo, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "E"));

                        if (filteredOdrDtl.Any(p => p.CdKbn == "E" && (p.CdKbnno == 1 || p.CdKbnno == 2)))
                        {
                            // X線撮影料計算ロジックへ
                            if (firstSinryoKoui == true)
                            {
                                firstSinryoKoui = false;
                            }
                            else
                            {
                                _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Gazo, ReceSinId.Gazo, odrInf.SanteiKbn);
                                _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Gazo, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "E"));
                            }

                            XLay(filteredOdrDtl, odrInf.HokenPid, odrInf.HokenId, odrInf.SanteiKbn);

                            if (filteredOdrDtl.Any(p => p.IsKihonKoumoku && !(ItemCdConst.GazoTaisatuei.Contains(p.ItemCd))))
                            {
                                // 他医撮影以外の算定がある場合
                                kingaPid = odrInf.HokenPid;
                                kingaId = odrInf.HokenId;
                                kingaSanteiKbn = odrInf.SanteiKbn;
                                if (santeiKinga != -1)
                                {
                                    santeiKinga = 1;
                                }
                            }

                        }
                        else
                        {

                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                            {
                                if (odrDtl.ItemCd == ItemCdConst.GazoCtMriGensan)
                                {
                                    // ２回目以降減算（ＣＴ、ＭＲＩ）は、後で計算
                                    continue;
                                }

                                //if (odrDtl.IsSorCommentItem(commentSkipFlg) ||
                                //    (_common.hokenKbn != HokenSyu.Jibai && odrDtl.IsFilm))
                                if (odrDtl.IsSorCommentItem(commentSkipFlg))
                                {
                                    if (odrDtl.IsKihonKoumoku)
                                    {
                                        if (firstSinryoKoui == true)
                                        {
                                            firstSinryoKoui = false;
                                        }
                                        else
                                        {
                                            //※画像は同一Rpにオーダーされていれば、一連の行為とみなすようにしておく
                                            //if (odrDtl.Kokuji2 != "7")
                                            //{
                                            //    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Gazo, ReceSinId.Gazo, odrInf.SanteiKbn);
                                            //}
                                            // 画像は行為も分けないようにしておく
                                            //_common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.Gazo, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "E"));
                                        }
                                    }

                                    // 診療行為・コメント or 自賠以外でフィルム
                                    if (!(_common.IsCommentItemCd(odrDtl.ItemCd)))
                                    {
                                        // コメント項目以外
                                        commentSkipFlg = false;
                                        //firstSinryoKoui = false;

                                        // 算定回数チェック
                                        if (_common.CheckSanteiKaisu(odrDtl.ItemCd, odrDtl.SanteiKbn, 0, odrDtl.Suryo) == 2)
                                        {
                                            // 算定回数マスタのチェックにより算定不可
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                        }
                                        else if(_common.CheckAge(odrDtl) == 2)
                                        {
                                            // 年齢チェックにより算定不可
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl), isDeleted: DeleteStatus.DeleteFlag);
                                        }
                                        else
                                        {
                                            _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                            if (_common.IsRosai == false && odrDtl.HokatuKbn == 201)
                                            {
                                                // CT/MRTの逓減チェック
                                                ctMriGensan(odrDtl, filteredOdrDtl);
                                            }

                                            // 年齢加算自動算定
                                            _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, filteredOdrDtl);

                                            // コメント自動追加
                                            _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);
                                        }
                                    }
                                    else
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                    }

                                    if(odrDtl.IsKihonKoumoku && !ItemCdConst.GazoTaisatuei.Contains(odrDtl.ItemCd))
                                    { 
                                        // 時間外緊急院内画像診断加算用に保険組み合わせIDと算定区分を退避
                                        kingaPid = odrInf.HokenPid;
                                        kingaId = odrInf.HokenId;
                                        kingaSanteiKbn = odrInf.SanteiKbn;
                                        if (santeiKinga != -1)
                                        {
                                            santeiKinga = 1;

                                        }
                                    }
                                }
                                else
                                {
                                    commentSkipFlg = true;
                                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.GazoYakuzai, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "E"));
                                }

                            }

                            commentSkipFlg = true;

                            if (firstSinryoKoui == true)
                            {
                                firstSinryoKoui = false;
                                _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.GazoYakuzai;
                            }
                            else
                            {
                                // 行為を追加する
                                _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.GazoYakuzai, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "E"));
                            }
                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                            {
                                if (_common.hokenKbn != HokenSyu.Jibai && odrDtl.IsOnlyFilm || (commentSkipFlg == false && odrDtl.IsComment))
                                {
                                    // 診療行為・コメント or 自賠以外でフィルム
                                    if (odrDtl.IsOnlyFilm)
                                    {
                                        // コメント項目以外
                                        commentSkipFlg = false;

                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    }
                                    else
                                    {
                                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                    }
                                }
                                else
                                {
                                    commentSkipFlg = true;
                                    _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.GazoYakuzai, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "E"));
                                }

                            }

                            // 薬剤・コメント算定

                            commentSkipFlg = (firstItem != 1);

                            if (firstSinryoKoui == true)
                            {
                                firstSinryoKoui = false;
                                _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.GazoYakuzai;
                            }
                            else
                            {
                                // 行為を追加する
                                _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.GazoYakuzai, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "E"));
                            }

                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                            {
                                if (odrDtl.IsYorCommentItem(commentSkipFlg) && odrDtl.ItemCd != ItemCdConst.GazoDensibaitaiHozon)
                                {
                                    // 薬剤・コメント
                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                                    commentSkipFlg = false;
                                }
                                else
                                {
                                    commentSkipFlg = true;
                                }
                            }

                            // 特材・コメント算定

                            commentSkipFlg = (firstItem != 2);

                            if (firstSinryoKoui == true)
                            {
                                firstSinryoKoui = false;
                                _common.Wrk.wrkSinKouis.Last().SyukeiSaki = ReceSyukeisaki.GazoYakuzai;
                            }
                            else
                            {
                                // 行為を追加する
                                _common.Wrk.AppendNewWrkSinKoui(odrInf.HokenPid, odrInf.HokenId, ReceSyukeisaki.GazoYakuzai, cdKbn: _common.GetCdKbn(odrInf.SanteiKbn, "E"));
                            }

                            foreach (OdrDtlTenModel odrDtl in filteredOdrDtl)
                            {
                                if (odrDtl.IsTorCommentItem(commentSkipFlg) && !(_common.hokenKbn != HokenSyu.Jibai && odrDtl.IsOnlyFilm))
                                {
                                    // 特材・コメント

                                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                                    commentSkipFlg = false;
                                }
                                else
                                {
                                    commentSkipFlg = true;
                                }
                            }
                        }
                    }
                }

                // 時間外緊急院内画像診断加算
                if (santeiKinga == 1)
                {
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Gazo, ReceSinId.Gazo, kingaSanteiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(kingaPid, kingaId, ReceSyukeisaki.Gazo, cdKbn: _common.GetCdKbn(kingaSanteiKbn, "E"));
                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoKinga);
                }

                _common.Wrk.CommitWrkSinRpInf();
            }
        }

        /// <summary>
        /// 時間外緊急院内画像診断加算を自動算定するかどうか
        /// </summary>
        /// <returns>-1: 自動算定なし</returns>
        private int Kinga()
        {
            int ret = 0;

            if(_common.jikan != JikanConst.JikanGai &&
               _common.jikan != JikanConst.Kyujitu &&
               _common.jikan != JikanConst.Sinya)
            {
                // 時間外ではない場合は自動算定しない
                ret = -1;
            }
            if(_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.GazoKinga))
            {
                // 手オーダーあり
                ret = -1;
            }
            else if(_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.GazoKingaCancel))
            {
                // キャンセル項目あり
                ret = -1;
            }
            else if(_common.Mst.ExistAutoSantei(ItemCdConst.GazoKinga) == false)
            {
                // 自動算定設定なし
                ret = -1;
            }
            return ret;            
        }

        /// <summary>
        /// X線撮影料計算ロジック
        /// </summary>
        /// <param name="filteredOdrDtl">撮影料/診断料のオーダー詳細</param>
        private void XLay(List<OdrDtlTenModel> filteredOdrDtl, int hokenPid, int hokenId, int santeiKbn)
        {

            // フィルムの数量から撮影料/診断料の回数セット
            // フィルムの上には、撮影料/診断料はそれぞれ0 or 1しか存在しない前提
            double filmCount = 0;

            // フィルムより下に撮影料/診断料が存在する場合にセットする数量
            double setFilmCount = 0;

            for (int i = 0; i < filteredOdrDtl.Count; i++)
            {
                if (filteredOdrDtl[i].IsSindanKizami || filteredOdrDtl[i].IsSatueiKizami)
                {
                    // 撮影料 or 診断料
                    // フィルムを処理する際に確定するので、ここでは一旦0にしておく
                    // ただし、上にあるフィルムの数量を採用する場合もある
                    filteredOdrDtl[i].Suryo = setFilmCount;
                }
                else if (filteredOdrDtl[i].IsFilm)
                {
                    // フィルム
                    filmCount = filteredOdrDtl[i].Suryo * (filteredOdrDtl[i].ShotCnt > 0 ? filteredOdrDtl[i].ShotCnt: 1);

                    bool satueiSet = false;
                    bool sindanSet = false;

                    // 遡って直前の撮影料/診断料に数量を反映する
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (satueiSet == false && filteredOdrDtl[j].IsSatueiKizami)
                        {
                            // すぐ上の撮影料の回数とする
                            filteredOdrDtl[j].Suryo += filmCount;
                            satueiSet = true;

                            if (satueiSet && sindanSet)
                            {
                                
                                break;
                            }
                        }
                        else if (sindanSet == false && filteredOdrDtl[j].IsSindanKizami)
                        {
                            // すぐ上の診断料の回数とする
                            filteredOdrDtl[j].Suryo += filmCount;
                            sindanSet = true;

                            if (satueiSet && sindanSet)
                            {
                                break;
                            }
                        }
                        else if (filteredOdrDtl[j].IsFilm)
                        {
                            // フィルムが来たら抜ける
                            break;
                        }
                        else if(filteredOdrDtl[j].IsSatuei || filteredOdrDtl[j].IsSindan)
                        {
                            // 基本項目が出てきたら抜ける
                            //break;
                        }
                    }

                    // これ以降にフィルムがない場合
                    bool existFilm = false;

                    for (int j = i + 1; j < filteredOdrDtl.Count; j++)
                    {
                        if(filteredOdrDtl[j].IsFilm)
                        {
                            existFilm = true;
                            break;
                        }
                    }

                    if(existFilm == false)
                    {
                        setFilmCount = filmCount;
                    }
                }
            }

            // 各パートごとに分ける

            List<AddItem> xLays = new List<AddItem>();

            AddItem addItem = new AddItem();

            int part = 0;
            int sindanSatueiExist = -1;
            int filmExist = -1;

            List<OdrDtlTenModel> yakuzaiOdrDtls = new List<OdrDtlTenModel>();
            List<OdrDtlTenModel> tokuzaiOdrDtls = new List<OdrDtlTenModel>();

            for(int i = 0; i < filteredOdrDtl.Count; i++)
            {
                bool isYakuzai = false;
                bool isTokuzai = false;

                // 撮影料 or 診断料で、既に同じ項目が追加されていて、この項目以降にフィルムがない場合は追加しない
                bool addOK = true;
                if ((filteredOdrDtl[i].IsSatueiKizami && addItem.satuei.Any(p=>p.ItemCd == filteredOdrDtl[i].ItemCd)) ||
                    (filteredOdrDtl[i].IsSindanKizami && addItem.sindan.Any(p => p.ItemCd == filteredOdrDtl[i].ItemCd)))
                {
                    addOK = false;
                    for (int j = i + 1; j < filteredOdrDtl.Count; j++)
                    {
                        if (filteredOdrDtl[j].IsFilm)
                        {
                            addOK = true;
                            break;
                        }
                    }
                }

                if (addOK)
                {
                    // フィルムの後のフィルム以外の場合で以降に撮影料 or 診断料がある場合、Listを分ける
                    if (part == 4 && filteredOdrDtl[i].IsFilm == false && 
                        filteredOdrDtl[i].IsCommentExcludeBuiComment == false
                        )
                    {
                        //// 撮影料/診断料の存在チェック
                        //if (sindanSatueiExist == -1)
                        //{
                        //    sindanSatueiExist = 0;
                        //    for (int j = i + 1; j < filteredOdrDtl.Count; j++)
                        //    {
                        //        if (filteredOdrDtl[j].IsSindan || filteredOdrDtl[j].IsSatuei)
                        //        {
                        //            sindanSatueiExist = 1;
                        //            break;
                        //        }
                        //    }
                        //}

                        //if (sindanSatueiExist == 1)
                        //{
                        //    // 撮影料か診断料が存在する場合、ここで一区切りとし、Listを分ける
                        //    xLays.Add(addItem);

                        //    addItem = new AddItem();

                        //    part = -1;
                        //    sindanSatueiExist = -1;
                        //}
                        if (filteredOdrDtl[i].IsSatueiKizami == false && filteredOdrDtl[i].IsSindanKizami == false && (filteredOdrDtl[i].IsSatuei || filteredOdrDtl[i].IsSindan))
                        {
                            // 撮影料、診断料以外の基本項目の場合、ここで一区切りとし、Listを分ける
                            xLays.Add(addItem);

                            addItem = new AddItem();
                            addItem.isNoKizami = true;

                            part = -1;
                            sindanSatueiExist = -1;
                            filmExist = -1;
                        }
                        else
                        {
                            // 撮影料/診断料の存在チェック
                            if (sindanSatueiExist == -1)
                            {
                                sindanSatueiExist = 0;
                                filmExist = 0;
                                for (int j = i; j < filteredOdrDtl.Count; j++)
                                {
                                    if (filteredOdrDtl[j].IsSindanKizami || filteredOdrDtl[j].IsSatueiKizami)
                                    {
                                        sindanSatueiExist = 1;
                                    }
                                    else if (filteredOdrDtl[j].IsFilm)
                                    {
                                        filmExist = 1;
                                    }

                                    if (sindanSatueiExist == 1 && filmExist == 1)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (sindanSatueiExist == 1 && filmExist == 1)
                            {
                                // 撮影料か診断料が存在し、フィルムも存在する場合、ここで一区切りとし、Listを分ける
                                xLays.Add(addItem);

                                addItem = new AddItem();

                                part = -1;
                                sindanSatueiExist = -1;
                                filmExist = -1;
                            }
                        }
                    }

                    // 部位項目
                    if (filteredOdrDtl[i].BuiKbn > 0 || filteredOdrDtl[i].IsBuiComment)
                    {
                        part = 0;
                    }
                    else if (filteredOdrDtl[i].IsSatueiKizami)
                    {
                        // 撮影料
                        part = 1;

                        if (filteredOdrDtl[i].KizamiMax == 4)
                        {
                            addItem.doujiSatuei = true;
                        }
                    }
                    else if (filteredOdrDtl[i].IsSindanKizami)
                    {
                        // 診断料
                        part = 2;
                    }
                    //else if (_common.hokenKbn != HokenSyu.Jibai && filteredOdrDtl[i].IsFilm)
                    else if (filteredOdrDtl[i].IsFilm)
                    {
                        // フィルム
                        part = 4;
                    }
                    else if (filteredOdrDtl[i].IsCommentExcludeBuiComment)
                    {
                        // コメント
                        // コメントの場合は前のレコードに付随させる
                    }
                    else if (filteredOdrDtl[i].MasterSbt == "Y")
                    {
                        // 薬剤はここでは算定しない
                        isYakuzai = true;
                    }
                    //else if((filteredOdrDtl[i].MasterSbt == "T" || filteredOdrDtl[i].MasterSbt =="U") && 
                    //    !(_common.hokenKbn == HokenSyu.Jibai && filteredOdrDtl[i].IsFilm))
                    else if (filteredOdrDtl[i].MasterSbt == "T" || filteredOdrDtl[i].MasterSbt == "U")
                    {
                        // フィルム以外の特材はここでは算定しない                    
                        isTokuzai = true;
                    }
                    else
                    {
                        // その他
                        part = 3;
                    }

                    if (isYakuzai)
                    {
                        // 薬剤
                        yakuzaiOdrDtls.Add(filteredOdrDtl[i]);
                    }
                    else if (isTokuzai)
                    {
                        // 特材
                        tokuzaiOdrDtls.Add(filteredOdrDtl[i]);
                    }
                    else
                    {
                        switch (part)
                        {
                            case 0: // 部位
                                addItem.bui.Add(filteredOdrDtl[i]);
                                break;
                            case 1: // 撮影料
                                addItem.satuei.Add(filteredOdrDtl[i]);
                                break;
                            case 2: // 診断料
                                addItem.sindan.Add(filteredOdrDtl[i]);
                                break;
                            case 3: // その他
                                addItem.sonota.Add(filteredOdrDtl[i]);
                                break;
                            case 4: // フィルム
                                if (filteredOdrDtl[i].ItemCd == ItemCdConst.GazoDensibaitaiHozon)
                                {
                                    // 電子媒体保存撮影は、レセプト上はコメントなので、その他に流す
                                    addItem.sonota.Add(filteredOdrDtl[i]);
                                }
                                else
                                {
                                    addItem.film.Add(filteredOdrDtl[i]);
                                }
                                break;

                        }
                    }
                }

            }

            if (part >= 0)
            {
                // 最後の1つ
                xLays.Add(addItem);
            }

            // 算定用データを作成
            List<AddItem> santeiXLays = new List<AddItem>();
            
            foreach (AddItem xlay in xLays)
            {
                if(santeiXLays.Count == 0 || xlay.doujiSatuei == false)
                {
                    // 新規
                    AddItem addSanteiItem = new AddItem();
                    addSanteiItem.doujiSatuei = xlay.doujiSatuei;
                    addSanteiItem.isNoKizami = xlay.isNoKizami;
                    addSanteiItem.bui.AddRange(xlay.bui);
                    addSanteiItem.sindan.AddRange(xlay.sindan);
                    addSanteiItem.satuei.AddRange(xlay.satuei);
                    addSanteiItem.sonota.AddRange(xlay.sonota);
                    addSanteiItem.film.AddRange(xlay.film);
                    santeiXLays.Add(addSanteiItem);
                }
                else
                {
                    // まとめる
                    bool sindanFind = false;
                    int i = 0;

                    // 同じ診断料は1つに統合する
                    while (sindanFind == false && i < santeiXLays.Last().sindan.Count)
                    {
                        if (santeiXLays.Last().sindan[i].IsSindanKizami)
                        {
                            int j = 0;

                            while (sindanFind == false && j < xlay.sindan.Count)
                            {
                                if (santeiXLays.Last().sindan[i].ItemCd == xlay.sindan[j].ItemCd)
                                {
                                    // 同じ診断料が見つかった場合、数量を合算して削除
                                    santeiXLays.Last().sindan[i].Suryo += xlay.sindan[j].Suryo;
                                    xlay.sindan.RemoveAt(j);
                                    sindanFind = true;
                                }
                                j++;
                            }
                        }
                        i++;
                    }

                    santeiXLays.Last().bui.AddRange(xlay.bui);
                    santeiXLays.Last().sindan.AddRange(xlay.sindan);
                    santeiXLays.Last().satuei.AddRange(xlay.satuei);
                    santeiXLays.Last().sonota.AddRange(xlay.sonota);
                    santeiXLays.Last().film.AddRange(xlay.film);
                }
            }

            bool filmNyuKasan = false;
            bool first = true;

            foreach (AddItem  xlay in santeiXLays)
            {
                if (first)
                {
                    first = false;
                }
                else if(xlay.isNoKizami == false)
                {
                    _common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Gazo, ReceSinId.Gazo, santeiKbn);
                    _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.Gazo, cdKbn: _common.GetCdKbn(santeiKbn, "E"));
                }

                // 部位からセット
                foreach (OdrDtlTenModel odrDtl in xlay.bui)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                    if(filmNyuKasan == false && 
                        (_common.IsYoJi && (odrDtl.BuiKbn == 5 || odrDtl.BuiKbn == 6 || odrDtl.CmtSbt == CmtSbtConst.SatueiBuiKyobu || odrDtl.CmtSbt == CmtSbtConst.SatueiBuiFukubu)))
                    {
                        // 胸部または腹部の場合
                        filmNyuKasan = true;
                    }
                }

                // 診断料
                double sindanCount = 5;

                foreach (OdrDtlTenModel odrDtl in xlay.sindan)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                    if (odrDtl.IsSindanKizami)
                    {
                        if (odrDtl.Suryo > sindanCount)
                        {
                            _common.Wrk.wrkSinKouiDetails.Last().Suryo = (sindanCount > 0) ? sindanCount : 1;
                        }
                        else
                        {
                            _common.Wrk.wrkSinKouiDetails.Last().Suryo = (odrDtl.Suryo > 0) ? odrDtl.Suryo : 1;
                        }
                        _common.Wrk.wrkSinKouiDetails.Last().Suryo2 = (odrDtl.Suryo > 0) ? odrDtl.Suryo : 1;
                        _common.Wrk.wrkSinKouiDetails.Last().FmtKbn = FmtKbnConst.GazoSindanSatuei;
                        sindanCount -= odrDtl.Suryo;
                    }

                    // 年齢加算自動算定
                    _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, xlay.sonota);

                    // コメント自動追加
                    _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);
                }

                // 撮影料
                double satueiCount = 5;

                foreach (OdrDtlTenModel odrDtl in xlay.satuei)
                {
                    if (odrDtl.IsSatueiKizami == false ||
                            (odrDtl.IsSatueiKizami &&
                                (odrDtl.Suryo > 0 || (odrDtl.Suryo == 0 && xlay.satuei.Count() == 1))))
                    {
                        _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                        if (odrDtl.IsSatueiKizami)
                        {
                            if (odrDtl.Suryo > satueiCount)
                            {
                                _common.Wrk.wrkSinKouiDetails.Last().Suryo = (satueiCount > 0) ? satueiCount : 1;
                            }
                            else
                            {
                                _common.Wrk.wrkSinKouiDetails.Last().Suryo = (odrDtl.Suryo > 0) ? odrDtl.Suryo : 1;
                            }
                            _common.Wrk.wrkSinKouiDetails.Last().Suryo2 = (odrDtl.Suryo > 0) ? odrDtl.Suryo : 1;
                            _common.Wrk.wrkSinKouiDetails.Last().FmtKbn = FmtKbnConst.GazoSindanSatuei;
                            satueiCount -= odrDtl.Suryo;
                        }

                        // 年齢加算自動算定
                        _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, xlay.sonota);

                        // コメント自動追加
                        _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);
                    }
                }

                // その他
                foreach (OdrDtlTenModel odrDtl in xlay.sonota)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                    
                    if (odrDtl.ItemCd == ItemCdConst.GazoDensibaitaiHozon)
                    {
                        // 電子媒体保存撮影
                        string cmtOpt = "";
                        cmtOpt = CIUtil.ToWide(odrDtl.Suryo.ToString());
                        string itemName = _common.GetCommentStr(odrDtl.ItemCd, ref cmtOpt);

                        _common.Wrk.wrkSinKouiDetails.Last().CmtOpt = cmtOpt;
                        _common.Wrk.wrkSinKouiDetails.Last().Suryo = 1;
                        _common.Wrk.wrkSinKouiDetails.Last().ItemName = itemName;
                    }

                    // 年齢加算自動算定
                    _common.AppendNewWrkSinKouiDetailAgeKasan(odrDtl, xlay.sonota);

                    // コメント自動追加
                    _common.Wrk.AppendNewWrkSinKouiDetailComment(odrDtl, filteredOdrDtl);
                }

                // フィルム
                if (_common.hokenKbn == HokenSyu.Jibai)
                {
                    // 自賠の場合、点数欄集計先は薬剤と一緒
                    _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.GazoYakuzai, cdKbn: _common.GetCdKbn(santeiKbn, "E"));
                }
                else
                {
                    // 健保労災の場合、点数欄の集計先は手技と一緒
                    _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.Gazo, cdKbn: _common.GetCdKbn(santeiKbn, "E"));
                }
                foreach (OdrDtlTenModel odrDtl in xlay.film)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));

                    if (odrDtl.IsFilm)
                    {
                        if (odrDtl.ItemCd == ItemCdConst.GazoDensibaitaiHozon)
                        {
                            // 電子媒体保存撮影
                            string cmtOpt = "";
                            cmtOpt = CIUtil.ToWide(odrDtl.Suryo.ToString());
                            string itemName = _common.GetCommentStr(odrDtl.ItemCd, ref cmtOpt);

                            _common.Wrk.wrkSinKouiDetails.Last().CmtOpt = cmtOpt;
                            _common.Wrk.wrkSinKouiDetails.Last().Suryo = 1;
                            _common.Wrk.wrkSinKouiDetails.Last().ItemName = itemName;
                        }
                        else if (filmNyuKasan && odrDtl.ItemCd != ItemCdConst.GazoNoFilm)
                        {
                            // フィルムの乳幼児加算
                            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoFilmNyu, autoAdd: 1);
                        }
                    }                                        
                }

            }

            // 薬剤
            if (yakuzaiOdrDtls.Any())
            {
                //_common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Gazo, ReceSinId.Gazo, santeiKbn);
                _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.GazoYakuzai, cdKbn: _common.GetCdKbn(santeiKbn, "E"));

                foreach (OdrDtlTenModel odrDtl in yakuzaiOdrDtls)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                }
            }

            // 特材
            if (tokuzaiOdrDtls.Any())
            {
                //_common.Wrk.AppendNewWrkSinRpInf(ReceKouiKbn.Gazo, ReceSinId.Gazo, santeiKbn);
                _common.Wrk.AppendNewWrkSinKoui(hokenPid, hokenId, ReceSyukeisaki.GazoYakuzai, cdKbn: _common.GetCdKbn(santeiKbn, "E"));

                foreach (OdrDtlTenModel odrDtl in tokuzaiOdrDtls)
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(odrDtl, _common.Odr.GetOdrCmt(odrDtl));
                }
            }
        }

        /// <summary>
        /// CT/MRIの減算チェック
        /// </summary>
        private void ctMriGensan(OdrDtlTenModel odrDtl, List<OdrDtlTenModel> filteredOdrDtl)
        {
            #region local method
            #region comment out
            //void _addSyokaiComment()
            //{
            //    if (_common.sinDate >= 20201001)
            //    {
            //        if (filteredOdrDtl.Any(p =>
            //            p.RpNo == odrDtl.RpNo &&
            //            p.RpEdaNo == odrDtl.RpEdaNo &&
            //            new string[] { ItemCdConst.CommentCTSyokai, ItemCdConst.CommentMRISyokai }.Contains(p.ItemCd)) == false)
            //        {
            //            List<TenMstModel> tenMsts = _common.Mst.GetTenMstByHokatuKbn(201);

            //            // まず、CTから
            //            List<string> tgtItemCds = tenMsts.Where(p => p.CdKbnno == 200).Select(p => p.ItemCd).ToList();
            //            List<SanteiDaysModel> santeiDays = _common.GetSanteiDaysSinYm(tgtItemCds);

            //            int minDate = 99999999;
            //            string commentCd = "";

            //            if (santeiDays != null && santeiDays.Any())
            //            {
            //                minDate = santeiDays.Min(p => p.SinDate);
            //                commentCd = ItemCdConst.CommentCTSyokai;
            //            }

            //            // 次にMRI
            //            tgtItemCds = tenMsts.Where(p => p.CdKbnno == 202).Select(p => p.ItemCd).ToList();
            //            santeiDays = _common.GetSanteiDaysSinYm(tgtItemCds);

            //            if (santeiDays != null && santeiDays.Any())
            //            {
            //                if (minDate > santeiDays.Min(p => p.SinDate))
            //                {
            //                    minDate = santeiDays.Min(p => p.SinDate);
            //                    commentCd = ItemCdConst.CommentMRISyokai;
            //                }
            //            }

            //            if (minDate < 99999999 && string.IsNullOrEmpty(commentCd) == false)
            //            {
            //                _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(commentCd, CIUtil.ToWide(CIUtil.SDateToWDate(minDate).ToString()), autoAdd: 1);
            //            }
            //            else
            //            {
            //                // 当日算定分をチェック
            //                commentCd = "";
            //                List<WrkSinKouiDetailModel> wrkDtls = _common.Wrk.FindWrkSinKouiDetailByItemCd(0, tenMsts.Select(p => p.ItemCd).ToList());

            //                if(wrkDtls != null && wrkDtls.Any())
            //                {
            //                    wrkDtls = wrkDtls.OrderBy(p => p.RpNo).ThenBy(p => p.SeqNo).ThenBy(p => p.RowNo).ToList();

            //                    if (wrkDtls.First().CdKbnno == 200)
            //                    {
            //                        commentCd = ItemCdConst.CommentCTSyokai;
            //                    }
            //                    else if (wrkDtls.First().CdKbnno == 202)
            //                    {
            //                        commentCd = ItemCdConst.CommentMRISyokai;
            //                    }

            //                    if (string.IsNullOrEmpty(commentCd) == false)
            //                    {
            //                        _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(commentCd, CIUtil.ToWide(CIUtil.SDateToWDate(_common.sinDate).ToString()), autoAdd: 1);
            //                    }
            //                }

            //                if(string.IsNullOrEmpty(commentCd))
            //                {
            //                    // もしかしたら、当来院にあるのかもしれない。。。
            //                    _addSyokaiCommentThisRaiin();
            //                }
            //            }
            //        }
            //    }
            //}

            //void _addSyokaiCommentThisRaiin()
            //{
            //    if (_common.sinDate >= 20201001)
            //    {
            //        if (filteredOdrDtl.Any(p =>
            //            p.RpNo == odrDtl.RpNo &&
            //            p.RpEdaNo == odrDtl.RpEdaNo &&
            //            new string[] { ItemCdConst.CommentCTSyokai, ItemCdConst.CommentMRISyokai }.Contains(p.ItemCd)) == false)
            //        {
            //            List<TenMstModel> tenMsts = _common.Mst.GetTenMstByHokatuKbn(201);

            //            List<string> tgtItemCds = tenMsts.Select(p => p.ItemCd).ToList();
            //            List<OdrDtlTenModel> tgtOdrDtls =
            //                _common.Odr.odrDtlls.FindAll(p =>
            //                    p.RaiinNo == _common.raiinNo &&
            //                    p.HokenSyu == _common.hokenKbn &&
            //                    !(p.RpNo == odrDtl.RpNo &&
            //                      p.RpEdaNo == odrDtl.RpEdaNo &&
            //                      p.RowNo == odrDtl.RowNo) &&
            //                    tgtItemCds.Contains(p.ItemCd))
            //                .OrderBy(p=>p.RpNo)
            //                .ThenBy(p=>p.RpEdaNo)
            //                .ThenBy(p=>p.RowNo)
            //                .ToList();
            //            if(tgtOdrDtls != null && tgtOdrDtls.Any())
            //            {
            //                string commentCd = "";

            //                if(tgtOdrDtls.First().CdKbnno == 200)
            //                {
            //                    commentCd = ItemCdConst.CommentCTSyokai;
            //                }
            //                else if (tgtOdrDtls.First().CdKbnno == 202)
            //                {
            //                    commentCd = ItemCdConst.CommentMRISyokai;
            //                }

            //                if (string.IsNullOrEmpty(commentCd) == false)
            //                {
            //                    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(commentCd, CIUtil.ToWide(CIUtil.SDateToWDate(_common.sinDate).ToString()), autoAdd: 1);
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            void _addSyokaiComment(int cdkbnno)
            {
                if (_common.sinDate >= 20201001)
                {
                    string commentCd = "";
                    if (cdkbnno == 200)
                    {
                        commentCd = ItemCdConst.CommentCTSyokai;
                    }
                    else if (cdkbnno == 202)
                    {
                        commentCd = ItemCdConst.CommentMRISyokai;
                    }

                    if (filteredOdrDtl.Any(p =>
                        p.RpNo == odrDtl.RpNo &&
                        p.RpEdaNo == odrDtl.RpEdaNo &&
                        p.ItemCd == commentCd) == false)
                    {
                        List<TenMstModel> tenMsts = _common.Mst.GetTenMstByHokatuKbn(201);

                        List<string> tgtItemCds = tenMsts.Where(p => p.CdKbnno == cdkbnno).Select(p => p.ItemCd).ToList();
                        List<SanteiDaysModel> santeiDays = _common.GetSanteiDaysSinYm(tgtItemCds);

                        int minDate = 99999999;
                        commentCd = "";

                        if (santeiDays != null && santeiDays.Any())
                        {
                            minDate = santeiDays.Min(p => p.SinDate);
                            if (cdkbnno == 200)
                            {
                                commentCd = ItemCdConst.CommentCTSyokai;
                            }
                            else if(cdkbnno == 202)
                            {
                                commentCd = ItemCdConst.CommentMRISyokai;
                            }
                        }

                        if (minDate < 99999999 && string.IsNullOrEmpty(commentCd) == false)
                        {
                            _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(commentCd, CIUtil.ToWide(CIUtil.SDateToWDate(minDate).ToString()), autoAdd: 1);
                        }
                        else
                        {
                            // 当日算定分をチェック
                            commentCd = "";
                            List<WrkSinKouiDetailModel> wrkDtls = _common.Wrk.FindWrkSinKouiDetailByItemCd(0, tenMsts.Where(p => p.CdKbnno == cdkbnno).Select(p => p.ItemCd).ToList());

                            if (wrkDtls != null && wrkDtls.Any())
                            {
                                wrkDtls = wrkDtls.OrderBy(p => p.RpNo).ThenBy(p => p.SeqNo).ThenBy(p => p.RowNo).ToList();

                                if (wrkDtls.First().CdKbnno == 200)
                                {
                                    commentCd = ItemCdConst.CommentCTSyokai;
                                }
                                else if (wrkDtls.First().CdKbnno == 202)
                                {
                                    commentCd = ItemCdConst.CommentMRISyokai;
                                }

                                if (string.IsNullOrEmpty(commentCd) == false)
                                {
                                    _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(commentCd, CIUtil.ToWide(CIUtil.SDateToWDate(_common.sinDate).ToString()), autoAdd: 1);
                                }
                            }

                            if (string.IsNullOrEmpty(commentCd))
                            {
                                // もしかしたら、当来院にあるのかもしれない。。。
                                _addSyokaiCommentThisRaiin(cdkbnno);
                            }
                        }
                    }
                }
            }

            void _addSyokaiCommentThisRaiin(int cdkbnno)
            {
                if (_common.sinDate >= 20201001)
                {
                    string commentCd = "";

                    if (cdkbnno == 200)
                    {
                        commentCd = ItemCdConst.CommentCTSyokai;
                    }
                    else if (cdkbnno == 202)
                    {
                        commentCd = ItemCdConst.CommentMRISyokai;
                    }

                    if (filteredOdrDtl.Any(p =>
                        p.RpNo == odrDtl.RpNo &&
                        p.RpEdaNo == odrDtl.RpEdaNo &&
                        p.ItemCd == commentCd) == false)
                    {
                        List<TenMstModel> tenMsts = _common.Mst.GetTenMstByHokatuKbn(201);

                        List<string> tgtItemCds = tenMsts.Select(p => p.ItemCd).ToList();
                        List<OdrDtlTenModel> tgtOdrDtls =
                            _common.Odr.odrDtlls.FindAll(p =>
                                p.RaiinNo == _common.raiinNo &&
                                p.HokenSyu == _common.hokenKbn &&
                                !(p.RpNo == odrDtl.RpNo &&
                                  p.RpEdaNo == odrDtl.RpEdaNo &&
                                  p.RowNo == odrDtl.RowNo) &&
                                p.CdKbnno == cdkbnno &&
                                tgtItemCds.Contains(p.ItemCd))
                            .OrderBy(p => p.RpNo)
                            .ThenBy(p => p.RpEdaNo)
                            .ThenBy(p => p.RowNo)
                            .ToList();
                        if (tgtOdrDtls != null && tgtOdrDtls.Any())
                        {
                            commentCd = "";

                            if (tgtOdrDtls.First().CdKbnno == 200)
                            {
                                commentCd = ItemCdConst.CommentCTSyokai;
                            }
                            else if (tgtOdrDtls.First().CdKbnno == 202)
                            {
                                commentCd = ItemCdConst.CommentMRISyokai;
                            }

                            if (string.IsNullOrEmpty(commentCd) == false)
                            {
                                _common.Wrk.AppendNewWrkSinKouiDetailCommentRecord(commentCd, CIUtil.ToWide(CIUtil.SDateToWDate(_common.sinDate).ToString()), autoAdd: 1);
                            }
                        }
                    }
                }
            }
            #endregion

            int yesterday = CIUtil.DaysBefore(_common.sinDate, 1);

            bool existHokatuItem =
                _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, yesterday, 201) > 0 ||
               _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, yesterday, -1, "E", 101, 3, -1) > 0 ||
               _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, yesterday, -1, "E", 101, 4, -1) > 0;

            bool existValidGensanItem = false;

            List<OdrDtlTenModel> gensanOdrDtls = _common.Odr.FilterOdrDetailByItemCd(ItemCdConst.GazoCtMriGensan);

            if(gensanOdrDtls.Any())
            {
                foreach(OdrDtlTenModel dtl in gensanOdrDtls)
                {
                    if(_common.Odr.ExistOdrDetailByHokatuKbn(dtl.RpNo, dtl.RpEdaNo, 201))
                    {
                        existValidGensanItem = true;
                        break;
                    }
                }
            }

            //if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.GazoCtMriGensan) == true)
            if (existHokatuItem == false && existValidGensanItem == true)
            {
                // 手オーダーあり

                if (_common.Odr.ExistOdrDetailByItemCdRp(odrDtl.RpNo, odrDtl.RpEdaNo, ItemCdConst.GazoCtMriGensan))
                {
                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoCtMriGensan, autoAdd: 0);

                    // 初回算定日のコメント追加
                    //_addSyokaiComment();
                    _addSyokaiComment(200);
                    _addSyokaiComment(202);
                }
            }
            //else if (_common.Odr.ExistOdrDetailByItemCd(ItemCdConst.GazoCtMriGensan) == false &&
            else if (!(existHokatuItem == false && existValidGensanItem == true) &&
                _common.Odr.ExistOdrDetailByItemCd(ItemCdConst.GazoCtMriGensanCancel) == false)
            {
                // 手オーダーもキャンセル項目もない場合

                double count = _common.Wrk.wrkSinKouiDetails.Count(p => p.TenMst != null && p.TenMst.HokatuKbn == 201 && p.IsDeleted == 0);
                //if (count>1)
                //{
                //    // 今回算定分にCT/MRIが存在している場合
                //    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoCtMriGensan, autoAdd: 1);
                //}
                //else
                //{
                //    count += _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, _common.sinDate, 201);
                //    if (count > 1)
                //    {
                //        // 同月にCT/MRIが存在しているかチェック
                //        _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoCtMriGensan, autoAdd: 1);
                //    }
                //    else
                //    {
                //        // コンピュータ断層撮影の算定があるかチェック
                //        if(_common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, _common.sinDate, -1, "E", 101, 3, -1) > 0 ||
                //            _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, _common.sinDate, -1, "E", 101, 4, -1) > 0)
                //        {
                //            _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoCtMriGensan, autoAdd: 1);
                //        }
                //    }
                //}

                if (count + _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, _common.sinDate, 201) > 1)
                {
                    // 同月にCT/MRIが存在しているかチェック
                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoCtMriGensan, autoAdd: 1);
                    // 初回算定日のコメント追加
                    //_addSyokaiComment();
                    _addSyokaiComment(200);
                    _addSyokaiComment(202);
                }
                else if (_common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, _common.sinDate, -1, "E", 101, 3, -1) > 0 ||
                        _common.SanteiCountByHokatuKbn(_common.SinFirstDateOfMonth, _common.sinDate, -1, "E", 101, 4, -1) > 0)
                {
                    // コンピュータ断層撮影の算定があるかチェック
                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoCtMriGensan, autoAdd: 1);
                    // 初回算定日のコメント追加
                    //_addSyokaiComment();
                    _addSyokaiComment(200);
                    _addSyokaiComment(202);
                }
                else if (count > 1)
                {
                    // 当来院にCT/MRIが存在している
                    _common.Wrk.AppendNewWrkSinKouiDetail(ItemCdConst.GazoCtMriGensan, autoAdd: 1);
                    _common.Wrk.wrkSinKouiDetails.Last().TeigenTargetInRaiin = true;
                    // 初回算定日のコメント追加
                    //_addSyokaiCommentThisRaiin();
                    _addSyokaiCommentThisRaiin(200);
                    _addSyokaiCommentThisRaiin(202);
                }
            }
        }

        /// <summary>
        /// 自費算定分を処理する
        /// </summary>
        private void CalculateJihi()
        {
            const string conFncName = nameof(CalculateJihi);

            _common.CalculateJihi(
                OdrKouiKbnConst.GazoMin, 
                OdrKouiKbnConst.GazoMax, 
                ReceKouiKbn.Gazo, 
                ReceSinId.Gazo, 
                ReceSyukeisaki.Gazo, 
                "JS");
        }
    }
}
