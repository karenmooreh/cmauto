// See https://aka.ms/new-console-template for more information
using TelupstreamDMUAPI;

Console.WriteLine("Telupstream DMU API Service v1.0.0.1");

ServiceCore __servcore = new ServiceCore();
__servcore.Start();

while (true) Thread.Sleep(0x64);
