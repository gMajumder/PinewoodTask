using CustomerTool.Ef;
using CustomerTool.Models.DTOs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerTool.Pages.Customers
{
    public class CustomersListModel : PageModel
    {
        readonly CustomerRepository _customerRepository;
        readonly ILogger<CustomersListModel> _logger;
        public List<CustomerView> Customers;
        public bool ErrorFetchingCustomers { get; set; }

        public CustomersListModel(CustomerRepository customerRepository, ILogger<CustomersListModel> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public void OnGet()
        {
            var result = _customerRepository.GetCustomers();

            if (!result.IsQuerySuccessful)
            {
                ErrorFetchingCustomers = true;
                _logger.LogError("Unable to fetch customers. Error: {Error}, Exception: {Exception}", result.ErrorMessage, result.Exception);

                return;
            }

            ErrorFetchingCustomers = false;
            Customers = result.QueryResult;
        }
    }
}
