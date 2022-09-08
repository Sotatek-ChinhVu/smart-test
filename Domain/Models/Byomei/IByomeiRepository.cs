namespace Domain.Models.Byomei;

public interface IByomeiRepository
{
    List<ByomeiMstModel> DiseaseSearch(bool isPrefix, bool isByomei, bool isSuffix, string keyword, int pageIndex, int pageCount);
}