using Helper.Common;
using Helper.Extension;
using Reporting.CommonMasters.Config;
using Reporting.Kensalrai.DB;
using Reporting.Kensalrai.Mapper;
using Reporting.Kensalrai.Model;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using System.Text;

namespace Reporting.Kensalrai.Service
{
    public class KensaIraiCoReportService : IKensaIraiCoReportService
    {

        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _extralData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();

        #region Init properties
        private int HpId;
        private List<string> _objectRseList;
        private List<CoKensaIraiPrintDataModel> printOutData;
        List<KensaIraiModel> KensaIrais = new List<KensaIraiModel>();
        string centerCd;
        string centerName;
        int IraiDate;
        int startDate;
        int endDate;
        int _currentPage;
        bool _hasNextPage;
        bool printYoki = false;
        private int _dataColCount;
        private int _dataRowCount;
        private string _rowCountFieldName = "lsSinDate";
        private DateTime _printoutDateTime = CIUtil.GetJapanDateTimeNow();
        private readonly ISystemConfig _systemConfig;

        private readonly ICoKensaIraiFinder _finder;
        private readonly IReadRseReportFileService _readRseReportFileService;

        public KensaIraiCoReportService(ICoKensaIraiFinder finder, IReadRseReportFileService readRseReportFileService, ISystemConfig systemConfig)
        {
            _finder = finder;
            _readRseReportFileService = readRseReportFileService;
            _systemConfig = systemConfig;
        }
        #endregion

        private void GetKensaIrais()
        {
            var listKensaIraiInf = _finder.GetKensaInfModelsPrint(HpId, startDate, endDate, centerCd);
            if (listKensaIraiInf != null && listKensaIraiInf.Count > 0)
            {
                KensaIrais = _finder.GetKensaIraiModelsForPrint(HpId, listKensaIraiInf);
            }
        }

        public CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd)
        {
            try
            {
                HpId = hpId;
                IraiDate = systemDate;
                startDate = fromDate;
                endDate = toDate;
                this.centerCd = centerCd;
                // get data to print
                GetFieldNameList();
                GetRowCount();
                GetKensaIrais();

                if (!KensaIrais.Any())
                {
                    return new();
                }

                GetData();

                _hasNextPage = true;

                _currentPage = 1;
                _dataColCount = 1;

                while (_objectRseList.Contains($"lsKensaItemCd{_dataColCount + 1}"))
                {
                    _dataColCount++;
                }

                MakePrintDataList();

                //印刷
                while (_hasNextPage)
                {
                    UpdateDrawForm();
                    _currentPage++;
                }

                return new KensalraiMapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
            }
            finally
            {
                _finder.ReleaseResource();
                _systemConfig.ReleaseResource();
            }
        }

        public CommonReportingRequestModel GetKensalraiData(int hpId, int systemDate, int fromDate, int toDate, string centerCd, List<KensaIraiModel> kensaIrais)
        {
            HpId = hpId;
            IraiDate = systemDate;
            startDate = fromDate;
            endDate = toDate;
            KensaIrais = kensaIrais;
            this.centerCd = centerCd;
            // get data to print
            GetFieldNameList();
            GetRowCount();

            if (!KensaIrais.Any())
            {
                return new();
            }

            GetData();

            _hasNextPage = true;

            _currentPage = 1;
            _dataColCount = 1;

            while (_objectRseList.Contains($"lsKensaItemCd{_dataColCount + 1}"))
            {
                _dataColCount++;
            }

            MakePrintDataList();

            //印刷
            while (_hasNextPage)
            {
                UpdateDrawForm();
                _currentPage++;
            }

            return new KensalraiMapper(_singleFieldData, _tableFieldData, _extralData, _rowCountFieldName).GetData();
        }


