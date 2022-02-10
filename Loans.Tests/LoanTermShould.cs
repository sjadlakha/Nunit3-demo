using System;
using System.Collections.Generic;
using Loans.Domain.Applications;
using NUnit.Framework;

namespace Loans.Tests
{
    //the attributes in [] are to tell nunit about the methods and classes
    [TestFixture] // this nunit attribute is optional with Nunit 3
    public class LoanTermShould
    {
        [Test]
        public void ReturnTermInMonths()
        {
            // create an instance of what we watn to test
            // sut = system under test
            var sut = new LoanTerm(1);

            Assert.That(sut.ToMonths(), Is.EqualTo(12), "Months = 12 x Years");
        }

        [Test]
        public void StoreYears()
        {
            var sut = new LoanTerm(1);

            Assert.That(sut.Years, Is.EqualTo(1));
        }

        [Test]
        public void RespectValueInEquality()
        {
            // 2 values are equal or 2 reference point to
            // the same location in memory unless in reference type you
            // have overriden the equals method(like here for LoanTerm)

            var a = new LoanTerm(1);
            var b = new LoanTerm(2);

            Assert.That(a, Is.Not.EqualTo(b));
        }


        [Test]
        public void ReferenceEqualityExample()
        {
            var a = new LoanTerm(1);
            var b = a;
            var c = new LoanTerm(1);

            Assert.That(a, Is.SameAs(b)); // passes
            // SameAs is a constraint for checking equality in reference types
            //Assert.That(a, Is.SameAs(c)); //fails
            Assert.That(a, Is.Not.SameAs(c)); //passes


            var x = new List<string> { "a", "b" };
            var y = x;

            Assert.That(x, Is.SameAs(y)); // passes
        }

        // Floating point values
        [Test]
        public void Double()
        {
            double a = 1.0 / 3.0;
            Assert.That(a, Is.EqualTo(0.33).Within(0.004)); // within provides tolerance
            Assert.That(a, Is.EqualTo(0.33).Within(10).Percent); 
        }

        // Exceptions assertion
        //uncomment to try
        [Test]
        [Ignore("This will always fail because of loan term argument = 0")]
        public void NotAllowZeroYears()
        {
            Assert.That(() => new LoanTerm(0), Throws.TypeOf<ArgumentOutOfRangeException>());

            Assert.That(() => new LoanTerm(0), Throws.TypeOf<ArgumentOutOfRangeException>()
                             .With
                             .Property("Message")
                             .EqualTo("Please specify a value greater than 0.\r\nParameter name: years"));

            Assert.That(() => new LoanTerm(0), Throws.TypeOf<ArgumentOutOfRangeException>()
                             .With
                             .Message
                             .EqualTo("Please specify a value greater than 0.\r\nParameter name: years"));

            // Correct ex and para name but don't care about the message
            Assert.That(() => new LoanTerm(0), Throws.TypeOf<ArgumentOutOfRangeException>()
                             .With
                             .Property("ParamName")
                             .EqualTo("years"));

            Assert.That(() => new LoanTerm(0), Throws.TypeOf<ArgumentOutOfRangeException>()
                                         .With
                                         .Matches<ArgumentOutOfRangeException>(
                                             ex => ex.ParamName == "years"));
        }

    }
}
