using Domain.Models.Futan;
using Domain.Models.MstItem;

namespace Domain.Models.Finder
{
    public interface IMasterFinder
    {
        SanteiGrpDetailModel FindSanteiGrpDetail(string itemCd);
        SanteiCntCheckModel FindSanteiCntCheck(int santeiGrpCd, int sinDate);
        double GetOdrCountInMonth(long ptId, int sinDate, string itemCd);
        TenMstModel FindTenMst(string itemCd, int sinDate);
        IpnNameMstModel FindIpnNameMst(string ipnNameCd, int sinDate);
    }
}
