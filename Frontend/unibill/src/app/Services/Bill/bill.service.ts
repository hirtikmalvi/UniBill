import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

import { CustomResult } from '../../Models/model.custom-result';
import { GetBill } from '../../Models/Bill/get-bill';
import { CreateBill } from '../../Models/Bill/create-bill';
import { UpdateBillStatus } from '../../Models/Bill/update-bill-status';

@Injectable({
  providedIn: 'root',
})
export class BillService {
  private URL: string = environment.BACKEND_URL;
  private http = inject(HttpClient);

  bills$ = this.getBills();

  getBills(): Observable<CustomResult<GetBill[]>> {
    return this.http.get<CustomResult<GetBill[]>>(`${this.URL}/bills`);
  }

  getBillById(id: number): Observable<CustomResult<GetBill>> {
    return this.http.get<CustomResult<GetBill>>(`${this.URL}/bills/${id}`);
  }

  createBill(request: CreateBill): Observable<CustomResult<GetBill>> {
    return this.http.post<CustomResult<GetBill>>(`${this.URL}/bills`, request);
  }

  updateBillStatus(
    id: number,
    request: UpdateBillStatus
  ): Observable<CustomResult<string>> {
    return this.http.put<CustomResult<string>>(
      `${this.URL}/bills/${id}/status`,
      request
    );
  }

  deleteBill(id: number): Observable<CustomResult<string>> {
    return this.http.delete<CustomResult<string>>(`${this.URL}/bills/${id}`);
  }
}
