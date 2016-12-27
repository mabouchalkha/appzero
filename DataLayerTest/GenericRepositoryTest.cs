using DataLayerTest.Repositories;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLayerTest.Entities;
using System.Collections.Generic;

namespace DataLayerTest
{
    [TestClass]
    public class GenericRepositoryTest
    {
        CustomerRepository _customerRepository;

        public GenericRepositoryTest()
        {
            _customerRepository = new CustomerRepository();
        }

        [TestMethod]
        public void Get_All_NoTracking()
        {
            IEnumerable<Customer> customer = _customerRepository.GetAll();
            var customers = _customerRepository.Query("LastName, FirstName", 1, 15);
    
            Assert.AreEqual(6, customer.Count());
        }

        [TestMethod]
        public void Get_ById_NoTracking()
        {
            var customer = _customerRepository.GetById(1);
            Assert.AreEqual("Amine", customer.FirstName);
        }

        [TestMethod]
        public void Get_By_NoTracking()
        {
            var customer = _customerRepository.GetBy(c => c.LastName == "bouchalkha");
            Assert.AreEqual(4, customer.Count());
        }

        [TestMethod]
        public void Add_Customer()
        {
            var customer = new Customer
            {
                FirstName = "Zuijdane",
                LastName = "ahaoulid"
            };

            customer = _customerRepository.Add(customer);

            Assert.IsNotNull(customer);
        }

        [TestMethod]
        public void Update_Customer()
        {
            var customer = new Customer
            {
                CustomerId = 7,
                FirstName = "Ouijdane",
                LastName = "chaoulid2"
            };

            customer = _customerRepository.Update(customer);

            Assert.IsNotNull(customer);
        }

        [TestMethod]
        public void Delete_Entity_Customer()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void Delete_ById_Customer()
        {
            //_customerRepository.Delete(6);

            Assert.IsTrue(false);
        }
    }
}
