using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using SkiaSharp;
using ArgumentOutOfRangeException = System.ArgumentOutOfRangeException;

namespace IctBaden.Pixoo;

public class Display
{
    private readonly IPAddress _address;
    private readonly HttpClient _client;
    public readonly SKCanvas Canvas;
    private readonly SKBitmap _bitmap;

    public readonly int Columns;
    public readonly int Rows;
    
    public Display(IPAddress address, DisplayType type)
    {
        _address = address;
        _client = new HttpClient();
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
        return _client.PostAsync(url, content).Result;
    }


    public void SetBrightness(uint percent)
    {
        var param = new Dictionary<string, object>
        {
            { "Brightness", percent }
        };
        PostCommand("Channel/SetBrightness", param);
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