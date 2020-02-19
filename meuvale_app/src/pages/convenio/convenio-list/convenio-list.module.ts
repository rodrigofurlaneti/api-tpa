import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ConvenioListPage } from './convenio-list';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    ConvenioListPage,
  ],
  imports: [
    IonicPageModule.forChild(ConvenioListPage),
    ComponentsModule
  ],
})
export class ConvenioListPageModule {}
