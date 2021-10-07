

namespace Models
{
    public class Customer
    {
        public Customer(){
            Id = 0;
        }
        public Customer(string name, long phoneNum)
        {
            this.PhoneNum = phoneNum;
            this.Name = name;
            Id = 0;
        }

        public int Id{get;set;}
        public string Name{get;set;}
        public long PhoneNum{get;set;}

    }
}
