using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.MstItem
{
    public class ItemGrpMstModel
    {
        public ItemGrpMstModel(int hpId, long grpSbt, long itemGrpCd, int startDate, int endDate, string itemCd, int seqNo)
        {
            HpId = hpId;
            GrpSbt = grpSbt;
            ItemGrpCd = itemGrpCd;
            StartDate = startDate;
            EndDate = endDate;
            ItemCd = itemCd;
            SeqNo = seqNo;
        }

        public int HpId { get; private set; }

        public long GrpSbt { get; private set; }

        public long ItemGrpCd { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public string ItemCd { get; private set; }

        public int SeqNo { get; private set; }
    }
}
