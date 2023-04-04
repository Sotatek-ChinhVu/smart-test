namespace EmrCalculateApi.Requests
{
    public class GetRecedenDataRequest
    {
        public int Mode { get; set;}

        public int Sort { get; set;}

        public int HpId { get; set;}

        public int SeikyuYM { get; set;}

        public int OutputYM { get; set;}

        public int SeikyuKbnMode { get; set;}

        public int KaId { get; set;}

        public int TantoId { get; set;}

        public bool IncludeTester { get; set;}

        public bool IncludeOutDrug { get; set;}
    }
}
