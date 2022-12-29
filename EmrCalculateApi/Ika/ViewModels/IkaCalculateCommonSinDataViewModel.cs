using Entity.Tenant;
using EmrCalculateApi.Ika.DB.Finder;
using EmrCalculateApi.Ika.DB.CommandHandler;
using EmrCalculateApi.Ika.Models;
using EmrCalculateApi.Ika.Constants;
using Helper.Constants;
using EmrCalculateApi.Utils;
using EmrCalculateApi.Interface;
using Helper.Common;
using Helper.Extension;
using Domain.Constant;
using EmrCalculateApi.Constants;

namespace EmrCalculateApi.Ika.ViewModels
{
    public class IkaCalculateCommonSinDataViewModel
    {
        /// <summary>
        /// 算定情報ファインダー
        /// </summary>
        SanteiFinder _santeiFinder;

        /// <summary>
        /// 診療Rp情報
        /// </summary>
        private List<SinRpInfModel> _sinRpInfs;
        /// <summary>
        /// 診療行為情報
        /// </summary>
        private List<SinKouiModel> _sinKouis;
        /// <summary>
        /// 診療行為詳細情報
        /// </summary>
        private List<SinKouiDetailModel> _sinKouiDetails;
        /// <summary>
        /// 診療Rp番号情報
        /// </summary>
        //private List<SinRpNoInfModel> _sinRpNoInfs;
        /// <summary>
        /// 診療行為カウント情報
        /// </summary>
        private List<SinKouiCountModel> _sinKouiCounts;

        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        int _hpId;
        /// <summary>
        /// 患者ID
        /// </summary>
        long _ptId;
        /// <summary>
        /// 診療日
        /// </summary>
        int _sinDate;
        /// <summary>
        /// 来院番号
        /// </summary>
        long _raiinNo;
        /// <summary>
        /// 保険区分
        /// </summary>
        int _hokenKbn;
        /// <summary>
        /// 計算モード
        /// </summary>
        int _calcMode;

        /// <summary>
        /// キー番号
        /// </summary>
        private int _sinKeyNo;
        /// <summary>
        /// Rp番号
        /// </summary>
        private int _sinRpNo;
        /// <summary>
        /// 連番
        /// </summary>
        private int _sinSeqNo;
        /// <summary>
        /// 行番号
        /// </summary>
        private int _sinRowNo;
        /// <summary>
        /// 項目連番
        /// </summary>
        private int _itemSeqNo;
        /// <summary>
        /// 最初に追加する項目かどうかのフラグ
        /// </summary>
        private bool _firstItem;

        List<int> checkHokenKbn;
        List<int> checkSanteiKbn;

        IkaCalculateCommonMasterViewModel _mstCommon;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        public IkaCalculateCommonSinDataViewModel(SanteiFinder santeiFinder, IkaCalculateCommonMasterViewModel mstCommon, int hpId, long ptId, int sinDate, long raiinNo, int hokenKbn, int calcMode, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _emrLogger = emrLogger;
            _systemConfigProvider = systemConfigProvider;

            //算定情報ファインダー
            _santeiFinder = santeiFinder;
            //マスタ情報管理クラス
            _mstCommon = mstCommon;

            //医療機関識別ID
            _hpId = hpId;
            //患者ID
            _ptId = ptId;
            //診療日
            _sinDate = sinDate;
            //来院番号
            _raiinNo = raiinNo;
            //保険区分
            _hokenKbn = hokenKbn;
            //計算モード
            _calcMode = calcMode;

            //キー番号
            _sinKeyNo = 0;
            //Rp番号
            _sinRpNo = 0;
            //連番
            _sinSeqNo = 0;
            //行番号
            _sinRowNo = 0;
            //項目連番
            _itemSeqNo = 0;
            //最初に追加する項目かどうかのフラグ
            _firstItem = true;

            // 指定の診療日が属する月の算定情報を取得する
            GetSinInf();

            // 算定情報の削除
            DeleteSanteiInf(_hpId, _ptId, _sinDate);

            checkHokenKbn = new List<int>();
            checkSanteiKbn = new List<int>();
        }

        /// <summary>
        /// 診療日が属する月の算定情報を取得する
        /// </summary>
        public void GetSinInf()
        {
            // 指定の診療日が属する月の算定情報を取得する
            _sinRpInfs = _santeiFinder.FindSinRpInfData(_hpId, _ptId, _sinDate);
            _sinKouis = _santeiFinder.FindSinKouiData(_hpId, _ptId, _sinDate);
            _sinKouiDetails = _santeiFinder.FindSinKouiDetailData(_hpId, _ptId, _sinDate);
            //_sinRpNoInfs = _santeiFinder.FindSinRpNoInfData(_hpId, _ptId, _sinDate);
            _sinKouiCounts = _santeiFinder.FindSinKouiCountData(_hpId, _ptId, _sinDate);
        }

        /// <summary>
        /// 指定日に関連する算定情報を削除する
        /// </summary>
        /// <param name="hpId">医療機関識別ID</param>
        /// <param name="ptId">患者ID</param>
        /// <param name="sinDate">診療日</param>
        private void DeleteSanteiInf(int hpId, long ptId, int sinDate)
        {
            // 本日の診療分の行為カウントを取得
            List<SinKouiCountModel> sinKouiCounts;

            if (_calcMode != CalcModeConst.Trial)
            {
                sinKouiCounts =
                    _sinKouiCounts.FindAll(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.SinDate == sinDate);
                    //p.SinYm == sinDate / 100 &&
                    //p.SinDay == sinDate % 100);
            }
            else
            {
                sinKouiCounts =
                    _sinKouiCounts.FindAll(p =>
                    p.HpId == hpId &&
                    p.PtId == ptId &&
                    p.RaiinNo == _raiinNo &&
                    p.SinDate == sinDate);
                    //p.SinYm == sinDate / 100 &&
                    //p.SinDay == sinDate % 100);
            }

            // 行為からマイナス
            int index;
            foreach (SinKouiCountModel sinKouiCount in sinKouiCounts)
            {
                // 更新ステータスを削除に設定
                sinKouiCount.UpdateState = UpdateStateConst.Delete;

                // 対応する行為レコード取得
                index =
                    _sinKouis.FindIndex(p =>
                        p.HpId == sinKouiCount.HpId &&
                        p.PtId == sinKouiCount.PtId &&
                        p.RpNo == sinKouiCount.RpNo &&
                        p.SeqNo == sinKouiCount.SeqNo);

                if (index >= 0)
                {
                    // 行為のカウントをマイナス
                    _sinKouis[index].Count -= sinKouiCount.Count;

                    // Day*もマイナスしておく

                    if (_sinKouiDetails.Any(p =>
                         p.RpNo == _sinKouis[index].RpNo &&
                         p.SeqNo == _sinKouis[index].SeqNo &&
                         _mstCommon.ZaiWeekCalcList.Contains(p.ItemCd) &&
                         p.IsNodspRece == 1 &&
                         p.FmtKbn == FmtKbnConst.ZaiCyosei))
                    {
                        // 週単位計算項目の調整項目（IsNodspRece = 1）の場合
                        int firstDate =
                            Math.Max(
                                CIUtil.GetFirstDateOfWeek(_sinDate),
                                _sinDate / 100 * 100 + 1);
                        int lastDate =
                            Math.Min(
                                CIUtil.GetLastDateOfWeek(_sinDate),
                                CIUtil.GetLastDateOfMonth(_sinDate));

                        for (int checkDate = firstDate; checkDate <= lastDate; checkDate++)
                        {
                            var property = typeof(SinKouiModel).GetProperty("Day" + (checkDate % 100).ToString());
                            property.SetValue(_sinKouis[index], 0);
                        }
                    }
                    else
                    {
                        var property = typeof(SinKouiModel).GetProperty("Day" + (sinKouiCount.SinDay).ToString());

                        int daycount = property.GetValue(_sinKouis[index]).AsInteger() - sinKouiCount.Count;
                        if (daycount < 0) { daycount = 0; }
                        property.SetValue(_sinKouis[index], daycount);
                    }

                    // 行為のカウントが0になった場合
                    if (_sinKouis[index].Count <= 0)
                    {
                        // 更新ステータスを削除に設定
                        _sinKouis[index].UpdateState = UpdateStateConst.Delete;
                        _sinKouis[index].IsDeleted = 1;

                        //foreach (SinKouiModel sinKoui in _sinKouis.FindAll(p =>
                        //                                    p.HpId == sinKouiCount.HpId &&
                        //                                    p.PtId == sinKouiCount.PtId &&
                        //                                    p.RpNo == sinKouiCount.RpNo &&
                        //                                    p.SeqNo == sinKouiCount.SeqNo))
                        //{
                        //    sinKoui.UpdateState = UpdateStateConst.Delete;
                        //    sinKoui.CreateId = -1;

                        //    foreach (SinKouiDetailModel sinDtl in _sinKouiDetails.FindAll(p =>
                        //                                            p.HpId == sinKoui.HpId &&
                        //                                            p.PtId == sinKoui.PtId &&
                        //                                            p.RpNo == sinKoui.RpNo &&
                        //                                            p.SeqNo == sinKoui.SeqNo))
                        //    {
                        //        sinDtl.UpdateState = UpdateStateConst.Delete;
                        //        sinDtl.IsNodspRece = -1;
                        //    }
                        //}

                        foreach (SinKouiDetailModel sinDtl in _sinKouiDetails.FindAll(p =>
                                        p.HpId == _sinKouis[index].HpId &&
                                        p.PtId == _sinKouis[index].PtId &&
                                        p.RpNo == _sinKouis[index].RpNo &&
                                        p.SeqNo == _sinKouis[index].SeqNo))
                        {
                            sinDtl.UpdateState = UpdateStateConst.Delete;
                            sinDtl.IsDeleted = 1;
                        }
                    }
                }
            }

            // RpInfの更新

            // 有効な行為を取得
            List<SinKouiModel> sinKouiChks =
                _sinKouis.FindAll(p => p.Count > 0 && p.UpdateState == UpdateStateConst.None);

            foreach (SinKouiModel sinKoui in sinKouiChks)
            {
                if (_sinKouiCounts.Any(p => p.RpNo == sinKoui.RpNo && p.SeqNo == sinKoui.SeqNo && p.UpdateState == UpdateStateConst.None) == false)
                {
                    sinKoui.UpdateState = UpdateStateConst.Delete;
                    sinKoui.IsDeleted = 1;
                }
            }

            List<SinKouiModel> sinKouis =
                _sinKouis.FindAll(p => p.Count > 0 && p.UpdateState == UpdateStateConst.None);

            var firstDays =
                _sinKouiCounts.Where(p => p.UpdateState == UpdateStateConst.None)
                .GroupBy(p => new { RpNo = p.RpNo, SinDay = p.SinDay })
                .Select(p => new { RpNo = p.Key.RpNo, FirstDay = p.Min(p2 => p2.SinDay) });

            for (int i = 0; i < _sinRpInfs.Count; i++)
            {
                if (sinKouis.Any(p => p.RpNo == _sinRpInfs[i].RpNo) == false)
                {
                    // 有効な行為が存在しなければDelete
                    _sinRpInfs[i].UpdateState = UpdateStateConst.Delete;
                    _sinRpInfs[i].IsDeleted = 1;
                }
                else if (firstDays != null)
                {
                    // 初回日更新
                    var firstDay = firstDays.Where(p => p.RpNo == _sinRpInfs[i].RpNo).FirstOrDefault();

                    if (firstDay != null)
                    {
                        _sinRpInfs[i].FirstDay = firstDay.FirstDay;
                    }
                }
            }

            // 本日の診療分のRpNoInfを取得
            //List<SinRpNoInfModel> sinRpNoInfs =
            //    _sinRpNoInfs.FindAll(p =>
            //        p.HpId == hpId &&
            //        p.PtId == ptId &&
            //        p.SinYm == sinDate / 100 &&
            //        p.SinDay == sinDate % 100);

            //for (int i = 0; i < _sinRpNoInfs.Count; i++)
            //{
            //    _sinRpNoInfs[i].UpdateState = UpdateStateConst.Delete;
            //}

            // SIN_KOUI_COUNTと紐づかないSIN_KOUI_DETAILのレコードを削除
            foreach(SinKouiDetailModel sinDtl in _sinKouiDetails.FindAll(p => p.IsDeleted != 1))
            {
                if (_sinKouiCounts.Any(p => p.RpNo == sinDtl.RpNo && p.SeqNo == sinDtl.SeqNo) == false)
                {
                    sinDtl.IsDeleted = 1;
                }
            }
        }

