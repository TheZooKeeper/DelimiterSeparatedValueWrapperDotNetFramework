using DelimiterSeparatedValueWrapperFramework;
using DelimiterSeparatedValueWrapperFramework.Exceptions;
using NUnit.Framework;
using System;
using System.IO;

namespace DelimitedSeparatedValueFileWrapperTestSuite
{
    [TestFixture]
    public class FileInitializationTests
    {
        const string FileLocation = "Delimited Files";
        string appPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Delimited Files");
        [Test]
        public void FileDoesNotExistTest()
        {
            string filePath = Path.Combine(FileLocation, "NonExistentFile.csv");
            Assert.Throws<FileNotFoundException>(() => new DelimiterSeparatedValueFileWrapper(filePath, ','));
        }
        [Test]
        public void BadNameTest()
        {
            string filePath = "";
            Assert.Throws<ArgumentNullException>(() => new DelimiterSeparatedValueFileWrapper(filePath, ','));

            filePath = null;
            Assert.Throws<ArgumentNullException>(() => new DelimiterSeparatedValueFileWrapper(filePath, ','));
        }
        [Test]
        public void EmptyFileTest()
        {
            string filePath = Path.Combine(appPath, "Empty.csv");
            Assert.Throws<Exception>(() => new DelimiterSeparatedValueFileWrapper(filePath, ','));
        }
        [Test]
        public void NoRecordsTest()
        {
            string filePath = Path.Combine(appPath, "NoRecords.csv");
            var csv = new DelimiterSeparatedValueFileWrapper(filePath, ',');
            Assert.That(csv.LineCount, Is.EqualTo(0));
        }
        [Test]
        public void MalformedLineInFirstRowTest()
        {
            string filePath = Path.Combine(appPath, "MalformedFirstLine.csv");
            Assert.Throws<MalformedLineException>(() => new DelimiterSeparatedValueFileWrapper(filePath, ','));
        }
        [Test]
        public void AlternateDelimiterTest()
        {
            string filePath = Path.Combine(appPath, "vehicles.tsv");
            new DelimiterSeparatedValueFileWrapper(filePath, ',');
        }
        [Test]
        public void RegularFileTest()
        {
            string filePath = Path.Combine(appPath, "vehicles.csv");
            new DelimiterSeparatedValueFileWrapper(filePath, ',');
        }
    }
}