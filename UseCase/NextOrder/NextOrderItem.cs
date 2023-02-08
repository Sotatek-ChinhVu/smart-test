using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.NextOrder
{
    public class NextOrderItem
    {
        public NextOrderItem(long rsvkrtNo, int rsvkrtKbn, int rsvDate, string rsvName, int sortNo, int isDeleted, List<RsvKrtByomeiItem> rsvKrtByomeiItems, RsvKrtKarteInfItem rsvkrtKarteInf, List<RsvKrtOrderInfItem> rsvKrtOrderInfItems, FileItemInputItem fileItem)
        {
            RsvkrtNo = rsvkrtNo;
            RsvkrtKbn = rsvkrtKbn;
            RsvDate = rsvDate;
            RsvName = rsvName;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            RsvKrtByomeiItems = rsvKrtByomeiItems;
            RsvkrtKarteInf = rsvkrtKarteInf;
            RsvKrtOrderInfItems = rsvKrtOrderInfItems;
            FileItem = fileItem;
        }

        public long RsvkrtNo { get; private set; }

        public int RsvkrtKbn { get; private set; }

        public int RsvDate { get; private set; }

        public string RsvName { get; private set; }

        public int SortNo { get; private set; }

        public int IsDeleted { get; private set; }

        public List<RsvKrtByomeiItem> RsvKrtByomeiItems { get; private set; }

        public RsvKrtKarteInfItem RsvkrtKarteInf { get; private set; }

        public List<RsvKrtOrderInfItem> RsvKrtOrderInfItems { get; private set; }

        public FileItemInputItem FileItem { get; private set; }
    }
}
