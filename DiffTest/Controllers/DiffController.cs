using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using static DiffTest.Controllers.DiffLogics;

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

    }
}
