using Helper.Common;
using Reporting.CommonMasters.Enums;
using Reporting.Karte3.DB;
using Reporting.Karte3.Model;
using static Reporting.Karte3.Enum.CoKarte3Column;

namespace Reporting.Karte3.Service
{
    public class Karte3CoReportService : IKarte3CoReportService
    {
        #region Constant

        #endregion
        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private readonly ICoKarte3Finder _finder;
        /// <summary>
        /// CoReport Model
        /// </summary>
        private List<CoKarte3Model> coModels;
        private CoKarte3Model coModel;

        /// <summary>
        /// 出力モード
        /// 0:印刷
        /// 1:ファイル
        /// </summary>
        private CoOutputMode OutputMode;
        /// <summary>
        /// ファイルタイプ
        /// 0:バイナリ
        /// 1:PDF
        /// </summary>
        private CoFileType FileType;
        /// <summary>
        /// 出力先フォルダ
        /// </summary>
        private string OutputDirectory;
        /// <summary>
        /// ファイル名
        /// </summary>
        private string OutputFileName;
        /// <summary>
        /// プリンタ名
        /// </summary>
        private string PrinterName;

        /// <summary>
        /// 印刷項目情報
        /// </summary>
        private List<CoKarte3PrintDataModel> printOutData;

        private int _dataRowCount;
        private DateTime _printoutDateTime;

        #endregion

        #region Constructor and Init
        public Karte3CoReportService(ICoKarte3Finder finder)
        {
            _finder = finder;
        }
        #endregion

        #region Override method
        #endregion

        #region Init properties
        private int _hpId;
        private long _ptId;
        private int _startSinYm;
        private int _endSinYm;
        private bool _includeHoken;
        private bool _includeJihi;

        private bool _hasNextPage;
        private int _currentPage;

        public void InitParam(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi)
        {
            _hpId = hpId;
            _ptId = ptId;
            _startSinYm = startSinYm;
            _endSinYm = endSinYm;
            _includeHoken = includeHoken;
            _includeJihi = includeJihi;
        }
        #endregion

