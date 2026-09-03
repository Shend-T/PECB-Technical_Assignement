import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Agent {
  id: number;
  fullName: string;
  email: string;
  department: 'General' | 'Technical' | 'Billing';
  active: boolean;
}

export interface CreateAgent {
  fullName: string;
  email: string;
  department: 'General' | 'Technical' | 'Billing';
  active: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class Agents {
  private apiUrl = 'http://localhost:5048/api/agents';

  constructor(private http: HttpClient) {}

  getAgents(): Observable<Agent[]> {
    return this.http.get<Agent[]>(this.apiUrl);
  }

  createAgent(agent: CreateAgent): Observable<Agent> {
    return this.http.post<Agent>(this.apiUrl, agent);
  }
}
