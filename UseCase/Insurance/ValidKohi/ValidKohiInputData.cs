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
        public ValidKohiInputData(int sinDate, int ptBirthday, bool isKohiEmptyModel1, bool isSelectedKohiMst1, string selectedKohiFutansyaNo1, string selectedKohiJyukyusyaNo1, string selectedKohiTokusyuNo1, int selectedKohiStartDate1, int selectedKohiEndDate1, int selectedKohiConfirmDate1, int selectedKohiHokenNo1, bool selectedKohiIsAddNew1, int selectedKohiMstFutansyaCheckFlag1, int selectedKohiMstJyukyusyaCheckFlag1, int selectedKohiMstJyuKyuCheckDigit1, int selectedKohiMst1TokusyuCheckFlag1, int selectedKohiMstStartDate1, int selectedKohiMstEndDate1, string selectedKohiMstDisplayText1, string selectedKohiMstHoubetu1, int selectedKohiMstCheckDigit1, int selectedKohiMstAgeStart1, int selectedKohiMstAgeEnd1, bool isKohiEmptyModel2, bool isSelectedKohiMst2, string selectedKohiFutansyaNo2, string selectedKohiJyukyusyaNo2, string selectedKohiTokusyuNo2, int selectedKohiStartDate2, int selectedKohiEndDate2, int selectedKohiConfirmDate2, int selectedKohiHokenNo2, bool selectedKohiIsAddNew2, int selectedKohiMstFutansyaCheckFlag2, int selectedKohiMstJyukyusyaCheckFlag2, int selectedKohiMstJyuKyuCheckDigit2, int selectedKohiMst2TokusyuCheckFlag2, int selectedKohiMstStartDate2, int selectedKohiMstEndDate2, string selectedKohiMstDisplayText2, string selectedKohiMstHoubetu2, int selectedKohiMstCheckDigit2, int selectedKohiMstAgeStart2, int selectedKohiMstAgeEnd2, bool isKohiEmptyModel3, bool isSelectedKohiMst3, string selectedKohiFutansyaNo3, string selectedKohiJyukyusyaNo3, string selectedKohiTokusyuNo3, int selectedKohiStartDate3, int selectedKohiEndDate3, int selectedKohiConfirmDate3, int selectedKohiHokenNo3, bool selectedKohiIsAddNew3, int selectedKohiMstFutansyaCheckFlag3, int selectedKohiMstJyukyusyaCheckFlag3, int selectedKohiMstJyuKyuCheckDigit3, int selectedKohiMst3TokusyuCheckFlag3, int selectedKohiMstStartDate3, int selectedKohiMstEndDate3, string selectedKohiMstDisplayText3, string selectedKohiMstHoubetu3, int selectedKohiMstCheckDigit3, int selectedKohiMstAgeStart3, int selectedKohiMstAgeEnd3, bool isKohiEmptyModel4, bool isSelectedKohiMst4, string selectedKohiFutansyaNo4, string selectedKohiJyukyusyaNo4, string selectedKohiTokusyuNo4, int selectedKohiStartDate4, int selectedKohiEndDate4, int selectedKohiConfirmDate4, int selectedKohiHokenNo4, bool selectedKohiIsAddNew4, int selectedKohiMstFutansyaCheckFlag4, int selectedKohiMstJyukyusyaCheckFlag4, int selectedKohiMstJyuKyuCheckDigit4, int selectedKohiMst4TokusyuCheckFlag4, int selectedKohiMstStartDate4, int selectedKohiMstEndDate4, string selectedKohiMstDisplayText4, string selectedKohiMstHoubetu4, int selectedKohiMstCheckDigit4, int selectedKohiMstAgeStart4, int selectedKohiMstAgeEnd4)
        {
            SinDate = sinDate;
            PtBirthday = ptBirthday;
            IsKohiEmptyModel1 = isKohiEmptyModel1;
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
            SelectedKohiMstDisplayText1 = selectedKohiMstDisplayText1;
            SelectedKohiMstHoubetu1 = selectedKohiMstHoubetu1;
            SelectedKohiMstCheckDigit1 = selectedKohiMstCheckDigit1;
            SelectedKohiMstAgeStart1 = selectedKohiMstAgeStart1;
            SelectedKohiMstAgeEnd1 = selectedKohiMstAgeEnd1;
            IsKohiEmptyModel2 = isKohiEmptyModel2;
            IsSelectedKohiMst2 = isSelectedKohiMst2;
            SelectedKohiFutansyaNo2 = selectedKohiFutansyaNo2;
            SelectedKohiJyukyusyaNo2 = selectedKohiJyukyusyaNo2;
            SelectedKohiTokusyuNo2 = selectedKohiTokusyuNo2;
            SelectedKohiStartDate2 = selectedKohiStartDate2;
            SelectedKohiEndDate2 = selectedKohiEndDate2;
            SelectedKohiConfirmDate2 = selectedKohiConfirmDate2;
            SelectedKohiHokenNo2 = selectedKohiHokenNo2;
            SelectedKohiIsAddNew2 = selectedKohiIsAddNew2;
            SelectedKohiMstFutansyaCheckFlag2 = selectedKohiMstFutansyaCheckFlag2;
            SelectedKohiMstJyukyusyaCheckFlag2 = selectedKohiMstJyukyusyaCheckFlag2;
            SelectedKohiMstJyuKyuCheckDigit2 = selectedKohiMstJyuKyuCheckDigit2;
            SelectedKohiMst2TokusyuCheckFlag2 = selectedKohiMst2TokusyuCheckFlag2;
            SelectedKohiMstStartDate2 = selectedKohiMstStartDate2;
            SelectedKohiMstEndDate2 = selectedKohiMstEndDate2;
            SelectedKohiMstDisplayText2 = selectedKohiMstDisplayText2;
            SelectedKohiMstHoubetu2 = selectedKohiMstHoubetu2;
            SelectedKohiMstCheckDigit2 = selectedKohiMstCheckDigit2;
            SelectedKohiMstAgeStart2 = selectedKohiMstAgeStart2;
            SelectedKohiMstAgeEnd2 = selectedKohiMstAgeEnd2;
            IsKohiEmptyModel3 = isKohiEmptyModel3;
            IsSelectedKohiMst3 = isSelectedKohiMst3;
            SelectedKohiFutansyaNo3 = selectedKohiFutansyaNo3;
            SelectedKohiJyukyusyaNo3 = selectedKohiJyukyusyaNo3;
            SelectedKohiTokusyuNo3 = selectedKohiTokusyuNo3;
            SelectedKohiStartDate3 = selectedKohiStartDate3;
            SelectedKohiEndDate3 = selectedKohiEndDate3;
            SelectedKohiConfirmDate3 = selectedKohiConfirmDate3;
            SelectedKohiHokenNo3 = selectedKohiHokenNo3;
            SelectedKohiIsAddNew3 = selectedKohiIsAddNew3;
            SelectedKohiMstFutansyaCheckFlag3 = selectedKohiMstFutansyaCheckFlag3;
            SelectedKohiMstJyukyusyaCheckFlag3 = selectedKohiMstJyukyusyaCheckFlag3;
            SelectedKohiMstJyuKyuCheckDigit3 = selectedKohiMstJyuKyuCheckDigit3;
            SelectedKohiMst3TokusyuCheckFlag3 = selectedKohiMst3TokusyuCheckFlag3;
            SelectedKohiMstStartDate3 = selectedKohiMstStartDate3;
            SelectedKohiMstEndDate3 = selectedKohiMstEndDate3;
            SelectedKohiMstDisplayText3 = selectedKohiMstDisplayText3;
            SelectedKohiMstHoubetu3 = selectedKohiMstHoubetu3;
            SelectedKohiMstCheckDigit3 = selectedKohiMstCheckDigit3;
            SelectedKohiMstAgeStart3 = selectedKohiMstAgeStart3;
            SelectedKohiMstAgeEnd3 = selectedKohiMstAgeEnd3;
            IsKohiEmptyModel4 = isKohiEmptyModel4;
            IsSelectedKohiMst4 = isSelectedKohiMst4;
            SelectedKohiFutansyaNo4 = selectedKohiFutansyaNo4;
            SelectedKohiJyukyusyaNo4 = selectedKohiJyukyusyaNo4;
            SelectedKohiTokusyuNo4 = selectedKohiTokusyuNo4;
            SelectedKohiStartDate4 = selectedKohiStartDate4;
            SelectedKohiEndDate4 = selectedKohiEndDate4;
            SelectedKohiConfirmDate4 = selectedKohiConfirmDate4;
            SelectedKohiHokenNo4 = selectedKohiHokenNo4;
            SelectedKohiIsAddNew4 = selectedKohiIsAddNew4;
            SelectedKohiMstFutansyaCheckFlag4 = selectedKohiMstFutansyaCheckFlag4;
            SelectedKohiMstJyukyusyaCheckFlag4 = selectedKohiMstJyukyusyaCheckFlag4;
            SelectedKohiMstJyuKyuCheckDigit4 = selectedKohiMstJyuKyuCheckDigit4;
            SelectedKohiMst4TokusyuCheckFlag4 = selectedKohiMst4TokusyuCheckFlag4;
            SelectedKohiMstStartDate4 = selectedKohiMstStartDate4;
            SelectedKohiMstEndDate4 = selectedKohiMstEndDate4;
            SelectedKohiMstDisplayText4 = selectedKohiMstDisplayText4;
            SelectedKohiMstHoubetu4 = selectedKohiMstHoubetu4;
            SelectedKohiMstCheckDigit4 = selectedKohiMstCheckDigit4;
            SelectedKohiMstAgeStart4 = selectedKohiMstAgeStart4;
            SelectedKohiMstAgeEnd4 = selectedKohiMstAgeEnd4;
        }

        public int SinDate { get; private set; }

        public int PtBirthday { get; private set; }

        public bool IsKohiEmptyModel1 { get; private set; }

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

        public string SelectedKohiMstDisplayText1 { get; private set; }

        public string SelectedKohiMstHoubetu1 { get; private set; }

        public int SelectedKohiMstCheckDigit1 { get; private set; }

        public int SelectedKohiMstAgeStart1 { get; private set; }

        public int SelectedKohiMstAgeEnd1 { get; private set; }

        public bool IsKohiEmptyModel2 { get; private set; }

        public bool IsSelectedKohiMst2 { get; private set; }

        public string SelectedKohiFutansyaNo2 { get; private set; }

        public string SelectedKohiJyukyusyaNo2 { get; private set; }

        public string SelectedKohiTokusyuNo2 { get; private set; }

        public int SelectedKohiStartDate2 { get; private set; }

        public int SelectedKohiEndDate2 { get; private set; }

        public int SelectedKohiConfirmDate2 { get; private set; }

        public int SelectedKohiHokenNo2 { get; private set; }

        public bool SelectedKohiIsAddNew2 { get; private set; }

        public int SelectedKohiMstFutansyaCheckFlag2 { get; private set; }

        public int SelectedKohiMstJyukyusyaCheckFlag2 { get; private set; }

        public int SelectedKohiMstJyuKyuCheckDigit2 { get; private set; }

        public int SelectedKohiMst2TokusyuCheckFlag2 { get; private set; }

        public int SelectedKohiMstStartDate2 { get; private set; }

        public int SelectedKohiMstEndDate2 { get; private set; }

        public string SelectedKohiMstDisplayText2 { get; private set; }

        public string SelectedKohiMstHoubetu2 { get; private set; }

        public int SelectedKohiMstCheckDigit2 { get; private set; }

        public int SelectedKohiMstAgeStart2 { get; private set; }

        public int SelectedKohiMstAgeEnd2 { get; private set; }

        public bool IsKohiEmptyModel3 { get; private set; }

        public bool IsSelectedKohiMst3 { get; private set; }

        public string SelectedKohiFutansyaNo3 { get; private set; }

        public string SelectedKohiJyukyusyaNo3 { get; private set; }

        public string SelectedKohiTokusyuNo3 { get; private set; }

        public int SelectedKohiStartDate3 { get; private set; }

        public int SelectedKohiEndDate3 { get; private set; }

        public int SelectedKohiConfirmDate3 { get; private set; }

        public int SelectedKohiHokenNo3 { get; private set; }

        public bool SelectedKohiIsAddNew3 { get; private set; }

        public int SelectedKohiMstFutansyaCheckFlag3 { get; private set; }

        public int SelectedKohiMstJyukyusyaCheckFlag3 { get; private set; }

        public int SelectedKohiMstJyuKyuCheckDigit3 { get; private set; }

        public int SelectedKohiMst3TokusyuCheckFlag3 { get; private set; }

        public int SelectedKohiMstStartDate3 { get; private set; }

        public int SelectedKohiMstEndDate3 { get; private set; }

        public string SelectedKohiMstDisplayText3 { get; private set; }

        public string SelectedKohiMstHoubetu3 { get; private set; }

        public int SelectedKohiMstCheckDigit3 { get; private set; }

        public int SelectedKohiMstAgeStart3 { get; private set; }

        public int SelectedKohiMstAgeEnd3 { get; private set; }

        public bool IsKohiEmptyModel4 { get; private set; }

        public bool IsSelectedKohiMst4 { get; private set; }

        public string SelectedKohiFutansyaNo4 { get; private set; }

        public string SelectedKohiJyukyusyaNo4 { get; private set; }

        public string SelectedKohiTokusyuNo4 { get; private set; }

        public int SelectedKohiStartDate4 { get; private set; }

        public int SelectedKohiEndDate4 { get; private set; }

        public int SelectedKohiConfirmDate4 { get; private set; }

        public int SelectedKohiHokenNo4 { get; private set; }

        public bool SelectedKohiIsAddNew4 { get; private set; }

        public int SelectedKohiMstFutansyaCheckFlag4 { get; private set; }

        public int SelectedKohiMstJyukyusyaCheckFlag4 { get; private set; }

        public int SelectedKohiMstJyuKyuCheckDigit4 { get; private set; }

        public int SelectedKohiMst4TokusyuCheckFlag4 { get; private set; }

        public int SelectedKohiMstStartDate4 { get; private set; }

        public int SelectedKohiMstEndDate4 { get; private set; }

        public string SelectedKohiMstDisplayText4 { get; private set; }

        public string SelectedKohiMstHoubetu4 { get; private set; }

        public int SelectedKohiMstCheckDigit4 { get; private set; }

        public int SelectedKohiMstAgeStart4 { get; private set; }

        public int SelectedKohiMstAgeEnd4 { get; private set; }
    }
}
