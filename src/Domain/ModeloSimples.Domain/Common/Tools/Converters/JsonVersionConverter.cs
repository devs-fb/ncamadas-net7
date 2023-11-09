namespace ModeloSimples.Domain.Common.Tools.Converters;

using ModeloSimples.Domain.Aggregates;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ModeloSimples.Domain.ValueObjects;

public class JsonVersionConverter : JsonConverter
{
    private const string Dados = "Dados";

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Versionamento);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var jObject = JObject.Load(reader);

        // Verifica se a propriedade "Dados" não tem o atributo [JsonIgnore]
        var dadosProperty = jObject.Property(Dados);
        if (dadosProperty != null && dadosProperty.HasValues)
        {
            // Deserializa a propriedade "Dados" para a lista de Pessoa
            var pessoas = dadosProperty.ToObject<IReadOnlyCollection<Pessoa>>(serializer);
            return new Versionamento(pessoas);
        }

        return null;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var versionamento = (Versionamento)value;

        // Serializa a propriedade "Dados" considerando atributos [JsonIgnore]
        var jObject = JObject.FromObject(versionamento, serializer);
        jObject.WriteTo(writer);
    }
}
