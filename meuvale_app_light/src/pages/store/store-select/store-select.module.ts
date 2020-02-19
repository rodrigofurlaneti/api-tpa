import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StoreSelectPage } from './store-select';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    StoreSelectPage,
  ],
  imports: [
    ComponentsModule,
    IonicPageModule.forChild(StoreSelectPage)
  ],
})
export class StoreSelectPageModule {}
