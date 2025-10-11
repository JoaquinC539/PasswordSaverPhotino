import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditDBComponent } from './edit-db.component';

describe('EditDBComponent', () => {
  let component: EditDBComponent;
  let fixture: ComponentFixture<EditDBComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditDBComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditDBComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
