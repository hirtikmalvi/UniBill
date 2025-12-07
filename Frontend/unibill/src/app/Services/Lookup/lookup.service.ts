import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CustomResult } from '../../Models/model.custom-result';
import { BusinessType } from '../../Models/Business/business-type';
import { UnitByBusiness } from '../../Models/Unit/unit-by-business';
import { ItemTypeByBusiness } from '../../Models/ItemType/item-type-by-business';
import { Category } from '../../Models/Category/category';
import { PaymentMode } from '../../Models/Bill/payment-mode';
import { BillStatus } from '../../Models/Bill/bill-status';

@Injectable({
  providedIn: 'root',
})
export class LookupService {
  private URL: string = environment.BACKEND_URL;
  private http = inject(HttpClient);
  constructor() {}

  getBusinessTypes() {
    return this.http.get<CustomResult<BusinessType[]>>(
      `${this.URL}/lookups/business-types`
    );
  }

  getUnitsByBusiness() {
    return this.http.get<CustomResult<UnitByBusiness[]>>(
      `${this.URL}/lookups/units/by-business`
    );
  }

  getItemTypesByBusiness() {
    return this.http.get<CustomResult<ItemTypeByBusiness[]>>(
      `${this.URL}/lookups/item-types/by-business`
    );
  }

  getCategoriesByItemType(id: number) {
    return this.http.get<CustomResult<Category[]>>(
      `${this.URL}/lookups/categories/by-item-type/${id}`
    );
  }

  getPaymentModes() {
    return this.http.get<CustomResult<PaymentMode[]>>(
      `${this.URL}/lookups/payment-modes`
    );
  }

  getBillStatuses() {
    return this.http.get<CustomResult<BillStatus[]>>(
      `${this.URL}/lookups/bill-statuses`
    );
  }
}
