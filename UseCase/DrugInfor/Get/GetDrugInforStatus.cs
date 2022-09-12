using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.DrugInfor.Get
{
    public enum GetDrugInforStatus: byte
    {
        Successed = 1,
        InValidHpId = 2,
        InValidSindate = 3,
        InValidItemCd = 4
    }
}
