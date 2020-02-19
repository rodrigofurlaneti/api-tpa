import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OrderRatePage } from './order-rate';
import { PipesModule } from '../../../pipes/pipes.module';
import { QRCodeModule } from 'angular2-qrcode';

@NgModule({
  declarations: [
    OrderRatePage,
  ],
  imports: [
    PipesModule,
    QRCodeModule,
    IonicPageModule.forChild(OrderRatePage),
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ]
})
export class OrderRatePageModule {}
