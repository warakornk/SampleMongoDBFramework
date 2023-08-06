using SampleMongoDBFramework.Models;
using SampleMongoDBFramework.Models.Entities;

namespace SampleMongoDBFramework.Repository.Interface
{
	public interface IDbRepository
	{
		#region Document

		public Task<Document> GetDcoumentByDocumentId(Guid documentId);

		public Task<List<Document>> GetDocuments();

		public Task CreateDocument(CreateDocumentDto createDocumentDto);

		public Task UpdateDocument(Guid documentId, UpdateDocumentDto updateDocumentDto);

		public Task DeleteDocument(Guid documentId);

		#endregion Document

		#region Province

		public Task<List<Province>> GetProvinces();

		public Task<Province> GetProvince(string provinceId);

		public Task CreateProvince(CreateProvinceDto createProvinceDto);

		public Task UpdateProvince(string provinceId, UpdateProvinceDto updateProvinceDto);

		public Task DeleteProvince(string provinceId);

		#endregion Province

		#region Amphur

		public Task<List<Amphur>> GetAmphurs(string provinceId);

		public Task<Amphur?> GetAmphur(string provinceId, string amphurId);

		public Task CreateAmphur(CreateAmphurDto createAmphurDto);

		public Task UpdateAmphur(string provinceId, string amphurId, UpdateAmphurDto updateAmphurDto);

		public Task DeleteAmphur(string provinceId, string amphueId);

		#endregion Amphur
	}
}