using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtInfNotificationItem : ObservableObject
    {
        public PtInfNotificationItem()
        {

        }

        private string _headerInfo;
        public string HeaderInfo
        {
            get => _headerInfo;
            set => Set(ref _headerInfo, value);
        }

        private string _headerName;
        public string HeaderName
        {
            get => _headerName;
            set => Set(ref _headerName, value);
        }

        private string _propertyColor;
        public string PropertyColor
        {
            get => _propertyColor;
            set => Set(ref _propertyColor, value);
        }

        private double _spaceHeaderName;
        public double SpaceHeaderName
        {
            get => _spaceHeaderName;
            set => Set(ref _spaceHeaderName, value);
        }

        private double _spaceHeaderInfo;
        public double SpaceHeaderInfo
        {
            get => _spaceHeaderInfo;
            set => Set(ref _spaceHeaderInfo, value);
        }

        public double HeaderNameSize { get; set; }

        public int GrpItemCd;

        private byte[] _rtext;
        public byte[] Rtext
        {
            get => _rtext;
            set => Set(ref _rtext, value);
        }
    }
}
