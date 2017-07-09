using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BITClientServer;
using System.Configuration;

namespace BITServices.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            BITClient s = new BITClient("Bob", "Down");

            //Act
            string str = s.ToString();

            //Assert
            Assert.AreEqual("Bob Down", str);
        }

        [TestMethod]
        public void BITClientCreation()
        {
            // Arrange
            BITClient s = new BITClient("Bob", "Down");
            //Assert
            Assert.IsInstanceOfType(s, typeof(BITClient));
            Assert.AreEqual("Bob", s.ClientFName);
            Assert.AreEqual("Down", s.ClientLName);
        }

        [TestMethod]
        public void ContractorCreation()
        {
            //Arrange
            Contractor c = new Contractor("Patrick", "Stump");
            
            //Assert
            Assert.IsInstanceOfType(c, typeof(Contractor));
            Assert.AreEqual("Patrick", c.EmployeeFName);
            Assert.AreEqual("Stump", c.EmployeeLName);
        }

        [TestMethod]
        public void CoordinatorCreation()
        {
            Coordinator coo = new Coordinator("Sally", "Supervisor");

            //Assert
            Assert.IsInstanceOfType(coo, typeof(Coordinator));
            Assert.AreEqual("Sally", coo.EmployeeFName);
            Assert.AreEqual("Supervisor", coo.EmployeeLName);
        }

        [TestMethod]
        public void EmployeeCreation()
        {
            Employee emp = new Employee("Worker", "Bee");

            Assert.IsInstanceOfType(emp, typeof(Employee));
            Assert.AreEqual("Worker", emp.EmployeeFName);
            Assert.AreEqual("Bee", emp.EmployeeLName);
        }

        [TestMethod]
        public void JobCreation()
        {
            DateTime createdate;
            DateTime duedate;
            bool fdatetime = DateTime.TryParse("06-06-2017", out createdate);
            bool sdatetime = DateTime.TryParse("06-31-2017", out duedate);
            JobRequest jr = new JobRequest(1, 2, "Please update Adobe Acrobat Reader", 23, createdate.Date, duedate.Date);

            Assert.IsInstanceOfType(jr, typeof(JobRequest));
            Assert.AreEqual(1, jr.ClientID);
            Assert.AreEqual(2, jr.CoordinatorID);
            Assert.AreEqual("Please update Adobe Acrobat Reader", jr.JobRequestDetails);
            Assert.AreEqual(createdate , jr.EntryDate);
            Assert.AreEqual(duedate, jr.DueDate);
        }

        [TestMethod]
        public void SkillCreation()
        {
            Skill skill = new Skill("Server Installation", "Contractor will install server stations for the customer. Requires the customer to have the actual servers first.");

            Assert.IsInstanceOfType(skill, typeof(Skill));
            Assert.AreEqual("Server Installation", skill.SkillTitle);
            Assert.AreEqual("Contractor will install server stations for the customer. Requires the customer to have the actual servers first.", skill.SkillDescription);
        }

        [TestMethod]
        public void LoginCreation()
        {
            Login log = new Login("username", "password");

            Assert.IsInstanceOfType(log, typeof(Login));
            Assert.AreEqual("username", log.Username);
            Assert.AreEqual("password", log.Password);
        }

        [TestMethod]
        public void TestConnStringFromAppConfig()
        {
            DataAccessLayer dal = new DataAccessLayer();

            dal.ConnectionString = "server=localhost;user id=root;database=bitservices".ToString();
            string expectedString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString.ToString();
            Assert.AreEqual(expectedString, dal.ConnectionString);
        }
    }
}
