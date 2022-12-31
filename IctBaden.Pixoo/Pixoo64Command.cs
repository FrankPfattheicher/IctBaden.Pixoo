using System.Globalization;
// ReSharper disable InconsistentNaming

#pragma warning disable CS0649
namespace IctBaden.Pixoo;

internal abstract record Pixoo64Command(string Command)
{
    public uint error_code;
}

// ****** Dial Control ******

// TODO

// ****** Channel Control ******

internal record SelectChannel() : Pixoo64Command("Channel/SetIndex")
{
    public ChannelId SelectIndex;

    public enum ChannelId
    {
        Faces = 0,
        CloudChannel = 1,
        Visualizer = 2,
        Custom = 3,
        BlackScreen = 4
    }
}

internal record ControlCustomChannel() : Pixoo64Command("Channel/SetCustomPageIndex")
{
    public CustomIndex CustomPageIndex;

    public enum CustomIndex
    {
        Index0 = 0,
        Index1 = 1,
        Index2 = 2
    }
}

internal record VisualizerChannel() : Pixoo64Command("Channel/SetEqPosition")
{
    public uint EqPosition;
}

internal record CloudChannel() : Pixoo64Command("Channel/CloudIndex")
{
    public CloudIndex Index;

    public enum CloudIndex
    {
        RecommendGallery = 0,
        Favourite = 1,
        SubscribeArtist = 2,
        Album = 3
    }
}

internal record GetCurrentChannel() : Pixoo64Command("Channel/GetIndex")
{
    public uint SelectIndex;
}

// ****** System Settings ******

internal record SetBrightness() : Pixoo64Command("Channel/SetBrightness")
{
    public uint Brightness;
}

internal record GetAllSettings() : Pixoo64Command("Channel/GetAllConf")
{
    public uint Brightness;
    public uint RotationFlag;
    public uint ClockTime;
    public uint GalleryTime;
    public uint SingleGalleyTime;
    public uint PowerOnChannelId;
    public uint GalleryShowTimeFlag;
    public uint CurClockId;
    public uint Time24Flag;
    public uint TemperatureMode;
    public uint GyrateAngle;
    public uint MirrorFlag;
    public uint LightSwitch;
}

internal record SetWeatherArea(double longitude, double latitude) : Pixoo64Command("Sys/LogAndLat")
{
    /// Format "30.29" 
    public readonly string Longitude = longitude.ToString("F2", CultureInfo.InvariantCulture);
    /// Format "20.58" 
    public readonly string Latitude = latitude.ToString("F2", CultureInfo.InvariantCulture);
}

internal record SetTimeZone(string timeZoneValue) : Pixoo64Command("Sys/TimeZone")
{
    /// Format "GMT-5" 
    public readonly string TimeZoneValue = timeZoneValue;
}

internal record SystemTime() : Pixoo64Command("Device/SetUTC")
{
    public uint Utc;
}

internal record ScreenSwitch() : Pixoo64Command("Channel/OnOffScreen")
{
    public Switch OnOff;

    public enum Switch
    {
        Off = 0,
        On = 1
    }
}

internal record GetDeviceTime() : Pixoo64Command("Device/GetDeviceTime")
{
    public uint UTCTime;
    public string? LocalTime;
}

internal record SetTemperatureMode() : Pixoo64Command("Device/SetDisTempMode")
{
    public TempMode Mode;

    public enum TempMode
    {
        Celsius = 0,
        Fahrenheit = 1
    }
}

internal record SetRotationAngle() : Pixoo64Command("Device/SetScreenRotationAngle")
{
    public Rotation Mode;

    public enum Rotation
    {
        Normal = 0,
        Rotation90 = 1,
        Rotation180 = 2,
        Rotation270 = 3
    }
}

internal record SetMirrorMode() : Pixoo64Command("Device/SetMirrorMode")
{
    public MirrorMode Mode;

    public enum MirrorMode
    {
        Disable = 0,
        Enable = 1
    }
}

internal record SetHourMode() : Pixoo64Command("Device/SetTime24Flag")
{
    public HourMode Mode;

    public enum HourMode
    {
        Hour12 = 0,
        Hour24 = 1
    }
}

internal record SetHighLightMode() : Pixoo64Command("Device/SetHighLightMode")
{
    public HighLightMode Mode;

    public enum HighLightMode
    {
        Close = 0,
        Open = 1
    }
}

internal record SetWhiteBalance() : Pixoo64Command("Device/SetWhiteBalance")
{
    /// 0..100
    public uint RValue;

    /// 0..100
    public uint GValue;

    /// 0..100
    public uint BValue;
}

internal record GetWeatherInfo() : Pixoo64Command("Device/GetWeatherInfo")
{
    public string? Weather;
    public double CurTemp;
    public double MinTemp;
    public double MaxTemp;
    public double Pressure;
    public double Humidity;
    public double Visibility;
    public double WindSpeed;
}

// ****** Tools ******

internal record SetCountdownTool() : Pixoo64Command("Tools/SetTimer")
{
    public uint Minute;
    public uint Second;
    public Control Status;

    public enum Control
    {
        Stop = 0,
        Start = 1
    }
}

internal record SetStopwatchTool() : Pixoo64Command("Tools/SetStopWatch")
{
    public uint Minute;
    public uint Second;
    public Control Status;

    public enum Control
    {
        Stop = 0,
        Start = 1,
        Reset = 2
    }
}

