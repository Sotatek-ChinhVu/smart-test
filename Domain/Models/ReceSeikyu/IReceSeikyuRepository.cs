namespace Domain.Models.ReceSeikyu
{
    public interface IReceSeikyuRepository
    {
        List<ReceSeikyuModel> GetListReceSeikyModel(int hpId, int sinDate, int sinYm);
    }
}