        #region Public function
        /// <summary>
        /// 検査依頼ファイル用文字列を取得する
        /// </summary>
        /// <param name="CenterCd">センターコード</param>
        /// <param name="kensaIrais">検査依頼データ</param>
        /// <param name="fileType">0-標準、1-加古川</param>
        /// <returns></returns>
        public List<string> GetIraiFileData(string CenterCd, List<KensaIraiModel> kensaIrais, int fileType = 0)
        {
            if (new int[] { 2 }.Contains(fileType))
            {
                // 福山臨床
                return GetIraiFileDataFukuyama(CenterCd, kensaIrais);
            }
            else
            {
                // 標準
                return GetIraiFileDataStandard(CenterCd, kensaIrais, fileType);
            }
        }
        public List<string> GetIraiFileDataDummy(string CenterCd, List<KensaIraiModel> kensaIrais, int fileType = 0)
        {
            if (new int[] { 2 }.Contains(fileType))
            {
                // 福山臨床
                return GetIraiFileDataFukuyama(CenterCd, kensaIrais, true);
            }
            else
            {
                // 標準
                return GetIraiFileDataStandard(CenterCd, kensaIrais, fileType, true);
            }
        }
        public List<string> GetIraiFileDataStandard(string CenterCd, List<KensaIraiModel> kensaIrais, int fileType = 0, bool dummy = false)
        {
            const int RightJustification = 0;
            const int LeftJustification = 1;

            #region local method
            // 文字列を指定の長さに調整する（文字カット、スペース埋め）
            string adjStr(string str, int length, int justification = LeftJustification)
            {
                //string result = CIUtil.Copy(str, 1, length);

                if (string.IsNullOrEmpty(str))
                {
                    str = "";
                }

                string result = str;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding enc = Encoding.GetEncoding("Shift_JIS");
                Byte[] b = enc.GetBytes(str);

                if (enc.GetByteCount(str) > length)
                {
                    //バイト数で文字カット
                    result = enc.GetString(b, 0, length);

                    if (enc.GetByteCount(str) >= length + 1)
                    {
                        //区切りが全角文字の途中の場合、１文字多くなってしまうのを防ぐ
                        string result2 = enc.GetString(b, 0, length + 1);
                        if (result.Length == result2.Length)
                        {
                            result = result.Remove(result.Length - 1);
                        }
                    }
                }

                //埋める文字数　=　最大バイト数　-　(対象文字列のバイト数 - 対象文字列の文字列数)
                int width = length - (enc.GetByteCount(result) - result.Length);
                if (justification == RightJustification)
                {
                    // 右寄せ（左にスペース）
                    result = result.PadLeft(width);
                }
                else
                {
                    // 左寄せ（右にスペース）
                    result = result.PadRight(width);
                }
                return result;
            }

            // 身長体重を出力用文字列に変換する
            string getHeightWeight(double val)
            {
                string retStr = "";
                if (val == 0)
                {
                    retStr = "00";
                }
                else
                {
                    val = Math.Round(val, 1, MidpointRounding.AwayFromZero) * 10;
                    retStr = val.ToString();
                }
                return adjStr(retStr, 4, RightJustification);
            }
            #endregion

            List<string> results = new List<string>();

            foreach (KensaIraiModel kensaIrai in kensaIrais)
            {
                #region O1レコード
                string o1 = "";

                // レコード区分   2桁
                o1 += "O1";
                // センターコード  6桁
                o1 += adjStr(CenterCd, 6);
                // 依頼コード    20桁              
                if (fileType == 3)
                {
                    // 岡山
                    o1 += adjStr(kensaIrai.SinDate.ToString() + kensaIrai.IraiCd.ToString(), 20);
                }
                else
                {
                    o1 += adjStr(kensaIrai.IraiCd.ToString(), 20);
                }
                // 科コード     15桁
                if (_systemConfig.OdrKensaIraiKaCode(HpId) == 1)
                {
                    string kacdname = kensaIrai.KaCodeName;

                    kacdname = CIUtil.CiCopyStrWidth(kacdname, 1, 15, 1);
                    o1 += kacdname;

                }
                else if (_systemConfig.OdrKensaIraiKaCode(HpId) == 2)
                {
                    string kacdname = "";

                    if (kensaIrai.KaId > 0)
                    {
                        kacdname = kensaIrai.KaId.ToString();
                    }

                    kacdname = kacdname.PadLeft(15, ' ');
                    o1 += kacdname;
                }
                else
                {
                    // 通常は未使用
                    o1 += new string(' ', 15);
                }
                // 病棟コード    15桁 ※未使用
                o1 += new string(' ', 15);
                // 入院外来区分   1桁  ※2固定
                o1 += "2";
                // 提出医      10桁 ※未使用
                if (new int[] { 1, 3 }.Contains(fileType))
                {
                    // 加古川
                    o1 += CIUtil.CiCopyStrWidth(kensaIrai.TantoKanaName, 1, 10, 1);
                }
                else
                {
                    o1 += new string(' ', 10);
                }
                // 被検者ＩＤ    15桁
                o1 += adjStr(kensaIrai.PtNum.ToString(), 15);
                // カルテＮＯ    15  ※未使用
                o1 += new string(' ', 15);
                // 被検者名     20桁
                o1 += adjStr(kensaIrai.KanaName, 20);
                // 性別       1桁  ※ M/F
                if (fileType == 1)
                {
                    // 加古川
                    o1 += adjStr(kensaIrai.GetSexStr("1", "2"), 1);
                }
                else
                {
                    o1 += adjStr(kensaIrai.GetSexStr("M", "F"), 1);
                }
                // 年齢区分     1桁  ※Y固定
                o1 += "Y";
                // 年齢       3桁
                o1 += adjStr(kensaIrai.Age.ToString(), 3, RightJustification);
                // 生年月日区分   1桁  ※スペース固定
                o1 += " ";
                // 年月日      6桁  ※YYMMDD（西暦）
                o1 += (kensaIrai.Birthday % 1000000).ToString().PadLeft(6, '0');
                // 採取日      6桁  ※YYMMDD（西暦）
                o1 += (kensaIrai.SinDate % 1000000).ToString().PadLeft(6, '0');
                // 採取時間     4桁  ※未使用
                o1 += new string(' ', 4);
                // 項目数      3桁  
                o1 += adjStr(kensaIrai.DetailCount.ToString(), 3, RightJustification);
                // 身長       4桁（前3桁整数部、後1桁小数点部）
                o1 += getHeightWeight(kensaIrai.Height);
                // 体重       4桁（前3桁整数部、後1桁小数点部）
                o1 += getHeightWeight(kensaIrai.Weight);
                // 尿量       4桁  ※未使用
                o1 += adjStr("0", 4, RightJustification);
                // 尿量単位     2桁  ※未使用
                o1 += new string(' ', 2);
                // 妊娠週数     2桁  ※未使用
                o1 += new string(' ', 2);
                // 透析前後     1桁  ※0はスペースで出力
                o1 += adjStr(CIUtil.ToStringIgnoreZero(kensaIrai.TosekiKbn), 1);
                // 至急報告     1桁
                if (fileType == 1)
                {
                    // 加古川
                    if (kensaIrai.SikyuKbn == 0)
                    {
                        o1 += adjStr(" ", 1);
                    }
                    else
                    {
                        o1 += adjStr(kensaIrai.SikyuKbn.ToString(), 1);
                    }
                }
                else
                {
                    o1 += adjStr(kensaIrai.SikyuKbn.ToString(), 1);
                }
                // 依頼コメント内容     20桁 ※未使用
                o1 += new string(' ', 20);
                // 空白       74桁 ※未使用
                o1 += new string(' ', 74);

                results.Add(o1);
                #endregion

                string o2 = "";
                int dtlCount = 0;
                foreach (KensaIraiDetailModel kensaDtl in kensaIrai.Details)
                {
                    dtlCount++;
                    if (dtlCount > 9)
                    {
                        o2 += new string(' ', 3);
                        dtlCount = 1;
                    }

                    if (dtlCount == 1)
                    {
                        if (!string.IsNullOrEmpty(o2))
                        {
                            results.Add(o2);
                            o2 = "";
                        }

                        // レコード区分   2桁
                        o2 += "O2";
                        // センターコード  6桁
                        o2 += adjStr(CenterCd, 6);
                        // 依頼コード    20桁             
                        if (fileType == 3)
                        {
                            // 岡山
                            o2 += adjStr(kensaIrai.SinDate.ToString() + kensaIrai.IraiCd.ToString(), 20);
                        }
                        else
                        {
                            o2 += adjStr(kensaIrai.IraiCd.ToString(), 20);
                        }
                    }

                    if (new int[] { 1, 3 }.Contains(fileType))
                    {
                        // 加古川
                        o2 += adjStr(kensaDtl.CenterItemCd, 25);
                    }
                    else
                    {
                        // 電子カルテ検査項目コード     15桁
                        o2 += adjStr(kensaDtl.KensaItemCd, 15);
                        // センター項目検査コード      10桁
                        o2 += adjStr(kensaDtl.CenterItemCd, 10);
                    }
                }

                if (dtlCount <= 9)
                {
                    // 不足分をスペース埋め
                    //o2 += new string(' ', 256 - o2.Length);
                    // 不足バイト数をスペース埋め
                    o2 = adjStr(o2, 256);
                    results.Add(o2);
                }
            }

            return results;
        }

