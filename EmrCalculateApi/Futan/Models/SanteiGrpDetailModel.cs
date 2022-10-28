using Entity.Tenant;

namespace EmrCalculateApi.Futan.Models
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
