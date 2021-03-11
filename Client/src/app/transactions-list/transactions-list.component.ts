import { Component, Input, OnInit } from '@angular/core';
import { TransactionsService } from '../shared/transactions.service';
import {TranslateService} from '@ngx-translate/core';

@Component({
  selector: 'app-transactions-list',
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.scss']
})
export class TransactionsListComponent implements OnInit {

  public loading: boolean = true;

  constructor(public transactionService: TransactionsService,
    public translate: TranslateService)
    {
      translate.addLangs(['en', 'nl']);
      translate.setDefaultLang('en');

      const browserLang = translate.getBrowserLang();
      translate.use(browserLang.match(/en|nl/) ? browserLang : 'en');
    }

  ngOnInit(): void {
    this.transactionService.fetchTransactions().subscribe(() => {
      this.loading = false;
    });
  }
}
