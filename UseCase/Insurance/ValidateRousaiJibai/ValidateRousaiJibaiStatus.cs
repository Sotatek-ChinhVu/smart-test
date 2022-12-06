using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.Insurance.ValidateRousaiJibai
{
    public enum ValidateRousaiJibaiStatus: byte
    {
        InvalidSuccess = 0,
        InvalidHokenKbn = 1,
        InvalidSinDate = 2,
        InvalidSelectedHokenInfRousaiSaigaiKbn = 3,
        InvalidSelectedHokenInfRousaiSyobyoDate = 4,
        InvalidSelectedHokenInfRyoyoStartDate = 5,
        InvalidSelectedHokenInfRyoyoEndDate = 6,
        InvalidSelectedHokenInfStartDate = 7,
        InvalidSelectedHokenInfEndDate = 8,
        InvalidSelectedHokenInfConfirmDate = 9,
        InvalidHpId = 10,
        InvalidFaild = 11,
        InvalidRodoBangoNull = 12,
        InvalidRodoBangoLengthNotEquals14 = 13,
        InvalidCheckItemFirstListRousaiTenki = 14,
        InvalidCheckRousaiTenkiSinkei = 15,
        InvalidCheckRousaiTenkiTenki = 16,
        InvalidCheckRousaiTenkiEndDate = 17,
        InvalidCheckRousaiSaigaiKbnNotEquals1And2 = 18,
        InvalidCheckRousaiSyobyoDateEquals0 = 19,
        InvalidCheckHokenKbnEquals13AndRousaiSyobyoCdIsNull = 20,
        InvalidCheckRousaiRyoyoDate = 21,
        InvalidCheckDateExpirated = 22,
        InvalidNenkinBangoIsNull = 23,
        InvalidNenkinBangoLengthNotEquals9 = 24,
        InvalidKenkoKanriBangoIsNull = 25,
        InvalidKenkoKanriBangoLengthNotEquals13 = 26,
        InvalidRousaiRyoyoDate = 27,
        InvalidSelectedHokenInfHokenMasterModelIsNull = 28
    }
}
