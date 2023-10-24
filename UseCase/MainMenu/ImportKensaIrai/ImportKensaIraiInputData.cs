using Helper.Messaging;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.ImportKensaIrai;

public class ImportKensaIraiInputData : IInputData<ImportKensaIraiOutputData>
{
    public ImportKensaIraiInputData(int hpId, int userId, IMessenger messenger, Stream datFile)
    {
        HpId = hpId;
        UserId = userId;
        DatFile = datFile;
        Messenger = messenger;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public IMessenger Messenger { get; private set; }

    public Stream DatFile { get; private set; }
}
