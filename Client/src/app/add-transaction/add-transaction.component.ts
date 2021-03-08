import { Component, OnInit } from '@angular/core';
import { Transaction } from '../Interfaces/Transaction';
import { TransactionsService } from '../shared/transactions.service';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-add-transaction',
  templateUrl: './add-transaction.component.html',
  styleUrls: ['./add-transaction.component.scss']
})
export class AddTransactionComponent implements OnInit {

  constructor(public transactionService: TransactionsService) { }

  transaction = new Transaction('', '', '', '', '', '', '', '', 0, 
  Guid.create().toString());

  ngOnInit(): void {
  }

  onSubmit() {
    this.transactionService.postTransaction(this.transaction);
    console.log("Transaction added.");
  }
}
