using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    public class KazeiKbnConst
    {
        // 非課税
        public const int Hikazei = 0;
        // 課税対象
        public const int Kazei = 1;
        // 軽減税率対象
        public const int Keigen = 2;
        // 内税
        public const int Uchizei = 3;
        // 内税軽減税率対象
        public const int UchiKeigenzei = 4;
    }
}
