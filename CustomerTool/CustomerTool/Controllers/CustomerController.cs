using CustomerTool.Ef;
using CustomerTool.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CustomerTool.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        readonly CustomerRepository _customerRepository;
        readonly ILogger<CustomerController> _logger;

        public CustomerController(CustomerRepository repository, ILogger<CustomerController> logger)
        {
            _customerRepository = repository;
            _logger = logger;
        }
        
        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok();
        }

        [HttpPost("Add")]
        public IActionResult AddCustomer([FromBody] NewCustomer newCustomer)
        {
            ModelState.Clear();
            var validationState = newCustomer.Validate();

            if (!validationState.IsValid)
            {
                foreach (var vs in validationState)
                {
                    foreach (var error in vs.Value.Errors)
                    {
                        ModelState.AddModelError(vs.Key, error.ErrorMessage);
                    }
                }

                return BadRequest(ModelState);
            }

            var addResult = _customerRepository.AddCustomer(newCustomer);
            if (!addResult.IsCommandSuccessful)
            {
                _logger.LogError("Unable to add new customer: {CustomerToAdd}. Error: {Error}. Exception: {Exception}", JsonSerializer.Serialize(newCustomer), addResult.ErrorMessage, addResult.Exception);

                if (addResult.Error == Models.DbCommandError.InvalidRequest || addResult.Error == Models.DbCommandError.RecordNotFound)
                {
                    return BadRequest();
                }

                return new StatusCodeResult(500);
            }

            return new CreatedResult("/Customer/GetById", new { Id = addResult .Data});
        }

        [HttpGet("GetById")]
        public IActionResult GetCustomer([FromQuery] string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return BadRequest(new { error = "customerId is null/empty/whitespace" });
            }

            if (!Guid.TryParse(customerId, out _))
            {
                return BadRequest(new { error = "Invalid guid passed as customerId" });
            }

            var getResult = _customerRepository.GetCustomerById(customerId);
            if (!getResult.IsQuerySuccessful)
            {
                _logger.LogError("Unable to fetch customer with id: {CustomerId}. Error: {Error}. Exception: {Exception}", customerId, getResult.ErrorMessage, getResult.Exception);

                return new StatusCodeResult(500);
            }

            return Ok(getResult.QueryResult);
        }


        [HttpPut("Edit")]
        public IActionResult Edit([FromBody] CustomerToUpdate customerToUpdate)
        {
            ModelState.Clear();
            var validationState = customerToUpdate.Validate();

            if (!validationState.IsValid)
            {
                foreach (var vs in validationState)
                {
                    foreach (var error in vs.Value.Errors)
                    {
                        ModelState.AddModelError(vs.Key, error.ErrorMessage);
                    }
                }

                return BadRequest(ModelState);
            }

            var updateResult = _customerRepository.UpdateCusomer(customerToUpdate);
            if (!updateResult.IsCommandSuccessful)
            {
                _logger.LogError("Unable to update customer: {CustomerToUpdate}. Error: {Error}. Exception: {Exception}", JsonSerializer.Serialize(customerToUpdate), updateResult.ErrorMessage, updateResult.Exception);

                if (updateResult.Error == Models.DbCommandError.InvalidRequest || updateResult.Error == Models.DbCommandError.RecordNotFound)
                {
                    return BadRequest();
                }

                return new StatusCodeResult(500);
            }

            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return BadRequest(new { error = "customerId is null/empty/whitespace" });
            }

            if (!Guid.TryParse(customerId, out _))
            {
                return BadRequest(new { error = "Invalid guid passed as customerId" });
            }

            var deleteResult = _customerRepository.DeleteCusomer(customerId);
            if (!deleteResult.IsCommandSuccessful)
            {
                _logger.LogError("Unable to delete customer with id: {CustomerId}. Error: {Error}. Exception: {Exception}", customerId, deleteResult.ErrorMessage, deleteResult.Exception);

                return new StatusCodeResult(500);
            }

            return Ok();
        }
    }
}
