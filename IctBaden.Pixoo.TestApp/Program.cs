// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Net;
using IctBaden.Pixoo;
using SkiaSharp;

Console.WriteLine("Pixoo64");

var address = IPAddress.Parse("192.168.2.163");
var display = new Display(address, DisplayType.Pixoo64);

// display.SetBrightness(50);
// Task.Delay(1000).Wait();
display.SetBrightness(100);

var paint = new SKPaint();

#if test1
paint.Typeface = SKTypeface.FromFamilyName("Arial");
paint.TextSize = 11;
paint.TextAlign = SKTextAlign.Right;
paint.Color = new SKColor(255, 255, 255);

display.Canvas.DrawCircle(6, 6, 5, new SKPaint { Color = SKColors.Yellow });
display.Canvas.DrawText("86%", 64, 10, paint );

var fieldInfos = typeof(SKColors).GetFields();
var field = fieldInfos
    .FirstOrDefault(f => string.Compare(f.Name, "green", StringComparison.InvariantCultureIgnoreCase) == 0);

var color = field != null 
    ? (SKColor)(field.GetValue(null) ?? SKColors.Transparent)
    : SKColors.Transparent;

display.Canvas.DrawRect(2,13,62,12, new SKPaint { Color = color });
display.Canvas.DrawRect(0, 15,2,8, new SKPaint { Color = SKColors.Green });
display.Canvas.DrawRect(3,14,60,10, new SKPaint { Color = SKColors.Black });

display.Canvas.DrawRect(0,26,64,12, new SKPaint { Color = new SKColor(255, 0, 0)});
display.Canvas.DrawRect(0,39,64,12, new SKPaint { Color = new SKColor(0, 255, 0)});
display.Canvas.DrawRect(0,52,64,12, new SKPaint { Color = SKColors.Blue});
#endif

paint.Typeface = SKTypeface.FromFamilyName("Arial");
paint.TextSize = 8;
paint.TextAlign = SKTextAlign.Left;
paint.Color = new SKColor(255, 255, 255);
display.Canvas.DrawRect(0,0,64,8, new SKPaint { Color = new SKColor(1, 40, 82)});
display.Canvas.DrawText("TEST", 1, 7, paint );

var color = Color.FromName("DarkOliveGreen");
display.Canvas.DrawRect(0,8,64,8, new SKPaint { Color = new SKColor(color.R, color.G, color.B)});

display.SetBitmap();

Console.WriteLine("done");

