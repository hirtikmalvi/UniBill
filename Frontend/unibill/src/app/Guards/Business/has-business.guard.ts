import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { BusinessService } from '../../Services/Business/business.service';
import { catchError, map, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { CustomResult } from '../../Models/model.custom-result';

export const hasBusinessGuard: CanActivateFn = (route, state) => {
  const businessService = inject(BusinessService);
  const toastr = inject(ToastrService);
  const router = inject(Router);

  return businessService.hasBusiness().pipe(
    map((res) => {
      return res.success;
    }),
    catchError((err) => {
      var response = err.error as CustomResult<boolean>;
      toastr.error(response.errors?.join(','), response.message);
      router.navigate(['business/register-business']);
      return of(response.success);
    })
  );
};
