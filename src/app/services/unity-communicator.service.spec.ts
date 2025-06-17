import { TestBed } from '@angular/core/testing';

import { UnityCommunicatorService } from './unity-communicator.service';

describe('UnityCommunicatorService', () => {
  let service: UnityCommunicatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UnityCommunicatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
