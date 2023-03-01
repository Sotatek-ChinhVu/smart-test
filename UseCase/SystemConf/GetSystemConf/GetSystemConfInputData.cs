using UseCase.Core.Sync.Core;

<<<<<<<< HEAD:UseCase/SystemConf/Get/GetSystemConfInputData.cs
namespace UseCase.SystemConf.Get
========
namespace UseCase.SystemConf.GetSystemConf
>>>>>>>> develop:UseCase/SystemConf/GetSystemConf/GetSystemConfInputData.cs
{
    public class GetSystemConfInputData : IInputData<GetSystemConfOutputData>
    {
        public GetSystemConfInputData(int hpId, int grpCd, int grpEdaNo)
        {
            HpId = hpId;
            GrpCd = grpCd;
            GrpEdaNo = grpEdaNo;
        }

        public int HpId { get; private set; }

        public int GrpCd { get; private set; }

        public int GrpEdaNo { get; private set; }
    }
}
