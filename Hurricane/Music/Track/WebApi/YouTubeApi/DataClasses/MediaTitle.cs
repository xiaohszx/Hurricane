﻿using Newtonsoft.Json;

namespace Hurricane.Music.Track.WebApi.YouTubeApi.DataClasses
{
    public class MediaTitle
    {
        [JsonProperty("$t")]
        public string Text { get; set; }
        public string type { get; set; }
    }
}
