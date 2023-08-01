using Domain.Models.SystemConf;
using Helper.Common;
using Reporting.Calculate.Constants;
using Reporting.Calculate.Receipt.Constants;
using Reporting.Calculate.Receipt.Models;
using Reporting.Mappers.Common;
using Reporting.Receipt.Constants;
using Reporting.Receipt.DB;
using Reporting.Receipt.Models;
using static Helper.Common.CIUtil;

namespace Reporting.Receipt.Mapper
{
    public class ReceiptPreviewMapper : CommonReportingRequest
    {
        private readonly CoReceiptModel _coReceipt;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ICoReceiptFinder _coReceiptFinder;
        List<CoReceiptByomeiModel> ByomeiModels;
        List<CoReceiptTekiyoModel> TekiyoModels;
        List<CoReceiptTekiyoModel> TekiyoEnModels;
        Dictionary<string, string> _fileName = new Dictionary<string, string>();
        protected int CurrentPage = 1;
        private int HpId;
        private int Target;
        private int _tekiyoRowCount;
        private int _tekiyoEnRowCount;
        private int _tekiyoRowCount2;

        public ReceiptPreviewMapper(Dictionary<string, string> fileName, CoReceiptModel coReceipt, List<CoReceiptByomeiModel> byomeiModels, List<CoReceiptTekiyoModel> tekiyoModels, List<CoReceiptTekiyoModel> tekiyoEnModels, int currentPage, int hpId, int target, ISystemConfRepository systemConfRepository, ICoReceiptFinder coReceiptFinder, int tekiyoRowCount, int tekiyoEnRowCount, int tekiyoRowCount2)
        {
            _fileName = fileName;
            _coReceipt = coReceipt;
            ByomeiModels = byomeiModels;
            TekiyoModels = tekiyoModels;
            TekiyoEnModels = tekiyoEnModels;
            CurrentPage = currentPage;
            HpId = hpId;
            Target = target;
            _systemConfRepository = systemConfRepository;
            _coReceiptFinder = coReceiptFinder;
            _tekiyoRowCount = tekiyoRowCount;
            _tekiyoEnRowCount = tekiyoEnRowCount;
            _tekiyoRowCount2 = tekiyoRowCount2;
            UpdateDrawForm();
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            return _fileName;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.Receipt;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return SingleData;
        }

        public override string GetRowCountFieldName()
        {
            return "lsTekiyo";
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return CellData;
        }

        public override string GetJobName()
        {
            return "レセプト";
        }
    }
}
