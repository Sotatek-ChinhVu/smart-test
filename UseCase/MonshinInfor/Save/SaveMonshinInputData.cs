using Domain.Models.MonshinInf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;
using UseCase.MonshinInfor.Insert;

namespace UseCase.MonshinInfor.Save
{
    public class SaveMonshinInputData : IInputData<SaveMonshinOutputData>
    {
        public SaveMonshinInputData(List<MonshinInforModel> monshinInfors, int hpId, long ptId, long raiinNo, int sinDate)
        {
            MonshinInfors = monshinInfors;
            HpId = hpId;
            PtId = ptId;
            RaiinNo = raiinNo;
            SinDate = sinDate;
        }

        public List<MonshinInforModel> MonshinInfors { get; set; }
        public int HpId { get; set; }
        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }
    }
}