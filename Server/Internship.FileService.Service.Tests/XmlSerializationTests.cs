using Xunit;
using System.IO;
using System;
using Internship.FileService.Infrastructure.SerializerFactoryMethod;
using System.Linq;
using System.Collections.Generic;
using Internship.FileService.Infrastructure.FileModels;
using Newtonsoft.Json;

namespace Internship.FileService.Infrastructure.Tests
{
    public class XmlSerializationTests
    {
        const string xmlPath = @"C:\Users\ruslan.rudenko\Desktop\12443_9822_11025.xml";
        readonly string xmlFileString = "";
        FileSerializer serializer;

        public XmlSerializationTests()
        {
            xmlFileString = File.ReadAllText(xmlPath);
            serializer = null;
        }

        [Fact]
        public void ShouldParseXmlProperly()
        {
            // Arrange
            serializer = new XmlFileSerializer();

            // Act

            var xmlFromFileUsingXmlSerializer =
                serializer.Deserialize<TransactionFileModel>(xmlFileString);

            // Assert
            Assert.True(xmlFromFileUsingXmlSerializer is not null);
        }

        [Fact]
        public void ShouldThrowWhileParsingXml()
        {
            // Arrange
            serializer = new JsonFileSerializer();

            // Act

            // Assert
            Assert.Throws<JsonReaderException>(() => 
                serializer.Deserialize<TransactionFileModel>(xmlFileString));
        }
    }
}
