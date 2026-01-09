using DelimiterSeparatedValueWrapperFramework;
using NUnit.Framework;
using System.IO;

namespace DelimitedSeparatedValueFileWrapperTestSuite
{

    [TestFixture]
    public class PropertiesTests
    {

        string appPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Delimited Files");
        string filePath;
        DelimiterSeparatedValueFileWrapper csv;
        [SetUp]
        public void Setup()
        {
            filePath = Path.Combine(appPath, "Vehicles.csv");
            csv = new DelimiterSeparatedValueFileWrapper(filePath, ',');
        }
        [Test]
        public void RecordCountTest()
        {
            string filePath = Path.Combine(appPath, "Vehicles.csv");
            var csv = new DelimiterSeparatedValueFileWrapper(filePath, ',');
            Assert.That(csv.LineCount, Is.EqualTo(142));
        }
        [Test]
        public void ColumnCountTest()
        {
            string filePath = Path.Combine(appPath, "Vehicles.csv");
            var csv = new DelimiterSeparatedValueFileWrapper(filePath, ',');
            Assert.That(csv.ColumnCount, Is.EqualTo(13));
        }
    }
}