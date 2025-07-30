import { Component, inject, OnInit } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { HeaderComponent } from './Components/header/header.component';
import { FooterComponent } from './Components/footer/footer.component';
import { BusinessService } from './Services/Business/business.service';
import { AuthService } from './Services/Auth/auth.service';
FooterComponent;

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent, FooterComponent, RouterModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'unibill';
  businessService = inject(BusinessService);
  authService = inject(AuthService);

  ngOnInit(): void {
    this.authService.ValidateUserAndUpdateStatus();
    this.authService.userStatus.subscribe((res) => {
      if (res) {
        this.businessService.hasBusinessAndUpdateStatus();
      }
    });
  }
}
