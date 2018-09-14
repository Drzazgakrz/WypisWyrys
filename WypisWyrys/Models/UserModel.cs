using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WypisWyrys.Models
{
    public class UserModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string location { get; set; }
        public string zipCode { get; set; }
        public string streetName { get; set; }
        public UserModel(string firstName, string lastName, string location, string zipCode, string streetName)
        {
            this.firstName = firstName;
            this.lastName = lastName;
            this.location = location;
            this.zipCode = zipCode;
            this.streetName = streetName;
        }
        public UserModel() { }
    }
}