        public List<string> GetIraiFileDataFukuyama(string CenterCd, List<KensaIraiModel> kensaIrais, bool dummy = false)
        {
            #region local method
            string _addQuote(string value, bool addComma = true)
            {
                return $"\"{value}\"" + (addComma ? "," : "");
            }
            #endregion

            List<string> results = new List<string>();

            foreach (KensaIraiModel kensaIrai in kensaIrais)
            {
                string head = string.Empty;

                #region head
                // 1   センターコード
                head += _addQuote(CenterCd);
                // 2   依頼者ＫＥＹ
                head += _addQuote(kensaIrai.IraiCd.ToString());
                // 3   予備１
                head += _addQuote("");
                // 4   予備２
                head += _addQuote("");
                // 5   科コード・科名
                head += _addQuote(kensaIrai.KaName);
                // 6   病棟コード病棟名
                head += _addQuote("");
                // 7   入院外来区分
                head += _addQuote("1");
                // 8   提出医
                head += _addQuote(kensaIrai.DrName);
                // 9   被検者ＩＤ
                head += _addQuote(kensaIrai.PtNum.ToString());
                //10  カルテＮＯ
                head += _addQuote(kensaIrai.PtNum.ToString());
                //11  カルテＮＯ区分
                head += _addQuote("");
                //12  被検者名カナ
                head += _addQuote(kensaIrai.KanaName);
                //13  被験者名漢字
                head += _addQuote(kensaIrai.Name);
                //14  性別
                head += _addQuote(kensaIrai.Sex.ToString());
                //15  年齢区分
                head += _addQuote("");
                //16  年齢
                head += _addQuote(kensaIrai.Age.ToString());
                //17  生年月日区分
                head += _addQuote("");
                //18  生年月日
                head += _addQuote(kensaIrai.Birthday.ToString());
                //19  採取日
                head += _addQuote((kensaIrai.SinDate % 1000000).ToString());
                if (dummy == false)
                {
                    //20  採取時間
                    head += _addQuote(kensaIrai.UpdateTime.PadLeft(4, '0'));
                }
                //21  保険情報
                head += _addQuote("");
                //22  身長
                head += _addQuote("");
                //23  体重
                head += _addQuote("");
                //24  尿量
                head += _addQuote("");
                //25  尿量単位
                head += _addQuote("");
                //26  妊娠週数
                head += _addQuote("");
                //27  透析前後
                head += _addQuote("");
                //28  至急報告
                if (kensaIrai.SikyuKbn == 0)
                {
                    head += _addQuote("");
                }
                else
                {
                    head += _addQuote("1");
                }
                //29  依頼コメント１
                head += _addQuote("");
                //30  依頼コメント２
                head += _addQuote("");
                //31  依頼コメント３
                head += _addQuote("");
                //32  検体材料
                head += _addQuote("");
                //33  報告材料
                head += _addQuote("");
                //34  細菌材料
                head += _addQuote("");
                //35  予備３
                head += _addQuote("");
                //36  予備４
                head += _addQuote("");
                //37  予備５
                head += _addQuote("");
                //38  予備６
                head += _addQuote("");
                //39  予備７
                head += _addQuote("");
                //40  予備８
                head += _addQuote("");
                #endregion

                //41~項目コード＆項目名称
                //※41以降を繰り返す(最大50項目)
                //※最後に改行コードをセットすること
                int dtlCount = 0;
                string dtl = "";

                foreach (KensaIraiDetailModel kensaDtl in kensaIrai.Details)
                {
                    dtlCount++;
                    if (dtlCount > 50)
                    {
                        dtlCount = 1;
                    }

                    if (dtlCount == 1)
                    {
                        if (string.IsNullOrEmpty(dtl) == false)
                        {
                            dtl = dtl.TrimEnd(',');
                            results.Add(head + dtl);
                            dtl = "";
                        }
                    }

                    dtl += _addQuote($"{kensaDtl.KensaItemCd}={kensaDtl.KensaName.TrimEnd(new char[] { ' ', '　' })}");
                }

                if (string.IsNullOrEmpty(dtl) == false)
                {
                    dtl = dtl.TrimEnd(',');
                    results.Add(head + dtl);
                    dtl = "";
                }
            }

            return results;
        }
        #endregion

