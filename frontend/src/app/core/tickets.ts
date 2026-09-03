import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Agent } from './agents';

export interface Ticket {
  id: number;
  referenceId: string;

  title: string;
  description: string;

  customerName: string;
  customerEmail: string;

  priority: 'Critical' | 'High' | 'Normal' | 'Low';
  status: 'New' | 'InProgress' | 'Resolved' | 'Closed';

  createdDate: string;
  lastModifiedDate: string;
  resolvedDate: string | null;
  closedDate: string | null;
  dueDate: string;

  assignedAgentId: number | null;
  assignedAgent: Agent | null;

  isOverdue: boolean;
}

export interface CreateTicket {
  title: string;
  description: string;
  customerName: string;
  customerEmail: string;

  priority: 'Critical' | 'High' | 'Normal' | 'Low';

  assignedAgentId: number | null;
}

export interface UpdateTicket {
  title: string;
  description: string;
  customerName: string;
  customerEmail: string;

  priority: 'Critical' | 'High' | 'Normal' | 'Low';
  status: 'New' | 'InProgress' | 'Resolved' | 'Closed';

  assignedAgentId: number | null;
}

@Injectable({
  providedIn: 'root',
})
export class TicketsService {
  private apiUrl = 'http://localhost:5048/api/tickets';

  constructor(private http: HttpClient) {}

  getTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(this.apiUrl);
  }

  getTicket(id: number): Observable<Ticket> {
    return this.http.get<Ticket>(`${this.apiUrl}/${id}`);
  }

  createTicket(ticket: CreateTicket): Observable<Ticket> {
    return this.http.post<Ticket>(this.apiUrl, ticket);
  }

  updateTicket(id: number, ticket: UpdateTicket): Observable<Ticket> {
    return this.http.put<Ticket>(`${this.apiUrl}/${id}`, ticket);
  }

  deleteTicket(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
