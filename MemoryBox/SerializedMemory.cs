
using Newtonsoft.Json;

namespace MemoryBox

{
    class SerializedMemory
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "memorybox")]
        public string MemoryBox { get; set; }

    }
}