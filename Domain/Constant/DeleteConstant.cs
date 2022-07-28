using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constant
{
    public static class DeleteStatus
    {
        public const int None = 0;
        public const int DeleteFlag = 1;
        public const int EditFlag = 2;
        public const int CreateFlag = 3;
    }
}
