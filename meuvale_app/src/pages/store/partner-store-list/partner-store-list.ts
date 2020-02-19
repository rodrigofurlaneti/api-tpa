import { Component, ViewChild } from '@angular/core';
import { IonicPage, NavController } from 'ionic-angular';
import { PartnerProvider } from '../../../providers/partner/partner';
import { Geolocation } from '@ionic-native/geolocation';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoresMapComponent } from '../../../components/stores-map/stores-map';

/**
 * Generated class for the PartnerStoreListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-partner-store-list',
  templateUrl: 'partner-store-list.html',
})
export class PartnerStoreListPage {

  @ViewChild("map") map: StoresMapComponent;
  lojas: any;

  constructor(public navCtrl: NavController,
    private geo: Geolocation,
    private utils: UtilsProvider,
    private partnerProvider: PartnerProvider) {
  }

  ionViewDidLoad() {
    this.utils.showLoader("localizando parceiros");
    this.geo.getCurrentPosition({
      timeout: 30000,
      enableHighAccuracy: true
    }).then(
      resp => {
        if (resp && resp.coords)
          this.getLojas(resp.coords.latitude, resp.coords.longitude);
        else {
          this.utils.hideLoader();
          this.utils.showToast("erro ao consultar o gps");
        }
      }
    );

  }

  getLojas(lat: number, lng: number) {
    this.partnerProvider.fornecedores()
      .subscribe(
        data => {
          this.lojas = data;
          let markers: any[] = [];
          for (var i = 0; i < this.lojas.length; i++) {
            var obj = {
              nome: this.lojas[i].Descricao,
              lat: parseFloat(this.lojas[i].Endereco.Latitude),
              lng: parseFloat(this.lojas[i].Endereco.Longitude)
            };
            markers.push(obj);
          }
          this.map.showMarks(markers);
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error);
        },
        () => this.utils.hideLoader()
      );
  }

  entrar(loja) {
    PartnerProvider.setCurrentPartner(loja);
    this.navCtrl.push("ShopPage", { "loja": loja, "visao": "lojista", "fornecedor": true });
  }

  goToStoreRegister(loja) {
    this.navCtrl.push("StoreRegisterPage", { "loja": loja, "fornecedor": true });
  }
}
