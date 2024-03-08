using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using SkiaSharp;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable MemberCanBePrivate.Global

namespace IctBaden.Pixoo;

public class Display
{
    private readonly IPAddress _address;
    private readonly HttpClient _client;
    public readonly SKCanvas Canvas;
    private readonly SKBitmap _bitmap;

    public int Columns { get; private set; }
    public int Rows { get; private set; }
    
    public Display(IPAddress address, DisplayType type)
    {
        _address = address;
        _client = new HttpClient();
        _client.Timeout = TimeSpan.FromSeconds(5);
        _client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

        switch (type)
        {
            case DisplayType.Pixoo64:
                Columns = 64;
                Rows = 64;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        _bitmap = new SKBitmap(Columns, Rows, SKColorType.Rgb888x, SKAlphaType.Opaque);
        Canvas = new SKCanvas(_bitmap);
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private HttpResponseMessage PostCommand(string command) =>
        PostCommand(command, new Dictionary<string, object>());
    
    private HttpResponseMessage PostCommand(string command, Dictionary<string, object> parameters)
    {
        var url = $"http://{_address}/post";

        var request = new Dictionary<string, object>
        {
            { "Command", command }
        };
        foreach (var (key, value) in parameters)
        {
            request.Add(key, value);
        }

        var data = JsonSerializer.Serialize(request);
        var content = new StringContent(data, Encoding.UTF8, MediaTypeNames.Application.Json);
        try
        {
            return _client.PostAsync(url, content).Result;
        }
        catch
        {
            // ignore
        }

        return new HttpResponseMessage(HttpStatusCode.InternalServerError);
    }


    public PixooClockInfo? GetClockInfo()
    {
        var responseMessage = PostCommand("Channel/GetClockInfo");
        if (!responseMessage.IsSuccessStatusCode) return null;
        
        var json = responseMessage.Content.ReadAsStringAsync().Result;
        var clockInfo = JsonSerializer.Deserialize<PixooClockInfo>(json);
        return clockInfo;
    }

    public PixooChannelIndex? GetCurrentChannel()
    {
        var responseMessage = PostCommand("Channel/GetIndex");
        if (!responseMessage.IsSuccessStatusCode) return null;
        
        var json = responseMessage.Content.ReadAsStringAsync().Result;
        var channelIndex = JsonSerializer.Deserialize<PixooChannelIndex>(json);
        return channelIndex;
    }

    public PixooConfiguration? GetConfiguration()
    {
        var responseMessage = PostCommand("Channel/GetAllConf");
        if (!responseMessage.IsSuccessStatusCode) return null;
        
        var json = responseMessage.Content.ReadAsStringAsync().Result;
        var configuration = JsonSerializer.Deserialize<PixooConfiguration>(json);
        return configuration;
    }

    public void SetBrightness(uint percent)
    {
        var param = new Dictionary<string, object>
        {
            { "Brightness", percent }
        };
        PostCommand("Channel/SetBrightness", param);
    }

    public void PlayBuzzer(int onMs = 500, int offMs = 500, int totalMs = 3000)
    {
        var param = new Dictionary<string, object>
        {
            { "ActiveTimeInCycle", onMs },
            { "OffTimeInCycle", offMs },
            { "PlayTotalTime", totalMs }
        };
        PostCommand("Device/PlayBuzzer", param);
    }
    
    public void SetBitmap()
    {
        PostCommand("Draw/ResetHttpGifId");
        
        var bytes = 
        _bitmap.Bytes.Chunk(4).SelectMany(pix => pix.Take(3)).ToArray();
        var data = Convert.ToBase64String(bytes);

        var param = new Dictionary<string, object>
        {
            { "PicNum", 1 },
            { "PicWidth", 64 },
            { "PicOffset", 0 },
            { "PicID", 1 },
            { "PicSpeed", 100 },
            { "PicData", data }
        };
        PostCommand("Draw/SendHttpGif", param);
    }
}
