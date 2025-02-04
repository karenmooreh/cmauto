// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, XProxy Usage!");

string __workpath = Regex.Replace(Path.GetDirectoryName(typeof(Program).Assembly.Location), "\\\\", "/");
string __confile = Regex.Replace(Path.Combine(__workpath, "config.json"), "\\\\", "/");

var __procstartinf = new ProcessStartInfo(
    Path.Combine(__workpath, $"xray{(OperatingSystem.IsWindows() ? ".exe" : string.Empty)}")
    ) {
    Arguments = $"run -c {__confile}",
    WorkingDirectory = __workpath,
};
var __proxyproc = new Process() { StartInfo = __procstartinf };
__proxyproc.Start();

while (true) Thread.Sleep(0x64);
