using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidMainInsurance
{
    public class ValidMainInsuranceInputData : IInputData<ValidMainInsuranceOutputData>
    {
        public ValidMainInsuranceInputData(int hpId, int sinDate, int ptBirthday, int hokenKbn, string hokenSyaNo, bool isSelectedHokenPattern, bool isSelectedHokenInf, bool isSelectedHokenMst, string selectedHokenInfHoubetu, int selectedHokenInfHokenNo, int selectedHokenInfHokenEdraNo, bool selectedHokenInfIsAddNew, bool selectedHokenInfIsJihi, int selectedHokenInfStartDate, int selectedHokenInfEndDate, string selectedHokenInfKigo, string selectedHokenInfBango, int selectedHokenInfHonkeKbn, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfIsShahoOrKokuho, bool selectedHokenInfIsExpirated, bool selectedHokenInfIsIsNoHoken, int selectedHokenInfConfirmDate, bool selectedHokenInfIsAddHokenCheck, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, bool selectedHokenPatternIsEmptyKohi1, bool selectedHokenPatternIsEmptyKohi2, bool selectedHokenPatternIsEmptyKohi3, bool selectedHokenPatternIsEmptyKohi4, bool selectedHokenPatternIsExpirated, bool selectedHokenPatternIsEmptyHoken,bool selectedHokenPatternIsAddNew)
        {
            HpId = hpId;
            SinDate = sinDate;
            PtBirthday = ptBirthday;
            HokenKbn = hokenKbn;
            HokenSyaNo = hokenSyaNo;
            IsSelectedHokenPattern = isSelectedHokenPattern;
            IsSelectedHokenInf = isSelectedHokenInf;
            IsSelectedHokenMst = isSelectedHokenMst;
            SelectedHokenInfHoubetu = selectedHokenInfHoubetu;
            SelectedHokenInfHokenNo = selectedHokenInfHokenNo;
            SelectedHokenInfHokenEdraNo = selectedHokenInfHokenEdraNo;
            SelectedHokenInfIsAddNew = selectedHokenInfIsAddNew;
            SelectedHokenInfIsJihi = selectedHokenInfIsJihi;
            SelectedHokenInfStartDate = selectedHokenInfStartDate;
            SelectedHokenInfEndDate = selectedHokenInfEndDate;
            SelectedHokenInfKigo = selectedHokenInfKigo;
            SelectedHokenInfBango = selectedHokenInfBango;
            SelectedHokenInfHonkeKbn = selectedHokenInfHonkeKbn;
            SelectedHokenInfTokureiYm1 = selectedHokenInfTokureiYm1;
            SelectedHokenInfTokureiYm2 = selectedHokenInfTokureiYm2;
            SelectedHokenInfIsShahoOrKokuho = selectedHokenInfIsShahoOrKokuho;
            SelectedHokenInfIsExpirated = selectedHokenInfIsExpirated;
            SelectedHokenInfIsIsNoHoken = selectedHokenInfIsIsNoHoken;
            SelectedHokenInfConfirmDate = selectedHokenInfConfirmDate;
            SelectedHokenInfIsAddHokenCheck = selectedHokenInfIsAddHokenCheck;
            SelectedHokenInfTokki1 = selectedHokenInfTokki1;
            SelectedHokenInfTokki2 = selectedHokenInfTokki2;
            SelectedHokenInfTokki3 = selectedHokenInfTokki3;
            SelectedHokenInfTokki4 = selectedHokenInfTokki4;
            SelectedHokenInfTokki5 = selectedHokenInfTokki5;
            SelectedHokenPatternIsEmptyKohi1 = selectedHokenPatternIsEmptyKohi1;
            SelectedHokenPatternIsEmptyKohi2 = selectedHokenPatternIsEmptyKohi2;
            SelectedHokenPatternIsEmptyKohi3 = selectedHokenPatternIsEmptyKohi3;
            SelectedHokenPatternIsEmptyKohi4 = selectedHokenPatternIsEmptyKohi4;
            SelectedHokenPatternIsExpirated = selectedHokenPatternIsExpirated;
            SelectedHokenPatternIsEmptyHoken = selectedHokenPatternIsEmptyHoken;
            SelectedHokenPatternIsAddNew = selectedHokenPatternIsAddNew;
        }

        public int HpId { get; private set; }

        public int SinDate { get; private set; }

        public int PtBirthday { get; private set; }

        public int HokenKbn { get; private set; }

        public string HokenSyaNo { get; private set; }

        public bool IsSelectedHokenPattern { get; private set; }

        public bool IsSelectedHokenInf { get; private set; }

        public bool IsSelectedHokenMst { get; private set; }

        public string SelectedHokenInfHoubetu { get; private set; }

        public int SelectedHokenInfHokenNo { get; private set; }

        public int SelectedHokenInfHokenEdraNo { get; private set; }

        public bool SelectedHokenInfIsAddNew { get; private set; }

        public bool SelectedHokenInfIsJihi { get; private set; }

        public int SelectedHokenInfStartDate { get; private set; }

        public int SelectedHokenInfEndDate { get; private set; }

        public string SelectedHokenInfKigo { get; private set; }

        public string SelectedHokenInfBango { get; private set; }

        public int SelectedHokenInfHonkeKbn { get; private set; }

        public int SelectedHokenInfTokureiYm1 { get; private set; }

        public int SelectedHokenInfTokureiYm2 { get; private set; }

        public bool SelectedHokenInfIsShahoOrKokuho { get; private set; }

        public bool SelectedHokenInfIsExpirated { get; private set; }

        public bool SelectedHokenInfIsIsNoHoken { get; private set; }

        public int SelectedHokenInfConfirmDate { get; private set; }

        public bool SelectedHokenInfIsAddHokenCheck { get; private set; }

        public string SelectedHokenInfTokki1 { get; private set; }

        public string SelectedHokenInfTokki2 { get; private set; }

        public string SelectedHokenInfTokki3 { get; private set; }

        public string SelectedHokenInfTokki4 { get; private set; }

        public string SelectedHokenInfTokki5 { get; private set; }

        public bool SelectedHokenPatternIsEmptyKohi1 { get; private set; }

        public bool SelectedHokenPatternIsEmptyKohi2 { get; private set; }

        public bool SelectedHokenPatternIsEmptyKohi3 { get; private set; }

        public bool SelectedHokenPatternIsEmptyKohi4 { get; private set; }

        public bool SelectedHokenPatternIsExpirated { get; private set; }

        public bool SelectedHokenPatternIsEmptyHoken { get; private set; }

        public bool SelectedHokenPatternIsAddNew { get; private  set; }

    }
}
