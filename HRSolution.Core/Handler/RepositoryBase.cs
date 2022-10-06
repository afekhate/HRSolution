using Dapper;
using HRSolution.Core.Contract;
using HRSolution.Infrastructure.Domain;
using HRSolution.Infrastructure.Domain.AuditLogs;
using HRSolution.Infrastructure.Enums;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HRSolution.Infrastructure.Core.Handler
{
    public abstract class RepositoryBase<T> : IAsyncGenericRepository<T> where T : BaseObject
    {
        private readonly string _tableName;
        protected string _connectionString;

       

        public RepositoryBase(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tableName = typeof(T).Name;
            
        }

        public async Task<int> AddAsync(T entity)
        {
            var columns = GetColumns();
            var stringOfColumns = string.Join(", ", columns);
            var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
            var query = $"insert into {_tableName} ({stringOfColumns}) values ({stringOfParameters})";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var result = await conn.ExecuteAsync(query, entity);


                //Audit Trail
                ///////////////////////////////////////////////////////////
                var Entity = entity;
                var allRecords = new List<string>();

                foreach (var item in columns)
                {
                    var propertyInfo = entity.GetType().GetProperty(item);
                    var dValue = propertyInfo.GetValue(entity, null);

                    allRecords.Add(item +"="+ dValue +" || ");
                }

                var audit = new AuditLog
                {
                    UserId = entity.CreatedBy, ModuleName = _tableName, ModuleAction = "Add" + _tableName,
                    Description = "A new record was added", Record = string.Join(" ", allRecords), OldRecord = "",
                    ActionType = ActionType.Create.ToString(), CreatedBy =  entity.CreatedBy, CreatedDate =  entity.CreatedDate
                };

                var auditCol = CustomColumns(audit);
                var auditColumns = string.Join(", ", auditCol);
                var auditOfParameters = string.Join(", ", auditCol.Select(e => "@" + e));
                var auditQuery = $"insert into AuditLog ({auditColumns}) values ({auditOfParameters})";
                var auditResponse = await conn.ExecuteAsync(auditQuery, audit);

               
                ///////////////////////////////////////////////////////////
                //End Audit Trail




                return result;
            }
        }
        public async Task DeleteAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                var sql = $"select * from {_tableName} where [Id] = {id}";
                var entity = await conn.QueryAsync<T>(sql);


                conn.Open();
                await conn.ExecuteAsync($"DELETE FROM {_tableName} WHERE [Id] = @Id", new { Id = id });

                //Audit Trail


                var audit = new AuditLog
                {
                    UserId = entity.FirstOrDefault().CreatedBy,
                    ModuleName = _tableName,
                    ModuleAction = "Delete" + _tableName,
                    Description = "A record was deleted",
                    Record = entity.ToString(),
                    OldRecord = "",
                    ActionType = ActionType.Delete.ToString(),
                    CreatedBy = entity.FirstOrDefault().CreatedBy,
                    CreatedDate = entity.FirstOrDefault().CreatedDate
                };

                var auditCol = CustomColumns(audit);
                var auditColumns = string.Join(", ", auditCol);
                var auditOfParameters = string.Join(", ", auditCol.Select(e => "@" + e));
                var auditQuery = $"insert into AuditLog ({auditColumns}) values ({auditOfParameters})";
                var auditResponse = await conn.ExecuteAsync(auditQuery, audit);
            }
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>($"SELECT * FROM {_tableName}");
                return data;
            }
        }
        public async Task<T> GetByIdAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>($"SELECT * FROM {_tableName} WHERE Id = @Id", new { Id = id });
                return data.FirstOrDefault();
            }
            
        }
        public async Task UpdateAsync(T entity)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                
                var Psql = $"select * from {_tableName} where [Id] = {entity.Id}";
                var existingRecord = await conn.QueryAsync<T>(Psql);
                entity.CreatedDate = existingRecord.FirstOrDefault().CreatedDate;
          

                var columns = GetSpecificColumns();
                var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
                var query = $"update {_tableName} set {stringOfColumns} where Id = @Id";

                await conn.ExecuteAsync(query, entity);

                //Audit Trail
                ///////////////////////////////////////////////////////////
               
                //New Record
                var Entity = entity;
                var allRecords = new List<string>();

                foreach (var item in columns)
                {
                    var propertyInfo = entity.GetType().GetProperty(item);
                    var dValue = propertyInfo.GetValue(entity, null);

                    allRecords.Add(item + "=" + dValue + " || ");
                }


                //Existing Record
                var existingEntity = existingRecord.FirstOrDefault();
                var existingRecords = new List<string>();

                foreach (var item in columns)
                {
                    var existpropertyInfo = entity.GetType().GetProperty(item);
                    var eValue = existpropertyInfo.GetValue(existingEntity, null);

                    existingRecords.Add(item + "=" + eValue + " || ");
                }



                var audit = new AuditLog
                {
                    UserId = entity.CreatedBy,
                    ModuleName = _tableName,
                    ModuleAction = "Update" + _tableName,
                    Description = "A record was updated",
                    Record = string.Join(" ", allRecords),
                    OldRecord = string.Join(" ", existingRecords),
                    ActionType = ActionType.Update.ToString(),
                    CreatedBy = entity.CreatedBy,
                    CreatedDate = entity.CreatedDate
                };


               
                var auditCol = CustomColumns(audit);
                var auditColumns = string.Join(", ", auditCol);
                var auditOfParameters = string.Join(", ", auditCol.Select(e => "@" + e));
                var auditQuery = $"insert into AuditLog ({auditColumns}) values ({auditOfParameters})";
                var auditResponse = await conn.ExecuteAsync(auditQuery, audit);

            }
        }
        public async Task<IEnumerable<T>> Query(string where = null)
        {
            var query = $"select * from {_tableName} ";

            if (!string.IsNullOrWhiteSpace(where))
                query += where;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>(query);
                return data;
            }
        }

        private IEnumerable<string> GetColumns()
        {
            return typeof(T)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }

        private IEnumerable<string> CustomColumns(AuditLog audit)
        {
            return typeof(AuditLog)
                    .GetProperties()
                    .Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
                    .Select(e => e.Name);
        }

        private IEnumerable<string> GetSpecificColumns()
        {
            return typeof(T)
                    .GetProperties()
                    .Where(e => e.Name != "Id")
                    .Select(e => e.Name);
        }


      

        public async Task<T> DetailsGenerator(T entity, int Id)
        {
            var query = $"select * from {entity.GetType().Name} where Id = {Id}  ";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var data = await conn.QueryAsync<T>(query);
                return data.FirstOrDefault();
            }
        }
    }
}