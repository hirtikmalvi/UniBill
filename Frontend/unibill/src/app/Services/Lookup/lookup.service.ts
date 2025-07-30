import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { CustomResult } from '../../Models/model.custom-result';
import { BusinessType } from '../../Models/Business/business-type';

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
}
