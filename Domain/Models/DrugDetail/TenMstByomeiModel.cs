using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class TenMstByomeiModel
    {
        public TenMstByomeiModel(string itemCd, string byomei)
        {
            ItemCd = itemCd;
            Byomei = byomei;
        }

        public TenMstByomeiModel()
        {
            ItemCd = "";
            Byomei = "";
        }

        public string ItemCd { get; set; }

        public string Byomei { get; private set; }
    }
}
