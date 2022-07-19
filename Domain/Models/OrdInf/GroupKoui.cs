using Domain.Core;
using UseCase.Common;

namespace Domain.Models.OrdInfs
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
