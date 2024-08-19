using CustomerTool.Models.Db;
using System.Collections.Concurrent;

namespace CustomerTool.Ef
{
    public class CustomerContext
    {
        public readonly ConcurrentBag<Customer> Customers;
        public CustomerContext()
        {
            Customers = new ConcurrentBag<Customer>();
            SeedData();
        }

        private void SeedData()
        {
            Customers.Add(new Customer { Name = "Customer One", DateOfBirth = new DateTime(1995, 4, 30), Address = "1A Manfield Avenue, Coventry", Gender = 'M', Email = "napsfihpi@gmail.com", Phone = string.Empty });
            Customers.Add(new Customer { Name = "Customer Two", DateOfBirth = new DateTime(1995, 4, 29), Address = "15 Manfield Avenue, Coventry", Gender = 'F', Email = string.Empty, Phone = "07587768453" });
            Customers.Add(new Customer { Name = "Customer Three", DateOfBirth = new DateTime(1995, 4, 28), Address = "120B Manfield Avenue, Coventry", Gender = 'M', Email = "npqjfpi@gmail.com", Phone = string.Empty });
            Customers.Add(new Customer { Name = "Customer Four", DateOfBirth = new DateTime(1995, 4, 27), Address = "11 Manfield Avenue, Coventry", Gender = 'F', Email = string.Empty, Phone = "07587768028" });
            Customers.Add(new Customer { Name = "Customer Five", DateOfBirth = new DateTime(1995, 4, 26), Address = "2C Manfield Avenue, Coventry", Gender = null, Email = "owdhfoa@gmail.com", Phone = string.Empty });
        }
    }
}
