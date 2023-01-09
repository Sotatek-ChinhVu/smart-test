using Infrastructure.Repositories;
using Interactor.MedicalExamination;

namespace CloudUnitTest
{
    public class Tests : BaseUT
    {
        [Test]
        public void Test1()
        {
            MstItemRepository mstItemRepository = new MstItemRepository(TenantProvider);

            CheckedSpecialItemInteractor checkedSpecialItemInteractor =
                new CheckedSpecialItemInteractor(null, mstItemRepository, null, null, null);

            Assert.Pass();
        }
    }
}