        private void GetData()
        {
            // センター名
            centerName = _finder.GetCenterName(HpId, centerCd);

            // 来院情報を取得
            List<CoRaiinInfModel> raiinInfs = _finder.GetRaiinInf(HpId, KensaIrais.Select(p => p.RaiinNo).ToList());

            foreach (KensaIraiModel kensaIrai in KensaIrais)
            {
                if (raiinInfs.Any(p => p.RaiinNo == kensaIrai.RaiinNo))
                {
                    CoRaiinInfModel raiinInf = raiinInfs.Find(p => p.RaiinNo == kensaIrai.RaiinNo);
                    kensaIrai.KaName = raiinInf.KaName;
                    kensaIrai.TantoName = raiinInf.TantoName;
                    kensaIrai.TantoKanaName = raiinInf.TantoKanaName;
                    kensaIrai.DrName = raiinInf.DrName;

                    // 身長体重
                    (kensaIrai.Height, kensaIrai.Weight) = _finder.GetHeightWeight(HpId, kensaIrai.PtId, kensaIrai.SinDate);

                    foreach (KensaIraiDetailModel kensaDtl in kensaIrai.Details)
                    {
                        if (kensaDtl.ContainerCd > 0)
                        {
                            kensaDtl.ContainerName = _finder.GetContainerName(HpId, kensaDtl.ContainerCd);
                        }
                    }
                }
            }
        }

