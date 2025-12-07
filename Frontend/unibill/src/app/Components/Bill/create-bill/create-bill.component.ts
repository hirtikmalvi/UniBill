import { Component, inject, OnInit } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  Validators,
  ReactiveFormsModule,
  AbstractControl,
} from '@angular/forms';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { BillService } from '../../../Services/Bill/bill.service';
import { ItemService } from '../../../Services/Item/item.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CustomerService } from '../../../Services/Customer/customer.service';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { catchError, of } from 'rxjs';
import { GetCustomer } from '../../../Models/Customer/get-customer';
import { CreateCustomer } from '../../../Models/Customer/create-customer';
import { CreateBill } from '../../../Models/Bill/create-bill';
import { GetItem } from '../../../Models/Item/get-item';
import { CustomResult } from '../../../Models/model.custom-result';
import { GetBill } from '../../../Models/Bill/get-bill';

@Component({
  selector: 'app-create-bill',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, SpinnerComponent, CurrencyPipe],
  templateUrl: './create-bill.component.html',
  styleUrls: ['./create-bill.component.css'],
})
export class CreateBillComponent implements OnInit {
  private billsService = inject(BillService);
  private itemService = inject(ItemService);
  private customerService = inject(CustomerService);
  private toastr = inject(ToastrService);
  private router = inject(Router);

  // States
  isProcessing = false;
  isCustomerProcessing = false;
  isItemSearchProcessing = false;

  doCustomerExists = false;
  mobileIs10Digits = false;

  allItems: GetItem[] = [];
  filteredItems: GetItem[] = [];
  customer!: CustomResult<GetCustomer>;

  billForm!: FormGroup;
  customerForm!: FormGroup;

  private _subTotal = 0;
  private _taxAmount = 0;
  private _discountAmount = 0;
  private _finalAmount = 0;

  // -----------------------------------------------
  // INITIALIZATION
  // -----------------------------------------------
  ngOnInit(): void {
    this.initializeForms();
    this.handleCustomerMobileChanges();
    this.fetchItems();
  }

  // -----------------------------------------------
  // INITIALIZE FORMS
  // -----------------------------------------------
  initializeForms() {
    // CUSTOMER FORM
    this.customerForm = new FormGroup({
      mobileNumber: new FormControl(null, [
        Validators.required,
        Validators.pattern('^[0-9]{10}$'),
      ]),
      customerName: new FormControl(null),
      customerEmail: new FormControl(null, [Validators.email]),
    });

    // BILL FORM
    this.billForm = new FormGroup({
      date: new FormControl(
        new Date(new Date().getTime() + 5.5 * 60 * 60 * 1000).toISOString(),
        Validators.required
      ),
      customerId: new FormControl(0, Validators.required),
      discount: new FormControl(0, [Validators.min(0), Validators.max(100)]),
      tax: new FormControl(0, [Validators.min(0), Validators.max(100)]),
      labourCharges: new FormControl(0, [Validators.min(0)]),
      billItems: new FormArray([]),
    });

    // Recalculate totals when discount/tax/labour changes
    this.billForm
      .get('discount')
      ?.valueChanges.subscribe(() => this.calculateTotals());
    this.billForm
      .get('tax')
      ?.valueChanges.subscribe(() => this.calculateTotals());
    this.billForm
      .get('labourCharges')
      ?.valueChanges.subscribe(() => this.calculateTotals());
  }

  // -----------------------------------------------
  // LISTENER: CUSTOMER MOBILE CHANGES
  // -----------------------------------------------
  handleCustomerMobileChanges() {
    this.customerForm.get('mobileNumber')?.valueChanges.subscribe((value) => {
      if (!value || value.length < 10) {
        this.resetCustomerState();
        return;
      }

      if (value.length === 10) {
        this.mobileIs10Digits = true;
        this.fetchCustomer(value);
      }
    });
  }

  resetCustomerState() {
    this.mobileIs10Digits = false;
    this.doCustomerExists = false;
    this.customer = undefined!;
    this.billForm.patchValue({ customerId: 0 });

    // Remove validators for new customer fields
    this.customerForm.get('customerName')?.clearValidators();
    this.customerForm.get('customerEmail')?.clearValidators();
    this.customerForm.get('customerName')?.updateValueAndValidity();
    this.customerForm.get('customerEmail')?.updateValueAndValidity();
  }

