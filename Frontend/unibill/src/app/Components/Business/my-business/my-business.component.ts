import { Component, inject, OnInit } from '@angular/core';
import { BusinessService } from '../../../Services/Business/business.service';
import { ToastrService } from 'ngx-toastr';
import { GetBusiness } from '../../../Models/Business/Get-Business';
import { catchError, map, of } from 'rxjs';
import { CustomResult } from '../../../Models/model.custom-result';
import { NgIf } from '@angular/common';
import { SpinnerComponent } from '../../Helper/spinner/spinner.component';
import { RouterModule } from '@angular/router';
import { NoBusinessComponent } from "../no-business/no-business.component";

@Component({
  selector: 'app-my-business',
  imports: [NgIf, SpinnerComponent, RouterModule, NoBusinessComponent],
  templateUrl: './my-business.component.html',
  styleUrl: './my-business.component.css',
})
export class MyBusinessComponent implements OnInit {
  businessService = inject(BusinessService);
  toastr = inject(ToastrService);
  myBusiness!: GetBusiness;
  isProcessing: boolean = false;
  hasBusiness: boolean = false;

  ngOnInit(): void {
    this.isProcessing = true;

    this.businessService.userHaveBusiness.subscribe({
      next: (res) => {
        this.hasBusiness = res;
        if (this.hasBusiness) {
          this.businessService
            .getBusiness()
            .pipe(
              map((res) => res),
              catchError((err) => {
                const response = err.error as CustomResult<GetBusiness>;
                return of(response);
              })
            )
            .subscribe({
              next: (res) => {
                if (res.success) {
                  this.myBusiness = res.data!;
                  this.toastr.success(res.message, 'My Business');
                } else {
                  this.toastr.error(res.errors?.join(' \n,'), res.message);
                }
                this.isProcessing = false;
              },
            });
        } else {
          this.isProcessing = false;
        }
      },
    });
  }
}
