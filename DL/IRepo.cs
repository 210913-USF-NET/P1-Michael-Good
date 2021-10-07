using System.Collections.Generic;
using Models;

namespace DL
{
    public interface IRepo
    {
        List<StoreFront> GetALLStoreFront();
        Customer GetCustomerById(int id);
        Customer GetCustomerByPhone(long phoneNum);
        List<Order> GetAllOrdersByCustomerByDate(int customerId);
        List<Order> GetAllOrdersByCustomerByCost(int customerId);
        List<Order> GetAllOrdersByStoreByDate(string storeAddress);
        List<Order> GetAllOrdersByStoreByCost(string storeAddress);
        void SendOrder(Order order);
        void AddNewStoreFront(StoreFront store);
        void AddNewCustomer(Customer customer);
        void UpdateInventory(Inventory inventory, int storeId);
        StoreFront GetStoreFrontById(int id);
        
    }
    
}