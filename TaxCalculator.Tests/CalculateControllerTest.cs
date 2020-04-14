using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using TaxCalculator.Domain.Entities;
using TaxCalculator.WebUI.Controllers;

namespace TaxCalculator.NUnitTests.Tests
{

    [TestFixture]
    public class CalculateControllerTests
    {
        CalculatorController calculateController;
        List<IncomeTax> iList;
        int count = 0;
        string taxCalculationType = "";
        
        [SetUp]
        public void Setup()
        {
            calculateController = new CalculatorController();

            iList = (List<IncomeTax>)calculateController.Index().Result;

            count = iList.Count();

            var entry = (IncomeTax)calculateController.Edit(iList.ElementAt(0).Id).Result;

            taxCalculationType = entry.TaxCalculationType;
        }
 
        [Test]
        public void GetAllTaxEntries()
        {
          Assert.IsTrue(count > 0);
        }
 
        [Test]
        public void GetTaxEntry()
        {
          Assert.IsTrue(taxCalculationType != "");
        }
    }
}