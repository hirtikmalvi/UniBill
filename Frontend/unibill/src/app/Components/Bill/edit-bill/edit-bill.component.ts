import { Component, inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { BillService } from '../../../Services/Bill/bill.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { BillStatus } from '../../../Models/Bill/bill-status';
import { PaymentMode } from '../../../Models/Bill/payment-mode';
import { LookupService } from '../../../Services/Lookup/lookup.service';
import { CustomResult } from '../../../Models/model.custom-result';
import { JsonPipe, NgClass, NgFor, NgIf } from '@angular/common';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { UpdateBillStatus } from '../../../Models/Bill/update-bill-status';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-edit-bill',
  imports: [
    JsonPipe,
    NgFor,
    NgIf,
    ReactiveFormsModule,
    SpinnerComponent,
    NgClass,
  ],
  templateUrl: './edit-bill.component.html',
  styleUrl: './edit-bill.component.css',
})
export class EditBillComponent implements OnInit {
  private billService = inject(BillService);
  private lookupService = inject(LookupService);
  private toastr = inject(ToastrService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  billStatusForm!: FormGroup;
  billId!: number;
  initialStatusId!: number;
  finalTotal!: number;

  billStatuses!: BillStatus[];
  paymentModes!: PaymentMode[];

  isBillLoading: boolean = false;
  isSubmitProcessing: boolean = false;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('billId');

    this.billId = id ? Number(id) : 0;
    if (!this.billId) {
      this.toastr.error('Invalid bill id');
      return;
    }

    this.isBillLoading = true;

    // LOAD PAYMENT MODES
    this.lookupService.getPaymentModes().subscribe({
      next: (res) => {
        if (res.success && res.data?.length! > 0) {
          this.paymentModes = res.data!;
        } else {
          this.paymentModes = [];
          this.toastr.error(res.errors?.join(','), res.message);
        }
      },
      error: (err) => {
        const response = err.error as CustomResult<PaymentMode[]>;
        this.toastr.error(response.errors?.join(','), response.message);
      },
    });

    // LOAD BILL STATUSES
    this.lookupService.getBillStatuses().subscribe({
      next: (res) => {
        if (res.success && res.data?.length! > 0) {
          this.billStatuses = res.data!;
        } else {
          this.billStatuses = [];
          this.toastr.error(res.errors?.join(','), res.message);
        }
      },
      error: (err) => {
        const response = err.error as CustomResult<BillStatus[]>;
        this.toastr.error(response.errors?.join(','), response.message);
      },
    });

    // SETUP FORM
    this.billStatusForm = new FormGroup(
      {
        statusId: new FormControl(0, Validators.required),
        paymentModeId: new FormControl(0),
        paidAmount: new FormControl(0),
        paymentDate: new FormControl(''),
        notes: new FormControl(''),
      },
      {
        validators: this.paymentFieldsValidator.bind(this),
      }
    );

    // LOAD BILL
    this.billService.getBillById(this.billId).subscribe({
      next: (res) => {
        if (res.success) {
          this.initialStatusId = res.data?.statusId ?? 0;
          this.finalTotal = res.data?.finalTotal ?? 0;

          if (!this.isValidStatusId(this.initialStatusId)) {
            this.initialStatusId = 0;
          }

          this.billStatusForm.patchValue({
            statusId: this.initialStatusId,
          });
        }
        this.isBillLoading = false;
      },
      error: () => {
        this.isBillLoading = false;
      },
    });

    // HANDLE STATUS CHANGE
    this.billStatusForm.get('statusId')?.valueChanges.subscribe((statusId) => {
      // FIX: previously wrong usage
      if (!this.isValidStatusId(statusId)) {
        this.billStatusForm.get('statusId')?.setValue(this.initialStatusId);
        return;
      }
      this.onStatusChange(Number(statusId));
    });
  }

  isValidStatusId(statusId: any): boolean {
    return (
      !Number.isNaN(Number(statusId)) &&
      this.billStatuses?.some((bs) => bs.statusId === Number(statusId))
    );
  }

