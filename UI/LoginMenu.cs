using System;
using StoreBL;
using DL;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace UI
{
    public class LoginMenu : IMenu
    {
        private IBL _bl;
        public LoginMenu(IBL bl)
        {
            _bl = bl;
        }

        /// <summary>
        /// menu for loging in customer by phone number to see if they are in the db
        /// then sends them to order menu
        /// </summary>
        public void Start()
        {
            string connectionString = File.ReadAllText(@"../connectionString.txt");
            DbContextOptions<IIDBContext> options = new DbContextOptionsBuilder<IIDBContext>().UseSqlServer(connectionString).Options;
            IIDBContext context = new IIDBContext(options);

            Models.Customer customer = null;
            string input;
            LogStart:
            Console.WriteLine("Please enter your phone number no (, ), -");
            input = Console.ReadLine();
            long parsedInput;
            bool parseSuccess = Int64.TryParse(input, out parsedInput);
            if(parseSuccess && parsedInput >= 0)
            {
                customer = _bl.GetCustomerByPhone(parsedInput);
                if(customer.Id == 0)
                {   
                    phoneStart:
                    Console.WriteLine("That number is not registared, would you like to try another?");
                    Console.WriteLine("[0] Yes");
                    Console.WriteLine("[1] No");
                    input = Console.ReadLine();
                    switch (input)
                    {
                        case "1":
                            return;

                        case "0":
                            goto LogStart;

                        default:
                            Console.WriteLine("Sorry, what you typed in was not a valid responce");
                            goto phoneStart;
                    }
                }
            }
            else
            {
                Console.WriteLine("invalid input");
                goto LogStart;
            }

            Console.WriteLine($"Welcome Back {customer.Name}!");
            new OrderMenu(new BL(new DBRepo(context))).Start(customer.Id);
        }
    }
}