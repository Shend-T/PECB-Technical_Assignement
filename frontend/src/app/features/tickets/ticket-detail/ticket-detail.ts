import { Component, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { TicketsService, Ticket } from '../../../core/tickets';
import { AgentsService, Agent } from '../../../core/agents';

@Component({
  selector: 'app-ticket-detail',
  imports: [DatePipe, FormsModule],
  templateUrl: './ticket-detail.html',
  styleUrl: './ticket-detail.css',
})
export class TicketDetail {
  ticket: Ticket | null = null;
  agents: Agent[] = [];

  selectedAgentId: number | null = null;
  assignmentError = '';

  errorMessage = '';

  commentAuthor = '';
  commentBody = '';

  commentError = '';

  statusError = '';

  constructor(
    private route: ActivatedRoute,
    private ticketsService: TicketsService,
    private cdr: ChangeDetectorRef,
    private agentsService: AgentsService,
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.loadAgents();
    this.loadTicket(id);
    this.cdr.detectChanges();
  }

  loadTicket(id: number) {
    this.errorMessage = '';

    this.ticketsService.getTicket(id).subscribe({
      next: (ticket) => {
        this.ticket = ticket;
        this.selectedAgentId = ticket.assignedAgentId;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage = error.error?.message ?? 'Error gjate marrjes se tiketes.';
        this.cdr.detectChanges();
      },
    });
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

  assignAgent() {
    if (!this.ticket) return;

    this.assignmentError = '';

    this.ticketsService.assignAgent(this.ticket.id, this.selectedAgentId).subscribe({
      next: (ticket) => {
        this.ticket = ticket;
      },
      error: (error) => {
        this.assignmentError = error.error?.message ?? 'Error gjate caktimit te agjendit.';
      },
    });
  }

  addComment() {
    this.commentError = '';

    if (!this.ticket) return;

    if (!this.commentAuthor.trim() || !this.commentBody.trim()) {
      this.commentError = 'Autori dhe komenti duhen mbushur';
      return;
    }

    this.ticketsService
      .addComment(this.ticket.id, {
        authorName: this.commentAuthor,
        body: this.commentBody,
      })
      .subscribe({
        next: () => {
          this.commentBody = '';

          this.loadTicket(this.ticket!.id);
          this.cdr.detectChanges();
        },
        error: (error) => {
          this.errorMessage = error.error?.message ?? 'Error gjate shtimit te komentit.';
          this.cdr.detectChanges();
        },
      });
  }

  changeStatus(status: Ticket['status']) {
    if (!this.ticket) return;

    this.statusError = '';

    this.ticketsService.changeStatus(this.ticket.id, status).subscribe({
      next: (ticket) => {
        this.ticket = ticket;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.statusError = error.error?.message ?? 'Error gjate ndrrimit te statusit.';
        this.cdr.detectChanges();
      },
    });
  }
}
