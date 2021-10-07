using System.Collections.Generic;

namespace Models
{
    public class StoreFront
    {
        public StoreFront(){
            Id = 0;
        }
        public StoreFront(string address)
        {
            this.Address = address;
        }

        public int Id{get;set;}
        public string Address{get;set;}
        public List<Inventory> Inventories{get;set;}
    }
}