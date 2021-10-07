using System.Collections.Generic;
using System.Linq;
using Model = Models;
using Entity = DL.Entities;
using Microsoft.EntityFrameworkCore;
using DL.Entities;
using Models;

namespace DL
{
    public class DBRepo : IRepo
    {
        private Entity.IIDBContext _context;
        public  DBRepo(Entity.IIDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all stores from database returns them in a list
        /// </summary>
        /// <returns> list of all stores from db </returns>
        public List<Model.StoreFront> GetALLStoreFront()
        {
            List<Model.StoreFront> sFronts = _context.StoreFronts.Select(StoreFront => new Model.StoreFront()
                {
                    Id = StoreFront.Id,
                    Address = StoreFront.Address,
                    
                }
            ).ToList();
            foreach(Model.StoreFront s in sFronts)
            {
                List<Model.Inventory> inventories = _context.Inventories.Include("Product").Where(i => i.StoreFrontId == s.Id).Select(i => new Model.Inventory{
                Id = i.Id,
                Quantity = i.Quantity ?? 0,
                Item = new Model.Product(){
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price ?? 0
                }
                }).ToList();

                s.Inventories = inventories;
            }
            return sFronts;
        }
        
        /// <summary>
        /// Gets Store from database by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Model.StoreFront with Id == id</returns>
        public Model.StoreFront GetStoreFrontById(int id)
        {
            Entity.StoreFront StoreById = _context.StoreFronts.Include("Inventories").FirstOrDefault(s => s.Id == id);
            if(StoreById == null)
            {
                return new Model.StoreFront(){};
            }
            Model.StoreFront store = new Model.StoreFront(){
                Id = StoreById.Id,
                Address = StoreById.Address
            };

            List<Model.Inventory> inventories = _context.Inventories.Include("Product").Where(i => i.StoreFrontId == StoreById.Id).Select(i => new Model.Inventory{
            Id = i.Id,
            Quantity = i.Quantity ?? 0,
            Item = new Model.Product(){
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price ?? 0
                }
            }).ToList();
            store.Inventories = inventories;
            return store;

        }

        /// <summary>
        /// sends order to db then sets up all orderlines related to order
        /// </summary>
        /// <param name="order"></param>
        public void SendOrder(Model.Order order)
        {
            Entity.Order orderSend = new Entity.Order(){
                CustomerId = order.Cust.Id,
                StoreAddress = order.StoreAddress,
                Total = order.Total,
                Date = order.DateOfOrder
            };
            _context.Add(orderSend);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
            foreach(Model.OrderLine ol in order.OrderItems)
            {
                Entity.OrderLine orderLineSend = new Entity.OrderLine(){
                    Id = ol.Id,
                    Quantity = ol.Quantity,
                    ProductId = ol.Item.Id,
                    OrderId = orderSend.Id
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
        public void AddNewCustomer(Model.Customer customer)
        {
            Entity.Customer customerAdd = new Entity.Customer(){
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
        public Model.Customer GetCustomerById(int id)
        {
            Entity.Customer customerById = _context.Customers.FirstOrDefault(c => c.Id == id);
            return new Model.Customer(){
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
        public Model.Customer GetCustomerByPhone(long phoneNum)
        {
            Entity.Customer customerById = _context.Customers.FirstOrDefault(c => c.PhoneNum == phoneNum);
            if(customerById == null)
            {
                return new Model.Customer(){};
            }
            
            return new Model.Customer(){
                Id = customerById.Id,
                Name = customerById.Name,
                PhoneNum = customerById.PhoneNum
            };
        }

        /// <summary>
        /// adds new store and then adds inventories from all products
        /// </summary>
        /// <param name="store"></param>
        public void AddNewStoreFront(Model.StoreFront store)
        {
            Entity.StoreFront storeAdd = new Entity.StoreFront(){
                Address = store.Address
            };
            _context.Add(storeAdd);
            _context.SaveChanges();
            _context.ChangeTracker.Clear();

            for(int i = 1; i <= 4; i++)
            {
                Entity.Inventory inventoryAdd = new Entity.Inventory(){
                    Quantity = 0,
                    ProductId = i,
                    StoreFrontId = storeAdd.Id
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
        public void UpdateInventory(Model.Inventory inventory, int storeId)
        {
            Entity.Inventory inventoryUpdate = new Entity.Inventory(){
                Id  = inventory.Id,
                ProductId = inventory.Item.Id,
                StoreFrontId = storeId,
                Quantity = inventory.Quantity
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
        public List<Model.Order> GetAllOrdersByCustomerByDate(int customerId)
        {
            List<Model.Order> orders = _context.Orders.Include("Customer").Where(Order => Order.CustomerId == customerId).OrderBy(Order => Order.Date).Select(Order => new Model.Order{
                Id = Order.Id,
                Cust = new Model.Customer(){
                    Id = Order.Customer.Id,
                    Name = Order.Customer.Name,
                    PhoneNum = Order.Customer.PhoneNum
                },
                StoreAddress = Order.StoreAddress,
                Total = Order.Total ?? 0,
                DateOfOrder = Order.Date
            }).ToList();
            foreach(Model.Order o in orders)
            {
                List<Model.OrderLine> orderLines = _context.OrderLines.Include("Product").Where(ol => ol.OrderId == o.Id).Select(i => new Model.OrderLine{
                Id = i.Id,
                Quantity = i.Quantity ?? 0,
                Item = new Model.Product(){
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price ?? 0
                }
                }).ToList();

                o.OrderItems = orderLines;
            }
            return orders;
        }

        /// <summary>
        /// gets all orders with from a customer sorted by total
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns> returns list of order sorted by total</returns>
        public List<Model.Order> GetAllOrdersByCustomerByCost(int customerId)
        {
            List<Model.Order> orders = _context.Orders.Include("Customer").Where(Order => Order.CustomerId == customerId).OrderBy(Order => Order.Total).Select(Order => new Model.Order{
                Id = Order.Id,
                Cust = new Model.Customer(){
                    Id = Order.Customer.Id,
                    Name = Order.Customer.Name,
                    PhoneNum = Order.Customer.PhoneNum
                },
                StoreAddress = Order.StoreAddress,
                Total = Order.Total ?? 0,
                DateOfOrder = Order.Date
            }).ToList();
            foreach(Model.Order o in orders)
            {
                List<Model.OrderLine> orderLines = _context.OrderLines.Include("Product").Where(ol => ol.OrderId == o.Id).Select(i => new Model.OrderLine{
                Id = i.Id,
                Quantity = i.Quantity ?? 0,
                Item = new Model.Product(){
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price ?? 0
                }
                }).ToList();

                o.OrderItems = orderLines;
            }
            return orders;
        }

        /// <summary>
        /// gets all orders with from a store sorted by date
        /// </summary>
        /// <param name="storeAddress"></param>
        /// <returns>returns list of order sorted by date</returns>
        public List<Model.Order> GetAllOrdersByStoreByDate(string storeAddress)
        {
            List<Model.Order> orders = _context.Orders.Include("Customer").Where(Order => Order.StoreAddress == storeAddress).OrderBy(Order => Order.Date).Select(Order => new Model.Order{
                Id = Order.Id,
                Cust = new Model.Customer(){
                    Id = Order.Customer.Id,
                    Name = Order.Customer.Name,
                    PhoneNum = Order.Customer.PhoneNum
                },
                StoreAddress = Order.StoreAddress,
                Total = Order.Total ?? 0,
                DateOfOrder = Order.Date
            }).ToList();
            foreach(Model.Order o in orders)
            {
                List<Model.OrderLine> orderLines = _context.OrderLines.Include("Product").Where(ol => ol.OrderId == o.Id).Select(i => new Model.OrderLine{
                Id = i.Id,
                Quantity = i.Quantity ?? 0,
                Item = new Model.Product(){
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price ?? 0
                }
                }).ToList();

                o.OrderItems = orderLines;
            }
            return orders;
        }

        /// <summary>
        /// gets all orders with from a store sorted by total
        /// </summary>
        /// <param name="storeAddress"></param>
        /// <returns>returns list of order sorted by total</returns>
        public List<Model.Order> GetAllOrdersByStoreByCost(string storeAddress)
        {
            List<Model.Order> orders = _context.Orders.Include("Customer").Where(Order => Order.StoreAddress == storeAddress).OrderBy(Order => Order.Total).Select(Order => new Model.Order{
                Id = Order.Id,
                Cust = new Model.Customer(){
                    Id = Order.Customer.Id,
                    Name = Order.Customer.Name,
                    PhoneNum = Order.Customer.PhoneNum
                },
                StoreAddress = Order.StoreAddress,
                Total = Order.Total ?? 0,
                DateOfOrder = Order.Date
            }).ToList();
            foreach(Model.Order o in orders)
            {
                List<Model.OrderLine> orderLines = _context.OrderLines.Include("Product").Where(ol => ol.OrderId == o.Id).Select(i => new Model.OrderLine{
                Id = i.Id,
                Quantity = i.Quantity ?? 0,
                Item = new Model.Product(){
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price ?? 0
                }
                }).ToList();

                o.OrderItems = orderLines;
            }
            return orders;
        }
    }
}