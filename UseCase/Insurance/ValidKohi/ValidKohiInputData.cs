using Domain.Models.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidKohi
{
    public class ValidKohiInputData : IInputData<ValidKohiOutputData>
    {
        public ValidKohiInputData(int sinDate, int ptBirthday, bool selectedHokenPatternIsExpirated, bool isKohi1, bool isSelectedKohiMst1, string selectedKohiFutansyaNo1, string selectedKohiJyukyusyaNo1, string selectedKohiTokusyuNo1, int selectedKohiStartDate1, int selectedKohiEndDate1, int selectedKohiConfirmDate1, int selectedKohiHokenNo1, bool selectedKohiIsAddNew1, int selectedKohiMstFutansyaCheckFlag1, int selectedKohiMstJyukyusyaCheckFlag1, int selectedKohiMstJyuKyuCheckDigit1, int selectedKohiMst1TokusyuCheckFlag1, int selectedKohiMstStartDate1, int selectedKohiMstEndDate1, string selectedKohiMstDisplayText, string selectedKohiMstHoubetu, int selectedKohiMstCheckDigit, int selectedKohiMstAgeStart1, int selectedKohiMstAgeEnd1, KohiInfModel kohi1)
        {
            SinDate = sinDate;
            PtBirthday = ptBirthday;
            SelectedHokenPatternIsExpirated = selectedHokenPatternIsExpirated;
            IsKohi1 = isKohi1;
            IsSelectedKohiMst1 = isSelectedKohiMst1;
            SelectedKohiFutansyaNo1 = selectedKohiFutansyaNo1;
            SelectedKohiJyukyusyaNo1 = selectedKohiJyukyusyaNo1;
            SelectedKohiTokusyuNo1 = selectedKohiTokusyuNo1;
            SelectedKohiStartDate1 = selectedKohiStartDate1;
            SelectedKohiEndDate1 = selectedKohiEndDate1;
            SelectedKohiConfirmDate1 = selectedKohiConfirmDate1;
            SelectedKohiHokenNo1 = selectedKohiHokenNo1;
            SelectedKohiIsAddNew1 = selectedKohiIsAddNew1;
            SelectedKohiMstFutansyaCheckFlag1 = selectedKohiMstFutansyaCheckFlag1;
            SelectedKohiMstJyukyusyaCheckFlag1 = selectedKohiMstJyukyusyaCheckFlag1;
            SelectedKohiMstJyuKyuCheckDigit1 = selectedKohiMstJyuKyuCheckDigit1;
            SelectedKohiMst1TokusyuCheckFlag1 = selectedKohiMst1TokusyuCheckFlag1;
            SelectedKohiMstStartDate1 = selectedKohiMstStartDate1;
            SelectedKohiMstEndDate1 = selectedKohiMstEndDate1;
            SelectedKohiMstDisplayText = selectedKohiMstDisplayText;
            SelectedKohiMstHoubetu = selectedKohiMstHoubetu;
            SelectedKohiMstCheckDigit = selectedKohiMstCheckDigit;
            SelectedKohiMstAgeStart1 = selectedKohiMstAgeStart1;
            SelectedKohiMstAgeEnd1 = selectedKohiMstAgeEnd1;
        }

        public int SinDate { get; private set; }

        public int PtBirthday { get; private set; }

        public bool SelectedHokenPatternIsExpirated { get; private set; }

        public bool IsKohi1 { get; private set; }

        public bool IsSelectedKohiMst1 { get; private set; }

        public string SelectedKohiFutansyaNo1 { get; private set; }

        public string SelectedKohiJyukyusyaNo1 { get; private set; }

        public string SelectedKohiTokusyuNo1 { get; private set; }

        public int SelectedKohiStartDate1 { get; private set; }

        public int SelectedKohiEndDate1 { get; private set; }

        public int SelectedKohiConfirmDate1 { get; private set; }

        public int SelectedKohiHokenNo1 { get; private set; }

        public bool SelectedKohiIsAddNew1 { get; private set; }

        public int SelectedKohiMstFutansyaCheckFlag1 { get; private set; }

        public int SelectedKohiMstJyukyusyaCheckFlag1 { get; private set; }

        public int SelectedKohiMstJyuKyuCheckDigit1 { get; private set; }

        public int SelectedKohiMst1TokusyuCheckFlag1 { get; private set; }

        public int SelectedKohiMstStartDate1 { get; private set; }

        public int SelectedKohiMstEndDate1 { get; private set; }

        public string SelectedKohiMstDisplayText { get; private set; }

        public string SelectedKohiMstHoubetu { get; private set; }

        public int SelectedKohiMstCheckDigit { get; private set; }

        public int SelectedKohiMstAgeStart1 { get; private set; }

        public int SelectedKohiMstAgeEnd1 { get; private set; }

    }
}
