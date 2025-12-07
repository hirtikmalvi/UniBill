export interface CreateBill {
  customerId: number;
  date: string;
  discount: number;
  tax: number;
  labourCharges: number;

  items: CreateBillItem[];
}

export interface CreateBillItem {
  itemId: number;
  quantity: number;
  rate: number;
}
