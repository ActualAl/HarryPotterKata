using HarryPotterKata.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarryPotterKata.BusinessLogic
{
	public static class Rules
	{
		public static decimal UnitCost
		{
			get { return 8.0M; }
		}

		public static decimal DiscountFor(int numberOfItems)
		{
			switch (numberOfItems)
			{
				case 0:
					return 0M;
				case 2:
					return 0.95M;
				case 3:
					return 0.9M;
				case 4:
					return 0.8M;
				case 5:
					return 0.75M;
				case 1:
				default:
					return 1.0M;
			}
		}
	}
}
