import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

interface Category {
  id: number;
  name: string;
  icon: string;
}

interface FoodItem {
  id: number;
  name: string;
  category: string;
  image: string;
  price: number;
  rating: number;
  veg: boolean;
}

@Component({
  selector: 'app-user-dashboard',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],
  templateUrl: './user-dashboard.html',
  styleUrl: './user-dashboard.scss',
})
export class UserDashboard {

  selectedCategory = 'All';

  categories: Category[] = [
    { id: 1, name: 'All', icon: 'apps' },
    { id: 2, name: 'Burger', icon: 'lunch_dining' },
    { id: 3, name: 'Pizza', icon: 'local_pizza' },
    { id: 4, name: 'Dessert', icon: 'cake' },
    { id: 5, name: 'Drinks', icon: 'local_bar' },
    { id: 6, name: 'Salad', icon: 'eco' }
  ];

  foods: FoodItem[] = [
    {
      id: 1,
      name: 'Classic Burger',
      category: 'Burger',
      image: 'https://images.unsplash.com/photo-1568901346375-23c9450c58cd',
      price: 12.99,
      rating: 4.8,
      veg: false
    },
    {
      id: 2,
      name: 'Farm Pizza',
      category: 'Pizza',
      image: 'https://images.unsplash.com/photo-1513104890138-7c749659a591',
      price: 14.99,
      rating: 4.9,
      veg: true
    },
    {
      id: 3,
      name: 'Chocolate Cake',
      category: 'Dessert',
      image: 'https://images.unsplash.com/photo-1578985545062-69928b1d9587',
      price: 8.99,
      rating: 4.7,
      veg: true
    },
    {
      id: 4,
      name: 'Fresh Mojito',
      category: 'Drinks',
      image: 'https://images.unsplash.com/photo-1578985545062-69928b1d9587',
      price: 6.99,
      rating: 4.6,
      veg: true
    }
  ];

  cart = [
    {
      name: 'Classic Burger',
      qty: 2,
      price: 12.99
    }
  ];

  get total() {
    return this.cart.reduce(
      (sum, item) => sum + item.price * item.qty,
      0
    );
  }

  addToCart(item: FoodItem) {
    console.log(item);
  }
}