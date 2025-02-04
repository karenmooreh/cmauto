using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelupstreamDMUAPI.Logger
{
    public partial class Logger
    {
        private static void __static_constructor_Logger()
        {
            if (!__status) {
                __status = true;

                __con_logsqueue = new ConcurrentQueue<log>();

                (__thd_logging = new Thread(new ThreadStart(__thdmtd_logging))
                    { IsBackground = true }).Start();
            }
        }

        private static void __log(log logdata)
            => __con_logsqueue.Enqueue(logdata);

        private static void __thdmtd_logging()
        {
            while(ServiceCore.Singleton.Status || __status)
            {
                if (__con_logsqueue.Count > 0x00)
                {
                    log __delog;
                    if(__con_logsqueue.TryDequeue(out __delog))
                    {
                        if (__delog.isdisplay)
                            Console.WriteLine($"[{__delog.regtime.ToString()}][{__delog.type}][{__delog.source}]:{__delog.intro}|{__delog.details}");
                        if (__delog.isstorage) {
                            EFCore.Contexts.MainContext __mcnt = new EFCore.Contexts.MainContext();
                            __mcnt.logs.Add(new EFCore.Models.log() { 
                                id = __delog.id,
                                source = __delog.source,
                                type = (int)__delog.type,
                                intro = __delog.intro,
                                details = __delog.details,
                                regtime = __delog.regtime
                            });
                            try { __mcnt.SaveChanges(); } catch { }
                        }
                    }
                }

                Thread.Sleep(0x64);
            }
        }
    }
}
