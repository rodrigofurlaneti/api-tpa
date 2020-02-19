import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ClientRegisterPage } from './client-register';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    ClientRegisterPage,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(ClientRegisterPage),
  ],
})
export class ClientRegisterPageModule {}
