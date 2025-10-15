import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AuthorsService } from '../../services/authors.service';
import { Author } from '../../../../core/models/domain.models';

@Component({
  selector: 'app-authors-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatTooltipModule
  ],
  templateUrl: './authors-list.component.html',
  styleUrl: './authors-list.component.scss'
})
export class AuthorsListComponent implements OnInit {
  authors: Author[] = [];
  loading = false;
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'phone', 'city', 'state', 'contract', 'actions'];

  constructor(private authorsService: AuthorsService) { }

  ngOnInit(): void {
    this.loadAuthors();
  }

  loadAuthors(): void {
    this.loading = true;
    this.authorsService.getAll().subscribe({
      next: (authors) => {
        this.authors = authors;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading authors:', error);
        this.loading = false;
      }
    });
  }

  editAuthor(author: Author): void {
    console.log('Edit author:', author);
  }

  deleteAuthor(author: Author): void {
    if (confirm(`Are you sure you want to delete ${author.firstName} ${author.lastName}?`)) {
      this.authorsService.delete(author.id).subscribe({
        next: () => {
          this.loadAuthors();
        },
        error: (error) => {
          console.error('Error deleting author:', error);
        }
      });
    }
  }
}
