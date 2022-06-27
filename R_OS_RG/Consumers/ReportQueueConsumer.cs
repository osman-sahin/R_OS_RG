using MassTransit;
using R_OS.Models;
using R_OS_RG.Services;

namespace R_OS_RG.Consumers
{
    internal class ReportQueueConsumer : IConsumer<ReportQueueModel>
    {
        private readonly IRestService _restService;
        public ReportQueueConsumer(IRestService restService)
        {
            _restService = restService;
        }
        public async Task Consume(ConsumeContext<ReportQueueModel> context)
        {
            List<ContactInformation> contactInfos = await _restService.GetReportData(context.Message.ReportUUID);

            if (contactInfos.Count > 0)
            {
                var contactInfosByLocation = contactInfos.GroupBy(c => c.Location)
                    .Select(grp => grp.ToList()).ToList();
            }
        }


        private async Task GenerateData(List<List<ContactInformation>> data)
        {
            var x = data;
            var y = data;
        }
    }
}
