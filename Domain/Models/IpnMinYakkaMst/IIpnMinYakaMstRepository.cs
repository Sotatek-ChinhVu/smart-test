namespace Domain.Models.IpnMinYakkaMst
{
    public interface IIpnMinYakaMstRepository
    {
        IpnMinYakkaMstModel? FindIpnMinYakkaMst(int hpId, string ipnNameCd, int sinDate);
    }
}
