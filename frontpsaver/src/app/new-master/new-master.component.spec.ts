import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewMasterComponent } from './new-master.component';

describe('NewMasterComponent', () => {
  let component: NewMasterComponent;
  let fixture: ComponentFixture<NewMasterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewMasterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewMasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
