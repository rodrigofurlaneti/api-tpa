import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StoreListQrCodesPage } from './store-list-qrcode';
import { ComponentsModule } from '../../../components/components.module';
import { QRCodeModule } from 'angular2-qrcode';

@NgModule({
  declarations: [
    StoreListQrCodesPage,
  ],
  imports: [
    QRCodeModule,
    IonicPageModule.forChild(StoreListQrCodesPage),
    ComponentsModule
  ],
})
export class StoreListQrCodesPageModule {}
