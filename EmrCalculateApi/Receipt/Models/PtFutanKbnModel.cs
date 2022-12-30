using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    class PtFutanKbnModel
    {
        int _hokenPid;
        int _futanS;
        int _futanK1;
        int _futanK2;
        int _futanK3;
        int _futanK4;

        private List<(string pattern, string futanKbnCd)> futanPatternls =
            new List<(string pattern, string futanKbnCd)>
            {
                ("10000", "1"),
                ("01000", "5"),
                ("00100", "6"),
                ("00010", "B"),
                ("00001", "C"),

                ("11000", "2"),
                ("10100", "3"),
                ("10010", "E"),
                ("10001", "G"),
                ("01100", "7"),
                ("01010", "H"),
                ("01001", "I"),
                ("00110", "J"),
                ("00101", "K"),

                ("11100", "4"),
                ("11010", "M"),
                ("11001", "N"),
                ("10110", "O"),
                ("10101", "P"),
                ("10011", "Q"),
                ("01110", "R"),
                ("01101", "S"),
                ("01011", "T"),
                ("00111", "U"),

                ("11110", "V"),
                ("11101", "W"),
                ("11011", "X"),
                ("10111", "Y"),
                ("01111", "Z"),

                ("11111", "9"),
            };

        public PtFutanKbnModel(int hokenPid, int futanS, int futanK1, int futanK2, int futanK3, int futanK4)
        {
            _hokenPid = hokenPid;
            _futanS = futanS;
            _futanK1 = futanK1;
            _futanK2 = futanK2;
            _futanK3 = futanK3;
            _futanK4 = futanK4;
        }

        public int HokenPid
        {
            get { return _hokenPid; }
            set { _hokenPid = value; }
        }

        public int FutanS
        {
            get { return _futanS; }
            set { _futanS = value; }
        }

        public int FutanK1
        {
            get { return _futanK1; }
            set { _futanK1 = value; }
        }
        public int FutanK2
        {
            get { return _futanK2; }
            set { _futanK2 = value; }
        }
        public int FutanK3
        {
            get { return _futanK3; }
            set { _futanK3 = value; }
        }
        public int FutanK4
        {
            get { return _futanK4; }
            set { _futanK4 = value; }
        }

        public string FutanKbnCd
        {
            get
            {
                string ret = "";

                string pattern =
                    FutanS.ToString() +
                    FutanK1.ToString() +
                    FutanK2.ToString() +
                    FutanK3.ToString() +
                    FutanK4.ToString();
                var retPattern = futanPatternls.FindAll(p => p.pattern == pattern);

                if (retPattern != null && retPattern.Any())
                {
                    ret = retPattern.First().futanKbnCd;
                }

                return ret;
            }
        }
    }
}
