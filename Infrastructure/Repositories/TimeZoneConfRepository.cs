using Domain.Models.TimeZoneConf;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class TimeZoneConfRepository : RepositoryBase, ITimeZoneConfRepository
    {
        public TimeZoneConfRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

        public List<TimeZoneConfGroupModel> GetTimeZoneConfGroupModels(int hpId)
        {
            var timeZoneConfEntities = NoTrackingDataContext.TimeZoneConfs.Where(x => x.HpId == hpId && x.IsDelete == 0).OrderBy(x => x.YoubiKbn);

            var result = timeZoneConfEntities
                        .GroupBy(x => x.YoubiKbn)
                        .AsEnumerable()
                        .Select(x => new TimeZoneConfGroupModel(
                            x.Key,
                            x.OrderBy((o) => o.StartTime).Select((detail, index) => new TimeZoneConfDetailModel(detail.HpId,
                                                                                                                index + 1,
                                                                                                                detail.YoubiKbn,
                                                                                                                detail.StartTime,
                                                                                                                detail.EndTime,
                                                                                                                detail.SeqNo,
                                                                                                                detail.TimeKbn,
                                                                                                                detail.IsDelete,
                                                                                                                false))
                        )).ToList();

            List<int> dayOfWeek = new List<int> { 1, 2, 3, 4, 5, 6, 7 };

            foreach (var day in dayOfWeek)
            {
                var group = result.Where(x => x.YoubiKbn == day).FirstOrDefault();
                if (group == null)
                {
                    result.Add(
                        new TimeZoneConfGroupModel(
                            day,
                            new List<TimeZoneConfDetailModel>() { new TimeZoneConfDetailModel(hpId, 0, day, 0, 0, 0, 0, 0,false) }
                    ));
                }
            }
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
