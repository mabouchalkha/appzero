using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using componentModel = System.ComponentModel;

namespace ExtensionsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetName()
        {
            var intro = Module.Intro;
            Assert.AreEqual("Intro", intro.ToString());
            Assert.AreEqual("Intro", Enum.GetName(typeof(Module), intro));
            Assert.AreEqual("Intro", intro.GetName()); //"Intro"
        }

        [TestMethod]
        public void GetDescription()
        {
            Assert.AreEqual("Introducing Extension Methods", Module.Intro.GetDescription()); //"Introducing Extension Methods"
            Assert.AreEqual("Library", Module.Library.GetDescription());

            Assert.AreEqual("In Progress", ModuleStatus.InProgress.GetDescription());
            Assert.AreEqual("Complete", ModuleStatus.Complete.GetDescription());
        }

        public enum ModuleStatus
        {
            Todo = 1,
            [componentModel.Description("In Progress")]
            InProgress = 2,
            Complete = 3
        }
        public enum Module
        {
            [componentModel.Description("Introducing Extension Methods")]
            Intro,
            Advanced,
            Library
        }

        [TestMethod]
        public void DivideBy0()
        {
            try
            {
                Divide(10, 0);
                Assert.Fail("Should throw exception");
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
                //Debug.WriteLine(ex);
                Debug.WriteLine(ex.FullMessage());
                Assert.IsInstanceOfType(ex, typeof(ApplicationException));
            }
        }

        public double Divide(int amount, int by)
        {
            try
            {
                return amount / by;
            }
            catch (Exception ex)
            {
                var invalidOpEx = new InvalidOperationException("Invalid operation", ex);
                var message = string.Format("Divide failed - amount: {0}, by: {1}", amount, by);
                throw new ApplicationException(message, invalidOpEx);
            }
        }
    }
}
