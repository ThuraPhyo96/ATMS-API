using ATMS.Web.API.AppServices.Dtos;
using ATMS.Web.API.Data;
using ATMS.Web.API.Helpers;
using ATMS.Web.API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATMS.Web.API.AppServices
{
    public class ATMCardAppService : IATMCardAppService
    {
        protected readonly ATMContext _context;
        protected readonly IMapper _mapper;

        public ATMCardAppService(ATMContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Card Login Validation
        public async Task<BankAccountDto> CardLogin(CheckBankCardDto input)
        {
            var isValid = await _context.BankCards
                  .AsNoTracking()
                  .AnyAsync(x =>
                  x.BankCardNumber == input.BankCardNumber &&
                  x.PIN == input.PIN);

            if (!isValid)
                return new BankAccountDto(StatusCodes.Status404NotFound, StatusMessage.InvalidCard, input.BankCardNumber, balance: 0m);

            var card = await _context.BankCards.FirstOrDefaultAsync(x => x.BankCardNumber == input.BankCardNumber);
            var account = await _context.BankAccounts.FirstOrDefaultAsync(x => x.BankAccountId == card.BankAccountId);
            if (account == null)
                return new BankAccountDto(StatusCodes.Status404NotFound, StatusMessage.InvalidCard, input.BankCardNumber, balance: 0m);

            return new BankAccountDto(StatusCodes.Status200OK, string.Empty, input.BankCardNumber, balance: account.Balance);
        }
        #endregion

        #region Withdraw/Deposit
        public async Task<BankAccountDto> UpdateBalance(UpdateBalanceByCustomerDto input)
        {
            var isValidCard = await _context.BankCards
                 .AsNoTracking()
                 .AnyAsync(x => x.BankCardNumber == input.BankCardNumber);

            if (!isValidCard)
                return new BankAccountDto(StatusCodes.Status404NotFound, StatusMessage.InvalidCard, input.BankCardNumber, balance: 0m);

            var card = await _context.BankCards.FirstOrDefaultAsync(x => x.BankCardNumber == input.BankCardNumber);
            var account = await _context.BankAccounts.FirstOrDefaultAsync(x => x.BankAccountId == card.BankAccountId);

            string message = string.Empty;
            if (input.ActionType == (int)EBalanceHistoryType.Withdraw)
            {
                if (input.Amount > account.Balance - 1000)
                    return new BankAccountDto(StatusCodes.Status406NotAcceptable, StatusMessage.NotEnoughAmount, input.BankCardNumber, balance: 0m);

                account.Balance -= input.Amount;
                message = StatusMessage.WithdrawSuccess;
            }
            else if (input.ActionType == (int)EBalanceHistoryType.Deposite)
            {
                account.Balance += input.Amount;
                message = StatusMessage.DepositSuccess;
            }

            CreateBalanceHistoryDto createBalanceHistory = new CreateBalanceHistoryDto(account.CustomerId, account.BankAccountId, input.Amount, input.ActionType);
            var balanceHistory = _mapper.Map<BalanceHistory>(createBalanceHistory);

            await _context.BalanceHistories.AddAsync(balanceHistory);
            _context.BankAccounts.Update(account);

            await _context.SaveChangesAsync();
            return new BankAccountDto(statusCode: StatusCodes.Status200OK, statusMessage: message, input.BankCardNumber, balance: account.Balance);
        }
        #endregion

        #region Get
        public async Task<BalanceHistoryByCardNumberDto> GetAllHistoryByCardNumber(string cardNumber)
        {
            var card = await _context.BankCards.FirstOrDefaultAsync(x => x.BankCardNumber == cardNumber);
            if (card == null)
                return new BalanceHistoryByCardNumberDto(StatusCodes.Status404NotFound, StatusMessage.InvalidCard, new List<BalanceHistoryDto>());

            var account = await _context.BankAccounts.FirstOrDefaultAsync(x => x.BankAccountId == card.BankAccountId);
            if (account == null)
                return new BalanceHistoryByCardNumberDto(StatusCodes.Status404NotFound, StatusMessage.InvalidAccount, new List<BalanceHistoryDto>());

            var objs = _context.BalanceHistories
                .AsNoTracking()
                .Where(x => x.BankAccountId == account.BankAccountId)
                .OrderByDescending(x => x.TransactionDate)
                .AsQueryable();

            var histories = _mapper.Map<List<BalanceHistoryDto>>(objs);

            return new BalanceHistoryByCardNumberDto(StatusCodes.Status200OK, StatusMessage.ValidCard, histories);
        }
        #endregion
    }
}
