using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.SyojyoSyoki.DB;
using Reporting.SyojyoSyoki.Mapper;
using Reporting.SyojyoSyoki.Model;

namespace Reporting.SyojyoSyoki.Service
{
    public class SyojyoSyokiCoReportService : ISyojyoSyokiCoReportService
    {
        #region Constant
        #endregion

        #region Private properties

        /// <summary>
        /// Finder
        /// </summary>
        private ICoSyojyoSyokiFinder _finder;
        /// <summary>
        /// CoReport Model
        /// </summary>
        private CoSyojyoSyokiModel coModel;
        private List<CoSyojyoSyokiModel> coModels;
        #endregion

        private readonly IReadRseReportFileService _readRseReportFileService;
        private int _currentPage = 1;
        private bool _hasNextPage;
        private int _hpId;
        private long _ptId;
        private int _seiKyuYm;
        private int _sinYm;
        private int _hokenId;
        private int _syojyoSyokiRowCount;
        private int _syojyoSyokiCharCount;
        private readonly string _rowCountFieldName = "lsSyojyoSyoki";
        private List<string> _syojyoSyokiList;
        private readonly Dictionary<string, string> _singleFieldData = new Dictionary<string, string>();
        private readonly List<Dictionary<string, CellModel>> _tableFieldData = new List<Dictionary<string, CellModel>>();

        public SyojyoSyokiCoReportService(IReadRseReportFileService readRseReportFileService, ICoSyojyoSyokiFinder finder)
        {
            _readRseReportFileService = readRseReportFileService;
            _finder = finder;
        }

        public CommonReportingRequestModel GetSyojyoSyokiReportingData(int hpId, long ptId, int seiKyuYm, int hokenId)
        {
            try
            {
                _hpId = hpId;
                _ptId = ptId;
                _seiKyuYm = seiKyuYm;
                _hokenId = hokenId;
                GetRowCount();
                coModels = GetData();

                if (coModels != null && coModels.Any())
                {
                    foreach (CoSyojyoSyokiModel model in coModels)
                    {
                        coModel = model;

                        if (coModel != null && coModel.ReceInf != null)
                        {
                            _hasNextPage = true;
                            _currentPage = 1;

                            // 症状詳記リスト
                            _syojyoSyokiList = new List<string>();

                            MakeSyojyoSyokiList();

                            while (_hasNextPage)
                            {
                                _hasNextPage = UpdateDrawForm();
                                _currentPage++;
                            }

                        }
                    }
                }

                return new SyojyoSyokiMapper(_singleFieldData, _tableFieldData, _rowCountFieldName).GetData();
            }
            finally
            {
                _finder.ReleaseResource();
            }
        }

        private List<CoSyojyoSyokiModel> GetData()
        {
            try
            {

                return _finder.FindSyoukiInf(_hpId, _ptId, _seiKyuYm, _sinYm, _hokenId);
            }
            finally
            {
                _finder.ReleaseResource();
            }
        }

        private void MakeSyojyoSyokiList()
        {
            #region sub func
            // 症状詳記リストに文字列を折り返して追加する。
            void _addList(string str)
            {
                string line = str;
                while (line != "")
                {
                    string tmp = line;
                    if (CIUtil.LenB(line) > _syojyoSyokiCharCount)
                    {
                        tmp = CIUtil.CiCopyStrWidth(line, 1, _syojyoSyokiCharCount);
                    }
                    _syojyoSyokiList.Add(tmp);

                    line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));
                }
            }
            #endregion

            _syojyoSyokiList.Clear();

