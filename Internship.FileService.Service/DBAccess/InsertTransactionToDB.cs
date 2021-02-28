﻿using System;
using System.Threading.Tasks;
using Internship.FileService.Domain.Models;
using MySql.Data.MySqlClient;

namespace Internship.FileService.Service.DBAccess
{
    public class InsertTransactionToDb
    {
        public async Task Insert(string connectionString, DateTime date, string type, string filename, byte[] file)
        {
            await using var sqlConnection = new MySqlConnection(connectionString);

            var sqlExpressionInsertTransaction = "INSERT INTO `db`.`payments`"+
                                                 "(`date`, `type`, `filename`, `file`) VALUES (@Date, @Type, @FileName, @File);";

            await using var insertTransaction = new MySqlCommand(sqlExpressionInsertTransaction, sqlConnection);
            
            insertTransaction.Parameters.Add("@Date", MySqlDbType.DateTime, 50).Value = date;
            insertTransaction.Parameters.Add("@Type", MySqlDbType.VarChar, 50).Value = type;
            insertTransaction.Parameters.Add("@FileName", MySqlDbType.VarChar, 50).Value = filename;
            insertTransaction.Parameters.Add("@File", MySqlDbType.Blob, 350).Value = file;

            await sqlConnection.OpenAsync();
            await insertTransaction.ExecuteNonQueryAsync();
        }
    }
}