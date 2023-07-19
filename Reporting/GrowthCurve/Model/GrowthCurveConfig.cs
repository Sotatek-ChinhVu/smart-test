namespace Reporting.GrowthCurve.Model
{
    public class GrowthCurveConfig
    {
        public GrowthCurveConfig(long ptNum, string ptName, int sex, int birthDay, int printModel, int printDate, bool weightVisible, bool heightVisible
            , bool per50, bool per25, bool per10, bool per3, bool sDAvg, bool sD1, bool sD2, bool sD25, bool legend, int scope) 
        {
            PtNum = ptNum;
            PtName = ptName;
            Sex = sex;
            BirthDay = birthDay;
            PrintMode = printModel;
            PrintDate = printDate;
            WeightVisible = weightVisible;
            HeightVisible = heightVisible;
            Per50 = per50;
            Per25 = per25;
            Per10 = per10;
            Per3 = per3;
            SDAvg = sDAvg;
            SD1 = sD1;
            SD2 = sD2;
            SD25 = sD25;
            Legend = legend;
            Scope = scope;
        }
        public long PtNum { get; set; }
        public string PtName { get; set; }
        public int Sex { get; set; }
        public int BirthDay { get; set; }
        public int PrintMode { get; set; }
        public int PrintDate { get; set; }
        public bool WeightVisible { get; set; } = true;
        public bool HeightVisible { get; set; } = true;
        public bool Per50 { get; set; } = true;
        public bool Per25 { get; set; } = true;
        public bool Per10 { get; set; } = true;
        public bool Per3 { get; set; } = true;
        public bool SDAvg { get; set; } = true;
        public bool SD1 { get; set; } = true;
        public bool SD2 { get; set; } = true;
        public bool SD25 { get; set; } = true;
        public bool Legend { get; set; } = true;
        public int Scope { get; set; }
    }
}
