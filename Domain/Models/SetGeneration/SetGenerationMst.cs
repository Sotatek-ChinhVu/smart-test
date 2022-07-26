namespace Domain.Models.SetGeneration
{
    public class SetGenerationMst
    {
        public SetGenerationMst(int hpId, int generationId, int startDate, int isDeleted)
        {
            HpId = hpId;
            GenerationId = generationId;
            StartDate = startDate;
            IsDeleted = isDeleted;
        }

        public int HpId { get; private set; }
        public int GenerationId { get; private set; }
        public int StartDate { get; private set; }
        public int IsDeleted { get; private set; }
    }
}
