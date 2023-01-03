namespace UseCase.RaiinKbn.GetPatientRaiinKubunList
{
    public class GetPatientRaiinKubunDto
    {
        public GetPatientRaiinKubunDto(int hpId, int groupId, int kbnCd, int sortNo)
        {
            HpId = hpId;
            GroupId = groupId;
            KbnCd = kbnCd;
            SortNo = sortNo;
        }

        public int HpId { get; private set; }

        public int GroupId { get; private set; }

        public int KbnCd { get; private set; }

        public int SortNo { get; private set; }
    }
}
