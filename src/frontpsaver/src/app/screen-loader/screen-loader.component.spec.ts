import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScreenLoaderComponent } from './screen-loader.component';

describe('ScreenLoaderComponent', () => {
  let component: ScreenLoaderComponent;
  let fixture: ComponentFixture<ScreenLoaderComponent>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScreenLoaderComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ScreenLoaderComponent);
    component = fixture.componentInstance;
    compiled = fixture.nativeElement;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should not show "Cargando" by default', () => {
    const loadingElement = compiled.querySelector('.text-white');
    expect(loadingElement).toBeNull();
  });
  it("shoud show cargando if true",()=>{
    fixture.componentRef.setInput("loading",true)
    // component.loading=true;
    fixture.detectChanges();
    const loading=compiled.querySelector(".text-white");
    expect(loading).toBeTruthy();
    expect(loading?.textContent).toContain("Cargando")
  })
  it("Should update when input change",()=>{
    fixture.componentRef.setInput("loading",false)
    fixture.detectChanges();
    const loadingElement = compiled.querySelector('.text-white');
    expect(loadingElement).toBeNull();
    fixture.componentRef.setInput("loading",true)
    fixture.detectChanges()
    const loading=compiled.querySelector(".text-white");
    expect(loading).toBeTruthy();
    expect(loading?.textContent).toContain("Cargando")
  })
  
 
  
});
