import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { MainAdminPage } from './main-admin';

@NgModule({
  declarations: [
    MainAdminPage,
  ],
  imports: [
    IonicPageModule.forChild(MainAdminPage),
  ],
})
export class MainAdminPageModule {}
