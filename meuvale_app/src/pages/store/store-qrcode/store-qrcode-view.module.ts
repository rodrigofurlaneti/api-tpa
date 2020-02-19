import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StoreQrcodeViewPage } from './store-qrcode-view';
import { QRCodeModule } from 'angular2-qrcode';

@NgModule({
  declarations: [
    StoreQrcodeViewPage,
  ],
  imports: [
    QRCodeModule,
    IonicPageModule.forChild(StoreQrcodeViewPage),
  ],
})
export class StoreQrcodeViewPageModule {}
