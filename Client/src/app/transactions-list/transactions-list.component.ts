import { Component, Input, OnInit } from '@angular/core';
import { TransactionsService } from '../shared/transactions.service';
import { TransactionCancelModel } from '../Models/Transactions';
import { TranslateService } from '@ngx-translate/core';
import { MatDialog } from '@angular/material/dialog';
import { CancelDialogComponent } from '../cancel-dialog/cancel-dialog.component';
import { CancelTransactionDialogResult } from '../Models/CancelTransactionDialogResult';

@Component({
  selector: 'app-transactions-list',
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.scss'],
})
export class TransactionsListComponent implements OnInit {
  public loading: boolean = true;
  public transactionCancel = new TransactionCancelModel();

  constructor(
    public transactionService: TransactionsService,
    public translate: TranslateService,
    public dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.transactionService.fetchTransactions().subscribe(() => {
      this.loading = false;
    });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(CancelDialogComponent, {
      width: '',
      data: this.transactionCancel,
    });

    dialogRef.afterClosed().subscribe((result) => {
      let dialogResult: CancelTransactionDialogResult = result as CancelTransactionDialogResult;

      if (dialogResult.toDelete === true) {
        this.transactionCancel = dialogResult.transactionCancel;
        this.cancelTransaction();
      }
    });
  }

  cancelTransactionDialog(transaction_id: string, reason: string) {
    this.transactionCancel.transactionId = transaction_id;
    this.transactionCancel.reason = reason;
    this.openDialog();
  }

  cancelTransaction() {
    this.transactionService.cancelTransaction(this.transactionCancel);
  }
}
