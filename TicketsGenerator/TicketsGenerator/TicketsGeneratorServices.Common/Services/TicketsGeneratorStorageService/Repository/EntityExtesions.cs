using AutoMapper;
using TicketsGeneratorServices.Db.Entities.Base;


namespace TicketsGeneratorServices.Common.Services.TicketsGeneratorStorageService.Repository
{
    public static class EntityExtesions
    {
        public static TLogEntity ToLog<TLogEntity>(this object baseRecord, IMapper mapper, enLogOperation logOperation) where TLogEntity : class, ILogEntity
        {
            var logRecord = mapper.Map<TLogEntity>(baseRecord);
            logRecord.ChangedOperation = logOperation;

            return logRecord;
        }


        public static List<TLogEntity> ToLog<TLogEntity>(this IEnumerable<object> baseRecords, IMapper mapper, enLogOperation logOperation) where TLogEntity : class, ILogEntity
        {
            var logRecords = new List<TLogEntity>(10);
            foreach (var baseRecord in baseRecords)
                logRecords.Add(baseRecord.ToLog<TLogEntity>(mapper, logOperation));

            return logRecords;
        }
    }
}