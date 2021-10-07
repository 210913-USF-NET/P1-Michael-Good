using System.Collections.Generic;

namespace Models
{
    public class Order
    {
        public Order(){}
        public Order(Customer cust, string address, string date, decimal total)
        {
            this.Cust = cust;
            this.StoreAddress = address;
            this.DateOfOrder = date;
            this.Total = total;
            this.OrderItems = new List<OrderLine>();
        }
        public int Id{get;set;}
        public Customer Cust{get;set;}
        public List<OrderLine> OrderItems{get;set;}
        public decimal Total{get;set;}
        public string DateOfOrder{get;set;}
        public string StoreAddress{get;set;}
    }
}