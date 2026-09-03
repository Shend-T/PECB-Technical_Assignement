import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';

import { AgentsService, Agent, CreateAgent, UpdateAgent } from '../../core/agents';

@Component({
  selector: 'app-agents',
  imports: [ReactiveFormsModule],
  templateUrl: './agents.html',
  styleUrl: './agents.css',
})
export class Agents {
  agentsList: Agent[] = [];

  agentForm;

  constructor(
    private agentsService: AgentsService,
    private formBuilder: FormBuilder,
  ) {
    this.agentForm = this.formBuilder.group({
      fullName: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email]],
      department: ['General' as 'General' | 'Technical' | 'Billing'],
      active: [false],
    });
  }

  agentId: number | null = null;

  ngOnInit() {
    this.loadAgents();
  }
  loadAgents() {
    console.log(2);
    this.agentsService.getAgents().subscribe({
      next: (agents) => {
        this.agentsList = agents;
        console.log(this.agentsList);
      },
      error: (error) => {
        console.error('Error:', error);
      },
    });
  }

  submitForm() {
    if (this.agentForm.invalid) {
      this.agentForm.markAllAsTouched();
      return;
    }

    if (this.agentId === null) {
      this.createAgent();
    } else {
      this.updateAgent();
    }
  }

  createAgent() {
    if (this.agentForm.invalid) {
      this.agentForm.markAllAsTouched();
      return;
    }

    const agent: CreateAgent = {
      fullName: this.agentForm.value.fullName!,
      email: this.agentForm.value.email!,
      department: this.agentForm.value.department!,
      active: this.agentForm.value.active!,
    };

    console.log(1);

    this.agentsService.createAgent(agent).subscribe({
      next: () => {
        this.loadAgents();
        this.resetForm();
        window.location.reload();
      },
      error: (error) => {
        console.error('Error:', error);
      },
    });
  }

  updateAgentForm(agent: Agent) {
    this.agentId = agent.id;

    this.agentForm.patchValue({
      fullName: agent.fullName,
      email: agent.email,
      department: agent.department,
      active: agent.active,
    });
  }

  updateAgent() {
    const agent: UpdateAgent = {
      fullName: this.agentForm.value.fullName!,
      email: this.agentForm.value.email!,
      department: this.agentForm.value.department!,
      active: this.agentForm.value.active!,
    };

    this.agentsService.updateAgent(this.agentId!, agent).subscribe({
      next: () => {
        this.resetForm();
        window.location.reload();
      },
      error: (error) => {
        console.error('Error:', error);
      },
    });
  }

  deleteAgent(id: number) {
    this.agentsService.deleteAgent(id).subscribe({
      next: () => {
        window.location.reload();
      },
      error: (error) => {
        console.error('Error:', error);
      },
    });
  }

  resetForm() {
    this.agentId = null;

    this.agentForm.reset({
      fullName: '',
      email: '',
      department: 'General',
      active: false,
    });
  }
}
