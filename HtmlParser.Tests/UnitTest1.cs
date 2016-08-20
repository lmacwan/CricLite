using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HtmlParser.Tests
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void GetPlayerXML()
        {
            int uniqueId = 36084;
            Parser.GetPlayerXML(uniqueId);
        }
    }
}
