import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

import { TicketsService, Ticket } from './tickets';

describe('TicketsService', () => {
  let service: TicketsService;
  let httpMock: HttpTestingController;

  const mockTicket: Ticket = {
    id: 1,
    referenceId: 'TCK-2026-0001',
    title: 'Nuk mund tbej `log in`',
    description: 'Nuk kam qasje ne account.',
    customerName: 'Test User',
    customerEmail: 'test@test.com',
    priority: 'High',
    status: 'New',
    assignedAgentId: null,
    assignedAgent: null,
    createdDate: '2026-09-04T10:00:00Z',
    lastModifiedDate: '2026-09-04T10:00:00Z',
    resolvedDate: null,
    closedDate: null,
    dueDate: '2026-09-05T10:00:00Z',
    isOverdue: false,
    comments: [],
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TicketsService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(TicketsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should get a ticket by id', () => {
    service.getTicket(1).subscribe((ticket) => {
      expect(ticket).toEqual(mockTicket);
    });

    const request = httpMock.expectOne('http://localhost:5048/api/tickets/1');

    expect(request.request.method).toBe('GET');

    request.flush(mockTicket);
  });

  it('should change the status of a ticket', () => {
    service.changeStatus(1, 'InProgress').subscribe((ticket) => {
      expect(ticket.status).toBe('InProgress');
    });

    const request = httpMock.expectOne('http://localhost:5048/api/tickets/1/status');

    expect(request.request.method).toBe('PUT');

    expect(request.request.body).toEqual({
      status: 'InProgress',
    });

    request.flush({
      ...mockTicket,
      status: 'InProgress',
    });
  });
});
