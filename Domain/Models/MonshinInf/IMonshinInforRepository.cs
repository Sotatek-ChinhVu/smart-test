namespace Domain.Models.MonshinInf
{
    public interface IMonshinInforRepository
    {
        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId, int sinDate, bool isDeleted);
    }
}
 