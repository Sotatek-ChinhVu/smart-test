using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidKohi
{
    public class ValidKohiInputData : IInputData<ValidKohiOutputData>
    {
        public ValidKohiInputData(int sinDate, int ptBirthday, bool isKohiEmptyModel, bool isSelectedKohiMst, string selectedKohiFutansyaNo, string selectedKohiJyukyusyaNo, string selectedKohiTokusyuNo, int selectedKohiStartDate, int selectedKohiEndDate, int selectedKohiConfirmDate, int selectedKohiHokenNo, int selectedKohiHokenEdraNo, bool selectedKohiIsAddNew,bool selectedHokenPatternIsExpirated)
        {
            SinDate = sinDate;
            PtBirthday = ptBirthday;
            IsKohiEmptyModel = isKohiEmptyModel;
            IsSelectedKohiMst = isSelectedKohiMst;
            SelectedKohiFutansyaNo = selectedKohiFutansyaNo;
            SelectedKohiJyukyusyaNo = selectedKohiJyukyusyaNo;
            SelectedKohiTokusyuNo = selectedKohiTokusyuNo;
            SelectedKohiStartDate = selectedKohiStartDate;
            SelectedKohiEndDate = selectedKohiEndDate;
            SelectedKohiConfirmDate = selectedKohiConfirmDate;
            SelectedKohiHokenNo = selectedKohiHokenNo;
            SelectedKohiHokenEdraNo = selectedKohiHokenEdraNo;
            SelectedKohiIsAddNew = selectedKohiIsAddNew;
            SelectedHokenPatternIsExpirated = selectedHokenPatternIsExpirated;
        }

        public int SinDate { get; private set; }

        public int PtBirthday { get; private set; }

        public bool IsKohiEmptyModel { get; private set; }

        public bool IsSelectedKohiMst { get; private set; }

        public string SelectedKohiFutansyaNo { get; private set; }

        public string SelectedKohiJyukyusyaNo { get; private set; }

        public string SelectedKohiTokusyuNo { get; private set; }

        public int SelectedKohiStartDate { get; private set; }

        public int SelectedKohiEndDate { get; private set; }

        public int SelectedKohiConfirmDate { get; private set; }

        public int SelectedKohiHokenNo { get; private set; }

        public int SelectedKohiHokenEdraNo { get; private set; }

        public bool SelectedKohiIsAddNew { get; private set; }

        public bool SelectedHokenPatternIsExpirated { get; private set; }
    }
}