using Entity.Tenant;
using Helper.Common;
using Reporting.Calculate.Receipt.ViewModels;

namespace Reporting.AccountingCardList.Model;

public class CoAccountingCardListModel
{
    List<CoKaikeiInfModel> KaikeiInfModels { get; }
    CoPtInfModel PtInf { get; }
    public SinMeiViewModel SinMeiVM { get; }
    public List<CoPtByomeiModel> PtByomeis { get; }

    public CoAccountingCardListModel(
        int sinYm, CoPtInfModel ptInfModel, List<CoKaikeiInfModel> kaikeiInfModels, SinMeiViewModel sinMeiViewModel, List<CoPtByomeiModel> ptByomeiModels)
    {
        SinYm = sinYm;
        PtInf = ptInfModel;
        KaikeiInfModels = kaikeiInfModels;
        SinMeiVM = sinMeiViewModel;
        PtByomeis = ptByomeiModels;
    }

    public int SinYm { get; } = 0;

    public string Name
    {
        get { return PtInf.Name; }
    }

    public long PtId
    {
        get { return PtInf.PtId; }
    }

    public long PtNum
    {
        get { return PtInf.PtNum; }
    }
    public int Birthday
    {
        get { return PtInf.Birthday; }
    }
    public int Age
    {
        get
        {
            return CIUtil.SDateToAge(Birthday, CIUtil.GetLastDateOfMonth(SinYm * 100 + 1));
        }
    }
    public int Nissu
    {
        get
        {
            int ret = 0;

            var groupsums = (
                from kaikeiInf in KaikeiInfModels
                group kaikeiInf by kaikeiInf.SinDate into A
                select new { A.Key, sum = A.Sum(a => a.Nissu) }
                ).ToList();

            foreach (var groupsum in groupsums)
            {
                if (groupsum.sum > 0)
                {
                    ret++;
                }
            }

            return ret;
        }
    }
}
