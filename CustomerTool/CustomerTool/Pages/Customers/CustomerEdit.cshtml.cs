using CustomerTool.Ef;
using CustomerTool.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CustomerTool.Pages.Customers
{
    public class CustomerEditModel : PageModel
    {
        readonly CustomerRepository _customerRepository;
        readonly ILogger<CustomerEditModel> _logger;

        [BindProperty]
        public CustomerView Customer { get; set; }

        [BindProperty]
        public CustomerToUpdate CustomerToUpdate { get; set; } = new CustomerToUpdate();

        public bool ErrorUpdatingCustomer { get; set; }

        public CustomerEditModel(CustomerRepository customerRepository, ILogger<CustomerEditModel> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public void OnGet(string customerId)
        {
            var result = _customerRepository.GetCustomerById(customerId);
            if (!result.IsQuerySuccessful)
            {
                _logger.LogError("Unable to fetch customer by id: {CustomerId}. Error: {Error}, Exception: {Exception}", customerId, result.ErrorMessage, result.Exception);

                return;
            }

            Customer = result.QueryResult;
            if (Customer is not null)
            {
                CustomerToUpdate.ExistingId = Customer.Id;
                CustomerToUpdate.Gender = Customer.Gender;
                CustomerToUpdate.Address = Customer.Address;
                CustomerToUpdate.Email = Customer.Email;
                CustomerToUpdate.Phone = Customer.Phone;
            }
        }

        public IActionResult OnPost(CustomerToUpdate customerToUpdate)
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

                return Page();
            }

            var updateResult = _customerRepository.UpdateCusomer(customerToUpdate);
            if (!updateResult.IsCommandSuccessful)
            {
                ErrorUpdatingCustomer = true;
                _logger.LogError("Unable to update customer: {CustomerToUpdate}. Error: {Error}. Exception: {Exception}", JsonSerializer.Serialize(customerToUpdate), updateResult.ErrorMessage, updateResult.Exception);

                return Page();
            }

            ErrorUpdatingCustomer = false;
            return RedirectToPage("CustomersList");
        }
    }
}
