using System;
using System.Net;
using chainsharp.logging;
using Microsoft.AspNetCore.Mvc;

namespace chainsharp.server.Controllers
{
    [Route("api/[controller]")]
    //[ApiController]
    public class ValuesController : Controller
    {
        #region Private Fields

        private readonly GlobalData _globalData;

        #endregion Private Fields

        #region Public Constructors

        public ValuesController(GlobalData globalData)
        {
            _globalData = globalData;
        }

        #endregion Public Constructors

        #region Public Methods

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok(new string[] { "value1", "value2" });
            }
            catch (Exception ex)
            {
                _globalData.Logger.LogError(ex.GetFullExceptionMessage());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok("value");
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] string value)
        {
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok();
        }

        #endregion Public Methods
    }
}
