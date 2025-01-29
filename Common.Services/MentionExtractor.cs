using System.Text.RegularExpressions;

namespace Foodie.Common.Services
{
    public class MentionExtractor
    {
        private static readonly Regex MentionRegex = new(@"@(\w+)", RegexOptions.Compiled);

        public static List<string> ExtractMentions(string content)
        {
            return MentionRegex.Matches(content)
                .Select(match => match.Groups[1].Value) // Extract usernames
                .Distinct()
                .ToList();
        }
    }
}
