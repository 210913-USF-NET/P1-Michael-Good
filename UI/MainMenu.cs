using System;
using StoreBL;
using DL;
using DL.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace UI
{
    public class MainMenu : IMenu
    {
        private IBL _bl;
        public MainMenu(IBL bl)
        {
            _bl = bl;
        }

        /// <summary>
        /// main menu used to send to either
        /// login menu
        /// register menu
        /// admin menu
        /// </summary>
        public void Start()
        {   
            string connectionString = File.ReadAllText(@"../connectionString.txt");
            DbContextOptions<IIDBContext> options = new DbContextOptionsBuilder<IIDBContext>().UseSqlServer(connectionString).Options;
            IIDBContext context = new IIDBContext(options);


            bool exit = false;
            string input = "";
            do
            {
                Console.WriteLine("Welcome to Impossible Items!");
                Console.WriteLine("Have you shopped with us before?");
                Console.WriteLine("[0] Yes");
                Console.WriteLine("[1] No");
                Console.WriteLine("[x] Leave");
                //Console.WriteLine(_bl.GetStoreFrontById(1));
                input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        new LoginMenu(new BL(new DBRepo(context))).Start();
                        break;

                    case "1":
                        new RegistrationMenu(new BL(new DBRepo(context))).Start();
                        break;

                    case "x":
                        Console.WriteLine("Goodbye!");
                        exit = true;
                        break;
                    
                    case "admin":
                        Console.WriteLine("enter password");
                        input = Console.ReadLine();
                        if(input == "Iloveshrek<3")
                        {
                            new AdminMenu(new BL(new DBRepo(context))).Start();
                        }
                        else
                        {
                            Console.WriteLine("Incorrect password");
                        }
                        break;

                    default:
                        Console.WriteLine("Sorry, what you typed in was not a valid responce");
                        break;
                }
            }while (!exit);
        }
    }
}