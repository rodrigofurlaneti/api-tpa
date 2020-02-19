import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { SocialSharing } from '@ionic-native/social-sharing';

/*
  Generated class for the StoreQrcodePage page.

  See http://ionicframework.com/docs/v2/components/#navigation for more info on
  Ionic pages and navigation.
*/

@IonicPage()
@Component({
  selector: 'page-store-qrcode',
  templateUrl: 'store-qrcode-view.html'
})
export class StoreQrcodeViewPage {

  QRCode: string = "0";
  loja: any;

  constructor(public navCtrl: NavController,
    private socialSharing: SocialSharing, 
    public navParams: NavParams) 
  {
    this.loja = this.navParams.get("loja");

    this.QRCode = 
    this.loja.Id.toString();
  }

  ionViewDidLoad() {
  }

  visualizar()
  {
    let params = {
      pet: this.loja
    };

    this.navCtrl.push("QrcodePayPage", params)
  }


  compartilhar(){
    this.socialSharing.share(
      this.QRCode,
      null,
      "http://meuvale.com.br/aplicativo");
  }

}
