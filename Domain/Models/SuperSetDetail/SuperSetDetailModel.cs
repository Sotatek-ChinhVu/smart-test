namespace Domain.Models.SuperSetDetail;

public class SuperSetDetailModel
{
    public SuperSetDetailModel(List<SetByomeiModel> setByomeiList, SetKarteInfModel setKarteInfList)
    {
        SetByomeiList = setByomeiList;
        SetKarteInfList = setKarteInfList;
    }

    public List<SetByomeiModel> SetByomeiList { get; private set; }

    public SetKarteInfModel SetKarteInfList { get; private set; }

}
