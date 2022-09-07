namespace Domain.Models.Byomei;

public interface IByomeiRepository
{
    List<ByomeiMstModel> DiseaseSearch(bool isSyusyoku, string keyword, int pageIndex, int pageCount);
}
