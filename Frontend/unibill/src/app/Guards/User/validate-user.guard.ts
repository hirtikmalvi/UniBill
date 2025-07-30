import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../../Services/Auth/auth.service';
import { CustomResult } from '../../Models/model.custom-result';
import { catchError, map, of, Subject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

export const validateUserGuard: CanActivateFn = (route, state) => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const toastr = inject(ToastrService);

  return authService.validUser().pipe(
    map((res) => {
      authService.setUserStatus(res.success);
      return true;
    }),
    catchError((err) => {
      const response = err.error as CustomResult<string>;

      authService.setUserStatus(response.success);

      toastr.error(response.errors?.join(','), response.message, {
        timeOut: 1500,
      });

      toastr.info('Redirecting to Login Page...', 'Redirecting', {
        timeOut: 1500,
      });

      setTimeout(() => {
        router.navigate(['/auth/login']);
      }, 1000);
      return of(false);
    })
  );
};
