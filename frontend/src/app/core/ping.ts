import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface PingResponse {
  message: string;
  time: string;
}

@Injectable({
  providedIn: 'root',
})
export class Ping {
  private readonly apiUrl = 'http://localhost:5048/api/ping';

  constructor(private http: HttpClient) {}

  getPing(): Observable<PingResponse> {
    return this.http.get<PingResponse>(this.apiUrl);
  }
}
