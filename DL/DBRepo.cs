using System.Collections.Generic;
using System.Linq;
//using Model = Models;
//using Entity = DL.Entities;
using Microsoft.EntityFrameworkCore;
//using DL.Entities;
using Models;

namespace DL
{
    public class DBRepo : IRepo
    {
        private IIDBContextW _context;
        public  DBRepo(IIDBContextW context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all stores from database returns them in a list
        /// </summary>
        /// <returns> list of all stores from db </returns>
        public List<StoreFront> GetALLStoreFront()
        {
            List<StoreFront> sFronts = _context.StoreFronts.Select(StoreFront => new StoreFront()
                {
                    Id = StoreFront.Id,
                    Address = StoreFront.Address,
                    
                }
            ).ToList();
            foreach(StoreFront s in sFronts)
            {
                //List<Inventory> inventories = _context.Inventories.Include("Product").Where(i => i.StoreFrontId == s.Id).Select(i => new Inventory{
                //Id = i.Id,
                //Quantity = i.Quantity ?? 0,
                //Item = new Product(){
                //    Id = i.Product.Id,
                //    Name = i.Product.Name,
                //    Price = i.Product.Price ?? 0
                //}
                //}).ToList();

                //s.Inventories = inventories;
            }
            return sFronts;
        }
        
        /// <summary>
        /// Gets Store from database by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>StoreFront with Id == id</returns>
        public StoreFront GetStoreFrontById(int id)
        {
            StoreFront StoreById = _context.StoreFronts.Include("Inventories").FirstOrDefault(s => s.Id == id);
            if(StoreById == null)
            {
                return new StoreFront(){};
            }
            StoreFront store = new StoreFront(){
                Id = StoreById.Id,
                Address = StoreById.Address
            };

            //List<Inventory> inventories = _context.Inventories.Include("Product").Where(i => i.StoreFrontId == StoreById.Id).Select(i => new Inventory{
            //Id = i.Id,
            //Quantity = i.Quantity ?? 0,
            //Item = new Product(){
            //        Id = i.Product.Id,
            //        Name = i.Product.Name,
            //        Price = i.Product.Price ?? 0
            //    }
            //}).ToList();
            //store.Inventories = inventories;
            return store;

        }

        /// <summary>
        /// sends order to db then sets up all orderlines related to order
        /// </summary>
        /// <param name="order"></param>
        public void SendOrder(Order order)
        {
            Order orderSend = new Order(){
                //CustomerId = order.Cust.Id,
                StoreAddress = order.StoreAddress,
                Total = order.Total,
                //Date = order.DateOfOrder
            };
            _context.Add(orderSend);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            foreach(OrderLine ol in order.OrderItems)
            {
                OrderLine orderLineSend = new OrderLine(){
                    Id = ol.Id,
                    Quantity = ol.Quantity,
                    //ProductId = ol.Item.Id,
                    //OrderId = orderSend.Id
                };
                _context.Add(orderLineSend);
                _context.SaveChanges();
                _context.ChangeTracker.Clear();
            }
        }

        /// <summary>
        /// adds new customer to db 
        /// </summary>
        /// <param name="customer"></param>
        public void AddNewCustomer(Customer customer)
        {
            Customer customerAdd = new Customer(){
                Name = customer.Name,
                PhoneNum = customer.PhoneNum
            };
            _context.Add(customerAdd);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        /// <summary>
        /// gets customer by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> returns customer with id</returns>
        public Customer GetCustomerById(int id)
        {
            Customer customerById = _context.Customers.FirstOrDefault(c => c.Id == id);
            return new Customer(){
                Id = customerById.Id,
                Name = customerById.Name,
                PhoneNum = customerById.PhoneNum
            };
        }

        /// <summary>
        /// gets customer by phonenum and returns default customer with id = 0 if no customer with phonenum exists in db
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <returns> returns customer with given phonenum or default</returns>
        public Customer GetCustomerByPhone(long phoneNum)
        {
            Customer customerById = _context.Customers.FirstOrDefault(c => c.PhoneNum == phoneNum);
            if(customerById == null)
            {
                return new Customer(){};
            }
            
            return new Customer(){
                Id = customerById.Id,
                Name = customerById.Name,
                PhoneNum = customerById.PhoneNum
            };
        }

        /// <summary>
        /// adds new store and then adds inventories from all products
        /// </summary>
        /// <param name="store"></param>
        public void AddNewStoreFront(StoreFront store)
        {
            StoreFront storeAdd = new StoreFront(){
                Address = store.Address
            };
            _context.Add(storeAdd);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            for(int i = 1; i <= 4; i++)
            {
                Inventory inventoryAdd = new Inventory(){
                    Quantity = 0,
                    //ProductId = i,
                    //StoreFrontId = storeAdd.Id
                };
                _context.Add(inventoryAdd);
            }
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        /// <summary>
        /// updates inventory with new inventory given
        /// </summary>
        /// <param name="inventory"></param>
        /// <param name="storeId"></param>
        public void UpdateInventory(Inventory inventory, int storeId)
        {
            Inventory inventoryUpdate = new Inventory(){
                Id  = inventory.Id,
                //ProductId = inventory.Item.Id,
                //StoreFrontId = storeId,
                //Quantity = inventory.Quantity
            };
            _context.Update(inventoryUpdate);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        /// <summary>
        /// gets all orders with from a customer sorted by date
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns> returns list of order sorted by date</returns>
        public List<Order> GetAllOrdersByCustomerByDate(int customerId)
        {
            //List<Order> orders = _context.Orders.Include("Customer").Where(Order => Order.CustomerId == customerId).OrderBy(Order => Order.Date).Select(Order => new Order{
            //    Id = Order.Id,
            //    Cust = new Customer(){
            //        Id = Order.Customer.Id,
            //        Name = Order.Customer.Name,
            //        PhoneNum = Order.Customer.PhoneNum
            //    },
            //    StoreAddress = Order.StoreAddress,
            //    Total = Order.Total ?? 0,
            //    DateOfOrder = Order.Date
            //}).ToList();
            //foreach(Order o in orders)
            //{
            //    List<OrderLine> orderLines = _context.OrderLines.Include("Product").Where(ol => ol.OrderId == o.Id).Select(i => new OrderLine{
            //    Id = i.Id,
            //    Quantity = i.Quantity ?? 0,
            //    Item = new Product(){
            //        Id = i.Product.Id,
            //        Name = i.Product.Name,
            //        Price = i.Product.Price ?? 0
            //    }
            //    }).ToList();

            //    o.OrderItems = orderLines;
            //}
            List<Order> orders = new List<Order>();
            return orders;
        }

        /// <summary>
        /// gets all orders with from a customer sorted by total
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns> returns list of order sorted by total</returns>
        public List<Order> GetAllOrdersByCustomerByCost(int customerId)
        {
            //List<Order> orders = _context.Orders.Include("Customer").Where(Order => Order.CustomerId == customerId).OrderBy(Order => Order.Total).Select(Order => new Order{
            //    Id = Order.Id,
            //    Cust = new Customer(){
            //        Id = Order.Customer.Id,
            //        Name = Order.Customer.Name,
            //        PhoneNum = Order.Customer.PhoneNum
            //    },
            //    StoreAddress = Order.StoreAddress,
            //    Total = Order.Total ?? 0,
            //    DateOfOrder = Order.Date
            //}).ToList();
            //foreach(Order o in orders)
            //{
            //    List<OrderLine> orderLines = _context.OrderLines.Include("Product").Where(ol => ol.OrderId == o.Id).Select(i => new OrderLine{
            //    Id = i.Id,
            //    Quantity = i.Quantity ?? 0,
            //    Item = new Product(){
            //        Id = i.Product.Id,
            //        Name = i.Product.Name,
            //        Price = i.Product.Price ?? 0
            //    }
            //    }).ToList();

            //    o.OrderItems = orderLines;
            //}
            //List<Order> orders = new List<Order>();
            List<Order> orders = new List<Order>();
            return orders;
        }

        /// <summary>
        /// gets all orders with from a store sorted by date
        /// </summary>
        /// <param name="storeAddress"></param>
        /// <returns>returns list of order sorted by date</returns>
        public List<Order> GetAllOrdersByStoreByDate(string storeAddress)
        {
            //List<Order> orders = _context.Orders.Include("Customer").Where(Order => Order.StoreAddress == storeAddress).OrderBy(Order => Order.Date).Select(Order => new Order{
            //    Id = Order.Id,
            //    Cust = new Customer(){
            //        Id = Order.Customer.Id,
            //        Name = Order.Customer.Name,
            //        PhoneNum = Order.Customer.PhoneNum
            //    },
            //    StoreAddress = Order.StoreAddress,
            //    Total = Order.Total ?? 0,
            //    DateOfOrder = Order.Date
            //}).ToList();
            //foreach(Order o in orders)
            //{
            //    List<OrderLine> orderLines = _context.OrderLines.Include("Product").Where(ol => ol.OrderId == o.Id).Select(i => new OrderLine{
            //    Id = i.Id,
            //    Quantity = i.Quantity ?? 0,
            //    Item = new Product(){
            //        Id = i.Product.Id,
            //        Name = i.Product.Name,
            //        Price = i.Product.Price ?? 0
            //    }
            //    }).ToList();

            //    o.OrderItems = orderLines;
            //}
            List<Order> orders = new List<Order>();
            return orders;
        }

        /// <summary>
        /// gets all orders with from a store sorted by total
        /// </summary>
        /// <param name="storeAddress"></param>
        /// <returns>returns list of order sorted by total</returns>
        public List<Order> GetAllOrdersByStoreByCost(string storeAddress)
        {
            List<Order> orders = _context.Orders.Include("Customer").Where(Order => Order.StoreAddress == storeAddress).OrderBy(Order => Order.Total).Select(Order => new Order{
                Id = Order.Id,
                Cust = new Customer(){
                    //Id = Order.Customer.Id,
                    //Name = Order.Customer.Name,
                    //PhoneNum = Order.Customer.PhoneNum
                },
                StoreAddress = Order.StoreAddress,
                //Total = Order.Total ?? 0,
                //DateOfOrder = Order.Date
            }).ToList();
            foreach(Order o in orders)
            {
                //List<OrderLine> orderLines = _context.OrderLines.Include("Product").Where(ol => ol.OrderId == o.Id).Select(i => new OrderLine{
                //Id = i.Id,
                //Quantity = i.Quantity ?? 0,
                //Item = new Product(){
                //    Id = i.Product.Id,
                //    Name = i.Product.Name,
                //    Price = i.Product.Price ?? 0
                //}
                //}).ToList();

                //o.OrderItems = orderLines;
            }
            return orders;
        }
    }
}