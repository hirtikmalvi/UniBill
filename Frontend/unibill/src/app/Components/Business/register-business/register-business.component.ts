import { Component, inject, OnInit } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { BusinessType } from '../../../Models/Business/business-type';
import { LookupService } from '../../../Services/Lookup/lookup.service';
import { catchError, map, of } from 'rxjs';
import { CustomResult } from '../../../Models/model.custom-result';
import { ToastrService } from 'ngx-toastr';
import { JsonPipe, NgFor, NgIf } from '@angular/common';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { BusinessService } from '../../../Services/Business/business.service';
import { RegisterBusinessResponse } from '../../../Models/Business/register-business-response';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-register-business',
  imports: [NgFor, NgIf, ReactiveFormsModule, SpinnerComponent, RouterModule],
  templateUrl: './register-business.component.html',
  styleUrl: './register-business.component.css',
})
export class RegisterBusinessComponent implements OnInit {
  businessService = inject(BusinessService);
  lookupService = inject(LookupService);
  toastr = inject(ToastrService);
  router = inject(Router);

  registerBusinessForm!: FormGroup;
  busienssTypes: BusinessType[];
  isProcessing: boolean = false;
  hasBusiness: boolean = false;

  constructor() {
    this.busienssTypes = [];
  }

  ngOnInit(): void {
    this.businessService.userHaveBusiness.subscribe({
      next: (res) => {
        this.hasBusiness = res;
      },
    });

    if (!this.hasBusiness) {
      this.lookupService
        .getBusinessTypes()
        .pipe(
          map((res) => res),
          catchError((err) => {
            const response = err.error as CustomResult<BusinessType[]>;

            this.toastr.error(response.errors?.join(','), response.message);

            return of(response);
          })
        )
        .subscribe({
          next: (res) => {
            this.busienssTypes = res.data!;
          },
        });

      this.registerBusinessForm = new FormGroup({
        businessTypeId: new FormControl(0, Validators.required),
        businessName: new FormControl(null, Validators.required),
        phoneNo: new FormControl(null, [
          Validators.required,
          Validators.pattern('^[1-9][0-9]{9}$'),
        ]),
        shopNo: new FormControl(null, [
          Validators.required,
          Validators.pattern('^[a-zA-Z0-9\\s-]{1,100}$'),
        ]),
        area: new FormControl(null, [
          Validators.required,
          Validators.pattern('^[a-zA-Z0-9\\s-]{1,100}$'),
        ]),
        landmark: new FormControl(null, [
          Validators.pattern('^[a-zA-Z0-9\\s-]{1,100}$'),
        ]),
        road: new FormControl(null, [
          Validators.pattern('^[a-zA-Z0-9\\s-]{1,100}$'),
        ]),
        city: new FormControl(null, [
          Validators.required,
          Validators.pattern('^[a-zA-Z\\s]{1,100}$'),
        ]),
        state: new FormControl(null, [
          Validators.required,
          Validators.pattern('^[a-zA-Z\\s]{1,100}$'),
        ]),
        country: new FormControl(null, [
          Validators.required,
          Validators.pattern('^[a-zA-Z\\s]{1,100}$'),
        ]),
        pinOrPostalCode: new FormControl(null, [
          Validators.required,
          Validators.pattern('^[0-9]{6}$'),
        ]),
      });
    }
  }

  hasControlError(controlName: string, errorName: string): boolean {
    let control = this.registerBusinessForm.get(controlName);

    if (control != null) {
      return (
        (control?.touched || control?.dirty) && control?.hasError(errorName)
      );
    } else {
      return false;
    }
  }

  onRegister() {
    if (!this.registerBusinessForm.valid) {
      this.registerBusinessForm.markAllAsTouched();
      return;
    } else {
      this.isProcessing = true;
      this.businessService
        .registerBusiness(this.registerBusinessForm.value)
        .pipe(
          map((res) => res),
          catchError((err) => {
            const response =
              err.error as CustomResult<RegisterBusinessResponse>;
            return of(response);
          })
        )
        .subscribe({
          next: (res) => {
            if (res.success) {
              this.toastr.success(res.message, 'Business Registration');
              if (localStorage.getItem('token') != null) {
                localStorage.removeItem('token');
                localStorage.setItem(
                  'token',
                  JSON.stringify(res.data?.accessToken)
                );
              }
              this.router.navigate(['business/my-business']);
              this.registerBusinessForm.reset();
            } else {
              this.toastr.error(res.errors?.join('\n,'), res.message);
            }
            this.isProcessing = false;
          },
        });
    }
  }
}
