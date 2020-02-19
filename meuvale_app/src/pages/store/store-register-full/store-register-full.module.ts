import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StoreRegisterFullPage } from './store-register-full';
import { BrMaskerModule } from 'brmasker-ionic-3';
import { FirebaseUploadModule } from '../../../components/firebase-upload/firebase-upload.module';

@NgModule({
  declarations: [
    StoreRegisterFullPage,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(StoreRegisterFullPage),
    FirebaseUploadModule.forRoot({
      firebase: {
        apiKey: "AIzaSyAOeWO7ZxHX6xG6H3O8WCcVVesufM-FBL8",
        authDomain: "okay-car-17a3c.firebaseapp.com",
        projectId: "okay-car-17a3c",
        storageBucket: "okay-car-17a3c.appspot.com",
      }, image: {
        width: 200,
        height: 200,
        quality: 100
      }
    })
  ],
})
export class StoreRegisterFullPageModule {}
