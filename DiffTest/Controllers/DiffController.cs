using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using static DiffTest.Controllers.DiffLogics;

namespace DiffTest.Controllers
{
    public class DiffController : ApiController
    {


        /* calling GET + id returns:
         * if left or right were not set: 404 NotFound
         * if left and right are same: 200 OK, "Equals"
         * if different sizes: 200 OK,  "SizeDoNotMatch"
         * if same sizes and different data: 200 OK, "ContentDoNotMatch" + offsets & lengths
         * ---------------------------------
         * GET example v1/diff/1 
         */
        [Route("v1/diff/{id}")]
        public object Get(string id)
        {

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

        /* PUT / id / side 
         * + data in body
         * sets data and returns:
         * if data is not a B64 value: BadRequest
         * if data is null: BadRequest
         * if data with id and side exists, replaces data on id position, returns: OK
         * else: returns OK
         * -----------------------
         * PUT example v1/diff/1/left
         * body {data:"AAAAAA=="}
         */
        [Route("v1/diff/{id}/{sideLR}")]
        public HttpResponseMessage Put(string id, string sideLR, [FromBody]DataInput input)
        {
            try
            {
                byte[] testForB64 = Convert.FromBase64String(input.data);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Not a B64 data");
            }

            if (input.data == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            else if (new[] { "left", "right" }.Contains(sideLR.ToString()))
            {
                AddOrReplaceInDictionary(input, id, sideLR.ToString());
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.Created);

        }

    }
}
