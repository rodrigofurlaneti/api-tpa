import { Component, ViewChild } from '@angular/core';
import { IonicPage, NavController, Platform, AlertController } from 'ionic-angular';
import { StoreProvider } from '../../../providers/store/store';
import { Geolocation } from '@ionic-native/geolocation';
import { UtilsProvider } from '../../../providers/utils/utils';
import { StoresMapComponent } from '../../../components/stores-map/stores-map';
import { HttpProvider } from '../../../providers/http/http';
import { GeolocationProvider } from '../../../providers/google/geolocation';
import { Diagnostic } from '@ionic-native/diagnostic/ngx';

/**
 * Generated class for the OursStoreListPage page.
 *
 * See https://ionicframework.com/docs/components/#navigation for more info on
 * Ionic pages and navigation.
 */

@IonicPage()
@Component({
  selector: 'page-ours-store-list',
  templateUrl: 'ours-store-list.html',
})
export class OursStoreListPage {

  lojas: any;
  perfilLojista = HttpProvider.userAuth.PerfilId == "2";

  constructor(public navCtrl: NavController,
    private geo: Geolocation,
    private diagnostic: Diagnostic,
    private geoProvider: GeolocationProvider,
    private platform: Platform,
    private alertCtrl: AlertController,
    private utils: UtilsProvider,
    private storeProvider: StoreProvider) {
  }

  ionViewWillEnter() {
    this.loadLojas();    
  }

  loadLojas(){
    this.utils.showLoader("localizando lojas");
    this.geo.getCurrentPosition({
      timeout: 10000,
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
    ).catch((pe: PositionError) => {
      this.utils.hideLoader();      
      this.showRequestAlert(pe.code);
    });
  }

  tryPermissionAgain() {
    if (this.platform.is('ios'))
      this.diagnostic.switchToSettings();
    else 
      this.diagnostic.switchToLocationSettings();

    this.navCtrl.pop();
  }
  
  showRequestAlert(code: number) {
    let _message: string = "";
      if (code == 1) {
        _message = "É necessário um parâmetro para efetuar a busca, foneça o CEP ou ative a permissão de geolocalização!";
      } else {
        _message = "Erro ao recuperar coordenadas do gps!";
      }
      this.alertCtrl.create({
        title: "Erro",
        message: _message,
        enableBackdropDismiss: false,
        inputs: [{
          name: "cep",
          type: 'tel',
          max: 9,
          placeholder: "somente números"
        }],
        buttons: [{
          text: 'Efetuar busca por cep',
          handler: (form) => {
              this.findLocationByCep(form.cep);
          }
        },
        {
          text: "Ativar gps",
          handler: () => { if (code == 1) this.tryPermissionAgain(); else this.loadLojas(); }
        }]
      }).present();
  }

  findLocationByCep(cep: any) {
    this.utils.showLoader("buscando...");
    if (cep) {
      this.geoProvider.getGeolocationByAdress(cep)
        .subscribe(data => {
          if (data.status == "OK" && data.results && data.results.length) {
            this.getLojas(data.results[0].geometry.location.lat, data.results[0].geometry.location.lng);
            } else {
            this.utils.hideLoader();
            this.utils.showToast("Cep não válido");
            this.showRequestAlert(1);
          }
        });
        
    } else {
      this.utils.hideLoader();
      this.utils.showToast("opção inválida");
      this.showRequestAlert(1);
    }
  }

  getLojas(lat: number, lng: number) {
    this.storeProvider.lojasUsuario(HttpProvider.userAuth.PessoaId,lat, lng)
      .subscribe(
        data => {
          this.lojas = data;
          for (var i = 0; i < this.lojas.length; i++) {
            var obj = {
              nome: this.lojas[i].Descricao,
              lat: parseFloat(this.lojas[i].Endereco.Latitude),
              lng: parseFloat(this.lojas[i].Endereco.Longitude)
            };
          }
        }, error => {
          this.utils.hideLoader();
          this.utils.showToast(error);
        },
        () => this.utils.hideLoader()
      );
  }

  qrCode(loja) {
    StoreProvider.setCurrentStore(loja);
    this.navCtrl.push("StoreQrcodeViewPage", { "loja": loja });
  }

  entrar(loja){
    StoreProvider.setCurrentStore(loja);
    this.navCtrl.push("ShopPage", { "loja": loja, "visao": "lojista", "lojista" : true });
  }

  goToStoreRegister(loja) {
    this.navCtrl.push("StoreSchedulePage", { "loja": loja });
  }
}
