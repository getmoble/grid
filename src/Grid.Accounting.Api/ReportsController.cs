using System;
using System.Globalization;
using System.Web.Mvc;
using Grid.Api.Infrastructure;

namespace Grid.Accounting.Api
{
    public partial class ReportsController : BaseApiController
    {
        readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet]
        public virtual ActionResult GeneralLedger(long Id, string FromDate, string ToDate)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               DateTime fromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               var accounts = _reportsService.GetAllAccountHead(WebUser.TenantId);
               var generalLedger = _reportsService.GetAllGeneralLedgers(WebUser.TenantId, Id, fromDate, toDate);
               var vm = new { Accounts = accounts, GeneralLedgers = generalLedger };
               return vm;
           }));
        }
        [HttpGet]
        public virtual ActionResult FilterdGeneralLedger(long Id, string FromDate, string ToDate)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                DateTime fromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var generalLedger = _reportsService.GetAllGeneralLedgers(WebUser.TenantId, Id, fromDate, toDate);
                return generalLedger;
            }));
        }
        [HttpGet]
        public virtual ActionResult GeneralLedgerDetails(long id)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                var ledgerDetails = _reportsService.GetGeneralLedgerDetails(id);
                var details = ledgerDetails.Select(t => new
                {
                    Date = t.Transaction.DateOfTransaction,
                    Title = t.Transaction.Title + "-" + t.Transaction.Id.ToString(CultureInfo.InvariantCulture),
                    Description = t.Transaction.Description,
                    Debit = t.Debit,
                    Credit = t.Credit

                });
                var vm = new { Details = details };
                return vm;
            }));
        }

        [HttpGet]
        public virtual ActionResult TrialBalance(string FromDate, string ToDate)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                DateTime fromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var trial = _reportsService.GetTrialBalance(fromDate, toDate, WebUser.TenantId);
                var trialBalance = trial.Select(t => new
                    {
                        AccountId = t.AccountId,
                        AccountCategory = t.AccountCategory,
                        AccountName = t.AccountName,
                        Amount = t.Debit - t.Credit,
                    }).ToList();
                var vm = new { TrialBalance = trialBalance };
                return vm;
            }));
        }
        [HttpGet]
        public virtual ActionResult IncomeAndExpense(string FromDate, string ToDate)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                DateTime fromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var incomeAndExpense = _reportsService.GetIncomeAndExpense(fromDate, toDate, WebUser.TenantId);
                var incomeHeads = incomeAndExpense.Where(i => i.AccountCategory == "Income").ToList();
                var expenseHead = incomeAndExpense.Where(e => e.AccountCategory == "Expense").ToList();
                double incomeSum = incomeHeads.Sum(income => (double?)income.Credit) ?? 0;
                double expenseSum = expenseHead.Sum(expense => (double?)expense.Debit) ?? 0;
                double profitOrLoss = incomeSum - expenseSum;

                var vm = new { Income = incomeHeads, Expense = expenseHead, ProfitOrLoss = profitOrLoss };
                return vm;
            }));
        }
        [HttpGet]
        public virtual ActionResult BalanceSheet(string asOnDate)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
           {
               var toDate = DateTime.ParseExact(asOnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
               var incomeAndExpense = _reportsService.GetIncomeAndExpense(DateTime.Parse("01/01/1900"), toDate, WebUser.TenantId);
               var incomeHeads = incomeAndExpense.Where(i => i.AccountCategory == "Income").ToList();
               var expenseHead = incomeAndExpense.Where(e => e.AccountCategory == "Expense").ToList();
               double incomeSum = incomeHeads.Sum(income => (double?)income.Credit) ?? 0;
               double expenseSum = expenseHead.Sum(expense => (double?)expense.Debit) ?? 0;
               double profitOrLoss = incomeSum - expenseSum;
               var balanceSheet = _reportsService.BalanceSheet(toDate, WebUser.TenantId);
               var assetHeads = balanceSheet.Where(i => i.AccountCategory == "Asset").ToList();
               var liabilityHeads = balanceSheet.Where(e => e.AccountCategory == "Liability").ToList();
               var vm = new { Asset = assetHeads, Liability = liabilityHeads, ProfitOrLoss = profitOrLoss };
               return vm;
           }));
        }
        [HttpGet]
        public virtual ActionResult Income(string FromDate, string ToDate)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                DateTime fromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var income = _reportsService.GetIncome(fromDate, toDate, WebUser.TenantId);
                var incomeReport = income.Select(t => new
                {
                    AccountId = t.AccountId,
                    AccountCategory = t.AccountCategory,
                    AccountName = t.AccountName,
                    Credit = t.Credit,
                    Debit = t.Debit,
                }).ToList();
                var vm = new { Income = incomeReport };
                return vm;
            }));
        }
        [HttpGet]
        public virtual ActionResult Expense(string FromDate, string ToDate)
        {
            return ThrowIfNotLoggedIn(() => TryExecute(() =>
            {
                DateTime fromDate = DateTime.ParseExact(FromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime toDate = DateTime.ParseExact(ToDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var expense = _reportsService.GetExpense(fromDate, toDate, WebUser.TenantId);
                var expenseReport = expense.Select(t => new
                {
                    AccountId = t.AccountId,
                    AccountCategory = t.AccountCategory,
                    AccountName = t.AccountName,
                    Credit = t.Credit,
                    Debit = t.Debit
                }).ToList();
                var vm = new { Expense = expenseReport };
                return vm;
            }));
        }

    }
}