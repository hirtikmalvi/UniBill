import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CustomResult } from '../../Models/model.custom-result';
import { GetCustomer } from '../../Models/Customer/get-customer';
import { CreateCustomer } from '../../Models/Customer/create-customer';

@Injectable({
  providedIn: 'root',
})
export class CustomerService {
  private URL: string = environment.BACKEND_URL;
  private http = inject(HttpClient);

  getCustomerById(id: number): Observable<CustomResult<GetCustomer>> {
    return this.http.get<CustomResult<GetCustomer>>(
      `${this.URL}/customers/${id}`
    );
  }

  getCustomers(): Observable<CustomResult<GetCustomer[]>> {
    return this.http.get<CustomResult<GetCustomer[]>>(`${this.URL}/customers`);
  }

  getCustomerByMobileNumber(
    mobileNumber: string
  ): Observable<CustomResult<GetCustomer>> {
    return this.http.get<CustomResult<GetCustomer>>(
      `${this.URL}/customers/by-mobile-no/${mobileNumber}`
    );
  }

  createCustomer(
    request: CreateCustomer
  ): Observable<CustomResult<GetCustomer>> {
    return this.http.post<CustomResult<GetCustomer>>(
      `${this.URL}/customers`,
      request
    );
  }
}
