export class TransactionReadModel {
  public debtorFirstName: string;
    public debtorLastName: string;
    public debtorAccountNumber: string;
    public debtorBankId: string;
    public creditorFirstName: string;
    public creditorLastName: string;
    public creditorAccountNumber: string;
    public creditorBankId: string;
    public amount: number;
    public transactionId: string;
    public status: string;
    public dateStatusChanged: string;

  constructor() { 
    this.debtorFirstName = '';
    this.debtorLastName = '';
    this.debtorAccountNumber = '';
    this.debtorBankId = '';
    this.creditorFirstName = '';
    this.creditorLastName = '';
    this.creditorAccountNumber = '';
    this.creditorBankId = '';
    this.amount = 0;
    this.transactionId = '';
    this.status = '';
    this.dateStatusChanged = '';
   }
}

export class TransactionCancelModel {
  public transactionId: string;
  public reason: string;

  constructor() {
    this.transactionId = '';
    this.reason = '';
  }
}

export class TransactionCreateModel {
  public debtorFirstName: string;
    public debtorLastName: string;
    public debtorAccountNumber: string;
    public debtorBankId: string;
    public creditorFirstName: string;
    public creditorLastName: string;
    public creditorAccountNumber: string;
    public creditorBankId: string;
    public amount: number;

  constructor() { 
    this.debtorFirstName = '';
    this.debtorLastName = '';
    this.debtorAccountNumber = '';
    this.debtorBankId = '';
    this.creditorFirstName = '';
    this.creditorLastName = '';
    this.creditorAccountNumber = '';
    this.creditorBankId = '';
    this.amount = 0;
   }
}
