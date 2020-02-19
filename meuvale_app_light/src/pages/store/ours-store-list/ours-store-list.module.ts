import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OursStoreListPage } from './ours-store-list';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    OursStoreListPage,
  ],
  imports: [
    IonicPageModule.forChild(OursStoreListPage),
    ComponentsModule
  ],
})
export class OursStoreListPageModule {}
