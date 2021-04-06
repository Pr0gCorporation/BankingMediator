import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TransactionCancelModel } from '../Models/Transactions';
import { CancelTransactionDialogResult } from '../Models/CancelTransactionDialogResult';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-cancel-dialog',
  templateUrl: './cancel-dialog.component.html',
  styleUrls: ['./cancel-dialog.component.scss'],
})
export class CancelDialogComponent implements OnInit {
  constructor(
    public dialogRef: MatDialogRef<CancelDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public transaction: TransactionCancelModel,
    public translate: TranslateService
  ) {}

  ngOnInit(): void {}

  generateCancelTransactionResult(
    reason: string
  ): CancelTransactionDialogResult {
    this.transaction.reason = reason;
    let transactionCancelResult = new CancelTransactionDialogResult();
    transactionCancelResult.transactionCancel = this.transaction;
    transactionCancelResult.toDelete = true;
    return transactionCancelResult;
  }

  onNoClick(): void {
    this.dialogRef.close();
  }
}
