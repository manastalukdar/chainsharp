using System;
using System.Net;
using System.Text;
using chainsharp.logging;
using Microsoft.AspNetCore.Mvc;

namespace chainsharp.server.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        #region Private Fields

        private readonly DataStore _dataStore;
        private readonly GlobalData _globalData;

        #endregion Private Fields

        #region Public Constructors

        public DataController(GlobalData globalData, DataStore dataStore)
        {
            _globalData = globalData;
            _dataStore = dataStore;
        }

        #endregion Public Constructors

        #region Public Methods

        [HttpPut("add/{index}")]
        public IActionResult Add(string index, [FromBody] string value)
        {
            try
            {
                if (_dataStore.Store.TryGet(index, out byte[] extantValue))
                {
                    return Conflict();
                }

                var bytes = GetBytes(value);
                var result = _dataStore.Store.TryAdd(index, bytes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _globalData.Logger.LogError(ex.GetFullExceptionMessage());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("addorupdate/{index}")]
        public IActionResult AddOrUpdate(string index, [FromBody] string value)
        {
            try
            {
                var bytes = GetBytes(value);
                _dataStore.Store.TryAddOrUpdate(index, bytes);
                return Ok();
            }
            catch (Exception ex)
            {
                _globalData.Logger.LogError(ex.GetFullExceptionMessage());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("delete/{index}")]
        public IActionResult Delete(string index)
        {
            try
            {
                _dataStore.Store.TryDelete(index);
                return Ok();
            }
            catch (Exception ex)
            {
                _globalData.Logger.LogError(ex.GetFullExceptionMessage());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("get/{index}")]
        public IActionResult Get(string index)
        {
            try
            {
                if (_dataStore.Store.TryGet(index, out byte[] value))
                {
                    var stringValue = GetString(value);
                    return Ok(stringValue);
                }
                else
                {
                    _globalData.Logger.LogError($"Failed to get value.");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _globalData.Logger.LogError(ex.GetFullExceptionMessage());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("indexes")]
        public IActionResult GetIndexes()
        {
            try
            {
                var result = _dataStore.Store.GetKeys();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _globalData.Logger.LogError(ex.GetFullExceptionMessage());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("exists/{index}")]
        public IActionResult IndexExists(string index)
        {
            try
            {
                var result = _dataStore.Store.KeyExists(index);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _globalData.Logger.LogError(ex.GetFullExceptionMessage());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("update/{index}")]
        public IActionResult Update(string index, [FromBody] string value)
        {
            try
            {
                if (!_dataStore.Store.KeyExists(index))
                {
                    return NotFound();
                }

                var bytes = GetBytes(value);
                var result = _dataStore.Store.TryUpdate(index, bytes);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _globalData.Logger.LogError(ex.GetFullExceptionMessage());
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        private string GetString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion Private Methods
    }
}
