using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;
using Reporting.SyojyoSyoki.DB;
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
        private int _maxRow;
        private string _rowCountFieldName = string.Empty;
        private List<string> _objectRseList = new();
        private bool _hasNextPage;
        private int _hpId;
        private long _ptId;
        private int _seiKyuYm;
        private int _sinYm;
        private int _hokenId;

        public SyojyoSyokiCoReportService(IReadRseReportFileService readRseReportFileService, ICoSyojyoSyokiFinder finder)
        {
            _readRseReportFileService = readRseReportFileService;
            _finder = finder;
        }

        private List<CoSyojyoSyokiModel> GetData()
        {
            return _finder.FindSyoukiInf(_hpId, _ptId, _seiKyuYm, _sinYm, _hokenId);
        }

        private bool UpdateDrawForm(out bool hasNextPage)
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
                SetFieldData("dfPage", _currentPage);
                // 患者番号
                SetFieldData("dfPtNo", coModel.PtNum);
                // 診療年月
                SetFieldData("dfSinYM", _getSinYm());
                // 県番号
                SetFieldData("dfPrefNo", coModel.PrefNo);
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
                    CoRep.ListText("lsSyojyoSyoki", 0, i, _syojyoSyokiList[dataIndex]);

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
                    hasNextPage = _hasNextPage;
                    return false;
                }
            }
            catch (Exception e)
            {
                hasNextPage = _hasNextPage;
                return false;
            }

            hasNextPage = _hasNextPage;
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

        private void GetFieldNameList()
        {
            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2011, "sta2011a.rse", new());
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _objectRseList = javaOutputData.objectNames;
        }

        private void GetRowCount()
        {
            _rowCountFieldName = putColumns.Find(p => _objectRseList.Contains(p.ColName)).ColName;
            List<ObjectCalculate> fieldInputList = new()
            {
                new ObjectCalculate(_rowCountFieldName, (int)CalculateTypeEnum.GetListRowCount)
            };

            CoCalculateRequestModel data = new CoCalculateRequestModel((int)CoReportType.Sta2011, "sta2011a.rse", fieldInputList);
            var javaOutputData = _readRseReportFileService.ReadFileRse(data);
            _maxRow = javaOutputData.responses?.FirstOrDefault(item => item.listName == _rowCountFieldName && item.typeInt == (int)CalculateTypeEnum.GetListRowCount)?.result ?? _maxRow;
        }
        #endregion
    }
}
