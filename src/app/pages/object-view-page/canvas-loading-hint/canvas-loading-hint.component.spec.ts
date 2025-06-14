import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CanvasLoadingHintComponent } from './canvas-loading-hint.component';

describe('CanvasLoadingHintComponent', () => {
  let component: CanvasLoadingHintComponent;
  let fixture: ComponentFixture<CanvasLoadingHintComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CanvasLoadingHintComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CanvasLoadingHintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
