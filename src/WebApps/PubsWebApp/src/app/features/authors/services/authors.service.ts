import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { Author } from '../../../core/models/domain.models';

@Injectable({
  providedIn: 'root'
})
export class AuthorsService {
  private readonly apiUrl = `${environment.apiUrls.authors}/api/authors`;

  constructor(private http: HttpClient) { }

  getAll(skip: number = 0, take: number = 100): Observable<Author[]> {
    return this.http.get<Author[]>(`${this.apiUrl}?skip=${skip}&take=${take}`);
  }

  getById(id: string): Observable<Author> {
    return this.http.get<Author>(`${this.apiUrl}/${id}`);
  }

  create(author: Partial<Author>): Observable<string> {
    return this.http.post<string>(this.apiUrl, author);
  }

  update(id: string, author: Partial<Author>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, { ...author, id });
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
