import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-publishers-list',
  standalone: true,
  imports: [CommonModule, MatCardModule],
  templateUrl: './publishers-list.component.html',
  styleUrl: './publishers-list.component.scss'
})
export class PublishersListComponent {

}
