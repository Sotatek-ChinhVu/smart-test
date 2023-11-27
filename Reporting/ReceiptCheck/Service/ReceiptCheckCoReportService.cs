using Domain.Models.SetMst;
using Helper.Common;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Reporting.Mappers.Common;
using Reporting.ReceiptCheck.DB;
using Reporting.ReceiptCheck.Mapper;
using Reporting.ReceiptCheck.Model;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace Reporting.ReceiptCheck.Service;

public class ReceiptCheckCoReportService : RepositoryBase, IReceiptCheckCoReportService
{
    private const int MAX_LENG_MESSAGE = 90;

    private readonly ITenantProvider _tenantProvider;
    private List<CoReceiptCheckModel> _coModels;
    private CoReceiptCheckModel _coModel;
    private string _messageOld;
    private readonly char[] _arrCharNotEnd = new char[] { '(', '"', '\'', '{', '[', '’', '′', '“', '「', '【', '［', '『', '（', '’', '″', '‘', '`', '‘' };
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private bool _hasNextPage = true;
    private readonly string key;
    private readonly IDatabase _cache;
    private readonly IConfiguration _configuration;
    private int currentPage = 1;

    public ReceiptCheckCoReportService(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        _tenantProvider = tenantProvider;
        _singleFieldData = new();
        _setFieldData = new();
        _listTextData = new();
        _extralData = new();
        _messageOld = string.Empty;
        _coModel = new();
        _coModels = new();
        key = GetCacheKey() + "ReceiptCheckCoReporting";
        _configuration = configuration;
        GetRedis();
        _cache = RedisConnectorHelper.Connection.GetDatabase();
    }

    public void GetRedis()
    {
        string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
        if (RedisConnectorHelper.RedisHost != connection)
        {
            RedisConnectorHelper.RedisHost = connection;
        }
    }

