namespace Domain.Models.RaiinKubunMst
{
    public class RaiinKbnModel
    {
        public RaiinKbnModel(int hpId, int grpCd, int sortNo, string grpName, int isDeleted, RaiinKbnInfModel raiinKbnInfModel, List<RaiinKbnDetailModel> raiinKbnDetailModels)
        {
            HpId = hpId;
            GrpCd = grpCd;
            SortNo = sortNo;
            GrpName = grpName;
            IsDeleted = isDeleted;
            RaiinKbnInfModel = raiinKbnInfModel;
            RaiinKbnDetailModels = raiinKbnDetailModels;
        }

        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public int SortNo { get; private set; }

        public string GrpName { get; private set; }

        public int IsDeleted { get; private set; }

        public RaiinKbnInfModel RaiinKbnInfModel { get; private set; }

        public List<RaiinKbnDetailModel> RaiinKbnDetailModels { get; private set; }
    }
}
