using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SampleMongoDBFramework.Models.Entities
{
	//[BsonIgnoreExtraElements(true)]
	public class Document
	{
		[BsonId][BsonRepresentation(BsonType.ObjectId)] public ObjectId Id { get; set; }
		public Guid DocumentId { get; set; }
		public string DocumentName { get; set; } = "";
		public int PageCount { get; set; } = 0;
		public DateTimeOffset? PublishDate { get; set; }
		public string? SysCreateUser { get; set; } = "";
		public string? SysCreateIP { get; set; } = "";
		public DateTimeOffset SysCreateDate { get; set; } = DateTimeOffset.Now;
		public string SysUpdateUser { get; set; } = "";
		public string SysUpdateIP { get; set; } = "";
		public DateTimeOffset? SysUpdateDate { get; set; } = null;
	}
}