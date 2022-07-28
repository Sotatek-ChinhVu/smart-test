using UseCase.Core.Sync.Core;

namespace UseCase.SetKbnMst.GetList
{
    public class GetSetKbnMstListInputData : IInputData<GetSetKbnMstListOutputData>
    {
        public GetSetKbnMstListInputData(int hpId, int sinDate, int setKbnFrom, int setKbnTo)
        {
            HpId = hpId;
            SinDate = sinDate;
            SetKbnFrom = setKbnFrom;
            SetKbnTo = setKbnTo;
        }

        public int HpId { get; set; }
        public int SinDate { get; set; }
        public int SetKbnFrom { get; set; }
        public int SetKbnTo { get; set; }
    }
}