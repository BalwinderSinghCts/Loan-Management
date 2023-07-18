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

        public async Task<ResponseMessage<LoanVM>> AddLoan(LoanVM model)
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
                }
                return finalResult > 0 ? new ResponseMessage<LoanVM>() { IsSuccess = true, Message = "Data saved successfully" } : null;

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

        public async Task<ResponseMessage<LoanVM>> UpdateLoan(LoanVM model)
        {
            return new ResponseMessage<LoanVM>() { };
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
