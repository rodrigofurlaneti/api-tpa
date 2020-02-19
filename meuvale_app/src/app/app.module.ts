import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule, CUSTOM_ELEMENTS_SCHEMA, LOCALE_ID } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';
import { IonicStorageModule } from '@ionic/storage';
import { Facebook } from '@ionic-native/facebook';
import { MyApp } from './app.component';
import { SocialSharing } from '@ionic-native/social-sharing';
import { StatusBar } from '@ionic-native/status-bar';
import { SplashScreen } from '@ionic-native/splash-screen';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AuthProvider } from '../providers/auth/auth';
import { HttpProvider } from '../providers/http/http';
import { HttpClientModule } from '@angular/common/http';
import { FacebookProvider } from '../providers/facebook/facebook';
import { Geolocation } from '@ionic-native/geolocation';
import { UtilsProvider } from '../providers/utils/utils';
import { PeopleProvider } from '../providers/people/people';
import { PaymentProvider } from '../providers/payment/payment';
import { FornecedorProvider } from '../providers/fornecedor/fornecedor';
import { StoreProvider } from '../providers/store/store';
import { ShoppingProvider } from '../providers/shopping/shopping';
import { BrMaskerModule } from 'brmasker-ionic-3';
import { PushSetupProvider } from '../providers/push-setup/push-setup';
import { Push } from '@ionic-native/push';
import { LaunchNavigator } from '@ionic-native/launch-navigator';
import { BarcodeScanner } from '@ionic-native/barcode-scanner';
import { Calendar } from '@ionic-native/calendar';
import { Camera } from '@ionic-native/camera/';
import { GeolocationProvider } from '../providers/google/geolocation';
import { CepProvider } from '../providers/adress/cep';
import { HttpModule } from '@angular/http';
import { PageProvider } from '../providers/page/page';
import { File } from "@ionic-native/file/ngx";
import { FirebaseProvider } from '../providers/firebase/firebaseConfig';
import { Diagnostic } from '@ionic-native/diagnostic/ngx';
import { registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
import { PartnerProvider } from '../providers/partner/partner';
import { OrderProvider } from '../providers/order/order';
import { ProductProvider } from '../providers/product/product';
import { InAppBrowser } from '@ionic-native/in-app-browser';
import { GoogleMaps } from "@ionic-native/google-maps";
import { SmsProvider } from '../providers/sms/sms';
import { BankProvider } from '../providers/bank/bank';
import { ConvenioProvider } from '../providers/convenio/convenio';
import { ProductPlanProvider } from '../providers/product-plan/product-plan';

registerLocaleData(ptBr)

class CameraMock extends Camera {
  getPicture(options) {
    return new Promise((resolve, reject) => {
      resolve("BASE_64_ENCODED_DATA_GOES_HERE");
    })
  }
}

@NgModule({
  declarations: [
    MyApp
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    HttpModule,
    ReactiveFormsModule,
    FormsModule,
    BrMaskerModule,
    IonicStorageModule.forRoot({
      name: '__vcmarchedb',
         driverOrder: ['indexeddb', 'sqlite', 'websql']
    }),
    IonicModule.forRoot(MyApp,
    {
      backButtonText: 'Voltar',
      iconMode: 'ios',
      pageTransition: 'ios-transition',
      monthNames: ['janeiro', 'fevereiro', 'março', 'abril', 'maio', 'junho', 'julho', 'agosto', 'setembro', 'outubro', 'novembro', 'dezembro' ],
      monthShortNames: ['jan', 'fev', 'mar', 'abr', 'mai', 'jun', 'jul', 'ago', 'set', 'out', 'nov', 'dez' ],
      dayNames: ['domingo', 'segunda-feira', 'terça-feira', 'quarta-feira', 'quinta-feira', 'sexta-feira', 'sábado' ],
      dayShortNames: ['dom', 'seg', 'ter', 'qua', 'qui', 'sex', 'sab' ]
    })
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp
  ],
  providers: [
    StatusBar,
    SplashScreen,
    {provide: LOCALE_ID, useValue: 'pt' },
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    AuthProvider,
    HttpProvider,
    Facebook,
    GoogleMaps,
    Geolocation,
    FacebookProvider,
    UtilsProvider,
    SocialSharing,
    PeopleProvider,
    PaymentProvider,
    FornecedorProvider,
    StoreProvider,
    ShoppingProvider,
    Push,
    PushSetupProvider,
    LaunchNavigator,
    Calendar,
    { provide: Camera, useClass: CameraMock },
    BarcodeScanner,
    GeolocationProvider,
    CepProvider,
    PageProvider,
    File,
    FirebaseProvider,
    Diagnostic,
    PartnerProvider,
    OrderProvider,
    ProductProvider,
    InAppBrowser,
    SmsProvider,
    BankProvider,
    ConvenioProvider,
    ProductPlanProvider
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ]
})
export class AppModule {}
