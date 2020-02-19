import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ClientRegisterSimplePage } from './client-register-simple';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    ClientRegisterSimplePage,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(ClientRegisterSimplePage),
  ],
})
export class ClientRegisterSimplePageModule {}
