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
        



        public async Task<List<Customer>> GetList(FilterCustomer filterCustomer)
        {
            var result = await dDBC.customers.ToListAsync();
            if(filterCustomer.Fullname != null)
            {
                result = result .Where(p => p.Fullname.Contains(filterCustomer.Fullname)).ToList();
            }
            if(filterCustomer.Birthday != null && filterCustomer.Birthday != new DateTime())
            {
                Console.WriteLine(filterCustomer.Birthday);
                result = result.Where(p => p.Birthday == filterCustomer.Birthday).ToList();
            }
            if (filterCustomer.PhoneNumber != null)
            {
                result = result.Where(p => p.PhoneNumber.Contains(filterCustomer.PhoneNumber)).ToList();
            }
            return result;
        }
        private async Task<bool> CheckEmail(string email)
        {
            var result = await dDBC.customers.Where(p => p.Email == email).ToListAsync();
            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> CheckPhoneNumber(string phone)
        {
            var reslut = await dDBC.customers.Where(p => p.PhoneNumber == phone).ToListAsync();
            if (reslut.Count() > 0) { return false; }
            return true;
        }
        public async Task<int> Create(CreateCustomer model)
        {
            try
            {
                var checkemail = await (CheckEmail(model.Email));
                if (checkemail == false)
                {
                    return 1;
                }
                var checkphone = await (CheckPhoneNumber(model.PhoneNumber));
                if (checkphone == false)
                {
                    return 2;
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
                dDBC.customers.AddRange(customer);
                dDBC.SaveChangesAsync();
                return 3;
            } catch (Exception ex)
            {
                return 4;
            }
        }
        async Task<Customer> ICustomerRepon.Createt(CreateCustomer model)
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
                dDBC.customers.AddRange(customer);
                dDBC.SaveChangesAsync();
                return customer;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<bool> CheckEmail(Guid id, string email)
        {
            var result = await dDBC.customers.Where(p => p.Id != id).ToListAsync();
            result = result.Where(p => p.Email == email).ToList();
            if (result.Count() > 0)
            {
                return false;
            }
            return true;
        }
        private async Task<bool> CheckPhoneNumber(Guid id, string phone)
        {
            var reslut = await dDBC.customers.Where(p => p.Id != id).ToListAsync();
            reslut = reslut.Where(p => p.PhoneNumber == phone).ToList();
            if (reslut.Count() > 0) { return false; }
            return true;
        }
        public async Task<int> Update(Guid Id, UpdateCustomer model)
        {
            try
            {
                var checkemail = await (CheckEmail(Id, model.Email));
                if (checkemail == false)
                {
                    return 1;
                }
                var checkphone = await (CheckPhoneNumber(Id, model.Email));
                if (checkphone == false)
                {
                    return 2;
                }
                var customer = await dDBC.customers.FindAsync(Id);

                customer.Fullname = model.Email;
                customer.PhoneNumber = model.PhoneNumber;
                customer.Birthday = model.Birthday;
                customer.Email = model.Email;
                customer.Address = model.Address;
                dDBC.customers.Update(customer);
                dDBC.SaveChangesAsync();
                return 3;
            } catch (Exception ex)
            {
                return 4;
            }
        }
        

        public async Task<int> Delete(Guid Id)
        {
            try
            {
                var customer = await dDBC.customers.FindAsync(Id);
                if (customer == null) return 1;
                dDBC.customers.Remove(customer);
                await dDBC.SaveChangesAsync();
                return 2;
            } catch(Exception ex)
            {
                return 3;
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
            var customers = await dDBC.customers.Where(x => x.PhoneNumber == sdt).ToListAsync();
            if (customers.Count() == 0) return null;
            Customer customer = customers[0];
            return customer;
        }

        
    }
}