        public void DelSinKouiCountByRaiinNo(List<long> delSinRaiinNos)
        {
            foreach(SinKouiCountModel sinKouiCounts in _sinKouiCounts.FindAll(p => delSinRaiinNos.Contains(p.RaiinNo)))
            {
                sinKouiCounts.UpdateState = UpdateStateConst.Delete;
            }
        }

        #region プロパティ
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId
        {
            get { return _hpId; }
        }

        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get { return _ptId; }
        }

        /// <summary>
        /// 診療日
        /// </summary>
        public int SinDate
        {
            get { return _sinDate; }
        }

        /// <summary>
        /// 来院番号
        /// </summary>
        public long RaiinNo
        {
            get { return _raiinNo; }
            set { _raiinNo = value; }
        }

        /// <summary>
        /// 保険区分
        /// </summary>
        public int HokenKbn
        {
            get { return _hokenKbn; }
            set
            {
                _hokenKbn = value;
                checkHokenKbn = CalcUtils.GetCheckHokenKbns(_hokenKbn, _systemConfigProvider.GetHokensyuHandling());
                checkSanteiKbn = CalcUtils.GetCheckSanteiKbns(_hokenKbn, _systemConfigProvider.GetHokensyuHandling());
            }
        }

        /// <summary>
        /// 診療Rp情報
        /// </summary>
        public List<SinRpInfModel> SinRpInfs
        {
            get { return _sinRpInfs; }
        }

        /// <summary>
        /// 診療行為情報
        /// </summary>
        public List<SinKouiModel> SinKouis
        {
            get { return _sinKouis; }
        }

        /// <summary>
        /// 診療行為詳細情報
        /// </summary>
        public List<SinKouiDetailModel> SinKouiDetails
        {
            get { return _sinKouiDetails; }
        }

        /// <summary>
        /// 診療Rp番号情報
        /// </summary>
        //public List<SinRpNoInfModel> SinRpNoInfs
        //{
        //    get { return _sinRpNoInfs; }
        //}

        /// <summary>
        /// 診療行為カウント情報
        /// </summary>
        public List<SinKouiCountModel> SinKouiCounts
        {
            get { return _sinKouiCounts; }
        }

        /// <summary>
        /// キー番号
        /// </summary>
        public int KeyNo
        {
            get { return _sinKeyNo; }
        }

        /// <summary>
        /// 連番
        /// </summary>
        public int SeqNo
        {
            get { return _sinSeqNo; }
        }

        #endregion

        /// <summary>
        /// ローカルデータの初期化
        /// </summary>
        public void ClearKeisanData()
        {
            _firstItem = true;

            // Update or Deleteは削除
            _sinRpInfs.RemoveAll(p => p.UpdateState == UpdateStateConst.Update || p.UpdateState == UpdateStateConst.Delete);
            _sinKouis.RemoveAll(p => p.UpdateState == UpdateStateConst.Update || p.UpdateState == UpdateStateConst.Delete);
            _sinKouiDetails.RemoveAll(p => p.UpdateState == UpdateStateConst.Update || p.UpdateState == UpdateStateConst.Delete);
            //_sinRpNoInfs.RemoveAll(p => p.UpdateState == UpdateStateConst.Update || p.UpdateState == UpdateStateConst.Delete);
            _sinKouiCounts.RemoveAll(p => p.UpdateState == UpdateStateConst.Update || p.UpdateState == UpdateStateConst.Delete);

            // AddはNoneに変更
            _sinRpInfs.FindAll(p => p.UpdateState == UpdateStateConst.Add).ForEach(p =>
                p.UpdateState = UpdateStateConst.None
            );
            _sinKouis.FindAll(p => p.UpdateState == UpdateStateConst.Add).ForEach(p =>
                p.UpdateState = UpdateStateConst.None
            );
            _sinKouiDetails.FindAll(p => p.UpdateState == UpdateStateConst.Add).ForEach(p =>
                p.UpdateState = UpdateStateConst.None
            );
            //_sinRpNoInfs.FindAll(p => p.UpdateState == UpdateStateConst.Add).ForEach(p =>
            //    p.UpdateState = UpdateStateConst.None
            //);
            _sinKouiCounts.FindAll(p => p.UpdateState == UpdateStateConst.Add).ForEach(p =>
                p.UpdateState = UpdateStateConst.None
            );

        }

        #region Rp情報関連

        /// <summary>
        /// 診療Rp情報を取得する
        /// </summary>
        /// <param name="wrkSinRp">ワーク診療Rp情報</param>
        /// <returns>診療Rp情報</returns>
        public SinRpInfModel GetSinRpInf(WrkSinRpInfModel wrkSinRp)
        {
            SinRpInfModel sinRpInfModel = new SinRpInfModel(new SinRpInf());

            sinRpInfModel.HpId = _hpId;
            sinRpInfModel.PtId = _ptId;
            sinRpInfModel.SinYm = _sinDate / 100;

            //sinRpInfModel.RpNo = wrkSinRp.RpNo;
            sinRpInfModel.FirstDay = 0;
            sinRpInfModel.HokenKbn = wrkSinRp.HokenKbn;
            sinRpInfModel.SinKouiKbn = wrkSinRp.SinKouiKbn;
            sinRpInfModel.SinId = wrkSinRp.SinId;
            sinRpInfModel.SanteiKbn = wrkSinRp.SanteiKbn;
            sinRpInfModel.CdNo = wrkSinRp.CdNo;

            return sinRpInfModel;
        }

        /// <summary>
        /// 診療Rp情報を取得する
        /// </summary>
        /// <param name="sinKouiKbn">診療行為区分</param>
        /// <param name="sinId">診療ID</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <param name="cdNo">コード表番号</param>
        /// <returns>診療Rp情報</returns>
        public SinRpInfModel GetSinRpInf(int sinKouiKbn, int sinId, int santeiKbn, string cdNo = "")
        {
            SinRpInfModel sinRpInfModel = new SinRpInfModel(new SinRpInf());

            sinRpInfModel.HpId = _hpId;
            sinRpInfModel.PtId = _ptId;
            sinRpInfModel.SinYm = _sinDate / 100;

            sinRpInfModel.FirstDay = 0;
            sinRpInfModel.HokenKbn = _hokenKbn;
            sinRpInfModel.SinKouiKbn = sinKouiKbn;
            sinRpInfModel.SinId = sinId;
            sinRpInfModel.SanteiKbn = santeiKbn;
            sinRpInfModel.CdNo = cdNo;

            return sinRpInfModel;
        }

        /// <summary>
        /// 診療Rp情報を追加する
        /// 現在、追加中のRp情報がある場合、付随する診療行為が存在するかチェックし、
        /// なければ追加を取り消して、新たに追加する
        /// </summary>
        /// <param name="SinRpInfModel">診療Rp情報</param>
        public void AppendSinRpInf(SinRpInfModel SinRpInfModel)
        {
            if (_sinRpNo > 0)
            {
                CommitSinRpInf();
            }

            _sinKeyNo++;
            _sinRpNo++;
            _sinSeqNo = 0;
            _sinRowNo = 0;

            SinRpInfModel.RpNo = _sinRpNo;

            SinRpInfModel.KeyNo = _sinKeyNo;
            SinRpInfModel.UpdateState = UpdateStateConst.Add;
            _sinRpInfs.Add(SinRpInfModel);

        }

        /// <summary>
        /// Rp情報を生成し、追加する
        /// </summary>
        /// <param name="wrkSinRp">ワーク診療Rp情報</param>
        public void AppendNewSinRpInf(WrkSinRpInfModel wrkSinRp)
        {
            AppendSinRpInf(GetSinRpInf(wrkSinRp));
        }

        /// <summary>
        /// Rp情報を生成し、追加する
        /// </summary>
        /// <param name="sinKouiKbn">診療行為区分</param>
        /// <param name="sinId">診療ID</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <param name="cdNo">コード表番号</param>
        public void AppendNewSinRpInf(int sinKouiKbn, int sinId, int santeiKbn, string cdNo = "")
        {
            AppendSinRpInf(GetSinRpInf(sinKouiKbn: sinKouiKbn, sinId: sinId, santeiKbn: santeiKbn, cdNo: cdNo));
        }

        /// <summary>
        /// Rp情報を確定する
        /// もし、付随する診療行為がない場合は、追加を取り消す
        /// </summary>
        public void CommitSinRpInf()
        {
            CommitSinKoui();

            if (_sinKouis.Any(p => p.RpNo == _sinRpNo) == false)
            {
                _sinRpInfs.RemoveAll(p => p.RpNo == _sinRpNo);
                _sinRpNo--;
            }
            else if (_sinKouis.Any())
            {
                // CdNoを採番
                string wrkCdNo = _sinRpInfs.Last().CdNo;
                _sinRpInfs.Last().CdNo = "";

                List<SinKouiDetailModel> wrkDtls = _sinKouiDetails.FindAll(p => p.RpNo == _sinRpNo && p.IsDeleted == DeleteStatus.None);

                if (wrkDtls.Any())
                {
                    string cdNo = "";

                    for (int i = 0; i <= 1; i++)
                    {
                        List<SinKouiDetailModel> filteredWrkDtls = new List<SinKouiDetailModel>();

                        if (i == 0)
                        {
                            filteredWrkDtls =
                                wrkDtls.FindAll(p => new string[] { "1", "3", "5" }.Contains(p.Kokuji2));
                        }
                        else
                        {
                            filteredWrkDtls =
                                wrkDtls.FindAll(p => !(new string[] { "1", "3", "5" }.Contains(p.Kokuji2)));
                        }

                        foreach (SinKouiDetailModel wrkDtl in filteredWrkDtls)
                        {
                            List<TenMstModel> tenMst = new List<TenMstModel>();

                            if (wrkDtl.TenMst != null)
                            {
                                if (wrkDtl.TenMst.CdKbn != "" && wrkDtl.TenMst.CdKbn != "-" && wrkDtl.TenMst.CdKbn != "*")
                                {
                                    string tmpCdNo =
                                    wrkDtl.TenMst.CdKbn +
                                        String.Format("{0:D3}{1:D2}{2:D2}",
                                            wrkDtl.TenMst.CdKbnno, wrkDtl.TenMst.CdEdano, wrkDtl.TenMst.CdKouno);

                                    if (string.IsNullOrEmpty(cdNo) || string.Compare(cdNo, tmpCdNo) == 1)
                                    {
                                        cdNo = tmpCdNo;
                                    }
                                }
                            }
                            else
                            {
                                tenMst = _mstCommon.GetTenMst(wrkDtl.ItemCd);

                                if (tenMst.First().CdKbn != "" && tenMst.First().CdKbn != "-" && tenMst.First().CdKbn != "*")
                                {
                                    string tmpCdNo =
                                    tenMst.First().CdKbn +
                                    String.Format("{0:D3}{1:D2}{2:D2}",
                                        tenMst.First().CdKbnno, tenMst.First().CdEdano, tenMst.First().CdKouno);

                                    if (string.IsNullOrEmpty(cdNo) || string.Compare(cdNo, tmpCdNo) == 1)
                                    {
                                        cdNo = tmpCdNo;
                                    }
                                }
                            }

                            _sinRpInfs.Last().CdNo = cdNo;
                        }

                        if (string.IsNullOrEmpty(_sinRpInfs.Last().CdNo) == false)
                        {
                            break;
                        }
                    }
                }

                if (_sinRpInfs.Last().CdNo == "")
                {
                    List<SinKouiModel> wrkSins = _sinKouis.FindAll(p => p.RpNo == _sinRpNo && p.IsDeleted == DeleteStatus.None);

                    if (wrkSins.Any())
                    {
                        if (wrkSins.First().CdKbn == "F" && new int[] { 21, 22, 23 }.Contains(_sinRpInfs.Last().SinId) &&
                            _sinKouiDetails.Any(p => p.RpNo == _sinRpNo && p.ItemCd.StartsWith("6")))
                        {
                            // 投薬で薬剤を含むRpの場合は、一番上にくるように
                            _sinRpInfs.Last().CdNo = wrkSins.First().CdKbn + "0000000";
                        }
                        else if (_sinKouiDetails.Any(p => p.RpNo == _sinRpNo && p.ItemCd.StartsWith("6")))
                        {
                            // 薬剤を含む場合
                            if (string.IsNullOrEmpty(wrkCdNo) == false && wrkCdNo.Length >= 8)
                            {
                                _sinRpInfs.Last().CdNo = wrkCdNo.Substring(0, wrkCdNo.Length - 7) + "9999997";
                            }
                            else
                            {
                                _sinRpInfs.Last().CdNo = wrkSins.First().CdKbn + "9999997";
                            }
                        }
                        else if (_sinKouiDetails.Any(p => p.RpNo == _sinRpNo && p.ItemCd.StartsWith("7")))
                        {
                            // 特材を含む場合
                            if (string.IsNullOrEmpty(wrkCdNo) == false && wrkCdNo.Length >= 8)
                            {
                                _sinRpInfs.Last().CdNo = wrkCdNo.Substring(0, wrkCdNo.Length - 7) + "9999998";
                            }
                            else
                            {
                                _sinRpInfs.Last().CdNo = wrkSins.First().CdKbn + "9999998";
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(wrkCdNo) == false && wrkCdNo.Length >= 8)
                            {
                                _sinRpInfs.Last().CdNo = wrkCdNo.Substring(0, wrkCdNo.Length - 7) + "9999999";
                            }
                            else
                            {
                                _sinRpInfs.Last().CdNo = wrkSins.First().CdKbn + "9999999";
                            }
                        }
                    }

                    if (_sinRpInfs.Last().CdNo == "")
                    {
                        // コードが確定できない場合、とりあえず、一番下になるようにしておく
                        _sinRpInfs.Last().CdNo = "Z9999999";
                    }
                }

                if (_sinRpInfs.Last().CdNo == "")
                {
                    _sinRpInfs.Last().CdNo = wrkCdNo;
                }

                // 削除分しかない場合
                if (_sinKouis.Any(p => p.RpNo == _sinRpNo && p.IsDeleted == 0) == false)
                {
                    _sinRpInfs.Last().IsDeleted = DeleteStatus.DeleteFlag;
                }
            }
        }

