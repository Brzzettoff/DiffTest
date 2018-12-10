using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static DiffTest.Controllers.DiffLogics;


namespace DiffTest.Tests.Controllers
{
    [TestClass]
    public class DiffLogicsTest
    {
        public DataInput mockData1 = new DataInput
        {
            data = "added"
        };
        public DataInput mockData2 = new DataInput
        {
            data = "replaced"
        };

        [TestMethod]
        public void AddToList()
        {
            AddOrReplaceInDictionary(mockData1, "1", "left");
            dataInputsLeft.TryGetValue("1", out DataInput input);
            Assert.AreEqual("added", input.data);
        }

        [TestMethod]
        public void ReplaceInList()
        {
            AddOrReplaceInDictionary(mockData1, "1", "left");
            AddOrReplaceInDictionary(mockData2, "1", "left");
            dataInputsLeft.TryGetValue("1", out DataInput input);
            Assert.AreEqual("replaced", input.data);
            // Assert.AreEqual("replaced", dataInputsLeft[0].data);
        }

        [TestMethod]
        public void CompareInputsSame()
        {
            ReturnResult returnResult = CompareInputs("AAA", "AAA");

            Assert.AreEqual("Equals", returnResult.diffResultType);
        }

        [TestMethod]
        public void CompareInputsDifferentSize()
        {
            ReturnResult returnResult = CompareInputs("AAAAA", "AAA");
            Assert.AreEqual("SizeDoNotMatch", returnResult.diffResultType);
        }

        [TestMethod]
        public void CompareInputsWithDiff()
        {
            DiffResult result = new DiffResult
            {
                offset = 4,
                length = 2
            };

            ReturnResult returnResult = CompareInputs("AAAAAA==", "AAAABB==");
            Assert.AreEqual("ContentDoNotMatch", returnResult.diffResultType);
            Assert.IsNotNull(returnResult.diffs);
            Assert.AreEqual(result.offset, returnResult.diffs[0].offset);
        }
    }
}
