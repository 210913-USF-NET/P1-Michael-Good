using System;
using System.Collections.Generic;
using Models;
using StoreBL;

namespace UI
{
    public class OrderMenu
    {
        private IBL _bl;
        public OrderMenu(IBL bl)
        {
            _bl = bl;
        }
        
        /// <summary>
        /// Gets user to pick store then to pick items for the that store to add to order
        /// after leaving the store the order will be processed and customer will be sent to main menu
        /// </summary>
        /// <param name="CustomerId"></param>
        public void Start(int CustomerId)
        {
            decimal Total = 0;
            Customer cust = _bl.GetCustomerByID(CustomerId);
            Order CurrentOrder = new Order();
            orderStart:
            Console.WriteLine("Select a Store to order from");
            List<StoreFront> allStores = _bl.GetALLStoreFront();
            if(allStores == null || allStores.Count == 0)
            {
                Console.WriteLine("No stores available");
                return;
            }
            for(int i = 0; i < allStores.Count; i++)
            {
                Console.WriteLine($"[{i}] {allStores[i].Address}");
            }
            string input = Console.ReadLine();
            int parsedInput;

            bool parseSuccess = Int32.TryParse(input, out parsedInput);
            if(parseSuccess && parsedInput >= 0 && parsedInput < allStores.Count)
            {
                StoreFront selectedStore = allStores[parsedInput];
                DateTime thisday = DateTime.Today;
                CurrentOrder = new Order(cust, selectedStore.Address, thisday.ToString(), Total);

                bool exit = false;
                Console.WriteLine($"You picked {selectedStore.Address}");
                do
                {
                    orderItemStart:
                    Console.WriteLine("Select an item to order");
                    if(selectedStore.Inventories == null || selectedStore.Inventories.Count == 0)
                    {
                        Console.WriteLine("This store has nothing in it");
                        return;
                    }
                    Console.WriteLine("[x] Send Order/Leave");
                    for(int i = 0; i < selectedStore.Inventories.Count; i++)
                    {
                        Console.WriteLine($"[{i}] {selectedStore.Inventories[i].Item.Name}: {selectedStore.Inventories[i].Quantity}");
                    }
                    input = Console.ReadLine();
                    parseSuccess = Int32.TryParse(input, out parsedInput);
                    if(parseSuccess && parsedInput >= 0 && parsedInput < selectedStore.Inventories.Count)
                    {
                        Inventory selectedInventory = selectedStore.Inventories[parsedInput];
                        orderManyStart:
                        Console.WriteLine("There are " + selectedInventory.Quantity + " " + selectedInventory.Item.Name + " left");
                        Console.WriteLine("How many do you want?");
                        input = Console.ReadLine();
                        parseSuccess = Int32.TryParse(input, out parsedInput);
                        if(parseSuccess && parsedInput >= 0 && parsedInput <= selectedInventory.Quantity)
                        {
                            int many = parsedInput;
                            Product aItem = selectedInventory.Item;
                            OrderLine orderLineItem = new OrderLine(aItem, many);
                            CurrentOrder.OrderItems.Add(orderLineItem);
                            selectedInventory.Quantity = selectedInventory.Quantity - many;
                            Total = Total + (aItem.Price * (decimal)many);
                            _bl.UpdateInventory(selectedInventory, selectedStore.Id);
                            Console.WriteLine("Item added to Order");
                        }
                        else
                        {
                            Console.WriteLine("invalid input");
                            goto orderManyStart;
                        }

                    }
                    else if(input == "x")
                    {
                        exit = true;
                    }
                    else
                    {
                        Console.WriteLine("invalid input");
                        goto orderItemStart;
                    }

                }while(!exit);
            }
            else
            {
                Console.WriteLine("invalid input");
                goto orderStart;
            }
            CurrentOrder.Total = Total;
            _bl.SendOrder(CurrentOrder);
        }
    }
}