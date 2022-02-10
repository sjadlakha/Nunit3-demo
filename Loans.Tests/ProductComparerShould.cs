using System;
using System.Collections.Generic;
using Loans.Domain.Applications;
using NUnit.Framework;

namespace Loans.Tests
{
    public class ProductComparerShould
    {
        //Let's reduce the repeated code

        private List<LoanProduct> products;
        private ProductComparer sut;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // Run code that might cause delay if it is executed before every test
            // Assume that this list will not be modified by any tests
            // as this will potentially break other tests.(i.e. break test isolation)
            products = new List<LoanProduct>
            {
                new LoanProduct(1,"a",1),
                new LoanProduct(2,"b",2),
                new LoanProduct(3,"c",3),
            };
        }


        [SetUp]
        public void Setup()
        {
            sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);
        }

        [TearDown]
        public void TearDown()
        {
            // Runs after each test executes
            // sut.Dispose(); // if sut implements idisposable
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // products.Dispose(); // if products implemented IDisposable
        }

        [Test]
        public void ReturnCorrectNumberOfComparisons()
        {

            //var products = new List<LoanProduct>
            //{
            //    new LoanProduct(1,"a",1),
            //    new LoanProduct(2,"b",2),
            //    new LoanProduct(3,"c",3),
            //};

            //var sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);

            List<MonthlyRepaymentComparison> comparisons = sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisons, Has.Exactly(3).Items);
        }

        // Asserting Exceptions
        //[Test] 
        //public void NotAllowedZeroYears()
        //{
        //    Assert.That(() => new LoanTerm(0), Throws.TypeOf<ArgumentOutOfRangeException>());
        //}

        [Test]
        public void NotReturnDuplicateComparisons()
        {
            //var products = new List<LoanProduct>
            //{
            //    new LoanProduct(1, "a", 1),
            //    new LoanProduct(2, "b", 2),
            //    new LoanProduct(3, "c", 3)
            //};

            //var sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);

            List<MonthlyRepaymentComparison> comparisons =
                sut.CompareMonthlyRepayments(new LoanTerm(30));

            Assert.That(comparisons, Is.Unique);
        }

        [Test]
        public void ReturnComparisonForFirstProduct()
        {
            //var products = new List<LoanProduct>
            //{
            //    new LoanProduct(1, "a", 1),
            //    new LoanProduct(2, "b", 2),
            //    new LoanProduct(3, "c", 3)
            //};

            //var sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);

            List<MonthlyRepaymentComparison> comparisons =
                sut.CompareMonthlyRepayments(new LoanTerm(30));

            // Need to also know the expected monthly repayment
            var expectedProduct = new MonthlyRepaymentComparison("a", 1, 643.28m);

            Assert.That(comparisons, Does.Contain(expectedProduct));
        }

        [Test]
        public void ReturnComparisonForFirstProduct_WithPartialKnownExpectedValues()
        {
            //var products = new List<LoanProduct>
            //{
            //    new LoanProduct(1, "a", 1),
            //    new LoanProduct(2, "b", 2),
            //    new LoanProduct(3, "c", 3)
            //};

            //var sut = new ProductComparer(new LoanAmount("USD", 200_000m), products);

            List<MonthlyRepaymentComparison> comparisons =
                sut.CompareMonthlyRepayments(new LoanTerm(30));

            // Don't care about the expected monthly repayment, only that the product is there
            //Assert.That(comparisons, Has.Exactly(1)
            //                             .Property("ProductName").EqualTo("a")
            //                             .And
            //                             .Property("InterestRate").EqualTo(1)
            //                             .And
            //                             .Property("MonthlyRepayment").GreaterThan(0));

            Assert.That(comparisons, Has.Exactly(1)
                                        .Matches<MonthlyRepaymentComparison>(
                                                item => item.ProductName == "a" &&
                                                        item.InterestRate == 1 &&
                                                        item.MonthlyRepayment > 0));
        }
    }
} 
