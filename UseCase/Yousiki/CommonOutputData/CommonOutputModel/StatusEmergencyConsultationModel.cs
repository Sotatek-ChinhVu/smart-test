using Domain.Models.Yousiki;

namespace UseCase.Yousiki.CommonOutputData.CommonOutputModel;

public class StatusEmergencyConsultationModel
{
    public StatusEmergencyConsultationModel(Yousiki1InfDetailModel emergencyConsultationDay, Yousiki1InfDetailModel destination, Yousiki1InfDetailModel consultationRoute, Yousiki1InfDetailModel outCome, bool isDeleted)
    {
        EmergencyConsultationDay = emergencyConsultationDay;
        Destination = destination;
        ConsultationRoute = consultationRoute;
        OutCome = outCome;
        IsDeleted = isDeleted;
    }

    public StatusEmergencyConsultationModel(Yousiki1InfDetailModel emergencyConsultationDay, Yousiki1InfDetailModel destination, Yousiki1InfDetailModel consultationRoute, Yousiki1InfDetailModel outCome)
    {
        EmergencyConsultationDay = emergencyConsultationDay;
        Destination = destination;
        ConsultationRoute = consultationRoute;
        OutCome = outCome;
        IsDeleted = false;
    }

    public Yousiki1InfDetailModel EmergencyConsultationDay { get; private set; }

    public Yousiki1InfDetailModel Destination { get; private set; }

    public Yousiki1InfDetailModel ConsultationRoute { get; private set; }

    public Yousiki1InfDetailModel OutCome { get; private set; }

    public bool IsDeleted { get; private set; }
}
