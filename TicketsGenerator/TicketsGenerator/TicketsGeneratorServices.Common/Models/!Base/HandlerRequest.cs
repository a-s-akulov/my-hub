using MediatR;


namespace TicketsGeneratorServices.Common.Models.Base
{
    /// <summary>
    /// Пограничная модель связки с одним параметром
    /// </summary>
    /// <typeparam name="T1">Тип первого элемента</typeparam>
    /// <typeparam name="TResponse">Тип ответа</typeparam>
    /// <param name="First">Первый элемент</param>
    public record HandlerRequest<T1, TResponse>(T1 First) : IRequest<TResponse>;

    /// <summary>
    /// Пограничная модель связки с двумя параметрами
    /// </summary>
    /// <typeparam name="T1">Тип первого элемента</typeparam>
    /// <typeparam name="T2">Тип второго элемента</typeparam>
    /// <typeparam name="TResponse">Ответ</typeparam>
    /// <param name="First">Первый элемент</param>
    /// <param name="Second">Второй элемент</param>
    public record HandlerRequest<T1, T2, TResponse>(T1 First, T2 Second) : HandlerRequest<T1, TResponse>(First);

    /// <summary>
    /// Пограничная модель связки с тремя параметрами
    /// </summary>
    /// <typeparam name="T1">Тип первого элемента</typeparam>
    /// <typeparam name="T2">Тип второго элемента</typeparam>
    /// <typeparam name="T3">Тип третьего элемента</typeparam>
    /// <typeparam name="TResponse">Ответ</typeparam>
    /// <param name="First">Первый элемент</param>
    /// <param name="Second">Второй элемент</param>
    /// <param name="Third">Третий элемент</param>
    public record HandlerRequest<T1, T2, T3, TResponse>(T1 First, T2 Second, T3 Third) : HandlerRequest<T1, T2, TResponse>(First, Second);
}