    public CommonReportingRequestModel GetReceiptCheckCoReportingData(int hpId, List<long> ptIds, int seikyuYm)
    {
        try
        {
            ptIds = ptIds.OrderBy(x => x).ToList();
            StringBuilder extendKey = new();
            extendKey.Append(seikyuYm);
            extendKey.Append("_");
            foreach (var item in ptIds)
            {
                extendKey.Append(item + "_");
            }
            var finalKey = key + "_" + extendKey.ToString();
            if (_cache.KeyExists(finalKey))
            {
                var results = _cache.StringGet(finalKey);
                var json = results.AsString();
                _cache.KeyDelete(finalKey);
                if (!string.IsNullOrEmpty(json))
                {
                    return new CommonReportingRequestModel()
                    {
                        DataJsonConverted = json
                    };
                }
            }

            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoReceiptCheckFinder(_tenantProvider);

                // データ取得
                _coModels = finder.GetCoReceiptChecks(hpId, ptIds, seikyuYm);
                if (_coModels != null && _coModels.Any())
                {
                    // レセプト印刷
                    while (_hasNextPage)
                    {
                        UpdateDrawForm(seikyuYm);
                    }
                }
                _extralData.Add("totalPage", currentPage.ToString());
                return new CoReceiptCheckMapper(_setFieldData, _singleFieldData, _listTextData, _extralData).GetData();
            }
        }
        finally
        {
            _tenantProvider.DisposeDataContext();
        }
    }

    public bool CheckOpenReceiptCheck(int hpId, List<long> ptIds, int seikyuYm)
    {
        try
        {
            ptIds = ptIds.OrderBy(x => x).ToList();
            StringBuilder extendKey = new();
            extendKey.Append(seikyuYm);
            extendKey.Append("_");
            foreach (var item in ptIds)
            {
                extendKey.Append(item + "_");
            }
            var finalKey = key + "_" + extendKey.ToString();

            if (_cache.KeyExists(finalKey))
            {
                _cache.KeyDelete(finalKey);
            }
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoReceiptCheckFinder(_tenantProvider);
                // データ取得
                _coModels = finder.GetCoReceiptChecks(hpId, ptIds, seikyuYm);
                bool exist = _coModels != null && _coModels.Any();
                if (exist)
                {
                    // レセプト印刷
                    while (_hasNextPage)
                    {
                        UpdateDrawForm(seikyuYm);
                    }
                    _extralData.Add("totalPage", currentPage.ToString());
                    var result = new CoReceiptCheckMapper(_setFieldData, _singleFieldData, _listTextData, _extralData).GetData();
                    var json = JsonSerializer.Serialize(result);
                    _cache.StringSet(finalKey, json);
                }
                return exist;
            }
        }
        finally
        {
            _tenantProvider.DisposeDataContext();
        }
    }

    private void UpdateDrawForm(int seikyuYm)
    {
        List<ListTextObject> listDataPerPage = new();
        Dictionary<string, string> fieldDataPerPage = new();
        string tempSinYm = seikyuYm.AsString().Insert(4, "年");
        string sYmd = CIUtil.SDateToShowSWDate(CIUtil.StrToIntDef(DateTime.Now.ToString("yyyyMMdd"), 0), fmtDate: 1) +
                      "（" + CIUtil.JapanDayOfWeek(DateTime.Now) + "）" +
                       DateTime.Now.ToString("HH:mm") + " 作成";

        string sTgtYm = tempSinYm + "月度";

        SetFieldData("DT_TgtYm", sTgtYm);
        SetFieldData("DT_Date", sYmd);

        int linePinted = 0;
        while (linePinted < 45)
        {
            string fieldMessage = "DT_ErrMsg" + (linePinted + 1);
            if (!string.IsNullOrEmpty(_messageOld))
            {
                string message = CIUtil.CiCopyStrWidth(_messageOld, 1, MAX_LENG_MESSAGE);
                fieldDataPerPage.Add(fieldMessage, message);
                _messageOld = _messageOld.Remove(0, message.Length);
            }
            else
            {
                var coModel = _coModels.FirstOrDefault();
                if (coModel == null)
                {
                    _hasNextPage = false;
                    break;
                }

                if (_coModel == null ||
                    coModel.SinYm != _coModel.SinYm ||
                    coModel.PtId != _coModel.PtId ||
                    coModel.HokenId != _coModel.HokenId)
                {
                    if (linePinted > 43)
                    {
                        _hasNextPage = true;
                        break;
                    }

                    string sinYm = coModel.SinYm.AsString().Insert(4, "年") + "月";
                    listDataPerPage.Add(new("DT_Ym", 0, linePinted, sinYm));
                    listDataPerPage.Add(new("DT_KanID", 0, linePinted, coModel.PtNum.AsString()));
                    listDataPerPage.Add(new("DT_KanNM", 0, linePinted, coModel.PtName));
                    listDataPerPage.Add(new("DT_HoInf", 0, linePinted, coModel.HokenName));

                    _coModel = coModel;
                }

                var messagetemp = coModel.ErrorMessage;
                string message = CIUtil.CiCopyStrWidth(messagetemp, 1, MAX_LENG_MESSAGE);
                if (_arrCharNotEnd.Contains(message.LastOrDefault()))
                {
                    message = message.Remove(message.Length - 1, 1);
                }

                fieldDataPerPage.Add(fieldMessage, message);
                _messageOld = messagetemp.Remove(0, message.Length);

                _coModels.RemoveAt(0);
            }

            linePinted++;
        }
        _listTextData.Add(currentPage, listDataPerPage);
        _setFieldData.Add(currentPage, fieldDataPerPage);
        _hasNextPage = !string.IsNullOrEmpty(_messageOld) || _coModels.Any();
        if (_hasNextPage)
        {
            currentPage++;
        }
    }


    private void SetFieldData(string field, string value)
    {
        if (string.IsNullOrEmpty(field) || !_singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }
}
