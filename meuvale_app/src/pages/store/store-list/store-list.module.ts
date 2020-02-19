import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { StoreListPage } from './store-list';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    StoreListPage,
  ],
  imports: [
    IonicPageModule.forChild(StoreListPage),
    ComponentsModule
  ],
})
export class StoreListPageModule {}
