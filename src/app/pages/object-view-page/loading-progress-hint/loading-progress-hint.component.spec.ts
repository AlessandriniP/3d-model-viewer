import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadingProgressHintComponent } from './loading-progress-hint.component';

describe('LoadingProgressHintComponent', () => {
  let component: LoadingProgressHintComponent;
  let fixture: ComponentFixture<LoadingProgressHintComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoadingProgressHintComponent]
    })
      .compileComponents();

    fixture = TestBed.createComponent(LoadingProgressHintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