        #region Private function
        /// <summary>
        /// データリスト生成
        /// </summary>
        private void MakePrintDataList()
        {

            List<CoKarte3DailyDataModel> dailyDatas = new List<CoKarte3DailyDataModel>();

            int sinDate = 0;

            CoKarte3DailyDataModel addDailyData = null;

            foreach (CoSinKouiModel sinKoui in coModel.SinKouis.FindAll(p => p.TotalTen != 0))
            {
                if (sinDate != sinKoui.SinDate)
                {
                    sinDate = sinKoui.SinDate;

                    if (addDailyData != null && addDailyData.MaxCount > 0)
                    {
                        dailyDatas.Add(addDailyData);
                    }
                    addDailyData = new CoKarte3DailyDataModel();
                    addDailyData.Date = sinKoui.SinDate;
                }

                if (sinKoui.SanteiKbn == 2 || new List<string> { "JS", "SZ" }.Contains(sinKoui.CdKbn) || sinKoui.SinId == 96)
                {
                    // 自費
                    addDailyData[Karte3Column.HokenGai].Add(sinKoui.TotalTen);
                    addDailyData.GetEnTenList(Karte3Column.HokenGai).Add(sinKoui.EntenKbn);
                }
                else if (sinKoui.CdKbn == "A")
                {
                    // 初再診
                    addDailyData[Karte3Column.Sinsatu].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Sinsatu).Add(sinKoui.EntenKbn);
                }
                else if (sinKoui.CdKbn == "C")
                {
                    // 在宅
                    addDailyData[Karte3Column.Zaitaku].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Zaitaku).Add(sinKoui.EntenKbn);
                }
                else if (sinKoui.CdKbn == "B")
                {
                    // 医学管理
                    addDailyData[Karte3Column.IgakuKanri].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.IgakuKanri).Add(sinKoui.EntenKbn);
                }
                else if (sinKoui.CdKbn == "F")
                {
                    // 投薬
                    addDailyData[Karte3Column.Toyaku].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Toyaku).Add(sinKoui.EntenKbn);
                }
                else if (sinKoui.CdKbn == "G")
                {
                    // 注射
                    addDailyData[Karte3Column.Chusya].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Chusya).Add(sinKoui.EntenKbn);
                }
                else if (sinKoui.CdKbn == "J")
                {
                    // 処置
                    addDailyData[Karte3Column.Shoti].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Shoti).Add(sinKoui.EntenKbn);
                }
                else if (new List<string> { "K", "L" }.Contains(sinKoui.CdKbn))
                {
                    // 手術・麻酔
                    addDailyData[Karte3Column.Ope].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Ope).Add(sinKoui.EntenKbn);
                }
                else if (new List<string> { "D", "N" }.Contains(sinKoui.CdKbn))
                {
                    // 検査・病理
                    addDailyData[Karte3Column.Kensa].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Kensa).Add(sinKoui.EntenKbn);
                }
                else if (new List<string> { "E" }.Contains(sinKoui.CdKbn))
                {
                    // 画像
                    addDailyData[Karte3Column.Gazo].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Gazo).Add(sinKoui.EntenKbn);
                }
                else if (new List<string> { "H", "I", "M", "R" }.Contains(sinKoui.CdKbn))
                {
                    // リハビリ・精神・放射・労災
                    addDailyData[Karte3Column.Sonota].Add(sinKoui.TotalTenDsp);
                    addDailyData.GetEnTenList(Karte3Column.Sonota).Add(sinKoui.EntenKbn);
                }
            }

            if (addDailyData != null && addDailyData.MaxCount > 0)
            {
                dailyDatas.Add(addDailyData);
            }

            printOutData = new List<CoKarte3PrintDataModel>();

            // 各列の合計を記録するリスト
            List<double> gokeiList = new List<double>();

            for (int i = 0; i < System.Enum.GetNames(typeof(Karte3Column)).Length; i++)
            {
                gokeiList.Add(0);
            }

            // 合計の合計
            int gokeiTensu = 0;
            int gokeiFutan = 0;

            int sinYm = 0;

            CoKarte3PrintDataModel addData = new CoKarte3PrintDataModel();

            foreach (CoKarte3DailyDataModel dailyData in dailyDatas)
            {
                addData = new CoKarte3PrintDataModel();

                if (sinYm == 0)
                {
                    sinYm = dailyData.Date / 100;
                }
                else if (sinYm != dailyData.Date / 100)
                {
                    sinYm = dailyData.Date / 100;

                    // 月合計を求める
                    addData = new CoKarte3PrintDataModel();
                    addData.DataType = 1;
                    addData.Date = "合計";

                    addData.GokeiTensu = gokeiTensu;
                    addData.GokeiFutan = gokeiFutan;

                    foreach (Karte3Column Col in System.Enum.GetValues(typeof(Karte3Column)))
                    {
                        addData[Col] = gokeiList[(int)Col];

                        gokeiList[(int)Col] = 0;//初期化
                    }

                    // 初期化
                    gokeiTensu = 0;
                    gokeiFutan = 0;

                    printOutData.Add(addData);

                }

                sinDate = 0;

                for (int i = 0; i < dailyData.MaxCount; i++)
                {
                    addData = new CoKarte3PrintDataModel();

                    if (i == 0)
                    {
                        // 日の先頭

                        addData.DataType = 0;

                        if (sinDate != dailyData.Date || (printOutData.Count() % _dataRowCount == 0))
                        {
                            // 日付が変わった場合、日付をセットする
                            addData.Date = $"{(dailyData.Date % 10000 / 100)}/{dailyData.Date % 100}";
                            sinDate = dailyData.Date;
                        }
                        addData.GokeiTensu = coModel.GetTotalTensu(dailyData.Date);
                        addData.GokeiFutan = coModel.GetTotalPtFutan(dailyData.Date);

                        gokeiTensu += addData.GokeiTensu;
                        gokeiFutan += addData.GokeiFutan;
                    }
                    else if (printOutData.Count() % _dataRowCount == 0)
                    {
                        // ページ先頭行の場合、日付をセットする
                        addData.Date = $"{(dailyData.Date % 10000 / 100)}/{dailyData.Date % 100}";
                    }

                    foreach (Karte3Column Col in System.Enum.GetValues(typeof(Karte3Column)))
                    {
                        if (i < dailyData.GetList(Col).Count())
                        {
                            addData[Col] = dailyData.GetList(Col)[i];
                            gokeiList[(int)Col] += addData[Col];
                        }
                    }

                    printOutData.Add(addData);
                }
            }

            // 最後の１回
            // 月合計を求める
            addData = new CoKarte3PrintDataModel();
            addData.DataType = 1;
            addData.Date = "合計";

            addData.GokeiTensu = gokeiTensu;
            addData.GokeiFutan = gokeiFutan;

            foreach (Karte3Column Col in System.Enum.GetValues(typeof(Karte3Column)))
            {
                addData[Col] = gokeiList[(int)Col];

                gokeiList[(int)Col] = 0;//初期化
            }
            printOutData.Add(addData);
        }

        /// <summary>
        /// 実際の印字処理
        /// </summary>
        /// <param name="hasNextPage"></param>
        /// <returns></returns>
        private bool UpdateDrawForm()
        {
            _hasNextPage = true;
            #region SubMethod

            // ヘッダー
            int UpdateFormHeader()
            {
                #region sub method
                string _getWarekiYm(int ym)
                {
                    string ret = "";
                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(ym * 100 + 1);
                    ret = $"{wareki.Gengo} {wareki.Year}年{wareki.Month}月";

                    return ret;
                }
                #endregion

                // 発行日時
                CoRep.SetFieldData("dfPrintDateTime", _printoutDateTime.ToString("yyyy/MM/dd HH:mm"));

                // ページ
                CoRep.SetFieldData("dfPage", _currentPage);

                // 患者番号
                CoRep.SetFieldData("dfPtNum", coModel.PtNum);
                // 患者氏名
                CoRep.SetFieldData("dfPtName", coModel.PtName);

                // 性別
                CoRep.SetFieldData("dfSex", coModel.PtSex);

                // 生年月日
                CoRep.SetFieldData("dfBirthDay", CIUtil.SDateToShowWDate3(coModel.BirthDay).Ymd);

                // 保険の種類
                CoRep.SetFieldData("dfHokenSbt", coModel.HokenSyu);

                // 集計期間
                CoRep.SetFieldData("dfSyukeiStartDate", _getWarekiYm(_startSinYm));
                CoRep.SetFieldData("dfSyukeiEndDate", _getWarekiYm(_endSinYm));

                return 1;
            }

            // 本体
            int UpdateFormBody()
            {
                int dataIndex = (_currentPage - 1) * _dataRowCount;

                if (printOutData == null || printOutData.Count == 0)
                {
                    _hasNextPage = false;
                    return dataIndex;
                }

                for (short i = 0; i < _dataRowCount; i++)
                {
                    CoRep.ListText("lsData", 0, i, printOutData[dataIndex].Date);
                    CoRep.ListText("lsData", 1, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Sinsatu], "#,0"));
                    CoRep.ListText("lsData", 2, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Zaitaku], "#,0"));
                    CoRep.ListText("lsData", 3, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.IgakuKanri], "#,0"));
                    CoRep.ListText("lsData", 4, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Toyaku], "#,0"));
                    CoRep.ListText("lsData", 5, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Chusya], "#,0"));
                    CoRep.ListText("lsData", 6, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Shoti] + printOutData[dataIndex][Karte3Column.Ope], "#,0"));
                    CoRep.ListText("lsData", 7, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Kensa], "#,0"));
                    CoRep.ListText("lsData", 8, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Gazo], "#,0"));
                    CoRep.ListText("lsData", 9, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.Sonota], "#,0"));

                    CoRep.ListText("lsData", 10, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex].GokeiTensu, "#,0"));
                    CoRep.ListText("lsData", 11, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex].GokeiFutan, "#,0"));

                    CoRep.ListText("lsData", 12, i, CIUtil.ToStringIgnoreZero(printOutData[dataIndex][Karte3Column.HokenGai], "#,0"));

                    #region セル装飾
                    // 行の四方位置を取得する
                    (long startX, long startY, long endX, long endY) = CoRep.GetListRowBounds("lsData", i);

                    // 塗りつぶしをクリアする
                    for (short j = 0; j < CoRep.GetListColCount("lsData"); j++)
                    {
                        CoRep.ListFillPattern("lsData", j, i, Hos.CnDraw.Constants.ConFillPattern.None);
                    }

                    if (printOutData[dataIndex].DataType == 1)
                    {
                        // 合計行の処理

                        // 上に線を引く（ただし、先頭行の場合は引かない）
                        if (i != 0)
                        {
                            CoRep.DrawLine(startX, startY, endX, startY, 30);
                        }
                        // 下に線を引く（ただし、最終行の場合は引かない）
                        if (i < _dataRowCount - 1)
                        {
                            CoRep.DrawLine(startX, endY, endX, endY, 30);
                        }

                        // 塗りつぶし
                        for (short j = 0; j < CoRep.GetListColCount("lsData"); j++)
                        {
                            CoRep.ListFillPattern("lsData", j, i, Hos.CnDraw.Constants.ConFillPattern.Pattern6);
                            CoRep.ListBackColor("lsData", j, i, System.Drawing.Color.LightGray);
                        }
                    }
                    else if (!(string.IsNullOrEmpty(printOutData[dataIndex].Date)))
                    {
                        // 日付のある行の場合、上に線を引く（ただし、先頭行の場合は引かない）
                        if (i != 0)
                        {
                            CoRep.DrawLine(startX, startY, endX, startY);
                        }
                    }
                    else
                    {
                        // 日付のない行の場合、2列目(index=1)から横線を引く
                        (long firstStartX, long firstStartY, long firstEndX, long firstEndY) = CoRep.GetListCellBounds("lsData", 1, i);

                        CoRep.DrawLine(firstStartX, startY, endX, startY, style: Hos.CnDraw.Constants.ConLineStyle.Dot);
                    }
                    #endregion


                    dataIndex++;
                    if (dataIndex >= printOutData.Count)
                    {
                        _hasNextPage = false;
                        break;
                    }
                }

                return dataIndex;

            }

            #endregion

            try
            {
                if (UpdateFormHeader() < 0 || UpdateFormBody() < 0)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// データ取得
        /// </summary>
        /// <returns></returns>
        private List<CoKarte3Model> GetData()
        {
            // 患者情報
            CoPtInfModel ptInf = _finder.FindPtInf(_hpId, _ptId);

            // 診療行為
            List<CoSinKouiModel> sinKouis = _finder.FindSinKoui(_hpId, _ptId, _startSinYm, _endSinYm, _includeHoken, _includeJihi);

            List<int> hokenIds = sinKouis.GroupBy(p => p.HokenId).Select(p => p.Key).ToList();

            List<CoKarte3Model> retDatas = new List<CoKarte3Model>();

            //foreach (int hokenId in hokenIds)
            //{
            //    // 患者保険
            //    CoPtHokenInfModel ptHoken = finder.FindPtHoken(PtId, hokenId, EndSinYm * 100 + 31);

            //    if (IncludeJihi == false && ptHoken.HokenSbtKbn == 8)
            //    {
            //        // 自費を除く設定の場合、自費保険は無視
            //    }
            //    else if (IncludeHoken == false && ptHoken.HokenSbtKbn != 8)
            //    {
            //        // 保険を除く設定の場合、自費保険以外は無視
            //    }
            //    else
            //    {
            //        List<CoKaikeiInfModel> kaikeiInfs = finder.FindKaikeiInf(PtId, StartSinYm, EndSinYm, hokenId);
            //        HashSet<string> houbetuNos = finder.FindKohiInf(PtId, StartSinYm, EndSinYm, hokenId);

            //        List<CoSinKouiModel> filteredSinKouis = sinKouis.FindAll(p => p.HokenId == hokenId);

            //        retDatas.Add(new CoKarte3Model(ptInf, ptHoken, filteredSinKouis, kaikeiInfs, houbetuNos));
            //    }
            //}

            List<int> targetHokenIds = new List<int>();
            foreach (int hokenId in hokenIds)
            {
                // 患者保険
                CoPtHokenInfModel ptHoken = _finder.FindPtHoken(_hpId, _ptId, hokenId, _endSinYm * 100 + 31);

                if (_includeJihi == false && ptHoken.HokenSbtKbn == 8)
                {
                    // 自費を除く設定の場合、自費保険は無視
                }
                else if (_includeHoken == false && ptHoken.HokenSbtKbn != 8)
                {
                    // 保険を除く設定の場合、自費保険以外は無視
                }
                else
                {
                    targetHokenIds.Add(hokenId);
                }
            }

            List<CoKaikeiInfModel> kaikeiInfs = _finder.FindKaikeiInf(_hpId, _ptId, _startSinYm, _endSinYm, targetHokenIds);
            //HashSet<string> houbetuNos = finder.FindKohiInf(PtId, StartSinYm, EndSinYm, hokenId);
            HashSet<string> houbetuNos = new HashSet<string>();

            List<CoSinKouiModel> filteredSinKouis =
                sinKouis.FindAll(p => targetHokenIds.Contains(p.HokenId))
                .OrderBy(p => p.SinDate)
                .ThenBy(p => p.HokenId)
                .ThenBy(p => p.SanteiKbn)
                .ThenBy(p => p.CdKbn)
                .ThenBy(p => p.CdNo)
                .ThenBy(p => p.RpNo)
                .ThenBy(p => p.SeqNo)
                .ToList();

            retDatas.Add(new CoKarte3Model(ptInf, null, filteredSinKouis, kaikeiInfs, houbetuNos));

            return retDatas;
        }

        #endregion
    }
}
