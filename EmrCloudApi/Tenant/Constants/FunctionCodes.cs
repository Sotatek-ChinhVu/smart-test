namespace EmrCloudApi.Tenant.Constants;

public static class FunctionCodes
{
    // Toàn bộ thông tin tiếp nhận thay đổi, vd như mở màn hình thông tin tiếp nhận để add hoặc update
    public const string ReceptionChanged = "ReceptionChanged";
    // Edit trực tiếp trên visiting list như thay đổi số thứ tự, status, thời gian tiếp nhận...
    public const string RaiinInfChanged = "RaiinInfChanged";
    // Toàn bộ thông tin của bệnh nhân thay đổi như tên tuổi, giới tính, thông tin pattern...
    public const string PatientInfChanged = "PatientInfChanged";
    // Thông tin bác sĩ thay đổi ở màn hình quản lý User
    public const string UserMstChanged = "UserMstChanged";
    // Thông tin phần dynamic cell thay đổi ở grid visitingList
    public const string RaiinKubunChanged = "RaiinKubunChanged";
    // Thông tin lock thay đổi nếu mở một số màn  hình như Medical
    public const string LockInfChanged = "LockInfChanged";
    // Thông tin comment cho 1 lần đến viện thay đổi từ visitinglist
    public const string RaiinCmtChanged = "RaiinCmtChanged";
    // Thông tin comment cho 1 bệnh nhân thay đổi từ visitinglist
    public const string PatientCmtChanged = "PatientCmtChanged";
    // Thông tin đặt lịch thay đổi
    public const string ReservationChanged = "ReservationChanged";
}
