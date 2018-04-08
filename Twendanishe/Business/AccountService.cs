using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twendanishe.Data;
using Twendanishe.Models;
using Twendanishe.ViewModels;

namespace Twendanishe.Business
{
    /// <summary>
    /// This is the class for handling operations on accounts
    /// </summary>
    public class AccountService
    {
        private readonly TwendanisheContext _context;
        private readonly HashingService _hashingService;

        public AccountService(TwendanisheContext context, HashingService hashingService)
        {
            _context = context;
            _hashingService = hashingService;
        }

        /// <summary>
        /// Process a payment from the user.
        /// </summary>
        /// <param name="payment">Payment item</param>
        /// <returns></returns>
        public async Task<bool> Pay(PaymentViewModel payment)
        {
            try
            {
                var user = _context.Users.Where(p => p.PhoneNumber == payment.PhoneNumber).FirstOrDefault();
                if (user == null) return false;

                var paymentItem = new Payment()
                {
                    UserId = user.Id,
                    Payer = payment.PhoneNumber.ToString(),
                    Amount = payment.Amount,
                    Reference = payment.Reference,
                    StateId = _context.States.Where(state => state.Name == "Completed").FirstOrDefault().Id
                };
                _context.Payments.Add(paymentItem);
                await _context.SaveChangesAsync();

                user.WalletBalance += paymentItem.Amount;

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a user.
        /// </summary>
        /// <param name="userViewModel">User view model</param>
        /// <returns></returns>
        public async Task<bool> Create(UserViewModel userViewModel)
        {
            try
            {
                var user = _context.Users
                    .Where(p => p.PhoneNumber == userViewModel.PhoneNumber 
                    || p.Email == userViewModel.Email).FirstOrDefault();
                if (user != null) return false;

                var newUser = new User()
                {
                    Firstname = userViewModel.Firstname,
                    Lastname = userViewModel.Lastname,
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email,
                    PhoneNumber = userViewModel.PhoneNumber,
                    TypeId = userViewModel.TypeId,
                    DateOfBirth = userViewModel.DateOfBirth,
                    Gender = userViewModel.Gender,
                    PasswordHash = _hashingService.Hash(userViewModel.Password),
                    StateId = _context.States.Where(state => state.Name == "Active").FirstOrDefault().Id
                };
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="loginViewModel">
        /// The login view model with the username and password for the user.
        /// The username could be email, username or phone number
        /// </param>
        /// <returns></returns>
        public async Task<bool> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var user = await _context.Users
                    .Where(p => p.PhoneNumber.ToString() == loginViewModel.UserName
                    || p.Email == loginViewModel.UserName || p.UserName == loginViewModel.UserName)
                    .FirstOrDefaultAsync();
                if (user == null) return false;

                if (_hashingService.Validate(user.PasswordHash, loginViewModel.Password))
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
