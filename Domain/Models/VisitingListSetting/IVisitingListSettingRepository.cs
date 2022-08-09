using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VisitingListSetting
{
    public interface IVisitingListSettingRepository
    {
        VisitingListSettingModel Get(int userId);
    }
}
