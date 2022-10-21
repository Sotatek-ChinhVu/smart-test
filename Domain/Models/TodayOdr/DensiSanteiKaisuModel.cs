using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.TodayOdr
{
    public class DensiSanteiKaisuModel
    {
        public DensiSanteiKaisuModel(int id, int hpId, string itemCd, int unitCd, int maxCount, int spJyoken, int startDate, int endDate, long seqNo, int userSetting, int targetKbn, int termCount, int termSbt, int isInvalid, int itemGrpCd)
        {
            Id = id;
            HpId = hpId;
            ItemCd = itemCd;
            UnitCd = unitCd;
            MaxCount = maxCount;
            SpJyoken = spJyoken;
            StartDate = startDate;
            EndDate = endDate;
            SeqNo = seqNo;
            UserSetting = userSetting;
            TargetKbn = targetKbn;
            TermCount = termCount;
            TermSbt = termSbt;
            IsInvalid = isInvalid;
            ItemGrpCd = itemGrpCd;
        }

        public int Id { get; private set; }

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }

        public int UnitCd { get; private set; }

        public int MaxCount { get; private set; }

        public int SpJyoken { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public long SeqNo { get; private set; }

        public int UserSetting { get; private set; }

        public int TargetKbn { get; private set; }

        public int TermCount { get; private set; }

        public int TermSbt { get; private set; }

        public int IsInvalid { get; private set; }

        public int ItemGrpCd { get; private set; }
    }
}
