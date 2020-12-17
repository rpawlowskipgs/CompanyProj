import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { AddToBasketComponent } from './add-to-basket.component';

describe('AddToBasketComponent', () => {
  let component: AddToBasketComponent;
  let fixture: ComponentFixture<AddToBasketComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AddToBasketComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddToBasketComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
