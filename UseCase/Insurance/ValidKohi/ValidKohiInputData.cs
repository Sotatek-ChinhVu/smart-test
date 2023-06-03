using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidKohi
{
    public class ValidKohiInputData : IInputData<ValidKohiOutputData>
    {
        public ValidKohiInputData(int sinDate, int ptBirthday, bool isKohiEmptyModel, bool isSelectedKohiMst, string selectedKohiFutansyaNo, string selectedKohiJyukyusyaNo, string selectedKohiTokusyuNo, int selectedKohiStartDate, int selectedKohiEndDate, int selectedKohiConfirmDate, int selectedKohiHokenNo, bool selectedKohiIsAddNew,bool selectedHokenPatternIsExpirated, int kohiMasterIsFutansyaNoCheck, int kohiMasterIsJyukyusyaNoCheck, int kohiMasterIsTokusyuNoCheck, int kohiMasterStartDate, int kohiMasterEndDate, string kohiMasterDisplayTextMaster, int kohiMasterJyukyuCheckDigit, int kohiMasterCheckDigit, string kohiMasterHoubetu, int kohiMasterAgeStart, int kohiMasterAgeEnd)
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
            SelectedKohiIsAddNew = selectedKohiIsAddNew;
            SelectedHokenPatternIsExpirated = selectedHokenPatternIsExpirated;
            KohiMasterIsFutansyaNoCheck = kohiMasterIsFutansyaNoCheck;
            KohiMasterIsJyukyusyaNoCheck = kohiMasterIsJyukyusyaNoCheck;
            KohiMasterIsTokusyuNoCheck = kohiMasterIsTokusyuNoCheck;
            KohiMasterStartDate = kohiMasterStartDate;
            KohiMasterEndDate = kohiMasterEndDate;
            KohiMasterDisplayTextMaster = kohiMasterDisplayTextMaster;
            KohiMasterJyukyuCheckDigit = kohiMasterJyukyuCheckDigit;
            KohiMasterCheckDigit = kohiMasterCheckDigit;
            KohiMasterHoubetu = kohiMasterHoubetu;
            KohiMasterAgeStart = kohiMasterAgeStart;
            KohiMasterAgeEnd = kohiMasterAgeEnd;
        }

        public int HpId { get; private set; }

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

        public bool SelectedKohiIsAddNew { get; private set; }

        public bool SelectedHokenPatternIsExpirated { get; private set; }

        #region info hokenMst
        public int KohiMasterIsFutansyaNoCheck { get; private set; }

        public int KohiMasterIsJyukyusyaNoCheck { get; private set; }

        public int KohiMasterIsTokusyuNoCheck { get; private set; }

        public int KohiMasterStartDate { get; private set; }

        public int KohiMasterEndDate { get; private set; }

        public string KohiMasterDisplayTextMaster { get; private set; }

        public int KohiMasterJyukyuCheckDigit { get; private set; }

        public int KohiMasterCheckDigit { get; private set; }

        public string KohiMasterHoubetu { get; private set; }

        public int KohiMasterAgeStart { get; private set; }

        public int KohiMasterAgeEnd { get; private set; }
        #endregion
    }
}