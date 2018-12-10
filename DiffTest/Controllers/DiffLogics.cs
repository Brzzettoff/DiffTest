using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiffTest.Controllers
{
    public class DiffLogics
    {
        public class DataInput
        {
            //TODO Tadej: I can delete this one. CHECK IT before!!! changed Lists to Dictionaries
            //public string id;
            public string data;
        }

        public static Dictionary<string, DataInput> dataInputsLeft = new Dictionary<string, DataInput>();
        public static Dictionary<string, DataInput> dataInputsRight = new Dictionary<string, DataInput>();

        public static void AddOrReplaceInList(DataInput input, string id, string side)
        {

            Dictionary<string, DataInput> currList = (side == "left") ? dataInputsLeft : dataInputsRight;
            {
                if (currList.ContainsKey(id))
                {
                    currList[id] = input;
                }
                else
                {
                    currList.Add(id, input);
                }

            }
        }


        public class DiffResult
        {
            public int offset;
            public int length;

        }

        public class ReturnResult
        {
            public string diffResultType;
            public List<DiffResult> diffs;

            public bool ShouldSerializediffs()
            {
                return (diffs != null);
            }

        }


        public static ReturnResult CompareInputs(string inputLeft, string inputRight)
        {

            ReturnResult returnRes = new ReturnResult();
            if (inputLeft.Equals(inputRight))
            {
                returnRes.diffResultType = "Equals";
                return returnRes;
            }
            else if (inputLeft.Length != inputRight.Length)
            {
                returnRes.diffResultType = "SizeDoNotMatch";
                return returnRes;
            }
            else
            {
                List<DiffResult> diffResults = new List<DiffResult>();

                char[] charLeft = inputLeft.ToCharArray();
                char[] charRight = inputRight.ToCharArray();

                int offset = 0;
                int length = 0;

                for (int i = 0; i < charLeft.Length; i++)
                {
                    if (charLeft[i] == charRight[i])
                    {
                        if (length != 0)
                        {

                            DiffResult diff = new DiffResult
                            {
                                offset = offset,
                                length = length
                            };
                            diffResults.Add(diff);
                        }
                        offset = i + 1;
                        length = 0;
                    }
                    else
                    {
                        length++;
                    }
                }

                returnRes.diffResultType = "ContentDoNotMatch";
                returnRes.diffs = diffResults;
                return returnRes;
            }
        }
    }
}