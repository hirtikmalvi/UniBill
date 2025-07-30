import { User } from '../User/user';
import { BusinessAddress } from './business-address';
import { BusinessType } from './business-type';

export interface GetBusiness {
  businessId: number;
  businessName: string;
  phone: string;
  user: User;
  businessType: BusinessType;
  address: BusinessAddress;
}
