using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DiffTest.Controllers
{
    public class DiffController : ApiController
    {

        // GET example v1/diff/1
        [Route("v1/diff/{id}")]
        public object Get(string id)
        {
            //TODO Tadej: I can delete this. CHECK IT before!!! changed Lists to Dictionaries
            //DataInput inputToFindLeft = dataInputsLeft.Find(x => x.id == id); 
            //DataInput inputToFindRight = dataInputsRight.Find(x => x.id == id);
            DataInput inputToFindLeft = new DataInput();
            DataInput inputToFindRight = new DataInput();

            if (!dataInputsLeft.TryGetValue(id, out inputToFindLeft) || !dataInputsRight.TryGetValue(id, out inputToFindRight))
                if (inputToFindLeft == null || inputToFindRight == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            return CompareInputs(inputToFindLeft.data, inputToFindRight.data);
        }

        //PUT example v1/diff/1/left
        //body {data:"AAAAAA=="}
        [Route("v1/diff/{id}/{sideLR}")]
        public HttpResponseMessage Put(string id, string sideLR, [FromBody]DataInput input)
        {
            try
            {
                byte[] testForB64 = Convert.FromBase64String(input.data);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (input.data == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else if (new[] { "left", "right" }.Contains(sideLR.ToString()))
            {
                AddOrReplaceInList(input, id, sideLR.ToString());
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.Created);

        }

        public class DataInput
        {
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
