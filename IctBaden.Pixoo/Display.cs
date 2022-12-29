using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace IctBaden.Pixoo;

public class Display
{
    private readonly IPAddress _address;
    private readonly HttpClient _client;
    private readonly Bitmap _bitmap;

    public Display(IPAddress address)
    {
        _address = address;
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        _bitmap = new Bitmap(64, 64, PixelFormat.Format24bppRgb);
        _bitmap.SetPixel(1,1,Color.Blue);
    }

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
        var converter = new ImageConverter();
        var bytes = (byte[])converter.ConvertTo(_bitmap, typeof(byte[]));
        var data = Convert.ToBase64String(bytes);

        var param = new Dictionary<string, object>
        {
            { "PicNum", 1 },
            { "PicWidth", 64 },
            { "PicOffset", 0 },
            { "PicID", 1 },
            { "PicSpeed", 100 },
            { "PicData", data
        }
        };
        PostCommand("Draw/SendHttpGif", param);
    }
}