import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

import { TicketsService, Ticket } from '../../../core/tickets';
import { AgentsService, Agent } from '../../../core/agents';

@Component({
  selector: 'app-ticket-list',
  imports: [DatePipe, FormsModule],
  templateUrl: './ticket-list.html',
  styleUrl: './ticket-list.css',
})
export class TicketList {
  ticketsList: Ticket[] = [];
  agents: Agent[] = [];

  search = '';
  status = '';
  priority = '';
  assignedAgentId: number | null = null;
  overdueOnly = false;

  page = 1;
  pageSize = 10;

  totalCount = 0;
  totalPages = 0;

  loading = false;
  errorMessage = '';

  private searchSubject = new Subject<string>();

  constructor(
    private ticketsService: TicketsService,
    private agentsService: AgentsService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.loadAgents();

    this.searchSubject.pipe(debounceTime(400)).subscribe(() => {
      this.page = 1;
      this.loadTickets();
      this.cdr.detectChanges();
    });

    this.loadTickets();
    this.cdr.detectChanges();
  }

  loadAgents() {
    this.agentsService.getAgents().subscribe({
      next: (agents) => {
        this.agents = agents;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage = error.error?.message ?? 'Error gjate marrjes se agjendeve.';
        this.cdr.detectChanges();
      },
    });
  }

  onSearchChange() {
    this.searchSubject.next(this.search);
  }

  loadTickets() {
    this.errorMessage = '';

    this.ticketsService
      .getTickets(
        this.page,
        this.pageSize,
        this.search,
        this.status,
        this.priority,
        this.assignedAgentId,
        this.overdueOnly,
      )
      .subscribe({
        next: (response) => {
          this.ticketsList = response.items;
          this.totalCount = response.totalCount;
          this.totalPages = response.totalPages;
          this.cdr.detectChanges();
        },
        error: (error) => {
          this.errorMessage = error.error?.message ?? 'Error gjate marrjes se tiketave.';
          this.cdr.detectChanges();
        },
      });
  }
  searchTickets() {
    this.page = 1;
    this.loadTickets();
    this.cdr.detectChanges();
  }
  clearFilters() {
    this.search = '';
    this.status = '';
    this.priority = '';
    this.assignedAgentId = null;
    this.overdueOnly = false;
    this.page = 1;

    this.loadTickets();
    this.cdr.detectChanges();
  }
  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) {
      return;
    }

    this.page = page;
    this.loadTickets();
    this.cdr.detectChanges();
  }
}
