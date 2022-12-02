using System.Globalization;

namespace Domain.Models.Reception;

public class ReceptionSaveDto
{
    public ReceptionSaveDto(ReceptionModel reception, string receptionComment,
        List<RaiinKbnInfDto> kubunInfs, List<InsuranceDto> insurances, List<DiseaseDto> diseases)
    {
        Reception = reception;
        ReceptionComment = receptionComment;
        KubunInfs = kubunInfs;
        Insurances = insurances;
        Diseases = diseases;
    }

    public ReceptionModel Reception { get; private set; }
    public string ReceptionComment { get; private set; }
    public List<RaiinKbnInfDto> KubunInfs { get; private set; }
    public List<InsuranceDto> Insurances { get; private set; }
    public List<DiseaseDto> Diseases { get; private set; }

    public ReceptionSaveDto ChangeUketukeNo(int uketukeNo)
    {
        Reception.ChangeUketukeNo(uketukeNo);
        return this;
    }
}

public class InsuranceDto
{
    public InsuranceDto(int hokenId, List<ConfirmDateDto> confirmDateList)
    {
        HokenId = hokenId;
        ConfirmDateList = confirmDateList;
    }

    public int HokenId { get; private set; }

    /// <summary>
    /// ConfirmDate template: yyyyMMdd
    /// </summary>
    public List<ConfirmDateDto> ConfirmDateList { get; private set; }

    public bool IsValidData()
    {
        if (ConfirmDateList.Any(c => c.SinDate.ToString().Length != 8))
        {
            return false;
        }

        //Check for duplicate
        if (ConfirmDateList.Count != ConfirmDateList.Select(item => item.SinDate).Distinct().Count())
        {
            return false;
        }

        foreach (var confirmDate in ConfirmDateList)
        {
            if (!DateTime.TryParseExact(confirmDate.SinDate.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return false;
            }
        }
        return true;
    }
}

public class DiseaseDto
{
    public DiseaseDto(long id, int tenkiKbn, int tenkiDate)
    {
        Id = id;
        TenkiKbn = tenkiKbn;
        TenkiDate = tenkiDate;
    }

    public long Id { get; private set; }
    public int TenkiKbn { get; private set; }
    public int TenkiDate { get; private set; }
}

public class RaiinKbnInfDto
{
    public RaiinKbnInfDto(int grpId, int kbnCd)
    {
        GrpId = grpId;
        KbnCd = kbnCd;
    }

    public int GrpId { get; private set; }
    public int KbnCd { get; private set; }
}
