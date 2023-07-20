using Microsoft.EntityFrameworkCore;
using WebApi.DataBaseContext;
using WebApi.Entities;
using WebApi.Models;

namespace WebApi.Repository
{
    public class CustomerRepository : ICustomerService
    {
        private readonly DatabaseDbContext _databaseDbContext = null;

        public CustomerRepository(DatabaseDbContext databaseDbContext)
        {
            _databaseDbContext = databaseDbContext;
        }
        public async Task<CustomerVM> AddCustomer(CustomerVM model)
        {
            try
            {
                Customer customer = new Customer();
                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.PhoneNumber = model.PhoneNumber;
                customer.Address = model.Address;
                customer.PanNo = model.PanNo;
                customer.GSTNo = model.GSTNo;
                customer.CreatedDate = DateTime.Now.Date;
                customer.CreatedBy = Convert.ToInt32(model.UserId);
                customer.Active = true;
                _databaseDbContext.Customer.Add(customer);
                int result = await _databaseDbContext.SaveChangesAsync();
                model.Id = result > 0 ? customer.Id : 0;
                return result > 0 ? model : null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<CustomerVM>> CustomerList()
        {
            try
            {
                List<CustomerVM> customerVMs = new List<CustomerVM>();
                var data = await _databaseDbContext.Customer.Where(x => x.Active).ToListAsync();
                data.ForEach(x =>
                {
                    customerVMs.Add(new CustomerVM() { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName, PhoneNumber = x.PhoneNumber });
                });
                return customerVMs;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<CustomerVM> GetCustomerDetail(int id)
        {
            try
            {
                CustomerVM customerVM = new CustomerVM();
                var data = await _databaseDbContext.Customer.FirstOrDefaultAsync(x => x.Active && x.Id == id);
                customerVM.Id = data.Id;
                customerVM.FirstName = data.FirstName;
                customerVM.LastName = data.LastName;
                customerVM.Active = data.Active;
                customerVM.PhoneNumber = data.PhoneNumber;
                customerVM.Address = data.Address;
                customerVM.Email = data.Email;
                customerVM.PanNo = data.PanNo;
                return customerVM;
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
