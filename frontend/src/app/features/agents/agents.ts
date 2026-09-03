import { Component } from '@angular/core';
import { AgentsService, Agent } from '../../core/agents';

@Component({
  selector: 'app-agents',
  imports: [],
  templateUrl: './agents.html',
  styleUrl: './agents.css',
})
export class Agents {
  agentsList: Agent[] = [];

  constructor(private agentsService: AgentsService) {}

  ngOnInit() {
    this.loadAgents();
  }
  loadAgents() {
    this.agentsService.getAgents().subscribe({
      next: (agents) => {
        console.log(1);
        this.agentsList = agents;
      },
      error: (error) => {
        console.error('Error loading agents:', error);
      },
    });
  }
}
