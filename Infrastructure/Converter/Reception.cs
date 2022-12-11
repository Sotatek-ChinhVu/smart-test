using Domain.Models.Reception;
using Entity.Tenant;

namespace Infrastructure.Converter
{
    internal class Reception
    {
        protected Reception()
        {
        }

        public static ReceptionModel FromRaiinInf(RaiinInf raiinInf)
        {
            return new ReceptionModel(
                raiinInf.HpId,
                raiinInf.PtId,
                raiinInf.SinDate,
                raiinInf.RaiinNo,
                raiinInf.OyaRaiinNo,
                raiinInf.HokenPid,
                raiinInf.SanteiKbn,
                raiinInf.Status,
                raiinInf.IsYoyaku,
                raiinInf.YoyakuTime ?? string.Empty,
                raiinInf.YoyakuId,
                raiinInf.UketukeSbt,
                raiinInf.UketukeTime ?? string.Empty,
                raiinInf.UketukeId,
                raiinInf.UketukeNo,
                raiinInf.SinStartTime ?? string.Empty,
                raiinInf.SinEndTime ?? string.Empty,
                raiinInf.KaikeiTime ?? string.Empty,
                raiinInf.KaikeiId,
                raiinInf.KaId,
                raiinInf.TantoId,
                raiinInf.SyosaisinKbn,
                raiinInf.JikanKbn,
                string.Empty
                );
        }
    }
}
