using Entity.Tenant;

namespace CommonChecker.Models.Futan
{
    public class SanteiGrpDetailModel
    {
        public SanteiGrpDetail SanteiGrpDetail { get; }

        public SanteiGrpDetailModel(SanteiGrpDetail santeiGrpDetail)
        {
            SanteiGrpDetail = santeiGrpDetail;
        }

        public int SanteiGroupCd => SanteiGrpDetail.SanteiGrpCd;
    }
}
