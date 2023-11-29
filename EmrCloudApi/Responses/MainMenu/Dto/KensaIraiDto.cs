using Domain.Models.KensaIrai;
using Helper.Common;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class KensaIraiDto
{
    public KensaIraiDto(KensaIraiModel model)
    {
        SinDate = model.SinDate;
        RaiinNo = model.RaiinNo;
        IraiCd = model.IraiCd;
        PtId = model.PtId;
        PtNum = model.PtNum;
        Name = model.Name;
        KanaName = model.KanaName;
        Sex = model.Sex;
        Birthday = model.Birthday;
        TosekiKbn = model.TosekiKbn;
        SikyuKbn = model.SikyuKbn;
        KaId = model.KaId;
        UpdateDate = model.UpdateDate;
        KensaIraiDetails = model.KensaIraiDetails.Select(item => new KensaIraiDetailDto(item)).ToList();
    }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long IraiCd { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public string Name { get; private set; }

    public string KanaName { get; private set; }

    public int Sex { get; private set; }

    public int Birthday { get; private set; }

    public int TosekiKbn { get; private set; }

    public int SikyuKbn { get; private set; }

    public int KaId { get; private set; }

    public DateTime UpdateDate { get; private set; }

    public List<KensaIraiDetailDto> KensaIraiDetails { get; private set; }

    public int Age
    {
        get { return CIUtil.SDateToAge(Birthday, SinDate); }
    }

    public string TosekiStr
    {
        get
        {
            string ret = "";

            switch (TosekiKbn)
            {
                case 1:
                    ret = "前";
                    break;
                case 2:
                    ret = "後";
                    break;
            }

            return ret;
        }
    }

    public string SikyuStr
    {
        get
        {
            string ret = "";

            if (SikyuKbn == 1)
            {
                ret = "●";
            }

            return ret;
        }
    }
}
