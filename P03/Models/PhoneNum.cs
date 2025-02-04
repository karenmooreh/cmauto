using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P03.Models
{
    internal class PhoneNum : INotifyPropertyChanged
    {
        public string sn
        {
            get { return __sn; }
            set { __sn = value; this.OnPropertyChanged("sn"); }
        }private string __sn;
        public int snum
        {
            get { return __snum; }
            set { __snum = value; this.OnPropertyChanged("snum"); }
        }
        private int __snum;
        public string phonenum {
            get { return __phonenum; }
            set { __phonenum = value; this.OnPropertyChanged("phonenum"); }
        }private string __phonenum;
        public string status
        {
            get { return __status; }
            set { __status = value; this.OnPropertyChanged("status"); }
        }private string __status;
        public string code
        {
            get { return __code; }
            set { __code = value; this.OnPropertyChanged("code"); }
        }private string __code;
        public string serialno
        {
            get { return __serialno; }
            set { __serialno = value; this.OnPropertyChanged("serialno"); }
        }private string __serialno;
        public string log
        {
            get { return __log; }
            set { __log = value; this.OnPropertyChanged("log"); }
        }
        private string __log;

        public Thread workthread { get; set; }
        public bool success { get; set; }
        public DateTime finishtime { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if(null != handler)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
