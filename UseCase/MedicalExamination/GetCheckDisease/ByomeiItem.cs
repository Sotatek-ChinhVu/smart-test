using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.MedicalExamination.GetCheckDisease
{
    public class ByomeiItem
    {
        public ByomeiItem(ByomeiMstModel byomeiMstModel)
        {
            ByomeiCd = byomeiMstModel.ByomeiCd;
            ByomeiType = byomeiMstModel.ByomeiType;
            Sbyomei = byomeiMstModel.Sbyomei;
            KanaName1 = byomeiMstModel.KanaName1;
            KanaName2 = byomeiMstModel.KanaName2;
            KanaName3 = byomeiMstModel.KanaName3;
            KanaName4 = byomeiMstModel.KanaName4;
            KanaName5 = byomeiMstModel.KanaName5;
            KanaName6 = byomeiMstModel.KanaName6;
            KanaName7 = byomeiMstModel.KanaName7;
            Sikkan = byomeiMstModel.Sikkan;
            NanByo = byomeiMstModel.NanByo;
            Icd10 = byomeiMstModel.Icd10;
            Icd102013 = byomeiMstModel.Icd102013;
            IsAdopted = byomeiMstModel.IsAdopted;
        }

        public string ByomeiCd { get; private set; }

        public string ByomeiType { get; private set; }

        public string Sbyomei { get; private set; }

        public string KanaName1 { get; private set; }

        public string KanaName2 { get; private set; }

        public string KanaName3 { get; private set; }

        public string KanaName4 { get; private set; }

        public string KanaName5 { get; private set; }

        public string KanaName6 { get; private set; }

        public string KanaName7 { get; private set; }

        public string Sikkan { get; private set; }

        public string NanByo { get; private set; }

        public string Icd10 { get; private set; }

        public string Icd102013 { get; private set; }

        public bool IsAdopted { get; private set; }
    }
}
