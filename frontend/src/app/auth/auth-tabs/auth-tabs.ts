import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserAuth } from '../user-auth/user-auth';
import { RestaurantAuth } from '../restaurant-auth/restaurant-auth';

@Component({
  selector: 'app-auth-tabs',
  standalone: true,
  imports: [
    CommonModule,
    UserAuth,
    RestaurantAuth
  ],
  templateUrl: './auth-tabs.html'
})
export class AuthTabs {

  selectedTab: number = 0;

}