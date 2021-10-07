namespace Models
{
    public class Product
    {
        public Product(){}
        public Product(string name, decimal price)
        {
            this.Name = name;
            this.Price = price;
        }
        public int Id{get;set;}
        public string Name{get;set;}
        public decimal Price{get;set;}

    }
}