import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { FirstLoginPage } from './first-login';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    FirstLoginPage,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(FirstLoginPage),
  ]
})
export class FirstLoginPageModule {}
