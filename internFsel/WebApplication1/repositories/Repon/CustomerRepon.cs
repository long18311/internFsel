using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.repositories.IRepon;
using WebApplication1.ViewModel.Customer;

namespace WebApplication1.repositories.Repon
{
    public class CustomerRepon : ICustomerRepon
    {
        private readonly DDBC dDBC;
        public CustomerRepon(DDBC dDBC)
        {
            this.dDBC = dDBC;
        }
        



        public async Task<List<Customer>> GetList()
        {
            var result = await dDBC.customers.ToListAsync();
            
            return result;
        }
        public async Task<bool> CheckEmail(string email)
        {
            var result = await dDBC.customers.Where(p => p.Email == email).ToListAsync();
            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CheckPhoneNumber(string phone)
        {
            var reslut = await dDBC.customers.Where(p => p.PhoneNumber == phone).ToListAsync();
            if (reslut.Count() > 0) { return false; }
            return true;
        }
        public async Task<int> Create(Customer customer)
        {
            try
            {
                dDBC.customers.AddRange(customer);
                dDBC.SaveChangesAsync();
                return 1;
            } catch (Exception ex) { return 0; }
        }
        /*public async Task<Customer> Createt(CreateCustomer model)
        {
            try
            {
                var checkemail = await (CheckEmail(model.Email));
                if (checkemail == false)
                {
                    return null;
                }
                var checkphone = await (CheckPhoneNumber(model.PhoneNumber));
                if (checkphone == false)
                {
                    return null;
                }
                Customer customer = new Customer()
                {
                    Id = Guid.NewGuid(),
                    Fullname = model.Fullname,
                    PhoneNumber = model.PhoneNumber,
                    Birthday = model.Birthday,
                    Email = model.Email,
                    Address = model.Address
                };
                try
                {
                    dDBC.customers.AddRange(customer);
                    dDBC.SaveChangesAsync();
                }catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return null;
                }
                return customer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }*/
        public async Task<bool> CheckEmail(Guid id, string email)
        {
            var result = await dDBC.customers.Where(p => p.Id != id).ToListAsync();
            result = result.Where(p => p.Email == email).ToList();
            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> CheckPhoneNumber(Guid id, string phone)
        {
            var reslut = await dDBC.customers.Where(p => p.Id != id).ToListAsync();
            reslut = reslut.Where(p => p.PhoneNumber == phone).ToList();
            if (reslut.Count() > 0) { return false; }
            return true;
        }
        public async Task<int> Update(Customer customer)
        {
            try { 
                dDBC.customers.Update(customer);
                await dDBC.SaveChangesAsync();
                 
                return 1;
            } catch (Exception ex)
            {
                return 0;
            }
        }
        

        public async Task<int> Delete(Customer customer)
        {
            try
            {                
                dDBC.customers.Remove(customer);
                await dDBC.SaveChangesAsync();
                return 1;
            } catch(Exception ex)
            {
                return 0;
            }
        }

        public async Task<Customer> GetById(Guid Id)
        {
            var customer = await dDBC.customers.FindAsync(Id);
            if (customer == null) return null;
            return customer;
        }

        public async Task<Customer> GetBySdt(string sdt)
        {
            //var customers = await dDBC.customers.Where(x => x.PhoneNumber == sdt).ToListAsync();
            Customer customer = await dDBC.customers.FirstOrDefaultAsync(x => x.PhoneNumber == sdt);
            return customer;
        }

        
    }
}
