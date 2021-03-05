export interface Transaction {
  debtorFirstName: string;
  debtorLastName: string;
  debtorAccountNumber: string;
  debtorBankId: string;
  creditorFirstName: string;
  creditorLastName: string;
  creditorAccountNumber: string;
  creditorBankId: string;
  amount: number;
  transactionId: string;
}
