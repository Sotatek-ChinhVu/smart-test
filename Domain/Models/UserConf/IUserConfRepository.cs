namespace Domain.Models.UserConf;

public interface IUserConfRepository
{
    List<UserConfModel> GetList(int userId, int fromGrpCd, int toGrpCd);

    Dictionary<string, int> GetList(int userId);

    void UpdateAdoptedByomeiConfig(int hpId, int userId, int adoptedValue);

    void UpdateUserConf(int hpId, int userId, int grpCd, int value);
}