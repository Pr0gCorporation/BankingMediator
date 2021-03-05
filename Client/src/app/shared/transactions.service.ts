import { Injectable } from "@angular/core";
import { Transaction } from '../Interfaces/Transaction';
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";
import { catchError, map, tap } from "rxjs/operators";
import { environment } from "../../environments/environment";

@Injectable({providedIn: 'root'})
export class TransactionsService {
  public transactions: Array<Transaction> = [];
  private transactionUrl: string = environment.transactionUrl; 

  constructor(public http: HttpClient) {};

  fetchTransactions() : Observable<Array<Transaction>> {
      return this.http.get<Array<Transaction>>(this.transactionUrl)
      .pipe(tap(transactions => this.transactions = transactions));
  }

  postTransaction(transaction: Transaction) {
    this.http.post<Transaction>(this.transactionUrl, transaction).subscribe();
    this.transactions.push(transaction);
  }
}
