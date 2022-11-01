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
        public ValidKohiInputData(int sinDate, int ptBirthday, bool isKohiEmptyModel1, bool isSelectedKohiMst1, string selectedKohiFutansyaNo1, string selectedKohiJyukyusyaNo1, string selectedKohiTokusyuNo1, int selectedKohiStartDate1, int selectedKohiEndDate1, int selectedKohiConfirmDate1, int selectedKohiHokenNo1, int selectedKohiHokenEdraNo1, bool selectedKohiIsAddNew1, bool isKohiEmptyModel2, bool isSelectedKohiMst2, string selectedKohiFutansyaNo2, string selectedKohiJyukyusyaNo2, string selectedKohiTokusyuNo2, int selectedKohiStartDate2, int selectedKohiEndDate2, int selectedKohiConfirmDate2, int selectedKohiHokenNo2, int selectedKohiHokenEdraNo2, bool selectedKohiIsAddNew2, bool isKohiEmptyModel3, bool isSelectedKohiMst3, string selectedKohiFutansyaNo3, string selectedKohiJyukyusyaNo3, string selectedKohiTokusyuNo3, int selectedKohiStartDate3, int selectedKohiEndDate3, int selectedKohiConfirmDate3, int selectedKohiHokenNo3, int selectedKohiHokenEdraNo3, bool selectedKohiIsAddNew3, bool isKohiEmptyModel4, bool isSelectedKohiMst4, string selectedKohiFutansyaNo4, string selectedKohiJyukyusyaNo4, string selectedKohiTokusyuNo4, int selectedKohiStartDate4, int selectedKohiEndDate4, int selectedKohiConfirmDate4, int selectedKohiHokenNo4, int selectedKohiHokenEdraNo4, bool selectedKohiIsAddNew4)
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
            SelectedKohiHokenEdraNo1 = selectedKohiHokenEdraNo1;
            SelectedKohiIsAddNew1 = selectedKohiIsAddNew1;
            IsKohiEmptyModel2 = isKohiEmptyModel2;
            IsSelectedKohiMst2 = isSelectedKohiMst2;
            SelectedKohiFutansyaNo2 = selectedKohiFutansyaNo2;
            SelectedKohiJyukyusyaNo2 = selectedKohiJyukyusyaNo2;
            SelectedKohiTokusyuNo2 = selectedKohiTokusyuNo2;
            SelectedKohiStartDate2 = selectedKohiStartDate2;
            SelectedKohiEndDate2 = selectedKohiEndDate2;
            SelectedKohiConfirmDate2 = selectedKohiConfirmDate2;
            SelectedKohiHokenNo2 = selectedKohiHokenNo2;
            SelectedKohiHokenEdraNo2 = selectedKohiHokenEdraNo2;
            SelectedKohiIsAddNew2 = selectedKohiIsAddNew2;
            IsKohiEmptyModel3 = isKohiEmptyModel3;
            IsSelectedKohiMst3 = isSelectedKohiMst3;
            SelectedKohiFutansyaNo3 = selectedKohiFutansyaNo3;
            SelectedKohiJyukyusyaNo3 = selectedKohiJyukyusyaNo3;
            SelectedKohiTokusyuNo3 = selectedKohiTokusyuNo3;
            SelectedKohiStartDate3 = selectedKohiStartDate3;
            SelectedKohiEndDate3 = selectedKohiEndDate3;
            SelectedKohiConfirmDate3 = selectedKohiConfirmDate3;
            SelectedKohiHokenNo3 = selectedKohiHokenNo3;
            SelectedKohiHokenEdraNo3 = selectedKohiHokenEdraNo3;
            SelectedKohiIsAddNew3 = selectedKohiIsAddNew3;
            IsKohiEmptyModel4 = isKohiEmptyModel4;
            IsSelectedKohiMst4 = isSelectedKohiMst4;
            SelectedKohiFutansyaNo4 = selectedKohiFutansyaNo4;
            SelectedKohiJyukyusyaNo4 = selectedKohiJyukyusyaNo4;
            SelectedKohiTokusyuNo4 = selectedKohiTokusyuNo4;
            SelectedKohiStartDate4 = selectedKohiStartDate4;
            SelectedKohiEndDate4 = selectedKohiEndDate4;
            SelectedKohiConfirmDate4 = selectedKohiConfirmDate4;
            SelectedKohiHokenNo4 = selectedKohiHokenNo4;
            SelectedKohiHokenEdraNo4 = selectedKohiHokenEdraNo4;
            SelectedKohiIsAddNew4 = selectedKohiIsAddNew4;
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

        public int SelectedKohiHokenEdraNo1 { get; private set; }

        public bool SelectedKohiIsAddNew1 { get; private set; }

        public bool IsKohiEmptyModel2 { get; private set; }

        public bool IsSelectedKohiMst2 { get; private set; }

        public string SelectedKohiFutansyaNo2 { get; private set; }

        public string SelectedKohiJyukyusyaNo2 { get; private set; }

        public string SelectedKohiTokusyuNo2 { get; private set; }

        public int SelectedKohiStartDate2 { get; private set; }

        public int SelectedKohiEndDate2 { get; private set; }

        public int SelectedKohiConfirmDate2 { get; private set; }

        public int SelectedKohiHokenNo2 { get; private set; }

        public int SelectedKohiHokenEdraNo2 { get; private set; }

        public bool SelectedKohiIsAddNew2 { get; private set; }

        public bool IsKohiEmptyModel3 { get; private set; }

        public bool IsSelectedKohiMst3 { get; private set; }

        public string SelectedKohiFutansyaNo3 { get; private set; }

        public string SelectedKohiJyukyusyaNo3 { get; private set; }

        public string SelectedKohiTokusyuNo3 { get; private set; }

        public int SelectedKohiStartDate3 { get; private set; }

        public int SelectedKohiEndDate3 { get; private set; }

        public int SelectedKohiConfirmDate3 { get; private set; }

        public int SelectedKohiHokenNo3 { get; private set; }

        public int SelectedKohiHokenEdraNo3 { get; private set; }

        public bool SelectedKohiIsAddNew3 { get; private set; }

        public bool IsKohiEmptyModel4 { get; private set; }

        public bool IsSelectedKohiMst4 { get; private set; }

        public string SelectedKohiFutansyaNo4 { get; private set; }

        public string SelectedKohiJyukyusyaNo4 { get; private set; }

        public string SelectedKohiTokusyuNo4 { get; private set; }

        public int SelectedKohiStartDate4 { get; private set; }

        public int SelectedKohiEndDate4 { get; private set; }

        public int SelectedKohiConfirmDate4 { get; private set; }

        public int SelectedKohiHokenNo4 { get; private set; }

        public int SelectedKohiHokenEdraNo4 { get; private set; }

        public bool SelectedKohiIsAddNew4 { get; private set; }
    }
}
