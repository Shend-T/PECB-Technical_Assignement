import { Routes } from '@angular/router';
import { Agents } from './features/agents/agents';
import { Tickets } from './features/tickets/tickets';

export const routes: Routes = [
  {
    path: '',
    component: Agents,
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
