using CustomerTool.Helpers.Parsers;
using CustomerTool.Models;
using CustomerTool.Models.Db;
using CustomerTool.Models.DTOs;
using System.Text.Json;

namespace CustomerTool.Ef
{
    public class CustomerRepository
    {
        readonly CustomerContext _customerContext;
        public CustomerRepository(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        public DbQueryResult<List<CustomerView>> GetCustomers()
        {
            try
            {
                var customers = _customerContext
                                .Customers
                                .Where(c => !c.IsDeleted)
                                .Select(customer => new CustomerView {
                                    Id = customer.Id.ToString(),
                                    Name = customer.Name,
                                    DateOfBirth = customer.DateOfBirth,
                                    Address = customer.Address,
                                    Gender = GenderParser.FromCharToEnum(customer.Gender),
                                    Email = customer.Email,
                                    Phone = customer.Phone,
                                })
                                .ToList();

                return new DbQueryResult<List<CustomerView>>
                {
                    IsQuerySuccessful = true,
                    QueryResult = customers,
                    ErrorMessage = string.Empty,
                    Exception  = null
                };
            }
            catch (Exception ex)
            {
                return new DbQueryResult<List<CustomerView>>
                {
                    IsQuerySuccessful = false,
                    ErrorMessage = $"Exception occured while executing GetCustomers.",
                    Exception = ex,
                    QueryResult = null
                };
            }
        }

        public DbQueryResult<CustomerView> GetCustomerById(string customerId)
        {
            try
            {
                if (!Guid.TryParse(customerId, out Guid id))
                {
                    return new DbQueryResult<CustomerView>
                    {
                        IsQuerySuccessful = false,
                        ErrorMessage = $"Unable to parse id: {customerId}.",
                        QueryResult = null,
                        Exception = null
                    };
                }

                var customer = _customerContext
                                .Customers
                                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);

                if (customer is null)
                {
                    return new DbQueryResult<CustomerView>
                    {
                        IsQuerySuccessful = false,
                        ErrorMessage = $"Customer with id: {customerId} not found in DB.",
                        QueryResult = null,
                        Exception = null
                    };
                }

                return new DbQueryResult<CustomerView>
                {
                    IsQuerySuccessful = true,
                    QueryResult = new CustomerView { 
                        Id = customer.Id.ToString(),
                        Name = customer.Name,
                        DateOfBirth = customer.DateOfBirth,
                        Address = customer.Address,
                        Gender = GenderParser.FromCharToEnum(customer.Gender),
                        Email = customer.Email,
                        Phone = customer.Phone,
                    },
                    ErrorMessage = string.Empty,
                    Exception = null
                };
            }
            catch (Exception ex)
            {
                return new DbQueryResult<CustomerView>
                {
                    IsQuerySuccessful = false,
                    ErrorMessage = $"Exception occured while executing GetCustomerById for id: {customerId}",
                    Exception = ex,
                    QueryResult = null
                };
            }
        }

        public DbCommandResult<Guid> AddCustomer(NewCustomer newCustomer)
        {
            try
            {
                if (newCustomer is null)
                {
                    return new DbCommandResult<Guid>
                    {
                        IsCommandSuccessful = false,
                        ErrorMessage = "New Customer is null.",
                        Data = Guid.Empty,
                        Error = DbCommandError.InvalidRequest
                    };
                }

                var customerToAdd = new Customer
                {
                    Name = newCustomer.Name,
                    DateOfBirth = newCustomer.DateOfBirth,
                    Address = newCustomer.Address,
                    Phone = newCustomer.Phone,
                    Email = newCustomer.Email,
                    Gender = GenderParser.FromEnumToChar(newCustomer.Gender),
                };
                _customerContext.Customers.Add(customerToAdd);

                return new DbCommandResult<Guid>
                {
                    IsCommandSuccessful = true,
                    ErrorMessage = string.Empty,
                    Data = customerToAdd.Id,
                };
            }
            catch (Exception ex)
            {
                return new DbCommandResult<Guid>
                {
                    IsCommandSuccessful = false,
                    ErrorMessage = $"Exception encountered while executing {nameof(AddCustomer)} for new customer: {JsonSerializer.Serialize(newCustomer)}",
                    Exception = ex,
                    Error = DbCommandError.Other
                };
            }
        }

        public DbCommandResult UpdateCusomer(CustomerToUpdate customerToUpdate)
        {
            try
            {
                if (customerToUpdate is null)
                {
                    return new DbCommandResult
                    {
                        IsCommandSuccessful = false,
                        ErrorMessage = "CustomerToUpdate is null.",
                        Error = DbCommandError.InvalidRequest
                    };
                }

                var customerFromDb = _customerContext.Customers.FirstOrDefault(c => c.Id == Guid.Parse(customerToUpdate.ExistingId) && !c.IsDeleted);
                if (customerFromDb is null)
                {
                    return new DbCommandResult
                    {
                        IsCommandSuccessful = false,
                        ErrorMessage = $"Customer with Id:{customerToUpdate.ExistingId} is not found in the DB.",
                        Error = DbCommandError.RecordNotFound
                    };
                }

                customerFromDb.Address = customerToUpdate.Address;
                customerFromDb.Email = customerToUpdate.Email;
                customerFromDb.Phone = customerToUpdate.Phone;
                customerFromDb.Gender = GenderParser.FromEnumToChar(customerToUpdate.Gender);

                return new DbCommandResult
                {
                    IsCommandSuccessful = true,
                    ErrorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new DbCommandResult
                {
                    IsCommandSuccessful = false,
                    ErrorMessage = $"Exception encountered while executing {nameof(UpdateCusomer)} for existing customer: {JsonSerializer.Serialize(customerToUpdate)}",
                    Exception = ex,
                    Error = DbCommandError.Other
                };
            }
        }

        public DbCommandResult DeleteCusomer(string customerId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(customerId))
                {
                    return new DbCommandResult
                    {
                        IsCommandSuccessful = false,
                        ErrorMessage = "Customer Id is empty/null.",
                        Error = DbCommandError.InvalidRequest
                    };
                }

                var id = Guid.Parse(customerId);
                var customerFromDb = _customerContext.Customers.FirstOrDefault(c => c.Id == id && !c.IsDeleted);

                if (customerFromDb is null)
                {
                    return new DbCommandResult
                    {
                        IsCommandSuccessful = false,
                        ErrorMessage = $"Customer with Id:{customerId} is not found in the DB.",
                        Error = DbCommandError.RecordNotFound
                    };
                }

                customerFromDb.IsDeleted = true;

                return new DbCommandResult
                {
                    IsCommandSuccessful = true,
                    ErrorMessage = string.Empty
                };
            }
            catch (Exception ex)
            {
                return new DbCommandResult
                {
                    IsCommandSuccessful = false,
                    ErrorMessage = $"Exception encountered while executing {nameof(DeleteCusomer)} for customer id: {customerId}",
                    Exception = ex,
                    Error = DbCommandError.Other
                };
            }
        }
    }

}
