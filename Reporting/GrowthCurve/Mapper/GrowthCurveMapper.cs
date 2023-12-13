using Reporting.Mappers.Common;

namespace Reporting.GrowthCurve.Mapper
{
    public class GrowthCurveMapper : CommonReportingRequest
    {
        private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
        private readonly Dictionary<int, Dictionary<int, List<ListDrawLineObject>>> _listDrawLineData;
        private readonly Dictionary<int, Dictionary<int, List<ListDrawTextObject>>> _listDrawTextData;
        private readonly Dictionary<int, Dictionary<int, List<ListDrawBoxObject>>> _listDrawBoxData;
        private readonly Dictionary<int, Dictionary<int, List<ListDrawCircleObject>>> _listDrawCircleData;
        private readonly Dictionary<string, string> _singleFieldData;
        private readonly Dictionary<int, List<ListTextObject>> _listTextData;
        private readonly Dictionary<string, string> _extralData;
        private readonly string _formFileName;
        private readonly Dictionary<string, bool> _visibleFieldData;
        private readonly string _jobName;

        public GrowthCurveMapper(Dictionary<int, Dictionary<string, string>> setFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileName, Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldData, Dictionary<int, Dictionary<int, List<ListDrawLineObject>>> listDrawLineData, Dictionary<int, Dictionary<int, List<ListDrawTextObject>>> listDrawTextData,
        Dictionary<int, Dictionary<int, List<ListDrawBoxObject>>> listDrawBoxData,
        Dictionary<int, Dictionary<int, List<ListDrawCircleObject>>> listDrawCircleData,
        string jobName)
        {
            _setFieldData = setFieldData;
            _listTextData = listTextData;
            _extralData = extralData;
            _formFileName = formFileName;
            _singleFieldData = singleFieldData;
            _visibleFieldData = visibleFieldData;
            _listDrawLineData = listDrawLineData;
            _listDrawTextData = listDrawTextData;
            _listDrawBoxData = listDrawBoxData;
            _listDrawCircleData = listDrawCircleData;
            _jobName = jobName;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.GrowthCurve;
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
            return  _jobName; 
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
            return _setFieldData;
        }

        public override Dictionary<int, Dictionary<int, List<ListDrawLineObject>>> GetDrawLineData()
        {
            return _listDrawLineData;
        }

        public override Dictionary<int, Dictionary<int, List<ListDrawTextObject>>> GetDrawTextData()
        {
            return _listDrawTextData;
        }

        public override Dictionary<int, Dictionary<int, List<ListDrawBoxObject>>> GetDrawBoxData()
        {
            return _listDrawBoxData;
        }

        public override Dictionary<int, Dictionary<int, List<ListDrawCircleObject>>> GetDrawCircleData()
        {
            return _listDrawCircleData;
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            var fileName = new Dictionary<string, string>
                           {
                                { "1", _formFileName }
                           };
            return fileName;
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return _visibleFieldData;
        }
    }
}