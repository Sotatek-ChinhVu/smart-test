using DevExpress.Models;

namespace DevExpress.Models;

public class Karte1ExportModel
{
    public Karte1ExportModel(string sysDateTimeS, string ptNum, string futansyaNo_K1, string hokensyaNo, string jyukyusyaNo_K1, string kigoBango, string ptKanaName, string ptName, string hokenKigenW, string setainusi, string birthDateW, string age, string sex, string hokenSyutokuW, string ptPostCode, string ptAddress1, string ptAddress2, string officeAddress, string officeTel, string ptTel, string office, string ptRenrakuTel, string hokensyaAddress, string zokugara, string hokensyaTel, string job, string hokensyaName, string futansyaNo_K2, string jyukyusyaNo_K2, List<Karte1ByomeiModel> listByomeiModelsPage1, List<Karte1ByomeiModel> listByomeiModelsPage2)
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
        ListByomeiModelsPage1 = listByomeiModelsPage1;
        ListByomeiModelsPage2 = listByomeiModelsPage2;
    }
    public Karte1ExportModel()
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
        ListByomeiModelsPage1 = new();
        ListByomeiModelsPage2 = new();
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
    public List<Karte1ByomeiModel> ListByomeiModelsPage1 { get; private set; }
    public List<Karte1ByomeiModel> ListByomeiModelsPage2 { get; private set; }

    #region gen data to display
    public char[] CharHokensyaNo { get { return HokensyaNo.PadLeft(8, ' ').ToCharArray(); } }
    public string HokensyaNo_1 { get { return CharHokensyaNo[0].ToString(); } }
    public string HokensyaNo_2 { get { return CharHokensyaNo[1].ToString(); } }
    public string HokensyaNo_3 { get { return CharHokensyaNo[2].ToString(); } }
    public string HokensyaNo_4 { get { return CharHokensyaNo[3].ToString(); } }
    public string HokensyaNo_5 { get { return CharHokensyaNo[4].ToString(); } }
    public string HokensyaNo_6 { get { return CharHokensyaNo[5].ToString(); } }
    public string HokensyaNo_7 { get { return CharHokensyaNo[6].ToString(); } }
    public string HokensyaNo_8 { get { return CharHokensyaNo[7].ToString(); } }

    public char[] CharFutansyaNo_K1 { get { return FutansyaNo_K1.PadLeft(8, ' ').ToCharArray(); } }
    public string FutansyaNo_K1_1 { get { return CharFutansyaNo_K1[0].ToString(); } }
    public string FutansyaNo_K1_2 { get { return CharFutansyaNo_K1[1].ToString(); } }
    public string FutansyaNo_K1_3 { get { return CharFutansyaNo_K1[2].ToString(); } }
    public string FutansyaNo_K1_4 { get { return CharFutansyaNo_K1[3].ToString(); } }
    public string FutansyaNo_K1_5 { get { return CharFutansyaNo_K1[4].ToString(); } }
    public string FutansyaNo_K1_6 { get { return CharFutansyaNo_K1[5].ToString(); } }
    public string FutansyaNo_K1_7 { get { return CharFutansyaNo_K1[6].ToString(); } }
    public string FutansyaNo_K1_8 { get { return CharFutansyaNo_K1[7].ToString(); } }

    public char[] CharFutansyaNo_K2 { get { return FutansyaNo_K2.PadLeft(8, ' ').ToCharArray(); } }
    public string FutansyaNo_K2_1 { get { return CharFutansyaNo_K2[0].ToString(); } }
    public string FutansyaNo_K2_2 { get { return CharFutansyaNo_K2[1].ToString(); } }
    public string FutansyaNo_K2_3 { get { return CharFutansyaNo_K2[2].ToString(); } }
    public string FutansyaNo_K2_4 { get { return CharFutansyaNo_K2[3].ToString(); } }
    public string FutansyaNo_K2_5 { get { return CharFutansyaNo_K2[4].ToString(); } }
    public string FutansyaNo_K2_6 { get { return CharFutansyaNo_K2[5].ToString(); } }
    public string FutansyaNo_K2_7 { get { return CharFutansyaNo_K2[6].ToString(); } }
    public string FutansyaNo_K2_8 { get { return CharFutansyaNo_K2[7].ToString(); } }

    public char[] CharJyukyusyaNo_K1 { get { return JyukyusyaNo_K1.PadLeft(7, ' ').ToCharArray(); } }
    public string JyukyusyaNo_K1_1 { get { return CharJyukyusyaNo_K1[0].ToString(); } }
    public string JyukyusyaNo_K1_2 { get { return CharJyukyusyaNo_K1[1].ToString(); } }
    public string JyukyusyaNo_K1_3 { get { return CharJyukyusyaNo_K1[2].ToString(); } }
    public string JyukyusyaNo_K1_4 { get { return CharJyukyusyaNo_K1[3].ToString(); } }
    public string JyukyusyaNo_K1_5 { get { return CharJyukyusyaNo_K1[4].ToString(); } }
    public string JyukyusyaNo_K1_6 { get { return CharJyukyusyaNo_K1[5].ToString(); } }
    public string JyukyusyaNo_K1_7 { get { return CharJyukyusyaNo_K1[6].ToString(); } }

    public char[] CharJyukyusyaNo_K2 { get { return JyukyusyaNo_K2.PadLeft(7, ' ').ToCharArray(); } }
    public string JyukyusyaNo_K2_1 { get { return CharJyukyusyaNo_K2[0].ToString(); } }
    public string JyukyusyaNo_K2_2 { get { return CharJyukyusyaNo_K2[1].ToString(); } }
    public string JyukyusyaNo_K2_3 { get { return CharJyukyusyaNo_K2[2].ToString(); } }
    public string JyukyusyaNo_K2_4 { get { return CharJyukyusyaNo_K2[3].ToString(); } }
    public string JyukyusyaNo_K2_5 { get { return CharJyukyusyaNo_K2[4].ToString(); } }
    public string JyukyusyaNo_K2_6 { get { return CharJyukyusyaNo_K2[5].ToString(); } }
    public string JyukyusyaNo_K2_7 { get { return CharJyukyusyaNo_K2[6].ToString(); } }

    #endregion

}
