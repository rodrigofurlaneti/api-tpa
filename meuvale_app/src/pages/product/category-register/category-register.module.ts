import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CategoryRegisterPage } from './category-register';

@NgModule({
  declarations: [
    CategoryRegisterPage,
  ],
  imports: [
    IonicPageModule.forChild(CategoryRegisterPage),
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ]
})
export class CategoryRegisterPageModule {}
