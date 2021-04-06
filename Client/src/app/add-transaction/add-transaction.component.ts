import { Component, OnInit } from '@angular/core';
import { TransactionCreateModel } from '../Models/Transactions';
import { TransactionsService } from '../shared/transactions.service';
import { TranslateService } from '@ngx-translate/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-add-transaction',
  templateUrl: './add-transaction.component.html',
  styleUrls: ['./add-transaction.component.scss'],
})
export class AddTransactionComponent implements OnInit {
  constructor(
    public transactionService: TransactionsService,
    public translate: TranslateService
  ) {}
  showMsg: boolean = false;
  transaction = new TransactionCreateModel();

  ngOnInit(): void {}

  onSubmit(transactionForm: NgForm) {
    let transactionPostedSuccessfully = this.transactionService.postTransaction(
      this.transaction
    );

    if (transactionPostedSuccessfully) {
      this.transaction = new TransactionCreateModel();
      transactionForm.reset();
      this.showMsg = true;
      setTimeout(() => {
        this.showMsg = false;
      }, 10 * 1000);
      console.log('Transaction added.');
    } else {
      console.log('Transaction failed.');
    }
  }
}
