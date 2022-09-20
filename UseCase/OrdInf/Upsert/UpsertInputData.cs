using UseCase.Core.Sync.Core;

namespace UseCase.OrdInfs.Upsert
{
    public class UpsertInputData : IInputData<UpsertOutputData>
    {
        public int Status { get; private set; }
        public int SyosaiKbn { get; private set; }
        public int JikanKbn { get; private set; }
        public int HokenPid { get; private set; }
        public int SanteiKbn { get; private set; }
        public int TantoId { get; private set; }
        public int KaId { get; private set; }
    }
}
