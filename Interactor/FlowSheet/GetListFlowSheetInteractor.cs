using Domain.Constant;
using Domain.Models.FlowSheet;
using Helper.Common;
using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.FlowSheet.GetList;

namespace Interactor.FlowSheet
{
    public class GetListFlowSheetInteractor : IGetListFlowSheetInputPort
    {
        private const int MAX_CALENDAR_MONTH = 12;

        private readonly IFlowSheetRepository _flowsheetRepository;
        public GetListFlowSheetInteractor(IFlowSheetRepository repository) { 
            _flowsheetRepository = repository;
        }
        public GetListFlowSheetOutputData Handle(GetListFlowSheetInputData inputData)
        {
            var flowsheetResult = _flowsheetRepository.GetListFlowSheet(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);
            var raiinListTags = _flowsheetRepository.GetRaiinListTags(inputData.HpId, inputData.PtId);
            var raiinListCmts = _flowsheetRepository.GetRaiinListCmts(inputData.HpId, inputData.PtId);
            var raiinListInf = _flowsheetRepository.GetRaiinListInfModels(inputData.HpId, inputData.PtId);
            var listRaiinNo = _flowsheetRepository.GetListRaiinNo(inputData.HpId, inputData.PtId, inputData.SinDate);
            foreach(FlowSheetModel model in flowsheetResult)
            {
                var raiinTag = raiinListTags?.FirstOrDefault(tag => tag.RaiinNo == model.RaiinNo && tag.SinDate == model.SinDate);
                if (raiinTag != null)
                {
                    model.RaiinListTag = new RaiinListTagModel(raiinTag);
                } 
                else
                {
                    model.RaiinListTag = new RaiinListTagModel(inputData.HpId, inputData.PtId, model.RaiinNo, model.SinDate);
                    model.RaiinListTag.IsAddNew = true;
                }
                var raiinCmt = raiinListCmts?.FirstOrDefault(cmt => cmt.RaiinNo == model.RaiinNo && cmt.SinDate == model.SinDate);
                if (raiinCmt != null)
                {
                    model.RaiinListCmt = new RaiinListCmtModel(raiinCmt);
                }
                else
                {
                    model.RaiinListCmt = new RaiinListCmtModel(inputData.HpId, inputData.PtId, model.RaiinNo, model.SinDate, 9, string.Empty);
                    model.RaiinListCmt.IsAddNew = true;
                }
                List<RaiinListInfModel> raiinList = new List<RaiinListInfModel>();
                var raiinListInfs = raiinListInf.FindAll(item => item.HpId == inputData.HpId
                                    && item.PtId == inputData.PtId
                                    && item.SinDate == model.SinDate
                                    && ((model.Status != 0 ? item.RaiinNo == model.RaiinNo : item.RaiinNo == 0) || (item.RaiinNo == 0 && item.RaiinListKbn == 4)))
                                    .OrderBy(item => item.SortNo)
                                    .ToList();
                foreach (var item in raiinListInfs)
                {
                    if (raiinList.Any(r => r.GrpId == item.GrpId)) continue;
                    raiinList.Add(new RaiinListInfModel(item)
                    {
                        IsContainsFile = raiinListInfs?.FirstOrDefault(x => x.GrpId == item.GrpId && x.KbnCd == item.KbnCd && item.RaiinListKbn == 4) != null
                    });
                }
                model.RaiinListInfs = raiinListInfs;
            }

            var _raiinListMst = _flowsheetRepository.GetRaiinListMsts(inputData.HpId);

            var _tempHolidayCollection = _flowsheetRepository.GetHolidayMst(inputData.HpId);
            var _staticCalendarDataSource = new List<CalendarGridModel>();
            DateTime now = CIUtil.IntToDate(inputData.SinDate);
            for (int i = 0; i < MAX_CALENDAR_MONTH; i++)
            {
                DateTime tempDate = now.AddMonths(-i);

                CalendarGridModel calendarGridVM = new CalendarGridModel(tempDate);
                _staticCalendarDataSource.Insert(0, calendarGridVM);
            }
            foreach (CalendarGridModel model in _staticCalendarDataSource)
            {
                SetCalendarItemList(model, _tempHolidayCollection, inputData.SinDate, flowsheetResult, listRaiinNo);
            }
            return new GetListFlowSheetOutputData(flowsheetResult, _raiinListMst, _staticCalendarDataSource);
        }
        public void SetCalendarItemList(CalendarGridModel model, List<HolidayModel> tempHolidayCollection, int sinDate, List<FlowSheetModel> flowSheetList, List<RaiinDateModel> listRaiinNo)
        {
            int startDate = model.MonthYear.Year * 10000 + model.MonthYear.Month * 100;
            int endDate = model.MonthYear.AddMonths(1).Year * 10000 + model.MonthYear.AddMonths(1).Month * 100;
            if (startDate < sinDate && endDate > sinDate)
            {
                model.SinDate = sinDate;
            }

            var list = tempHolidayCollection.Where(item => item.SinDate < endDate && item.SinDate > startDate)
                .Select(item => new HolidayModel(item.SinDate % 100, item.HolidayKbn, item.KyusinKbn, item.HolidayName)).ToList();

            model.HolidayModels = list;

            List<KeyValuePair<int, int>> tempList = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, string>> tagList = new List<KeyValuePair<int, string>>();
            List<KeyValuePair<int, RaiinStateDictObjectValue>> stateList = new();
            foreach (var item in flowSheetList)
            {
                if (item.SinDate < endDate && item.SinDate > startDate)
                {
                    tempList.Add(new KeyValuePair<int, int>(item.SinDate, item.SyosaisinKbn));
                    stateList.Add(new KeyValuePair<int, RaiinStateDictObjectValue>(item.SinDate, new(item.Status, listRaiinNo.FirstOrDefault(c => c.SinDate == item.SinDate)?.RaiinNo ?? 0)));
                    if (!string.IsNullOrEmpty(item.RaiinListTag.StarResource))
                    {
                        tagList.Add(new KeyValuePair<int, string>(item.SinDate, item.RaiinListTag.StarResource));
                    }
                }
            }
            model.RaiinStateDict = stateList;
            model.RaiinTags = tagList;
            model.RaiinDayDict = tempList;
            CreateCalendarDate(model);
        }
        private void CreateCalendarDate(CalendarGridModel model)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (model.Year == 0 || model.Month == 0) return;
            DateTime firstDay = new DateTime(model.Year, model.Month, 1);

