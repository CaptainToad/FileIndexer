using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileIndexer.Services.Tests
{
    [TestClass()]
    public class StringTokeniserTests
    {
        private StringTokenizer _tokeniser = new StringTokenizer();

        [TestInitialize()]
        public void TestInitialize()
        {
        }

        [TestMethod()]
        [DataRow("[This][bit]should not be tokenised and this bit[should][be][a][list][of][strings].mp4", "[", "]", "this", "bit", "should", "be", "a", "list", "of", "strings")]
        [DataRow("(This)(bit) should not be tokenised and this bit(should)(be)(a)(list)(of)(strings).mp4", "(", ")", "this", "bit", "should", "be", "a", "list", "of", "strings")]
        [DataRow("[This]bit]should not be tokenised and this bit [should][be[a][list][of][strings].mp4", "[", "]", "this", "bit", "should", "be", "a", "list", "of", "strings")]
        [DataRow("[This[bit]should not be tokenised and this bit [should][be[a][list][of][strings].mp4", "[", "]", "this", "bit", "should", "be", "a", "list", "of", "strings")]
        [DataRow("[This[bit]should not be tokenised and this bit [should][be[a][list][of][strings]].mp4", "[", "]", "this", "bit", "should", "be", "a", "list", "of", "strings")]
        [DataRow("[This[bit]should not be tokenised and this bit [should][be[a][list][of][strings]]].mp4", "[", "]", "this", "bit", "should", "be", "a", "list", "of", "strings")]
        public void Tokenize_FilenameHasBracketedSingleTokens_ReturnsListOfStrings(string filename, string startChar, string endChar, params string[] tokenList)
        {
            var tokens = _tokeniser.GetSingleTokens(filename, startChar, endChar);

            Assert.IsNotNull(tokens);
            Assert.AreEqual(tokens.Count(), tokenList.Length);
            Assert.IsTrue(tokenList.SequenceEqual(tokens));
        }

        [TestMethod()]
        [DataRow("[This][bit]should not be tokenised and this bit (should be a list of strings).mp4", "(", ")", "should", "be", "a", "list", "of", "strings")]
        public void Tokenize_FilenameHasParenthesizedMultipleTokens_ReturnsListOfStrings(string filename, string startChar, string endChar, params string[] tokenList)
        {
            var tokens = _tokeniser.GetMultipleTokens(filename, startChar, endChar);

            Assert.IsNotNull(tokens);
            Assert.AreEqual(tokens.Count(), tokenList.Length);
            Assert.IsTrue(tokenList.SequenceEqual(tokens));
        }

        [TestMethod()]
        [DataRow("This bit should not be tokenised and this bit should be a list of strings.mp4", "[", "]")]
        [DataRow("[This][bit]should not be tokenised and this bit[should][be][a][list][of][strings].mp4", "(", ")")]
        [DataRow("This bit should not be tokenised and ((this bit should be a list of strings.mp4", "(", ")")]
        [DataRow("This bit should not be tokenised and this bit should be a list of strings)).mp4", "(", ")")]
        public void Tokenize_FilenameHasNoTokens_ReturnsEmptyListOfStrings(string filename, string startChar, string endChar)
        {
            var singleTokens = _tokeniser.GetSingleTokens(filename, startChar, endChar);
            var multipleTokens = _tokeniser.GetMultipleTokens(filename, startChar, endChar);

            Assert.IsNotNull(singleTokens);
            Assert.AreEqual(singleTokens.Count(), 0);
            Assert.IsFalse(singleTokens.Any());

            Assert.IsNotNull(multipleTokens);
            Assert.AreEqual(multipleTokens.Count(), 0);
            Assert.IsFalse(multipleTokens.Any());
        }
    }
}