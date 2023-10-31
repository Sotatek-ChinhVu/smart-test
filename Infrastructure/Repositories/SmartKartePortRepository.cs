using Domain.Models.SmartKartePort;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class SmartKartePortRepository : RepositoryBase, ISmartKartePortRepository
    {
        public SmartKartePortRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }
        public bool UpdateSignalRPort(int userId, SmartKarteAppSignalRPortModel signalRPortModel)
        {
            var signalRPort = TrackingDataContext.SmartKarteAppSignalRPorts.FirstOrDefault(i => i.Ip == signalRPortModel.Ip && i.MachineName == signalRPortModel.MachineName);
            if (signalRPort != null)
            {
                signalRPort.PortNumber = signalRPortModel.PortNumber;
                _UpdateSignalRPort(userId, signalRPort);
                TrackingDataContext.SmartKarteAppSignalRPorts.Update(signalRPort);
            }
            else
            {
                var smartKarteAppSignalRPort = new SmartKarteAppSignalRPort();
                smartKarteAppSignalRPort.PortNumber = signalRPortModel.PortNumber;
                smartKarteAppSignalRPort.MachineName = signalRPortModel.MachineName;
                smartKarteAppSignalRPort.Ip = signalRPortModel.Ip;
                _CreateSignalRPort(userId, smartKarteAppSignalRPort);
                TrackingDataContext.SmartKarteAppSignalRPorts.Add(smartKarteAppSignalRPort);
            }
            TrackingDataContext.SaveChanges();
            return TrackingDataContext.SaveChanges() > 0;
        }

        public SmartKarteAppSignalRPortModel GetSignalPort(string machineName, string ip)
        {
            var signalRPort = TrackingDataContext.SmartKarteAppSignalRPorts.FirstOrDefault(i => i.Ip == ip && i.MachineName == machineName);
            if (signalRPort != null)
            {
                return new SmartKarteAppSignalRPortModel(signalRPort.PortNumber, signalRPort.MachineName, signalRPort.Ip);
            }
            else
            {
                return new SmartKarteAppSignalRPortModel();
            }
        }

        private void _UpdateSignalRPort(int userId, SmartKarteAppSignalRPort jihiSbtMst)
        {
            jihiSbtMst.CreateDate = TimeZoneInfo.ConvertTimeToUtc(jihiSbtMst.CreateDate);
            jihiSbtMst.UpdateId = userId;
            jihiSbtMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
        }

        private void _CreateSignalRPort(int userId, SmartKarteAppSignalRPort jihiSbtMst)
        {
            jihiSbtMst.CreateDate = CIUtil.GetJapanDateTimeNow();
            jihiSbtMst.CreateId = userId;
            jihiSbtMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
            jihiSbtMst.UpdateId = userId;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
