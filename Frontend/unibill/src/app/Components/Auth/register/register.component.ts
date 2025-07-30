import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CustomValidators } from '../../../Validators/CustomValidators';
import { NgIf } from '@angular/common';
import { AuthService } from '../../../Services/Auth/auth.service';
import { CustomResult } from '../../../Models/model.custom-result';
import { UserResponse } from '../../../Models/Registration/registration.UserResponse';
import { catchError, map, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, NgIf, SpinnerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent implements OnInit {
  registerForm!: FormGroup;
  authService = inject(AuthService);
  router = inject(Router);
  toastr = inject(ToastrService);

  isProcessing: boolean = false;

  ngOnInit(): void {
    this.registerForm = new FormGroup(
      {
        email: new FormControl(null, [Validators.required, Validators.email]),
        password: new FormControl(null, [
          Validators.required,
          Validators.pattern(
            '^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$]).{6,15}$'
          ),
        ]),
        confirmPassword: new FormControl(null, [Validators.required]),
      },
      {
        validators: CustomValidators.confirmPasswordValidator(),
      }
    );
  }

  handleSubmit() {
    if (!this.registerForm.valid) {
      this.registerForm.markAllAsTouched();
      return;
    } else {
      this.isProcessing = true;
      this.authService
        .registerUser(this.registerForm.value)
        .pipe(
          map((res) => res),
          catchError((err) => {
            return of(err.error);
          })
        )
        .subscribe({
          next: (res: CustomResult<UserResponse>) => {
            if (res.success) {
              this.toastr.success(res.message, 'Registration');
              this.router.navigate(['auth/login']);
              this.registerForm.reset();
            } else {
              this.toastr.error(res.errors?.join('\n,'), res.message);
            }
            this.isProcessing = false;
          },
        });
    }
  }
}
