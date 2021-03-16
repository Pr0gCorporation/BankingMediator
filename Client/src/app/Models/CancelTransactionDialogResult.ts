import { TransactionCancelModel } from './Transactions';

export class CancelTransactionDialogResult {
  public transactionCancel: TransactionCancelModel;
  public toDelete: boolean;

  constructor() {
    this.transactionCancel = new TransactionCancelModel();
    this.toDelete = false;
  }
}
