namespace CommonChecker.DB
{
    public interface ISystemConfigRepository
    {
        /// <summary>
        /// System GenerationConfig
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="presentDate"></param>
        /// <returns></returns>
        public int RefillSetting(int hpId, int presentDate);
    }
}
