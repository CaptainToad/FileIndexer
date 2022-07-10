namespace FileIndexer.Services
{
    public interface IStringTokenizer
    {
        public IEnumerable<string> GetSingleTokens(string value, string tokenStartString, string tokenEndString);
        public IEnumerable<string> GetMultipleTokens(string value, string tokenStartString, string tokenEndString);
        public IEnumerable<string> GetAllTokens(string value, DirectoryInfo path);
    }

    public class StringTokenizer : IStringTokenizer
    {
        public IEnumerable<string> GetAllTokens(string filename, DirectoryInfo path)
        {
            var bracketTokens = this.GetSingleTokens(filename, "[", "]");
            var pathTokens = path.FullName.Replace(path.Root.FullName, "").Split(Path.DirectorySeparatorChar);
            IEnumerable<string> parenthesisTokens = null;

            var parensCount = filename.Count(paren => (paren == '('));
            if (parensCount > 1)
            {
                parenthesisTokens = this.GetMultipleTokens(filename, "(", ")");
            }
            else if (parensCount == 1)
            {
                parenthesisTokens = this.GetSingleTokens(filename, "(", ")");
            }

            //Make sure we have something that can be used in the union
            parenthesisTokens = parenthesisTokens ?? new List<string>();

            var allTokens = bracketTokens.Union(parenthesisTokens).Union(pathTokens);

            return allTokens;
        }

        public IEnumerable<string> GetMultipleTokens(string value, string tokenStartString, string tokenEndString)
        {
            //Convert the incoming text to a span that we can slice and dice more easily
            var spanValue = value.ToLowerInvariant().AsSpan();

            //Get last instance of the start token
            var tokenStart = spanValue.LastIndexOf(tokenStartString) + 1;
            //Get last instance of the end token
            var tokenEnd = spanValue.LastIndexOf(tokenEndString);

            if (tokenStart == 0 || tokenEnd == -1)
            {
                return new List<string>();
            }
            //Get the text between the two points
            var tokenSource = spanValue.Slice(tokenStart, (tokenEnd - tokenStart));

            //Split the text 
            var tokensArray = tokenSource.ToString().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var tokens = tokensArray.ToList();

            return tokens;
        }

        public IEnumerable<string> GetSingleTokens(string value, string tokenStartString, string tokenEndString)
        {
            //Convert the incoming text to a span that we can slice and dice more easily
            var spanValue = value.ToLowerInvariant().AsSpan();

            //Get last instance of the start token
            var tokenStart = spanValue.IndexOf(tokenStartString);
            //Get last instance of the end token
            var tokenEnd = spanValue.IndexOf(tokenEndString);

            if (tokenStart == -1 || tokenEnd == -1)
            {
                return new List<string>();
            }

            //Split the text 
            var tokens = new List<string>();
            while (tokenStart < tokenEnd)
            {
                var token = spanValue.Slice(tokenStart + tokenStartString.Length, tokenEnd - (tokenStart + tokenEndString.Length));
                var subTokenStart = token.IndexOf(tokenStartString);
                if (subTokenStart > 0)
                {
                    var subToken = token.Slice(0, (subTokenStart));
                    while (token.IndexOf(tokenStartString) > 0)
                    {
                        tokens.Add(subToken.ToString());

                        if (subTokenStart <= token.Length)
                        {
                            token = token.Slice(subTokenStart + tokenStartString.Length);
                        }
                        else
                        {
                            token = string.Empty;
                        }
                    }
                }

                tokens.Add(token.ToString());

                spanValue = spanValue.Slice(tokenEnd + (tokenEndString.Length));
                tokenStart = spanValue.IndexOf(tokenStartString);
                tokenEnd = spanValue.IndexOf(tokenEndString);

                if (tokenEnd < tokenStart)
                {
                    tokenStart = -1;
                }

                if (tokenStart == -1 && tokenEnd == 0)
                {
                    break;
                }

            }

            return tokens;
        }
    }
}
