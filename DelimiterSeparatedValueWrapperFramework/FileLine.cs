using System;
using System.Collections.Generic;

namespace DelimiterSeparatedValueWrapperFramework
{
    public class FileLine
    {
        private readonly string[] values;
        private readonly Dictionary<string, int> columnLocationPairs = new Dictionary<string, int>();
        internal FileLine(string lineContent, char delimiter, Dictionary<string, int> columnLocationPairs)
        {
            values = lineContent.Split(delimiter);
            this.columnLocationPairs = columnLocationPairs;
        }

        /// <summary>
        /// Gets The value for the specified column name
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string Value(string columnName)
        {
            if (!columnLocationPairs.TryGetValue(columnName, out var columnNumber))
                throw new ArgumentException($"The column name {columnName} does not exist in the file.");

            return values[columnNumber];
        }
    }
}