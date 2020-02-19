import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StoreSchedulePage } from './store-schedule';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    StoreSchedulePage,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(StoreSchedulePage),
  ],
})
export class StoreSchedulePageModule {}
