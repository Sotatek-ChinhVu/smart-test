using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.KensaSet
{
    public class KensaSetUpdateModel
    {
        public int HpId { get; private set; }

        public int SetId { get; private set; }

        public int SortNo { get; private set; }
        public string SetName { get; private set; }


        public int IsDeleted { get; private set; }
    }
}
