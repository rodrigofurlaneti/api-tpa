import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OrderPaymentNewPage } from './order-payment-new';
import { PipesModule } from '../../../pipes/pipes.module';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    OrderPaymentNewPage,
  ],
  imports: [
    PipesModule,
    BrMaskerModule,
    IonicPageModule.forChild(OrderPaymentNewPage),
  ],
})
export class OrderPaymentNewPageModule {}
