import { NgModule } from '@angular/core';
import { StoresMapComponent } from './stores-map/stores-map';
import { HeaderRetireComponent } from './header-retire/header-retire';
import { IonicModule } from 'ionic-angular';
import { ProductStructComponent } from './product/product-struct';
// import { ProductImageComponent } from './product/product-images/product-image';

@NgModule({
	declarations: [
		StoresMapComponent,
		HeaderRetireComponent,
		ProductStructComponent,
		//ProductImageComponent
	],
	imports: [
		IonicModule
	],
	exports: [	
		StoresMapComponent,
		HeaderRetireComponent,
		ProductStructComponent,
		//ProductImageComponent
	]
})
export class ComponentsModule {}
