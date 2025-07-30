import { inject, Injectable } from '@angular/core';
import { UserResponse } from '../../Models/Registration/registration.UserResponse';
import { RegisterUser } from '../../Models/Registration/registration.user';
import { HttpClient } from '@angular/common/http';
import { CustomResult } from '../../Models/model.custom-result';
import { LoginRequest } from '../../Models/Login/login-request';
import { LoginResponse } from '../../Models/Login/login-response';
import {
  BehaviorSubject,
  catchError,
  map,
  Observable,
  of,
  Subject,
} from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private URL = environment.BACKEND_URL;

  private http = inject(HttpClient);
  private userStatusSubject$ = new BehaviorSubject<boolean>(false);
  userStatus = this.userStatusSubject$.asObservable();

  registerUser(request: RegisterUser): Observable<CustomResult<UserResponse>> {
    return this.http.post<CustomResult<UserResponse>>(
      `${this.URL}/auth/register`,
      request
    );
  }
  loginUser(request: LoginRequest): Observable<CustomResult<LoginResponse>> {
    return this.http.post<CustomResult<LoginResponse>>(
      `${this.URL}/auth/login`,
      request
    );
  }

  validUser(): Observable<CustomResult<string>> {
    return this.http.get<CustomResult<string>>(`${this.URL}/auth/validate`);
  }

  logoutUser(): void {
    if (localStorage.getItem('token') != null) {
      this.setUserStatus(false);
      localStorage.removeItem('token');
    }
  }

  ValidateUserAndUpdateStatus() {
    this.validUser().subscribe({
      next: (res) => {
        this.setUserStatus(true);
      },
      error: (err) => {
        this.setUserStatus(false);
      },
    });
  }

  setUserStatus(status: boolean) {
    this.userStatusSubject$.next(status);
  }
}
