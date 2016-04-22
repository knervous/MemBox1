using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MemoryBox
{
    class SerializedMemory
    {
        public string Data { get; set; }

        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }
    }
}