using System.Collections.Generic;
using Models;

namespace StoreBL
{
    public interface IBL
    {
        List<StoreFront> GetALLStoreFront();
        void SendOrder(Order order);
        Customer GetCustomerByID(int CustomerId);
        Customer GetCustomerByPhone(long phoneNum);
        void AddNewStoreFront(StoreFront store);
        void UpdateInventory(Inventory inventory, int StoreId);
        void AddNewCustomer(Customer customer);
        StoreFront GetStoreFrontById(int id);
        List<Order> GetAllOrdersByCustomerByDate(int customerId);
        List<Order> GetAllOrdersByCustomerByCost(int customerId);
        List<Order> GetAllOrdersByStoreByDate(string storeAddress);
        List<Order> GetAllOrdersByStoreByCost(string storeAddress);
    }
}