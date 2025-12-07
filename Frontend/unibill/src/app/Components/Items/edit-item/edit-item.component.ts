import {
  Component,
  EventEmitter,
  inject,
  Input,
  OnChanges,
  Output,
  SimpleChanges,
} from '@angular/core';
import { ItemService } from '../../../Services/Item/item.service';
import { LookupService } from '../../../Services/Lookup/lookup.service';
import { ToastrService } from 'ngx-toastr';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { UnitByBusiness } from '../../../Models/Unit/unit-by-business';
import { ItemTypeByBusiness } from '../../../Models/ItemType/item-type-by-business';
import { Category } from '../../../Models/Category/category';
import { catchError, map, of } from 'rxjs';
import { CustomResult } from '../../../Models/model.custom-result';
import { CreateItem } from '../../../Models/Item/create-item';
import { NgFor, NgIf } from '../../../../../node_modules/@angular/common';
import { GetItem } from '../../../Models/Item/get-item';
import { PutItem } from '../../../Models/Item/put-item';
import { NgForOf } from '../../../../../node_modules/@angular/common/common_module.d-NEF7UaHr';

@Component({
  selector: 'app-edit-item',
  imports: [ReactiveFormsModule, NgIf, NgFor],
  templateUrl: './edit-item.component.html',
  styleUrl: './edit-item.component.css',
})
export class EditItemComponent implements OnChanges {
  private itemService = inject(ItemService);
  private lookupService = inject(LookupService);
  private toastr = inject(ToastrService);

  itemForm!: FormGroup;
  units!: UnitByBusiness[];
  itemTypes!: ItemTypeByBusiness[];
  categories!: Category[];
  isCategoryProcessing: boolean = false;
  isProcessing: boolean = false;
  existingItem!: CustomResult<GetItem>;
  updatedItem!: PutItem;

  @Input() itemId!: number;

  @Output() itemEdited = new EventEmitter<void>();

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['itemId'] && changes['itemId'].currentValue) {
      this.getItem();
    }
  }

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
      unitId: new FormControl(0, Validators.min(1)),
      itemTypeId: new FormControl(0, Validators.min(1)),
      categoryId: new FormControl(0),
    });

    this.itemForm.get('itemTypeId')?.valueChanges.subscribe((id: number) => {
      this.isCategoryProcessing = true;
      if (id && id != 0) {
        this.getCategoryByItemType(id);
      } else {
        this.categories = [];
        this.itemForm.patchValue({
          categoryId: 0,
        });
        this.isCategoryProcessing = false;
      }
    });
  }

  getItem(): void {
    if (!this.itemId) {
      return;
    }

    this.itemService
      .getItemById(this.itemId)
      .pipe(
        map((res) => {
          return res;
        }),
        catchError((err) => {
          const response = err.error as CustomResult<GetItem>;
          this.toastr.error(response.errors?.join('\n'), response.message);
          return of(response);
        })
      )
      .subscribe({
        next: (res) => {
          this.existingItem = res;

          // Resets form each time
          this.itemForm.reset({
            itemName: null,
            itemRate: null,
            unitId: 0,
            itemTypeId: 0,
            categoryId: 0,
          });

          if (this.existingItem?.data) {
            this.itemForm.patchValue({
              itemName: this.existingItem.data.itemName,
              itemRate: this.existingItem.data.itemRate,
              unitId: this.existingItem.data.unitId,
              itemTypeId: this.existingItem.data.itemTypeId,
            });
          }
        },
        error: () => {},
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
        if (this.existingItem?.data?.categoryId) {
          this.itemForm.patchValue({
            categoryId: this.existingItem.data.categoryId,
          });
        }
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

    const updatedItem = {
      itemId: this.itemId,
      ...this.itemForm.value,
      unitId: parseInt(this.itemForm.value.unitId),
      itemTypeId: parseInt(this.itemForm.value.itemTypeId),
      categoryId:
        this.itemForm.value.categoryId == 0
          ? null
          : this.itemForm.value.categoryId,
    } as PutItem;

    this.itemService.updateItemById(this.itemId, updatedItem).subscribe({
      next: (res) => {
        if (res.success) {
          this.toastr.success(res.message, 'Item Updated');
          this.itemForm.reset();
          this.itemEdited.emit(); // 🔑 notify parent
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
