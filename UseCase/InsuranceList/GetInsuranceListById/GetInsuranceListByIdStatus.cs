using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.InsuranceList.GetInsuranceListById
{
    public enum GetInsuranceListByIdStatus: byte
    {
        InvalidId = 0,
        Successed = 1,
        DataNotExist = 2
    }
}
