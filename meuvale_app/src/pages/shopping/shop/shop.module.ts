import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ShopPage } from './shop';
import { IonicModule } from 'ionic-angular';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    ShopPage,
  ],
  imports: [
    IonicModule,
    ComponentsModule,
    IonicPageModule.forChild(ShopPage)
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ],
})
export class ShopPageModule {}
