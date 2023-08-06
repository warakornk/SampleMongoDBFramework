using MongoFramework;
using SampleMongoDBFramework.Models.Entities;

namespace SampleMongoDBFramework.DataAccess
{
	public class DbContext : MongoDbContext
	{
		public DbContext(IMongoDbConnection connection) : base(connection)
		{
		}

		// Add model which map to MongoDb collection here
		// Model will use in singularity and property name use plural.

		public MongoDbSet<Document> Documents { get; set; } = null!;

		public MongoDbSet<Province> Provinces { get; set; } = null!;
	}
}