            foreach (CoSyoukiInfModel syoukiInf in coModel.SyoukiInfs)
            {
                if (_syojyoSyokiList.Any() && _syojyoSyokiList.Count() % _syojyoSyokiRowCount != 0)
                {
                    _syojyoSyokiList.Add("");
                }

                // 区分名
                _addList($"【{syoukiInf.KbnName}】");

                // 症状詳記
                string[] del = { "\r\n", "\r", "\n" };

                foreach (string addstr in syoukiInf.Syouki.Split(del, StringSplitOptions.None).ToList())
                {
                    _addList(addstr);
                }

            }
        }

        private bool UpdateDrawForm()
        {
            _hasNextPage = true;
            #region SubMethod

            // ヘッダー印刷
            int UpdateFormHeader()
            {
                #region sub func
                string _getSinYm()
                {
                    string ret = "";
                    CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(coModel.SinYm * 100 + 1);
                    ret = $"{wareki.Gengo} {wareki.Year,2}年{wareki.Month,2}月分";
                    return ret;
                }
                // レセ種別１
                string _getReceSbt1()
                {
                    string ret = "";

                    switch (CIUtil.Copy(coModel.ReceiptSbt, 2, 1))
                    {
                        case "1":
                            if (coModel.HokenKbn == 1)
                            {
                                ret = "1 社";
                            }
                            else
                            {
                                ret = "2 国";
                            }
                            break;
                        case "2":
                            ret = "2 公費";
                            break;
                        case "3":
                            ret = "3 後期";
                            break;
                        case "4":
                            ret = "4 退職";
                            break;
                    }
                    return ret;
                }

                // レセ種別２
                string _getReceSbt2()
                {
                    string ret = "";

                    switch (CIUtil.Copy(coModel.ReceiptSbt, 3, 1))
                    {
                        case "1":
                            ret = "1 単独";
                            break;
                        case "2":
                            ret = "2 ２併";
                            break;
                        case "3":
                            ret = "3 ３併";
                            break;
                        case "4":
                            ret = "4 ４併";
                            break;
                        case "5":
                            ret = "5 ５併";
                            break;
                    }
                    return ret;
                }

                // レセ種別３
                string _getReceSbt3()
                {
                    string ret = "";

                    switch (CIUtil.Copy(coModel.ReceiptSbt, 4, 1))
                    {
                        case "2":
                            ret = "2 本外";
                            break;
                        case "4":
                            ret = "4 六外";
                            break;
                        case "6":
                            ret = "6 家外";
                            break;
                        case "8":
                            ret = "8 高外一";
                            break;
                        case "0":
                            ret = "0 高外７";
                            break;
                    }
                    return ret;
                }
                #endregion

                // ページ
                SetFieldData("dfPage", _currentPage.ToString());
                // 患者番号
                SetFieldData("dfPtNo", coModel.PtNum.ToString());
                // 診療年月
                SetFieldData("dfSinYM", _getSinYm());
                // 県番号
                SetFieldData("dfPrefNo", coModel.PrefNo.ToString());
                // 医療機関コード
                SetFieldData("dfHpNo", CIUtil.FormatHpCd(coModel.HpCd, coModel.PrefNo));
                // レセ種別１
                SetFieldData("dfReceSbt1", _getReceSbt1());

                // レセ種別２
                SetFieldData("dfReceSbt2", _getReceSbt2());

                // レセ種別３
                SetFieldData("dfReceSbt3", _getReceSbt3());

                // 医療機関名
                SetFieldData("dfHpName", coModel.HpName);
                // 患者名
                SetFieldData("dfPtKanjiName", coModel.PtName);
                // 生年月日
                SetFieldData("dfBirthDay", CIUtil.SDateToShowWDate3(coModel.Birthday).Ymd);

                // 保険者番号
                SetFieldData("dfHokensyaNo", string.Format("{0, 8}", coModel.HokensyaNo));

                // 記号
                SetFieldData("dfKigo", coModel.Kigo);

                // 番号
                SetFieldData("dfBango", coModel.Bango);

                // 枝番
                SetFieldData("dfEdano", coModel.EdaNo);

                int fieldIndex = 1;
                for (int i = 1; i <= 4; i++)
                {
                    if (coModel.KohiReceKisai(i) == 1)
                    {
                        //公費負担者番号
                        SetFieldData($"dfFutanNoK{fieldIndex}", string.Format("{0, 8}", coModel.KohiFutansyaNo(i)));
                        //公費受給者番号
                        SetFieldData($"dfJyukyuNoK{fieldIndex}", string.Format("{0, 7}", coModel.KohiJyukyusyaNo(i)));

                        fieldIndex++;
                    }
                }

                return 1;
            }

            // 本体部印刷
            int UpdateFormBody()
            {
                int dataIndex = (_currentPage - 1) * _syojyoSyokiRowCount;

                if (_syojyoSyokiList == null || _syojyoSyokiList.Count == 0 || _syojyoSyokiRowCount <= 0)
                {
                    _hasNextPage = false;
                    return -1;
                }

                for (short i = 0; i < _syojyoSyokiRowCount; i++)
                {
                    Dictionary<string, CellModel> data = new();

                    AddListData(ref data, "lsSyojyoSyoki", _syojyoSyokiList[dataIndex]);

                    _tableFieldData.Add(data);

                    dataIndex++;
                    if (dataIndex >= _syojyoSyokiList.Count)
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

        #region get field java

        private void GetRowCount()
        {
            List<ObjectCalculate> fieldInputList = new();

            fieldInputList.Add(new ObjectCalculate("lsSyojyoSyoki", (int)CalculateTypeEnum.GetFormatLength));
            fieldInputList.Add(new ObjectCalculate("lsSyojyoSyoki", (int)CalculateTypeEnum.GetListRowCount));

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.SyojyoSyoki, "fmSyojyoSyoki.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _syojyoSyokiRowCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsSyojyoSyoki" && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? 0;
            _syojyoSyokiCharCount = javaOutputData.responses?.FirstOrDefault(item => item.listName == "lsSyojyoSyoki" && item.typeInt == (int)CalculateTypeEnum.GetFormatLength)?.result ?? 0;
        }
        #endregion
    }
}
