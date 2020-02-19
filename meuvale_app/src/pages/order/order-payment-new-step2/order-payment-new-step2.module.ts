import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OrderPaymentNewStep2Page } from './order-payment-new-step2';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    OrderPaymentNewStep2Page,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(OrderPaymentNewStep2Page),
  ],
})
export class OrderPaymentNewStep2PageModule {}
