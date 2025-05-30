using System;
using Newtonsoft.Json;
using UnityEngine;

public class PrefabConverter : JsonConverter<GameObject>
{
    public override void WriteJson(JsonWriter writer, GameObject value, JsonSerializer serializer)
    {
        writer.WriteValue(value.name);
    }

    public override GameObject ReadJson(JsonReader reader, Type objectType, GameObject existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        /*string address = reader.Value as string;
        
        return string.IsNullOrEmpty(address) ? null : GameResource<GameObject>.GetResource(address);*/
        return null;
    }
}