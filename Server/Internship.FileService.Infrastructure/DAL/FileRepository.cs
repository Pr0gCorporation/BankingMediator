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

        public FileRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> Add(DateTime date, bool type, string filename, byte[] file)
        {
            const string sqlExpressionToInsert = "INSERT INTO `fileservice_db`.`files`"+
                                                 "(`date`, `incoming`, `filename`, `file`) VALUES (@Date, @Type, @FileName, @File);";

            await using var connection = new MySqlConnection(_configuration.GetConnectionString("MYSQLConnection"));
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
    }
}