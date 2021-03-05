import { Component, OnInit } from '@angular/core';
import { Transaction } from '../Interfaces/Transaction';
import { TransactionsService } from '../shared/transactions.service';

@Component({
  selector: 'app-add-transaction',
  templateUrl: './add-transaction.component.html',
  styleUrls: ['./add-transaction.component.scss']
})
export class AddTransactionComponent implements OnInit {

  constructor(public transactionService: TransactionsService) { }

  ngOnInit(): void {
  }

  addTransaction(debtorFirstName: string, debtorLastName: string, debtorAccountNumber: string, debtorBankId: string, 
    creditorFirstName: string, creditorLastName: string, creditorAccountNumber: string, creditorBankId: string, 
    amount: string, transactionId: string) {
      let transaction: Transaction = {
        debtorFirstName, debtorLastName, debtorAccountNumber, debtorBankId,
        creditorFirstName, creditorLastName, creditorAccountNumber, creditorBankId,
        amount: Number(amount), transactionId
      };

      // TODO: validate transaction

      this.transactionService.postTransaction(transaction);
  }
}
