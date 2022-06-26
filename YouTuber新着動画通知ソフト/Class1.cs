using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTuber新着動画通知ソフト
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Author
    {
        public string name { get; set; }
        public string uri { get; set; }
    }

    public class Entry
    {
        public string id { get; set; }

        [JsonProperty("yt:videoId")]
        public string YtVideoId { get; set; }

        [JsonProperty("yt:channelId")]
        public string YtChannelId { get; set; }
        public string title { get; set; }
        public Link link { get; set; }
        public Author author { get; set; }
        public DateTime published { get; set; }
        public DateTime updated { get; set; }

        [JsonProperty("media:group")]
        public MediaGroup MediaGroup { get; set; }
    }

    public class Feed
    {
        [JsonProperty("@xmlns:yt")]
        public string XmlnsYt { get; set; }

        [JsonProperty("@xmlns:media")]
        public string XmlnsMedia { get; set; }

        [JsonProperty("@xmlns")]
        public string Xmlns { get; set; }
        public List<Link> link { get; set; }
        public string id { get; set; }

        [JsonProperty("yt:channelId")]
        public string YtChannelId { get; set; }
        public string title { get; set; }
        public Author author { get; set; }
        public DateTime published { get; set; }
        public List<Entry> entry { get; set; }
    }

    public class Link
    {
        [JsonProperty("@rel")]
        public string Rel { get; set; }

        [JsonProperty("@href")]
        public string Href { get; set; }
    }

    public class MediaCommunity
    {
        [JsonProperty("media:starRating")]
        public MediaStarRating MediaStarRating { get; set; }

        [JsonProperty("media:statistics")]
        public MediaStatistics MediaStatistics { get; set; }
    }

    public class MediaContent
    {
        [JsonProperty("@url")]
        public string Url { get; set; }

        [JsonProperty("@type")]
        public string Type { get; set; }

        [JsonProperty("@width")]
        public string Width { get; set; }

        [JsonProperty("@height")]
        public string Height { get; set; }
    }

    public class MediaGroup
    {
        [JsonProperty("media:title")]
        public string MediaTitle { get; set; }

        [JsonProperty("media:content")]
        public MediaContent MediaContent { get; set; }

        [JsonProperty("media:thumbnail")]
        public MediaThumbnail MediaThumbnail { get; set; }

        [JsonProperty("media:description")]
        public string MediaDescription { get; set; }

        [JsonProperty("media:community")]
        public MediaCommunity MediaCommunity { get; set; }
    }

    public class MediaStarRating
    {
        [JsonProperty("@count")]
        public string Count { get; set; }

        [JsonProperty("@average")]
        public string Average { get; set; }

        [JsonProperty("@min")]
        public string Min { get; set; }

        [JsonProperty("@max")]
        public string Max { get; set; }
    }

    public class MediaStatistics
    {
        [JsonProperty("@views")]
        public string Views { get; set; }
    }

    public class MediaThumbnail
    {
        [JsonProperty("@url")]
        public string Url { get; set; }

        [JsonProperty("@width")]
        public string Width { get; set; }

        [JsonProperty("@height")]
        public string Height { get; set; }
    }

    public class ConvJson
    {
        [JsonProperty("?xml")]
        public Xml Xml { get; set; }
        public Feed feed { get; set; }
    }

    public class Xml
    {
        [JsonProperty("@version")]
        public string Version { get; set; }

        [JsonProperty("@encoding")]
        public string Encoding { get; set; }
    }


}
