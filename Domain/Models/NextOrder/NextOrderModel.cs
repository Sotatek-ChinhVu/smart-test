using static Helper.Constants.NextOrderConst;

namespace Domain.Models.NextOrder
{
    public class NextOrderModel
    {
        public NextOrderModel(int hpId, long ptId, long rsvkrtNo, int rsvkrtKbn, int rsvDate, string rsvName, int isDeleted, int sortNo, List<RsvkrtByomeiModel> rsvkrtByomeis, RsvkrtKarteInfModel rsvkrtKarteInf, List<RsvkrtOrderInfModel> rsvkrtOrderInfs, FileItemModel fileItem)
        {
            HpId = hpId;
            PtId = ptId;
            RsvkrtNo = rsvkrtNo;
            RsvkrtKbn = rsvkrtKbn;
            RsvDate = rsvDate;
            RsvName = rsvName;
            IsDeleted = isDeleted;
            SortNo = sortNo;
            RsvkrtByomeis = rsvkrtByomeis;
            RsvkrtKarteInf = rsvkrtKarteInf;
            RsvkrtOrderInfs = rsvkrtOrderInfs;
            FileItem = fileItem;
        }
        public NextOrderStatus Validation()
        {
            if (RsvkrtNo < 0)
            {
                return NextOrderStatus.InvalidRsvkrtNo;
            }
            if (RsvkrtKbn != 0 && RsvkrtKbn != 1)
            {
                return NextOrderStatus.InvalidRsvkrtKbn;
            }
            if (RsvDate <= 0)
            {
                return NextOrderStatus.InvalidRsvDate;
            }
            if (RsvName.Length > 120)
            {
                return NextOrderStatus.InvalidRsvName;
            }
            if (IsDeleted < 0)
            {
                return NextOrderStatus.InvalidIsDeleted;
            }
            if (SortNo < 0)
            {
                return NextOrderStatus.InvalidSortNo;
            }

            return NextOrderStatus.Valid;
        }
        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long RsvkrtNo { get; private set; }

        public int RsvkrtKbn { get; private set; }

        public int RsvDate { get; private set; }

        public string RsvName { get; private set; }

        public int IsDeleted { get; private set; }

        public int SortNo { get; private set; }

        public List<RsvkrtByomeiModel> RsvkrtByomeis { get; private set; }

        public RsvkrtKarteInfModel RsvkrtKarteInf { get; private set; }

        public List<RsvkrtOrderInfModel> RsvkrtOrderInfs { get; private set; }

        public FileItemModel FileItem { get; private set; }
    }
}