internal record SetScoreboardTool() : Pixoo64Command("Tools/SetScoreBoard")
{
    /// 0..999
    public uint BlueScore;

    /// 0..999
    public uint RedScore;
}

internal record SetNoiseTool() : Pixoo64Command("Tools/SetNoiseStatus")
{
    public Control NoiseStatus;

    public enum Control
    {
        Stop = 0,
        Start = 1
    }
}

// ****** Animation Functions ******

internal record PlayGif() : Pixoo64Command("Device/PlayTFGif")
{
    public Type FileType;

    public enum Type
    {
        PlayTfsFile = 0,
        PlayTfsFolder = 1,
        PlayNetFile = 2
    }

    public Name FileName;

    public enum Name
    {
        FilePath = 0,
        FolderPath = 1,
        NetFileAddress = 2
    }
}

internal record GetSendingAnimationPicId() : Pixoo64Command("Draw/GetHttpGifId")
{
    public uint PicId;
}

internal record ResetSendingAnimationPicId() : Pixoo64Command("Draw/ResetHttpGifId");

internal record SendAnimation() : Pixoo64Command("Draw/SendHttpGif")
{
    public uint PicNum;
    public uint PicWidth = 64;

    /// 0 .. PicNum-1
    public uint PicOffset;

    public uint PicID;
    public uint PicSpeed;
    public string? PicData;
}

internal record SendText() : Pixoo64Command("Draw/SendHttpText")
{
    public uint TextId;
    public uint x;
    public uint y;
    public uint dir;
    public uint font;
    public uint TextWidth;
    public string? TextString;
    public uint speed;

    /// HTML color code like #FFFF00
    public string? color;

    public Alignment align;

    public enum Alignment
    {
        Left = 1,
        Middle = 2,
        Right = 3
    }
}

internal record ClearAllTextArea() : Pixoo64Command("Draw/ClearHttpText");

internal record SendDisplayList() : Pixoo64Command("Draw/SendHttpItemList")
{
    public record TextItem
    {
        public uint TextId;
        public DisplayType type;
        public uint x;
        public uint y;
        public uint dir;
        public uint font;
        public uint TextWidth;
        public string? TextString;
        public uint speed;

        /// HTML color code like #FFFF00
        public string? color;

        public Alignment align;
    }

    public List<TextItem> ItemList = new();

    public enum Alignment
    {
        Left = 1,
        Middle = 2,
        Right = 3
    }

    public enum DisplayType
    {
        // ReSharper disable IdentifierTypo
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_SECOND = 1, //second , font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_MIN = 2, //min, font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_HOUR = 3, //hour, font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_TIME_AM_PM = 4, //am or pm, font should include a,m,p
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_HOUR_MIN = 5, //hour：min , font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_HOUR_MIN_SEC = 6, //hour:min:sec, , font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_YEAR = 7, //year,, font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_DAY = 8, //day, font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_MON = 9, //month, font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_MON_YEAR = 10, //mon-year, font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_ENG_MONTH_DOT_DAY = 11, //month, font should include english letters
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_DATE_WEEK_YEAR = 12, //day:month:year, font should include digit
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_ENG_WEEK = 13,

        ///weekday-"SU","MO","TU","WE","TH","FR","SA", font should include english letters
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_ENG_WEEK_THREE =
            14, //weekday-"SUN","MON","TUE","WED","THU","FRI","SAT", font should include english letters

        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_ENG_WEEK_ALL =
            15, //weekday-"SUNDAY","MONDAY","TUESDAY","WEDNESDAY","THURSDAY","FRIDAY","SATURDAY", font should include english letters

        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_ENG_MON =
            16, //month-"JAN","FEB","MAR","APR","MAY","JUN","JUL","AUG","SEP","OCT","NOV","DEC", font should include english letters
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_TEMP_DIGIT = 17, //temperature, font should include digit and c,f
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_TODAY_MAX_TEMP = 18, //the max temperature, font should include digit and c,f
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_TODAY_MIN_TEMP = 19, //the min temperature, font should include digit and c,f
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_WEATHER_WORD = 20, //the weather, font should include english letters
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_NOISE_DIGIT = 21, //the noise value, font should include digit 
        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_TEXT_MESSAGE = 22, //the text string, font should include text information

        DIVOOM_DISP_CUSTOM_DIAL_SUPPORT_NET_TEXT_MESSAGE =
            23, //the url request string, font should include url information, response should be json encode including the "DispData" string element, eg:http://appin.divoom-gz.com/Device/ReturnCurrentDate?test=0 repone {"DispData": "2022-01-22 13:51:56"}
        // ReSharper restore IdentifierTypo
    }
}

internal record PlayBuzzer() : Pixoo64Command("Device/PlayBuzzer")
{
    /// Working time of buzzer in one cycle in milliseconds
    public uint ActiveTimeInCycle;
    /// Idle time of buzzer in one cycle in milliseconds
    public uint OffTimeInCycle;
    /// Working total time of buzzer in milliseconds
    public uint PlayTotalTime;
}

internal record PlayDivoomGif(string fileId) : Pixoo64Command("Draw/SendRemote")
{
    public readonly string FileId = fileId;
}

// ****** Command lList ******

internal record GetCommandList() : Pixoo64Command("Draw/CommandList")
{
    public record CommandInfo
    {
        public string? Command;
        public Dictionary<string, object> Parameters = new();
    }

    public List<CommandInfo> CommandList = new();
}



