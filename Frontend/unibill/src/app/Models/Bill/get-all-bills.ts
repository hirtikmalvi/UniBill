export interface GetAllBills {
  billId: number;
  customerId: number;
  customer: string;
  customerMobileNumber: string;
  date: string; // ISO string
  discount: number;
  tax: number;
  labourCharges: number;
  billStatusId?: number;
  billStatus?: string;
  paymentModeId?: number;
  paymentMode?: string;
  paidAmount?: number;
  paymentDate?: number;
  subTotal: number;
  discountAmount: number;
  taxAmount: number;
  finalTotal: number;
}
