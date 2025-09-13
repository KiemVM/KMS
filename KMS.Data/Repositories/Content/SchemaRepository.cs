using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KMS.Common.Helper;
using KMS.Core.Entities.Content;
using KMS.Core.ViewModels;
using KMS.Core.ViewModels.Content;
using KMS.Core.ViewModels.Extension;
using KMS.Core.ViewModels.Identity;
using KMS.Data.SeedWorks;

namespace KMS.Data.Repositories.Content
{
    public interface ISchemaRepository : IRepositoryBase<Schema>
    {
        Task<PagedResult<SchemaViewModel>> PagingAsync(int page, int pageSize, string search, SchemaStatus schemaStatus, Guid companyId, Guid applicationId);

        Task<SchemaViewModel> InsertAsync(SchemaViewModel schemaViewModel);

        Task<SchemaViewModel> UpdateAsync(SchemaViewModel schemaViewModel);

        Task<SchemaViewModel> FindByIdAsync(Guid id);

        Task<List<ComboboxViewModel>> SelectAsync(string? q);

        Task RemoveAsync(Guid id);
        Task<List<SchemaViewModel>> GetAllSchema();
        Task<List<ComboboxTreeViewModel>> SelectTreeByTypeAsync(string? q = "");
    }

    public class SchemaRepository : RepositoryBase<Schema>, ISchemaRepository
    {
        private readonly IMapper _mapper;

        public SchemaRepository(SaaSContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<List<ComboboxTreeViewModel>> SelectTreeByTypeAsync(string? q = "")
        {
            var allSchema = await (from schema in FindAll()
                where schema.Status != SchemaStatus.Delete
                select new ComboboxTreeViewModel()
                {
                    Value = schema.Id,
                    Name = schema.Name,
                    Description = schema.Description,
                }).ToListAsync();

            if (!string.IsNullOrWhiteSpace(q))
            {
                allSchema = allSchema.Where(x => (x.Name ?? "").Contains(q)).ToList();
            }

            // Xây dựng cây và tính level
            var result = new List<ComboboxTreeViewModel>();
            BuildTree(allSchema, result, Guid.Empty, 0);

            return result;
        }

        private void BuildTree(List<ComboboxTreeViewModel> allCategories, List<ComboboxTreeViewModel> result, Guid parentId, int level)
        {
            var children = allCategories.Where(x => x.ParentId == parentId).OrderBy(x => x.Name);

            foreach (var child in children)
            {
                child.Level = level;
                // Tạo prefix để hiển thị level
                var prefix = new string('─', level * 2);
                if (level > 0)
                {
                    prefix = "├" + prefix;
                }
                child.DisplayName = prefix + " " + child.Name;

                result.Add(child);

                // Đệ quy để thêm các con của node hiện tại
                BuildTree(allCategories, result, child.Value, level + 1);
            }
        }

        public async Task<List<SchemaViewModel>> GetAllSchema()
        {
            var query = from schema in FindAll()
                join user in _context.Users on schema.UserIdCreated equals user.Id
                join app in _context.Applications on schema.ApplicationId equals app.Id
                where schema.Status != SchemaStatus.Delete
                orderby schema.Count
                select new SchemaViewModel()
                {
                    Id = schema.Id,
                    Name = schema.Name,
                    Database = schema.Database,
                    DateCreated = schema.DateCreated,
                    DateModified = schema.DateModified,
                    UserIdCreated = schema.UserIdCreated,
                    UserIdModified = schema.UserIdModified,
                    Number = schema.Number,
                    Status = schema.Status,
                    AvatarViewModel = AvatarViewModel.UpdateAvatar(user),
                    ApplicationName = app.Name // Add this line
                };
            return query.ToList();
        }

        public async Task<PagedResult<SchemaViewModel>> PagingAsync(
    int page, int pageSize, string search, SchemaStatus schemaStatus, Guid companyId, Guid applicationId)
        {
            var query = from schema in FindAll()
                        join user in _context.Users on schema.UserIdCreated equals user.Id
                        join app in _context.Applications on schema.ApplicationId equals app.Id
                        join company in _context.Companies on app.CompanyId equals company.Id
                        where schema.Status != SchemaStatus.Delete
                        select new SchemaViewModel()
                        {
                            Id = schema.Id,
                            Name = schema.Name,
                            Database = schema.Database,
                            DateCreated = schema.DateCreated,
                            DateModified = schema.DateModified,
                            UserIdCreated = schema.UserIdCreated,
                            UserIdModified = schema.UserIdModified,
                            Number = schema.Number,
                            Status = schema.Status,
                            AvatarViewModel = AvatarViewModel.UpdateAvatar(user),
                            ApplicationId = schema.ApplicationId,
                            ApplicationName = app.Name,
                            ApplicationStatus = app.Status,
                            CompanyId = company.Id,
                            CompanyName = company.Name,
                            CompanyStatus = company.Status
                        };

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => (x.Number ?? "").Contains(search)
                                         || (x.AvatarViewModel!.UserName ?? "").Contains(search)
                                         || (x.ApplicationName ?? "").Contains(search)
                                         || (x.CompanyName ?? "").Contains(search));
            }
            if (schemaStatus != SchemaStatus.All)
            {
                query = query.Where(x => x.Status == schemaStatus);
            }
            if (applicationId != Guid.Empty)
            {
                query = query.Where(x => x.ApplicationId == applicationId);
            }
            if (companyId != Guid.Empty)
            {
                query = query.Where(x => x.CompanyId == companyId);
            }

