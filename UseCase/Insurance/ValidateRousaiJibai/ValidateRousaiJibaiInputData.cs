using Domain.Models.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidateRousaiJibai
{
    public class ValidateRousaiJibaiInputData : IInputData<ValidateRousaiJibaiOutputData>
    {
        public ValidateRousaiJibaiInputData(int hpId, int hokenKbn, int sinDate, bool isSelectedHokenInf, string selectedHokenInfRodoBango, List<RousaiTenkiModel> listRousaiTenki, int selectedHokenInfRousaiSaigaiKbn, int selectedHokenInfRousaiSyobyoDate, string selectedHokenInfRousaiSyobyoCd, int selectedHokenInfRyoyoStartDate, int selectedHokenInfRyoyoEndDate, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsAddNew, string selectedHokenInfNenkinBango, string selectedHokenInfKenkoKanriBango, int selectedHokenInfConfirmDate)
        {
            HpId = hpId;
            HokenKbn = hokenKbn;
            SinDate = sinDate;
            IsSelectedHokenInf = isSelectedHokenInf;
            SelectedHokenInfRodoBango = selectedHokenInfRodoBango;
            ListRousaiTenki = listRousaiTenki;
            SelectedHokenInfRousaiSaigaiKbn = selectedHokenInfRousaiSaigaiKbn;
            SelectedHokenInfRousaiSyobyoDate = selectedHokenInfRousaiSyobyoDate;
            SelectedHokenInfRousaiSyobyoCd = selectedHokenInfRousaiSyobyoCd;
            SelectedHokenInfRyoyoStartDate = selectedHokenInfRyoyoStartDate;
            SelectedHokenInfRyoyoEndDate = selectedHokenInfRyoyoEndDate;
            SelectedHokenInfStartDate = selectedHokenInfStartDate;
            SelectedHokenInfEndDate = selectedHokenInfEndDate;
            SelectedHokenInfIsAddNew = selectedHokenInfIsAddNew;
            SelectedHokenInfNenkinBango = selectedHokenInfNenkinBango;
            SelectedHokenInfKenkoKanriBango = selectedHokenInfKenkoKanriBango;
            SelectedHokenInfConfirmDate = selectedHokenInfConfirmDate;
        }

        public int HpId { get; private set; }

        public int HokenKbn { get; private set; }

        public int SinDate { get; private set; }

        public bool IsSelectedHokenInf { get; private set; }

        public string SelectedHokenInfRodoBango { get; private set; }

        public List<RousaiTenkiModel> ListRousaiTenki { get; private set; }

        public int SelectedHokenInfRousaiSaigaiKbn { get; private set; }

        public int SelectedHokenInfRousaiSyobyoDate { get; private set; }

        public string SelectedHokenInfRousaiSyobyoCd { get; private set; }

        public int SelectedHokenInfRyoyoStartDate { get; private set; }

        public int SelectedHokenInfRyoyoEndDate { get; private set; }

        public int SelectedHokenInfStartDate { get; private set; }

        public int SelectedHokenInfEndDate { get; private set; }

        public bool SelectedHokenInfIsAddNew { get; private set; }

        public string SelectedHokenInfNenkinBango { get; private set; }

        public string SelectedHokenInfKenkoKanriBango { get; private set; }

        public int SelectedHokenInfConfirmDate { get; private set; }
    }
}