        private void MakePrintDataList()
        {
            int seqNo = 0;
            CoKensaIraiPrintDataModel addData;
            printOutData = new List<CoKensaIraiPrintDataModel>();

            foreach (KensaIraiModel kensaIrai in KensaIrais)
            {
                seqNo++;
                addData = new CoKensaIraiPrintDataModel();

                addData.SinDate = kensaIrai.SinDate;
                addData.IraiCd = kensaIrai.IraiCd;
                addData.RaiinNo = kensaIrai.RaiinNo;
                addData.PtNum = kensaIrai.PtNum;
                addData.PtName = kensaIrai.KanaName;
                addData.Age = kensaIrai.Age;
                addData.Sex = kensaIrai.GetSexStr("男", "女");
                addData.Sikyu = kensaIrai.SikyuStr;
                addData.Toseki = kensaIrai.TosekiStr;
                addData.KaName = kensaIrai.KaName;
                addData.TantoName = kensaIrai.TantoName;
                addData.TantoKanaName = kensaIrai.TantoKanaName;
                if (kensaIrai.Height > 0)
                {
                    addData.Height = Math.Round(kensaIrai.Height, 1, MidpointRounding.AwayFromZero).ToString();
                }
                if (kensaIrai.Weight > 0)
                {
                    addData.Weight = Math.Round(kensaIrai.Weight, 1, MidpointRounding.AwayFromZero).ToString();
                }

                printOutData.Add(addData);

                // detail
                addData = new CoKensaIraiPrintDataModel();
                addData.SeqNo = seqNo;

                int dtlCount = 0;
                int dtlSeqNo = 0;
                List<KensaIraiDetailModel> details = kensaIrai.Details;

                if (printYoki)
                {
                    // 容器名を印字する場合、容器名順にソートしておく
                    details =
                        details.OrderByDescending(p => !string.IsNullOrEmpty(p.ContainerName))
                            .ThenBy(p => p.ContainerName)
                            .ThenBy(p => p.SeqNo)
                            .ToList();
                }

                foreach (KensaIraiDetailModel kensaIraiDtl in details)
                {
                    dtlCount++;

                    if (dtlCount > _dataColCount)
                    {
                        printOutData.Add(addData);
                        addData = new CoKensaIraiPrintDataModel();

                        dtlCount = 1;
                    }

                    addData.ItemDatas.Add(
                        (
                            kensaIraiDtl.KensaItemCd,
                            kensaIraiDtl.CenterItemCd,
                            kensaIraiDtl.KensaKana,
                            kensaIraiDtl.ContainerName
                        )
                    );

                }
                printOutData.Add(addData);
            }
        }

