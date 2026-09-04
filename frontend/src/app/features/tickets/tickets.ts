import { Component, ChangeDetectorRef } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

import { TicketsService, Ticket, CreateTicket, UpdateTicket } from '../../core/tickets';

@Component({
  selector: 'app-tickets',
  imports: [ReactiveFormsModule],
  templateUrl: './tickets.html',
  styleUrl: './tickets.css',
})
export class Tickets {
  ticketsList: Ticket[] = [];
  ticketForm;
  ticketId: number | null = null;
  errorMessage = '';

  constructor(
    private ticketsService: TicketsService,
    private cdr: ChangeDetectorRef,
    private formBuilder: FormBuilder,
  ) {
    this.ticketForm = this.formBuilder.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      customerName: ['', Validators.required],
      customerEmail: ['', [Validators.required, Validators.email]],
      priority: ['Normal', Validators.required],
      status: ['New', Validators.required],
      assignedAgentId: [null as number | null],
    });
  }

  ngOnInit() {
    this.loadTickets();
  }
  loadTickets() {
    this.ticketsService.getTickets().subscribe({
      next: (response) => {
        this.ticketsList = response.items;
        this.cdr.detectChanges();
      },
      error: (error) => {
        this.errorMessage = error.error.message;
        this.cdr.detectChanges();
      },
    });
  }

  submitForm() {
    if (this.ticketForm.invalid) {
      this.ticketForm.markAllAsTouched();
      return;
    }

    if (this.ticketId === null) {
      this.createTicket();
    } else {
      this.updateTicket();
    }
  }

  createTicket() {
    const ticket: CreateTicket = {
      title: this.ticketForm.value.title!,
      description: this.ticketForm.value.description!,
      customerName: this.ticketForm.value.customerName!,
      customerEmail: this.ticketForm.value.customerEmail!,
      priority: this.ticketForm.value.priority as CreateTicket['priority'],
      assignedAgentId: this.ticketForm.value.assignedAgentId ?? null,
    };

    this.ticketsService.createTicket(ticket).subscribe({
      next: () => {
        this.resetForm();
        this.loadTickets();
        window.location.reload();
      },
      error: (error) => {
        console.error('Error:', error.error.message);
        this.errorMessage = error.error.message;
        this.cdr.detectChanges();
      },
    });
  }

  updateTicketForm(ticket: Ticket) {
    this.ticketId = ticket.id;

    this.ticketForm.patchValue({
      title: ticket.title,
      customerName: ticket.customerName,
      customerEmail: ticket.customerEmail,
      priority: ticket.priority,
      status: ticket.status,
      assignedAgentId: ticket.assignedAgentId,
      description: ticket.description,
    });
  }

  updateTicket() {
    if (this.ticketId === null) {
      return;
    }

    const ticket: UpdateTicket = {
      title: this.ticketForm.value.title!,
      description: this.ticketForm.value.description!,
      customerName: this.ticketForm.value.customerName!,
      customerEmail: this.ticketForm.value.customerEmail!,
      priority: this.ticketForm.value.priority as UpdateTicket['priority'],
      status: this.ticketForm.value.status as UpdateTicket['status'],
      assignedAgentId: this.ticketForm.value.assignedAgentId ?? null,
    };

    this.ticketsService.updateTicket(this.ticketId, ticket).subscribe({
      next: (ticket) => {
        this.resetForm();
        this.loadTickets();
        window.location.reload();
      },
      error: (error) => {
        console.error('Error:', error);
        this.errorMessage = error.error.message;
        this.cdr.detectChanges();
      },
    });
  }

  deleteTicket(id: number) {
    const confirmed = window.confirm('A jeni sigurt per fshirjen e tiketes?');

    if (!confirmed) {
      return;
    }

    this.ticketsService.deleteTicket(id).subscribe({
      next: () => {
        this.loadTickets();
        this.cdr.detectChanges();
        window.location.reload();
      },
      error: (error) => {
        this.errorMessage = error.error.message;
        this.cdr.detectChanges();
      },
    });
  }

  resetForm() {
    this.ticketId = null;

    this.ticketForm.reset({
      title: '',
      description: '',
      customerName: '',
      customerEmail: '',
      priority: 'Normal',
      status: 'New',
      assignedAgentId: null,
    });
  }
}
