import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../Services/Auth/auth.service';
import { LoginResponse } from '../../../Models/Login/login-response';
import { Router } from '@angular/router';
import { CustomResult } from '../../../Models/model.custom-result';
import { NgIf } from '@angular/common';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, of, Subject } from 'rxjs';
import { RegisterUser } from '../../../Models/Registration/registration.user';
import { BusinessService } from '../../../Services/Business/business.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, NgIf, SpinnerComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  authService = inject(AuthService);
  businessService = inject(BusinessService);
  loginForm!: FormGroup;
  isProcessing: boolean | undefined;

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, [Validators.required]),
    });
  }

  constructor(private router: Router, private toastr: ToastrService) {}

  handleLogin() {
    if (!this.loginForm.valid) {
      this.loginForm.markAllAsTouched();
      return;
    } else {
      this.isProcessing = true;
      this.authService
        .loginUser(this.loginForm.value)
        .pipe(
          map((res) => {
            return res;
          }),
          catchError((err) => {
            return of(err.error);
          })
        )
        .subscribe({
          next: (res: CustomResult<LoginResponse>) => {
            if (res.success) {
              localStorage.setItem('token', res.data?.accessToken!);
              this.toastr.success(res.message, 'Login');

              if (res.data?.requiresBusiness) {
                this.router.navigate(['business/register-business']);
              } else {
                this.router.navigate(['/dashboard']);
              }
              this.authService.ValidateUserAndUpdateStatus();
              this.businessService.hasBusinessAndUpdateStatus();
              this.loginForm.reset();
            } else {
              this.toastr.error(res.errors?.join('\n,'), res.message);
            }
            this.isProcessing = false;
          },
        });
    }
  }
}
