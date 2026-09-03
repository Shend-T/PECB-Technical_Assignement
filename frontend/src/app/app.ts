import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { Ping } from './core/ping';
import { AgentsService, Agent, CreateAgent } from './core/agents';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('frontend');

  message = '';

  agentsList: Agent[] = [];

  constructor(
    private ping: Ping,
    private agents: AgentsService,
  ) {}

  testConnection() {
    this.ping.getPing().subscribe((response) => {
      this.message = response.message;
    });
  }

  getAgents() {
    this.agents.getAgents().subscribe({
      next: (agents) => {
        this.agentsList = agents;
      },
      error: (error) => {
        console.error('Error:', error);
      },
    });
  }

  createAgent() {
    const agent: CreateAgent = {
      fullName: 'Test Test',
      email: 'test@test.com',
      department: 'Technical',
      active: true,
    };

    this.agents.createAgent(agent).subscribe({
      next: (response) => {
        console.log('Agjendi u krijua:', response);
      },
      error: (error) => {
        console.error('Error:', error);
      },
    });
  }
}
