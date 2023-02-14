using KarteReport.Interface;
using System.Reflection;
using System.Text;


namespace KarteReport.Utility
{
    public class TemplateGenerator
    {
        private readonly IReportServices _reportService;

        public TemplateGenerator(IReportServices reportService)
        {
            _reportService = reportService;
        }

        private string GetTemplate(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            if (assembly == null)
            {
                return string.Empty;
            }
            var resourceStream = assembly.GetManifestResourceStream(@"KarteReport.Templates." + fileName);
            if (resourceStream == null)
            {
                return string.Empty;
            }
            StringBuilder stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
            {
                while (!reader.EndOfStream)
                {
                    string? splitMe = reader.ReadLine();
                    if (splitMe == null)
                    {
                        continue;
                    }
                    stringBuilder.Append(splitMe);
                }
            }

            return stringBuilder.ToString();
        }

        public string GetHTMLString(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
        {
            var sb = new StringBuilder();
            string template = GetTemplate("Karte1.Karte1Template.txt");
            sb.Append(template);

            var rootPath = Environment.CurrentDirectory;
            string fileName = Path.Combine(rootPath, "print1.jpg");
            sb.Replace("{{fileName}}", fileName);

            var karte1Data = _reportService.GetKarte1ReportingData(hpId, ptId, sinDate, hokenPid, tenkiByomei, syuByomei);
            var bmCount = karte1Data.ListByomeis.Count;

            for (int i = 0; i < bmCount; i++)
            {
                sb.Replace(string.Concat("{isByomei_", (i + 1).ToString(), "}"), karte1Data.ListByomeis[i].Byomei);
                sb.Replace(string.Concat("{isByomeiStartDateW", (i + 1).ToString(), "}"), karte1Data.ListByomeis[i].ByomeiStartDateWFormat);
                sb.Replace(string.Concat("{isByomeiTenkiDate", (i + 1).ToString(), "}"), karte1Data.ListByomeis[i].ByomeiTenkiDateWFormat);
                if (karte1Data.ListByomeis[i].TenkiTiyuMaru)
                {
                    sb.Replace(string.Concat("{IsTenkiTiyuMaru", (i + 1).ToString(), "}"), " O");
                    sb.Replace(string.Concat("{IsTenkiSiboMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiChusMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSonoTa", (i + 1).ToString(), "}"), string.Empty);
                }
                else if (karte1Data.ListByomeis[i].TenkiSiboMaru)
                {
                    sb.Replace(string.Concat("{IsTenkiTiyuMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSiboMaru", (i + 1).ToString(), "}"), " O");
                    sb.Replace(string.Concat("{IsTenkiChusMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSonoTa", (i + 1).ToString(), "}"), string.Empty);
                }
                else if (karte1Data.ListByomeis[i].TenkiChusiMaru)
                {
                    sb.Replace(string.Concat("{IsTenkiTiyuMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSiboMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiChusMaru", (i + 1).ToString(), "}"), " O");
                    sb.Replace(string.Concat("{IsTenkiSonoTa", (i + 1).ToString(), "}"), string.Empty);
                }
                else if (karte1Data.ListByomeis[i].TenkiSonota)
                {
                    sb.Replace(string.Concat("{IsTenkiTiyuMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSiboMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiChusMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSonoTa", (i + 1).ToString(), "}"), " O");
                }
                else
                {
                    sb.Replace(string.Concat("{IsTenkiTiyuMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSiboMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiChusMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSonoTa", (i + 1).ToString(), "}"), string.Empty);
                }

            }

            if (bmCount < 10)
            {
                for (int i = bmCount; i < 10; i++)
                {
                    sb.Replace(string.Concat("{isByomei_", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{isByomeiStartDateW", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{isByomeiTenkiDate", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiTiyuMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSiboMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiChusMaru", (i + 1).ToString(), "}"), string.Empty);
                    sb.Replace(string.Concat("{IsTenkiSonoTa", (i + 1).ToString(), "}"), string.Empty);
                }
            }



            sb.Replace("{dfJob}", karte1Data.Job);
            sb.Replace("{dfZokugara}", karte1Data.Zokugara);
            sb.Replace("{dfHokensyaName}", karte1Data.HokensyaName);
            sb.Replace("{dfHokensyaAddress}", karte1Data.HokensyaAddress);
            sb.Replace("{dfHokensyaTel}", karte1Data.HokensyaTel);
            sb.Replace("{dfOffice}", karte1Data.OfficeName);
            sb.Replace("{dfOfficeTel}", karte1Data.OfficeTel);
            sb.Replace("{dfOfficeAddress}", karte1Data.OfficeAddress);
            sb.Replace("{dfHokenSyutokuW}", karte1Data.HokenSyutokuW);
            sb.Replace("{dfSetainusi}", karte1Data.SetaiNusi);
            sb.Replace("{dfHokenKigenW}", karte1Data.HokenKigenW);
            sb.Replace("{dfKigoBango}", karte1Data.KigoBango);
            sb.Replace("{dfHokensyaNo}", karte1Data.HokensyaNo);
            sb.Replace("{dfPtRennakuTel}", karte1Data.PtRenrakuTel);
            sb.Replace("{dfPtTel}", karte1Data.PtTel);
            sb.Replace("{dfPtPostCode}", karte1Data.PtPostCd);
            sb.Replace("{dfPtAddress1}", karte1Data.PtHomeAddress1);
            sb.Replace("{dfPtAddress2}", karte1Data.PtHomeAddress2);
            sb.Replace("{dfPtPostCode}", karte1Data.PtPostCd);
            sb.Replace("{dfBirthDateW}", karte1Data.BirthDateW);
            sb.Replace("{dfAge}", karte1Data.Age.ToString());
            var gender = "男";
            if (karte1Data.Sex == 0) gender = "女";
            sb.Replace("{Sex}", gender);
            sb.Replace("{dfPtKanaName}", karte1Data.PtKanaName);
            sb.Replace("{dfPtName}", karte1Data.PtName);
            sb.Replace("{dfFutansyaNo_K1}", karte1Data.FutansyaNo_K1);
            sb.Replace("{dfJukyusyaNo_K1}", karte1Data.JyukyusyaNo_K1);
            sb.Replace("{dfFutansyaNo_K2}", karte1Data.FutansyaNo_K2);
            sb.Replace("{dfJyukyusyaNo_K2}", karte1Data.JyukyusyaNo_K2);
            sb.Replace("{dfPtNum}", karte1Data.PtNum.ToString());
            sb.Replace("{dfSysDateTimeS}", karte1Data.SysDateTimeS);
            return sb.ToString();
        }
    }
}
