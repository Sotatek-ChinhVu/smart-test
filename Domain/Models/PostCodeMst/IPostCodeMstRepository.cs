namespace Domain.Models.PostCodeMst
{
    public interface IPostCodeMstRepository
    {
        public List<PostCodeMstModel> PostCodeMstModels(int hpId, string postCode1, string postCode2, string address, int pageIndex, int pageSize);
    }
}
