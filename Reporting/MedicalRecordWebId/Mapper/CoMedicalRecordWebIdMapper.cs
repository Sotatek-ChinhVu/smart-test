using Helper.Common;
using Reporting.Mappers.Common;
using Reporting.MedicalRecordWebId.Model;

namespace Reporting.MedicalRecordWebId.Mapper;

public class CoMedicalRecordWebIdMapper : CommonReportingRequest
{
    private readonly CoMedicalRecordWebIdModel? _coModel;

    public CoMedicalRecordWebIdMapper(CoMedicalRecordWebIdModel? printOutData)
    {
        _coModel = printOutData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.MedicalRecordWebId;
    }

    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override string GetJobName()
    {
        return "Web登録用ID";
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        Dictionary<string, string> data = new();
        if (_coModel != null)
        {
            data.Add("dfKanNm", _coModel.Name + "様");
            data.Add("dfKanID", _coModel.PtNum.ToString());
            data.Add("dfWebID", _coModel.WebId);
            data.Add("dfDate", CIUtil.SDateToShowWDate2(_coModel.SinDate));
            data.Add("dfHpNm", _coModel.HpName);

            var qrCode = _coModel.WebIdQrCode;
            qrCode = qrCode.Replace("{0}", _coModel.MedicalInstitutionCode);
            qrCode = qrCode.Replace("{1}", _coModel.WebId);

            data.Add("qrURL", qrCode);
            data.Add("dfPCURL", _coModel.WebIdUrlForPc);
        }
        return data;
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        return new();
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return new();
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }
}
