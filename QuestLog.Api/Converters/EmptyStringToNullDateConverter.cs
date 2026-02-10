using System.Text.Json;
using System.Text.Json.Serialization;

public class EmptyStringToNullDateConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Якщо прийшов рядок
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            // Якщо він пустий — повертаємо null
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }
            // Якщо не пустий — пробуємо розпарсити дату
            if (DateTime.TryParse(value, out var date))
            {
                return date;
            }
        }
        
        // В інших випадках (наприклад, прийшов реальний null)
        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value);
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}