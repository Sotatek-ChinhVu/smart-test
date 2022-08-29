using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.DrugDetail
{
    public class DrugDetailModel
    {
        public DrugDetailModel(int isProductInf, int maxLevel, string drugInfName, SyohinModel syohinInf, List<DrugMenuItemModel> kikakuCollection, List<DrugMenuItemModel> tenpuCollection, int isKanjaMuke, YakuModel yakuInf, List<FukuModel> fukuInf, SyokiModel syokiInf, List<SougoModel> sougoInf, List<ChuiModel> chuiInf, int isMdbByomei, List<TenMstByomeiModel> tenMstInf)
        {
            IsProductInf = isProductInf;
            MaxLevel = maxLevel;
            DrugInfName = drugInfName;
            SyohinInf = syohinInf;
            KikakuCollection = kikakuCollection;
            TenpuCollection = tenpuCollection;
            IsKanjaMuke = isKanjaMuke;
            YakuInf = yakuInf;
            FukuInf = fukuInf;
            SyokiInf = syokiInf;
            SougoInf = sougoInf;
            ChuiInf = chuiInf;
            IsMdbByomei = isMdbByomei;
            TenMstInf = tenMstInf;
        }

        public DrugDetailModel()
        {
            IsProductInf = 0;
            MaxLevel = 0;
            DrugInfName = "";
            SyohinInf = new SyohinModel();
            KikakuCollection = new List<DrugMenuItemModel>();
            TenpuCollection = new List<DrugMenuItemModel>();
            IsKanjaMuke = 0;
            YakuInf = new YakuModel();
            FukuInf = new List<FukuModel>();
            SyokiInf = new SyokiModel();
            SougoInf = new List<SougoModel>();
            ChuiInf = new List<ChuiModel>();
            IsMdbByomei = 0;
            TenMstInf = new List<TenMstByomeiModel>();
        }

        public int IsProductInf { get; private set; }

        public int MaxLevel { get; private set; }

        public string DrugInfName { get; private set; }

        public SyohinModel SyohinInf { get; private set; }

        public List<DrugMenuItemModel> KikakuCollection { get; private set; }

        public List<DrugMenuItemModel> TenpuCollection { get; set; }

        public int IsKanjaMuke { get; private set; }

        public YakuModel YakuInf { get; private set; }

        public List<FukuModel> FukuInf { get; private set; }

        public SyokiModel SyokiInf { get; private set; }

        public List<SougoModel> SougoInf { get; private set; }

        public List<ChuiModel> ChuiInf { get; private set; }

        public int IsMdbByomei { get; private set; }

        public List<TenMstByomeiModel> TenMstInf { get; private set; }
    }
}
