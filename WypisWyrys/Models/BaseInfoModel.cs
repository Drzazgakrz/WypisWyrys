using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    public class BaseInfoModel
    {
        private object p;

        public string caseSign { get; set; }
        public string location { get; set; }
        public string community { get; set; }
        public DateTime date { get; set; }
        public DateTime receiveDate { get; set; }
        public double price { get; set; }
        public string price2 { get; set; }
        public BaseInfoModel(string caseSign, string location, string community, DateTime date, DateTime receivedDate, double price, string price2)
        {
            this.caseSign = caseSign;
            this.location = location;
            this.community = community;
            this.date = date;
            this.receiveDate = receivedDate;
            this.price = price;
            this.price2 = price2;
        }

        public BaseInfoModel(string caseSign, string location, string community, DateTime date, object p, double price, string price2)
        {
            this.caseSign = caseSign;
            this.location = location;
            this.community = community;
            this.date = date;
            this.p = p;
            this.price = price;
            this.price2 = price2;
        }
    }
}
