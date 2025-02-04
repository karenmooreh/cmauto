// See https://aka.ms/new-console-template for more information
using fsLocalService;

Console.WriteLine("Hello, World!");

Dictionary<string, string> __args = new Dictionary<string, string>();
for(var i = 0x00; i < args.Length; i += 0x02)
{
    if (args[i].StartsWith("-"))
    {
        string __argkey = args[i].Substring(0x01);
        if(i+0x01 < args.Length)
        {
            if(!args[i + 0x01].StartsWith("-"))
                __args.Add(__argkey, args[i + 0x01]);
            else
            {
                __args.Add(__argkey, string.Empty);
                i--;
            }
        }
    }
}

Thread.Sleep(5000);
ServCore __servcore = new ServCore(__args);
__servcore.Start();

while(true) Thread.Sleep(1000);