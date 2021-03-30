using System;
using System.Threading.Tasks;
using Dapper;
using Internship.FileService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Internship.FileService.Infrastructure.DAL
{
    public class FileRepository : IFileRepository
    {
        private readonly IConfiguration _configuration;
        private const string _connectionString = "MYSQLConnection";

        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> Add(DateTime date, bool type, string filename, byte[] file)
        {
            const string sqlExpressionToInsert = "INSERT INTO `fileservice_db`.`files`"+
                                                 "(`date`, `incoming`, `filename`, `file`) VALUES (@Date, @Type, @FileName, @File);";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString(_connectionString));
            connection.Open();
            
            var inserted = await connection.ExecuteAsync(sqlExpressionToInsert, new
            {
                Date = date,
                Type = type,
                FileName = filename,
                File = file
            });

            return inserted;
        }

        public async Task<int> GetNextPrimaryKey()
        {
            const string sqlExpressionToInsert = @"SELECT MAX(fileid) + 1 as NextId FROM fileservice_db.files;";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString(_connectionString));
            connection.Open();

            int nextId = await connection.QueryFirstOrDefaultAsync<int>(sqlExpressionToInsert);

            return nextId;
        }
    }
}