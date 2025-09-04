using System.Diagnostics;
using AutoMapper;


namespace TicketsGeneratorServices.Api.RequestHandlers.Base
{
    public abstract class RequestHandlerBase
    {

        #region Поля



        #endregion Поля


        #region Конструкторы

        public RequestHandlerBase(ILogger logger, IMapper mapper, ActivitySource activitySource)
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

        #endregion Свойства


        #region Методы



        #endregion Методы
    }
}
