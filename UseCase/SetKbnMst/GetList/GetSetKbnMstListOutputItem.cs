using Domain.Models.SetKbnMst;

namespace UseCase.SetKbnMst.GetList
{
    public class GetSetKbnMstListOutputItem
    {
        public SetKbnMstModel? SetKbnMst { get; } = null;

        public GetSetKbnMstListOutputItem(SetKbnMstModel setKbnMst)
        {
            SetKbnMst = setKbnMst;
        }

        public int SetKbn
        {
            get => SetKbnMst != null ? SetKbnMst.SetKbn : -1;
        }

        public int SetKbnEdaNo
        {
            get => SetKbnMst != null ? SetKbnMst.SetKbnEdaNo : -1;
        }

        public string SetKbnName
        {
            get => SetKbnMst != null ? SetKbnMst.SetKbnName : string.Empty;
        }

        public bool IsSelected { get; set; }

        public int SetKbnEdaLastSelected { get; set; }

        public int SortNo { get; set; }
    }
}
