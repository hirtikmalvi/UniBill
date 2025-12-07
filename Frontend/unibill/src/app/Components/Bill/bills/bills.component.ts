import { Component, inject, OnInit } from '@angular/core';
import { BillService } from '../../../Services/Bill/bill.service';
import { ToastrService } from 'ngx-toastr';
import { CustomResult } from '../../../Models/model.custom-result';
import { NgFor, NgIf, CurrencyPipe, DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { GetBill } from '../../../Models/Bill/get-bill';
import { catchError, map, of } from 'rxjs';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { GetAllBills } from '../../../Models/Bill/get-all-bills';

@Component({
  selector: 'app-bills',
  standalone: true,
  imports: [NgFor, NgIf, CurrencyPipe, DatePipe, SpinnerComponent],
  templateUrl: './bills.component.html',
  styleUrls: ['./bills.component.css'],
})
export class BillsComponent implements OnInit {
  private billService = inject(BillService);
  private toastr = inject(ToastrService);
  private router = inject(Router);

  isProcessing: boolean = false;
  bills!: CustomResult<GetAllBills[]>;

  ngOnInit(): void {
    this.fetchBills();
  }

  fetchBills() {
    this.isProcessing = true;

    this.billService
      .getBills()
      .pipe(
        map((res) => {
          this.toastr.success(res.message, 'Bills');
          return res;
        }),
        catchError((err) => {
          const response = err.error as CustomResult<GetAllBills[]>;
          this.toastr.error(response.errors?.join('\n'), response.message);
          return of(response);
        })
      )
      .subscribe({
        next: (res) => {
          this.bills = res;
          console.log(res);
          this.isProcessing = false;
        },
        error: () => {
          this.isProcessing = false;
        },
      });
  }

  onCreateBill() {
    this.router.navigate(['/bills/create']);
  }

  onViewBill(billId: number) {
    this.router.navigate(['/bills', billId, 'view']);
  }

  onEditBill(billId: number) {
    this.router.navigate(['/bills', billId, 'edit']);
  }

  onDeleteBill(id: number) {
    if (confirm(`Are you sure you want to delete Bill #${id}?`)) {
      this.billService.deleteBill(id).subscribe({
        next: (res: CustomResult<string>) => {
          if (res.success) {
            this.toastr.success(res.message, res.data!);
            this.fetchBills();
          }
        },
        error: (err) => {
          const response = err.error as CustomResult<string>;
          this.toastr.error(response.errors?.join(', '), response.message);
        },
      });
    }
  }
}
