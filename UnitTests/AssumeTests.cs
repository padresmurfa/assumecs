using System;
using Xunit;
using Assumptions;

namespace UnitTests
{
    public class AssumeTests
    {
        [Fact]
        public void Fail()
        {
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.Fail();
            });
        }
        
        [Fact]
        public void Unreachable()
        {
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.Unreachable();
            });
        }
        
        [Fact]
        public void AreTrue()
        {
            Assume.IsTrue(true);
            
            Assume.IsTrue((bool?)true);
            
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.IsTrue(false);
            });
            
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.IsTrue((bool?)false);
            });
        }
        
        [Fact]
        public void AreFalse()
        {
            Assume.IsFalse(false);

            Assume.IsFalse((bool?)false);
            
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.IsFalse(true);
            });
            
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.IsFalse((bool?)true);
            });
        }
        
        [Fact]
        public void AreEqual()
        {
            Assume.AreEqual(1, 1);
            
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.AreEqual(1, 2);
            });
            
            Assume.AreEqual((int?)null, (int?)null);

            Assert.Throws<AssumptionFailure>(()=>{
                Assume.AreEqual((int?)1, (int?)null);
            });
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.AreEqual((int?)null, (int?)1);
            });
        }
        
        [Fact]
        public void AreNotEqual()
        {
            Assume.AreNotEqual(2, 1);
            
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.AreNotEqual(1, 1);
            });

            Assume.AreNotEqual((int?)2, (int?)null);
            Assume.AreNotEqual((int?)null, (int?)1);
        }
        
        [Fact]
        public void IsNull()
        {
            Assume.IsNull((int?)null);
            
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.IsNull((int?)1);
            });
        }
        
        [Fact]
        public void IsNotNull()
        {
            Assume.IsNotNull((int?)1);
            
            Assert.Throws<AssumptionFailure>(()=>{
                Assume.IsNotNull((int?)null);
            });
        }
    }
}
