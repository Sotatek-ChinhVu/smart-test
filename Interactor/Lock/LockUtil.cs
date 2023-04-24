using Domain.Models.Lock;
using Helper.Common;
using Helper.Constants;

namespace Interactor.Lock
{
    public static class LockUtil
    {
        public static LockModel GetLockInf(List<LockModel> lockInfList)
        {
            var dateTimeNow = CIUtil.GetJapanDateTimeNow();
            foreach (var lockInf in lockInfList.OrderBy(l => l.LockLevel).ThenBy(l => l.LockRange).ToList())
            {
                if (lockInf.LockDateTime.AddMinutes(LockConst.LockTtl) > dateTimeNow)
                {
                    return lockInf;
                }
            }
            return new LockModel();
        }
    }
}
