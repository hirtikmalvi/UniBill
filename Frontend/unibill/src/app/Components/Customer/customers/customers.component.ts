import { Component, inject, OnInit } from '@angular/core';
import { CustomerService } from '../../../Services/Customer/customer.service';
import { GetCustomer } from '../../../Models/Customer/get-customer';
import { CustomResult } from '../../../Models/model.custom-result';
import { GetBill } from '../../../Models/Bill/get-bill';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, of } from 'rxjs';
import { NgFor, NgIf } from '@angular/common';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';

@Component({
  selector: 'app-customers',
  imports: [NgIf, NgFor, SpinnerComponent],
  templateUrl: './customers.component.html',
  styleUrl: './customers.component.css',
})
export class CustomersComponent implements OnInit {
  isProcessing: boolean = false;

  customers: GetCustomer[] = [];

  private customerService = inject(CustomerService);
  private toastr = inject(ToastrService);

  ngOnInit(): void {
    this.isProcessing = true;
    this.customerService
      .getCustomers()
      .pipe(
        map((res) => res),
        catchError((err) => {
          const response = err.error as CustomResult<GetCustomer[]>;
          return of(response);
        })
      )
      .subscribe((res) => {
        if (res.success) {
          this.customers = res.data!;
          this.toastr.success(res.message, 'Customers');
        } else {
          this.customers = [];
          this.toastr.error(res.errors?.join(', '), res.message);
        }
        this.isProcessing = false;
      });
  }
}