        /// <summary>
        /// 診療行為データを取得する
        /// （付随する診療行為の内容を文字列化したデータ）
        /// </summary>
        /// <param name="sinRpInf">診療行為データを取得したい診療Rp</param>
        /// <returns>付随する診療行為の内容を文字列化したデータ</returns>
        public string GetKouiData(SinRpInfModel sinRpInf)
        {
            string ret = "";
            string tmpStr = "";

            List<SinKouiModel> sinKouis = _sinKouis.FindAll(p => p.KeyNo == sinRpInf.KeyNo);

            ret = MakeData("hk", sinRpInf.HokenKbn);
            ret = AddStr(ret, MakeData("st", sinRpInf.SanteiKbn));
            ret = AddStr(ret, MakeData("sid", sinRpInf.SinId));
            ret = AddStr(ret, MakeData("cdn", sinRpInf.CdNo));
            
            foreach (SinKouiModel sinKoui in sinKouis)
            {
                // 詳細の内容を文字列化する
                tmpStr = MakeKouiData(sinKoui);

                if (tmpStr != "")
                {
                    ret = AddStr(ret, tmpStr);
                }
            }

            return ret;
        }

        #endregion

        #region 診療行為情報関連
        /// <summary>
        /// 診療行為を生成する
        /// </summary>
        /// <param name="wrkSinKoui">ワーク診療行為情報</param>
        /// <returns>診療行為情報</returns>
        public SinKouiModel GetSinKoui(WrkSinKouiModel wrkSinKoui)
        {
            SinKouiModel sinKouiModel = new SinKouiModel(new SinKoui());

            sinKouiModel.HpId = _hpId;
            sinKouiModel.PtId = _ptId;
            sinKouiModel.SinYm = _sinDate / 100;
            sinKouiModel.HokenPid = wrkSinKoui.HokenPid;
            sinKouiModel.HokenId = wrkSinKoui.HokenId;
            if (wrkSinKoui.SyukeiSaki == ReceSyukeisaki.SonotaSyohoSenComment)
            {
                // 集計先が処方箋コメントの場合、処方箋として扱う
                sinKouiModel.SyukeiSaki = ReceSyukeisaki.SonotaSyohoSen;
            }
            else
            {
                sinKouiModel.SyukeiSaki = wrkSinKoui.SyukeiSaki;
            }
            sinKouiModel.Count = wrkSinKoui.Count;
            sinKouiModel.EntenKbn = 0;
            sinKouiModel.JihiSbt = wrkSinKoui.JihiSbt;
            sinKouiModel.KazeiKbn = wrkSinKoui.KazeiKbn;
            sinKouiModel.IsNodspRece = wrkSinKoui.IsNodspRece;
            sinKouiModel.IsNodspPaperRece = wrkSinKoui.IsNodspPaperRece;
            sinKouiModel.InoutKbn = wrkSinKoui.InoutKbn;
            sinKouiModel.CdKbn = wrkSinKoui.CdKbn;

            return sinKouiModel;
        }

        /// <summary>
        /// 診療行為を生成する
        /// </summary>
        /// <param name="hokenPid">保険組み合わせID</param>
        /// <param name="hokenId">保険ID</param>
        /// <param name="syukeiSaki">集計先</param>
        /// <param name="count">回数</param>
        /// <param name="jihiSbt">自費種別</param>
        /// <param name="isNodspRece">レセプト表示区分</param>
        /// <param name="isNodspPaperRece">紙レセプト表示区分</param>
        /// <param name="cdKbn">コード区分</param>
        /// <param name="inoutKbn">院内院外区分</param>
        /// <param name="kazeiKbn">課税区分</param>
        /// <returns>診療行為</returns>
        public SinKouiModel GetSinKoui(
            int hokenPid, int hokenId, string syukeiSaki, int count = 0, int jihiSbt = 0,
            int isNodspRece = NoDspConst.Dsp, int isNodspPaperRece = NoDspConst.Dsp, string cdKbn = "", int inoutKbn = 0, int kazeiKbn = 0)
        {
            SinKouiModel sinKouiModel = new SinKouiModel(new SinKoui());

            sinKouiModel.HpId = _hpId;
            sinKouiModel.PtId = _ptId;
            sinKouiModel.SinYm = _sinDate / 100;
            sinKouiModel.HokenPid = hokenPid;
            sinKouiModel.HokenId = hokenId;
            sinKouiModel.SyukeiSaki = syukeiSaki;
            sinKouiModel.Count = count;
            sinKouiModel.EntenKbn = 0;
            sinKouiModel.JihiSbt = jihiSbt;
            sinKouiModel.KazeiKbn = kazeiKbn;
            sinKouiModel.IsNodspRece = isNodspRece;
            sinKouiModel.IsNodspPaperRece = isNodspPaperRece;
            sinKouiModel.CdKbn = cdKbn;
            sinKouiModel.InoutKbn = inoutKbn;

            return sinKouiModel;
        }

        /// <summary>
        /// 診療行為を追加する
        /// </summary>
        /// <param name="sinKouiModel"></param>
        public SinKouiModel AppendSinKoui(SinKouiModel sinKouiModel)
        {
            if (_sinSeqNo > 0)
            {
                CommitSinKoui();
            }

            _sinSeqNo++;
            _sinRowNo = 0;
            _itemSeqNo = 0;
            _firstItem = true;

            sinKouiModel.KeyNo = _sinKeyNo;
            sinKouiModel.RpNo = _sinRpNo;
            sinKouiModel.SeqNo = _sinSeqNo;

            sinKouiModel.UpdateState = UpdateStateConst.Add;
            _sinKouis.Add(sinKouiModel);

            return _sinKouis.Last();
        }

        /// <summary>
        /// 診療行為を生成し、追加する
        /// </summary>
        /// <param name="wrkSinKoui">ワーク診療行為情報</param>
        public SinKouiModel AppendNewSinKoui(WrkSinKouiModel wrkSinKoui)
        {
            return AppendSinKoui(GetSinKoui(wrkSinKoui));
        }

        /// <summary>
        /// 診療行為を生成し、追加する
        /// </summary>
        /// <param name="hokenPid">保険組み合わせID</param>
        /// <param name="hokenId">保険ID</param>
        /// <param name="syukeiSaki">集計先</param>
        /// <param name="count">回数</param>
        /// <param name="jihiSbt">自費種別</param>
        /// <param name="isNodspRece">レセプト表示区分</param>
        /// <param name="isNodspPaperRece">紙レセプト表示区分</param>
        /// <param name="cdKbn">コード区分</param>
        /// <param name="inoutKbn">院内院外区分</param>
        /// <param name="kazeiKbn">課税区分</param>
        /// <returns></returns>
        public SinKouiModel AppendNewSinKoui(
            int hokenPid, int hokenId, string syukeiSaki, int count = 0, int jihiSbt = 0,
            int isNodspRece = NoDspConst.Dsp, int isNodspPaperRece = NoDspConst.Dsp, string cdKbn = "", int inoutKbn = 0, int kazeiKbn = 0)
        {
            return AppendSinKoui(GetSinKoui(
                    hokenPid: hokenPid, hokenId: hokenId, syukeiSaki: syukeiSaki, count: count, jihiSbt: jihiSbt,
                    isNodspRece: isNodspRece, isNodspPaperRece: isNodspPaperRece, cdKbn: cdKbn, inoutKbn: inoutKbn, kazeiKbn: kazeiKbn));
        }

        /// <summary>
        /// 診療行為を確定する
        /// もし、付随する診療行為詳細がない場合は、追加を取り消す
        /// </summary>
        public void CommitSinKoui()
        {
            if (_sinKouiDetails.Any(p => p.RpNo == _sinRpNo && p.SeqNo == _sinSeqNo) == false)
            {
                //付随する詳細がない場合、ワーク診療行為を削除する
                _sinKouis.RemoveAll(p => p.RpNo == _sinRpNo && p.SeqNo == _sinSeqNo);
                _sinSeqNo--;
            }
            else
            {
                // RecId
                if (_sinKouiDetails.Any(p =>
                        p.RpNo == _sinKouis.Last().RpNo &&
                        p.SeqNo == _sinKouis.Last().SeqNo &&
                        p.IsNodspRece == 0 &&
                        p.RecId == "SI"))
                {
                    _sinKouis.Last().RecId = "SI";
                }
                else if (_sinKouis.Last().CdKbn == "J" &&
                    _sinKouiDetails.Any(p =>
                        p.RpNo == _sinKouis.Last().RpNo &&
                        p.SeqNo == _sinKouis.Last().SeqNo &&
                        p.TenMst != null &&
                        new int[] { 2, 3, 4, 5 }.Contains(p.TenMst.SansoKbn)))
                {
                    // 処置酸素は手技にまとめる
                    _sinKouis.Last().RecId = "SI";
                }
                //else if (_wrkSinRpInfs.Any(p => 
                //            p.RpNo == _wrkSinKouis.Last().RpNo && 
                //            (p.SinId == 31 || p.SinId == 32)))
                //{
                //    // 静脈注射、皮下筋肉内注射行為は手技薬剤特材をひとつのRpにまとめる
                //    _wrkSinKouis.Last().RecId = "SI";
                //}
                else if (_sinKouiDetails.Any(p =>
                            p.RpNo == _sinKouis.Last().RpNo &&
                            p.SeqNo == _sinKouis.Last().SeqNo &&
                            p.RecId == "IY"))
                {
                    _sinKouis.Last().RecId = "IY";
                }
                else if (_sinKouiDetails.Any(p =>
                            p.RpNo == _sinKouis.Last().RpNo &&
                            p.SeqNo == _sinKouis.Last().SeqNo &&
                            p.IsNodspRece == 0 &&
                            p.RecId == "CO") &&
                        _sinKouiDetails.Any(p =>
                            p.RpNo == _sinKouis.Last().RpNo &&
                            p.SeqNo == _sinKouis.Last().SeqNo &&
                            p.IsNodspRece == 0 &&
                            p.RecId != "CO") == false &&
                        _sinKouiDetails.Any(p =>
                            p.RpNo == _sinKouis.Last().RpNo &&
                            p.SeqNo == _sinKouis.Last().SeqNo &&
                            p.IsNodspRece != 0 &&
                            p.RecId == "SI") &&
                         _sinKouiDetails.Any(p =>
                            p.RpNo == _sinKouis.Last().RpNo &&
                            p.SeqNo == _sinKouis.Last().SeqNo &&
                            p.IsNodspRece != 0 &&
                            p.RecId != "SI") == false)
                {
                    // IS_NODSP_RECE=0でCO以外の項目がなく、
                    // IS_NODSP_RECE<>0でSI以外の項目がない場合
                    // つまり、COのみが表示項目で、SIのみが非表示項目の場合はSI扱い
                    _sinKouis.Last().RecId = "SI";
                }
                else
                {
                    _sinKouis.Last().RecId = "TO";
                }
            }
        }

        /// <summary>
        /// 最後に追加した行為レコードと付随する詳細レコードを削除する
        /// </summary>
        public void RemoveLastKoui()
        {
            // 詳細削除
            SinKouiDetails.RemoveAll(p =>
                                p.KeyNo == _sinKouis.Last().KeyNo &&
                                p.RpNo == _sinKouis.Last().RpNo &&
                                p.SeqNo == _sinKouis.Last().SeqNo);
            // 行為削除
            SinKouis.RemoveAll(p =>
                p.KeyNo == _sinKouis.Last().KeyNo &&
                p.RpNo == _sinKouis.Last().RpNo &&
                p.SeqNo == _sinKouis.Last().SeqNo);

            // シーケンスを戻しておく
            _sinSeqNo--;
        }

