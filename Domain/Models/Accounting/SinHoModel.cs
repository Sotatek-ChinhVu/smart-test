using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.Accounting
{
    public class SinHoModel
    {
        public SinHoModel(string codeHo, double point, double pointKohi1, double pointKohi2, double pointKohi3, double pointKohi4, bool isForegroundBlue)
        {
            CodeHo = codeHo;
            Point = point;
            PointKohi1 = pointKohi1;
            PointKohi2 = pointKohi2;
            PointKohi3 = pointKohi3;
            PointKohi4 = pointKohi4;
            IsForegroundBlue = isForegroundBlue;
        }

        public string CodeHo { get; private set; }
        public double Point { get; private set; }
        public double PointKohi1 { get; private set; }
        public double PointKohi2 { get; private set; }
        public double PointKohi3 { get; private set; }
        public double PointKohi4 { get; private set; }
        public bool IsForegroundBlue { get; private set; }
        public string CodeHoBinding { get => SinHoConstant.CodeHoDic[CodeHo]; }
        public string PointBinding { get => Point > 0 ? Point.AsString() : ""; }
        public string PointTrialBinding { get => Point > 0 ? Point.AsString() + "点" : "0" + "点"; }
        public string PointKohi1Binding { get => PointKohi1 > 0 ? PointKohi1.AsString() : ""; }
        public string PointKohi2Binding { get => PointKohi2 > 0 ? PointKohi2.AsString() : ""; }
        public string PointKohi3Binding { get => PointKohi3 > 0 ? PointKohi3.AsString() : ""; }
        public string PointKohi4Binding { get => PointKohi4 > 0 ? PointKohi4.AsString() : ""; }
        public bool CheckDefaultValue()
        {
            return false;
        }

    }
}
