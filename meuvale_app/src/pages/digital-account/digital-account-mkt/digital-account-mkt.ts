import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';

/**
 * Generated class for the DigitalAccountMktPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-digital-account-mkt',
  templateUrl: 'digital-account-mkt.html',
})
export class DigitalAccountMktPage {

  constructor(public navCtrl: NavController,
    public navParams: NavParams) {
  }

  ionViewDidLoad() {

  }

  showForm(){
    this.navCtrl.push("ContactPage", { tipo: "ContaDigital" });
  }

}
