using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class ValidInsuranceOtherModel
    {
        public ValidInsuranceOtherModel(int sindate, int ptBirthday, bool isSelectedHokenInfEmptyModel, bool selectedHokenInfIsShahoOrKokuho, List<HokenInfor> listHokenInf, List<HokenPatternsModel> listHokenPattern)
        {
            Sindate = sindate;
            PtBirthday = ptBirthday;
            IsSelectedHokenInfEmptyModel = isSelectedHokenInfEmptyModel;
            SelectedHokenInfIsShahoOrKokuho = selectedHokenInfIsShahoOrKokuho;
            ListHokenInf = listHokenInf;
            ListHokenPattern = listHokenPattern;
        }

        public int Sindate { get; private set; }

        public int PtBirthday { get; private set; }

        public bool IsSelectedHokenInfEmptyModel { get; private set; }

        public bool SelectedHokenInfIsShahoOrKokuho { get; private set; }

        public List<HokenInfor> ListHokenInf { get; private set; }

        public List<HokenPatternsModel> ListHokenPattern { get; private set; }
    }
    public class HokenInfor
    {
        public HokenInfor(int endDate, string hokensyaNo, int hokenId, int isDeleted, bool isExpirated)
        {
            EndDate = endDate;
            HokensyaNo = hokensyaNo;
            HokenId = hokenId;
            IsDeleted = isDeleted;
            IsExpirated = isExpirated;
        }

        public int EndDate { get; private set; }

        public string HokensyaNo { get; private set; }

        public int HokenId { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsExpirated { get; private set; }
    }

    public class HokenPatternsModel
    {
        public HokenPatternsModel(int hokenId, int hokenKbn, bool isExpirated, int isDeleted, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id)
        {
            HokenId = hokenId;
            HokenKbn = hokenKbn;
            IsExpirated = isExpirated;
            IsDeleted = isDeleted;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
        }

        public int HokenId { get; private set; }

        public int HokenKbn { get; private set; }

        public bool IsExpirated { get; private set; }

        public int IsDeleted { get; private set; }

        public int Kohi1Id { get; private set; }

        public int Kohi2Id { get; private set; }

        public int Kohi3Id { get; private set; }

        public int Kohi4Id { get; private set; }
    }
}
