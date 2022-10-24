using Domain.Core;
using Helper.Common;

namespace Domain.Types
{
    public class GroupKoui : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private GroupKoui(int value)
        {
            _value = OdrUtil.GetGroupKoui(value);
        }

        public static GroupKoui From(int value)
        {
            return new GroupKoui(value);
        }
    }
}