        /// <summary>
        /// 診療詳細データを取得する
        /// （付随する診療詳細情報の内容を文字列化したもの）
        /// </summary>
        /// <param name="sinKoui">取得したい診療行為情報</param>
        /// <returns>
        /// 付随する診療詳細情報の内容を文字列化したもの
        /// 並び順は問わないので、キー順でソートする
        /// </returns>
        public string GetDetailData(SinKouiModel sinKoui)
        {
            string ret = "";
            string tmpStr = "";

            List<SinKouiDetailModel> sinDtls = 
                _sinKouiDetails.FindAll(p => p.KeyNo == sinKoui.KeyNo && p.SeqNo == sinKoui.SeqNo)
                .OrderBy(p=>p.ItemCd)
                .ThenBy(p=>p.ItemName)
                .ThenBy(p=>p.Suryo)
                .ThenBy(p=>p.Suryo2)
                .ThenBy(p=>p.FmtKbn)
                .ThenBy(p=>p.UnitCd)
                .ThenBy(p=>p.UnitName)
                .ThenBy(p=>p.Ten)
                .ThenBy(p => p.Zei)
                .ThenBy(p => p.IsNodspRece)
                .ThenBy(p => p.IsNodspPaperRece)
                .ThenBy(p => p.IsNodspRyosyu)
                .ThenBy(p=>p.CmtOpt)
                .ThenBy(p=>p.CmtCd1)
                .ThenBy(p=>p.CmtOpt1)
                .ThenBy(p => p.CmtCd2)
                .ThenBy(p => p.CmtOpt2)
                .ThenBy(p => p.CmtCd3)
                .ThenBy(p => p.CmtOpt3)
                .ThenBy(p => p.RecId)
                .ToList();

            foreach (SinKouiDetailModel sinDtl in sinDtls)
            {
                // 詳細の内容を文字列化する
                tmpStr = MakeDetailData(sinDtl);

                if (tmpStr != "")
                {
                    ret = AddStr(ret, tmpStr);
                }
            }

            return ret;
        }

        /// <summary>
        /// 集計先を取得する
        /// </summary>
        /// <param name="keyNo">キー番号</param>
        /// <param name="seqNo">連番</param>
        /// <returns>
        /// 指定のキー番号、連番に該当する診療行為情報の集計先
        /// 診療行為情報が見つからない場合、空文字を返す
        /// </returns>
        public string GetSyukeiSaki(int keyNo, int seqNo)
        {
            string ret = "";

            List<SinKouiModel> sinKouis =
                _sinKouis.FindAll(p => p.KeyNo == keyNo && p.SeqNo == seqNo);

            if (sinKouis.Any())
            {
                ret = sinKouis.First().SyukeiSaki;
            }

            return ret;
        }

        /// <summary>
        /// 診療行為データを生成する
        /// </summary>
        /// <param name="sinKoui">診療行為情報</param>
        /// <returns>指定の診療行為情報から生成した診療行為データ</returns>
        private string MakeKouiData(SinKouiModel sinKoui)
        {
            string ret = "";

            ret = AddStr(ret, MakeData("sk", sinKoui.SyukeiSaki));
            ret = AddStr(ret, MakeData("ndrc", sinKoui.IsNodspRece));
            ret = AddStr(ret, MakeData("ndp", sinKoui.IsNodspPaperRece));
            ret = AddStr(ret, MakeData("io", sinKoui.InoutKbn));
            if (sinKoui.DetailData.Contains(ItemCdConst.CommentSaisinDojitu) ||
               sinKoui.DetailData.Contains(ItemCdConst.CommentDenwaSaisin) ||
               sinKoui.DetailData.Contains(ItemCdConst.CommentDenwaSaisinDojitu) ||
               sinKoui.DetailData.Contains(ItemCdConst.CommentSyosinJikanNai))
            {
                // 初再診のコメントの場合は１つにまとめたいのでpidは不問にする
            }
            else
            {
                ret = AddStr(ret, MakeData("pid", sinKoui.HokenPid));                
            }
            ret = AddStr(ret, MakeData("hid", sinKoui.HokenId));
            ret = AddStr(ret, MakeData("jihi", sinKoui.JihiSbt));
            ret = AddStr(ret, MakeData("kazei", sinKoui.KazeiKbn));
            ret = AddStr(ret, MakeData("rid", sinKoui.RecId));
            //ret = AddStr(ret, MakeData("et", sinKoui.EntenKbn));
            ret = AddStr(ret, String.Format("\"{0}\":{1}", "et", sinKoui.EntenKbn));
            ret = AddStr(ret, MakeData("rp", sinKoui.DetailData, "[", "]"));

            if (ret != "")
            {
                ret = "{" + ret + "}";
            }

            return ret;
        }

        /// <summary>
        /// 指定のキー番号に紐づく診療行為を取得する
        /// </summary>
        /// <param name="keyNo">キー番号</param>
        /// <returns>指定のキー番号に紐づく診療行為</returns>
        public List<SinKouiModel> FilterSinKoui(int keyNo)
        {
            return _sinKouis.FindAll(p => p.KeyNo == keyNo);
        }

        #endregion

        #region 診療行為詳細情報関連

        /// <summary>
        /// 診療行為詳細情報を生成する
        /// </summary>
        /// <param name="wrkSinDtl">ワーク診療行為詳細情報</param>
        /// <returns>診療行為詳細情報</returns>
        public SinKouiDetailModel GetSinKouiDetail(WrkSinKouiDetailModel wrkSinDtl)
        {
            SinKouiDetailModel sinKouiDtl = new SinKouiDetailModel(new SinKouiDetail(), new TenMst());

            sinKouiDtl.HpId = _hpId;
            sinKouiDtl.PtId = _ptId;
            sinKouiDtl.SinYm = _sinDate / 100;
            sinKouiDtl.RecId = wrkSinDtl.RecId;
            sinKouiDtl.ItemSbt = wrkSinDtl.ItemSbt;
            sinKouiDtl.ItemCd = wrkSinDtl.ItemCd;
            sinKouiDtl.OdrItemCd = wrkSinDtl.OdrItemCd;
            sinKouiDtl.ItemName = wrkSinDtl.ItemName;
            sinKouiDtl.Suryo = wrkSinDtl.Suryo;
            sinKouiDtl.Suryo2 = wrkSinDtl.Suryo2;
            sinKouiDtl.FmtKbn = wrkSinDtl.FmtKbn;
            sinKouiDtl.UnitCd = wrkSinDtl.UnitCd;
            sinKouiDtl.UnitName = wrkSinDtl.UnitName;
            sinKouiDtl.IsNodspRece = wrkSinDtl.IsNodspRece;
            sinKouiDtl.IsNodspPaperRece = wrkSinDtl.IsNodspPaperRece;
            sinKouiDtl.IsNodspRyosyu = wrkSinDtl.IsNodspRyosyu;
            sinKouiDtl.CmtOpt = wrkSinDtl.CmtOpt;
            sinKouiDtl.CmtCd1 = wrkSinDtl.CmtCd1;
            sinKouiDtl.CmtOpt1 = wrkSinDtl.CmtOpt1;
            sinKouiDtl.CmtCd2 = wrkSinDtl.CmtCd2;
            sinKouiDtl.CmtOpt2 = wrkSinDtl.CmtOpt2;
            sinKouiDtl.CmtCd3 = wrkSinDtl.CmtCd3;
            sinKouiDtl.CmtOpt3 = wrkSinDtl.CmtOpt3;
            sinKouiDtl.MinYakka = wrkSinDtl.MinYakka;
            sinKouiDtl.TenZero = wrkSinDtl.TenZero;

            // 点数マスタ取得
            if (wrkSinDtl.TenMst == null)
            {
                List<TenMstModel> tenMsts = _mstCommon.GetTenMst(wrkSinDtl.ItemCd);
                if (tenMsts.Any())
                {
                    sinKouiDtl.TenMst = tenMsts.First();
                }
            }
            else
            {
                sinKouiDtl.TenMst = wrkSinDtl.TenMst;
            }

            // 個別点数計算
            if (sinKouiDtl.TenMst != null)
            {
                sinKouiDtl.Ten = (double)((decimal)(sinKouiDtl.Suryo * sinKouiDtl.ItemTen));
            }
            
            return sinKouiDtl;
        }

        /// <summary>
        /// 診療行為詳細情報を生成する
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="recId">レコード識別</param>
        /// <param name="itemSbt">項目種別</param>
        /// <param name="itemName">項目名称</param>
        /// <param name="suryo">数量</param>
        /// <param name="suryo2">数量２</param>
        /// <param name="fmtKbn">書式区分</param>
        /// <param name="unitCd">単位コード</param>
        /// <param name="unitName">単位名称</param>
        /// <param name="isNodspRece">レセプト非表示区分</param>
        /// <param name="isNodspPaperRece">紙レセプト非表示区分</param>
        /// <param name="isNodspRyosyu">領収証日表示区分</param>
        /// <param name="cmtOpt">コメントデータ</param>
        /// <param name="cmtCd1">コメントコード１</param>
        /// <param name="cmtOpt1">コメントデータ１</param>
        /// <param name="cmtCd2">コメントコード２</param>
        /// <param name="cmtOpt2">コメントデータ２</param>
        /// <param name="cmtCd3">コメントコード３</param>
        /// <param name="cmtOpt3">コメントデータ３</param>
        /// <returns></returns>
        public SinKouiDetailModel GetSinKouiDetail(string itemCd, string recId = "", int itemSbt = 0,
            string itemName = "", double suryo = 0, double suryo2 = 0, int fmtKbn = 0, int unitCd = 0, string unitName = "",
            int isNodspRece = 0, int isNodspPaperRece = 0, int isNodspRyosyu = 0, string cmtOpt = "", string cmtCd1 = "", string cmtOpt1 = "",
            string cmtCd2 = "", string cmtOpt2 = "", string cmtCd3 = "", string cmtOpt3 = "")
        {
            SinKouiDetailModel sinKouiDtl = new SinKouiDetailModel(new SinKouiDetail(), new TenMst());

            sinKouiDtl.HpId = _hpId;
            sinKouiDtl.PtId = _ptId;
            sinKouiDtl.SinYm = _sinDate / 100;
            sinKouiDtl.RecId = recId;
            sinKouiDtl.ItemSbt = itemSbt;
            sinKouiDtl.ItemCd = itemCd;
            sinKouiDtl.OdrItemCd = itemCd;

            List<TenMstModel> tenMsts = _mstCommon.GetTenMst(itemCd);
            if (tenMsts.Any())
            {
                sinKouiDtl.TenMst = tenMsts.First();

                if (itemName != "")
                {
                    sinKouiDtl.ItemName = itemName;
                }
                else
                {
                    sinKouiDtl.ItemName = tenMsts.First().ReceName;
                }

                if (unitCd > 0)
                {
                    sinKouiDtl.UnitCd = unitCd;
                }
                else
                {
                    sinKouiDtl.UnitCd = CIUtil.StrToIntDef(tenMsts.First().ReceUnitCd, 0);
                }

                if (unitName != "")
                {
                    sinKouiDtl.UnitName = unitName;
                }
                else
                {
                    sinKouiDtl.UnitName = tenMsts.First().ReceUnitName;
                }

                if (isNodspRece > 0)
                {
                    sinKouiDtl.IsNodspRece = isNodspRece;
                }
                else
                {
                    sinKouiDtl.IsNodspRece = tenMsts.First().IsNodspRece;
                }

                if (isNodspPaperRece > 0)
                {
                    sinKouiDtl.IsNodspPaperRece = isNodspPaperRece;
                }
                else
                {
                    sinKouiDtl.IsNodspPaperRece = tenMsts.First().IsNodspPaperRece;
                }

                if (isNodspRyosyu > 0)
                {
                    sinKouiDtl.IsNodspRyosyu = isNodspRyosyu;
                }
                else
                {
                    sinKouiDtl.IsNodspRyosyu = tenMsts.First().IsNodspRyosyu;
                }
            }
            else
            {
                sinKouiDtl.ItemName = itemName;
                sinKouiDtl.UnitCd = unitCd;
                sinKouiDtl.UnitName = unitName;
                sinKouiDtl.IsNodspRece = isNodspRece;
                sinKouiDtl.IsNodspPaperRece = isNodspPaperRece;
                sinKouiDtl.IsNodspRyosyu = isNodspRyosyu;
            }

            if (suryo > 0)
            {
                sinKouiDtl.Suryo = suryo;
            }
            else
            {
                sinKouiDtl.Suryo = 1;
            }

            sinKouiDtl.Suryo2 = suryo2;
            sinKouiDtl.FmtKbn = fmtKbn;

            sinKouiDtl.CmtOpt = cmtOpt;
            sinKouiDtl.CmtCd1 = cmtCd1;
            sinKouiDtl.CmtOpt1 = cmtOpt1;
            sinKouiDtl.CmtCd2 = cmtCd2;
            sinKouiDtl.CmtOpt2 = cmtOpt2;
            sinKouiDtl.CmtCd3 = cmtCd3;
            sinKouiDtl.CmtOpt3 = cmtOpt3;

            // 個別点数計算
            if (sinKouiDtl.TenMst != null)
            {
                sinKouiDtl.Ten = (double)((decimal)(sinKouiDtl.Suryo * sinKouiDtl.ItemTen));
            }

            return sinKouiDtl;
        }

