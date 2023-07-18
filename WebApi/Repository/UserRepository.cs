using Microsoft.EntityFrameworkCore;
using WebApi.DataBaseContext;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Repository
{
    public class UserRepository : IUserService
    {
        private readonly DatabaseDbContext _databaseDbContext = null;

        public UserRepository(DatabaseDbContext databaseDbContext)
        {
            _databaseDbContext = databaseDbContext;
        }
        public User GetUser(string email)
        {
            return _databaseDbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public bool UserExists(string email)
        {
            return _databaseDbContext.Users.Where(x => x.Email == email).Any();
        }

        public void UserExists1(string email)
        {
            var fname = "";
            var lname = "";
            int loanNumber = 0;

            List<Movie> movies = new List<Movie>();
            movies.Add(new Movie() { FName = "A", LName = "B", LoanNumber = 1 });
            movies.Add(new Movie() { FName = "AA", LName = "BB", LoanNumber = 2 });
            movies.Add(new Movie() { FName = "CC", LName = "DD", LoanNumber = 3 });
            movies.Add(new Movie() { FName = "CC1", LName = "DD1", LoanNumber = 4 });
            movies.Add(new Movie() { FName = "CC2", LName = "DD2", LoanNumber = 5 });
            movies.Add(new Movie() { FName = "CC3", LName = "DD3", LoanNumber = 6 });
            var data = movies
     .Where(t =>
         t.FName.Contains(fname) ||
         t.LName.Contains(lname) ||
           t.LoanNumber == loanNumber).ToList();

        }


    }
}
