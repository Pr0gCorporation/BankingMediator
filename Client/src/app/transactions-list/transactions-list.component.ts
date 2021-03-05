import { Component, Input, OnInit } from '@angular/core';
import { TransactionsService } from '../shared/transactions.service';

@Component({
  selector: 'app-transactions-list',
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.scss']
})
export class TransactionsListComponent implements OnInit {

  public loading: boolean = true;

  constructor(public transactionService: TransactionsService) { }

  ngOnInit(): void {
    this.transactionService.fetchTransactions().subscribe(() => {
      this.loading = false;
    });
  }
}
