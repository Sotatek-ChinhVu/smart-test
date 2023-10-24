using Domain.Common;

namespace Domain.Models.SystemStartDb
{
    public interface ISystemStartDbRepository : IRepositoryBase
    {
        void DeleteAndUpdateData(int dateDelete);
    }
}
