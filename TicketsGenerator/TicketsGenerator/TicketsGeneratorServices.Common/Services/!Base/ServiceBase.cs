using System.Diagnostics;
using AutoMapper;


namespace TicketsGeneratorServices.Common.Services.Base
{
    public abstract class ServiceBase
    {
        #region Конструкторы

        public ServiceBase(ILogger logger, IMapper mapper, ActivitySource activitySource)
        {
            Log = logger;
            Map = mapper;
            Trace = activitySource;
        }

        #endregion Конструкторы


        #region Свойства

        /// <summary>
        /// Логгер
        /// </summary>
        protected ILogger Log { get; }

        /// <summary>
        /// Поставщик маппинга сущностей
        /// </summary>
        protected IMapper Map { get; }

        /// <summary>
        /// Поставщик трассировки
        /// </summary>
        protected ActivitySource Trace { get; }



        /// <summary>
        /// Текущая системная дата и время (UTC)
        /// </summary>
        protected DateTime NowSystemDate => DateTime.UtcNow; // UTC

        #endregion Свойства
    }
}
