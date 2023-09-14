namespace EmrCloudApi.Responses.MstItem
{
    public class GetContainerMstsResponse
    {
        public GetContainerMstsResponse(Dictionary<int, string> containerMsts)
        {
            ContainerMsts = containerMsts;
        }

        public Dictionary<int, string> ContainerMsts { get; private set; }
    }
}
