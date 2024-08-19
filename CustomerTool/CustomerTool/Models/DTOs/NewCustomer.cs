using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.RegularExpressions;

namespace CustomerTool.Models.DTOs
{
    public class NewCustomer
    {
        public string Name { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; } = Gender.Undeclared;

        public string Address { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public ModelStateDictionary Validate()
        {
            var result = new ModelStateDictionary();

            if (string.IsNullOrWhiteSpace(Name))
            {
                result.AddModelError("NewCustomer.Name", "Name field is required");
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                result.AddModelError("NewCustomer.Address", "Address field is required");
            }

            var minimumAge = DateTime.UtcNow.Date.AddYears(-18);
            if (DateOfBirth == default || DateOfBirth == DateTime.MaxValue || DateOfBirth == DateTime.MinValue || DateOfBirth >= minimumAge)
            {
                result.AddModelError("NewCustomer.DateOfBirth", "Invalid DateOfBirth.");
            }

            if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Phone))
            {
                result.AddModelError("NewCustomer.Email", "Either email or phone must be provided.");
                result.AddModelError("NewCustomer.Phone", "Either email or phone must be provided.");
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                var regEx = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
                if (!regEx.Match(Email).Success)
                {
                    result.AddModelError("NewCustomer.Email", "Invalid email.");
                }
            }

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                if (!Phone.All(c => char.IsDigit(c)))
                {
                    result.AddModelError("NewCustomer.Phone", "Invalid phone number.");
                }
            }

            return result;
        }
    }
}
