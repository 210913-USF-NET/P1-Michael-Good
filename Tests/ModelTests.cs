using System;
using Xunit;
using Models;

namespace Tests
{
    public class ModelTests
    {
        [Fact]
        public void CustomerShouldSetValidName()
        {
            //Arrange
            Customer test = new Customer();
            string testName = "test Customer";

            //Act
            test.Name = testName;

            //Assert
            Assert.Equal(testName, test.Name);

        }

        [Fact]
        public void CustomerShouldSetValidId()
        {
            //Arrange
            Customer test = new Customer();
            int testId = 123;

            //Act
            test.Id = testId;

            //Assert
            Assert.Equal(testId, test.Id);

        }

        [Fact]
        public void CustomerShouldSetValidPhoneNum()
        {
            //Arrange
            Customer test = new Customer();
            long testPhoneNum = 8434556926;

            //Act
            test.PhoneNum = testPhoneNum;

            //Assert
            Assert.Equal(testPhoneNum, test.PhoneNum);
        }

        [Fact]
        public void StoreFrontShouldSetValidAddress()
        {
             //Arrange
            StoreFront test = new StoreFront();
            string testAddress = "693 Liberty StreetProvidence, RI 02904";

            //Act
            test.Address = testAddress;

            //Assert
            Assert.Equal(testAddress, test.Address);
        }
        
        [Fact]
        public void StoreFrontShouldSetValidId()
        {
             //Arrange
            StoreFront test = new StoreFront();
            int testId = 123;

            //Act
            test.Id = testId;

            //Assert
            Assert.Equal(testId, test.Id);
        }

        [Fact]
        public void OrderShouldSetValidId()
        {
             //Arrange
            Order test = new Order();
            int testId = 123;

            //Act
            test.Id = testId;

            //Assert
            Assert.Equal(testId, test.Id);
        }

        [Fact]
        public void OrderShouldSetValidTotal()
        {
             //Arrange
            Order test = new Order();
            decimal testTotal = (decimal)12.5400;

            //Act
            test.Total = testTotal;

            //Assert
            Assert.Equal(testTotal, test.Total);
        }

        [Fact]
        public void OrderShouldSetValidAdress()
        {
             //Arrange
            Order test = new Order();
            string testAddress = "693 Liberty StreetProvidence, RI 02904";

            //Act
            test.StoreAddress = testAddress;

            //Assert
            Assert.Equal(testAddress, test.StoreAddress);
        }

        [Fact]
        public void OrderShouldSetValidDate()
        {
             //Arrange
            Order test = new Order();
            string testDate = "9/28/2021 12:00:00 AM";

            //Act
            test.DateOfOrder = testDate;

            //Assert
            Assert.Equal(testDate, test.DateOfOrder);
        }

        [Fact]
        public void ProductShouldSetValidPrice()
        {
             //Arrange
            Product test = new Product();
            decimal testPrice = (decimal)19.42;

            //Act
            test.Price = testPrice;

            //Assert
            Assert.Equal(testPrice, test.Price);
        }
    }
}
