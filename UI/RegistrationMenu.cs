using System;
using Serilog;
using StoreBL;
using DL;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace UI
{
    public class RegistrationMenu : IMenu
    {
        private IBL _bl;
        public RegistrationMenu(IBL bl)
        {
            _bl = bl;
        }

        /// <summary>
        /// registers new customer into db given their phone number and name
        /// then sends them to the order menu
        /// </summary>
        public void Start()
        {
            string connectionString = File.ReadAllText(@"../connectionString.txt");
            DbContextOptions<IIDBContext> options = new DbContextOptionsBuilder<IIDBContext>().UseSqlServer(connectionString).Options;
            IIDBContext context = new IIDBContext(options);


            Models.Customer customer;
            string input;
            RegStart:
            Console.WriteLine("Please enter your phone number no (, ), -");
            input = Console.ReadLine();
            long parsedInput;
            bool parseSuccess = Int64.TryParse(input, out parsedInput);
            if(parseSuccess && parsedInput >= 1000000000)
            {
                customer = _bl.GetCustomerByPhone(parsedInput);
                if(customer.Id != 0)
                {   
                    bool exit = false;
                    do{
                        Console.WriteLine("That number is already registared, would you like to try another?");
                        Console.WriteLine("[0] Yes");
                        Console.WriteLine("[1] No");
                        input = Console.ReadLine();
                        switch (input)
                        {
                            case "1":
                                return;

                            case "0":
                                goto RegStart;

                            default:
                                Console.WriteLine("Sorry, what you typed in was not a valid responce");
                                break;
                        }
                    }while(exit);
                }
            }
            else
            {
                Console.WriteLine("invalid input");
                goto RegStart;
            }

            Console.WriteLine("Please enter your name");
            string name = Console.ReadLine();

            customer = new Models.Customer(name, parsedInput);

            Log.Information($"Customer being added Name: {customer.Name}, PhoneNum: {customer.PhoneNum}");
            _bl.AddNewCustomer(customer);
            customer = _bl.GetCustomerByPhone(parsedInput);
            Console.WriteLine($"Welcome {customer.Name}!");
            new OrderMenu(new BL(new DBRepo(context))).Start(customer.Id);
        }
    }
}