using Helper.Common;
using Helper.Extension;
using Infrastructure.Interfaces;
using Reporting.Mappers.Common;
using Reporting.ReceiptCheck.DB;
using Reporting.ReceiptCheck.Mapper;
using Reporting.ReceiptCheck.Model;

namespace Reporting.ReceiptCheck.Service;

public class ReceiptCheckCoReportService : IReceiptCheckCoReportService
{
    private const int MAX_LENG_MESSAGE = 90;

    private readonly ITenantProvider _tenantProvider;
    private List<CoReceiptCheckModel> coModels;
    private CoReceiptCheckModel coModel;
    private string messageOld;
    private readonly char[] arrCharNotEnd = new char[] { '(', '"', '\'', '{', '[', '’', '′', '“', '「', '【', '［', '『', '（', '’', '″', '‘', '`', '‘' };
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private bool hasNextPage;

    public ReceiptCheckCoReportService(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
        _tableFieldData = new();
        _singleFieldData = new();
        messageOld = string.Empty;
        coModel = new();
        coModels = new();
    }

    public CommonReportingRequestModel GetReceiptCheckCoReportingData(int hpId, List<long> ptIds, int seikyuYm)
    {
        using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
        {
            var finder = new CoReceiptCheckFinder(_tenantProvider);

            // データ取得
            coModels = finder.GetCoReceiptChecks(hpId, ptIds, seikyuYm);
            // レセプト印刷
            while (hasNextPage)
            {
                UpdateDrawForm(seikyuYm);
            }

            return new CoReceiptCheckMapper(_singleFieldData, _tableFieldData).GetData();
        }
    }

    private void UpdateDrawForm(int seikyuYm)
    {
        string tempSinYm = seikyuYm.AsString().Insert(4, "年");
        string sYmd = CIUtil.SDateToShowSWDate(CIUtil.StrToIntDef(DateTime.Now.ToString("yyyyMMdd"), 0), fmtDate: 1) +
                      "（" + CIUtil.JapanDayOfWeek(DateTime.Now) + "）" +
                       DateTime.Now.ToString("HH:mm") + " 作成";

        string sTgtYm = tempSinYm + "月度";

        setFieldData("DT_TgtYm", sTgtYm);
        setFieldData("DT_Date", sYmd);

        int linePinted = 0;
        while (linePinted < 45)
        {
            string fieldMessage = "DT_ErrMsg" + (linePinted + 1);
            if (!string.IsNullOrEmpty(messageOld))
            {
                string message = CIUtil.CiCopyStrWidth(messageOld, 1, MAX_LENG_MESSAGE);
                setFieldData(fieldMessage, message);
                messageOld = messageOld.Remove(0, message.Length);
            }
            else
            {
                var coModelItem = coModels.FirstOrDefault();
                if (coModelItem == null)
                {
                    hasNextPage = false;
                    return;
                }

                if (coModel == null ||
                    coModelItem.SinYm != coModel.SinYm ||
                    coModelItem.PtId != coModel.PtId ||
                    coModelItem.HokenId != coModel.HokenId)
                {
                    if (linePinted > 43)
                    {
                        hasNextPage = true;
                    }

                    string sinYm = coModelItem?.SinYm.AsString().Insert(4, "年") + "月";
                    var data = new Dictionary<string, CellModel>
                    {
                        { "DT_Ym", new CellModel(sinYm) },
                        { "DT_KanID", new CellModel((coModelItem?.PtNum??0).ToString()) },
                        { "DT_KanNM", new CellModel(coModelItem?.PtName??string.Empty) },
                        { "DT_HoInf", new CellModel(coModelItem?.HokenName??string.Empty) }
                    };
                    _tableFieldData.Add(data);
                    coModel = coModelItem!;
                }

                var messagetemp = coModelItem?.ErrorMessage ?? string.Empty;
                string message = CIUtil.CiCopyStrWidth(messagetemp, 1, MAX_LENG_MESSAGE);
                if (arrCharNotEnd.Contains(message.LastOrDefault()))
                {
                    message = message.Remove(message.Length - 1, 1);
                }

                setFieldData(fieldMessage, message);
                messageOld = messagetemp.Remove(0, message.Length);

                coModels.RemoveAt(0);
            }

            linePinted++;
        }
        hasNextPage = !string.IsNullOrEmpty(messageOld) || coModels.Count > 0;
    }


    private void setFieldData(string field, string value)
    {
        if (string.IsNullOrEmpty(field) || _singleFieldData.ContainsKey(field))
        {
            _singleFieldData.Add(field, value);
        }
    }

}
