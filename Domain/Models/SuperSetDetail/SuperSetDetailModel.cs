namespace Domain.Models.SuperSetDetail;

public class SuperSetDetailModel
{
    public SuperSetDetailModel(List<SetByomeiModel> setByomeiList, SetKarteInfModel setKarteInf)
    {
        SetByomeiList = setByomeiList;
        SetKarteInf = setKarteInf;
    }

    public List<SetByomeiModel> SetByomeiList { get; private set; }

    public SetKarteInfModel SetKarteInf{ get; private set; }

}
