import { inject, Injectable } from '@angular/core';
import { GetItem } from '../../Models/Item/get-item';
import { CustomResult } from '../../Models/model.custom-result';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { CreateItem } from '../../Models/Item/create-item';
import { PutItem } from '../../Models/Item/put-item';

@Injectable({
  providedIn: 'root',
})
export class ItemService {
  private URL: string = environment.BACKEND_URL;
  private http = inject(HttpClient);
  items$ = this.getItems();

  getItems(): Observable<CustomResult<GetItem[]>> {
    return this.http.get<CustomResult<GetItem[]>>(`${this.URL}/items`);
  }

  getItemById(id: number): Observable<CustomResult<GetItem>> {
    return this.http.get<CustomResult<GetItem>>(`${this.URL}/items/${id}`);
  }

  createItem(request: CreateItem): Observable<CustomResult<GetItem>> {
    return this.http.post<CustomResult<GetItem>>(`${this.URL}/items`, request);
  }

  updateItemById(
    id: number,
    request: PutItem
  ): Observable<CustomResult<GetItem>> {
    return this.http.put<CustomResult<GetItem>>(
      `${this.URL}/items/${id}`,
      request
    );
  }

  deleteItem(id: number): Observable<CustomResult<string>> {
    return this.http.delete<CustomResult<string>>(`${this.URL}/items/${id}`);
  }
}