        private bool UpdateDrawForm()
        {
            _hasNextPage = true;
            #region SubMethod

            // ヘッダー
            int UpdateFormHeader()
            {
                // 発行日時
                SetFieldData("dfPrintDateTime", _printoutDateTime.ToString("yyyy/MM/dd HH:mm"));

                // ページ
                SetFieldData("dfPage", _currentPage.ToString());

                // 開始日
                SetFieldData("dfStartDate", CIUtil.SDateToShowSDate3(startDate));

                // 終了日
                SetFieldData("dfEndDate", CIUtil.SDateToShowSDate3(endDate));

                // センター名
                SetFieldData("dfCenterName", centerName);

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
                    Dictionary<string, CellModel> data = new();

                    if (printOutData[dataIndex].SinDate > 0)
                    {
                        AddListData(ref data, "lsSinDate", CIUtil.SDateToShowSDate3(printOutData[dataIndex].SinDate));
                        AddListData(ref data, "lsIraiCd", printOutData[dataIndex].IraiCd.AsString());
                        AddListData(ref data, "lsRaiinNo", printOutData[dataIndex].RaiinNo.AsString());
                        AddListData(ref data, "lsPtNum", printOutData[dataIndex].PtNum.AsString());
                        AddListData(ref data, "lsPtName", printOutData[dataIndex].PtName);
                        AddListData(ref data, "lsAge", printOutData[dataIndex].Age.AsString());
                        AddListData(ref data, "lsSex", printOutData[dataIndex].Sex);
                        AddListData(ref data, "lsSikyu", printOutData[dataIndex].Sikyu);
                        AddListData(ref data, "lsToseki", printOutData[dataIndex].Toseki);
                        AddListData(ref data, "lsKaName", printOutData[dataIndex].KaName);
                        AddListData(ref data, "lsTantoName", printOutData[dataIndex].TantoName);
                        AddListData(ref data, "lsHeight", printOutData[dataIndex].Height);
                        AddListData(ref data, "lsWeight", printOutData[dataIndex].Weight);

                        #region セル装飾
                        if (i > 0)
                        {
                            // 行の四方位置を取得する

                            string rowNoKey = (i - 1) + "_" + _currentPage;
                            _extralData.Add("baseListName_" + rowNoKey, "lsLine");
                            _extralData.Add("rowNo_" + rowNoKey, (i - 1).ToString());
                        }
                        #endregion
                    }
                    else
                    {
                        AddListData(ref data, "lsSeqNo", CIUtil.ToStringIgnoreZero(printOutData[dataIndex].SeqNo));
                        for (int j = 1; j <= _dataColCount && j <= printOutData[dataIndex].ItemDatas.Count(); j++)
                        {
                            AddListData(ref data, $"lsKensaItemCd{j}", printOutData[dataIndex].ItemDatas[j - 1].kensaItemCd);
                            AddListData(ref data, $"lsCenterItemCd{j}", printOutData[dataIndex].ItemDatas[j - 1].CenterItemCd);
                            AddListData(ref data, $"lsKensaKana{j}", printOutData[dataIndex].ItemDatas[j - 1].KensaKanaName);
                            AddListData(ref data, $"lsYoki{j}", printOutData[dataIndex].ItemDatas[j - 1].Yoki);
                        }
                    }

                    _tableFieldData.Add(data);

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

        #region get data java

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void AddListData(ref Dictionary<string, CellModel> dictionary, string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !dictionary.ContainsKey(field))
            {
                dictionary.Add(field, new CellModel(value));
            }
        }

        private void GetFieldNameList()
        {
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.KensaIrai, "fmKensaIraiList.rse", new());
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _objectRseList = javaOutputData.objectNames;
        }

        private void GetRowCount()
        {
            List<ObjectCalculate> fieldInputList = new()
        {
            new ObjectCalculate("lsSinDate", (int)CalculateTypeEnum.GetListRowCount),
            new ObjectCalculate("lsYoki1", (int)CalculateTypeEnum.GetObjectVisible)
        };

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.KensaIrai, "fmKensaIraiList.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _dataRowCount = javaOutputData.responses?.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
            printYoki = javaOutputData.responses.FirstOrDefault(item => item.typeInt == (int)CalculateTypeEnum.GetObjectVisible)?.result == 1;
        }
        #endregion
    }
}
