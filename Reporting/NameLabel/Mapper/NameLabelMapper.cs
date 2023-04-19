using Reporting.Mappers.Common;
using Reporting.NameLabel.Models;

namespace Reporting.NameLabel.Mapper;

public class NameLabelMapper : CommonReportingRequest
{
    private readonly CoNameLabelModel _coModel;

    public NameLabelMapper(CoNameLabelModel coModel)
    {
        _coModel = coModel;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.NameLabel;
    }
    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        Dictionary<string, string> data = new();

        // 患者番号
        data.Add("KanNo", _coModel.PtNum.ToString());

        // 患者カナ氏名
        data.Add("KanKana", _coModel.KanaName);

        // 患者氏名
        data.Add("KanName", _coModel.Name);

        // 性別
        data.Add("KanSex", _coModel.SexCnv);

        // 生年月日
        data.Add("KanBirthday", _coModel.BirthdayCnv);
        data.Add("KanBirthdayW", _coModel.WBirthdayCnv);

        // バーコード
        data.Add("Barcode", _coModel.PtNum.ToString());
        data.Add("QRcode", _coModel.PtNum.ToString());

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
