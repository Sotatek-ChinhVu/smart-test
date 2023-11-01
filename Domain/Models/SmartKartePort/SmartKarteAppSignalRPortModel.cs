using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SmartKartePort
{
    public class SmartKarteAppSignalRPortModel
    {
        public SmartKarteAppSignalRPortModel() { }
        public SmartKarteAppSignalRPortModel(int portNumber, string? machineName, string? ip)
        {
            PortNumber = portNumber;
            MachineName = machineName;
            Ip = ip;
        }
        public int PortNumber { get; private set; }
        public string? MachineName { get; private set; }
        public string? Ip { get; private set; }
    }
}
