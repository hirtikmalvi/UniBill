import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../Services/Auth/auth.service';
import { NgIf } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { BusinessService } from '../../Services/Business/business.service';

@Component({
  selector: 'app-header',
  imports: [RouterModule, NgIf],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css',
})
export class HeaderComponent implements OnInit {
  authService = inject(AuthService);
  businessService = inject(BusinessService);
  router = inject(Router);
  toastr = inject(ToastrService);
  isValidUser: boolean = false;
  hasBusiness: boolean = false;

  ngOnInit(): void {
    this.authService.userStatus.subscribe({
      next: (res) => {
        this.isValidUser = res;
      },
    });
    this.businessService.userHaveBusiness.subscribe({
      next: (res) => {
        this.hasBusiness = res;
      },
    });
  }

  onBusinessClick() {
    if (this.hasBusiness) {
      this.router.navigate(['business/my-business']);
    } else {
      this.router.navigate(['business/register-business']);
    }
  }

  onLogout() {
    this.authService.logoutUser();
    this.businessService.hasBusinessAndUpdateStatus();
    this.toastr.success('User logged out successfully.', 'Logout');
    this.router.navigate(['auth/login']);
  }
}
