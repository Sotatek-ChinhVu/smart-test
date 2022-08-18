using Domain.Models.SetKbnMst;
using Helper.Extendsions;

namespace UseCase.SetKbnMst.GetList
{
    public class GetSetKbnMstListOutputItem : ObservableObject
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

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set => Set(ref _isSelected, value);
        }

        private int _setKbnEdaLastSelected;
        public int SetKbnEdaLastSelected
        {
            get => _setKbnEdaLastSelected;
            set => Set(ref _setKbnEdaLastSelected, value);
        }

        private int _sortNo;
        public int SortNo
        {
            get => _sortNo;
            set => Set(ref _sortNo, value);
        }
    }
}
