export class Transaction {
  constructor(
    public debtorFirstName: string,
    public debtorLastName: string,
    public debtorAccountNumber: string,
    public debtorBankId: string,
    public creditorFirstName: string,
    public creditorLastName: string,
    public creditorAccountNumber: string,
    public creditorBankId: string,
    public amount: number,
    public transactionId: string
  ) {  }
}
