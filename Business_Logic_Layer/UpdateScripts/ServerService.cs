﻿using Business_Logic_Layer.AppConstants;
using Business_Logic_Layer.IUpdateScripts;
using Dapper;
using Data_Access_Layer.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Models.AppConstants;
using Models.DTOs;
using Models.Entities;
using System.Data;
using System.Text;

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
                    StatusId = (int)Enums.Status.NotStarted,
                    AuditItems = new List<AuditItem>()
                };
                try
                {

                    var tennantConnectionString = _databaseSettings.GetTenantConnectionString(tenant);

                    using (var connection = new SqlConnection(tennantConnectionString))
                    {
                        audit.StatusId = (int)Enums.Status.InProgress;

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
                                        StatusId = (int)Enums.Status.InProgress
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

                                    fileAuditToUpdate.ResultMessage = Enums.Result.Success.ToString();
                                }

                                transaction.Commit();

                                //audit.Result = (int)Enums.Result.Success;
                            }
                            catch (Exception ex)
                            {
                                var failedAuditItem = audit.AuditItems.LastOrDefault();
                                failedAuditItem.StatusId = (int)Enums.Status.Failed;
                                failedAuditItem.ResultMessage = ex.Message;
                                
                                transaction.Rollback();

                                audit.RollbackDone = true;

                                await AddExceptionAudit(ex, audit);
                            }
                        }
                    }

                    audit.EndDate = DateTime.Now;
                    audit.StatusId = (int)Enums.Status.Finished;

                    //await AddAudit(audit);
                }
                catch (Exception ex)
                {
                    await AddExceptionAudit(ex, audit);
                }
            }

            return results;
        }

        private async Task AddExceptionAudit(Exception ex, Audit audit)
        {
            audit.StatusId = (int)Enums.Status.Failed;
            audit.EndDate = DateTime.Now;
            audit.AuditItems.LastOrDefault().ResultMessage = ex.Message;

            await AddAudit(audit);
        }

        private async Task AddAudit(Audit audit)
        {
            await _auditRepository.CreateAudit(audit);
        }
    }
}
