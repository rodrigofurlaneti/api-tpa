import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { MainHomePage } from './main-home';
 import { ComponentsModule } from '../../components/components.module';

@NgModule({
  declarations: [
    MainHomePage,
  ],
  imports: [
     ComponentsModule,
    IonicPageModule.forChild(MainHomePage),
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ]
})
export class MainHomePageModule {}
