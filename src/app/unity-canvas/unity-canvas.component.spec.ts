import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UnityCanvasComponent } from './unity-canvas.component';

describe('UnityCanvasComponent', () => {
  let component: UnityCanvasComponent;
  let fixture: ComponentFixture<UnityCanvasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UnityCanvasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UnityCanvasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
