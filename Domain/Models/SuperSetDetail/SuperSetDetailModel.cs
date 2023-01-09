namespace Domain.Models.SuperSetDetail;

public class SuperSetDetailModel
{
    public SuperSetDetailModel(List<SetByomeiModel> setByomeiList, SetKarteInfModel setKarteInf, List<SetGroupOrderInfModel> setGroupOrderInfList, List<SetFileInfModel> setKarteFileModelList)
    {
        SetByomeiList = setByomeiList;
        SetKarteInf = setKarteInf;
        SetGroupOrderInfList = setGroupOrderInfList;
        SetKarteFileModelList = setKarteFileModelList;
    }

    public List<SetByomeiModel> SetByomeiList { get; private set; }

    public SetKarteInfModel SetKarteInf { get; private set; }

    public List<SetGroupOrderInfModel> SetGroupOrderInfList { get; private set; }

    public List<SetFileInfModel> SetKarteFileModelList { get; private set; }

}
