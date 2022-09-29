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
        public HokenPatternsModel(int hokenId, bool isExpirated, int isDeleted)
        {
            HokenId = hokenId;
            IsExpirated = isExpirated;
            IsDeleted = isDeleted;
        }

        public int HokenId { get; private set; }

        public bool IsExpirated { get; private set; }

        public int IsDeleted { get; private set; }
    }
}
