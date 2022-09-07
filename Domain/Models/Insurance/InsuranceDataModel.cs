using Domain.Models.InsuranceInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class InsuranceDataModel
    {
        public InsuranceDataModel(List<InsuranceModel> listInsurance, List<HokenInfModel> listHokenInf, List<KohiInfModel> listKohi)
        {
            ListInsurance = listInsurance;
            ListHokenInf = listHokenInf;
            ListKohi = listKohi;
        }
        
        public InsuranceDataModel()
        {
            ListInsurance = new List<InsuranceModel>();
            ListHokenInf = new List<HokenInfModel>();
            ListKohi = new List<KohiInfModel>();
        }

        public List<InsuranceModel> ListInsurance { get; private set; }

        public List<HokenInfModel> ListHokenInf { get; private set; }

        public List<KohiInfModel> ListKohi { get; private set; }
    }
}
