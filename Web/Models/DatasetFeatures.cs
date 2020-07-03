using System.Text.Json.Serialization;

namespace Web.Models
{
    public class DatasetFeatures
    {
        [JsonPropertyName("numeric")]
        public bool Numerical { get; set; }
        
        [JsonPropertyName("categoric")]
        public bool Categorical { get; set; }
        
        [JsonPropertyName("mixed_categoric_numeric")]
        public bool MixedNumericalCategorical { get; set; }
        
        [JsonPropertyName("geospatial")]
        public bool Geospatial { get; set; }
        
        [JsonPropertyName("temporal")]
        public bool Temporal { get; set; }
        
        [JsonPropertyName("one_ts")]
        public bool OneTimeSeries { get; set; }
        
        [JsonPropertyName("multiple_ts")]
        public bool MultipleTimeSeries { get; set; }
        
        [JsonPropertyName("negative_values")]
        public bool NegativeValues { get; set; }
    }
}
