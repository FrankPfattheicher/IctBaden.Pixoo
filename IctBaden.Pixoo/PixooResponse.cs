using System.Text.Json.Serialization;

namespace IctBaden.Pixoo;

public class PixooResponse
{
    [JsonPropertyName("error_code")]
    public int ErrorCode { get; set; }
}