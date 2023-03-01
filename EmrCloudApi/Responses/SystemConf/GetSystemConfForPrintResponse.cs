using System.Collections;

namespace EmrCloudApi.Responses.SystemConf
{
    public class GetSystemConfForPrintResponse
    {
        public GetSystemConfForPrintResponse(Hashtable values)
        {
            Values = values;
        }

        public Hashtable Values { get; private set; }
    }
}