        /// <summary>
        /// 診療行為詳細情報を追加する
        /// </summary>
        /// <param name="sinKouiDtl">診療行為詳細情報</param>
        /// <returns>追加した診療行為詳細情報</returns>
        public SinKouiDetailModel AppendSinKouiDetail(SinKouiDetailModel sinKouiDtl)
        {
            _sinRowNo++;

            if (sinKouiDtl.ItemSbt == 0)
            {
                if (_itemSeqNo > 0 && _firstItem == true)
                {
                    // この場合、前にコメントがあるような状況なので、_itemSeqNoは既に採番済みのはず
                }
                else
                {
                    // 診療行為項目が来た場合、枝番は1に戻す
                    _itemSeqNo++;
                }
                _firstItem = false;
            }
            else
            {
                //コメントの場合
                if (_itemSeqNo == 0)
                {
                    // この項目より前に診療行為項目はない→この後の診療行為項目に付随するコメントとみなす
                    _itemSeqNo++;
                }
            }

            sinKouiDtl.KeyNo = _sinKeyNo;
            sinKouiDtl.RpNo = _sinRpNo;
            sinKouiDtl.SeqNo = _sinSeqNo;
            sinKouiDtl.RowNo = _sinRowNo;
            sinKouiDtl.ItemSeqNo = _itemSeqNo;

            sinKouiDtl.UpdateState = UpdateStateConst.Add;
            _sinKouiDetails.Add(sinKouiDtl);
            return _sinKouiDetails.Last();
        }

        /// <summary>
        /// 診療行為詳細情報を生成し、追加する
        /// </summary>
        /// <param name="wrkSinKouiDtl">ワーク診療行為詳細</param>
        /// <returns>追加した診療行為詳細</returns>
        public SinKouiDetailModel AppendNewSinKouiDetail(WrkSinKouiDetailModel wrkSinKouiDtl)
        {
            return AppendSinKouiDetail(GetSinKouiDetail(wrkSinKouiDtl));
        }

        public SinKouiDetailModel AppendNewSinKouiDetail(string itemCd, string recId = "", int itemSbt = 0,
            string itemName = "", double suryo = 0, double suryo2 = 0, int fmtKbn = 0, int unitCd = 0, string unitName = "",
            int isNodspRece = 0, int isNodspPaperRece = 0, int isNodspRyosyu = 0, string cmtOpt = "", string cmtCd1 = "", string cmtOpt1 = "",
            string cmtCd2 = "", string cmtOpt2 = "", string cmtCd3 = "", string cmtOpt3 = "")
        {
            return AppendSinKouiDetail(
                GetSinKouiDetail(
                    itemCd,
                    recId: recId, itemSbt: itemSbt, itemName: itemName, suryo: suryo, suryo2: suryo2,
                    fmtKbn: fmtKbn, unitCd: unitCd, unitName: unitName,
                    isNodspRece: isNodspRece, isNodspPaperRece: isNodspPaperRece, isNodspRyosyu: isNodspRyosyu,
                    cmtOpt: cmtOpt, cmtCd1: cmtCd1, cmtOpt1: cmtOpt1, cmtCd2: cmtCd2, cmtOpt2: cmtOpt2, cmtCd3: cmtCd3, cmtOpt3: cmtOpt3));
        }

        /// <summary>
        /// 指定Rpの診療行為詳細データのリストを返す
        /// </summary>
        /// <param name="keyNo">キー番号</param>
        /// <param name="seqNo">連番</param>
        /// <returns>指定のキー番号、連番に紐づく診療行為詳細のリスト</returns>
        public List<SinKouiDetailModel> FilterSinKouiDetail(int keyNo, int seqNo)
        {
            return _sinKouiDetails.FindAll(p => p.KeyNo == keyNo && p.SeqNo == seqNo);
        }