  // -----------------------------------------------
  // API: FETCH CUSTOMER
  // -----------------------------------------------
  fetchCustomer(mobile: string) {
    this.isCustomerProcessing = true;

    this.customerService
      .getCustomerByMobileNumber(mobile)
      .pipe(
        catchError((err) => {
          this.isCustomerProcessing = false;
          const response = err.error as CustomResult<GetCustomer>;
          return of(response);
        })
      )
      .subscribe((res) => {
        this.customer = res;
        this.doCustomerExists = res.success;

        if (this.doCustomerExists) {
          this.billForm.patchValue({
            customerId: res.data!.customerId,
          });

          // Remove validators
          this.customerForm.get('customerName')?.clearValidators();
          this.customerForm.get('customerEmail')?.clearValidators();
        } else {
          this.billForm.patchValue({ customerId: 0 });

          // Add validators for new customer
          this.customerForm
            .get('customerName')
            ?.setValidators([Validators.required]);
          this.customerForm
            .get('customerEmail')
            ?.setValidators([Validators.email]);
        }

        this.customerForm.get('customerName')?.updateValueAndValidity();
        this.customerForm.get('customerEmail')?.updateValueAndValidity();

        this.isCustomerProcessing = false;
      });
  }

  // -----------------------------------------------
  // CREATE CUSTOMER
  // -----------------------------------------------
  onAddCustomer() {
    if (!this.customerForm.valid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    this.isCustomerProcessing = true;

    const newCustomer: CreateCustomer = {
      mobileNumber: this.customerForm.value.mobileNumber,
      customerName: this.customerForm.value.customerName,
      email: this.customerForm.value.customerEmail,
    };

    this.customerService
      .createCustomer(newCustomer)
      .pipe(
        catchError((err) => {
          this.isCustomerProcessing = false;
          const response = err.error as CustomResult<GetCustomer>;
          this.toastr.error(response.errors?.join('\n'), response.message);
          return of(response);
        })
      )
      .subscribe((res) => {
        if (res.success) {
          this.toastr.success(res.message, 'Customer Created');

          this.customer = res;
          this.doCustomerExists = true;

          this.billForm.patchValue({
            customerId: res.data!.customerId,
          });

          this.customerForm.patchValue({
            customerName: null,
            customerEmail: null,
          });
        }

        this.isCustomerProcessing = false;
      });
  }

  // -----------------------------------------------
  // BILL ITEMS - FORMARRAY HELPERS
  // -----------------------------------------------
  get billItems(): FormArray {
    return this.billForm.get('billItems') as FormArray;
  }

  addItemRow() {
    const billItemForm = new FormGroup({
      itemId: new FormControl(null, Validators.required),
      itemName: new FormControl('', Validators.required),
      quantity: new FormControl(1, [Validators.required, Validators.min(1)]),
      rate: new FormControl(0, [Validators.required, Validators.min(0)]),
      unit: new FormControl(''),
      category: new FormControl(''),
      itemType: new FormControl(''),
      total: new FormControl({ value: 0, disabled: true }),
      showDropdown: new FormControl(false),
    });

    this.billItems.push(billItemForm);

    this.calculateTotals();
  }

  removeBillItem(index: number) {
    this.billItems.removeAt(index);
    this.calculateTotals();
  }

  // -----------------------------------------------
  // FETCH ALL ITEMS
  // -----------------------------------------------
  fetchItems() {
    this.itemService.getItems().subscribe({
      next: (res) => {
        if (res.success) this.allItems = res.data ?? [];
      },
      error: (err) => {
        const response = err.error as CustomResult<GetItem[]>;
        this.toastr.error(response.errors?.join('\n'), response.message);
      },
    });
  }

  // -----------------------------------------------
  // ITEM SEARCH (AUTOCOMPLETE)
  // -----------------------------------------------
  onItemSearch(index: number) {
    const searchText: string =
      this.billItems.at(index).get('itemName')?.value?.trim().toLowerCase() ??
      '';

    if (!searchText) {
      this.filteredItems = [];
      return;
    }

    this.filteredItems = this.allItems.filter(
      (ai) =>
        ai.itemName.toLowerCase().includes(searchText) &&
        !this.billItems.value.some((bi: any) => bi.itemId === ai.itemId)
    );

    this.billItems.at(index).get('showDropdown')?.setValue(true);
  }

  // -----------------------------------------------
  // SELECT ITEM FROM DROPDOWN
  // -----------------------------------------------
  selectedItem(item: GetItem, index: number) {
    const row = this.billItems.at(index);

    row.patchValue({
      itemId: item.itemId,
      itemName: item.itemName,
      rate: item.itemRate,
      quantity: 1,
      unit: item.unit,
      category: item.category,
      itemType: item.itemType,
      total: item.itemRate,
      showDropdown: false,
    });

    this.updateAmount(index);
  }

  hideDropdown(index: number) {
    setTimeout(() => {
      this.billItems.at(index).get('showDropdown')?.setValue(false);
    }, 200);
  }

  // -----------------------------------------------
  // CALCULATE AMOUNT
  // -----------------------------------------------
  updateAmount(index: number) {
    const row = this.billItems.at(index);
    const qty = row.get('quantity')?.value || 0;
    const rate = row.get('rate')?.value || 0;

    row.get('total')?.setValue(qty * rate);
    this.calculateTotals();
  }

  // ---------- calculate live totals to match formula ----------
  calculateTotals() {
    const items = this.billItems.controls as FormGroup[];

    const subtotal = items.reduce((sum, row) => {
      const qty = Number(row.get('quantity')?.value) || 0;
      const rate = Number(row.get('rate')?.value) || 0;
      const total = qty * rate;
      return sum + total;
    }, 0);

    const discountPercent = Number(this.billForm.get('discount')?.value) || 0;
    const taxPercent = Number(this.billForm.get('tax')?.value) || 0;
    const labour = Number(this.billForm.get('labourCharges')?.value) || 0;

    const discountAmount = (discountPercent / 100) * subtotal;
    const afterDiscount = subtotal - discountAmount;
    const taxAmount = (taxPercent / 100) * subtotal;
    const finalTotal = subtotal - discountAmount + taxAmount + labour;

    // update private fields
    this._subTotal = this.roundTo2(subtotal);
    this._discountAmount = this.roundTo2(discountAmount);
    this._taxAmount = this.roundTo2(taxAmount);
    this._finalAmount = this.roundTo2(finalTotal);
  }

  // helper to round to 2 decimals (keeps UI neat)
  roundTo2(value: number) {
    return Math.round((value + Number.EPSILON) * 100) / 100;
  }

  // ---------- getters for template usage ----------
  get subTotal() {
    return this._subTotal;
  }
  get discountAmount() {
    return this._discountAmount;
  }
  get taxAmount() {
    return this._taxAmount;
  }
  get finalAmount() {
    return this._finalAmount;
  }

  // -----------------------------------------------
  // VALIDATION HELPER
  // -----------------------------------------------
  hasControlError(form: AbstractControl, control: string, error: string) {
    const c = form.get(control);
    return c && (c.dirty || c.touched) && c.hasError(error);
  }

  // -----------------------------------------------
  // SUBMIT BILL
  // -----------------------------------------------
  onSubmit() {
    if (!this.billForm.valid || this.billItems.length === 0) {
      this.billForm.markAllAsTouched();
      this.toastr.error('Please fill all fields & add at least 1 item.');
      return;
    }

    const request: CreateBill = {
      ...this.billForm.value,
      date: new Date(new Date().getTime() + 5.5 * 60 * 60 * 1000).toISOString(),
      billItems: this.billItems.value.map((x: any) => ({
        itemId: x.itemId,
        quantity: x.quantity,
        rate: x.rate,
      })),
    };

    this.isProcessing = true;

    this.billsService.createBill(request).subscribe({
      next: (res) => {
        if (res.success) {
          this.toastr.success(res.message, 'Bill Created');
          this.router.navigate(['/bills']);
        } else {
          this.toastr.error(res.errors?.join(', '), res.message);
        }
        this.isProcessing = false;
      },
      error: (err) => {
        const response = err.error as CustomResult<GetBill>;
        this.toastr.error(response.errors?.join(','), response.message);
        this.isProcessing = false;
      },
    });
  }
}
