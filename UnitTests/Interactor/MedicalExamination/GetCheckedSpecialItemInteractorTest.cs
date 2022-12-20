using Domain.Models.User;
using Helper.Constants;
using Interactor.User;
using Moq;
using UseCase.User.GetByLoginId;

namespace UnitTests.Interactor.MedicalExamination;

public class GetCheckedSpecialItemInteractorTest
{
    [Fact]
    public void ReturnsSuccess_ForValidLoginId()
    {
        // Arrange
        var testLoginId = "test login id";
        var mockUserRepo = new Mock<IUserRepository>();
        mockUserRepo.Setup(repo => repo.GetByLoginId(testLoginId))
            .Returns(new UserMstModel(1, 1, 1, 1, 1, 1, string.Empty, string.Empty,
                string.Empty, string.Empty, testLoginId, string.Empty,
                string.Empty, 1, 1, 1, string.Empty, DeleteTypes.None));
        var interactor = new GetUserByLoginIdInteractor(mockUserRepo.Object);
        var input = new GetUserByLoginIdInputData(testLoginId);

        // Act
        var output = interactor.Handle(input);

        // Assert
        Assert.Equal(GetUserByLoginIdStatus.Success, output.Status);
        Assert.NotNull(output.User);
    }
}
