using Helper.Common;
using Helper.Extension;
using Reporting.Kensalrai.DB;
using Reporting.Kensalrai.Mapper;
using Reporting.Kensalrai.Model;
using Reporting.Mappers.Common;
using Reporting.ReadRseReportFile.Model;
using Reporting.ReadRseReportFile.Service;

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

        private readonly ICoKensaIraiFinder _finder;
        private readonly IReadRseReportFileService _readRseReportFileService;

        public KensaIraiCoReportService(ICoKensaIraiFinder finder, IReadRseReportFileService readRseReportFileService)
        {
            _finder = finder;
            _readRseReportFileService = readRseReportFileService;
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
