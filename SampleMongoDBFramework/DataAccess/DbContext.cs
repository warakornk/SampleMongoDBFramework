using MongoFramework;
using SampleMongoDBFramework.Models.Entities;

namespace SampleMongoDBFramework.DataAccess
{
	public class DbContext : MongoDbContext
	{
		public DbContext(IMongoDbConnection connection) : base(connection)
		{
		}

		public MongoDbSet<Document> Documents { get; set; } = null!;

		public MongoDbSet<Province> Provinces { get; set; } = null!;
	}
}