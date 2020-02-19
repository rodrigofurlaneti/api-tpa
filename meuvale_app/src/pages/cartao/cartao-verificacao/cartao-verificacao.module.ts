
import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CartaoVerificacaoPage } from './cartao-verificacao';
import { BrMaskerModule } from 'brmasker-ionic-3';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CartaoVerificacaoPage,
  ],
  imports: [
    IonicPageModule.forChild(CartaoVerificacaoPage),
    ReactiveFormsModule,
    BrMaskerModule,
  ],
})
export class CartaoVerificacaoPageModule {}
