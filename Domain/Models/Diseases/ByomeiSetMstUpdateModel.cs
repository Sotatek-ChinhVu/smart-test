using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Diseases
{
    public class ByomeiSetMstUpdateModel
    {
        public int HpId { get; private set; }
        public int GenerationId { get; private set; }
        public int SeqNo { get; private set; } = 0;
        public string ByomeiCd { get; private set; }
        public string SetName { get; private set; }
        public string ItemCd { get; private set; }
        public int Level1 { get; private set; }
        public int Level2 { get; private set; }
        public int Level3 { get; private set; }
        public int Level4 { get; private set; }
        public int Level5 { get; private set; }
        public int IsDeleted { get; private set; }
        public int IsTitle { get; private set; }
        public int SelectType { get; private set; }

        public ByomeiSetMstUpdateModel(int hpId, int generationId, int seqNo, string byomeiCd, string setName, string itemCd, int level1, int level2, int level3, int level4, int level5, int isTitle, int selectType)
        {
            HpId = hpId;
            GenerationId = generationId;
            SeqNo = seqNo;
            ByomeiCd = byomeiCd;
            SetName = setName;
            ItemCd = itemCd;
            Level1 = level1;
            Level2 = level2;
            Level3 = level3;
            Level4 = level4;
            Level5 = level5;
            IsTitle = isTitle;
            SelectType = selectType;
        }
    }
}
