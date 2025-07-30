import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoBusinessComponent } from './no-business.component';

describe('NoBusinessComponent', () => {
  let component: NoBusinessComponent;
  let fixture: ComponentFixture<NoBusinessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NoBusinessComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NoBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
