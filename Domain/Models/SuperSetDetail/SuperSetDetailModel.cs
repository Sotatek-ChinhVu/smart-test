namespace Domain.Models.SuperSetDetail;

public class SuperSetDetailModel
{
    public SuperSetDetailModel(List<SetByomeiModel> setByomeiList, SetKarteInfModel setKarteInf, List<SetOrdInfModel> setOrdInfList)
    {
        SetByomeiList = setByomeiList;
        SetKarteInf = setKarteInf;
        SetOrdInfList = setOrdInfList;
    }

    public List<SetByomeiModel> SetByomeiList { get; private set; }

    public SetKarteInfModel SetKarteInf { get; private set; }

    public List<SetOrdInfModel> SetOrdInfList { get; private set; }

}
