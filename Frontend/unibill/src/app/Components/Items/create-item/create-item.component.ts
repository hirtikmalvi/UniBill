import { Component, EventEmitter, inject, OnInit, Output } from '@angular/core';
import {
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UnitByBusiness } from '../../../Models/Unit/unit-by-business';
import { ItemService } from '../../../Services/Item/item.service';
import { LookupService } from '../../../Services/Lookup/lookup.service';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, of } from 'rxjs';
import { CustomResult } from '../../../Models/model.custom-result';
import { NgFor, NgIf } from '@angular/common';
import { ItemTypeByBusiness } from '../../../Models/ItemType/item-type-by-business';
import { Category } from '../../../Models/Category/category';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { Router, RouterModule } from '@angular/router';
import { CreateItem } from '../../../Models/Item/create-item';

@Component({
  selector: 'app-create-item',
  imports: [
    ReactiveFormsModule,
    FormsModule,
    NgFor,
    NgIf,
    SpinnerComponent,
    RouterModule,
  ],
  templateUrl: './create-item.component.html',
  styleUrl: './create-item.component.css',
})
export class CreateItemComponent implements OnInit {
  private itemService = inject(ItemService);
  private lookupService = inject(LookupService);
  private toastr = inject(ToastrService);

  itemForm!: FormGroup;
  units!: UnitByBusiness[];
  itemTypes!: ItemTypeByBusiness[];
  categories!: Category[];
  isCategoryProcessing: boolean = false;
  isProcessing: boolean = false;

  /** 🔑 Event to notify parent when item is created */
  @Output() itemCreated = new EventEmitter<void>();

  ngOnInit(): void {
    this.getUnits();
    this.getItemTypes();

    this.itemForm = new FormGroup({
      itemName: new FormControl(null, [
        Validators.required,
        Validators.minLength(1),
        Validators.maxLength(200),
      ]),
      itemRate: new FormControl(null, [Validators.required, Validators.min(0)]),
      unitId: new FormControl(0, Validators.required),
      itemTypeId: new FormControl(0, Validators.required),
      categoryId: new FormControl(0),
    });

    this.itemForm.get('itemTypeId')?.valueChanges.subscribe((id: number) => {
      this.isCategoryProcessing = true;
      if (id && id != 0) {
        this.getCategoryByItemType(id);
      } else {
        this.categories = [];
        this.isCategoryProcessing = false;
      }
    });
  }

  getUnits(): void {
    this.lookupService
      .getUnitsByBusiness()
      .pipe(
        map((res) => res),
        catchError((err) => {
          const response = err.error as CustomResult<UnitByBusiness[]>;
          this.toastr.error(response.errors?.join(', '), response.message);
          return of(response);
        })
      )
      .subscribe((res) => {
        this.units = res.data ?? [];
      });
  }

  getItemTypes(): void {
    this.lookupService
      .getItemTypesByBusiness()
      .pipe(
        map((res) => res),
        catchError((err) => {
          const response = err.error as CustomResult<ItemTypeByBusiness[]>;
          this.toastr.error(response.errors?.join(', '), response.message);
          return of(response);
        })
      )
      .subscribe((res) => {
        this.itemTypes = res.data ?? [];
      });
  }

  getCategoryByItemType(id: number): void {
    this.lookupService
      .getCategoriesByItemType(id)
      .pipe(
        map((res) => res),
        catchError((err) => {
          const response = err.error as CustomResult<Category[]>;
          this.toastr.error(response.errors?.join(', '), response.message);
          this.isCategoryProcessing = false;
          return of(response);
        })
      )
      .subscribe((res) => {
        this.categories = res.data ?? [];
        this.isCategoryProcessing = false;
      });
  }

  hasControlError(controlName: string, errorName: string): boolean | undefined {
    const control = this.itemForm.get(controlName);
    return (control?.dirty || control?.touched) && control?.hasError(errorName);
  }

  onSubmit(): void {
    if (!this.itemForm.valid) {
      this.itemForm.markAllAsTouched();
      return;
    }
    this.isProcessing = true;

    const itemToCreate = {
      ...this.itemForm.value,
      categoryId:
        this.itemForm.value.categoryId == 0
          ? null
          : this.itemForm.value.categoryId,
    } as CreateItem;

    this.itemService.createItem(itemToCreate).subscribe({
      next: (res) => {
        if (res.success) {
          this.toastr.success(res.message, 'Item Created');
          this.itemForm.reset();
          this.itemCreated.emit(); // 🔑 notify parent
        } else {
          this.toastr.error(res.errors?.join(', '), res.message);
        }
        this.isProcessing = false;
      },
      error: (err) => {
        const response = err.error as CustomResult<any>;
        this.toastr.error(response.errors?.join(', '), response.message);
      },
    });
  }
}
