import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ClientAddressPage } from './client-address';
import { ReactiveFormsModule } from '@angular/forms';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    ClientAddressPage,
  ],
  imports: [
    IonicPageModule.forChild(ClientAddressPage),
    ReactiveFormsModule,
    BrMaskerModule,
  ],
})
export class ClientAddressPageModule {}
