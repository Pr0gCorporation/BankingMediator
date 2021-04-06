import { Injectable } from '@angular/core';
import {
  TransactionReadModel,
  TransactionCreateModel,
  TransactionCancelModel,
} from '../Models/Transactions';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class TransactionsService {
  public transactions: Array<TransactionReadModel> = [];
  private transactionUrl: string =
    environment.baseUrl + environment.transactionUrlPart;
  private transactionCancelUrl: string =
    environment.baseUrl + environment.transactionCancelUrlPart;

  constructor(public http: HttpClient) {}

  fetchTransactions(): Observable<Array<TransactionReadModel>> {
    return this.http.get<Array<TransactionReadModel>>(this.transactionUrl).pipe(
      tap((transactions) => {
        return (this.transactions = transactions.reverse());
      })
    );
  }

  postTransaction(transaction: TransactionCreateModel): number {
    try {
      this.http
        .post<TransactionCreateModel>(this.transactionUrl, transaction)
        .subscribe(() => {
          this.fetchTransactions().subscribe();
        });
      return 1;
    } catch {
      return 0;
    }
  }

  cancelTransaction(transactionCancel: TransactionCancelModel) {
    try {
      this.http
        .post<TransactionCancelModel>(
          this.transactionCancelUrl,
          transactionCancel
        )
        .subscribe(() => {
          this.fetchTransactions().subscribe();
        });
    } catch {}
  }
}
