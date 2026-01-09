using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DelimiterSeparatedValueWrapperFramework
{
    /// <summary>
    /// List of all the regular expressions used for Line sanitization
    /// </summary>
    public class SanitizationRegexList
    {
        private List<(Regex regex, string replacementString)> RegexList = new List<(Regex regex, string replacementString)>();
        public SanitizationRegexList()
        { }

        /// <summary>
        /// Adds a regex and its replacement string to the list
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="replacementString"></param>
        public void AddRegex(Regex regex, string replacementString)
        {
            RegexList.Add((regex, replacementString));
        }

        /// <summary>
        /// Adds a list of regex and their replacement strings to the list
        /// </summary>
        /// <param name="regexList"></param>
        public void AddRegex(List<(Regex regex, string replacementString)> regexList)
        {
            RegexList.AddRange(regexList);
        }
        internal List<(Regex regex, string replacementString)> GetRegexList()
        {
            return RegexList;
        }
    }
}