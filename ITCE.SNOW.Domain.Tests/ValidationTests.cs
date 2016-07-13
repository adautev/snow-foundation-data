using System;
using FluentValidation;
using ITCE.SNOW.Domain.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITCE.SNOW.Domain.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void TestSupportGroupValidation()
        {
            var supportGroupValidator = new SupportGroupValidator();
            var supportGroup = new SupportGroup();
            var result = supportGroupValidator.Validate(supportGroup);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(3, result.Errors.Count);
        }
    }
}
