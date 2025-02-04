using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fsClodService
{
    public partial class ServCore
    {
        public ServCore() => __constructor_ServCore();

        public static ServCore Singleton => __singleton;
        public ConcurrentQueue<billing> Billings => __con_billings;
        public ConcurrentDictionary<string, billing> Histories => __con_histories;
        public ConcurrentQueue<string> BillingNotifies => __con_billingnotifies;

        public void Start() => __start();

        public int BillingEnqueue(string billingno, string phonenum, double amount, string? notifyuri, string? param)
            => __billingenqueue(billingno, phonenum, amount, notifyuri, param);

        public List<billing> GetUntaskBillings(int? block)
            => __getuntaskbillings(block);

        public static string Sign(Dictionary<string, string> data, string appkey)
            => __sign(data, appkey);
    }
}
