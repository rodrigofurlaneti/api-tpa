import { NgModule, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { MainAgentePage } from './main-agente';
 import { ComponentsModule } from '../../components/components.module';

@NgModule({
  declarations: [
    MainAgentePage,
  ],
  imports: [
     ComponentsModule,
    IonicPageModule.forChild(MainAgentePage),
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ]
})
export class MainAgentePageModule {}
