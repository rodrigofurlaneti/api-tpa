import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { MeuClubePage } from './meu-clube';
import { ComponentsModule } from '../../components/components.module';

@NgModule({
  declarations: [
    MeuClubePage,
  ],
  imports: [
    IonicPageModule.forChild(MeuClubePage),
    ComponentsModule
  ],
})
export class MeuClubePageModule {}
