import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CartaoListPage } from './cartao-list';
import { PipesModule } from '../../../pipes/pipes.module';

@NgModule({
  declarations: [
    CartaoListPage,
  ],
  imports: [
    PipesModule,
    IonicPageModule.forChild(CartaoListPage),
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ]
})
export class CartaoListPageModule {}
