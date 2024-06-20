using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeGood.Repository.Exceptions
{
	public class ReportNotFoundExceoption : NotFoundException
	{
		public ReportNotFoundExceoption(int id) : base($"Report with id {id} is not found.")
		{

		}
	}
}