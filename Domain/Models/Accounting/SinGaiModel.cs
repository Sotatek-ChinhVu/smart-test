using Helper.Extension;

namespace Domain.Models.Accounting
{
    public class SinGaiModel
    {
        public SinGaiModel(string propertyName, int point, bool isForegroundBlue)
        {
            PropertyName = propertyName;
            Point = point;
            IsForegroundBlue = isForegroundBlue;
        }

        public string PropertyName { get; private set; }
        public int Point { get; private set; }
        public bool IsForegroundBlue { get; private set; }
        public string PointBinding { get => Point.AsString() + "円"; }
    }
}
