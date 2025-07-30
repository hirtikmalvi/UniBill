export interface RegisterBusiness {
  businessTypeId: number;
  businessName: string;
  phoneNo: string;
  shopNo: string;
  area: string;
  landmark: string | null;
  road: string | null;
  city: string;
  state: string;
  country: string;
  pinOrPostalCode: string;
}
