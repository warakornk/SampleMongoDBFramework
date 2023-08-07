using SampleMongoDBFramework.Models;
using SampleMongoDBFramework.Models.Entities;

namespace SampleMongoDBFramework.Repository.Interface
{
	public interface IDbRepository
	{
		#region Document

		public Task<Document> GetDocumentByDocumentIdAsync(Guid documentId);

		public Task<List<Document>> GetDocumentsAsync();

		public Task CreateDocumentAsync(CreateDocumentDto createDocumentDto);

		public Task UpdateDocumentAsync(Guid documentId, UpdateDocumentDto updateDocumentDto);

		public Task DeleteDocumentAsync(Guid documentId);

		#endregion Document

		#region Province

		public Task<List<Province>> GetProvincesAsync();

		public Task<Province> GetProvinceAsync(string provinceId);

		public Task CreateProvinceAsync(CreateProvinceDto createProvinceDto);

		public Task UpdateProvinceAsync(string provinceId, UpdateProvinceDto updateProvinceDto);

		public Task DeleteProvinceAsync(string provinceId);

		#endregion Province

		#region Amphur

		public Task<List<Amphur>> GetAmphursAsync(string provinceId);

		public Task<Amphur?> GetAmphurAsync(string provinceId, string amphurId);

		public Task CreateAmphurAsync(CreateAmphurDto createAmphurDto);

		public Task UpdateAmphurAsync(string provinceId, string amphurId, UpdateAmphurDto updateAmphurDto);

		public Task DeleteAmphurAsync(string provinceId, string amphueId);

		#endregion Amphur
	}
}