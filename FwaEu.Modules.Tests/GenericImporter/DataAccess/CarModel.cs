using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Modules.GenericImporter;
using FwaEu.Modules.GenericImporter.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Tests.GenericImporter.DataAccess
{
	public class CarModel
	{
		public int Id { get; set; }
		public string Color { get; set; }
		public string Brand { get; set; }
		public decimal Price { get; set; }
	}
}
