using UseCase.ReceSeikyu.ImportFile;

namespace EmrCloudApi.Responses.ReceSeikyu;

public class ImportFileReceSeikyuResponse
{
    public ImportFileReceSeikyuResponse(List<ReceInfoDto> receInfoList)
    {
        ReceInfoList = receInfoList;
    }

    public List<ReceInfoDto> ReceInfoList { get; private set; }
}
