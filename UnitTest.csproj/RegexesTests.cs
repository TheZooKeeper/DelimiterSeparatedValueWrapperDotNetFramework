using DelimiterSeparatedValueWrapperFramework;
using DelimiterSeparatedValueWrapperFramework.Exceptions;
using NUnit.Framework;
using System.IO;
using System.Text.RegularExpressions;

namespace DelimitedSeparatedValueFileWrapperTestSuite
{
    public class RegexesTests
    {

        string appPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Delimited Files");
        string filePath;
        DelimiterSeparatedValueFileWrapper csv;
        private SanitizationRegexList singleRegex;
        private SanitizationRegexList multipleRegexes;
        private SanitizationRegexList normalRegex;

        [SetUp]
        public void Setup()
        {
            filePath = Path.Combine(appPath, "Vehicles.csv");
            csv = new DelimiterSeparatedValueFileWrapper(filePath, ',');

            singleRegex = new SanitizationRegexList();
            singleRegex.AddRegex(new Regex(@"\s+"), " ");

            multipleRegexes = new SanitizationRegexList();
            multipleRegexes.AddRegex(new Regex(@"\s+"), " ");
            multipleRegexes.AddRegex(new Regex(@"""*\$([0-9]*),*([0-9.]*) USD""*"), "$1$2");

            normalRegex = new SanitizationRegexList();
            normalRegex.AddRegex(new Regex(@"""*\$([0-9]*),*([0-9.]*) USD""*"), "$1$2");
        }

        [Test]
        public void ValidFileWithSingleRegexTest()
        {
            var csv = new DelimiterSeparatedValueFileWrapper(filePath, ',', singleRegex);
            Assert.That(csv.LineCount, Is.EqualTo(142));
        }
        [Test]
        public void ValidFileWithMultipleRegexesTest()
        {
            var csv = new DelimiterSeparatedValueFileWrapper(filePath, ',', multipleRegexes);
            Assert.That(csv.LineCount, Is.EqualTo(142));
        }
        [Test]
        public void PriceCausesColumnErrorTest()
        {
            var testFile = Path.Combine(appPath, "CommasInPrice.csv");
            Assert.Throws<MalformedLineException>(() => new DelimiterSeparatedValueFileWrapper(testFile, ',', singleRegex));
        }
        [Test]
        public void FixesCommaInPriceTest()
        {
            var testFile = Path.Combine(appPath, "CommasInPrice.csv");
            new DelimiterSeparatedValueFileWrapper(testFile, ',', multipleRegexes);
        }
        [Test]
        public void FixesSingleLineTest()
        {
            var testFile = Path.Combine(appPath, "CommasInPrice.csv");
            var commasFile = new DelimiterSeparatedValueFileWrapper(testFile, ',', normalRegex);
            var line = commasFile.GetLine(2);
            Assert.That(line.Value("Invoice amount"), Is.EqualTo("993.00"));
        }

    }
}