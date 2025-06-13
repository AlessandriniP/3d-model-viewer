import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CanvasControlsComponent } from './canvas-controls.component';

describe('CanvasControlsComponent', () => {
  let component: CanvasControlsComponent;
  let fixture: ComponentFixture<CanvasControlsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CanvasControlsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CanvasControlsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
