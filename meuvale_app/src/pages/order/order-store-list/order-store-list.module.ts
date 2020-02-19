import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OrderStoreListPage } from './order-store-list';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    OrderStoreListPage,
  ],
  imports: [
    ComponentsModule,
    IonicPageModule.forChild(OrderStoreListPage),
  ],
})
export class OrderStoreListPageModule {}
