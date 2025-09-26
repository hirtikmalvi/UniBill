import {
  Component,
  ElementRef,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ItemService } from '../../../Services/Item/item.service';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, of } from 'rxjs';
import { CustomResult } from '../../../Models/model.custom-result';
import { GetItem } from '../../../Models/Item/get-item';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { CreateItemComponent } from '../create-item/create-item.component';
import * as bootstrap from 'bootstrap';
import { Modal } from 'bootstrap';
import { EditItemComponent } from '../edit-item/edit-item.component';

@Component({
  selector: 'app-items',
  imports: [
    RouterModule,
    NgFor,
    NgIf,
    SpinnerComponent,
    CurrencyPipe,
    CreateItemComponent,
    EditItemComponent,
  ],
  templateUrl: './items.component.html',
  styleUrl: './items.component.css',
})
export class ItemsComponent implements OnInit {
  private itemService = inject(ItemService);
  private toastr = inject(ToastrService);
  isProcessing: boolean = false;
  result!: CustomResult<GetItem[]>;
  selectedItemToEdit: number = 0;
  @ViewChild(CreateItemComponent) createItemComponent!: CreateItemComponent;
  @ViewChild(EditItemComponent) editItemComponent!: EditItemComponent;

  ngOnInit(): void {
    this.fetchItems();
  }

  fetchItems() {
    this.isProcessing = true;

    this.itemService
      .getItems()
      .pipe(
        map((res) => {
          this.toastr.success(res.message, 'Items');
          return res;
        }),
        catchError((err) => {
          const response = err.error as CustomResult<GetItem[]>;
          this.toastr.error(response.errors?.join('\n'), response.message);
          return of(response);
        })
      )
      .subscribe({
        next: (res) => {
          this.result = res;
          this.isProcessing = false;
        },
        error: () => {
          this.isProcessing = false;
        },
      });
  }

  onSaveChanges_CreateItem() {
    this.createItemComponent.onSubmit();
  }

  onSaveChanges_EditItem() {
    this.editItemComponent.onSubmit();
  }

  handleItemCreated() {
    this.closeCreateItemModal();
    this.fetchItems();
  }

  handleItemEdited() {
    this.closeEditItemModal();
    this.fetchItems();
  }

  closeCreateItemModal() {
    const modalEl = document.getElementById('createItemModal');
    if (modalEl) {
      const modal = Modal.getInstance(modalEl) || new Modal(modalEl);

      modalEl.addEventListener(
        'hidden.bs.modal',
        () => {
          document.body.style.overflow = '';
          document.body.style.paddingRight = '';
        },
        {
          once: true,
        }
      );
      const backdrop = document.querySelector('.modal-backdrop');
      if (backdrop) {
        backdrop.remove();
      }
      modal.hide();
    }
  }

  closeEditItemModal() {
    const modalEl = document.getElementById('editItemModal');
    if (modalEl) {
      const modal = Modal.getInstance(modalEl) || new Modal(modalEl);

      modalEl.addEventListener(
        'hidden.bs.modal',
        () => {
          document.body.style.overflow = '';
          document.body.style.paddingRight = '';
        },
        {
          once: true,
        }
      );
      const backdrop = document.querySelector('.modal-backdrop');
      if (backdrop) {
        backdrop.remove();
      }
      modal.hide();
    }
    this.selectedItemToEdit = 0;
  }

  onEditItem(id: number) {
    this.selectedItemToEdit = id;
  }

  onDeleteItem(item: GetItem) {
    if (confirm(`Are you sure you want to delete "${item.itemName}"?`)) {
      this.itemService.deleteItem(item.itemId).subscribe({
        next: (res) => {
          if (res.success) {
            this.toastr.success(res.message, 'Deleted');
            this.fetchItems();
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
