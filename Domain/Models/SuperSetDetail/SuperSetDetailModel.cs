namespace Domain.Models.SuperSetDetail;

public class SuperSetDetailModel
{
    public SuperSetDetailModel(List<SetByomeiModel> setByomeiList, List<SetKarteInfModel> setKarteInfList)
    {
        SetByomeiList = setByomeiList;
        SetKarteInfList = setKarteInfList;
    }

    public List<SetByomeiModel> SetByomeiList { get; private set; }

    public List<SetKarteInfModel> SetKarteInfList { get; private set; }

}
