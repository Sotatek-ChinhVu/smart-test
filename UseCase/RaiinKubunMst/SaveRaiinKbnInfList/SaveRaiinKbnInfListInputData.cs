using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKubunMst.SaveRaiinKbnInfList
{
    public class SaveRaiinKbnInfListInputData : IInputData<SaveRaiinKbnInfListOutputData>
    {
        public SaveRaiinKbnInfListInputData(int hpId, long ptId, int sinDate, long raiinNo, int userId, List<RaiinKbnInfDto> kbnInfDtos)
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            UserId = userId;
            KbnInfDtos = kbnInfDtos;
        }

        public int HpId { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public long RaiinNo { get; private set; }
        public int UserId { get; private set; }
        public List<RaiinKbnInfDto> KbnInfDtos { get; private set; }
    }
}
