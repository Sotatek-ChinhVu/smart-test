using UseCase.MedicalExamination.GetHistory;
using UseCase.OrdInfs.GetListTrees;

namespace DevExpress.Models
{
    public class Karte2ExportModel
    {
        public int HpId { get; set; }

        public long PtId { get; set; }
        public long RaiinNo { get; set; }
        public int SinDate { get; set; }

        public string KanaName { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Sex { get; set; } = string.Empty;

        public string Birthday { get; set; } = string.Empty;

        public string CurrentTime { get; set; } = string.Empty;

        public string StartDate { get; set; } = string.Empty;

        public string EndDate { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public List<RichTextKarte2Model> RichTextKarte2Models { get; set; } = new List<RichTextKarte2Model>();

    }
    public class RichTextKarte2Model
    {
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public string RichText { get; set; } = string.Empty;
        public List<GroupNameKarte2Model> GroupNameKarte2Models { get; set; } = new List<GroupNameKarte2Model>();
    }
    public class GroupNameKarte2Model
    {
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public List<RpNameKarte2Model> RpNameKarte2Models { get; set; } = new List<RpNameKarte2Model>();
        public string GroupName { get; set; } = string.Empty;
    }
    public class RpNameKarte2Model 
    {
        public string RpName { get; set; } = string.Empty;
        public List<ItemNameKarte2Model> ItemNameKarte2Models { get; set; } = new List<ItemNameKarte2Model>();
    }
    public class ItemNameKarte2Model
    {
        public string ItemName { get; set; } = string.Empty;
    }

}

