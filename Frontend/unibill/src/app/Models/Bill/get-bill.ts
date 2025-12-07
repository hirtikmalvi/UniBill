export interface GetBill {
  billId: number;
  customerId: number;
  customer: string;
  customerMobileNumber: string;
  date: string; // ISO string
  discount: number;
  tax: number;
  labourCharges: number;
  statusId?: number;
  statusName?: string;
  paymentModeId?: number;
  paymentModeName?: string;
  paidAmount?: number;
  paymentDate?: number;
  notes?: string;
  isDeleted?: boolean;
  deletedAt?: string;
  subTotal: number;
  discountAmount: number;
  taxAmount: number;
  finalTotal: number;

  billItems?: BillItem[];
}

export interface BillItem {
  itemId: number;
  itemName: string;
  quantity: number;
  rate: number;
  amount: number;
  categoryId: number;
  categoryName: string;
  unitId: number;
  unitName: string;
  unitShortName: string;
  itemTypeId: number;
  itemTypeName: string;
}
