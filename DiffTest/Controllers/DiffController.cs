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

        // GET: api/Diff/5
        public string Get(int id)
        {
            return "value";
        }

        //PUT example v1/diff//1/left
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

    }
}
