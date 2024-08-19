using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CustomerTool.Models.DTOs
{
    public class CustomerToUpdate
    {
        [Required]
        public string ExistingId { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public string Address { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public ModelStateDictionary Validate()
        {
            var result = new ModelStateDictionary();

            if (!Guid.TryParse(ExistingId, out _))
            {
                result.AddModelError("CustomerToUpdate.ExistingId", "Invalid Guid passed as id");
            }

            if (string.IsNullOrWhiteSpace(Address))
            {
                result.AddModelError("CustomerToUpdate.Address", "Address field is required");
            }

            if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(Phone))
            {
                result.AddModelError("CustomerToUpdate.Email", "Either email or phone must be provided.");
                result.AddModelError("CustomerToUpdate.Phone", "Either email or phone must be provided.");
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                var regEx = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
                if (!regEx.Match(Email).Success)
                {
                    result.AddModelError("CustomerToUpdate.Email", "Invalid email.");
                }
            }

            if (!string.IsNullOrWhiteSpace(Phone))
            {
                if (!Phone.All(c => char.IsDigit(c)))
                {
                    result.AddModelError("CustomerToUpdate.Phone", "Invalid phone.");
                }
            }

            return result;
        }
    }
}
