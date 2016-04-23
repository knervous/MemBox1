using NewtonSoft.Json;

namespace MemoryBox
{
    class SerializedMemory
    {
        [JsonProperty(PropertyName = "MemoryBox")]
        public string Data { get; set; }

    }
}