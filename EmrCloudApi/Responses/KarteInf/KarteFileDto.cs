using UseCase.KarteInf.GetList;

namespace EmrCloudApi.Responses.KarteInf;

public class KarteFileDto
{
    public KarteFileDto(KarteFileOutputItem model)
    {
        Id = model.Id;
        FileName = model.FileName;
    }

    public long Id { get; private set; }

    public string FileName { get; private set; }
}
