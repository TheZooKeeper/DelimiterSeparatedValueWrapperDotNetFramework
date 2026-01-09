using DelimiterSeparatedValueWrapperFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace DelimitedSeparatedValueFileWrapperTestSuite
{
    public class LineTests
    {
        string appPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Delimited Files");
        private Dictionary<string, int> columnLocationPairs = new Dictionary<string, int>();
        string filePath;
        DelimiterSeparatedValueFileWrapper csv;
        private SanitizationRegexList normalRegex;
        private FileLine line;

        [SetUp]
        public void Setup()
        {
            filePath = Path.Combine(appPath, "Vehicles.csv");
            csv = new DelimiterSeparatedValueFileWrapper(filePath, ',');

            normalRegex = new SanitizationRegexList();
            normalRegex.AddRegex(new Regex(@"""*\$([0-9]*),*([0-9.]*) USD""*"), "$1$2");

            line = csv.GetLine(1);
        }

        [Test]
        public void TestGetColumnValue()
        {
            Assert.That(line.Value("Lot/inv #"), Is.EqualTo("91053555"));
        }

        [Test]
        public void TestGetInvalidColumnValue()
        {
            Assert.Throws<ArgumentException>(() => line.Value("InvalidColumnName"));
        }

        [Test]
        public void TestColumnCaseValue()
        {
            Assert.That(line.Value("lot/inv #"), Is.EqualTo("91053555"));
        }
    }
}