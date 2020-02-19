import { Component } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';

/**
 * Generated class for the OdontoMktPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-odonto-mkt',
  templateUrl: 'odonto-mkt.html',
})
export class OdontoMktPage {

  constructor(public navCtrl: NavController,
    public navParams: NavParams) {
  }

  ionViewDidLoad() {

  }

  showForm() {
    this.navCtrl.push("ContactPage", { tipo: "PlanoOdonto" });
  }

}
