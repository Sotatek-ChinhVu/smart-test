﻿namespace CommonChecker.DB
{
    public interface ISystemConfigRepository
    {
        public int RefillSetting(int hpId, int presentDate);
    }
}
