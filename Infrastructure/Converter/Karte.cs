using Domain.Models.KarteInfs;
using Entity.Tenant;
using System.Text;

namespace Infrastructure.Converter
{
    internal class Karte
    {
        protected Karte()
        {

        }

        public static KarteInfModel FromKarte(KarteInf karteInf, string updateName)
        {
            return new KarteInfModel(
                karteInf.HpId,
                karteInf.RaiinNo,
                karteInf.KarteKbn,
                karteInf.SeqNo,
                karteInf.PtId,
                karteInf.SinDate,
                karteInf.Text ?? string.Empty,
                karteInf.IsDeleted,
                karteInf.RichText == null ? string.Empty : Encoding.UTF8.GetString(karteInf.RichText),
                karteInf.CreateDate,
                karteInf.UpdateDate,
                updateName
                );
        }
    }
}
