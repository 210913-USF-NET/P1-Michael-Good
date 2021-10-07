using System.Collections.Generic;
using Models;
using DL;

namespace StoreBL
{
    public class BL : IBL
    {
        private IRepo _repo;

        public BL(IRepo repo)
        {
            _repo = repo;
        }

        public List<StoreFront> GetALLStoreFront()
        {
            return _repo.GetALLStoreFront();
        }
        public void SendOrder(Order order)
        {
            _repo.SendOrder(order);
        }
        public Customer GetCustomerByID(int CustomerId)
        {
            return _repo.GetCustomerById(CustomerId);
        }
        public Customer GetCustomerByPhone(long phoneNum)
        {
            return _repo.GetCustomerByPhone(phoneNum);
        }
        public void AddNewStoreFront(StoreFront store)
        {
            _repo.AddNewStoreFront(store);
        }
        public void UpdateInventory(Inventory inventory,int storeId)
        {
            _repo.UpdateInventory(inventory, storeId);
        }
        public void AddNewCustomer(Customer customer)
        {
            _repo.AddNewCustomer(customer);
        }
        public StoreFront GetStoreFrontById(int id)
        {
            return _repo.GetStoreFrontById(id);
        }
        List<Order> IBL.GetAllOrdersByCustomerByDate(int customerId)
        {
            return _repo.GetAllOrdersByCustomerByDate(customerId);
        }
        List<Order> IBL.GetAllOrdersByCustomerByCost(int customerId)
        {
            return _repo.GetAllOrdersByCustomerByCost(customerId);
        }
        List<Order> IBL.GetAllOrdersByStoreByDate(string storeAddress)
        {
            return _repo.GetAllOrdersByStoreByDate(storeAddress);
        }
        List<Order> IBL.GetAllOrdersByStoreByCost(string storeAddress)
        {
            return _repo.GetAllOrdersByStoreByCost(storeAddress);
        }
    }
}
