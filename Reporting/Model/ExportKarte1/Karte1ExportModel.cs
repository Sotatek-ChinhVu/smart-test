namespace Reporting.Model.ExportKarte1;

public class Karte1ExportModel
{
    public Karte1ExportModel(string sysDateTimeS, string ptNum, string futansyaNo_K1, string hokensyaNo, string jyukyusyaNo_K1, string kigoBango, string ptKanaName, string ptName, string hokenKigenW, string setainusi, string birthDateW, string age, string sex, string hokenSyutokuW, string ptPostCode, string ptAddress1, string ptAddress2, string officeAddress, string officeTel, string ptTel, string office, string ptRenrakuTel, string hokensyaAddress, string zokugara, string hokensyaTel, string job, string hokensyaName, string futansyaNo_K2, string jyukyusyaNo_K2, List<Karte1ByomeiModel> listByomeis)
    {
        SysDateTimeS = sysDateTimeS;
        PtNum = ptNum;
        FutansyaNo_K1 = futansyaNo_K1;
        HokensyaNo = hokensyaNo;
        JyukyusyaNo_K1 = jyukyusyaNo_K1;
        KigoBango = kigoBango;
        PtKanaName = ptKanaName;
        PtName = ptName;
        HokenKigenW = hokenKigenW;
        Setainusi = setainusi;
        BirthDateW = birthDateW;
        Age = age;
        Sex = sex;
        HokenSyutokuW = hokenSyutokuW;
        PtPostCode = ptPostCode;
        PtAddress1 = ptAddress1;
        PtAddress2 = ptAddress2;
        OfficeAddress = officeAddress;
        OfficeTel = officeTel;
        PtTel = ptTel;
        Office = office;
        PtRenrakuTel = ptRenrakuTel;
        HokensyaAddress = hokensyaAddress;
        Zokugara = zokugara;
        HokensyaTel = hokensyaTel;
        Job = job;
        HokensyaName = hokensyaName;
        FutansyaNo_K2 = futansyaNo_K2;
        JyukyusyaNo_K2 = jyukyusyaNo_K2;
        ListByomeis = listByomeis;
    }

    public string SysDateTimeS { get; private set; }

    public string PtNum { get; private set; }

    public string FutansyaNo_K1 { get; private set; }

    public string HokensyaNo { get; private set; }

    public string JyukyusyaNo_K1 { get; private set; }

    public string KigoBango { get; private set; }

    public string PtKanaName { get; private set; }

    public string PtName { get; private set; }

    public string HokenKigenW { get; private set; }

    public string Setainusi { get; private set; }

    public string BirthDateW { get; private set; }

    public string Age { get; private set; }

    public string Sex { get; private set; }

    public string HokenSyutokuW { get; private set; }

    public string PtPostCode { get; private set; }

    public string PtAddress1 { get; private set; }

    public string PtAddress2 { get; private set; }

    public string OfficeAddress { get; private set; }

    public string OfficeTel { get; private set; }

    public string PtTel { get; private set; }

    public string Office { get; private set; }

    public string PtRenrakuTel { get; private set; }

    public string HokensyaAddress { get; private set; }

    public string Zokugara { get; private set; }

    public string HokensyaTel { get; private set; }

    public string Job { get; private set; }

    public string HokensyaName { get; private set; }

    public string FutansyaNo_K2 { get; private set; }

    public string JyukyusyaNo_K2 { get; private set; }

    public List<Karte1ByomeiModel> ListByomeis { get; private set; }
}