            return new PagedResult<SchemaViewModel>
            {
                Results = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(),
                CurrentPage = page,
                RowCount = await query.CountAsync(),
                PageSize = pageSize
            };
        }

        public async Task<SchemaViewModel> InsertAsync(SchemaViewModel schemaViewModel)
        {
            Schema? schema = new Schema() { Id = Guid.NewGuid() };
            schema.UpdateViewModel(schemaViewModel);
            schema.UserIdCreated = schemaViewModel.UserIdCreated;
            schema.Count = ((await FindAll().OrderBy(x => x.Count).LastOrDefaultAsync())?.Count ?? 0) + 1;
            schema.Number = SchemaConst.PreNumber + schema.Count.ToString("0000");
            await AddAsync(schema);
            schemaViewModel.Id = schema.Id;
            schemaViewModel.Number = schema.Number;
            return schemaViewModel;
        }

        public async Task<SchemaViewModel> UpdateAsync(SchemaViewModel schemaViewModel)
        {
            Schema? schema = await FindIdAsync(schemaViewModel.Id);
            if (schema is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            schema.UpdateViewModel(schemaViewModel);
            schema.UserIdModified = schemaViewModel.UserIdModified;
            Update(schema);
            return schemaViewModel;
        }

        public async Task<SchemaViewModel> FindByIdAsync(Guid id)
        {
            var schema = await _context.Schemas
                .Where(s => s.Id == id && s.Status != SchemaStatus.Delete)
                .Select(s => new SchemaViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    Database = s.Database,
                    ApplicationId = s.ApplicationId,
                    DateCreated = s.DateCreated,
                    DateModified = s.DateModified,
                    UserIdCreated = s.UserIdCreated,
                    UserIdModified = s.UserIdModified,
                    Number = s.Number,
                    Status = s.Status,
                    Tables = _context.Tables
                        .Where(t => t.SchemaId == s.Id && t.Status != TableStatus.Delete)
                        .Select(t => new TableViewModel
                        {
                            Id = t.Id,
                            Name = t.Name,
                            SchemaId = t.SchemaId,
                            UserIdCreated = t.UserIdCreated,
                            UserIdModified = t.UserIdModified,
                            Status = t.Status,
                            TableDetails = _context.TableDetails
                                .Where(td => td.TableId == t.Id && td.Status != TableDetailStatus.Delete)
                                .Select(td => new TableDetailViewModel
                                {
                                    Id = td.Id,
                                    TableId = td.TableId,
                                    Status = td.Status,
                                    Name = td.Name,
                                    DataType = td.DataType,
                                    IsEncrypt = td.IsEncrypt
                                }).ToList()
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return schema ?? new SchemaViewModel();
        }

        public async Task<List<ComboboxViewModel>> SelectAsync(string? q)
        {
            var query = from schema in FindAll()
                        where schema.Status != SchemaStatus.Delete
                        select new ComboboxViewModel()
                        {
                            Value = schema.Id,
                            Name = schema.Number,
                            Description = schema.Name,
                        };
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(x => (x.Name ?? "").Contains(q));
            }

            return await query.ToListAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            Schema? schema = await FindIdAsync(id);
            if (schema is null) throw new ServiceExceptionHelper("Không tồn tại đối tượng này.");
            schema.Status = SchemaStatus.Delete;
            Update(schema);
        }
    }
}
