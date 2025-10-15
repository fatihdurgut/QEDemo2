import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { AuthService } from '../../../../auth/services/auth.service';

interface DashboardCard {
  title: string;
  icon: string;
  route: string;
  color: string;
  description: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatGridListModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  currentUser$ = this.authService.currentUser$;

  dashboardCards: DashboardCard[] = [
    {
      title: 'Authors',
      icon: 'person',
      route: '/authors',
      color: '#3f51b5',
      description: 'Manage author information and contracts'
    },
    {
      title: 'Publishers',
      icon: 'business',
      route: '/publishers',
      color: '#00bcd4',
      description: 'Manage publisher details and locations'
    },
    {
      title: 'Titles',
      icon: 'book',
      route: '/titles',
      color: '#4caf50',
      description: 'Manage book titles and publications'
    },
    {
      title: 'Sales',
      icon: 'shopping_cart',
      route: '/sales',
      color: '#ff9800',
      description: 'Track sales and orders'
    },
    {
      title: 'Employees',
      icon: 'badge',
      route: '/employees',
      color: '#9c27b0',
      description: 'Manage employee records and positions'
    },
    {
      title: 'Analytics',
      icon: 'analytics',
      route: '/analytics',
      color: '#f44336',
      description: 'View reports and analytics'
    }
  ];

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }

  logout(): void {
    this.authService.logout();
  }
}
