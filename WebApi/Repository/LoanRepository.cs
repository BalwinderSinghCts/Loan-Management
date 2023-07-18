using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApi.DataBaseContext;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Repository
{
    public class LoanRepository : ILoanService
    {
        private readonly DatabaseDbContext _databaseDbContext = null;

        public LoanRepository(DatabaseDbContext databaseDbContext)
        {
            _databaseDbContext = databaseDbContext;
        }
        public async Task<LoanVM> LoanDetails(string loanNumber)
        {
            var data = await _databaseDbContext.Loan
                .Include(i => i.Customer)
                .Include(i => i.Documents)
                .Include(i => i.LoanHistory)
                .Select(s =>
                new LoanVM()
                {
                    LoanNumber = s.LoanNumber,
                    LoanStatus = s.Status,
                    LoanType = s.LoanType,
                    LoanTerm = s.LoanTerm,
                    FirstName = s.Customer.FirstName,
                    LastName = s.Customer.LastName,
                    CustomerPhone = s.Customer.PhoneNumber,
                    CustomerAddress = s.Customer.Address,
                })
                .FirstOrDefaultAsync(x => x.LoanNumber == loanNumber);
            return data;
        }

        public async Task<IEnumerable<LoanVM>> SearchLoan(LoanVMFilters loanVMFilters)
        {
            var data = await _databaseDbContext.Loan
                .Include(i => i.Customer)
                .Include(i => i.Documents)
                .Include(i => i.LoanHistory)
                .Select(s =>
                new LoanVM()
                {
                    LoanNumber = s.LoanNumber,
                    LoanStatus = s.Status,
                    LoanType = s.LoanType,
                    LoanTerm = s.LoanTerm,
                    FirstName = s.Customer.FirstName,
                    LastName = s.Customer.LastName,
                    CustomerPhone = s.Customer.PhoneNumber,
                    CustomerAddress = s.Customer.Address,
                }).ToListAsync();

            if (string.IsNullOrEmpty(loanVMFilters.FirstName) && string.IsNullOrEmpty(loanVMFilters.LastName) && string.IsNullOrEmpty(loanVMFilters.LoanNumber))
            {
                return data?.Skip((loanVMFilters.PageNo - 1) * loanVMFilters.PageSize)
            .Take(loanVMFilters.PageSize);
            }
            return data.Where(x => (!string.IsNullOrEmpty(loanVMFilters.FirstName)
            && x.FirstName.ToLower().Contains(loanVMFilters?.FirstName.ToLower()))
            || (!string.IsNullOrEmpty(loanVMFilters.LastName)
            && x.LastName.ToLower().Contains(loanVMFilters?.LastName.ToLower()))
            || (!string.IsNullOrEmpty(loanVMFilters.LoanNumber)
            && x.LoanNumber.Contains(loanVMFilters.LoanNumber)))
            ?.Skip((loanVMFilters.PageNo - 1) * loanVMFilters.PageSize)
            .Take(loanVMFilters.PageSize)
            .ToList();
        }

        public async Task<LoanVM> AddLoan(LoanVM model)
        {
            int finalResult = 0;
            try
            {
                Customer customer = new Customer();
                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.PhoneNumber = model.CustomerPhone;
                customer.Address = model.CustomerAddress;
                customer.PanNo = model.CustomerPanNo;
                customer.GSTNo = model.CustomerGSTNo;
                customer.CreatedDate = DateTime.Now.Date;
                customer.Active = true;
                _databaseDbContext.Customer.Add(customer);
                int result = await _databaseDbContext.SaveChangesAsync();
                Loan loan = new Loan();
                if (result > 0)
                {
                    loan.Active = true;
                    loan.LoanNumber = GetLastLoanNo();
                    loan.CustomerId = customer.Id;
                    loan.Amount = model.Amount;
                    loan.Status = "Pending";
                    loan.LoanTerm = model.LoanTerm;
                    loan.RateOfinterst = model.RateOfinterst;
                    loan.ProcessingFee = model.Amount * 0.05m;
                    loan.GSTAmount = model.Amount * 0.18m;
                    loan.CreatedDate = DateTime.Now.Date;
                    _databaseDbContext.Loan.Add(loan);
                    finalResult = await _databaseDbContext.SaveChangesAsync();
                    if (finalResult > 0)
                    {
                        model.LoanId = loan.Id;
                        model.CustomerId = customer.Id;
                }
                }
                return finalResult > 0 ? model : null;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GetLastLoanNo()
        {
            var lastLoanNumber = _databaseDbContext.Loan.OrderByDescending(x => x.Id).Select(x => x.LoanNumber).FirstOrDefault();
            double newLoanNo = Convert.ToInt32(lastLoanNumber);
            return string.Format("{0:0000000}", ++newLoanNo);
        }

        public async Task<LoanVM> UpdateLoan(LoanVM model)
        {
            int finalResult = 0;
            try
            {
                var customerExisits = await _databaseDbContext.Customer.FirstOrDefaultAsync(x => x.Id == model.CustomerId);
                if (customerExisits is not null)
                {
                    customerExisits.FirstName = model.FirstName;
                    customerExisits.LastName = model.LastName;
                    customerExisits.PhoneNumber = model.CustomerPhone;
                    customerExisits.Address = model.CustomerAddress;
                    customerExisits.PanNo = model.CustomerPanNo;
                    customerExisits.GSTNo = model.CustomerGSTNo;
                    customerExisits.UpdtateBy = model.UserId;
                    customerExisits.UpdtateDate = DateTime.Now.Date;
                }

                _databaseDbContext.Entry(customerExisits).State = EntityState.Modified;
                int result = await _databaseDbContext.SaveChangesAsync();
                if (result > 0)
                {

                    var loanExisits = _databaseDbContext.Loan.FirstOrDefault(x => x.Id == model.LoanId);
                    if (loanExisits is not null)
                    {
                        loanExisits.Amount = model.Amount;
                        loanExisits.Status = model.LoanStatus;
                        loanExisits.LoanTerm = model.LoanTerm;
                        loanExisits.RateOfinterst = model.RateOfinterst;
                        loanExisits.ProcessingFee = model.Amount * 0.05m;
                        loanExisits.GSTAmount = model.Amount * 0.18m;
                        loanExisits.UpdatedDate = DateTime.Now.Date;
                        loanExisits.UpdatedBy = model.UserId;
                        _databaseDbContext.Entry(loanExisits).State = EntityState.Modified;
                        finalResult = await _databaseDbContext.SaveChangesAsync();

                    }

                    if (finalResult > 0)
                    {
                        model.LoanId = loanExisits.Id;
                        model.CustomerId = customerExisits.Id;
                    }
                }
                return finalResult > 0 ? model : null;

            }
            catch (Exception)
        {
                throw;
            }

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
