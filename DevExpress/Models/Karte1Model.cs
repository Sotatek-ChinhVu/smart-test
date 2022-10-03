using DevExpress.Mode;

namespace DevExpress.Models;

public class Karte1Model
{
    public Karte1Model(string sysDateTimeS, string ptNum, string futansyaNo_K1, string hokensyaNo, string jyukyusyaNo_K1, string kigoBango, string ptKanaName, string ptName, string hokenKigenW, string setainusi, string birthDateW, string age, string sex, string hokenSyutokuW, string ptPostCode, string ptAddress1, string ptAddress2, string officeAddress, string officeTel, string ptTel, string office, string ptRenrakuTel, string hokensyaAddress, string zokugara, string hokensyaTel, string job, string hokensyaName, string futansyaNo_K2, string jyukyusyaNo_K2, List<Karte1ByomeiModel> listByomeiModels_p1, List<Karte1ByomeiModel> listByomeiModels_p2)
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
        ListByomeiModels_p1 = listByomeiModels_p1;
        ListByomeiModels_p2 = listByomeiModels_p2;
    }
    public Karte1Model()
    {
        SysDateTimeS = "SysDateTimeS";
        PtNum = "ptNum";
        FutansyaNo_K1 = "futansyaNo_K1";
        HokensyaNo = "hokensyaNo";
        JyukyusyaNo_K1 = "jyukyusyaNo_K1";
        KigoBango = "kigoBango";
        PtKanaName = "ptKanaName";
        PtName = "ptName";
        HokenKigenW = "hokenKigenW";
        Setainusi = "setainusi";
        BirthDateW = "birthDateW";
        Age = "age";
        Sex = "sex";
        HokenSyutokuW = "hokenSyutokuW";
        PtPostCode = "ptPostCode";
        PtAddress1 = "ptAddress1";
        PtAddress2 = "ptAddress2";
        OfficeAddress = "officeAddress";
        OfficeTel = "officeTel";
        PtTel = "ptTel";
        Office = "office";
        PtRenrakuTel = "ptRenrakuTel";
        HokensyaAddress = "hokensyaAddress";
        Zokugara = "zokugara";
        HokensyaTel = "hokensyaTel";
        Job = "job";
        HokensyaName = "hokensyaName";
        FutansyaNo_K2 = "futansyaNo_K2";
        JyukyusyaNo_K2 = "jyukyusyaNo_K2";
        ListByomeiModels_p1 = new();
        ListByomeiModels_p2 = new();
    }

    public string SysDateTimeS { get; set; }
    public string PtNum { get; set; }
    public string FutansyaNo_K1 { get; set; }
    public string HokensyaNo { get; set; }
    public string JyukyusyaNo_K1 { get; set; }
    public string KigoBango { get; set; }
    public string PtKanaName { get; set; }
    public string PtName { get; set; }
    public string HokenKigenW { get; set; }
    public string Setainusi { get; set; }
    public string BirthDateW { get; set; }
    public string Age { get; set; }
    public string Sex { get; set; }
    public string HokenSyutokuW { get; set; }
    public string PtPostCode { get; set; }
    public string PtAddress1 { get; set; }
    public string PtAddress2 { get; set; }
    public string OfficeAddress { get; set; }
    public string OfficeTel { get; set; }
    public string PtTel { get; set; }
    public string Office { get; set; }
    public string PtRenrakuTel { get; set; }
    public string HokensyaAddress { get; set; }
    public string Zokugara { get; set; }
    public string HokensyaTel { get; set; }
    public string Job { get; set; }
    public string HokensyaName { get; set; }
    public string FutansyaNo_K2 { get; set; }
    public string JyukyusyaNo_K2 { get; set; }
    public List<Karte1ByomeiModel> ListByomeiModels_p1 { get; set; }
    public List<Karte1ByomeiModel> ListByomeiModels_p2 { get; set; }
}
