using Domain.Models.SuperSetDetail;

namespace UseCase.SuperSetDetail.GetSuperSetDetail
{
    public class SuperSetDetailItem
    {
        public SuperSetDetailItem(List<SetByomeiItem> setByomeiList, SetKarteInfModel setKarteInf, List<SetGroupOrderInfItem> setGroupOrderInfList)
        {
            SetByomeiList = setByomeiList;
            SetKarteInf = setKarteInf;
            SetGroupOrderInfList = setGroupOrderInfList;
        }

        public List<SetByomeiItem> SetByomeiList { get; private set; }

        public SetKarteInfModel SetKarteInf { get; private set; }

        public List<SetGroupOrderInfItem> SetGroupOrderInfList { get; private set; }
    }
}
