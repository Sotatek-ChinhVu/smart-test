using Entity.Tenant;
using Helper.Constants;

namespace Reporting.DailyStatic.Model;

public class StatisticModelBase
{
    public StaMenu StaMenu { get; set; }

    public List<StaConf> ListStaConf { get; set; }

    protected void SettingConfig(int hpId, int configId, string value)
    {
        var conf = ListStaConf.Find(x => x.HpId == hpId && x.ConfId == configId);

        if (conf == null)
        {
            conf = new StaConf();
            conf.HpId = hpId;
            conf.MenuId = StaMenu.MenuId;
            conf.ConfId = configId;
            
            ListStaConf.Add(conf);
            if (ModelStatus == ModelStatus.None)
            {
                ModelStatus = ModelStatus.Modified;
            }
        }
        else
        {

            if (conf.Val != value && ModelStatus == ModelStatus.None)
            {
                ModelStatus = ModelStatus.Modified;
            }
        }

        conf.Val = value;
    }

    protected string GetValueConf(int hpId, int config)
    {
        var conf = ListStaConf.Find(x => x.HpId == hpId && x.ConfId == config);

        if (conf == null)
        {
            return string.Empty;
        }

        return conf.Val ?? string.Empty;
    }

    public ModelStatus ModelStatus { get; set; }
}
