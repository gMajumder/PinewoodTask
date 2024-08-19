using CustomerTool.Ef;
using CustomerTool.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerTool.Pages.Customers
{
    public class CustomerDeleteModel : PageModel
    {
        readonly CustomerRepository _customerRepository;

        readonly ILogger<CustomerDeleteModel> _logger;

        [BindProperty]
        public CustomerView Customer { get; set; }

        public bool ErrorDeletingCustomer { get; set; }

        string _customerId;

        public CustomerDeleteModel(CustomerRepository customerRepository, ILogger<CustomerDeleteModel> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public void OnGet(string customerId)
        {
            _customerId = customerId;
            var result = _customerRepository.GetCustomerById(customerId);
            if (!result.IsQuerySuccessful)
            {
                _logger.LogError("Unable to fetch customer by id: {CustomerId}. Error: {Error}, Exception: {Exception}", customerId, result.ErrorMessage, result.Exception);

                return;
            }

            Customer = result.QueryResult;
        }

        public IActionResult OnPostDelete()
        {
            var deleteResult = _customerRepository.DeleteCusomer(Customer.Id);
            if (!deleteResult.IsCommandSuccessful)
            {
                ErrorDeletingCustomer = true;
                _logger.LogError("Unable to delete customer with id: {CustomerId}. Error: {Error}. Exception: {Exception}", _customerId, deleteResult.ErrorMessage, deleteResult.Exception);

                return Page();
            }

            ErrorDeletingCustomer = false;
            return RedirectToPage("CustomersList");
        }
    }
}
