using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MedicalExamination.SummaryInf
{
    public class SummaryInfItem
    {
        public SummaryInfItem(string headerInfo, string headerName, string propertyColor, double spaceHeaderName, double spaceHeaderInfo, double headerNameSize, int grpItemCd, string text)
        {
            HeaderInfo = headerInfo;
            HeaderName = headerName;
            PropertyColor = propertyColor;
            SpaceHeaderName = spaceHeaderName;
            SpaceHeaderInfo = spaceHeaderInfo;
            HeaderNameSize = headerNameSize;
            GrpItemCd = grpItemCd;
            Text = text;
        }

        public SummaryInfItem()
        {
            HeaderInfo = string.Empty;
            HeaderName = string.Empty;
            PropertyColor = string.Empty;
            Text = string.Empty;
        }

        public SummaryInfItem ChangePropertyColor(string propertyColor)
        {
            PropertyColor = propertyColor;
            return this;
        }

        public string HeaderInfo { get; private set; }

        public string HeaderName { get; private set; }

        public string PropertyColor { get; private set; }

        public double SpaceHeaderName { get; private set; }

        public double SpaceHeaderInfo { get; private set; }

        public double HeaderNameSize { get; private set; }

        public int GrpItemCd { get; private set; }

        public string Text { get; private set; }
    }
}
