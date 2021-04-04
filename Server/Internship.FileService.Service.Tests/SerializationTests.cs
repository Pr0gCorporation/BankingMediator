using Xunit;
using System.IO;
using Internship.FileService.Domain.Models.Transaction;
using System;
using Internship.FileService.Infrastructure.SerializerFactoryMethod;
using System.Linq;
using System.Collections.Generic;

namespace Internship.FileService.Infrastructure.Tests
{
    public class SerializationTests
    {
        const string xmlPath = @"C:\Users\ruslan.rudenko\Desktop\12443_9822_11025.xml";
        const string jsonPath = @"C:\Users\ruslan.rudenko\Desktop\12443_9822_11025.json";
        readonly string xmlFileString = "";
        readonly string jsonFileString = "";
        FileSerializer serializer;

        public SerializationTests()
        {
            xmlFileString = File.ReadAllText(xmlPath);
            jsonFileString = File.ReadAllText(jsonPath);
            serializer = null;
        }

        [Fact]
        public void ShouldParseXmlAndJsonProperly()
        {
            // Arrange
            var xmlTransaction = GetXmlTransaction();
            var jsonTransaction = GetJsonTransaction();

            // Act
            serializer = new XmlFileSerializerMethod();

            var xmlFromFileUsingXmlSerializer =
                serializer.Deserialize<TransactionFileModel>(xmlFileString);

            serializer = new JsonFileSerializerMethod();

            var jsonFromFileUsingJsonSerializer =
                serializer.Deserialize<TransactionFileModel>(jsonFileString);

            // Assert
            Assert.True(
                FileModelsAreEqual(xmlTransaction, xmlFromFileUsingXmlSerializer)
                &&
                FileModelsAreEqual(jsonTransaction, jsonFromFileUsingJsonSerializer));
        }

        private bool FileModelsAreEqual(TransactionFileModel firstFileModel,
            TransactionFileModel secondFileModel)
        {
            return
                firstFileModel.FileId == secondFileModel.FileId &&
                //firstFileModel.Date == secondFileModel.Date &&
                firstFileModel.Transactions.First().Amount == secondFileModel.Transactions.First().Amount &&
                firstFileModel.Transactions.First().EndToEndId == secondFileModel.Transactions.First().EndToEndId &&
                firstFileModel.Transactions.First().Debtor.FirstName == secondFileModel.Transactions.First().Debtor.FirstName &&
                firstFileModel.Transactions.First().Debtor.LastName == secondFileModel.Transactions.First().Debtor.LastName &&
                firstFileModel.Transactions.First().Debtor.AccountNumber == secondFileModel.Transactions.First().Debtor.AccountNumber &&
                firstFileModel.Transactions.First().Debtor.BankId == secondFileModel.Transactions.First().Debtor.BankId &&
                firstFileModel.Transactions.First().Creditor.FirstName == secondFileModel.Transactions.First().Creditor.FirstName &&
                firstFileModel.Transactions.First().Creditor.LastName == secondFileModel.Transactions.First().Creditor.LastName &&
                firstFileModel.Transactions.First().Creditor.AccountNumber == secondFileModel.Transactions.First().Creditor.AccountNumber &&
                firstFileModel.Transactions.First().Creditor.BankId == secondFileModel.Transactions.First().Creditor.BankId;
        }

        private TransactionFileModel GetXmlTransaction()
        {
            return new TransactionFileModel()
            {
                FileId = 15645,
                Date = DateTime.Now,
                Transactions = new List<TransactionToFile>()
                {
                    new TransactionToFile()
                    {
                        Amount = 10,
                        EndToEndId = Guid.Parse("b715b30a-67df-49b9-9953-455b24ebd523"),
                        Debtor = new AccountToFile()
                        {
                            FirstName = "Robert",
                            LastName = "Smith",
                            AccountNumber = "UA213223130000026007233566001",
                            BankId = "3451"
                        },
                        Creditor = new AccountToFile()
                        {
                            FirstName = "John",
                            LastName = "William",
                            AccountNumber = "UA213223130000026007233566200",
                            BankId = "9822"
                        }
                    }
                }
            };
        }

        private TransactionFileModel GetJsonTransaction()
        {
            return new TransactionFileModel()
            {
                FileId = 15645,
                Date = DateTime.Now,
                Transactions = new List<TransactionToFile>()
                {
                    new TransactionToFile()
                    {
                        Amount = (decimal)90.0,
                        EndToEndId = Guid.Parse("82b9fc3f-bb59-47a2-8ec0-d820b21428a2"),
                        Debtor = new AccountToFile()
                        {
                            FirstName = "Lol",
                            LastName = "Kek",
                            AccountNumber = "34245",
                            BankId = "1234"
                        },
                        Creditor = new AccountToFile()
                        {
                            FirstName = "Debil",
                            LastName = "Ya",
                            AccountNumber = "63573",
                            BankId = "4522"
                        }
                    }
                }
            };
        }
    }
}
