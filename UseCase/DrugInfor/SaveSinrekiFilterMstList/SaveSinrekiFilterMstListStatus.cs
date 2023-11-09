namespace UseCase.DrugInfor.SaveSinrekiFilterMstList;

public enum SaveSinrekiFilterMstListStatus
{
    ValidateSuccess = 0,
    Successed = 1,
    Failed = 2,
    InvalidItemCd = 3,
    InvalidSinrekiFilterMstName = 4,
    InvalidSinrekiFilterMstGrpCd = 5,
    InvalidSinrekiFilterMstDetailId = 6,
    InvalidSinrekiFilterMstKouiKbnId = 7,
    InvalidSinrekiFilterMstKouiSeqNo = 8,
    InvalidSinrekiFilterMstDetaiDuplicateItemCd = 9,
}
