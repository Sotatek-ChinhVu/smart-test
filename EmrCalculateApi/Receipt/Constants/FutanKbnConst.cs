using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Constants
{
    public class FutanKbnConst
    {
        public static List<(string pattern, string futanKbnCd)> futanPatternls =
            new List<(string pattern, string futanKbnCd)>
            {
                // 単独
                            ("10000", "1"),
                            ("01000", "5"),
                            ("00100", "6"),
                            ("00010", "B"),
                            ("00001", "C"),
                // 2併
                            ("11000", "2"),
                            ("10100", "3"),
                            ("10010", "E"),
                            ("10001", "G"),
                            ("01100", "7"),
                            ("01010", "H"),
                            ("01001", "I"),
                            ("00110", "J"),
                            ("00101", "K"),
                // 3併
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
                // 4併
                            ("11110", "V"),
                            ("11101", "W"),
                            ("11011", "X"),
                            ("10111", "Y"),
                            ("01111", "Z"),

                            ("11111", "9"),
            };
    }
}
