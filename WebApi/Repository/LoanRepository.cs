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
                .Select(s =>
                new LoanVM()
                {
                    Amount = s.Amount,
                    RateOfinterst = s.RateOfinterst,
                    LoanNumber = s.LoanNumber,
                    LoanStatus = s.Status,
                    LoanType = s.LoanType,
                    LoanTerm = s.LoanTerm,
                    FirstName = s.Customer.FirstName,
                    LastName = s.Customer.LastName,
                    CustomerPhone = s.Customer.PhoneNumber,
                    CustomerAddress = s.Customer.Address
                })
                .FirstOrDefaultAsync(x => x.LoanNumber == loanNumber);
            data.LoanHistory = await LoanHistoryDetails(data.LoanNumber);
            data.LoanDocuments = await LoanDocumentDetails(Convert.ToInt32(data.LoanNumber));
            data.LoanTypeName = data.LoanType > 0 ? LoantypeVM.GetLoanTypes().FirstOrDefault(l => l.Id == data.LoanType).Name : "";
            return data;
        }
        public async Task<List<LoanHistoryVM>> LoanHistoryDetails(string loanNumber)
        {
            List<LoanHistoryVM> list = new List<LoanHistoryVM>();
            var data = await _databaseDbContext.LoanHistory.Where(x => x.LoanNumber == loanNumber).ToListAsync();
            data.ForEach(x =>
            {
                list.Add(new LoanHistoryVM() { TranscationId = x.TranscationId, Id = x.Id, EMIAmount = x.EMIAmount.ToString(), EMIDueDate = x.EMIDueDate, EMIPaidDate = x.EMIPaidDate, LoanNumber = x.LoanNumber, PaymentStatus = x.PaymentStatus });
            });
            return list;
        }
        public async Task<List<LoanDocumentVM>> LoanDocumentDetails(Int32 loanNumber)
        {
            List<LoanDocumentVM> list = new List<LoanDocumentVM>();
            var data = await _databaseDbContext.LoanDocument.Where(x => x.LoanNumber == loanNumber).ToListAsync();
            data.ForEach(x =>
            {
                list.Add(new LoanDocumentVM() { Id = x.Id, Name = x.Name, LoanNumber = x.LoanNumber, Type = x.Type });
            });
            return list;
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
                    FirstName = s.Customer.FirstName,
                    LastName = s.Customer.LastName,
                    CustomerPhone = s.Customer.PhoneNumber,
                    CustomerAddress = s.Customer.Address,
                }).ToListAsync();

            data.ForEach(x =>
            {

                x.LoanTypeName = x.LoanType > 0 ? LoantypeVM.GetLoanTypes().FirstOrDefault(l => l.Id == x.LoanType).Name : "";
            });
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
                Loan loan = new Loan();

                loan.Active = true;
                loan.LoanNumber = GetLastLoanNo();
                loan.CustomerId = model.CustomerId;
                loan.Amount = model.Amount;
                loan.Status = "Pending";
                loan.LoanTerm = model.LoanTerm;
                loan.LoanType = model.LoanType;
                //loan.LoanTypeName = LoantypeVM.GetLoanTypes().FirstOrDefault(l => l.Id == model.LoanType).Name;
                loan.RateOfinterst = model.RateOfinterst;
                loan.ProcessingFee = model.Amount * 0.05m;
                loan.GSTAmount = model.Amount * 0.18m;
                loan.CreatedDate = DateTime.Now.Date;
                loan.CreatedBy = model.UserId;
                _databaseDbContext.Loan.Add(loan);
                finalResult = await _databaseDbContext.SaveChangesAsync();
                if (finalResult > 0)
                {
                    model.LoanId = loan.Id;
                    model.LoanNumber = loan.LoanNumber;
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

                var loanExisits = _databaseDbContext.Loan.FirstOrDefault(x => x.LoanNumber == model.LoanNumber);
                if (loanExisits is not null)
                {
                    loanExisits.Amount = model.Amount;
                    loanExisits.LoanType = model.LoanType;
                    loanExisits.LoanTerm = model.LoanTerm;
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
                    model.CustomerId = loanExisits.CustomerId;
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
