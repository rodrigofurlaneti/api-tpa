import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CartaoEditPage } from './cartao-edit';
import { BrMaskerModule } from 'brmasker-ionic-3';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    CartaoEditPage,
  ],
  imports: [
    IonicPageModule.forChild(CartaoEditPage),
    ReactiveFormsModule,
    BrMaskerModule,
  ],
})
export class CartaoEditPageModule {}
