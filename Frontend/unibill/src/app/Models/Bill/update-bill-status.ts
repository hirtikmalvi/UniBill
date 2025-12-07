export interface UpdateBillStatus {
  statusId: number;
  paymentModeId: number | null;
  paidAmount?: number | null;
  paymentDate?: string | null;
  notes?: string | null;
}
