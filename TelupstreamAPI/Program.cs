// See https://aka.ms/new-console-template for more information
using TelupstreamAPI;

Console.WriteLine("Telupstream API v1.0");

ServCore __serv = new ServCore();
__serv.Start();

while (true) Thread.Sleep(0x64);