using CustomerTool.Ef;
using CustomerTool.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace CustomerTool.Pages.Customers
{
    public class CustomerAddModel : PageModel
    {
        readonly CustomerRepository _customerRepository;

        readonly ILogger<CustomerAddModel> _logger;

        [BindProperty]
        public NewCustomer NewCustomer { get; set; }

        [BindProperty]
        public bool ErrorAddingCustomer { get; set; }

        public CustomerAddModel(CustomerRepository customerRepository, ILogger<CustomerAddModel> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }


        public void OnGet()
        {
        }

        public IActionResult OnPost(NewCustomer newCustomer) 
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

                return Page();
            }

            var addResult = _customerRepository.AddCustomer(newCustomer);
            if (!addResult.IsCommandSuccessful)
            {
                ErrorAddingCustomer = true;
                _logger.LogError("Unable to add new customer: {CustomerToAdd}. Error: {Error}. Exception: {Exception}", JsonSerializer.Serialize(newCustomer), addResult.ErrorMessage, addResult.Exception);

                return Page();
            }

            ErrorAddingCustomer = false;
            return RedirectToPage("CustomersList");
        }    
    }
}
