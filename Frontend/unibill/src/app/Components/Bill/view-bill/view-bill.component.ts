import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { BillService } from '../../../Services/Bill/bill.service';
import { ToastrService } from 'ngx-toastr';
import {
  CommonModule,
  CurrencyPipe,
  DatePipe,
  JsonPipe,
} from '@angular/common';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { catchError, of } from 'rxjs';
import { GetBill, BillItem } from '../../../Models/Bill/get-bill';

@Component({
  selector: 'app-view-bill',
  standalone: true,
  imports: [CommonModule, SpinnerComponent, CurrencyPipe, JsonPipe, RouterLink],
  templateUrl: './view-bill.component.html',
  styleUrls: ['./view-bill.component.css'],
})
export class ViewBillComponent implements OnInit {
  private billService = inject(BillService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private toastr = inject(ToastrService);

  billId!: number;
  bill!: GetBill;
  isLoading = false;
  hasError = false;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('billId');
    this.billId = id ? Number(id) : 0;

    if (!this.billId) {
      this.toastr.error('Invalid bill id');
      this.hasError = true;
      return;
    }

    this.loadBill();
  }

  loadBill() {
    this.isLoading = true;
    this.hasError = false;

    this.billService
      .getBillById(this.billId)
      .pipe(
        catchError((err) => {
          const msg = err?.error?.message ?? 'Failed to fetch bill';
          const errors = err?.error?.errors ?? [];
          this.toastr.error(
            (errors.length ? errors.join(', ') + ' • ' : '') + msg,
            'Error'
          );
          this.isLoading = false;
          this.hasError = true;
          return of(null);
        })
      )
      .subscribe((res: any) => {
        this.isLoading = false;

        if (!res || !res.success || !res.data) {
          this.hasError = true;
          return;
        }

        const api = res.data;

        // FINAL MAPPED BILL OBJECT
        this.bill = {
          billId: api.billId,
          customerId: api.customer?.customerId,
          customer: api.customer?.customerName ?? '',
          customerMobileNumber: api.customer?.mobileNumber ?? '',
          date: api.date,
          discount: api.discount ?? 0,
          tax: api.tax ?? 0,
          labourCharges: api.labourCharges ?? 0,

          statusId: api.statusId ?? null,
          statusName: api.statusName ?? '',
          paymentModeId: api.paymentModeId ?? null,
          paymentModeName: api.paymentModeName ?? '',
          paidAmount: api.paidAmount ?? null,
          paymentDate: api.paymentDate ?? null,
          notes: api.notes ?? '',
          isDeleted: api.isDeleted ?? false,
          deletedAt: api.deletedAt ?? null,

          subTotal: api.subTotal ?? 0,
          discountAmount: api.discountAmount ?? 0,
          taxAmount: api.taxAmount ?? 0,
          finalTotal: api.finalTotal ?? 0,

          billItems: Array.isArray(api.billItems)
            ? api.billItems.map((bi: any): BillItem => {
                const item = bi.item || {};

                return {
                  itemId: item.itemId ?? 0,
                  itemName: item.itemName ?? 'Unknow Item',
                  quantity: bi.quantity ?? 0,
                  rate: bi.rate ?? item.itemRate ?? 0,
                  amount: bi.total ?? (bi.rate ?? 0) * (bi.quantity ?? 0),

                  categoryId: item.categoryId ?? 0,
                  categoryName: item.category ?? 'N/A',

                  unitId: item.unitId ?? 0,
                  unitName: item.unit ?? 'N/A',
                  unitShortName: item.unitShortName ?? '',

                  itemTypeId: item.itemTypeId ?? 0,
                  itemTypeName: item.itemType ?? 'N/A',
                };
              })
            : [],
        };
      });
  }

  goBack() {
    this.router.navigate(['/bills']);
  }
}
