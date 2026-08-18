import { Routes } from '@angular/router';
import { AssetTypeFormComponent } from './asset-type-form/asset-type-form.component';
import { AssetTypeListComponent } from './asset-type-list/asset-type-list.component';

export const ASSET_TYPE_ROUTES: Routes = [  
    { path: '', component: AssetTypeListComponent },
    { path: 'new', component: AssetTypeFormComponent },
    { path: ':id/edit', component: AssetTypeFormComponent }
];