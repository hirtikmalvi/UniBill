import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { hasBusinessGuard } from '../Business/has-business.guard';

describe('hasBusinessGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => hasBusinessGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});
