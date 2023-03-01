<<<<<<<< HEAD:UseCase/SystemConf/Get/GetSystemConfStatus.cs
﻿namespace UseCase.SystemConf.Get
========
﻿namespace UseCase.SystemConf.GetSystemConf
>>>>>>>> develop:UseCase/SystemConf/GetSystemConf/GetSystemConfStatus.cs
{
    public enum GetSystemConfStatus : byte
    {
        Successed = 1,
        InvalidHpId = 2,
        InvalidGrpCd = 3,
        InvalidGrpEdaNo = 4,
        Failed = 5
    }
}
