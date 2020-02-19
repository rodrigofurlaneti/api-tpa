import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ClientAddressEditPage } from './client-address-edit';
import { ReactiveFormsModule } from '@angular/forms';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    ClientAddressEditPage,
  ],
  imports: [
    IonicPageModule.forChild(ClientAddressEditPage),
    ReactiveFormsModule,
    BrMaskerModule,
  ],
})
export class ClientAddressEditPageModule {}
