using System;
using System.Collections.Generic;

namespace Assignment3.Q1
{
	// a) Transaction record
	public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

	// b) Transaction processor interface
	public interface ITransactionProcessor
	{
		void Process(Transaction transaction);
	}

	// c) Concrete processors
	public class BankTransferProcessor : ITransactionProcessor
	{
		public void Process(Transaction transaction) =>
			Console.WriteLine($"[BankTransfer] Processed {transaction.Amount:C} for {transaction.Category} on {transaction.Date:d}.");
	}

	public class MobileMoneyProcessor : ITransactionProcessor
	{
		public void Process(Transaction transaction) =>
			Console.WriteLine($"[MobileMoney] Processed {transaction.Amount:C} for {transaction.Category} on {transaction.Date:d}.");
	}

	public class CryptoWalletProcessor : ITransactionProcessor
	{
		public void Process(Transaction transaction) =>
			Console.WriteLine($"[CryptoWallet] Processed {transaction.Amount:C} for {transaction.Category} on {transaction.Date:d}.");
	}

	// d) Base Account
	public class Account
	{
		public string AccountNumber { get; }
		public decimal Balance { get; protected set; }

		public Account(string accountNumber, decimal initialBalance)
		{
			AccountNumber = accountNumber;
			Balance = initialBalance;
		}

		public virtual void ApplyTransaction(Transaction transaction)
		{
			Balance -= transaction.Amount;
		}
	}

	// e) Sealed SavingsAccount
	public sealed class SavingsAccount : Account
	{
		public SavingsAccount(string accountNumber, decimal initialBalance)
			: base(accountNumber, initialBalance) { }

		public override void ApplyTransaction(Transaction transaction)
		{
			if (transaction.Amount > Balance)
			{
				Console.WriteLine("Insufficient funds");
				return;
			}

			base.ApplyTransaction(transaction);
			Console.WriteLine($"Applied {transaction.Amount:C}. New balance: {Balance:C}");
		}
	}

	// f) FinanceApp
	public class FinanceApp
	{
		private readonly List<Transaction> _transactions = new();

		public void Run()
		{
			var account = new SavingsAccount("SA-001", 1000m);
			Console.WriteLine($"SavingsAccount {account.AccountNumber} opened with balance {account.Balance:C}\n");

			var t1 = new Transaction(1, DateTime.Now, 120.50m, "Groceries");
			var t2 = new Transaction(2, DateTime.Now, 75m, "Utilities");
			var t3 = new Transaction(3, DateTime.Now, 400m, "Entertainment");

			ITransactionProcessor mobile = new MobileMoneyProcessor();
			ITransactionProcessor bank = new BankTransferProcessor();
			ITransactionProcessor crypto = new CryptoWalletProcessor();

			mobile.Process(t1);
			account.ApplyTransaction(t1);

			bank.Process(t2);
			account.ApplyTransaction(t2);

			crypto.Process(t3);
			account.ApplyTransaction(t3);

			_transactions.AddRange(new[] { t1, t2, t3 });

			Console.WriteLine("\nAll transactions recorded.");
			Console.WriteLine($"Final Balance: {account.Balance:C}");
		}
	}
}
