using EventProcessor.Model;

namespace EventProcessor.Interfaces;

public interface IEventProcessorService
{
    bool DoEvent(List<ArgumentModel> listAuditTraiLogModels);
}