  // ---------------------------
  // FIXED VALIDATION LOGIC
  // ---------------------------
  private paymentFieldsValidator(formGroup: AbstractControl) {
    const statusId = Number(formGroup.get('statusId')?.value);
    const pMode = formGroup.get('paymentModeId');
    const pAmt = formGroup.get('paidAmount');
    const pDate = formGroup.get('paymentDate');

    if (!pMode || !pAmt || !pDate) return null;

    // FIX: previously wrong condition
    if (!this.isValidStatusId(statusId)) {
      return { statusRequired: true };
    }

    // PENDING (1)
    if (statusId === 1) {
      if (
        (pMode.value && Number(pMode.value) > 0) ||
        (pAmt.value && Number(pAmt.value) > 0) ||
        pDate.value
      ) {
        return { pendingHasPayment: true };
      }
      return null;
    }

    // PARTIAL (3)
    if (statusId === 3) {
      if (!pMode.value || Number(pMode.value) <= 0)
        return { paymentModeRequired: true };

      if (!pAmt.value || Number(pAmt.value) <= 0)
        return { paidAmountRequired: true };

      if (this.finalTotal && Number(pAmt.value) >= this.finalTotal)
        return { paidTooLargeForPartial: true };

      if (!pDate.value) return { paymentDateRequired: true };
      return null;
    }

    // PAID (2)
    if (statusId === 2) {
      if (!pMode.value || Number(pMode.value) <= 0)
        return { paymentModeRequired: true };

      if (!pAmt.value || Number(pAmt.value) <= 0)
        return { paidAmountRequired: true };

      if (Math.abs(Number(pAmt.value) - this.finalTotal) > 0)
        return { paidMustMatchTotal: true };

      if (!pDate.value) return { paymentDateRequired: true };
      return null;
    }

    // CANCELLED (4)
    if (statusId === 4) {
      if (
        (pMode.value && Number(pMode.value) > 0) ||
        (pAmt.value && Number(pAmt.value) > 0) ||
        pDate.value
      ) {
        return { cancelledHasPayment: true };
      }
      return null;
    }

    // REFUNDED (5)
    if (statusId === 5) {
      if (!pMode.value || Number(pMode.value) <= 0)
        return { paymentModeRequired: true };

      if (pAmt.value && Number(pAmt.value) > 0)
        return { paidAmountNotRequired: true };

      if (!pDate.value) return { paymentDateRequired: true };

      return null;
    }

    return null;
  }

  // ---------------------------
  // FIX STATUS CHANGE LOGIC
  // ---------------------------
  private onStatusChange(statusId: number) {
    const pMode = this.billStatusForm.get('paymentModeId');
    const pAmt = this.billStatusForm.get('paidAmount');
    const pDate = this.billStatusForm.get('paymentDate');

    if (!this.isValidStatusId(statusId)) return;

    if (statusId === 1 || statusId === 4) {
      pMode?.disable();
      pAmt?.disable();
      pDate?.disable();

      pMode?.reset(0);
      pAmt?.reset(0);
      pDate?.reset(null);
    } else if (statusId === 2 || statusId === 3 || statusId === 5) {
      pMode?.enable();
      pDate?.enable();
      pAmt?.reset(0);

      pMode?.setValidators([Validators.required, Validators.min(1)]);
      pDate?.setValidators([Validators.required]);

      if (statusId === 5) {
        pAmt?.disable();
        pAmt?.reset(0);
      } else if (statusId === 2) {
        pAmt?.disable();
        pAmt?.setValue(this.finalTotal);
      } else {
        pAmt?.enable();
        pAmt?.setValidators([Validators.required, Validators.min(0.01)]);
        pAmt?.updateValueAndValidity();
      }

      pMode?.updateValueAndValidity();
      pDate?.updateValueAndValidity();
    }
  }

  // Utility
  hasControlError(form: AbstractControl, control: string, error: string) {
    const c = form.get(control);
    return c && (c.dirty || c.touched) && c.hasError(error);
  }

  // ---------------------------
  // SUBMIT
  // ---------------------------
  onSubmit() {
    if (!this.billId) {
      this.toastr.error('Bill Id missing');
      return;
    }

    if (!this.billStatusForm.valid) {
      this.billStatusForm.markAllAsTouched();
      return;
    }

    const payload: UpdateBillStatus = {
      statusId: Number(this.billStatusForm.get('statusId')?.value),
      notes: this.billStatusForm.get('notes')?.value || null,
      paidAmount: null,
      paymentDate: null,
      paymentModeId: null,
    };

    if (
      payload.statusId === 2 ||
      payload.statusId === 3 ||
      payload.statusId === 5
    )
      if (payload.statusId === 5) {
        payload.paidAmount = 0;
      } else {
        payload.paidAmount =
          this.billStatusForm.get('paidAmount')?.value || null;
      }

    payload.paymentDate = this.billStatusForm.get('paymentDate')?.value || null;

    payload.paymentModeId =
      this.billStatusForm.get('paymentModeId')?.value || null;

    this.isSubmitProcessing = true;

    this.billService
      .updateBillStatus(this.billId, payload)
      .pipe(
        catchError((err) => {
          this.isSubmitProcessing = false;
          const response = err.error as CustomResult<string>;
          this.toastr.error(response.errors?.join(','), response.message);
          return of(response);
        })
      )
      .subscribe((res: CustomResult<string>) => {
        this.isSubmitProcessing = false;
        if (res.success) {
          this.router.navigate(['/bills', this.billId, 'view']);
          this.toastr.success(res.message, res.data!);
        } else {
          this.toastr.error(res.errors?.join(','), res.message);
        }
      });
  }
}
