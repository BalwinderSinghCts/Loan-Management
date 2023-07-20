﻿using WebApi.Models;

namespace WebApi.Repository
{
    public interface ICustomerService : IDisposable
    {
        Task<CustomerVM> AddCustomer(CustomerVM model);
        Task<IEnumerable<CustomerVM>> CustomerList();
        Task<CustomerVM> GetCustomerDetail(int id);
    }
}
