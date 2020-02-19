import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { DepartamentRegisterPage } from './departament-register';
import { FirebaseUploadModule } from '../../../components/firebase-upload/firebase-upload.module';

@NgModule({
  declarations: [
    DepartamentRegisterPage,
  ],
  imports: [
    IonicPageModule.forChild(DepartamentRegisterPage),
    FirebaseUploadModule.forRoot({
      firebase: {
        apiKey: "AIzaSyCZF42Ey5-Ofk3JuMjWeeZ7DR6IPOj2jsE",
        authDomain: "meuvale-aed97.firebaseapp.com",
        projectId: "meuvale-aed97",
        storageBucket: "meuvale-aed97.appspot.com",
      }, image: {
        width: 151,
        height: 141,
        quality: 100
      }
    })
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ]
})
export class DepartamentRegisterPageModule {}
