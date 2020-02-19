import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ConvenioRegisterPage } from './convenio-register';
import { BrMaskerModule } from 'brmasker-ionic-3';
import { FirebaseUploadModule } from '../../../components/firebase-upload/firebase-upload.module';

@NgModule({
  declarations: [
    ConvenioRegisterPage,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(ConvenioRegisterPage),
    FirebaseUploadModule.forRoot({
      firebase: {
        apiKey: "AIzaSyCZF42Ey5-Ofk3JuMjWeeZ7DR6IPOj2jsE",
        authDomain: "meuvale-aed97.firebaseapp.com",
        projectId: "meuvale-aed97",
        storageBucket: "meuvale-aed97.appspot.com",
      }, image: {
        width: 200,
        height: 200,
        quality: 100
      }
    })
  ],
})
export class ConvenioRegisterPageModule {}
