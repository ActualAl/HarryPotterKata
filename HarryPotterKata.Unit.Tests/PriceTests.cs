using HarryPotterKata.BusinessLogic;
using HarryPotterKata.Core;
using NUnit.Framework;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HarryPotterKata.Unit.Tests
{
	[TestFixture]
  public class PriceTests
  {
		private Price _price;

		[SetUp]
		public void Setup()
		{
			_price = new Price();
		}

		[Test]
		public void Calculate_Raises_Arguement_Exception_When_Argument_Null()
		{
			//Arrange
			var books = default(List<Book>);
			
			//Act
			Action testAction = () => _price.Calculate(books);
			
			//Assert
			testAction.ShouldThrow<ArgumentException>("No error thrown for null books").WithMessage("No books submitted");
		}

		[Test]
		public void Calculate_Returns_Zero_When_Books_Empty()
		{
			//Arrange
			var books = new List<Book>();

			//Act
			var result = _price.Calculate(books);

			//Assert
			result.Should().Be(0);
		}

		[Test]
		public void Caclulate_Returns_Unit_Cost_When_One_Book_Submitted()
		{
			//Arrange
			var bookTypes = Enum.GetNames(typeof(Book));

			foreach (var bookType in bookTypes)
			{
				var books = new List<Book>();
				var book = (Book)Enum.Parse(typeof(Book), bookType, false); 
				books.Add(book);

				//Act
				var result = _price.Calculate(books);

				//Assert
				result.Should().Be(Rules.UnitCost, String.Format("{0} should cost £{1}", bookType, Rules.UnitCost.ToString()));
			}
		}

		[Test]
		public void Calculate_Returns_Correct_Discount_For_Simple_Combinations()
		{
			//Arrange
			var simpleScenarios = new List<List<Book>>();
			simpleScenarios.Add(new List<Book> { Book.One, Book.Two, Book.Three });
			simpleScenarios.Add(new List<Book> { Book.One, Book.Two, Book.Three, Book.Four, Book.Five });
			simpleScenarios.Add(new List<Book>() { Book.One, Book.Three });

			foreach (List<Book> scenario in simpleScenarios)
			{
				var numberOfBooks = scenario.Count();
				var expectedPrice = numberOfBooks * Rules.DiscountFor(numberOfBooks) * Rules.UnitCost;

				//Act
				var result = _price.Calculate(scenario);

				//Assert
				result.Should().Be(expectedPrice, "Incorrect price");
			}
		}

		[Test]
		public void Calculate_Returns_Correct_Discount_For_Multiple_Combinations()
		{
			//Arrange
			var inputAndExpected = new Dictionary<List<Book>, decimal>();
			
			inputAndExpected.Add(
				new List<Book> { Book.One, Book.One, Book.Two },
				(2 * Rules.DiscountFor(2) * Rules.UnitCost) + (1 * Rules.DiscountFor(1) * Rules.UnitCost));

			inputAndExpected.Add(
				new List<Book> { Book.One, Book.One, Book.Two, Book.Three, Book.Three },
				(3 * Rules.DiscountFor(3) * Rules.UnitCost) + (2 * Rules.DiscountFor(2) * Rules.UnitCost ));
			
			inputAndExpected.Add(
				new List<Book> { Book.One, Book.Two, Book.Two, Book.Three, Book.Three, Book.Four },
				(4 * Rules.DiscountFor(4) * Rules.UnitCost) + (2 * Rules.DiscountFor(2) * Rules.UnitCost));

			foreach (var scenario in inputAndExpected)
			{
				var books = scenario.Key;
				var expectedPrice = scenario.Value;

				//Act
				var price = _price.Calculate(books);

				//Assert
				price.Should().Be(expectedPrice, "Incorrect price");
			}
		}

		[Test]
		public void Calculate_Returns_Correct_Price_For_Edge_Cases()
		{
			//Arrange
			var inputAndExpected = new Dictionary<List<Book>, decimal>();

			inputAndExpected.Add(
				new List<Book> { Book.One, Book.One, Book.Two, Book.Two, Book.Three, Book.Three, Book.Four, Book.Five },
				(4 * Rules.DiscountFor(4) * Rules.UnitCost ) * 2);

			inputAndExpected.Add(
				new List<Book> 
				{ 
					Book.One, Book.One, Book.One, Book.One, Book.One,
					Book.Two, Book.Two, Book.Two, Book.Two, Book.Two,
					Book.Three, Book.Three, Book.Three, Book.Three,
					Book.Four, Book.Four, Book.Four, Book.Four, Book.Four, 
					Book.Five, Book.Five, Book.Five, Book.Five
				},
				((5 * Rules.DiscountFor(5) * Rules.UnitCost) * 3) + ((4 * Rules.DiscountFor(4) * Rules.UnitCost ) * 2)
				);

			for (var i = 0; i < inputAndExpected.Count; i ++)
			{
				var scenario = inputAndExpected.ElementAt(i);
				var books = scenario.Key;
				var expectedPrice = scenario.Value;

				//Act
				var price = _price.Calculate(books);

				//Assert
				price.Should().Be(expectedPrice, String.Format("Expected {0} but got {1} in scenrio {2}", expectedPrice.ToString(), price.ToString(), i.ToString()));
			}
		}
  }
}
