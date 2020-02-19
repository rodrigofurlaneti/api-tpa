import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ProductPlanListPage } from './product-plan-list';
import { ComponentsModule } from '../../../components/components.module';

@NgModule({
  declarations: [
    ProductPlanListPage,
  ],
  imports: [
    IonicPageModule.forChild(ProductPlanListPage),
    ComponentsModule
  ],
})
export class ProductPlanListPageModule {}
