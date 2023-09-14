using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class UpdateByomeiMstModel
    {
        public UpdateByomeiMstModel(string byomeiCd, string kanaName2, int sikkanCd, int nanbyoCd, bool isAdopted)
        {
            ByomeiCd = byomeiCd;
            KanaName2 = kanaName2;
            SikkanCd = sikkanCd;
            NanbyoCd = nanbyoCd;
            IsAdopted = isAdopted;
        }

        public string ByomeiCd { get; private set; }
        public string KanaName2 { get; private set; }
        public int SikkanCd { get; private set; }
        public int NanbyoCd { get; private set; }
        public bool IsAdopted { get; private set; }
    }
}
