using System.Collections.ObjectModel;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface ILoanService
    {
        Task<IEnumerable<LoanVM>> SearchLoan(LoanVMFilters loanVMFilters);
        Task<LoanVM> LoanDetails(string loanNumber);
        Task<ResponseMessage<LoanVM>> AddLoan(LoanVM model);
        Task<ResponseMessage<LoanVM>> UpdateLoan(LoanVM model);
    }
}
