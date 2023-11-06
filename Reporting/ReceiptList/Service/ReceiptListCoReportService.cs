using Helper.Common;
using Infrastructure.Interfaces;
using Reporting.CommonMasters.Config;
using Reporting.Mappers.Common;
using Reporting.ReceiptList.DB;
using Reporting.ReceiptList.Mapper;
using Reporting.ReceiptList.Model;

namespace Reporting.ReceiptList.Service;

public class ReceiptListCoReportService : IReceiptListCoReportService
{
    private readonly ITenantProvider _tenantProvider;
    private readonly ISystemConfig _systemConfig;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private List<ReceiptListModel> _receiptListModels;
    private DateTime _printoutDateTime = CIUtil.GetJapanDateTimeNow();

    public ReceiptListCoReportService(ITenantProvider tenantProvider, ISystemConfig systemConfig)
    {
        _tenantProvider = tenantProvider;
        _tableFieldData = new();
        _singleFieldData = new();
        _receiptListModels = new();
        _systemConfig = systemConfig;
    }

    public CommonReportingRequestModel GetReceiptListReportingData(int hpId, int seikyuYm, List<ReceiptInputModel> receiptListModels)
    {
        try
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoReceiptListFinder(_tenantProvider, _systemConfig);
                _receiptListModels = finder.GetDataReceReport(hpId, seikyuYm, receiptListModels);
                if (_receiptListModels.Any())
                {
                    _printoutDateTime = CIUtil.GetJapanDateTimeNow();
                    UpdateDrawForm(seikyuYm);
                }
                return new ReceiptListMapper(_singleFieldData, _tableFieldData).GetData();
            }
        }
        finally
        {
            _systemConfig.ReleaseResource();
            _tenantProvider.DisposeDataContext();
        }
    }

    private void UpdateDrawForm(int seikyuYm)
    {
        UpdateFormHeader(seikyuYm);
        UpdateFormBody();

        string getYm(int ym)
        {
            return $"{ym / 100}年{ym % 100}月";
        }

        // ヘッダー
        void UpdateFormHeader(int seikyuYm)
        {
            // 請求月
            setFieldData("dfSeikyuYm", getYm(seikyuYm));

            // 発行日時
            int date = CIUtil.DateTimeToInt(_printoutDateTime);
            setFieldData("dfPrintDateTime", CIUtil.SDateToShowSDate(date) + "　" + _printoutDateTime.ToString("HH:mm"));
        }


        // 本体
        void UpdateFormBody()
        {
            if (_receiptListModels == null || _receiptListModels.Count == 0)
            {
                return;
            }

            foreach (var item in _receiptListModels)
            {
                var data = new Dictionary<string, CellModel>
                    {
                        // 診療月
                        { "lsSinYm", new CellModel(item.SinYmDisplay)},
                        // 患者番号
                        { "lsPtNum", new CellModel(item.PtNum.ToString())},
                        // 氏名
                        { "lsPtName", new CellModel(item.Name)},
                        // 性別
                        { "lsSex", new CellModel(item.SexDisplay)},
                        // 年齢
                        { "lsAge", new CellModel(item.AgeDisplay)},
                        // 保険情報
                        { "lsHok", new CellModel(item.ReceSbtDisplay)},
                        // 保険者番号
                        { "lsHokensyaNo", new CellModel(item.HokensyaNo)},
                        // 公費１負担者番号
                        { "lsKo1FutansyaNo", new CellModel(item.FutansyaNoKohi1)},
                        // 公費２負担者番号
                        { "lsKo2FutansyaNo", new CellModel(item.FutansyaNoKohi2)},
                        // 公費３負担者番号
                        { "lsKo3FutansyaNo", new CellModel(item.FutansyaNoKohi3)},
                        // 公費４負担者番号
                        { "lsKo4FutansyaNo", new CellModel(item.FutansyaNoKohi4)},
                        // 請求点数
                        { "lsTensu", new CellModel(item.TensuDisplay)},
                        // 実日数
                        { "lsNisu", new CellModel(item.NissuDisplay)},
                    };
                _tableFieldData.Add(data);
            }
        }
    }

    private void setFieldData(string field, string value)
    {
        if (string.IsNullOrEmpty(field) || !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }
}
