// See https://aka.ms/new-console-template for more information

using System.Net;
using IctBaden.Pixoo;

Console.WriteLine("Pixoo64");

var address = IPAddress.Parse("192.168.2.163");
var display = new Display(address);

display.SetBrightness(50);
Task.Delay(1000).Wait();
display.SetBrightness(100);

display.SetBitmap();
