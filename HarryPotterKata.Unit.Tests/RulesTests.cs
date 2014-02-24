using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HarryPotterKata.BusinessLogic;

namespace HarryPotterKata.Unit.Tests
{
	[TestFixture]
	public class RulesTests
	{

		private string reason(int number)
		{
			return String.Format("Incorrect discount for {0} item", number);
		}

		[Test]
		public void Rules_Returns_Correct_Discount_For_Item_Count()
		{
			//Arrange
			Dictionary<int, decimal> inputAndExpectedDiscount = new Dictionary<int, decimal> ();

			inputAndExpectedDiscount.Add(0, 0M);
			inputAndExpectedDiscount.Add(1, 1M);
			inputAndExpectedDiscount.Add(2, 0.95M);
			inputAndExpectedDiscount.Add(3, 0.9M);
			inputAndExpectedDiscount.Add(4, 0.8M);
			inputAndExpectedDiscount.Add(5, 0.75M);

			foreach (var scenario in inputAndExpectedDiscount)
			{
				//Act
				var discount = Rules.DiscountFor(scenario.Key);
				var expected = scenario.Value;

				//Assert
				discount.Should().Be(expected, reason(scenario.Key));
			}
			
		}
	}
}
