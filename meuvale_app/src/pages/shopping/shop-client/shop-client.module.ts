import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ShopClientPage } from './shop-client';
import { IonicModule } from 'ionic-angular';

@NgModule({
  declarations: [
    ShopClientPage,
  ],
  imports: [
    IonicModule,
    IonicPageModule.forChild(ShopClientPage)
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ],
})
export class ShopClientPageModule {}
