using Domain.Models.HpInf;
using Domain.Models.KensaSet;
using Reporting.KensaHistory.DB;
using Reporting.KensaHistory.Mapper;
using Reporting.Mappers.Common;
using Reporting.Sokatu;

namespace Reporting.KensaHistory.Service
{
    public class KensaHistoryCoReportService : IKensaHistoryCoReportService
    {
        private IKensaSetRepository _kokhoFinder;
        private ICoKensaHistoryFinder _coKensaHistoryFinder;
        private HpInfModel hpInf;
        private int hpId;
        private List<string> printHokensyaNos;
        private bool hasNextPage;
        private int currentPage;

        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<string, string> _extralData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private const string _formFileName = "p08KoukiSeikyu.rse";
        private readonly Dictionary<int, ReportConfigModel> _reportConfigPerPage;
        private readonly Dictionary<string, bool> _visibleAtPrint;

        public KensaHistoryCoReportService(IKensaSetRepository kokhoFinder)
        {
            _kokhoFinder = kokhoFinder;
            _setFieldData = new();
            _singleFieldData = new();
            _extralData = new();
            _listTextData = new();
            _visibleFieldData = new();
            _visibleAtPrint = new();
        }

        public CommonReportingRequestModel GetKensaHistoryPrintData(int hpId, long ptId, int startSinYm, int endSinYm, bool includeHoken, bool includeJihi)
        {
            var pageIndex = _listTextData.Select(item => item.Key).Distinct().Count();
            _extralData.Add("totalPage", pageIndex.ToString());
            return new KensaHistoryMapper(_reportConfigPerPage, _setFieldData, _listTextData, _extralData, _formFileName, _singleFieldData, _visibleFieldData, _visibleAtPrint).GetData();
        }

        private bool GetData()
        {
            hpInf = _coKensaHistoryFinder.GetHpInf(hpId);
            receInfs = _kokhoFinder.GetReceInf(hpId, seikyuYm, seikyuType, KokhoKind.Kouki, PrefKbn.PrefAll, MyPrefNo, HokensyaNoKbn.NoSum);
            //保険者番号の指定がある場合は絞り込み
            var wrkReceInfs = printHokensyaNos == null ? receInfs.ToList() :
                receInfs.Where(r => printHokensyaNos.Contains(r.HokensyaNo)).ToList();
            //保険者番号リストを取得
            hokensyaNos = wrkReceInfs.GroupBy(r => r.HokensyaNo).OrderBy(r => r.Key).Select(r => r.Key).ToList();

            return (receInfs?.Count ?? 0) > 0;
        }

        private void SetFieldData(string field, string value)
        {
            if (!string.IsNullOrEmpty(field) && !_singleFieldData.ContainsKey(field))
            {
                _singleFieldData.Add(field, value);
            }
        }

        private void SetVisibleFieldData(string field, bool value)
        {
            if (!string.IsNullOrEmpty(field) && !_visibleFieldData.ContainsKey(field))
            {
                _visibleFieldData.Add(field, value);
            }
        }
    }
}