        /// <summary>
        /// 指定の診療行為コードに紐づく診療行為詳細情報を取得する
        /// （複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <returns>指定の診療行為コードに紐づく診療行為詳細情報のリスト</returns>
        public List<SinKouiDetailModel> FilterSinKouiDetailByItemCd(List<string> itemCds)
        {
            return _sinKouiDetails.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.UpdateState != UpdateStateConst.Delete &&
                itemCds.Contains(p.ItemCd));
        }

        /// <summary>
        /// 指定の診療行為コードに紐づく診療行為詳細情報を取得する
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns>指定の診療行為コードに紐づく診療行為詳細情報のリスト</returns>
        public List<SinKouiDetailModel> FilterSinKouiDetailByItemCd(string itemCd)
        {
            return _sinKouiDetails.FindAll(p =>
                p.HpId == _hpId &&
                p.PtId == _ptId &&
                p.UpdateState != UpdateStateConst.Delete
                && p.ItemCd == itemCd);
        }

        /// <summary>
        /// 指定の診療行為コードが存在するかチェックする
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <returns>true: 存在する</returns>
        public bool ExistSinKouiDetailByItemCd(string itemCd)
        {
            var sinKouiCounts =
                _sinKouiCounts.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinDate == _sinDate &&
                    //p.SinYm == _sinDate / 100 &&
                    //p.SinDay == _sinDate % 100 &&
                    p.UpdateState == UpdateStateConst.None);
            var sinDtls =
                _sinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinYm == _sinDate / 100 &&
                    p.ItemCd == itemCd &&
                    p.UpdateState == UpdateStateConst.None);
            var _join = (
                from sinCount in sinKouiCounts
                join sinDtl in sinDtls on
                    new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo }
                where
                sinCount.HpId == _hpId &&
                sinCount.PtId == _ptId &&
                sinCount.SinDate == _sinDate
                //sinCount.SinYm == _sinDate / 100 &&
                //sinCount.SinDay == _sinDate % 100
                select new
                {
                    sinCount
                }
               );

            return _join.ToList().Any();
        }

        /// <summary>
        /// 指定の診療行為コードが診療日に存在するかチェックする（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <returns>true: 存在する</returns>
        public bool ExistSinKouiDetailByItemCd(List<string> itemCds)
        {
            var sinKouiCounts =
                _sinKouiCounts.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinDate == _sinDate &&
                    //p.SinYm == _sinDate / 100 &&
                    //p.SinDay == _sinDate % 100 &&
                    p.UpdateState == UpdateStateConst.None);
            var sinDtls =
                _sinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinYm == _sinDate / 100 &&
                    itemCds.Contains(p.ItemCd) &&
                    p.UpdateState == UpdateStateConst.None);
            var sinRpInfs = _sinRpInfs.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinYm == _sinDate / 100 &&
                    //o.HokenKbn != 4 &&
                    checkHokenKbn.Contains(o.HokenKbn) &&
                    o.SanteiKbn != SanteiKbnConst.SanteiGai &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                    o.IsDeleted == DeleteStatus.None
                );
            var _join = (
                from sinCount in sinKouiCounts
                join sinDtl in sinDtls on
                    new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.SinYm, sinDtl.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                sinCount.HpId == _hpId &&
                sinCount.PtId == _ptId &&
                sinCount.SinDate == _sinDate
                //sinCount.SinYm == _sinDate / 100 &&
                //sinCount.SinDay == _sinDate % 100
                select new
                {
                    sinCount
                }
               );

            return _join.ToList().Any();
        }

        /// <summary>
        /// 指定の診療行為コードが指定の来院に存在するかチェックする（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <returns>true: 存在する</returns>
        public bool ExistSinKouiDetailByItemCdRaiinNo(List<string> itemCds, long raiinNo)
        {
            var sinKouiCounts =
                _sinKouiCounts.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinDate == _sinDate &&
                    //p.SinYm == _sinDate / 100 &&
                    //p.SinDay == _sinDate % 100 &&
                    p.RaiinNo == raiinNo &&
                    p.UpdateState == UpdateStateConst.None);
            var sinDtls =
                _sinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinYm == _sinDate / 100 &&
                    itemCds.Contains(p.ItemCd) &&
                    p.UpdateState == UpdateStateConst.None);
            var sinRpInfs = _sinRpInfs.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinYm == _sinDate / 100 &&
                    //o.HokenKbn != 4 &&
                    checkHokenKbn.Contains(o.HokenKbn) &&
                    o.SanteiKbn != SanteiKbnConst.SanteiGai &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                    o.IsDeleted == DeleteStatus.None
                );
            var _join = (
                from sinCount in sinKouiCounts
                join sinDtl in sinDtls on
                    new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.SinYm, sinDtl.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                sinCount.HpId == _hpId &&
                sinCount.PtId == _ptId &&
                sinCount.SinDate == _sinDate &&
                //sinCount.SinYm == _sinDate / 100 &&
                //sinCount.SinDay == _sinDate % 100 &&
                sinCount.RaiinNo == raiinNo
                select new
                {
                    sinCount
                }
               );

            return _join.ToList().Any();
        }
        /// <summary>
        /// 指定の診療行為コードが指定の来院に存在するかチェックする（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <returns>true: 存在する</returns>
        public bool ExistSinKouiDetailByItemCdRaiinNo(List<string> itemCds, List<long> raiinNos)
        {
            var sinKouiCounts =
                _sinKouiCounts.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinDate == _sinDate &&
                    //p.SinYm == _sinDate / 100 &&
                    //p.SinDay == _sinDate % 100 &&
                    raiinNos.Contains(p.RaiinNo) &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add));
            var sinDtls =
                _sinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinYm == _sinDate / 100 &&
                    itemCds.Contains(p.ItemCd) &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add));
            var sinRpInfs = _sinRpInfs.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinYm == _sinDate / 100 &&
                    //o.HokenKbn != 4 &&
                    checkHokenKbn.Contains(o.HokenKbn) &&
                    o.SanteiKbn != SanteiKbnConst.SanteiGai &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                    o.IsDeleted == DeleteStatus.None
                );
            var _join = (
                from sinCount in sinKouiCounts
                join sinDtl in sinDtls on
                    new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.SinYm, sinDtl.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                sinCount.HpId == _hpId &&
                sinCount.PtId == _ptId &&
                sinCount.SinDate == _sinDate &&
                //sinCount.SinYm == _sinDate / 100 &&
                //sinCount.SinDay == _sinDate % 100 &&
                raiinNos.Contains(sinCount.RaiinNo)
                select new
                {
                    sinCount
                }
               );

            return _join.ToList().Any();
        }
        /// <summary>
        /// 指定の診療行為コードが診療日に存在するか、削除される予定のものも含めてチェックする（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <returns>true: 存在する</returns>
        public bool ExistSinKouiDetailByItemCdIncludeDelete(List<string> itemCds, List<long> excludeRaiinNos = null)
        {
            var sinKouiCounts =
                _sinKouiCounts.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinDate == _sinDate &&
                    ((excludeRaiinNos != null && excludeRaiinNos.Any()) ? excludeRaiinNos.Contains(p.RaiinNo) == false: true));
            var sinDtls =
                _sinKouiDetails.FindAll(p =>
                    p.HpId == _hpId &&
                    p.PtId == _ptId &&
                    p.SinYm == _sinDate / 100 &&
                    itemCds.Contains(p.ItemCd));
            var sinRpInfs = _sinRpInfs.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinYm == _sinDate / 100 &&
                    checkHokenKbn.Contains(o.HokenKbn) &&
                    o.SanteiKbn != SanteiKbnConst.SanteiGai
                );
            var _join = (
                from sinCount in sinKouiCounts
                join sinDtl in sinDtls on
                    new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } equals
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinDtl.HpId, sinDtl.PtId, sinDtl.SinYm, sinDtl.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                sinCount.HpId == _hpId &&
                sinCount.PtId == _ptId &&
                sinCount.SinDate == _sinDate
                select new
                {
                    sinCount
                }
               );

            return _join.ToList().Any();
        }
        /// <summary>
        /// 当月の内分泌検査の合計点数を取得する
        /// </summary>
        /// <returns>当月の内分泌検査の合計点数</returns>
        public double GetNaibunpituTotalCost()
        {
            var sinKouis = _sinKouis.FindAll(p => 
                p.IsDeleted == DeleteStatus.None && 
                p.UpdateState == UpdateStateConst.None);
            var sinKouiDetails = _sinKouiDetails.FindAll(p => 
                (p.TenMst == null ? 0 : p.TenMst.HokatuKensa) == 8 && 
                p.IsDeleted == DeleteStatus.None && 
                p.UpdateState == UpdateStateConst.None);
            var sinKouiCounts = _sinKouiCounts.FindAll(p => 
                p.SinDate <= _sinDate);

            var join = (

                    from sinKoui in sinKouis
                    join sinDtl in sinKouiDetails on
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.RpNo, sinKoui.SeqNo } equals
                        new { sinDtl.HpId, sinDtl.PtId, sinDtl.RpNo, sinDtl.SeqNo } into joinSinDtls
                    from joinSinDtl in joinSinDtls
                    join sinCount in sinKouiCounts on
                        new { sinKoui.HpId, sinKoui.PtId, sinKoui.RpNo, sinKoui.SeqNo } equals
                        new { sinCount.HpId, sinCount.PtId, sinCount.RpNo, sinCount.SeqNo } into joinSinCounts
                    from joinSinCount in joinSinCounts
                    select new
                    {
                        totalTen = joinSinCount.Count * joinSinDtl.Ten
                    }
                );

            //double ret =
            //    _sinKouiDetails.Where(p =>
            //        p.HpId == _hpId &&
            //        p.PtId == _ptId &&
            //        p.SinYm == _sinDate / 100 &&
            //        (p.TenMst == null ? 0 : p.TenMst.HokatuKensa) == 8 &&
            //        p.UpdateState == UpdateStateConst.None)
            //    .Sum(p => p.Ten);

            double ret = join.Sum(p => p.totalTen);

            if (ret < 0) ret = 0;

            return ret;
        }
        #endregion

        #region 診療行為カウント関連

        /// <summary>
        /// 診療行為カウント情報を生成する
        /// </summary>
        /// <param name="rpNo">Rp番号</param>
        /// <param name="seqNo">連番</param>
        /// <param name="keyNo">キー番号</param>
        /// <param name="count">カウント</param>
        /// <returns>診療行為カウント情報</returns>
        public SinKouiCountModel GetSinKouiCount(int rpNo, int seqNo, long keyNo, int count)
        {
            SinKouiCountModel sinKouiCount = new SinKouiCountModel(new SinKouiCount());

            sinKouiCount.HpId = _hpId;
            sinKouiCount.PtId = _ptId;
            sinKouiCount.SinYm = _sinDate / 100;
            sinKouiCount.SinDay = _sinDate % 100;
            sinKouiCount.SinDate = _sinDate;
            sinKouiCount.RaiinNo = _raiinNo;
            sinKouiCount.RpNo = rpNo;
            sinKouiCount.SeqNo = seqNo;
            sinKouiCount.KeyNo = keyNo;
            sinKouiCount.Count = count;

            return sinKouiCount;
        }

        /// <summary>
        /// 診療行為カウント情報を追加する
        /// </summary>
        /// <param name="sinKouiCount">診療行為カウント情報</param>
        public void AppendSinKouiCount(SinKouiCountModel sinKouiCount)
        {
            sinKouiCount.UpdateState = UpdateStateConst.Add;
            _sinKouiCounts.Add(sinKouiCount);
        }

        /// <summary>
        /// 診療行為カウント情報を生成し、追加する
        /// </summary>
        /// <param name="rpNo">Rp番号</param>
        /// <param name="seqNo">連番</param>
        /// <param name="keyNo">キー番号</param>
        /// <param name="count">カウント</param>
        public void AppendNewSinKouiCount(int rpNo, int seqNo, long keyNo, int count)
        {
            AppendSinKouiCount(GetSinKouiCount(rpNo, seqNo, keyNo, count));
        }

        /// <summary>
        /// 診療行為詳細データを取得する
        /// （診療行為詳細情報の内容を文字列化したもの）
        /// </summary>
        /// <param name="sinDtl">診療行為詳細情報</param>
        /// <returns>診療行為詳細情報の内容を文字列化したもの</returns>
        private string MakeDetailData(SinKouiDetailModel sinDtl)
        {
            string ret = "";

            ret = AddStr(ret, MakeData("cd", sinDtl.ItemCd));
            ret = AddStr(ret, MakeData("nm", sinDtl.ItemName));
            ret = AddStr(ret, MakeData("su", sinDtl.Suryo));
            ret = AddStr(ret, MakeData("su2", sinDtl.Suryo2));
            ret = AddStr(ret, MakeData("fmt", sinDtl.FmtKbn));
            ret = AddStr(ret, MakeData("ucd", sinDtl.UnitCd));
            ret = AddStr(ret, MakeData("ten", sinDtl.Ten));
            ret = AddStr(ret, MakeData("zei", sinDtl.Zei));
            ret = AddStr(ret, MakeData("ndr", sinDtl.IsNodspRece));
            ret = AddStr(ret, MakeData("ndp", sinDtl.IsNodspPaperRece));
            ret = AddStr(ret, MakeData("nds", sinDtl.IsNodspRyosyu));
            ret = AddStr(ret, MakeData("opt", sinDtl.CmtOpt));
            ret = AddStr(ret, MakeData("ccd1", sinDtl.CmtCd1));
            ret = AddStr(ret, MakeData("opt1", sinDtl.CmtOpt1));
            ret = AddStr(ret, MakeData("ccd2", sinDtl.CmtCd2));
            ret = AddStr(ret, MakeData("opt2", sinDtl.CmtOpt2));
            ret = AddStr(ret, MakeData("ccd3", sinDtl.CmtCd3));
            ret = AddStr(ret, MakeData("opt3", sinDtl.CmtOpt3));
            ret = AddStr(ret, MakeData("rid", sinDtl.RecId));

            if (ret != "")
            {
                ret = "{" + ret + "}";
            }

            return ret;
        }
        #endregion

        #region 診療Ｒｐ番号情報関連

        /// <summary>
        /// 診療Rp番号情報を生成する
        /// </summary>
        /// <param name="rpNo">Rp番号</param>
        /// <param name="keyNo">キー番号</param>
        /// <returns>診療Rp番号情報</returns>
        public SinRpNoInfModel GetSinRpNoInf(int rpNo, long keyNo)
        {
            SinRpNoInfModel sinRpNo = new SinRpNoInfModel(new SinRpNoInf());

            sinRpNo.HpId = _hpId;
            sinRpNo.PtId = _ptId;
            sinRpNo.SinYm = _sinDate / 100;
            sinRpNo.SinDay = _sinDate % 100;
            sinRpNo.RaiinNo = _raiinNo;
            sinRpNo.RpNo = rpNo;
            sinRpNo.KeyNo = keyNo;

            return sinRpNo;
        }

        /// <summary>
        /// 診療行為Rp番号情報を追加する
        /// </summary>
        /// <param name="sinRpNo">診療行為Rp番号情報</param>
        //public void AppendSinRpNoInf(SinRpNoInfModel sinRpNo)
        //{
        //    sinRpNo.UpdateState = UpdateStateConst.Add;
        //    _sinRpNoInfs.Add(sinRpNo);
        //}

        /// <summary>
        /// 診療行為Rp番号情報を生成し、追加する
        /// </summary>
        /// <param name="rpNo">Rp番号</param>
        /// <param name="keyNo">キー番号</param>
        //public void AppendNewSinRpNo(int rpNo, long keyNo)
        //{
        //    AppendSinRpNoInf(GetSinRpNoInf(rpNo, keyNo));
        //}
        #endregion

        /// <summary>
        /// 詳細データ、行為データのキーバリューを生成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="delimiter1"></param>
        /// <param name="delimiter2"></param>
        /// <returns></returns>
        private string MakeData(string id, string data, string delimiter1 = "\"", string delimiter2 = "\"")
        {
            string ret = "";

            if (data != null && data != "")
            {
                ret = String.Format("\"{0}\":" + delimiter1 + "{1}" + delimiter2, id, data);
            }

            return ret;
        }

        /// <summary>
        /// 詳細データ、行為データのキーバリューを生成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <param name="delimiter1"></param>
        /// <param name="delimiter2"></param>
        /// <returns></returns>
        private string MakeData(string id, double data, string delimiter1 = "", string delimiter2 = "")
        {
            string ret = "";

            if (data != 0)
            {
                ret = String.Format("\"{0}\":" + delimiter1 + "{1}" + delimiter2, id, data);
            }

            return ret;
        }

        private string AddStr(string s1, string s2)
        {
            if (s1 != "" && s2 != "")
            {
                s1 += ",";
            }
            return s1 + s2;
        }

        /// <summary>
        /// 指定のRpの薬剤を削除
        /// </summary>
        /// <param name="keyNo"></param>
        public void RemoveYakuzai(int keyNo, int seqNo)
        {
            List<int> itemSeqNos = new List<int>();
            List<SinKouiDetailModel> sinDtlYakus = _sinKouiDetails.FindAll(p => p.KeyNo == keyNo && p.SeqNo == seqNo && p.TenMst != null && p.TenMst.DrugKbn > 0);

            foreach (SinKouiDetailModel sinDtlYaku in sinDtlYakus)
            {
                itemSeqNos.Add(sinDtlYaku.ItemSeqNo);
            }

            // 付随するコメントも一緒に削除
            for (int i = 0; i < itemSeqNos.Count; i++)
            {
                _sinKouiDetails.RemoveAll(p => p.KeyNo == keyNo && p.SeqNo == seqNo && p.ItemSeqNo == itemSeqNos[i]);
            }
        }

        #region 算定チェック

        /// <summary>
        /// 当日の算定回数をカウントする
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns></returns>
        public bool CheckSanteiSinday(string itemCd, int santeiKbn = 0)
        {
            List<string> itemCds = new List<string>();
            itemCds.Add(itemCd);

            return CheckSanteiSinday(itemCds, santeiKbn);
        }

        /// <summary>
        /// 当日の算定回数をカウントする(複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns></returns>
        public bool CheckSanteiSinday(List<string> itemCds, int santeiKbn = 0)
        {
            int sinYm = SinDate / 100;
            
            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                //o.SinYm * 100 + o.SinDay == SinDate &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
                );
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None);

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == sinYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    sinKouiCount.SinDate == SinDate
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay == SinDate
                group sinKouiDetail by sinKouiDetail.HpId
            );

            return joinQuery.Any();
        }

        /// <summary>
        /// 指定期間（診療日と同月内のみ）の算定回数をカウントする(複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns></returns>
        public bool CheckSanteiTerm(List<string> itemCds, int startDate, int endDate, int santeiKbn = 0)
        {
            int sinYm = SinDate / 100;

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.SinDate != SinDate &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
                );
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None);

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == sinYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    sinKouiCount.SinDate >= startDate &&
                    sinKouiCount.SinDate <= endDate &&
                    sinKouiCount.SinDate != SinDate
                group sinKouiDetail by sinKouiDetail.HpId
            );

            return joinQuery.Any();
        }
        public bool CheckSyukeisakiTerm(List<string> syukeiSakis, int startDate, int endDate, int santeiKbn = 0)
        {
            int sinYm = SinDate / 100;

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouis = _sinKouis.FindAll(p=>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.IsDeleted == DeleteStatus.None &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                syukeiSakis.Contains(p.SyukeiSaki)
            );

            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.SinDate != SinDate &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
                );

            var joinQuery = (
                from sinKoui in sinKouis
                join sinKouiCount in sinKouiCounts on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo, sinKoui.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKoui.HpId, sinKoui.PtId, sinKoui.SinYm, sinKoui.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKoui.HpId == HpId &&
                    sinKoui.PtId == PtId &&
                    sinKoui.SinYm == sinYm &&
                    syukeiSakis.Contains(sinKoui.SyukeiSaki) &&
                    sinKouiCount.SinDate >= startDate &&
                    sinKouiCount.SinDate <= endDate &&
                    sinKouiCount.SinDate != SinDate
                group sinKoui by sinKoui.HpId
            );

            return joinQuery.Any();
        }
        /// <summary>
        /// 指定の診療日の、指定の診療行為の、算定回数
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定回数</returns>
        public double SanteiCountSinday(string itemCd, int santeiKbn = 0)
        {
            List<string> itemCds = new List<string>();
            itemCds.Add(itemCd);

            return SanteiCountSinday(itemCds, santeiKbn);
        }

        /// <summary>
        /// 指定の診療日の、指定の診療行為の、算定回数（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定回数</returns>
        public double SanteiCountSinday(List<string> itemCds, int santeiKbn = 0)
        {
            const string conFncName = nameof(SanteiCountSinday);

            int sinYm = SinDate / 100;
            
            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                //o.SinYm * 100 + o.SinDay >= SinDate &&
                o.RaiinNo != RaiinNo &&
                o.UpdateState != UpdateStateConst.Delete);
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == sinYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    sinKouiCount.SinDate == SinDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay == SinDate &&
                    sinKouiCount.RaiinNo != RaiinNo
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 ? 1 : a.sinKouiDetail.Suryo)) }
            );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 指定の期間（診療日と同じ月のみ、診療日除く）の、指定の診療行為の、算定回数
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定回数</returns>
        public double SanteiCountTerm(string itemCd, int startDate, int endDate, List<int> santeiKbns = null)
        {
            List<string> itemCds = new List<string>();
            itemCds.Add(itemCd);

            return SanteiCountTerm(itemCds, startDate, endDate, santeiKbns);
        }

        /// <summary>
        /// 指定の期間（診療日と同じ月のみ、診療日除く）の、指定の診療行為の、算定回数（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定回数</returns>
        public double SanteiCountTerm(List<string> itemCds, int startDate, int endDate, List<int> santeiKbns = null, List<int> hokenKbns = null)
        {
            const string conFncName = nameof(SanteiCountSinday);

            List<int> checkHokenKbns = checkHokenKbn;

            if (hokenKbns != null)
            {
                checkHokenKbns = hokenKbns;
            }

            List<int> checkSanteiKbns = new List<int>();

            if(santeiKbns != null)
            {
                checkSanteiKbns.AddRange(santeiKbns);
            }
            else
            {
                checkSanteiKbns.AddRange(checkSanteiKbn);
            }

            int sinYm = SinDate / 100;

            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                o.SinDate != SinDate &&
                o.RaiinNo != RaiinNo &&
                o.UpdateState != UpdateStateConst.Delete);
            if(sinKouiCounts.Any() == false) { return 0; }

            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None
                );
            if (sinKouiDetails.Any() == false) { return 0; }

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbns.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbns.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            if (sinRpInfs.Any() == false) { return 0; }

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == sinYm &&
                    itemCds.Contains(sinKouiDetail.ItemCd) &&
                    sinKouiCount.SinDate >= startDate &&
                    sinKouiCount.SinDate <= endDate &&
                    sinKouiCount.SinDate != SinDate &&
                    sinKouiCount.RaiinNo != RaiinNo
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 ? 1 : a.sinKouiDetail.Suryo)) }
            );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }

        }

        /// <summary>
        /// 診療日の指定の包括区分の項目の算定回数を取得する
        /// </summary>
        /// <param name="hokatuKbn">包括区分</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>診療日の指定の包括区分の項目の算定回数</returns>
        public double SanteiCountByHokatuKbnSinDay(int startDate, int endDate, int sinDate, long raiinNo, int hokatuKbn)
        {
            int sinYm = SinDate / 100;

            //List<int> pHokenKbn = 
            //    new List<int>() { hokenKbn };
            //if(hokenKbn == 4)
            //{
            //    // 自費保険の場合は健保も含める
            //    pHokenKbn.Add(0);
            //}
            //List<int> pHokenKbn = CalcUtils.GetCheckHokenKbns(hokenKbn);
            //List<int> pSanteiKbn = CalcUtils.GetCheckSanteiKbns(hokenKbn);

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                o.IsDeleted == DeleteStatus.None &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                //o.SinYm * 100 + o.SinDay == SinDate &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                (sinDate >= 0 ? o.SinDate != sinDate : true) &&
                (raiinNo >= 0 ? o.RaiinNo != raiinNo : true) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
                );
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                p.IsDeleted == DeleteStatus.None &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add)                
                );
            var tenMsts = _mstCommon.GetTenMstByHokatuKbn(hokatuKbn);

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                join tenMst in tenMsts on
                    new { sinKouiDetail.HpId, sinKouiDetail.ItemCd } equals
                    new { tenMst.HpId, tenMst.ItemCd }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == SinDate / 100 
                    //sinKouiCount.SinDate == SinDate
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay == SinDate
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 ? 1 : a.sinKouiDetail.Suryo)) }
            );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }


        }

        public double SanteiCountByHokatuKbnSinDay(int startDate, int endDate, int sinDate, long raiinNo, int hokatuKbn, string cdKbn, int cdKbnno, int cdEdano, int cdKouno)
        {            
            int sinYm = SinDate / 100;

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                o.IsDeleted == DeleteStatus.None &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                //o.SinYm * 100 + o.SinDay == SinDate &&
                (sinDate >= 0 ? o.SinDate != sinDate : true) &&
                (raiinNo >= 0 ? o.RaiinNo != RaiinNo : true) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
                );
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                p.TenMst != null &&
                (hokatuKbn >= 0 ? p.TenMst.HokatuKbn == hokatuKbn : true) &&
                (string.IsNullOrEmpty(cdKbn) == false ? p.TenMst.CdKbn == cdKbn : true) &&
                (cdKbnno >= 0 ? p.TenMst.CdKbnno == cdKbnno : true) &&
                (cdEdano >= 0 ? p.TenMst.CdEdano == cdEdano : true) &&
                (cdKouno >= 0 ? p.TenMst.CdKouno == cdKouno : true) &&
                p.IsDeleted == DeleteStatus.None &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add)
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId 
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 ? 1 : a.sinKouiDetail.Suryo)) }
            );

            var result = joinQuery.ToList();
            if (result.Any())
            {
                return result.FirstOrDefault().sum;
            }
            else
            {
                return 0;
            }


        }

        /// <summary>
        /// 診療日が属する月の指定の項目の算定日を取得する
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定日のリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysSinDay(string itemCd, List<int> santeiKbns = null)
        {
            List<string> itemCds = new List<string>();
            itemCds.Add(itemCd);

            return GetSanteiDaysSinDay(itemCds, santeiKbns);
        }

        /// <summary>
        /// 診療日の指定の項目の算定日を取得する（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定日のリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysSinDay(List<string> itemCds, List<int> santeiKbns = null, List<int> hokenKbns = null)
        {
            const string conFncName = nameof(GetSanteiDaysSinDay);

            List<int> checkHokenKbns = new List<int>();
            checkHokenKbns.AddRange(checkHokenKbn);
            
            if(hokenKbns != null)
            {
                checkHokenKbns = hokenKbns;
            }

            List<int> checkSanteiKbns = new List<int>();

            if(santeiKbns == null)
            {
                checkSanteiKbns = checkSanteiKbn;
            }
            else
            {
                checkSanteiKbns = santeiKbns;
            }

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == SinDate / 100 &&
                //o.HokenKbn != 4 &&
                checkHokenKbns.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbns.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate == SinDate &&
                //o.SinYm * 100 + o.SinDay == SinDate &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == SinDate / 100 &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == SinDate / 100 &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay == SinDate
                    sinKouiCount.SinDate == SinDate
                group new { sinKouiCount, sinKouiDetail }
                   //by new { sinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay, itemCd = sinKouiDetail.ItemCd } into A
                   by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd } into A
                orderby A.Key.sinDate
                select new { A.Key.sinDate, A.Key.itemCd }
            );

            var entities = joinQuery.ToList();

            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            entities?.ForEach(entity => {
                results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd));
            });

            return results;

        }

        public List<SanteiDaysModel> GetSanteiDaysSinDayHaihan(List<string> itemCds, List<int> santeiKbns = null, List<int> hokenKbns = null)
        {
            const string conFncName = nameof(GetSanteiDaysSinDay);

            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            if (itemCds.Any(p => p.StartsWith("J") || p.StartsWith("Z")))
            {
                List<int> checkHokenKbns = new List<int>();
                checkHokenKbns.AddRange(checkHokenKbn);

                if (hokenKbns != null)
                {
                    checkHokenKbns = hokenKbns;
                }

                List<int> checkSanteiKbns = new List<int>();

                if (santeiKbns == null)
                {
                    checkSanteiKbns = checkSanteiKbn;
                }
                else
                {
                    checkSanteiKbns = santeiKbns;
                }

                var sinRpInfs = _sinRpInfs.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinYm == SinDate / 100 &&
                    checkHokenKbns.Contains(o.HokenKbn) &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                    o.IsDeleted == DeleteStatus.None
                );
                var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinDate == SinDate &&
                    o.RaiinNo != RaiinNo &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
                var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                    p.HpId == HpId &&
                    p.PtId == PtId &&
                    p.SinYm == SinDate / 100 &&
                    (itemCds.FindAll(q => q.StartsWith("J")).Contains(p.ItemCd) ||
                     itemCds.FindAll(q => q.StartsWith("Z")).Contains(p.OdrItemCd)) &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                    p.IsDeleted == DeleteStatus.None
                    );

                var joinQuery = (
                    from sinKouiDetail in sinKouiDetails
                    join sinKouiCount in sinKouiCounts on
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                    join sinRpInf in sinRpInfs on
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                        new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                    where
                        sinKouiDetail.HpId == HpId &&
                        sinKouiDetail.PtId == PtId &&
                        sinKouiDetail.SinYm == SinDate / 100 &&
                        sinKouiCount.SinDate == SinDate
                    group new { sinKouiCount, sinKouiDetail }
                       by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd, odrItemCd = sinKouiDetail.OdrItemCd, sinRpInf.SanteiKbn } into A
                    orderby A.Key.sinDate
                    select new { A.Key.sinDate, A.Key.itemCd, A.Key.odrItemCd, A.Key.SanteiKbn }
                );

                var entities = joinQuery.ToList();

                entities?.ForEach(entity =>
                {
                    if (entity.itemCd.StartsWith("J") || (entity.odrItemCd.StartsWith("Z") && checkSanteiKbns.Contains(entity.SanteiKbn)))
                    {
                        results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd));
                    }
                });
            }

            return results;

        }
        /// <summary>
        /// 診療日の指定の項目の算定日を取得する（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定日のリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysInSinYM(int startDate, int endDate, List<string> itemCds, int santeiKbn = 0)
        {
            const string conFncName = nameof(GetSanteiDaysInSinYM);

            int startYm = startDate / 100;
            int endYm = endDate / 100;

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm >= startYm &&
                o.SinYm <= endYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinDate >= startDate &&
                o.SinDate <= endDate &&
                //o.SinYm * 100 + o.SinDay >= startDate &&
                //o.SinYm * 100 + o.SinDay <= endDate &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm >= startYm &&
                p.SinYm <= endYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add)&&
                p.IsDeleted == DeleteStatus.None
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm >= startYm &&
                    sinKouiDetail.SinYm <= endYm &&
                    sinKouiCount.SinDate >= startDate &&
                    sinKouiCount.SinDate <= endDate
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay >= startDate &&
                    //sinKouiCount.SinYm * 100 + sinKouiCount.SinDay <= endDate 
                group new { sinKouiCount, sinKouiDetail }
                   //by new { sinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay, itemCd = sinKouiDetail.ItemCd } into A
                   by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd } into A
                orderby A.Key.sinDate
                select new { A.Key.sinDate, A.Key.itemCd }
            );

            var entities = joinQuery.ToList();

            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            entities?.ForEach(entity => {
                results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd));
            });

            return results;

        }
        /// <summary>
        /// 診療日の指定の項目の算定日を取得する（複数指定）背反チェック処理専用
        /// </summary>
        /// <param name="startDate">取得期間From</param>
        /// <param name="endDate">取得期間To</param>
        /// <param name="itemCds">取得する診療行為コード</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns></returns>
        public List<SanteiDaysModel> GetSanteiDaysInSinYMHaihan(int startDate, int endDate, List<string> itemCds, int santeiKbn = 0)
        {
            const string conFncName = nameof(GetSanteiDaysInSinYMHaihan);
            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            if (itemCds.Any(p => p.StartsWith("J")) ||
                itemCds.Any(p => p.StartsWith("Z")))
            {
                // J自費、Z特材を含む場合
                int startYm = startDate / 100;
                int endYm = endDate / 100;

                var sinRpInfs = _sinRpInfs.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinYm >= startYm &&
                    o.SinYm <= endYm &&
                    checkHokenKbn.Contains(o.HokenKbn) &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
                );
                var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinDate >= startDate &&
                    o.SinDate <= endDate &&
                    o.RaiinNo != RaiinNo &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
                var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                    p.HpId == HpId &&
                    p.PtId == PtId &&
                    p.SinYm >= startYm &&
                    p.SinYm <= endYm &&
                    (itemCds.FindAll(q=>q.StartsWith("J")).Contains(p.ItemCd) || 
                     (itemCds.FindAll(q => q.StartsWith("Z")).Contains(p.OdrItemCd))) &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                    p.IsDeleted == DeleteStatus.None
                    );

                var joinQuery = (
                    from sinKouiDetail in sinKouiDetails
                    join sinKouiCount in sinKouiCounts on
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                    join sinRpInf in sinRpInfs on
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                        new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                    where
                        sinKouiDetail.HpId == HpId &&
                        sinKouiDetail.PtId == PtId &&
                        sinKouiDetail.SinYm >= startYm &&
                        sinKouiDetail.SinYm <= endYm &&
                        sinKouiCount.SinDate >= startDate &&
                        sinKouiCount.SinDate <= endDate
                    group new { sinKouiCount, sinKouiDetail }
                       by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd, odrItemCd = sinKouiDetail.OdrItemCd, sinRpInf.SanteiKbn } into A
                    orderby A.Key.sinDate
                    select new { A.Key.sinDate, A.Key.itemCd, A.Key.odrItemCd, A.Key.SanteiKbn }
                );

                var entities = joinQuery.ToList();

                entities?.ForEach(entity =>
                {
                    //if (entity.itemCd.StartsWith("J") || (entity.odrItemCd.StartsWith("Z") && checkSanteiKbn.Contains(entity.SanteiKbn)))
                    if (entity.itemCd.StartsWith("J") || (entity.odrItemCd.StartsWith("Z")))
                    {
                        // J自費 or Z特材
                        results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd, entity.odrItemCd, entity.SanteiKbn));
                    }
                });
            }
            return results;

        }
        public List<SanteiDaysModel> GetSanteiDaysInSinYMHaihanWithHokenKbn(int startDate, int endDate, List<string> itemCds, int santeiKbn = 0)
        {
            const string conFncName = nameof(GetSanteiDaysInSinYMHaihanWithHokenKbn);
            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            if (itemCds.Any(p => p.StartsWith("J")) ||
                itemCds.Any(p => p.StartsWith("Z")))
            {
                // J自費、Z特材を含む場合
                int startYm = startDate / 100;
                int endYm = endDate / 100;

                var sinRpInfs = _sinRpInfs.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinYm >= startYm &&
                    o.SinYm <= endYm &&
                    checkHokenKbn.Contains(o.HokenKbn) &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)
                );
                var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                    o.HpId == HpId &&
                    o.PtId == PtId &&
                    o.SinDate >= startDate &&
                    o.SinDate <= endDate &&
                    o.RaiinNo != RaiinNo &&
                    (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
                var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                    p.HpId == HpId &&
                    p.PtId == PtId &&
                    p.SinYm >= startYm &&
                    p.SinYm <= endYm &&
                    (itemCds.FindAll(q => q.StartsWith("J")).Contains(p.ItemCd) ||
                     (itemCds.FindAll(q => q.StartsWith("Z")).Contains(p.OdrItemCd))) &&
                    (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                    p.IsDeleted == DeleteStatus.None
                    );

                var joinQuery = (
                    from sinKouiDetail in sinKouiDetails
                    join sinKouiCount in sinKouiCounts on
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                        new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                    join sinRpInf in sinRpInfs on
                        new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                        new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                    where
                        sinKouiDetail.HpId == HpId &&
                        sinKouiDetail.PtId == PtId &&
                        sinKouiDetail.SinYm >= startYm &&
                        sinKouiDetail.SinYm <= endYm &&
                        sinKouiCount.SinDate >= startDate &&
                        sinKouiCount.SinDate <= endDate
                    group new { sinKouiCount, sinKouiDetail }
                       by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd, odrItemCd = sinKouiDetail.OdrItemCd, sinRpInf.SanteiKbn, sinRpInf.HokenKbn } into A
                    orderby A.Key.sinDate
                    select new { A.Key.sinDate, A.Key.itemCd, A.Key.odrItemCd, A.Key.SanteiKbn, A.Key.HokenKbn }
                );

                var entities = joinQuery.ToList();

                entities?.ForEach(entity =>
                {
                    //if (entity.itemCd.StartsWith("J") || (entity.odrItemCd.StartsWith("Z") && checkSanteiKbn.Contains(entity.SanteiKbn)))
                    if (entity.itemCd.StartsWith("J") || (entity.odrItemCd.StartsWith("Z")))
                    {
                        // J自費 or Z特材
                        results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd, entity.odrItemCd, entity.SanteiKbn));
                    }
                });
            }
            return results;

        }
        /// <summary>
        /// 診療日が属する月の指定の項目の算定日を取得する
        /// </summary>
        /// <param name="itemCd">診療行為コード</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定日のリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysSinYm(string itemCd, int santeiKbn = 0)
        {
            List<string> itemCds = new List<string>();
            itemCds.Add(itemCd);

            return GetSanteiDaysSinYm(itemCds, santeiKbn);
        }

        /// <summary>
        /// 診療日が属する月の指定の項目の算定日を取得する（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定日のリスト</returns>
        public List<SanteiDaysModel> GetSanteiDaysSinYm(List<string> itemCds, int santeiKbn = 0)
        {
            const string conFncName = nameof(GetSanteiDaysSinYm);

            int sinYm = SinDate / 100;
            
            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add)&&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == sinYm
                group new { sinKouiCount, sinKouiDetail }
                   //by new { sinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay, itemCd = sinKouiDetail.ItemCd } into A
                   by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd } into A
                orderby A.Key.sinDate
                select new { A.Key.sinDate, A.Key.itemCd }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());

            var entities = joinQuery.ToList();

            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            entities?.ForEach(entity => {
                results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd));
            });

            return results;

        }
        /// <summary>
        /// 診療日が属する月の指定の項目の算定日を取得する（複数指定）
        /// </summary>
        /// <param name="itemCds">診療行為コード（複数指定）</param>
        /// <param name="santeiKbn">算定区分</param>
        /// <returns>算定日のリスト</returns>
        public double GetSanteiCountSinYm(List<string> itemCds, int endDate, int santeiKbn = 0)
        {
            const string conFncName = nameof(GetSanteiCountSinYm);

            int sinYm = SinDate / 100;

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                o.SinDate <= endDate &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == sinYm
                //select new { sinKouiCount.Count }
                group new { sinKouiDetail, sinKouiCount } by new { sinKouiCount.HpId } into A
                select new { sum = A.Sum(a => (double)a.sinKouiCount.Count * (a.sinKouiDetail.Suryo <= 0 ? 1 : a.sinKouiDetail.Suryo)) }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());

            var entities = joinQuery.ToList();

            double result = 0;

            //entities?.ForEach(entity => {
            //    result += entity.Count;
            //});
            if(entities != null && entities.Any())
            {
                result = entities.First().sum;
            }

            return result;

        }
        public List<SanteiDaysModel> GetSanteiDaysSinYmWithSanteiKbn(List<string> itemCds, int santeiKbn = 0)
        {
            const string conFncName = nameof(GetSanteiDaysSinYmWithSanteiKbn);

            int sinYm = SinDate / 100;

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                //checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == sinYm
                group new { sinKouiCount, sinKouiDetail, sinRpInf }
                   //by new { sinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay, itemCd = sinKouiDetail.ItemCd } into A
                   by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd, santeiKbn = sinRpInf.SanteiKbn } into A
                orderby A.Key.sinDate
                select new { A.Key.sinDate, A.Key.itemCd, A.Key.santeiKbn }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());

            var entities = joinQuery.ToList();

            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            entities?.ForEach(entity => {
                results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd, entity.itemCd, entity.santeiKbn));
            });

            return results;

        }
        public List<SanteiDaysModel> GetSanteiDaysSinYmWithHokenKbn(List<string> itemCds, int santeiKbn = 0)
        {
            const string conFncName = nameof(GetSanteiDaysSinYmWithHokenKbn);

            int sinYm = SinDate / 100;

            var sinRpInfs = _sinRpInfs.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                //o.HokenKbn != 4 &&
                checkHokenKbn.Contains(o.HokenKbn) &&
                //o.SanteiKbn == santeiKbn &&
                //checkSanteiKbn.Contains(o.SanteiKbn) &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add) &&
                o.IsDeleted == DeleteStatus.None
            );
            var sinKouiCounts = _sinKouiCounts.FindAll(o =>
                o.HpId == HpId &&
                o.PtId == PtId &&
                o.SinYm == sinYm &&
                o.RaiinNo != RaiinNo &&
                (o.UpdateState == UpdateStateConst.None || o.UpdateState == UpdateStateConst.Add));
            var sinKouiDetails = _sinKouiDetails.FindAll(p =>
                p.HpId == HpId &&
                p.PtId == PtId &&
                p.SinYm == sinYm &&
                itemCds.Contains(p.ItemCd) &&
                (p.UpdateState == UpdateStateConst.None || p.UpdateState == UpdateStateConst.Add) &&
                p.IsDeleted == DeleteStatus.None
                );

            var joinQuery = (
                from sinKouiDetail in sinKouiDetails
                join sinKouiCount in sinKouiCounts on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo, sinKouiDetail.SeqNo } equals
                    new { sinKouiCount.HpId, sinKouiCount.PtId, sinKouiCount.SinYm, sinKouiCount.RpNo, sinKouiCount.SeqNo }
                join sinRpInf in sinRpInfs on
                    new { sinKouiDetail.HpId, sinKouiDetail.PtId, sinKouiDetail.SinYm, sinKouiDetail.RpNo } equals
                    new { sinRpInf.HpId, sinRpInf.PtId, sinRpInf.SinYm, sinRpInf.RpNo }
                where
                    sinKouiDetail.HpId == HpId &&
                    sinKouiDetail.PtId == PtId &&
                    sinKouiDetail.SinYm == sinYm
                group new { sinKouiCount, sinKouiDetail, sinRpInf }
                   //by new { sinDate = sinKouiCount.SinYm * 100 + sinKouiCount.SinDay, itemCd = sinKouiDetail.ItemCd } into A
                   by new { sinDate = sinKouiCount.SinDate, itemCd = sinKouiDetail.ItemCd, santeiKbn = sinRpInf.SanteiKbn, hokenKbn = sinRpInf.HokenKbn } into A
                orderby A.Key.sinDate
                select new { A.Key.sinDate, A.Key.itemCd, A.Key.santeiKbn, A.Key.hokenKbn }
            );

            //_emrLogger.WriteLogMsg( this, conFncName, joinQuery.AsString());

            var entities = joinQuery.ToList();

            List<SanteiDaysModel> results = new List<SanteiDaysModel>();

            entities?.ForEach(entity => {
                results.Add(new SanteiDaysModel(entity.sinDate, entity.itemCd, entity.itemCd, entity.santeiKbn, entity.hokenKbn));
            });

            return results;

        }
        #endregion

        public void Reload()
        {
            //foreach(SinRpInfModel sinRpInf in _sinRpInfs.FindAll(p=>p.UpdateState != UpdateStateConst.None))
            //{
            //    _santeiFinder.SinRpInfReload(sinRpInf.SinRpInf);
            //}

            //foreach (SinRpNoInfModel sinRpNoInf in _sinRpNoInfs.FindAll(p => p.UpdateState != UpdateStateConst.None))
            //{
            //    _santeiFinder.SinRpNoInfReload(sinRpNoInf.SinRpNoInf);
            //}

            //foreach (SinKouiModel sinKoui in _sinKouis.FindAll(p => p.UpdateState != UpdateStateConst.None))
            //{
            //    _santeiFinder.SinKouiReload(sinKoui.SinKoui);
            //}

            //foreach (SinKouiCountModel sinKouiCount in _sinKouiCounts.FindAll(p => p.UpdateState != UpdateStateConst.None))
            //{
            //    _santeiFinder.SinKouiCountReload(sinKouiCount.SinKouiCount);
            //}

            //foreach (SinKouiDetailModel sinKouiDetail in _sinKouiDetails.FindAll(p => p.UpdateState != UpdateStateConst.None))
            //{
            //    _santeiFinder.SinKouiDetailReload(sinKouiDetail.SinKouiDetail);
            //}

            foreach (SinRpInfModel sinRpInf in _sinRpInfs)
            {
                _santeiFinder.SinRpInfReload(sinRpInf.SinRpInf);
            }

            //foreach (SinRpNoInfModel sinRpNoInf in _sinRpNoInfs)
            //{
            //    _santeiFinder.SinRpNoInfReload(sinRpNoInf.SinRpNoInf);
            //}

            foreach (SinKouiModel sinKoui in _sinKouis)
            {
                _santeiFinder.SinKouiReload(sinKoui.SinKoui);
            }

            foreach (SinKouiCountModel sinKouiCount in _sinKouiCounts)
            {
                _santeiFinder.SinKouiCountReload(sinKouiCount.SinKouiCount);
            }

            foreach (SinKouiDetailModel sinKouiDetail in _sinKouiDetails)
            {
                _santeiFinder.SinKouiDetailReload(sinKouiDetail.SinKouiDetail);
            }
        }
    }
}
