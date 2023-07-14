using Domain.Models.FlowSheet;

namespace EmrCloudApi.Responses.FlowSheet.GetListFlowSheetDto
{
    public class FlowSheetDto
    {
        public FlowSheetDto(FlowSheetModel model)
        {
            SinDate = model.SinDate;
            TagNo = model.TagNo;
            Karte = model.FullLineOfKarte;
            RaiinNo = model.RaiinNo;
            SyoKbn = model.SyosaisinKbn;
            Cmt = model.Comment;
            Status = model.Status;
            IsConFile = model.IsContainsFile;
            IsNextOdr = model.IsNextOrder;
            IsTodayOdr = model.IsToDayOdr;
            IsNotSaved = model.IsNotSaved;
        }

        public bool IsNext
        {
            get
            {
                return SyoKbn < 0;
            }
        }


        public int SinDate { get; private set; }

        public int TagNo { get; private set; }

        public string Karte { get; private set; }

        public long RaiinNo { get; private set; }

        public int SyoKbn { get; private set; }

        public string Cmt { get; private set; }

        public int Status { get; private set; }

        public bool IsConFile { get; private set; }

        public bool IsNextOdr { get; private set; }

        public bool IsTodayOdr { get; private set; }

        public bool IsNotSaved { get; private set; }
    }
}
