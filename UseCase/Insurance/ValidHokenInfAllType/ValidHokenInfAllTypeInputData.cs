using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.ValidHokenInfAllType
{
    public class ValidHokenInfAllTypeInputData : IInputData<ValidHokenInfAllTypeOutputData>
    {
        public ValidHokenInfAllTypeInputData(int hpId, int hokenKbn, int sinDate, bool isSelectedHokenInf, string selectedHokenInfRodoBango, List<RousaiTenkiModel> listRousaiTenki, int selectedHokenInfRousaiSaigaiKbn, int selectedHokenInfRousaiSyobyoDate, string selectedHokenInfRousaiSyobyoCd, int selectedHokenInfRyoyoStartDate, int selectedHokenInfRyoyoEndDate, int selectedHokenInfStartDate, int selectedHokenInfEndDate, bool selectedHokenInfIsAddNew, string selectedHokenInfNenkinBango, string selectedHokenInfKenkoKanriBango, int selectedHokenInfConfirmDate, bool selectedHokenInfHokenMasterModelIsNull, bool selectedHokenInf, string selectedHokenInfTokki1, string selectedHokenInfTokki2, string selectedHokenInfTokki3, string selectedHokenInfTokki4, string selectedHokenInfTokki5, string selectedHokenInfHoubetu, bool selectedHokenInfIsJihi, string hokenSyaNo, string selectedHokenInfKigo, string selectedHokenInfBango, int selectedHokenInfTokureiYm1, int selectedHokenInfTokureiYm2, bool selectedHokenInfisShahoOrKokuho, bool selectedHokenInfisExpirated, int selectedHokenInfHokenNo, int selectedHokenInfHokenEdraNo, bool isSelectedHokenMst, int selectedHokenInfHonkeKbn, int ptBirthday,bool selectedHokenInfIsAddHokenCheck,int selectedHokenInfHokenChecksCount,bool hokenInfIsNoHoken,int hokenInfConfirmDate)
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
            SelectedHokenInfHokenMasterModelIsNull = selectedHokenInfHokenMasterModelIsNull;
            SelectedHokenInf = selectedHokenInf;
            SelectedHokenInfTokki1 = selectedHokenInfTokki1;
            SelectedHokenInfTokki2 = selectedHokenInfTokki2;
            SelectedHokenInfTokki3 = selectedHokenInfTokki3;
            SelectedHokenInfTokki4 = selectedHokenInfTokki4;
            SelectedHokenInfTokki5 = selectedHokenInfTokki5;
            SelectedHokenInfHoubetu = selectedHokenInfHoubetu;
            SelectedHokenInfIsJihi = selectedHokenInfIsJihi;
            HokenSyaNo = hokenSyaNo;
            SelectedHokenInfKigo = selectedHokenInfKigo;
            SelectedHokenInfBango = selectedHokenInfBango;
            SelectedHokenInfTokureiYm1 = selectedHokenInfTokureiYm1;
            SelectedHokenInfTokureiYm2 = selectedHokenInfTokureiYm2;
            SelectedHokenInfisShahoOrKokuho = selectedHokenInfisShahoOrKokuho;
            SelectedHokenInfisExpirated = selectedHokenInfisExpirated;
            SelectedHokenInfHokenNo = selectedHokenInfHokenNo;
            SelectedHokenInfHokenEdraNo = selectedHokenInfHokenEdraNo;
            IsSelectedHokenMst = isSelectedHokenMst;
            SelectedHokenInfHonkeKbn = selectedHokenInfHonkeKbn;
            PtBirthday = ptBirthday;
            SelectedHokenInfIsAddHokenCheck = selectedHokenInfIsAddHokenCheck;
            SelectedHokenInfHokenChecksCount = selectedHokenInfHokenChecksCount;
            HokenInfIsNoHoken = hokenInfIsNoHoken;
            HokenInfConfirmDate = hokenInfConfirmDate;
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

        public bool SelectedHokenInfHokenMasterModelIsNull { get; private set; }

        public bool SelectedHokenInf { get; private set; }

        public string SelectedHokenInfTokki1 { get; private set; }

        public string SelectedHokenInfTokki2 { get; private set; }

        public string SelectedHokenInfTokki3 { get; private set; }

        public string SelectedHokenInfTokki4 { get; private set; }

        public string SelectedHokenInfTokki5 { get; private set; }

        public string SelectedHokenInfHoubetu { get; private set; }

        public bool SelectedHokenInfIsJihi { get; private set; }

        public string HokenSyaNo { get; private set; }

        public string SelectedHokenInfKigo { get; private set; }

        public string SelectedHokenInfBango { get; private set; }

        public int SelectedHokenInfTokureiYm1 { get; private set; }

        public int SelectedHokenInfTokureiYm2 { get; private set; }

        public bool SelectedHokenInfisShahoOrKokuho { get; private set; }

        public bool SelectedHokenInfisExpirated { get; private set; }

        public int SelectedHokenInfHokenNo { get; private set; }

        public int SelectedHokenInfHokenEdraNo { get; private set; }

        public bool IsSelectedHokenMst { get; private set; }

        public int SelectedHokenInfHonkeKbn { get; private set; }

        public int PtBirthday { get; private set; }

        public bool SelectedHokenInfIsAddHokenCheck { get; private set; }

        public int SelectedHokenInfHokenChecksCount { get; private set; }

        public bool HokenInfIsNoHoken { get; set; }

        public int HokenInfConfirmDate { get; set; }
    }
}