namespace music_blog_server.Utility
{
    using System.Globalization;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string RemoveNonEnglishChars(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            string normalizedString = input.Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder();
            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            string cleanedString = Regex.Replace(stringBuilder.ToString(), @"[^\u0000-\u007F]+", string.Empty);

            cleanedString = cleanedString.Trim().Replace(" ", "-").ToLower();

            return cleanedString;
        }
    }

}
