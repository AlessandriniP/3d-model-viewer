import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ObjectViewPageComponent } from './object-view-page.component';

describe('ObjectViewPageComponent', () => {
  let component: ObjectViewPageComponent;
  let fixture: ComponentFixture<ObjectViewPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ObjectViewPageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ObjectViewPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
