import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { MainStorePage } from './main-store';

@NgModule({
  declarations: [
    MainStorePage,
  ],
  imports: [
    IonicPageModule.forChild(MainStorePage),
  ],
})
export class MainStorePageModule {}
