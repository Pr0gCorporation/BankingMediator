import { Component, OnInit } from '@angular/core';
import { TransactionCreateModel } from '../Models/Transactions';
import { TransactionsService } from '../shared/transactions.service';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-add-transaction',
  templateUrl: './add-transaction.component.html',
  styleUrls: ['./add-transaction.component.scss']
})
export class AddTransactionComponent implements OnInit {

  constructor(public transactionService: TransactionsService, 
    public translate: TranslateService)
  {
    translate.addLangs(['en', 'nl']);
    translate.setDefaultLang('en');

    const browserLang = translate.getBrowserLang();
    translate.use(browserLang.match(/en|nl/) ? browserLang : 'en');
  }
  showMsg: boolean = false;
  transaction = new TransactionCreateModel();

  ngOnInit(): void {
  }

  onSubmit() {
    let transactionPostedSuccessfully = this.transactionService.postTransaction(this.transaction);

    if(transactionPostedSuccessfully)
    {
      this.transaction = new TransactionCreateModel();
      this.showMsg= true;
      setTimeout(() => { this.showMsg= false; }, 10 * 1000);
      console.log("Transaction added.");
    } else {
      console.log("Transaction failed.");
    }
  }
}
