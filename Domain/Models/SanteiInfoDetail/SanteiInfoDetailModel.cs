using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SanteiInfo
{
    public class SanteiInfoDetailModel
    {
        public SanteiInfoDetailModel(int hpId, long ptId, string itemCd, int seqNo, int endDate, int kisanSbt, int kisanDate, string byomei, string hosokuComment, string comment, int isDeleted, long id)
        {
            HpId = hpId;
            PtId = ptId;
            ItemCd = itemCd;
            SeqNo = seqNo;
            EndDate = endDate;
            KisanSbt = kisanSbt;
            KisanDate = kisanDate;
            Byomei = byomei;
            HosokuComment = hosokuComment;
            Comment = comment;
            IsDeleted = isDeleted;
            Id = id;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public string ItemCd { get; private set; }
        public int SeqNo { get; private set; }
        public int EndDate { get; private set; }
        public int KisanSbt { get; private set; }
        public int KisanDate { get; private set; }
        public string Byomei { get; private set; }
        public string HosokuComment { get; private set; }
        public string Comment { get; private set; }
        public int IsDeleted { get; private set; }
        public long Id { get; private set; }
    }
}
