using UseCase.Core.Sync.Core;

namespace UseCase.InsuranceMst.GetHokenSyaMst
{
    public class GetHokenSyaMstInputData : IInputData<GetHokenSyaMstOutputData>
    {
       
        public GetHokenSyaMstInputData(int hpId, string hokensyaNo, int hokenKbn)
        {
            HpId = hpId;
            HokensyaNo = hokensyaNo;
            HokenKbn = hokenKbn;
        }

        public int HpId { get; private set; }

        public string HokensyaNo { get; private set; }

        public int HokenKbn { get; private set; }
    }
}
