import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { RegisterBusiness } from '../../Models/Business/register-business';
import { HttpClient } from '@angular/common/http';
import { CustomResult } from '../../Models/model.custom-result';
import { RegisterBusinessResponse } from '../../Models/Business/register-business-response';
import { GetBusiness } from '../../Models/Business/Get-Business';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BusinessService {
  private URL: string = environment.BACKEND_URL;
  private http = inject(HttpClient);
  private doesUserHaveBusiness$ = new BehaviorSubject<boolean>(false);
  userHaveBusiness = this.doesUserHaveBusiness$.asObservable();

  registerBusiness(registerBusiness: RegisterBusiness) {
    return this.http.post<CustomResult<RegisterBusinessResponse>>(
      `${this.URL}/business/register`,
      registerBusiness
    );
  }

  getBusiness() {
    return this.http.get<CustomResult<GetBusiness>>(
      `${this.URL}/business/profile`
    );
  }

  hasBusiness() {
    return this.http.get<CustomResult<boolean>>(
      `${this.URL}/business/has-business`
    );
  }

  hasBusinessAndUpdateStatus() {
    this.hasBusiness().subscribe({
      next: (res) => {
        this.changeUserHasBusinessStatus(res.success);
      },
      error: () => {
        this.changeUserHasBusinessStatus(false);
      },
    });
  }

  private changeUserHasBusinessStatus(status: boolean) {
    this.doesUserHaveBusiness$.next(status);
  }
}
