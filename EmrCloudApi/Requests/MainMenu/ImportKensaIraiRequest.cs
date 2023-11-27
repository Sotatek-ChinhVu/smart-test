namespace EmrCloudApi.Requests.MainMenu;

public class ImportKensaIraiRequest
{
    public ImportKensaIraiRequest(Stream datFile)
    {
        DatFile = datFile;
    }

    public Stream DatFile { get; set; }
}
