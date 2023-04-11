namespace Reporting.MedicalRecordWebId.Model;

public class CoMedicalRecordWebIdModel
{
    public CoHpInfModel HpInfModel { get; }

    public CoPtInfModel PtInfModel { get; }

    public CoPtJibkarModel PtJibkarModel { get; }

    public CoMedicalRecordWebIdModel(int sinDate, CoHpInfModel hpInfModel, CoPtInfModel ptInfModel, CoPtJibkarModel ptJibkarModel, string webIdQrCode, string medicalInstitutionCode, string webIdUrlForPc)
    {
        HpInfModel = hpInfModel;
        PtInfModel = ptInfModel;
        PtJibkarModel = ptJibkarModel;
        SinDate = sinDate;
        WebIdQrCode = webIdQrCode;
        WebIdUrlForPc = webIdUrlForPc;
        MedicalInstitutionCode = medicalInstitutionCode;
    }

    public string Name => PtInfModel.Name;

    public string KanaName => PtInfModel.KanaName;

    public long PtNum => PtInfModel.PtNum;

    public long PtId => PtInfModel.PtId;

    public int Birthday => PtInfModel.Birthday;

    public string HpName => HpInfModel.HpName;

    public string WebId => PtJibkarModel.WebId;

    public int SinDate { get; }

    public string WebIdQrCode { get; }

    public string MedicalInstitutionCode { get; }

    public string WebIdUrlForPc { get; }
}
