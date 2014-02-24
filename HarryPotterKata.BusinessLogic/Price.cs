﻿using HarryPotterKata.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPotterKata.BusinessLogic
{
    public class Price
    {
			public decimal Calculate(List<Book> books)
			{
				if (books == null)
					throw new ArgumentException(Strings.ErrorMessages.NoBooksSubmitted);
				if (!books.Any())
					return 0M;
				if (books.Count == 1)
					return Rules.DiscountFor(1) * Rules.UnitCost;
				return CalculateMultipleBooks(books);
			}

			private decimal CalculateMultipleBooks(List<Book> books)
			{
				var totalNumberOfBooks = books.Count();
				var numberOfDistinctBooks = books.Distinct().Count();
				if (numberOfDistinctBooks == totalNumberOfBooks)
					return CalculateDistinct(books);
				return CalculateMultipleCombinations(books);
			}

			private decimal CalculateMultipleCombinations(List<Book> books)
			{
				EdgeCaseType edgeCaseType;
				if (isEdgeCase(books, out edgeCaseType))
					return CalculateEdgeCaseCombinations(books, edgeCaseType);
				else
					return CalculateNonEdgeCaseCombinations(books);
			}

			private decimal CalculateNonEdgeCaseCombinations(List<Book> books)
			{
				var maxListSize = books.Count();
				var accumulativePrice = Rules.DiscountFor(0);
				var originalBooks = new List<Book>(books);
				for (var i = 0; i < originalBooks.Count(); i++)
				{
					var distinct = new List<Book>();
					for (var i2 = books.Count() - 1; i2 >= 0; i2--)
					{
						if (!distinct.Contains(books[i2]))
						{
							distinct.Add(books[i2]);
							books.RemoveAt(i2);
						}
					}
					accumulativePrice += CalculateDistinct(distinct);
					if (books.Count() == 0)
					{
						break;
					}
				}
				return accumulativePrice;
			}

			private static bool isEdgeCase(List<Book> books, out EdgeCaseType edgeCaseType)
			{
				if (isEdgeCaseOfType(books, EdgeCaseType.TotalIsEightFiveDistinct))
				{
					edgeCaseType = EdgeCaseType.TotalIsEightFiveDistinct;
					return true;
				}
				edgeCaseType = default(EdgeCaseType);
				return false;
			}

			private static bool isEdgeCaseOfType(List<Book> books, EdgeCaseType edgeCaseType)
			{
				if (edgeCaseType == EdgeCaseType.TotalIsEightFiveDistinct)
					return (books.Count() == 8 && books.Distinct().Count() == 5);
				return false;
			}

			private decimal CalculateEdgeCaseCombinations(List<Book> books, EdgeCaseType edgeCaseType)
			{
				switch (edgeCaseType)
				{
					case EdgeCaseType.TotalIsEightFiveDistinct :
						var maxListSize = 4;
						throw new NotImplementedException();
					default :
						throw new NotImplementedException("Unknown edge case");
				}
			}

			private decimal CalculateDistinct(IEnumerable<Book> distinctBooks)
			{
				var number = distinctBooks.Count();
				return number * Rules.DiscountFor(number) * Rules.UnitCost;
			}

			private enum EdgeCaseType
			{
				TotalIsEightFiveDistinct = 0
			}
    }
}