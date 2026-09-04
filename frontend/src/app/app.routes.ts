import { Routes } from '@angular/router';
import { Agents } from './features/agents/agents';
import { Tickets } from './features/tickets/tickets';
import { TicketList } from './features/tickets/ticket-list/ticket-list';

export const routes: Routes = [
  {
    path: '',
    component: TicketList,
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
