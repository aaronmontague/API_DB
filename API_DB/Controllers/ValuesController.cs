using System.Collections.Generic;
using System.Web.Http;
using API_DB.Classes;
using Newtonsoft.Json;

namespace API_DB.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            generalDB newGetAll = new generalDB();
            //Apparently the List + return gives us valid JSON
            //TODO need error handling
            return Ok(newGetAll.getAllPersons());
        }

        // GET api/values/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            generalDB newGetSinglePerson = new generalDB();
            Person returnPerson = newGetSinglePerson.getSinglePersonById(id);
            //500
            if (returnPerson == null)
            {
                return InternalServerError();
            }

            //404
            else if (returnPerson.firstName == "notFound" && returnPerson.lastName == "notFound")
            {
                return NotFound();
            }

            //200
            else
            {
                return Ok(returnPerson);
            }
            
        }

        // POST api/values
        // POST {"firstName": "Mary", "lastName": "Douglas" } from Postman
        public IHttpActionResult Post([FromBody]Person newPerson)
        {
            //write to DB
            generalDB newInsert = new generalDB();
            if (newInsert.insertPerson(newPerson))
            {
                //if write is successful 
                string postResult = "Created: " + newPerson.firstName + " " + newPerson.lastName;
                return Created(postResult, postResult);
            }
            else
            {
                //if write is unsuccessful 
                return InternalServerError(); 
            }

        }

        // PUT api/values/5
        public IHttpActionResult Put(int id, [FromBody]Person personToUpdate)
        {
            int resultType;
            //update if ID already exists, create if it does not
            generalDB newUpdate = new generalDB();
            resultType = newUpdate.updatePerson(id, personToUpdate);

            if (resultType == 200)
            {
                return Ok("Id: " + id + " Was Updated");
            }
            else if (resultType == 500)
            {
                return InternalServerError();
            }
            else
            {
                return Created("201", "Id:" + id + " has been created");
            }
            
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
