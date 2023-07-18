using System.Collections.ObjectModel;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface ILoanService:IDisposable
    {
        Task<IEnumerable<LoanVM>> SearchLoan(LoanVMFilters loanVMFilters);
        Task<LoanVM> LoanDetails(string loanNumber);
        Task<LoanVM> AddLoan(LoanVM model);
        Task<LoanVM> UpdateLoan(LoanVM model);
    }
}
