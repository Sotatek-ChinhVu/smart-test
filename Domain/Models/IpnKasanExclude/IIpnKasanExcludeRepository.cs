using Domain.Models.InputItem;

namespace Domain.Models.IpnKasanExcludeItem
{
    public interface IIpnKasanExcludeRepository
    {
        bool CheckIsGetYakkaPrice(int hpId, InputItemModel? tenMst, int sinDate);
    }
}
