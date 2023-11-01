using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SmartKartePort
{
    public interface ISmartKartePortRepository : IRepositoryBase
    {
        bool UpdateSignalRPort(int userId, SmartKarteAppSignalRPortModel signalRPortModel);
        SmartKarteAppSignalRPortModel GetSignalRPort(string machineName, string ip);
    }
}