            int rowCount = 0;

            while (rowCount < 6)
            {
                int days = firstDay.DayOfWeek - DayOfWeek.Monday + 1;
                DateTime start = firstDay.AddDays(-days);
                WeekOfMonthModel weekOfMonth;
                if (rowCount < model.WeekOfMonths.Count)
                {
                    weekOfMonth = model.WeekOfMonths[rowCount];
                }
                else
                {
                    weekOfMonth = new WeekOfMonthModel();
                }

                for (int i = 0; i <= 6; i++)
                {
                    DateTime current = start.AddDays(i);
                    DayInfo dayInfo;
                    switch (current.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            dayInfo = weekOfMonth.Mon;
                            break;
                        case DayOfWeek.Tuesday:
                            dayInfo = weekOfMonth.Tue;
                            break;
                        case DayOfWeek.Wednesday:
                            dayInfo = weekOfMonth.Wed;
                            break;
                        case DayOfWeek.Thursday:
                            dayInfo = weekOfMonth.Thu;
                            break;
                        case DayOfWeek.Friday:
                            dayInfo = weekOfMonth.Fri;
                            break;
                        case DayOfWeek.Saturday:
                            dayInfo = weekOfMonth.Sat;
                            break;
                        case DayOfWeek.Sunday:
                            dayInfo = weekOfMonth.Sun;
                            break;
                        default:
                            dayInfo = new DayInfo();
                            break;
                    }

                    if (dayInfo == null)
                    {
                        dayInfo = new DayInfo();
                    }
                    dayInfo.Day = string.Empty;
                    dayInfo.Date = 0;
                    dayInfo.Background = EmrCalendarDateColor.Default;
                    dayInfo.ToolTip = string.Empty;
                    if (current.Month == model.Month)
                    {
                        dayInfo.Day = current.Day.AsString();

                        UpdateDayInfo(dayInfo, model);

                        switch (current.DayOfWeek)
                        {
                            case DayOfWeek.Monday:
                                weekOfMonth.Mon = dayInfo;
                                break;
                            case DayOfWeek.Tuesday:
                                weekOfMonth.Tue = dayInfo;
                                break;
                            case DayOfWeek.Wednesday:
                                weekOfMonth.Wed = dayInfo;
                                break;
                            case DayOfWeek.Thursday:
                                weekOfMonth.Thu = dayInfo;
                                break;
                            case DayOfWeek.Friday:
                                weekOfMonth.Fri = dayInfo;
                                break;
                            case DayOfWeek.Saturday:
                                dayInfo.Foreground = EmrCalendarDateColor.BlueForeGround;
                                weekOfMonth.Sat = dayInfo;
                                break;
                            case DayOfWeek.Sunday:
                                dayInfo.Foreground = EmrCalendarDateColor.RedForeGround;
                                weekOfMonth.Sun = dayInfo;
                                break;
                        }
                    }

                    UpdateBorder(dayInfo, model);
                }
                if (rowCount < model.WeekOfMonths.Count)
                {
                    model.WeekOfMonths[rowCount] = weekOfMonth;
                }
                else
                {
                    model.WeekOfMonths.Add(weekOfMonth);
                }
                firstDay = start.AddDays(7);
                rowCount++;
            }
            watch.Stop();
        }
        private void UpdateBorder(DayInfo dayInfo, CalendarGridModel model)
        {
            if (model.HolidayModels == null) return;
            dayInfo.BorderBrush = EmrCalendarDateColor.Default;
            int day = dayInfo.Day.AsInteger();
            int date = model.Year * 10000 + model.Month * 100 + day;
            var calendarHolidayModel = model.HolidayModels.FirstOrDefault(item => item.SinDate == day);
            if (calendarHolidayModel != null && calendarHolidayModel.KyusinKbn == 1 && !(model.HighlightToday && date == model.SinDate))
            {
                dayInfo.BorderBrush = EmrCalendarDateColor.RedBorder;
            }
            if (model.HighlightToday && date == model.SinDate)
            {
                dayInfo.IsToday = true;
            }
            else
            {
                dayInfo.IsToday = false;
            }
        }
        private void UpdateDayInfo(DayInfo dayInfo, CalendarGridModel model)
        {
            void GetBackgroud(KeyValuePair<int, int> dateSyosaiItem)
            {
                switch (dateSyosaiItem.Value)
                {
                    case SyosaiConst.Syosin:
                    case SyosaiConst.Syosin2:
                        dayInfo.Background = EmrCalendarDateColor.HasFirstVisit;
                        break;
                    case SyosaiConst.Saisin:
                    case SyosaiConst.Saisin2:
                    case SyosaiConst.SaisinDenwa:
                    case SyosaiConst.SaisinDenwa2:
                        dayInfo.Background = EmrCalendarDateColor.CalHasCallAgain;
                        break;
                    case SyosaiConst.None:
                        dayInfo.Background = EmrCalendarDateColor.CalHasData;
                        break;
                    case SyosaiConst.NextOrder:
                        dayInfo.Background = EmrCalendarDateColor.CalNextOrder;
                        break;
                    case SyosaiConst.Jihi:
                        dayInfo.Background = EmrCalendarDateColor.HasOwnExpense;
                        break;
                }
            }
            dayInfo.Background = EmrCalendarDateColor.Default;
            dayInfo.Foreground = EmrCalendarDateColor.NormalForeGround;

            int day = dayInfo.Day.AsInteger();
            int date = model.Year * 10000 + model.Month * 100 + day;

            // Set full date
            dayInfo.Date = date;

            // Set background and tooltip

            if (model.HolidayModels != null)
            {
                var calendarHolidayModel = model.HolidayModels.FirstOrDefault(item => item.SinDate == day);

                if (calendarHolidayModel != null)
                {
                    switch (calendarHolidayModel.HolidayKbn)
                    {
                        case 1:
                            dayInfo.Background = EmrCalendarDateColor.HolidayRed;
                            dayInfo.Foreground = EmrCalendarDateColor.RedForeGround;
                            break;
                        case 2:
                            dayInfo.Background = EmrCalendarDateColor.Holiday;
                            break;
                    }

                    if (!string.IsNullOrEmpty(calendarHolidayModel.HolidayName))
                    {
                        dayInfo.ToolTip = string.Format("{0} {1}", CIUtil.IntToDate(dayInfo.Date).ToString("MM/dd"), calendarHolidayModel.HolidayName);
                    }
                }
            }

            var dateSyosaiItems = model.RaiinDayDict.Where(item => item.Key == date);
            var datetateItem = model.RaiinStateDict.FirstOrDefault(item => item.Key == date);
            foreach (var dateSyosaiItem in dateSyosaiItems)
            {
                if (!dateSyosaiItem.Equals(default(KeyValuePair<int, int>)))
                {
                    if (!(!datetateItem.Equals(default(KeyValuePair<int, int>)) && date == model.SinDate && datetateItem.Value.Value < RaiinState.TempSave))
                    {
                        dayInfo.ToolTip = (string.IsNullOrEmpty(dayInfo.ToolTip) ? "" : dayInfo.ToolTip + Environment.NewLine) + SyosaiConst.FlowSheetCalendarDict[dateSyosaiItem.Value];
                    }

                    GetBackgroud(dateSyosaiItem);
                }
            }

            var dateSyosaiNoneStatus = model.RaiinDayDict.FirstOrDefault(item => item.Key == date && item.Value == SyosaiConst.None);
            if (!dateSyosaiNoneStatus.Equals(default(KeyValuePair<int, int>))) GetBackgroud(dateSyosaiNoneStatus);
            var dateSyosaiJihiStatus = model.RaiinDayDict.FirstOrDefault(item => item.Key == date && item.Value == SyosaiConst.Jihi);
            if (!dateSyosaiJihiStatus.Equals(default(KeyValuePair<int, int>))) GetBackgroud(dateSyosaiJihiStatus);
            var dateSyosaiSaisinStatus = model.RaiinDayDict.FirstOrDefault(item => item.Key == date && item.Value == SyosaiConst.Saisin);
            if (!dateSyosaiSaisinStatus.Equals(default(KeyValuePair<int, int>))) GetBackgroud(dateSyosaiSaisinStatus);
            var dateSyosaiSyosinStatus = model.RaiinDayDict.FirstOrDefault(item => item.Key == date && item.Value == SyosaiConst.Syosin);
            if (!dateSyosaiSyosinStatus.Equals(default(KeyValuePair<int, int>))) GetBackgroud(dateSyosaiSyosinStatus);

            var dateTagItem = model.RaiinTags.FirstOrDefault(item => item.Key == date);
            if (!dateTagItem.Equals(default(KeyValuePair<int, string>)) && !string.IsNullOrEmpty(dateTagItem.Value))
            {
                dayInfo.ToolTip = dayInfo.ToolTip;
            }
            if (model.HighlightToday && date == model.SinDate)
            {
                dayInfo.Background = EmrCalendarDateColor.Default;
            }
        }
    }
}
