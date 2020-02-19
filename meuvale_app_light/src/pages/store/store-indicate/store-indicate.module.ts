import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StoreIndicatePage } from './store-indicate';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    StoreIndicatePage,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(StoreIndicatePage)
  ],
})
export class StoreIndicatePageModule {}
