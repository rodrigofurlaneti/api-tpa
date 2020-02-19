import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OrderDetailPage } from './order-detail';
import { PipesModule } from '../../../pipes/pipes.module';
import { QRCodeModule } from 'angular2-qrcode';

@NgModule({
  declarations: [
    OrderDetailPage,
  ],
  imports: [
    PipesModule,
    QRCodeModule,
    IonicPageModule.forChild(OrderDetailPage),
  ],
})
export class OrderDetailPageModule {}
