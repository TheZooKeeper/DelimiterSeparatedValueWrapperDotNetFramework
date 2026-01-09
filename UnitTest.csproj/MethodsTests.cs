using DelimiterSeparatedValueWrapperFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DelimitedSeparatedValueFileWrapperTestSuite
{
    public class MethodsTests
    {
        string appPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Delimited Files");
        private Dictionary<string, int> columnLocationPairs = new Dictionary<string, int>();
        string filePath;
        DelimiterSeparatedValueFileWrapper csv;
        private SanitizationRegexList normalRegex;

        [SetUp]
        public void Setup()
        {
            filePath = Path.Combine(appPath, "Vehicles.csv");
            csv = new DelimiterSeparatedValueFileWrapper(filePath, ',');

            normalRegex = new SanitizationRegexList();
            normalRegex.AddRegex(new Regex(@"""*\$([0-9]*),*([0-9.]*) USD""*"), "$1$2");
        }

        [TestCase(-1)]
        [TestCase(5000)]
        public void TestGetLineRange(int lineNumber)
        {
            Assert.Throws<IndexOutOfRangeException>(() => csv.GetLine(lineNumber));
        }

        [Test]
        public void TestGetLineNumber()
        {
            var line = csv.GetLine(1);
            Assert.That(line.Value("Lot/inv #"), Is.EqualTo("91053555"));
        }

        [Test]
        public void TestGetLineNumberFromColumnValue()
        {
            var lineNumber = csv.GetFileLineByColumnValue("Lot/inv #", "91053555");
            Assert.That(lineNumber, Is.EqualTo(1));
        }

        public void TestGetAllLines()
        {
            var allLines = csv.GetFileContents();
            Assert.That(allLines.Count, Is.EqualTo(142));
        }

        [Test]
        public void TestPeekColumnValues()
        {
            var allPrices = csv.PeekColumnValues("Lot/inv #");
            Assert.That(allPrices[1], Is.EqualTo("91053555"));
            Assert.That(allPrices[allPrices.Length - 1], Is.EqualTo("96530975"));
        }

    }
}
