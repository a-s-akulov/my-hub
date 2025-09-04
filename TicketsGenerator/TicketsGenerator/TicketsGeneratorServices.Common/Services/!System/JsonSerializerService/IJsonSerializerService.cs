namespace TicketsGeneratorServices.Common.Services.JsonSerializerService
{

    public interface IJsonSerializerService
    {
        public string Serialize<TValue>(TValue value);

        public TValue? Deserialize<TValue>(string json);
    }
}