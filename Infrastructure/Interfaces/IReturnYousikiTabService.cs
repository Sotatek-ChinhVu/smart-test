using Domain.Models.Yousiki;
using Domain.Models.Yousiki.CommonModel;

namespace Infrastructure.Interfaces;

public interface IReturnYousikiTabService
{
    TabYousikiModel RenderTabYousiki(Yousiki1InfModel yousiki1Inf, Dictionary<string, string> kacodeYousikiMstDict);
}
