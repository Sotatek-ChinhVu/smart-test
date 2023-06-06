using Reporting.Mappers.Common;

namespace Reporting.Sokatu.KokhoSokatu.Mapper
{
    public class P14KokhoSokatuCoReportServiceMapper : CommonReportingRequest
    {
        private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, string> _extralData;
        private readonly string _formFileName1;
        private readonly string _formFileName2;
        private readonly string _formFileName3;
        private readonly Dictionary<string, bool> _visibleFieldData;

        public P14KokhoSokatuCoReportServiceMapper(Dictionary<int, Dictionary<string, string>> singleFieldDataM, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileName1, string formFileName2, string formFileName3, Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldData)
        {
            _singleFieldDataM = singleFieldDataM;
            _listTextData = listTextData;
            _extralData = extralData;
            _formFileName1 = formFileName1;
            _formFileName2 = formFileName2;
            _formFileName3 = formFileName3;
            _singleFieldData = singleFieldData;
            _visibleFieldData = visibleFieldData;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.KokhoSokatu;
        }

        public override string GetRowCountFieldName()
        {
            return string.Empty;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return new();
        }

        public override Dictionary<string, string> GetExtralData()
        {
            return _extralData;
        }

        public override string GetJobName()
        {
            return string.Empty;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return _singleFieldData;
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new();
        }

        public override Dictionary<int, List<ListTextObject>> GetListTextData()
        {
            return _listTextData;
        }

        public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
        {
            return _singleFieldDataM;
        }
        public override Dictionary<string, string> GetFileNamePageMap()
        {
            var fileName = new Dictionary<string, string>
        {
            { "1", _formFileName1 }, {"2", _formFileName2}, {"3", _formFileName3}
        };
            return fileName;
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return _visibleFieldData;
        }
    }
}
