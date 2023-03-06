using Domain.Models.SystemGenerationConf;
using Domain.Models.TimeZone;
using Domain.Models.TodayOdr;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using UseCase.MedicalExamination.GetDefaultSelectedTime;

namespace Interactor.MedicalExamination
{
    public class GetDefaultSelectedTimeInteractor : IGetDefaultSelectedTimeInputPort
    {
        private readonly ITimeZoneRepository _timezoneRepository;
        private readonly ITodayOdrRepository _todayOdrRepository;
        private readonly ISystemGenerationConfRepository _generationRepository;

        public GetDefaultSelectedTimeInteractor(ITimeZoneRepository timezoneRepository, ITodayOdrRepository todayOdrRepository, ISystemGenerationConfRepository generationRepository)
        {
            _timezoneRepository = timezoneRepository;
            _todayOdrRepository = todayOdrRepository;
            _generationRepository = generationRepository;
        }

        public GetDefaultSelectedTimeOutputData Handle(GetDefaultSelectedTimeInputData inputData)
        {
            try
            {
                if (inputData.SinDate <= 0)
                {
                    return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidSinDate, 0, string.Empty, 0, 0, 0);
                }
                if (inputData.HpId <= 0)
                {
                    return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidHpId, 0, string.Empty, 0, 0, 0);
                }
                if (inputData.UketukeTime < 0)
                {
                    return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidUketukeTime, 0, string.Empty, 0, 0, 0);
                }
                if (inputData.BirthDay < 0)
                {
                    return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidBirthDay, 0, string.Empty, 0, 0, 0);
                }
                if (inputData.DayOfWeek < 0 || inputData.DayOfWeek > 6)
                {
                    return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.InvalidDayOfWeek, 0, string.Empty, 0, 0, 0);
                }

                var result = GetDefaultValue(inputData.HpId, inputData.SinDate, inputData.UketukeTime, inputData.BirthDay, inputData.DayOfWeek, inputData.FromOutOfSystem);

                return new GetDefaultSelectedTimeOutputData(GetDefaultSelectedTimeStatus.Successed, result.result, result.message, result.timeKbnForChild, result.currentTimeKbn, result.beforeTimeKbn);
            }
            finally
            {
                _timezoneRepository.ReleaseResource();
                _todayOdrRepository.ReleaseResource();
                _generationRepository.ReleaseResource();
            }
        }

        private bool IsPatientChildren(int hpId, int birthDay, int sinDate)
        {
            if (_generationRepository.GetSettingValue(hpId, 8001, 0, sinDate).Item1 == 1)
            {
                int age = CIUtil.SDateToAge(birthDay, sinDate);
                if (age >= 0 && age < 6)
                {
                    return true;
                }
            }
            return false;
        }

        private (int result, string message, int timeKbnForChild, int beforeTimeKbn, int currentTimeKbn) GetDefaultValue(int hpId, int sinDate, int uketukeTime, int birthDay, int dayOfWeek, bool fromOutOfSystem)
        {
            //Child Patient
            int timeKbnForChild = 0;
            bool isHoliday = _todayOdrRepository.IsHolidayForDefaultTime(hpId, sinDate);
            if (IsPatientChildren(hpId, birthDay, sinDate))
            {
                if (isHoliday && uketukeTime >= 600 && uketukeTime < 2200)
                {
                    return (JikanConst.KyujituKotoku, string.Empty, 0, 0, 0);
                }
                //夜間小特 : 6h-8h or 18h-22h
                if ((uketukeTime >= 600 && uketukeTime < 800) ||
                    ((dayOfWeek == 7 ? uketukeTime >= 1200 : uketukeTime >= 1800) && uketukeTime < 2200))
                {
                    timeKbnForChild = JikanConst.YakanKotoku;
                }
                //深夜小特 : 22h-6h
                else if (uketukeTime >= 2200 || uketukeTime < 600)
                {
                    timeKbnForChild = JikanConst.SinyaKotoku;
                }
            }

            //Adult Patient
            if (isHoliday)
            {
                return (JikanConst.Kyujitu, string.Empty, 0, 0, 0);
            }
            if (sinDate != DateTime.Now.ToString("yyyyMMdd").AsInteger())
            {
                return (JikanConst.JikanNai, string.Empty, 0, 0, 0);
            }
            var timeZoneConfs = _timezoneRepository.GetTimeZoneConfs(hpId);
            if (timeZoneConfs == null)
            {
                return (JikanConst.JikanNai, string.Empty, 0, 0, 0);
            }
            int currentTimeKbn = 0, beforeTimeKbn = 0;
            var timeZoneConf = timeZoneConfs.FirstOrDefault(t => t.YoubiKbn == dayOfWeek && t.StartTime <= uketukeTime && t.EndTime > uketukeTime);
            if (timeZoneConf != null)
            {
                currentTimeKbn = timeZoneConf.TimeKbn;
            }
            var timeZoneDayInf = _timezoneRepository.GetLatestTimeZoneDayInf(hpId, sinDate, uketukeTime);
            if (timeZoneDayInf != null)
            {
                beforeTimeKbn = timeZoneDayInf.TimeKbn;
            }
            string timeKbnName = JikanConst.JikanDict[currentTimeKbn];
            string text = string.Empty;
            if (fromOutOfSystem && timeZoneConf != null && timeZoneDayInf != null && uketukeTime < timeZoneDayInf.StartTime)
            {
                text = Environment.NewLine + "時間加算を算定できる時間になりました。" + Environment.NewLine
                           + "この患者から、" + timeKbnName + "加算を算定するように変更しますか？" + Environment.NewLine + Environment.NewLine
                           + (timeKbnName + "加算設定").PadRight(9, '　')
                           + Environment.NewLine + "        マスター設定  " + CIUtil.TimeToShowTime(timeZoneConf.StartTime)
                           + "-" + CIUtil.TimeToShowTime(timeZoneConf.EndTime)
                           + Environment.NewLine + "        今日の設定  " + CIUtil.TimeToShowTime(timeZoneDayInf.StartTime)
                           + "-" + CIUtil.TimeToShowTime(timeZoneDayInf.EndTime == 0 ? timeZoneConf.EndTime : timeZoneDayInf.EndTime)
                           + Environment.NewLine + "来院時間".PadRight(9, '　') + CIUtil.TimeToShowTime(uketukeTime);
            }
            else if (currentTimeKbn != beforeTimeKbn)
            {
                //change out of time => in of time
                if (currentTimeKbn == JikanConst.JikanNai)
                {
                    text = "時間加算を算定できない時間になりました。" + Environment.NewLine
                                                                            + "この患者から、" + timeKbnName + "に変更しますか？" + Environment.NewLine
                                                                            + Environment.NewLine + "来院時間".PadRight(9, '　') + CIUtil.TimeToShowTime(uketukeTime);
                }
                //change in of time => out of time
                else
                {
                    string startTime = "", endTime = "";
                    startTime = CIUtil.TimeToShowTime(timeZoneConf?.StartTime ?? 0);
                    endTime = CIUtil.TimeToShowTime(timeZoneConf?.EndTime ?? 0);
                    text = "時間加算を算定できる時間になりました。" + Environment.NewLine
                                                                           + "この患者から、" + timeKbnName + "加算を算定するように変更しますか？" + Environment.NewLine + Environment.NewLine
                                                                           + (timeKbnName + "加算設定").PadRight(9, '　') + startTime + "-" + endTime + Environment.NewLine
                                                                           + "来院時間".PadRight(9, '　') + CIUtil.TimeToShowTime(uketukeTime);
                }
            }

            if (currentTimeKbn == 0) return (currentTimeKbn, text, timeKbnForChild, beforeTimeKbn, currentTimeKbn);
            if (timeKbnForChild > 0 &&
              (timeKbnForChild == JikanConst.YakanKotoku && currentTimeKbn == JikanConst.Yasou) ||
              (timeKbnForChild == JikanConst.SinyaKotoku && currentTimeKbn == JikanConst.Sinya))
            {
                return (timeKbnForChild, text, timeKbnForChild, beforeTimeKbn, currentTimeKbn);
            }

            return (currentTimeKbn, text, timeKbnForChild, beforeTimeKbn, currentTimeKbn);
        }
    }
}
