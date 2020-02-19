import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { PartnerStoreListPage } from './partner-store-list';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    PartnerStoreListPage,
  ],
  imports: [
    IonicPageModule.forChild(PartnerStoreListPage),
    ComponentsModule
  ],
})
export class PartnerStoreListPageModule {}
