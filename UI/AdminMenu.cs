using System;
using System.Collections.Generic;
using Models;
using Serilog;
using StoreBL;


namespace UI
{
    public class AdminMenu : IMenu
    {
        private IBL _bl;
        public AdminMenu(IBL bl)
        {
            _bl = bl;
        }

        /// <summary>
        /// menu for admin use only
        /// add inventory
        /// add store
        /// view orders
        /// </summary>
        public void Start()
        {
            bool exit = false;
            string input = "";
            do
            {
                Console.WriteLine("Admin menu");
                Console.WriteLine("[0] Restock Store");
                Console.WriteLine("[1] Make new Store");
                Console.WriteLine("[2] View Orders");
                Console.WriteLine("[x] Leave");
                
                input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        FillInventory();
                        break;

                    case "1":
                        CreateStore();
                        break;
                        
                    case "2":
                        ViewOrders();
                        break;

                    case "x":
                        exit = true;
                        break;
                    
                    default:
                        Console.WriteLine("Sorry, what you typed in was not a valid responce");
                        break;
                }
            }while (!exit);
        }

        /// <summary>
        /// takes user to fill inventory part of admin menu, used to help clean up switch statement
        /// </summary>
        public void FillInventory()
        {
            
            fillStart:
            Console.WriteLine("Select a Store to Fill Inventory");
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
                bool storeExit = false;
                Console.WriteLine($"You picked {selectedStore.Address}");
                do
                {
                    fillItemStart:
                    Console.WriteLine("Select an item to Fill");
                    if(selectedStore.Inventories == null || selectedStore.Inventories.Count == 0)
                    {
                        Console.WriteLine("This store has nothing in it");
                        return;
                    }
                    Console.WriteLine("[x] Leave");
                    for(int i = 0; i < selectedStore.Inventories.Count; i++)
                    {
                        Console.WriteLine($"[{i}] {selectedStore.Inventories[i].Item.Name}: {selectedStore.Inventories[i].Quantity}");
                    }
                    input = Console.ReadLine();
                    parseSuccess = Int32.TryParse(input, out parsedInput);
                    if(parseSuccess && parsedInput >= 0 && parsedInput < selectedStore.Inventories.Count)
                    {
                        Inventory selectedInventory = selectedStore.Inventories[parsedInput];
                        fillManyStart:
                        Console.WriteLine("How much is being added");
                        input = Console.ReadLine();
                        parseSuccess = Int32.TryParse(input, out parsedInput);
                        if(parseSuccess && parsedInput >= 0)
                        {
                            int many = parsedInput;
                            selectedInventory.Quantity = selectedInventory.Quantity + many;
                            _bl.UpdateInventory(selectedInventory, selectedStore.Id);
                        }
                        else if(input == "x")
                        {
                            storeExit = true;
                        }
                        else
                        {
                            Console.WriteLine("invalid input");
                            goto fillManyStart;
                        }

                    }
                    else if(input == "x")
                    {
                        storeExit = true;
                    }
                    else
                    {
                        Console.WriteLine("invalid input");
                        goto fillItemStart;
                    }

                }while(!storeExit);
            }
            else
            {
                Console.WriteLine("invalid input");
                goto fillStart;
            }
        }

        /// <summary>
        /// takes use to create store part of admin menu, used to help clean up switch statement
        /// </summary>
        public void CreateStore()
        {
            Console.WriteLine("please enter the address for the new store");
            string input = Console.ReadLine();
            StoreFront store = new StoreFront(input);
            Log.Information($"StoreFront being added Address: {store.Address}");
            _bl.AddNewStoreFront(store);
        }

        /// <summary>
        /// takes user to vieworder part of admin menu, used to help clean up switch statement
        /// </summary>
        public void ViewOrders()
        {
            Console.WriteLine("[0] View Orders by customer ordered by price");
            Console.WriteLine("[1] View Orders by customer ordered by date");
            Console.WriteLine("[2] View Orders by store ordered by price");
            Console.WriteLine("[3] View Orders by store ordered by date");
            Console.WriteLine("[x] Leave");
            string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        cpStart:
                        Customer cust;
                        Console.WriteLine("Enter customer Phone Number");
                        input = Console.ReadLine();
                        long parsedInput;
                        bool parseSuccess = Int64.TryParse(input, out parsedInput);
                        if(parseSuccess && parsedInput >= 0)
                        {
                            cust = _bl.GetCustomerByPhone(parsedInput);
                            if(cust.Id == 0)
                            {
                                Console.WriteLine("No Customer with that Phone Number");
                                goto cpStart;
                            }
                        }
                        else
                        {   
                            Console.WriteLine("invalid input");
                            goto cpStart;
                        }
                        List<Order> orders = _bl.GetAllOrdersByCustomerByCost(cust.Id);
                        foreach(Order o in orders)
                        {
                            Console.WriteLine("Order:");
                            Console.WriteLine($" Customer Name: {o.Cust.Name}, Store ordered From: [{o.StoreAddress}], Date Ordered: {o.DateOfOrder}, Total: {o.Total}");
                            Console.WriteLine("   Contains:");
                            foreach(OrderLine ol in o.OrderItems)
                            {
                                Console.WriteLine($"    Product Name: {ol.Item.Name}, Amount: {ol.Quantity}");
                            }
                        }
                        break;

                    case "1":
                        cdStart:
                        Console.WriteLine("Enter customer Phone Number");
                        input = Console.ReadLine();
                        parseSuccess = Int64.TryParse(input, out parsedInput);
                        if(parseSuccess && parsedInput >= 0)
                        {
                            cust = _bl.GetCustomerByPhone(parsedInput);
                            if(cust.Id == 0)
                            {
                                Console.WriteLine("No Customer with that Phone Number");
                                goto cdStart;
                            }
                        }
                        else
                        {   
                            Console.WriteLine("invalid input");
                            goto cdStart;
                        }
                        orders = _bl.GetAllOrdersByCustomerByDate(cust.Id);
                        foreach(Order o in orders)
                        {
                            Console.WriteLine("Order:");
                            Console.WriteLine($" Customer Name: {o.Cust.Name}, Store ordered From: [{o.StoreAddress}], Date Ordered: {o.DateOfOrder}, Total: {o.Total}");
                            Console.WriteLine("   Contains:");
                            foreach(OrderLine ol in o.OrderItems)
                            {
                                Console.WriteLine($"    Product Name: {ol.Item.Name}, Amount: {ol.Quantity}");
                            }
                        }
                        break;
                        
                    case "2":
                        spStart:
                        Console.WriteLine("Enter Store Id");
                        input = Console.ReadLine();
                        StoreFront store = new StoreFront();
                        int parsedInput2;
                        parseSuccess = Int32.TryParse(input, out parsedInput2);
                        if(parseSuccess && parsedInput2 >= 0)
                        {
                            store = _bl.GetStoreFrontById(parsedInput2);
                            if(store.Id == 0)
                            {
                                Console.WriteLine("No store with that Id");
                                goto spStart;
                            }
                        }
                        else
                        {   
                            Console.WriteLine("invalid input");
                            goto spStart;
                        }
                        orders = _bl.GetAllOrdersByStoreByCost(store.Address);
                        foreach(Order o in orders)
                        {
                            Console.WriteLine("Order:");
                            Console.WriteLine($" Customer Name: {o.Cust.Name}, Store ordered From: [{o.StoreAddress}], Date Ordered: {o.DateOfOrder}, Total: {o.Total}");
                            Console.WriteLine("   Contains:");
                            foreach(OrderLine ol in o.OrderItems)
                            {
                                Console.WriteLine($"    Product Name: {ol.Item.Name}, Amount: {ol.Quantity}");
                            }
                        }
                        break;
                    
                    case "3":
                        sdStart:
                        Console.WriteLine("Enter Store Id");
                        input = Console.ReadLine();
                        store = new StoreFront();
                        parseSuccess = Int32.TryParse(input, out parsedInput2);
                        if(parseSuccess && parsedInput2 >= 0)
                        {
                            store = _bl.GetStoreFrontById(parsedInput2);
                            if(store.Id == 0)
                            {
                                Console.WriteLine("No store with that Id");
                                goto sdStart;
                            }
                        }
                        else
                        {   
                            Console.WriteLine("invalid input");
                            goto sdStart;
                        }
                        orders = _bl.GetAllOrdersByStoreByDate(store.Address);
                        foreach(Order o in orders)
                        {
                            Console.WriteLine("Order:");
                            Console.WriteLine($" Customer Name: {o.Cust.Name}, Store ordered From: [{o.StoreAddress}], Date Ordered: {o.DateOfOrder}, Total: {o.Total}");
                            Console.WriteLine("   Contains:");
                            foreach(OrderLine ol in o.OrderItems)
                            {
                                Console.WriteLine($"    Product Name: {ol.Item.Name}, Amount: {ol.Quantity}");
                            }
                        }
                        break;

                    case "x":
                        break;
                    
                    default:
                        Console.WriteLine("Sorry, what you typed in was not a valid responce");
                        break;
                }

        }
    }
}