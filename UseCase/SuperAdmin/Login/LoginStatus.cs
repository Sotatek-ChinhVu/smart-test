namespace UseCase.SuperAdmin.Login;

public enum LoginStatus : byte
{
    Successed = 1,
    InvalidLoginId = 2,
    InvalidPassWord = 3,
    Failed = 4
}
