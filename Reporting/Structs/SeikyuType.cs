namespace Reporting.Structs
{
    public struct SeikyuType
    {
        public bool IsNormal { get; }
        public bool IsPaper { get; }
        public bool IsDelay { get; }
        public bool IsHenrei { get; }
        public bool IsOnline { get; }

        public SeikyuType(bool isNormal, bool isPaper, bool isDelay, bool isHenrei, bool isOnline)
        {
            IsNormal = isNormal;
            IsPaper = isPaper;
            IsDelay = isDelay;
            IsHenrei = isHenrei;
            IsOnline = isOnline;
        }
    }
}
