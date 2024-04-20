using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.Encounters.Core.Domain.Encounters;

namespace Explorer.Encounters.Core.Domain.Converters
{
    public class SocialEncounterEventConverter : JsonConverter<List<DomainEvent>>
    {
        public static List<DomainEvent> Read(string json)
        {
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                JsonElement root = doc.RootElement;

                List<DomainEvent> events = new List<DomainEvent>();

                foreach (JsonElement element in root.EnumerateArray())
                {
                    string eventTypeProperty = "";
                    if (element.TryGetProperty("ActivationDate", out var g))
                        eventTypeProperty = "SocialEncounterActivated";
                    else if (element.TryGetProperty("CompletionDate", out var h))
                        eventTypeProperty = "SocialEncounterCompleted";
                    else if (element.TryGetProperty("DateOfCreation", out var j))
                        eventTypeProperty = "SocialEncounterCreated";
                    else if (element.TryGetProperty("DateOfUpdate", out var k))
                        eventTypeProperty = "SocialEncounterUpdated";
                    else if (element.TryGetProperty("RangeCheckedDate", out var l))
                        eventTypeProperty = "SocialEncounterChecked";
                    // Check if the "eventType" property exists in the JSON element
                    if (eventTypeProperty != "")
                    {
                        string eventType = eventTypeProperty;

                        switch (eventType)
                        {
                            case "SocialEncounterActivated":
                                events.Add(JsonSerializer.Deserialize<SocialEncounterActivated>(element.GetRawText()));
                                break;
                            case "SocialEncounterCompleted":
                                events.Add(JsonSerializer.Deserialize<SocialEncounterCompleted>(element.GetRawText()));
                                break;
                            case "SocialEncounterCreated":
                                events.Add(JsonSerializer.Deserialize<SocialEncounterCreated>(element.GetRawText()));
                                break;
                            case "SocialEncounterUpdated":
                                events.Add(JsonSerializer.Deserialize<SocialEncounterLocationUpdated>(element.GetRawText()));
                                break;
                            case "SocialEncounterChecked":
                                events.Add(JsonSerializer.Deserialize<SocialEncounterRangeChecked>(element.GetRawText()));
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        // Handle the case when the "eventType" property is missing
                        // You can log a warning or take appropriate action based on your requirements.
                    }
                }

                return events;
            }
        }


        public static string Write(List<DomainEvent> value)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartArray();

                    foreach (var domainEvent in value)
                    {
                        JsonSerializer.Serialize(writer, domainEvent, domainEvent.GetType());
                    }

                    writer.WriteEndArray();
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        public override List<DomainEvent>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, List<DomainEvent> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
