import { Routes } from '@angular/router';
import { Agents } from './features/agents/agents';
import { Tickets } from './features/tickets/tickets';
import { TicketList } from './features/tickets/ticket-list/ticket-list';
import { TicketDetail } from './features/tickets/ticket-detail/ticket-detail';

export const routes: Routes = [
  {
    path: '',
    component: TicketList,
  },
  {
    path: 'ticket/:id',
    component: TicketDetail,
  },
  {
    path: 'agents',
    component: Agents,
  },
  {
    path: 'tickets',
    component: Tickets,
  },
];
