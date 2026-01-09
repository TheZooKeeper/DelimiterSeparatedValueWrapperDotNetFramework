using DelimiterSeparatedValueWrapperFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DelimiterSeparatedValueWrapperFramework
{
    public class DelimiterSeparatedValueFileWrapper
    {
        private readonly string delimitedFileName;
        private readonly char delimiter;
        private string[] fileContent = Array.Empty<string>();
        private readonly Dictionary<string, int> columnLocationPairs = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// Number of data lines in the file (excluding header)
        /// </summary>
        public int LineCount { get; private set; }
        /// <summary>
        /// Number of Columns in the file
        /// </summary>
        public int ColumnCount { get; private set; }
        private readonly SanitizationRegexList sanitizationRegex = new SanitizationRegexList();
        /// <summary>
        /// Initialized a new instance of the <see cref="DelimiterSeparatedValueFileWrapper"/> class.
        /// </summary>
        /// <param name="delimitedFileName">Name of File</param>
        /// <param name="delimiter">delimiter value</param>
        /// <exception cref="ArgumentNullException"></exception>"
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="MalformedLineException">Malformed First line</exception>
        public DelimiterSeparatedValueFileWrapper(string delimitedFileName, char delimiter)
        {
            this.delimitedFileName = delimitedFileName;
            this.delimiter = delimiter;
            InitializeFile();
        }
        /// <summary>
        /// Initialized a new instance of the <see cref="DelimiterSeparatedValueFileWrapper"/> class.
        /// </summary>
        /// <param name="delimitedFileName">Name of File</param>
        /// <param name="delimiter">delimiter value</param>
        /// <param name="sanitizationRegex">List of regex replacements for line sanitization</param>
        /// <exception cref="ArgumentNullException"></exception>"
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="MalformedLineException">Malformed First line</exception>
        public DelimiterSeparatedValueFileWrapper(string delimitedFileName, char delimiter, SanitizationRegexList sanitizationRegex)
        {
            this.delimitedFileName = delimitedFileName;
            this.delimiter = delimiter;
            this.sanitizationRegex = sanitizationRegex;
            InitializeFile();
        }

        private void InitializeFile()
        {
            // Is file valid?
            if (delimitedFileName == null || delimitedFileName == "")
                throw new ArgumentNullException(nameof(delimitedFileName), "The file must have a name");

            if (delimiter == '\0')
                throw new ArgumentNullException(nameof(delimiter), "The delimiter must be a valid character");

            if (!File.Exists(delimitedFileName))
                throw new FileNotFoundException($"The file {delimitedFileName} was not found.");

            fileContent = File.ReadAllLines(delimitedFileName);

            if (fileContent == null || fileContent.Length == 0)
                throw new Exception("The file is empty");

            //first line is header
            LineCount = fileContent.Length - 1;

            if (LineCount == 0)
                return;

            if (fileContent[0] == null || fileContent[0] == "")
                throw new Exception("The header line is empty");

            ColumnCount = CharacterInstanceCount(fileContent[0], delimiter) + 1;

            CheckColumnConsistency(1);

            GenerateColumnList();
        }

        /// <summary>
        /// Gets a specific line from the file.
        /// </summary>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public FileLine GetLine(int lineNumber)
        {
            if (lineNumber > LineCount - 1 || lineNumber < 0)
                throw new IndexOutOfRangeException("Line number outside of range.");

            CheckColumnConsistency(lineNumber);
            return new FileLine(SanitizeLine(fileContent[lineNumber + 1]), delimiter, columnLocationPairs);
        }

        /// <summary>
        /// Gets All Lines from the file.
        /// </summary>
        /// <returns></returns>
        public FileLine[] GetFileContents()
        {
            var lines = new List<FileLine>();
            for (var lineNumber = 1; lineNumber < fileContent.Length; lineNumber++)
            {
                CheckColumnConsistency(lineNumber);
                var line = GetLine(lineNumber);
                lines.Add(line);
            }
            return lines.ToArray();
        }

        /// <summary>
        /// Checks if column exists in the file.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public bool ColumnExists(string columnName) => columnLocationPairs.ContainsKey(columnName);

        /// <summary>
        /// Gets a list of all values from the specified column.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string[] PeekColumnValues(string columnName)
        {
            var values = new List<string>();
            for (var lineNumber = 0; lineNumber < fileContent.Length - 1; lineNumber++)
            {
                var line = GetLine(lineNumber);
                values.Add(line.Value(columnName));
            }
            return values.ToArray();

        }

        /// <summary>
        /// Gets the line number for the first occurrence of the specified column value.
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public int GetFileLineByColumnValue(string columnName, string columnValue)
        {
            for (var lineNumber = 0; lineNumber < fileContent.Length - 1; lineNumber++)
            {
                var line = GetLine(lineNumber);
                if (line.Value(columnName) == columnValue)
                    return lineNumber;
            }
            return -1;
        }
        private void GenerateColumnList()
        {
            var line = SanitizeLine(fileContent[0]).Split(delimiter);
            for (var lineNumber = 0; lineNumber < line.Length; lineNumber++)
            {
                var column = line[lineNumber];
                columnLocationPairs.Add(column, lineNumber);
            }
        }

        private void CheckColumnConsistency(int lineNumber)
        {
            if (ColumnCount != CharacterInstanceCount(SanitizeLine(fileContent[lineNumber] + 1), delimiter) + 1)
                throw new MalformedLineException("The file is malformed. The number of columns is inconsistent.");
        }

        private static int CharacterInstanceCount(string line, char character) => line.Count(c => c == character);
        private string SanitizeLine(string line)
        {
            foreach (var expression in sanitizationRegex.GetRegexList())
            {
                line = expression.regex.Replace(line, expression.replacementString);
            }
            return line;
        }
    }
}