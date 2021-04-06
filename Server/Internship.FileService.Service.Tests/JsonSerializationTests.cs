using Xunit;
using System.IO;
using System;
using Internship.FileService.Infrastructure.SerializerFactoryMethod;
using System.Linq;
using System.Collections.Generic;
using Internship.FileService.Infrastructure.FileModels;

namespace Internship.FileService.Infrastructure.Tests
{
    public class JsonSerializationTests
    {
        const string jsonPath = @"C:\Users\ruslan.rudenko\Desktop\12443_9822_11025.json";
        readonly string jsonFileString = "";
        FileSerializer serializer;

        public JsonSerializationTests()
        {
            jsonFileString = File.ReadAllText(jsonPath);
            serializer = null;
        }

        [Fact]
        public void ShouldParseJsonProperly()
        {
            // Arrange
            serializer = new JsonFileSerializer();

            // Act
            var jsonFromFileUsingJsonSerializer =
                serializer.Deserialize<TransactionFileModel>(jsonFileString);

            // Assert
            Assert.True(jsonFromFileUsingJsonSerializer is not null);
        }

        [Fact]
        public void ShouldThrowWhileParsingJson()
        {
            // Arrange
            serializer = new XmlFileSerializer();

            // Act

            // Assert
            Assert.Throws<InvalidOperationException>(() =>
                serializer.Deserialize<TransactionFileModel>(jsonFileString));
        }
    }
}
