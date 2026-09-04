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

export interface TicketListResponse {
  items: Ticket[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

@Injectable({
  providedIn: 'root',
})
export class TicketsService {
  private apiUrl = 'http://localhost:5048/api/tickets';

  constructor(private http: HttpClient) {}

  getTickets(
    page = 1,
    pageSize = 10,
    search = '',
    status = '',
    priority = '',
    assignedAgentId: number | null = null,
    overdueOnly = false,
  ): Observable<TicketListResponse> {
    const params: any = {
      page,
      pageSize,
    };

    if (search) {
      params.search = search;
    }

    if (status) {
      params.status = status;
    }

    if (priority) {
      params.priority = priority;
    }

    if (assignedAgentId !== null) {
      params.assignedAgentId = assignedAgentId;
    }

    if (overdueOnly) {
      params.overdueOnly = true;
    }

    return this.http.get<TicketListResponse>(this.apiUrl, { params });
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
