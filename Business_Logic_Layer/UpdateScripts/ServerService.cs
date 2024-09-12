using Business_Logic_Layer.AppConstants;
using Business_Logic_Layer.IUpdateScripts;
using Dapper;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Models.AppConstants;
using Models.DTOs;
using Models.Entities;
using SafeScriptDb_BE.AppConstants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.UpdateScripts
{
    public class ServerService : IServerService
    {
        private readonly IDatabaseSettings _databaseSettings;
        private readonly IAuditRepository _logRepository;
        private readonly IAuditRepository _auditRepository;

        public ServerService(IDatabaseSettings databaseSettings, IAuditRepository logRepository, IAuditRepository auditRepository)
        {
            _databaseSettings = databaseSettings;
            _logRepository = logRepository;
            _auditRepository = auditRepository;
        }

        public List<string> GetDatabases(DbServer credentials)
        {
            string sql = "SELECT name FROM sys.databases";

            var mainTenants = new List<string>();
            _databaseSettings.SetConnectionString(credentials.Name, credentials.Username, credentials.Password);

            var connectionString = _databaseSettings.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var multi = connection.QueryMultiple(sql))
                {
                    mainTenants = multi.Read<string>().ToList();
                }
            }

            return mainTenants;
        }

        public async Task<List<string>> ExecuteSqlScripts(List<string> tenantDatabases, List<IFormFile> files)
        {
            List<string> results = new List<string>();

            foreach (var tenant in tenantDatabases)
            {
                var audit = new Audit()
                {
                    StartDate = DateTime.Now,
                    DatabaseName = tenant,
                    Status = (int)Enums.Status.NotStarted,
                    AuditItems = new List<AuditItem>()
                };
                try
                {

                    var connectionString = _databaseSettings.GetConnectionString();

                    using (var connection = new SqlConnection(connectionString))
                    {
                        audit.Status = (int)Enums.Status.InProgress;

                        await connection.OpenAsync();
                        using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            try
                            {
                                foreach (var file in files)
                                {
                                    var fileAudit = new AuditItem
                                    {
                                        ScriptName = file.Name,
                                        Status = (int)Enums.Status.InProgress
                                    };

                                    audit.AuditItems.Add(fileAudit);

                                    string sqlScript;
                                    using (var stream = new MemoryStream())
                                    {
                                        await file.CopyToAsync(stream);
                                        sqlScript = Encoding.UTF8.GetString(stream.ToArray());
                                    }

                                    await connection.ExecuteAsync(sqlScript, transaction: transaction);

                                    var fileAuditToUpdate = audit.AuditItems.LastOrDefault();

                                    fileAuditToUpdate.Result = (int)Enums.Result.Success;
                                    fileAuditToUpdate.ResultMessage = "Success";
                                }

                                transaction.Commit();

                                audit.Result = (int)Enums.Result.Success;
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();

                                audit.RollbackDone = true;

                                //AddExceptionAudit(ex, audit);
                            }
                        }
                    }

                    audit.EndDate = DateTime.Now;
                    audit.Status = (int)Enums.Status.Finished;

                    //await AddAudit(audit);
                }
                catch (Exception ex)
                {
                    AddExceptionAudit(ex, audit);
                }
            }

            return results;
        }

        private async Task AddExceptionAudit(Exception ex, Audit audit)
        {
            audit.Status = (int)Enums.Status.Failed;
            audit.EndDate = DateTime.Now;
            audit.Result = (int)Enums.Result.Failed;
            audit.AuditItems.LastOrDefault().ResultMessage = ex.Message;

            await AddAudit(audit);
        }

        private async Task AddAudit(Audit audit)
        {
            await _auditRepository.CreateAudit(audit);
        }

    }
}
