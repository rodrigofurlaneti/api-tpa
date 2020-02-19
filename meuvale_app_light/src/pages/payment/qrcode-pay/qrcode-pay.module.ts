import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { QrcodePayPage } from './qrcode-pay';

@NgModule({
  declarations: [
    QrcodePayPage,
  ],
  imports: [
    IonicPageModule.forChild(QrcodePayPage),
  ],
})
export class QrcodePayPageModule {}
