using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace app.Models
{
    public class Notes
    {
        [JsonProperty(PropertyName = "conten")]
        public string Content { get; set; }
    }
}
