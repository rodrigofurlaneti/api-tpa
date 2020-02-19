import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CartaoExtratoPage } from './cartao-extrato';
import { BrMaskerModule } from 'brmasker-ionic-3';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CartaoExtratoPage,
  ],
  imports: [
    IonicPageModule.forChild(CartaoExtratoPage),
    ReactiveFormsModule,
    BrMaskerModule,
  ],
})
export class CartaoExtratoPageModule {}
